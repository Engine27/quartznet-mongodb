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
        public override JobDataMap Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
        {
            BsonReader bsonReader = (BsonReader)context.Reader;
            Type nominalType = args.NominalType;
            Type actualType = args.NominalType;

            if (nominalType != typeof(JobDataMap) || actualType != typeof(JobDataMap))
            {
                var message = string.Format(DESERIALIZE_ERROR, nominalType.FullName, this.GetType().Name);
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
                var message = string.Format(DESERIALIZE_ERROR_BSON, nominalType.FullName, bsonType);
                throw new BsonSerializationException(message);
            }
        }

        public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, JobDataMap value)
        {
            BsonWriter bsonWriter = (BsonWriter)context.Writer;
            JobDataMap item = (JobDataMap)value;
            bsonWriter.WriteStartDocument();

            foreach (string key in item.Keys)
            {
                bsonWriter.WriteName(key);
                BsonSerializer.Serialize(bsonWriter, item[key]);
            }

            bsonWriter.WriteEndDocument();
        }

        private static string DESERIALIZE_ERROR = "Can't deserialize a {0} with {1}.";
        private static string DESERIALIZE_ERROR_BSON = "Can't deserialize a {0} from BsonType {1}.";
    }
}
