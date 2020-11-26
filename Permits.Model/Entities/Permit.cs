using Permits.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Permits.Model.Entities
{
    public class Permit : BaseEntity
    {
        public int PermitId { get; set; } 
        public string EmployeeName { get; set; }
        public string EmployeeLastName { get; set; }
        public virtual PermitType PermitType { get; set; }
        public DateTimeOffset PermitDate { get; set; }
    }
}
