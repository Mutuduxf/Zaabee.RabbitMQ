﻿using Zaabee.RabbitMQ.ISerialize;
using Zaabee.Xml;

namespace Zaabee.RabbitMQ.Xml
{
    public class Serializer : ISerializer
    {
        public byte[] Serialize<T>(T o) =>
            o.ToBytes();

        public T Deserialize<T>(byte[] bytes) =>
            bytes is null || bytes.Length == 0 ? default(T) : bytes.FromBytes<T>();

        public string BytesToText(byte[] bytes) =>
            bytes != null ? XmlHelper.DefaultEncoding.GetString(bytes) : null;

        public T FromText<T>(string text) =>
            string.IsNullOrWhiteSpace(text) ? default(T) : text.FromXml<T>();
    }
}