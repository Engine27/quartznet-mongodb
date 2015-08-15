using System;

using MongoDB.Bson;
using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Options;

namespace Quartz.Impl.MongoDB
{
    public class TimeOfDaySerializer : IBsonSerializer
    {
        //public object Deserialize(BsonReader bsonReader, Type nominalType, IBsonSerializationOptions options)
        //{
        //    return Deserialize(bsonReader);
        //}

        //public object Deserialize(BsonReader bsonReader, Type nominalType, Type actualType, IBsonSerializationOptions options)
        //{
        //    return Deserialize(bsonReader);
        //}

        //static object Deserialize(BsonReader bsonReader)
        //{
        //    var currentBsonType = bsonReader.GetCurrentBsonType();
        //    switch (currentBsonType)
        //    {
        //        case BsonType.Null:
        //            return null;
        //        case BsonType.Document:
        //            bsonReader.ReadStartDocument();
        //            var hour = bsonReader.ReadInt32("Hour");
        //            var minute = bsonReader.ReadInt32("Minute");
        //            var second = bsonReader.ReadInt32("Second");
        //            bsonReader.ReadEndDocument();
        //            return new TimeOfDay(hour, minute, second);
        //        default:
        //            throw new NotSupportedException(
        //                string.Format("Bson type : {0} is not supported for TimeOfDay type property", currentBsonType));
        //    }
        //}

        //public IBsonSerializationOptions GetDefaultSerializationOptions()
        //{
        //    return new DocumentSerializationOptions();
        //}

        //public void Serialize(BsonWriter bsonWriter, Type nominalType, object value, IBsonSerializationOptions options)
        //{
        //    var timeOfDay = (TimeOfDay)value;
        //    bsonWriter.WriteStartDocument();
        //    bsonWriter.WriteInt32("Hour", timeOfDay.Hour);
        //    bsonWriter.WriteInt32("Minute", timeOfDay.Minute);
        //    bsonWriter.WriteInt32("Second", timeOfDay.Second);
        //    bsonWriter.WriteEndDocument();
        //}

        public object Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
        {
            context.Reader.ReadStartDocument();
            var hour = context.Reader.ReadInt32("Hour");
            var minute = context.Reader.ReadInt32("Minute");
            var second = context.Reader.ReadInt32("Second");
            context.Reader.ReadEndDocument();
            return new TimeOfDay(hour, minute, second);
        }

        public void Serialize(BsonSerializationContext context, BsonSerializationArgs args, object value)
        {
            TimeOfDay timeOfDAy = (TimeOfDay)value;
            context.Writer.WriteStartDocument();
            context.Writer.WriteInt32("Hour", timeOfDAy.Hour);
            context.Writer.WriteInt32("Minute", timeOfDAy.Minute);
            context.Writer.WriteInt32("Second", timeOfDAy.Second);
            context.Writer.WriteEndDocument();
        }

        public Type ValueType
        {
            get 
            { 
                return typeof(TimeOfDay); 
            }
        }
    }
}