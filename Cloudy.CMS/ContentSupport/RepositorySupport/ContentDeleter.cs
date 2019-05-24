﻿using MongoDB.Driver;
using Cloudy.CMS.DocumentSupport;
using Cloudy.CMS.ContentSupport;
using Cloudy.CMS.ContentSupport.Serialization;
using Cloudy.CMS.ContentTypeSupport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cloudy.CMS.Core.ContentSupport.RepositorySupport
{
    public class ContentDeleter : IContentDeleter
    {
        IDocumentRepository DocumentRepository { get; }
        string Container { get; } = "content";

        public ContentDeleter(IDocumentRepository documentRepository)
        {
            DocumentRepository = documentRepository;
        }

        public void Delete(IContent content)
        {
            DeleteAsync(content).WaitAndUnwrapException();
        }

        public async Task DeleteAsync(IContent content)
        {
            if (content.Id == null)
            {
                throw new InvalidOperationException($"This content cannot be deleted as it doesn't seem to exist (Id is null)");
            }

            await DocumentRepository.Documents.FindOneAndDeleteAsync(Builders<Document>.Filter.Eq(d => d.Id, content.Id));
        }
    }
}