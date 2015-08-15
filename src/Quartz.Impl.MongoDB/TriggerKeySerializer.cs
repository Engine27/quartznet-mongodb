﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Bson.Serialization;
using MongoDB.Bson;
using MongoDB.Bson.IO;

namespace Quartz.Impl.MongoDB
{
    public class TriggerKeySerializer : IBsonSerializer
    {
        public object Deserialize(global::MongoDB.Bson.IO.BsonReader bsonReader, Type nominalType, Type actualType)
        {
            if (nominalType != typeof(TriggerKey) || actualType != typeof(TriggerKey))
            {
                var message = string.Format("Can't deserialize a {0} from {1}.", nominalType.FullName, this.GetType().Name);
                throw new BsonSerializationException(message);
            }

            var bsonType = bsonReader.CurrentBsonType;
            if (bsonType == BsonType.Document)
            {
                TriggerKey item;

                string name;
                bsonReader.ReadName("Name");
                name = bsonReader.ReadString();
                string group;
                bsonReader.ReadName("Group");
                group = bsonReader.ReadString();

                bsonReader.ReadStartDocument();
                item = new TriggerKey(name, group);
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
            TriggerKey item = (TriggerKey)value;

            bsonWriter.WriteStartDocument();
            bsonWriter.WriteString("Name", item.Name);
            bsonWriter.WriteString("Group", item.Group);
            bsonWriter.WriteEndDocument();
        }

        public object Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
        {
            return Deserialize((BsonReader)context.Reader, args.NominalType);
        }

        public void Serialize(BsonSerializationContext context, BsonSerializationArgs args, object value)
        {
            Serialize((BsonWriter)context.Writer, args.NominalType, value);
        }

        public Type ValueType
        {
            get { return typeof(TriggerKey); }
        }
    }
}