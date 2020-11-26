using Permits.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Permits.BL.Dto
{
    public class PermitTypeDto: BaseDto
    {
        public int PermitTypeId { get; set; }
        public string Description { get; set; }
    }
}
