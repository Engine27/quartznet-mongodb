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
            var hour = context.Reader.ReadInt32("Hour");
            var minute = context.Reader.ReadInt32("Minute");
            var second = context.Reader.ReadInt32("Second");
            context.Reader.ReadEndDocument();
            return new TimeOfDay(hour, minute, second);
        }

        public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, TimeOfDay value)
        {
            TimeOfDay timeOfDAy = (TimeOfDay)value;
            context.Writer.WriteStartDocument();
            context.Writer.WriteInt32("Hour", timeOfDAy.Hour);
            context.Writer.WriteInt32("Minute", timeOfDAy.Minute);
            context.Writer.WriteInt32("Second", timeOfDAy.Second);
            context.Writer.WriteEndDocument();
        }
    }
}