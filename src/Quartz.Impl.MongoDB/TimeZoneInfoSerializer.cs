using System;

using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

namespace Quartz.Impl.MongoDB
{
    public class TimeZoneInfoSerializer : SerializerBase<TimeZoneInfo>
    {
        public override TimeZoneInfo Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
        {
            var timeZoneId = context.Reader.ReadString();
            return TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
        }

        public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, TimeZoneInfo value)
        {
            var timeZoneInfo = (TimeZoneInfo)value;
            context.Writer.WriteString(timeZoneInfo.Id);
        }
    }
}