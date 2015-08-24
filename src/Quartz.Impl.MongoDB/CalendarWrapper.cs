using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.IO;
using Quartz.Impl.Calendar;

namespace Quartz.Impl.MongoDB
{
    public class CalendarWrapper : IBsonSerializer
    {
        
        public string Name { get; set; }
        public ICalendar Calendar { get; set; }

        public bool GetDocumentId(out object id, out Type idNominalType, out IIdGenerator idGenerator)
        {
            id = this.Name;
            idNominalType = typeof(string);
            idGenerator = null;
            
            return true;
        }

        public object Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
        {
            BsonReader bsonReader = (BsonReader)context.Reader;

            CalendarWrapper item = new CalendarWrapper();

            bsonReader.ReadStartDocument();
            item.Name = bsonReader.ReadString(ID);
            var binaryData = bsonReader.ReadBinaryData(CONTENT_STREAM);
            item.Calendar = (ICalendar)new BinaryFormatter().Deserialize(new MemoryStream(binaryData.Bytes));
            bsonReader.ReadEndDocument();

            return item;
        }

        public void Serialize(BsonSerializationContext context, BsonSerializationArgs args, object value)
        {
            BsonWriter bsonWriter = (BsonWriter)context.Writer;

            bsonWriter.WriteStartDocument();
            bsonWriter.WriteString(ID, this.Name);
            MemoryStream stream = new MemoryStream();
            new BinaryFormatter().Serialize(stream, this.Calendar);
            bsonWriter.WriteBinaryData(CONTENT_STREAM, new BsonBinaryData(stream.ToArray(), BsonBinarySubType.Binary));
            bsonWriter.WriteEndDocument();
        }

        public Type ValueType
        {
            get 
            { 
                return typeof(ICalendar); 
            }
        }

        private static string ID = "_id";
        private static string CONTENT_STREAM = "ContentStream";
    }
}
