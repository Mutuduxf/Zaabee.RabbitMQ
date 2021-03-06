﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Zaabee.RabbitMQ.Abstractions;
using Zaabee.Serializer.Abstractions;

namespace Zaabee.RabbitMQ
{
    public partial class ZaabeeRabbitMqClient : IZaabeeRabbitMqClient
    {
        private readonly IConnection _publishConn;
        private readonly IConnection _subscribeConn;
        private readonly ITextSerializer _serializer;

        private readonly ConcurrentDictionary<Type, string> _queueNameDic = new ();

        public ZaabeeRabbitMqClient(MqConfig config, ITextSerializer serializer)
        {
            if (config is null) throw new ArgumentNullException(nameof(config));
            if (serializer is null) throw new ArgumentNullException(nameof(serializer));
            if (config.Hosts.Count is 0) throw new ArgumentNullException(nameof(config.Hosts));

            var factory = new ConnectionFactory
            {
                RequestedHeartbeat = config.HeartBeat,
                AutomaticRecoveryEnabled = config.AutomaticRecoveryEnabled,
                NetworkRecoveryInterval = config.NetworkRecoveryInterval,
                UserName = config.UserName,
                Password = config.Password,
                VirtualHost = string.IsNullOrWhiteSpace(config.VirtualHost) ? "/" : config.VirtualHost,
            };

            _publishConn = config.Hosts.Any() ? factory.CreateConnection(config.Hosts) : factory.CreateConnection();
            _subscribeConn = config.Hosts.Any() ? factory.CreateConnection(config.Hosts) : factory.CreateConnection();
            _serializer = serializer;
        }

        public ZaabeeRabbitMqClient(IConnectionFactory connectionFactory, ITextSerializer serializer)
        {
            if (connectionFactory is null) throw new ArgumentNullException(nameof(connectionFactory));
            _serializer = serializer ?? throw new ArgumentNullException(nameof(serializer));

            _publishConn = connectionFactory.CreateConnection();
            _subscribeConn = connectionFactory.CreateConnection();
        }

        public ZaabeeRabbitMqClient(IConnectionFactory connectionFactory, string clientProvidedName,
            ITextSerializer serializer)
        {
            if (connectionFactory is null) throw new ArgumentNullException(nameof(connectionFactory));
            _serializer = serializer ?? throw new ArgumentNullException(nameof(serializer));

            _publishConn = connectionFactory.CreateConnection(clientProvidedName);
            _subscribeConn = connectionFactory.CreateConnection(clientProvidedName);
        }

        public ZaabeeRabbitMqClient(IConnectionFactory connectionFactory, IList<string> hosts, ITextSerializer serializer)
        {
            if (connectionFactory is null) throw new ArgumentNullException(nameof(connectionFactory));
            _serializer = serializer ?? throw new ArgumentNullException(nameof(serializer));

            _publishConn = connectionFactory.CreateConnection(hosts);
            _subscribeConn = connectionFactory.CreateConnection(hosts);
        }

        public ZaabeeRabbitMqClient(IConnectionFactory connectionFactory, IList<string> hosts,
            string clientProvidedName, ITextSerializer serializer)
        {
            if (connectionFactory is null) throw new ArgumentNullException(nameof(connectionFactory));
            _serializer = serializer ?? throw new ArgumentNullException(nameof(serializer));

            _publishConn = connectionFactory.CreateConnection(hosts, clientProvidedName);
            _subscribeConn = connectionFactory.CreateConnection(hosts, clientProvidedName);
        }

        public ZaabeeRabbitMqClient(IConnectionFactory connectionFactory, IList<AmqpTcpEndpoint> endpoints,
            ITextSerializer serializer)
        {
            if (connectionFactory is null) throw new ArgumentNullException(nameof(connectionFactory));
            _serializer = serializer ?? throw new ArgumentNullException(nameof(serializer));

            _publishConn = connectionFactory.CreateConnection(endpoints);
            _subscribeConn = connectionFactory.CreateConnection(endpoints);
        }

        public ZaabeeRabbitMqClient(IConnectionFactory connectionFactory, IList<AmqpTcpEndpoint> endpoints,
            string clientProvidedName, ITextSerializer serializer)
        {
            if (connectionFactory is null) throw new ArgumentNullException(nameof(connectionFactory));
            _serializer = serializer ?? throw new ArgumentNullException(nameof(serializer));

            _publishConn = connectionFactory.CreateConnection(endpoints, clientProvidedName);
            _subscribeConn = connectionFactory.CreateConnection(endpoints, clientProvidedName);
        }

        public void RepublishDeadLetterEvent<T>(string deadLetterQueueName, ushort prefetchCount = 1)
        {
            var queueParam = new QueueParam {Queue = deadLetterQueueName};
            var channel = GetReceiverChannel(null, queueParam, prefetchCount);

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                try
                {
                    var body = ea.Body;
                    var msg = _serializer.DeserializeFromBytes<DeadLetterMsg>(body.ToArray());

                    var republishExchangeParam =
                        new ExchangeParam {Exchange = $"republish-{deadLetterQueueName}", Durable = true};
                    var republishQueueParam =
                        new QueueParam {Queue = FromDeadLetterName(deadLetterQueueName), Durable = true};
                    using (var republishChannel = GetPublisherChannel(republishExchangeParam, republishQueueParam))
                    {
                        var properties = republishChannel.CreateBasicProperties();
                        properties.Persistent = true;
                        var routingKey = republishExchangeParam.Exchange;

                        var deadLetter = _serializer.DeserializeFromString<T>(msg.BodyString);

                        republishChannel.BasicPublish(republishExchangeParam.Exchange, routingKey, properties,
                            _serializer.SerializeToBytes(deadLetter));
                    }

                    channel.BasicAck(ea.DeliveryTag, false);
                }
                catch
                {
                    channel.BasicNack(ea.DeliveryTag, false, true);
                }
            };
            channel.BasicConsume(queue: deadLetterQueueName, autoAck: false, consumer: consumer);
        }

        private string GetTypeName(Type type) =>
            _queueNameDic.GetOrAdd(type,
                _ => type.GetCustomAttributes(typeof(MessageVersionAttribute), false).FirstOrDefault()
                    is MessageVersionAttribute msgVerAttr
                    ? $"{type}[{msgVerAttr.Version}]"
                    : type.ToString());

        private string GetQueueName<T>(Action<T> handle)
        {
            var messageName = GetTypeName(typeof(T));
            return $"{handle.Method.ReflectedType?.FullName}.{handle.Method.Name}[{messageName}]";
        }

        private string GetQueueName<T>(Func<Action<T>> resolve)
        {
            var handle = resolve();
            return GetQueueName(handle);
        }

        private static string GetDeadLetterName(string name) =>
            $"dead-letter-{name}";

        private static string FromDeadLetterName(string deadLetterName) =>
            deadLetterName.Replace("dead-letter-", "");

        public void Dispose()
        {
            foreach (var keyValuePair in _subscriberChannelDic)
                keyValuePair.Value.Dispose();
            _publishConn.Dispose();
            _subscribeConn.Dispose();
        }
    }
}