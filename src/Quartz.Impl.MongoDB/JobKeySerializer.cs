﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Bson.Serialization;
using MongoDB.Bson;
using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization.Serializers;

namespace Quartz.Impl.MongoDB
{
    public class JobKeySerializer : SerializerBase<JobKey>
    {
        public override JobKey Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
        {
            Type nominalType = args.NominalType;
            Type actualType = args.NominalType;
            BsonReader bsonReader = (BsonReader)context.Reader;

            if (nominalType != typeof(JobKey) || actualType != typeof(JobKey))
            {
                var message = string.Format(DESERIALIZE_ERROR, nominalType.FullName, this.GetType().Name);
                throw new BsonSerializationException(message);
            }

            var bsonType = bsonReader.CurrentBsonType;
            if (bsonType == BsonType.Document)
            {
                JobKey item;

                bsonReader.ReadStartDocument();
                item = new JobKey(
                    bsonReader.ReadString(NAME),
                    bsonReader.ReadString(GROUP));
                bsonReader.ReadEndDocument();

                return item;
            }
            else if (bsonType == BsonType.Null)
            {
                bsonReader.ReadNull();
                return null;
            }
            else
            {
                var message = string.Format(DESERIALIZE_ERROR, nominalType.FullName, bsonType);
                throw new BsonSerializationException(message);
            }
        }

        public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, JobKey value)
        {
            BsonWriter  bsonWriter = (BsonWriter) context.Writer;
            JobKey item = (JobKey)value;

            bsonWriter.WriteStartDocument();
            bsonWriter.WriteString(NAME, item.Name);
            bsonWriter.WriteString(GROUP, item.Group);
            bsonWriter.WriteEndDocument();
        }

        private static string NAME = "Name";
        private static string GROUP = "Group";
        private static string DESERIALIZE_ERROR = "Can't deserialize a {0} from BsonType {1}.";
    }
}