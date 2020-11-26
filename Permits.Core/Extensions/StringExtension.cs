using System.Text.RegularExpressions;

namespace Permits.Core.Extensions
{
    public static class StringExtension
    {
        public static string ReplaceIgnoreCase(this string text, string oldValue, string replaceWith)
        {
            var regex = new Regex(oldValue, RegexOptions.IgnoreCase);
            return regex.Replace(text, replaceWith);
        }
    }
}
