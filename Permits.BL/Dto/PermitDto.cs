using Permits.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Permits.BL.Dto
{
    public class PermitDto : BaseDto
    {
        public int PermitId { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeeLastName { get; set; }
        public virtual PermitTypeDto PermitType { get; set; }
        public DateTimeOffset PermitDate { get; set; }
    }
}
