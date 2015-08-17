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
        public JobKey Deserialize(global::MongoDB.Bson.IO.BsonReader bsonReader, Type nominalType, Type actualType)
        {
            if (nominalType != typeof(JobKey) || actualType != typeof(JobKey))
            {
                var message = string.Format("Can't deserialize a {0} from {1}.", nominalType.FullName, this.GetType().Name);
                throw new BsonSerializationException(message);
            }

            var bsonType = bsonReader.CurrentBsonType;
            if (bsonType == BsonType.Document)
            {
                JobKey item;
                
                bsonReader.ReadStartDocument();
                item = new JobKey(
                    bsonReader.ReadString("Name"),
                    bsonReader.ReadString("Group"));
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
                var message = string.Format("Can't deserialize a {0} from BsonType {1}.", nominalType.FullName, bsonType);
                throw new BsonSerializationException(message);
            }
        }

        public object Deserialize(global::MongoDB.Bson.IO.BsonReader bsonReader, Type nominalType)
        {
            return this.Deserialize(bsonReader, nominalType, nominalType);
        }

        public void Serialize(global::MongoDB.Bson.IO.BsonWriter bsonWriter, Type nominalType, object value)
        {
            JobKey item = (JobKey)value;

            bsonWriter.WriteStartDocument();
            bsonWriter.WriteString("Name", item.Name);
            bsonWriter.WriteString("Group", item.Group);
            bsonWriter.WriteEndDocument();
        }

        public override JobKey Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
        {
            return Deserialize((BsonReader)context.Reader, args.NominalType, args.NominalType);
        }

        public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, JobKey value)
        {
            BsonWriter  bsonWriter = (BsonWriter) context.Writer;
            Serialize(bsonWriter, args.NominalType, value);
        }
    }
}