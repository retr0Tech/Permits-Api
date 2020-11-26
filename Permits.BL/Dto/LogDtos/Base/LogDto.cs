
using Microsoft.EntityFrameworkCore;
using Permits.Core.Models;
using Permits.Model.Entities.LogEntities.Base;

namespace Permits.BL.Dto.LogDtos.Base
{
    public class LogDto : BaseDto, ILog
    {
        public int EntityId { get; set; }
        public EntityState EntityState { get; set; }
        public string EntityName { get; set; }
        public string PropertyName { get; set; }
        public string OriginalValue { get; set; }
        public string CurrentValue { get; set; }
        public string MappedOriginalValue { get; set; }
        public string MappedCurrentValue { get; set; }
        public int? ParentEntityId { get; set; }
        public int? RelatedEntityId { get; set; }
    }
}
