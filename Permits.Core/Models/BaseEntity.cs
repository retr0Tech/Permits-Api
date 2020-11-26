using System;
using System.Collections.Generic;
using System.Text;

namespace Permits.Core.Models
{
    public class BaseEntity : IBaseEntity
    {
        public virtual int Id { get; set; }
        public virtual DateTimeOffset? CreatedDate { get; set; }
        public virtual DateTimeOffset? UpdatedDate { get; set; }
        public virtual string CreatedBy { get; set; }
        public virtual string UpdatedBy { get; set; }
        public virtual bool Deleted { get; set; }
    }
}
