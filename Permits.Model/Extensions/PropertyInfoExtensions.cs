using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Permits.Model.Extensions
{
    public static class PropertyInfoExtensions
    {
        public static string GetDescription(this PropertyInfo propertyInfo)
        {
            DescriptionAttribute[] attributes = propertyInfo.GetCustomAttributes(typeof(DescriptionAttribute), false) as DescriptionAttribute[];
            if (attributes != null && attributes.Any())
            {
                return attributes.FirstOrDefault()?.Description;
            }
            return null;
        }
    }
}
