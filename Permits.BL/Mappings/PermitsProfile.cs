using AutoMapper;
using Permits.BL.Dto;
using Permits.BL.Extensions;
using Permits.Model.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Permits.BL.Mappings
{
    public class PermitsProfile : Profile
    {
        public PermitsProfile()
        {
            this._CreateMap_WithConventions_FromAssemblies<Permit, PermitDto>();
            // Custom Mappings

            CreateMap<Permit, PermitDto>()
                .ForMember(x => x.PermitType, y => y.MapFrom(z => z.PermitType.Description));

        }
    }
}
