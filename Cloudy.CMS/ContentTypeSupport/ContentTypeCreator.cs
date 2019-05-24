﻿using Poetry.ComponentSupport;
using Cloudy.CMS.ContentSupport;
using Cloudy.CMS.ContentTypeSupport.PropertyMappingSupport;
using Cloudy.CMS.Core;
using Cloudy.CMS.Core.ContentSupport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Cloudy.CMS.ContentTypeSupport
{
    public class ContentTypeCreator : IContentTypeCreator
    {
        IPropertyDefinitionCreator PropertyDefinitionCreator { get; }
        ICoreInterfaceCreator CoreInterfaceCreator { get; }
        IPropertyMappingProvider PropertyMappingRepository { get; }
        IComponentProvider ComponentProvider { get; }

        public ContentTypeCreator(IPropertyDefinitionCreator propertyDefinitionCreator, ICoreInterfaceCreator coreInterfaceCreator, IPropertyMappingProvider propertyMappingRepository, IComponentProvider componentProvider)
        {
            PropertyDefinitionCreator = propertyDefinitionCreator;
            CoreInterfaceCreator = coreInterfaceCreator;
            PropertyMappingRepository = propertyMappingRepository;
            ComponentProvider = componentProvider;
        }

        public IEnumerable<ContentTypeDescriptor> Create()
        {
            var types = ComponentProvider
                    .GetAll()
                    .SelectMany(a => a.Assembly.Types)
                    .Where(a => typeof(IContent).IsAssignableFrom(a));

            var allCoreInterfaces = new Dictionary<string, CoreInterfaceDescriptor>();

            foreach (var type in types)
            {
                var contentTypeAttribute = type.GetTypeInfo().GetCustomAttribute<ContentTypeAttribute>();

                if (contentTypeAttribute == null)
                {
                    continue;
                }

                var propertyDefinitions = new List<PropertyDefinitionDescriptor>();

                foreach (var property in type.GetProperties())
                {
                    var mapping = PropertyMappingRepository.Get(property);

                    if (mapping.PropertyMappingType == PropertyMappingType.Ignored)
                    {
                        continue;
                    }

                    if (mapping.PropertyMappingType == PropertyMappingType.CoreInterface)
                    {
                        continue;
                    }

                    if (mapping.PropertyMappingType == PropertyMappingType.Incomplete)
                    {
                        continue;
                    }

                    propertyDefinitions.Add(PropertyDefinitionCreator.Create(property));
                }

                var coreInterfaces = new List<CoreInterfaceDescriptor>();

                foreach(var interfaceType in type.GetInterfaces())
                {
                    if(interfaceType.GetCustomAttribute<CoreInterfaceAttribute>() == null)
                    {
                        continue;
                    }

                    if (!allCoreInterfaces.ContainsKey(interfaceType.FullName))
                    {
                        allCoreInterfaces[interfaceType.FullName] = CoreInterfaceCreator.Create(interfaceType);
                    }

                    coreInterfaces.Add(allCoreInterfaces[interfaceType.FullName]);
                }

                yield return new ContentTypeDescriptor(contentTypeAttribute.Id, type, propertyDefinitions, coreInterfaces);
            }
        }
    }
}