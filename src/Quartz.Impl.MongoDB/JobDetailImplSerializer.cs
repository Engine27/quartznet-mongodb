using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Bson.Serialization;
using MongoDB.Bson;
using MongoDB.Bson.IO;
using System.Reflection;
using MongoDB.Bson.Serialization.Serializers;

namespace Quartz.Impl.MongoDB
{
    public class JobDetailImplSerializer : SerializerBase<IJobDetail>
    {
        public override IJobDetail Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
        {
            BsonReader bsonReader = (BsonReader)context.Reader;
            Type nominalType = args.NominalType;

            var bsonType = bsonReader.GetCurrentBsonType();
            if (bsonType == BsonType.Document)
            {
                bsonReader.ReadStartDocument();

                BsonSerializer.Deserialize(bsonReader, typeof(JobKey));
                bsonReader.ReadString(TYPE);

                Assembly assembly = Assembly.Load(bsonReader.ReadString(ASSEMBLY));
                Type jobType = assembly.GetType(bsonReader.ReadString(CLASS));
                string name = bsonReader.ReadString(NAME);
                string group = bsonReader.ReadString(GROUP);
                bool requestRecovery = bsonReader.ReadBoolean(REQUEST_RECOVERY);
                bool durable = bsonReader.ReadBoolean(DURABLE);

                IJobDetail jobDetail = new JobDetailImpl(name, group, jobType, durable, requestRecovery);

                bsonReader.ReadBsonType();
                JobDataMap map = (JobDataMap)BsonSerializer.Deserialize(bsonReader, typeof(JobDataMap));
                /*bsonReader.ReadBsonType();
                string description = (string)BsonSerializer.Deserialize(bsonReader, typeof(string));*/

                jobDetail = jobDetail.GetJobBuilder()
                    .UsingJobData(map)
                    /*.WithDescription(description)*/
                    .Build();

                bsonReader.ReadEndDocument();

                return jobDetail;
            }
            else if (bsonType == BsonType.Null)
            {
                bsonReader.ReadNull();
                return null;
            }
            else
            {
                var message = string.Format(DESERIALIZE_ERROR_MESSAGE, nominalType.FullName, bsonType);
                throw new BsonSerializationException(message);
            }
        }

        public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, IJobDetail value)
        {
            BsonWriter bsonWriter = (BsonWriter)context.Writer;

            JobDetailImpl item = (JobDetailImpl)value;
            bsonWriter.WriteStartDocument();

            bsonWriter.WriteName(ID);
            BsonSerializer.Serialize<JobKey>(bsonWriter, item.Key);

            bsonWriter.WriteString(TYPE, INSTANCE);
            bsonWriter.WriteString(ASSEMBLY, item.JobType.Assembly.FullName);
            bsonWriter.WriteString(CLASS, item.JobType.FullName);

            bsonWriter.WriteString(NAME, item.Name);
            bsonWriter.WriteString(GROUP, item.Group);
            bsonWriter.WriteBoolean(REQUEST_RECOVERY, item.RequestsRecovery);
            bsonWriter.WriteBoolean(DURABLE, item.Durable);

            bsonWriter.WriteName(JOB_DATA_MAP);
            BsonSerializer.Serialize(bsonWriter, item.JobDataMap);

            /*bsonWriter.WriteName("Description");
            BsonSerializer.Serialize<string>(bsonWriter, item.Description);*/

            bsonWriter.WriteEndDocument();
        }

        private static string DESERIALIZE_ERROR_MESSAGE = "Can't deserialize a {0} from BsonType {1}.";

        private static string ID = "_id";
        private static string TYPE = "_t";
        private static string INSTANCE = "JobDetailImpl";
        private static string ASSEMBLY = "_assembly";
        private static string CLASS = "_class";
        private static string NAME = "Name";
        private static string GROUP = "Group";
        private static string REQUEST_RECOVERY = "RequestRecovery";
        private static string DURABLE = "Durable";
        private static string JOB_DATA_MAP = "JobDataMap";
    }
}
