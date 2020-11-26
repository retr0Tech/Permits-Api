using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Permits.BL.Extensions
{
    public static class StringExtensions
    {
        public static string GetPropertyName(this string name)
        {
            if (name == null)
                return null;

            if (name.Contains("."))
                return name.Split('.').LastOrDefault().GetPropertyName();

            return name;
        }

        public static string LowerCaseAndReplaceAcuteAccents(this string str)
        {
            return str
                .ToLower()
                .Replace('á', 'a')
                .Replace('é', 'e')
                .Replace('í', 'i')
                .Replace('ó', 'o')
                .Replace('ú', 'u');
        }
    }
}
