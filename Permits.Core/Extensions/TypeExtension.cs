using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Permits.Core.Extensions
{
    public static class TypeExtension
    {
        public static string GetDescription(this Type type)
        {
            DescriptionAttribute[] attributes = (DescriptionAttribute[])Attribute.GetCustomAttributes(type, typeof(DescriptionAttribute));
            if (attributes != null && attributes.Any())
            {
                return attributes.FirstOrDefault()?.Description;
            }
            return null;
        }
    }
}
