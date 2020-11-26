using System;
using System.Collections.Generic;
using System.Text;

namespace Permits.Core.Models
{
    public interface IBaseDto
    {
        int? Id { get; set; }
        string CreatedBy { get; set; }
        string UpdatedBy { get; set; }
        bool Deleted { get; set; }
    }

}
