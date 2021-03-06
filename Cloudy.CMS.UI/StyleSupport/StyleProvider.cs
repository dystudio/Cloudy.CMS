﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Poetry.ComponentSupport;

namespace Poetry.UI.StyleSupport
{
    public class StyleProvider : IStyleProvider
    {
        IEnumerable<StyleDescriptor> Styles { get; }

        public StyleProvider(IStyleCreator styleCreator)
        {
            Styles = styleCreator.Create().ToList().AsReadOnly();
        }

        public IEnumerable<StyleDescriptor> GetAll()
        {
            return Styles;
        }
    }
}
