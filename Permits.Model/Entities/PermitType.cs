using Permits.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Permits.Model.Entities
{
    public class PermitType : BaseEntity
    {
        public int PermitTypeId { get; set; }
        public string Description { get; set; }
    }
}
