using System;
using System.Collections.Generic;
using System.Text;

namespace Permits.Core.Models
{
    public interface IBaseEntity
    {
        int Id { get; set; }
        DateTimeOffset? CreatedDate { get; set; }
        DateTimeOffset? UpdatedDate { get; set; }
        string CreatedBy { get; set; }
        string UpdatedBy { get; set; }
        bool Deleted { get; set; }
        
    }
}
