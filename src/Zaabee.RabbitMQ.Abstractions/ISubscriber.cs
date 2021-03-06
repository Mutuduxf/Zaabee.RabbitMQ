using System;

namespace Zaabee.RabbitMQ.Abstractions
{
    public interface ISubscriber
    {
        #region Event

        /// <summary>
        /// The subscriber cluster will receive the Event by the default queue.
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="prefetchCount"></param>
        /// <typeparam name="T"></typeparam>
        void ReceiveEvent<T>(Action<T> handle, ushort prefetchCount = 10);

        /// <summary>
        /// The subscriber cluster will receive the Event by its own queue.
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="prefetchCount"></param>
        /// <typeparam name="T"></typeparam>
        void SubscribeEvent<T>(Action<T> handle, ushort prefetchCount = 10);

        /// <summary>
        /// The subscriber cluster will receive the Event by the specified queue.
        /// </summary>
        /// <param name="queue"></param>
        /// <param name="handle"></param>
        /// <param name="prefetchCount"></param>
        /// <typeparam name="T"></typeparam>
        void SubscribeEvent<T>(string queue, Action<T> handle, ushort prefetchCount = 10);

        /// <summary>
        /// The subscriber cluster will receive the Event by the specified exchange and queue.
        /// </summary>
        /// <param name="exchange"></param>
        /// <param name="queue"></param>
        /// <param name="handle"></param>
        /// <param name="prefetchCount"></param>
        /// <typeparam name="T"></typeparam>
        void SubscribeEvent<T>(string exchange, string queue, Action<T> handle, ushort prefetchCount = 10);

        /// <summary>
        /// The subscriber cluster will receive the Event by the default queue.
        /// </summary>
        /// <param name="resolve"></param>
        /// <param name="prefetchCount"></param>
        /// <typeparam name="T"></typeparam>
        void ReceiveEvent<T>(Func<Action<T>> resolve, ushort prefetchCount = 10);

        /// <summary>
        /// The subscriber cluster will receive the Event by its own queue.
        /// </summary>
        /// <param name="resolve"></param>
        /// <param name="prefetchCount"></param>
        /// <typeparam name="T"></typeparam>
        void SubscribeEvent<T>(Func<Action<T>> resolve, ushort prefetchCount = 10);

        /// <summary>
        /// The subscriber cluster will receive the Event by the specified queue.
        /// </summary>
        /// <param name="queue"></param>
        /// <param name="resolve"></param>
        /// <param name="prefetchCount"></param>
        /// <typeparam name="T"></typeparam>
        void SubscribeEvent<T>(string queue, Func<Action<T>> resolve, ushort prefetchCount = 10);

        /// <summary>
        /// The subscriber cluster will receive the Event by the specified exchange and queue.
        /// </summary>
        /// <param name="exchange"></param>
        /// <param name="queue"></param>
        /// <param name="resolve"></param>
        /// <param name="prefetchCount"></param>
        /// <typeparam name="T"></typeparam>
        void SubscribeEvent<T>(string exchange, string queue, Func<Action<T>> resolve, ushort prefetchCount = 10);

        #endregion

        #region Message

        /// <summary>
        /// The subscriber cluster will receive the Message by the default queue.
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="prefetchCount"></param>
        /// <typeparam name="T"></typeparam>
        void ReceiveMessage<T>(Action<T> handle, ushort prefetchCount = 10);

        /// <summary>
        /// The subscriber cluster will receive the Message by its own queue.
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="prefetchCount"></param>
        /// <typeparam name="T"></typeparam>
        void SubscribeMessage<T>(Action<T> handle, ushort prefetchCount = 10);

        /// <summary>
        /// The subscriber cluster will receive the Message by the specified queue.
        /// </summary>
        /// <param name="queue"></param>
        /// <param name="handle"></param>
        /// <param name="prefetchCount"></param>
        /// <typeparam name="T"></typeparam>
        void SubscribeMessage<T>(string queue, Action<T> handle, ushort prefetchCount = 10);

        /// <summary>
        /// The subscriber cluster will receive the Message by the specified exchange and queue.
        /// </summary>
        /// <param name="exchange"></param>
        /// <param name="queue"></param>
        /// <param name="handle"></param>
        /// <param name="prefetchCount"></param>
        /// <typeparam name="T"></typeparam>
        void SubscribeMessage<T>(string exchange, string queue, Action<T> handle, ushort prefetchCount = 10);

        /// <summary>
        /// The subscriber node will receive the Message by its own queue.
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="prefetchCount"></param>
        /// <typeparam name="T"></typeparam>
        void ListenMessage<T>(Action<T> handle, ushort prefetchCount = 10);

        /// <summary>
        /// The subscriber cluster will receive the Message by the default queue.
        /// </summary>
        /// <param name="resolve"></param>
        /// <param name="prefetchCount"></param>
        /// <typeparam name="T"></typeparam>
        void ReceiveMessage<T>(Func<Action<T>> resolve, ushort prefetchCount = 10);

        /// <summary>
        /// The subscriber cluster will receive the Message by its own queue.
        /// </summary>
        /// <param name="resolve"></param>
        /// <param name="prefetchCount"></param>
        /// <typeparam name="T"></typeparam>
        void SubscribeMessage<T>(Func<Action<T>> resolve, ushort prefetchCount = 10);

        /// <summary>
        /// The subscriber cluster will receive the Message by the specified queue.
        /// </summary>
        /// <param name="queue"></param>
        /// <param name="resolve"></param>
        /// <param name="prefetchCount"></param>
        /// <typeparam name="T"></typeparam>
        void SubscribeMessage<T>(string queue, Func<Action<T>> resolve, ushort prefetchCount = 10);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="exchange"></param>
        /// <param name="queue"></param>
        /// <param name="resolve"></param>
        /// <param name="prefetchCount"></param>
        /// <typeparam name="T"></typeparam>
        void SubscribeMessage<T>(string exchange, string queue, Func<Action<T>> resolve, ushort prefetchCount = 10);

        /// <summary>
        /// The subscriber node will receive the Message by its own queue.
        /// </summary>
        /// <param name="resolve"></param>
        /// <param name="prefetchCount"></param>
        /// <typeparam name="T"></typeparam>
        void ListenMessage<T>(Func<Action<T>> resolve, ushort prefetchCount = 10);

        #endregion

        #region Command

        /// <summary>
        /// The subscriber cluster will receive the Command by the default queue.
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="prefetchCount"></param>
        /// <typeparam name="T"></typeparam>
        void ReceiveCommand<T>(Action<T> handle, ushort prefetchCount = 10);

        /// <summary>
        /// The subscriber cluster will receive the Command by the default queue.
        /// </summary>
        /// <param name="resolve"></param>
        /// <param name="prefetchCount"></param>
        /// <typeparam name="T"></typeparam>
        void ReceiveCommand<T>(Func<Action<T>> resolve, ushort prefetchCount = 10);

        /// <summary>
        /// The subscriber cluster will receive the Command by the default queue.
        /// </summary>
        /// <param name="queue"></param>
        /// <param name="handle"></param>
        /// <param name="prefetchCount"></param>
        /// <typeparam name="T"></typeparam>
        void ReceiveCommand<T>(string queue, Action<T> handle, ushort prefetchCount = 10);

        /// <summary>
        /// The subscriber cluster will receive the Command by the default queue.
        /// </summary>
        /// <param name="queue"></param>
        /// <param name="resolve"></param>
        /// <param name="prefetchCount"></param>
        /// <typeparam name="T"></typeparam>
        void ReceiveCommand<T>(string queue, Func<Action<T>> resolve, ushort prefetchCount = 10);

        #endregion

        void RepublishDeadLetterEvent<T>(string deadLetterQueueName, ushort prefetchCount = 10);
    }
}