﻿using System;
using System.Collections.Generic;
using System.Text;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;

namespace Cloudy.CMS.DocumentSupport
{
    public class DocumentRepository : IDocumentRepository
    {
        public IMongoCollection<Document> Documents { get; }

        static DocumentRepository() {
            BsonClassMap.RegisterClassMap<Document>(cm => { cm.AutoMap(); new ImmutableTypeClassMapConvention().Apply(cm); });
            BsonClassMap.RegisterClassMap<DocumentFacet>(cm => { cm.AutoMap(); new ImmutableTypeClassMapConvention().Apply(cm); });
            BsonClassMap.RegisterClassMap<DocumentInterface>(cm => { cm.AutoMap(); new ImmutableTypeClassMapConvention().Apply(cm); });
        }

        public DocumentRepository()
        {
            var client = new MongoClient();

            var db = client.GetDatabase("content");

            Documents = db.GetCollection<Document>("content");
        }
    }
}