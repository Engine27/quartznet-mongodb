using System;

using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization;

namespace Quartz.Impl.MongoDB
{
    public class TimeZoneInfoSerializer : IBsonSerializer
    {
        //public object Deserialize(BsonReader bsonReader, Type nominalType, IBsonSerializationOptions options)
        //{
        //    return Deserialize(bsonReader);
        //}

        //public object Deserialize(BsonReader bsonReader, Type nominalType, Type actualType, IBsonSerializationOptions options)
        //{
        //    return Deserialize(bsonReader);
        //}

        //private static object Deserialize(BsonReader bsonReader)
        //{
        //    var timeZoneId = bsonReader.ReadString();
        //    return TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
        //}

        //public IBsonSerializationOptions GetDefaultSerializationOptions()
        //{
        //    return null;
        //}

        public void Serialize(BsonWriter bsonWriter, Type nominalType, object value)
        {
            var timeZoneInfo = (TimeZoneInfo)value;
            bsonWriter.WriteString(timeZoneInfo.Id);
        }

        public object Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
        {
            var timeZoneId = context.Reader.ReadString();
            return TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
        }

        public void Serialize(BsonSerializationContext context, BsonSerializationArgs args, object value)
        {
            var timeZoneInfo = (TimeZoneInfo)value;
            context.Writer.WriteString(timeZoneInfo.Id);
        }

        public Type ValueType
        {
            get { throw new NotImplementedException(); }
        }
    }
}