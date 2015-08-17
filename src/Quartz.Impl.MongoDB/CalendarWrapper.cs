﻿using System;
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

        public object Deserialize(global::MongoDB.Bson.IO.BsonReader bsonReader, Type nominalType)
        {
            CalendarWrapper item = new CalendarWrapper();
            
            bsonReader.ReadStartDocument();
            item.Name = bsonReader.ReadString("_id");
            var binaryData = bsonReader.ReadBinaryData("ContentStream");
            item.Calendar = (ICalendar)new BinaryFormatter().Deserialize(new MemoryStream(binaryData.Bytes));
            bsonReader.ReadEndDocument();
            
            return item;
        }

        public bool GetDocumentId(out object id, out Type idNominalType, out IIdGenerator idGenerator)
        {
            id = this.Name;
            idNominalType = typeof(string);
            idGenerator = null;

            return true;
        }

        public void Serialize(global::MongoDB.Bson.IO.BsonWriter bsonWriter, Type nominalType)
        {
            bsonWriter.WriteStartDocument();
            bsonWriter.WriteString("_id", this.Name);
            MemoryStream stream = new MemoryStream();
            new BinaryFormatter().Serialize(stream, this.Calendar);
            bsonWriter.WriteBinaryData("ContentStream", new BsonBinaryData(stream.ToArray(), BsonBinarySubType.Binary));
            bsonWriter.WriteEndDocument();
        }

        public object Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
        {
            return Deserialize((BsonReader)context.Reader, args.NominalType);
        }

        public void Serialize(BsonSerializationContext context, BsonSerializationArgs args, object value)
        {
            Serialize((BsonWriter)context.Writer, args.NominalType);
        }

        public Type ValueType
        {
            get { return typeof(ICalendar); }
        }
    }
}
