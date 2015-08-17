﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using MongoDB.Bson.Serialization;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Options;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Bson.IO;
using System.Globalization;

namespace Quartz.Impl.MongoDB
{
    /// <summary>
    /// Represents a serializer for DateTimeOffsets.
    /// </summary>
    public class DateTimeOffsetSerializer : SerializerBase<DateTimeOffset>
    {
        private static DateTimeOffsetSerializer __instance = new DateTimeOffsetSerializer();

        /// <summary>
        /// Initializes a new instance of the DateTimeSerializer class.
        /// </summary>
        public DateTimeOffsetSerializer()            
        {
        }

        /// <summary>
        /// Gets an instance of the DateTimeSerializer class.
        /// </summary>
        public static DateTimeOffsetSerializer Instance
        {
            get { return __instance; }
        }

        public override DateTimeOffset Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
        {
            BsonReader bsonReader = (BsonReader)context.Reader;

            var bsonType = bsonReader.GetCurrentBsonType();
            DateTimeOffset value;
            switch (bsonType)
            {
                case BsonType.DateTime:
                    // use an intermediate BsonDateTime so MinValue and MaxValue are handled correctly
                    value = (new BsonDateTime(bsonReader.ReadDateTime())).ToUniversalTime();
                    break;
                case BsonType.Document:
                    bsonReader.ReadStartDocument();
                    bsonReader.ReadDateTime("DateTimeUTC"); // ignore value (use Ticks instead)
                    value = new DateTime(bsonReader.ReadInt64("Ticks"), DateTimeKind.Utc);
                    bsonReader.ReadEndDocument();
                    break;
                case BsonType.Int64:
                    value = new DateTime(bsonReader.ReadInt64(), DateTimeKind.Utc);
                    break;
                case BsonType.String:
                    var formats = new string[] { "yyyy-MM-ddK", "yyyy-MM-ddTHH:mm:ssK", "yyyy-MM-ddTHH:mm:ss.FFFFFFFK" };
                    value = DateTime.ParseExact(bsonReader.ReadString(), formats, null, DateTimeStyles.AdjustToUniversal | DateTimeStyles.AssumeUniversal);
                    break;
                default:
                    var message = string.Format("Cannot deserialize DateTimeOffset from BsonType {0}.", bsonType);
                    throw new FormatException(message);
            }

            return value;
        }

        public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, DateTimeOffset value)
        {
            BsonWriter bsonWriter = (BsonWriter)context.Writer;

            var dateTime = (DateTimeOffset)value;
            DateTime utcDateTime;
            utcDateTime = BsonUtils.ToUniversalTime(dateTime.UtcDateTime);
            var millisecondsSinceEpoch = BsonUtils.ToMillisecondsSinceEpoch(utcDateTime);
            bsonWriter.WriteDateTime(millisecondsSinceEpoch);
        }
    }
}