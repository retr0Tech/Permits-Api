using System;
using System.Collections.Generic;
using System.Text;

namespace Permits.Core.Models
{
    public class BaseDto: IBaseDto
    {
        public virtual int? Id { get; set; }
        public virtual string CreatedBy { get; set; }
        public virtual string UpdatedBy { get; set; }
        public virtual bool Deleted { get; set; }
    }
}
