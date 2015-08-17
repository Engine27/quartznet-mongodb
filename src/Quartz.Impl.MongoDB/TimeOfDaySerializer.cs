using System;

using MongoDB.Bson;
using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Options;
using MongoDB.Bson.Serialization.Serializers;

namespace Quartz.Impl.MongoDB
{
    public class TimeOfDaySerializer : SerializerBase<TimeOfDay>
    {
        public override TimeOfDay Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
        {
            context.Reader.ReadStartDocument();
            var hour = context.Reader.ReadInt32(HOUR);
            var minute = context.Reader.ReadInt32(MINUTE);
            var second = context.Reader.ReadInt32(SECOND);
            context.Reader.ReadEndDocument();
            return new TimeOfDay(hour, minute, second);
        }

        public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, TimeOfDay value)
        {
            TimeOfDay timeOfDAy = (TimeOfDay)value;
            context.Writer.WriteStartDocument();
            context.Writer.WriteInt32(HOUR, timeOfDAy.Hour);
            context.Writer.WriteInt32(MINUTE, timeOfDAy.Minute);
            context.Writer.WriteInt32(SECOND, timeOfDAy.Second);
            context.Writer.WriteEndDocument();
        }

        private static string HOUR = "Hour";
        private static string MINUTE = "Minute";
        private static string SECOND = "Second";
    }
}