using Permits.Core.Extensions;
using System.Text.RegularExpressions;

namespace Permits.Core.Services.Parser
{
    public class UrlFilterToDynamicLinqParser : IUrlFilterToDynamicLinqParser
    {
        public string Parse(string filter)
        {
            filter = filter ?? "";
            filter = Regex.Replace(filter, "(?<=\\(\\\").*?(?=\\\"\\))", m => m.Value.Replace("\"", ""));
            return filter
                //equal
                .ReplaceIgnoreCase(" eq ", " = ")
                //not equal
                .ReplaceIgnoreCase(" ne ", " != ")
                //greater than
                .ReplaceIgnoreCase(" gt ", " > ")
                //greater than or equal
                .ReplaceIgnoreCase(" gte ", " >= ")
                //less than
                .ReplaceIgnoreCase(" lt ", " < ")
                //less than or equal
                .ReplaceIgnoreCase(" lte ", " <= ");
        }
    }
}
