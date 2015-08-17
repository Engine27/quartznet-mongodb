using System;
using System.Collections.Generic;

using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization;

namespace Quartz.Impl.MongoDB
{
    public class SetSerializer<T> : IBsonSerializer
    {
        private IBsonSerializer enumerableSerializer;

        public SetSerializer()
        {
            enumerableSerializer = BsonSerializer.LookupSerializer(typeof(IEnumerable<T>));
        }

        public object Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
        {
            var enumerable = (IEnumerable<T>)enumerableSerializer.Deserialize(context, args);
            return new Quartz.Collection.HashSet<T>(enumerable);
        }

        public void Serialize(BsonSerializationContext context, BsonSerializationArgs args, object value)
        {
            enumerableSerializer.Serialize(context, args, value);
        }

        public Type ValueType
        {
            get { return typeof(T); }
        }
    }
}