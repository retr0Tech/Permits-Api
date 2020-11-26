﻿using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Permits.BL.Dto;
using Permits.Model.Contexts.Permits;
using Permits.Model.Entities;
using Permits.Model.UnitOfWorks;

namespace Permits.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PermitsController : BaseController<Permit, PermitDto>
    {
        public PermitsController(IMapper mapper, IUnitOfWork<PermitsDbContext> uow, IValidatorFactory validationFactory) : base(mapper, uow, validationFactory)
        {
        }
    }
}

