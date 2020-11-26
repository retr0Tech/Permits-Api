using System;
using System.Collections.Generic;
using System.Text;

namespace Permits.Core.Models
{
    public class SerilogSettings
    {
        public string FilePath { get; set; }
        public string Table { get; set; }
        public string ConnectionStrings { get; set; }
        public string FullFilePath => $"{AppContext.BaseDirectory}{FilePath}";
    }
}
