﻿using Cloudy.CMS.ContentTypeSupport;
using System;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Cloudy.CMS.ContentSupport.Serialization;
using Cloudy.CMS.DocumentSupport;
using MongoDB.Driver;
using Cloudy.CMS.ContentSupport.Serialization;

namespace Cloudy.CMS.ContentSupport.RepositorySupport
{
    public class ContentGetter : IContentGetter
    {
        IDocumentRepository DocumentRepository { get; }
        IContentTypeProvider ContentTypeRepository { get; }
        string Container { get; } = "content";
        IContentDeserializer ContentDeserializer { get; }

        public ContentGetter(IDocumentRepository documentRepository, IContentTypeProvider contentTypeRepository, IContentDeserializer contentDeserializer)
        {
            DocumentRepository = documentRepository;
            ContentTypeRepository = contentTypeRepository;
            ContentDeserializer = contentDeserializer;
        }


        public T Get<T>(string id, string language) where T : class
        {
            return GetAsync<T>(id, language).WaitAndUnwrapException();
        }

        public async Task<T> GetAsync<T>(string id, string language) where T : class
        {
            if (id == null)
            {
                throw new ArgumentNullException(nameof(id));
            }
            if (language == null)
            {
                throw new ArgumentNullException(nameof(language));
            }

            var document = (await DocumentRepository.Documents.FindAsync(Builders<Document>.Filter.Eq(d => d.Id, id))).FirstOrDefault();

            if (document == null)
            {
                return null;
            }

            if (document.LanguageFacets[language].Interfaces["Properties"] == null)
            {
                return null;
            }

            var contentType = ContentTypeRepository.Get(document.GlobalFacet.Interfaces["IContent"].Properties["ContentTypeId"] as string);

            return (T)ContentDeserializer.Deserialize(document, contentType, language);
        }
    }
}