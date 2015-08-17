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
    public class TriggerKeySerializer : SerializerBase<TriggerKey>
    {
        public TriggerKey Deserialize(global::MongoDB.Bson.IO.BsonReader bsonReader, Type nominalType, Type actualType)
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

                bsonReader.ReadStartDocument();
                item = new TriggerKey(
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

        public TriggerKey Deserialize(global::MongoDB.Bson.IO.BsonReader bsonReader, Type nominalType)
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

        public override TriggerKey Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
        {
            return Deserialize((BsonReader)context.Reader, args.NominalType);
        }

        public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, TriggerKey value)
        {
            Serialize((BsonWriter)context.Writer, args.NominalType, value);
        }
    }
}