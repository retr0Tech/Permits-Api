using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Permits.Model.Entities.LogEntities.Base
{
    public interface ILog
    {
        int EntityId { get; set; }
        EntityState EntityState { get; set; }
        string EntityName { get; set; }
        string PropertyName { get; set; }
        string OriginalValue { get; set; }
        string CurrentValue { get; set; }
        string MappedOriginalValue { get; set; }
        string MappedCurrentValue { get; set; }
        int? ParentEntityId { get; set; }
        int? RelatedEntityId { get; set; }
    }
}
