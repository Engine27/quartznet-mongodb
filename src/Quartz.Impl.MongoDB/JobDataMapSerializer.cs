using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Bson.Serialization;
using MongoDB.Bson;
using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization.Serializers;

namespace Quartz.Impl.MongoDB
{
    public class JobDataMapSerializer : SerializerBase<JobDataMap>
    {
        public JobDataMap Deserialize(global::MongoDB.Bson.IO.BsonReader bsonReader, Type nominalType, Type actualType)
        {
            if (nominalType != typeof(JobDataMap) || actualType != typeof(JobDataMap))
            {
                var message = string.Format("Can't deserialize a {0} with {1}.", nominalType.FullName, this.GetType().Name);
                throw new BsonSerializationException(message);
            }

            var bsonType = bsonReader.CurrentBsonType;
            if (bsonType == BsonType.Document)
            {
                JobDataMap item = new JobDataMap();
                bsonReader.ReadStartDocument();

                while (bsonReader.ReadBsonType() != BsonType.EndOfDocument)
                {
                    string key = bsonReader.ReadName();
                    object value = BsonSerializer.Deserialize<object>(bsonReader);
                    item.Add(key, value);
                }

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

        public JobDataMap Deserialize(global::MongoDB.Bson.IO.BsonReader bsonReader, Type nominalType)
        {
            return this.Deserialize(bsonReader, nominalType, nominalType);
        }

        public void Serialize(global::MongoDB.Bson.IO.BsonWriter bsonWriter, Type nominalType, JobDataMap value)
        {
            JobDataMap item = (JobDataMap)value;
            bsonWriter.WriteStartDocument();

            foreach (string key in item.Keys)
            {
                bsonWriter.WriteName(key);
                BsonSerializer.Serialize(bsonWriter, item[key]);
            }
            
            bsonWriter.WriteEndDocument();
        }


        public override JobDataMap Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
        {
            return Deserialize((BsonReader)context.Reader, args.NominalType, args.NominalType);
        }

        public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, JobDataMap value)
        {
            Serialize((BsonWriter)context.Writer, args.NominalType, value);
        }
    }
}
