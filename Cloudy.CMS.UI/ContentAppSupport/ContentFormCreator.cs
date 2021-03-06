﻿using Poetry.ComponentSupport;
using Poetry.DependencyInjectionSupport;
using Poetry.UI.FormSupport;
using Poetry.UI.FormSupport.FieldSupport;
using Cloudy.CMS.ContentTypeSupport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cloudy.CMS.UI.ContentAppSupport
{
    public class ContentFormCreator : IFormCreator
    {
        IContentTypeProvider ContentTypeRepository { get; }

        public ContentFormCreator(IContentTypeProvider contentTypeRepository)
        {
            ContentTypeRepository = contentTypeRepository;
        }

        public IEnumerable<FormDescriptor> CreateAll()
        {
            var result = new List<FormDescriptor>();

            foreach (var type in ContentTypeRepository.GetAll())
            {
                result.Add(new FormDescriptor($"Cloudy.CMS.Content[type={type.Id}]", type.Type));
            }

            return result.AsReadOnly();
        }
    }
}
