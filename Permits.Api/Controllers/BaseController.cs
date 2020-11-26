using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Permits.Core.Models;
using Permits.Core.Options;
using Permits.Model.Contexts.Permits;
using Permits.Model.Repositories.Generic;
using Permits.Model.UnitOfWorks;

namespace Permits.Api.Controllers
{
    public interface IBaseController
    {
        Type TypeDto { get; set; }
        IMapper _mapper { get; set; }
        IValidatorFactory _validationFactory { get; set; }
        UnprocessableEntityObjectResult UnprocessableEntity(object error);
    }

    [Produces("application/json")]
    [Route("api/[controller]")]
    public class BaseController<TEntity, TEntityDto> : ControllerBase, IBaseController
        where TEntity : class, IBaseEntity
        where TEntityDto : class, IBaseDto
    {
        public IMapper _mapper { get; set; }
        public IValidatorFactory _validationFactory { get; set; }

        protected readonly IUnitOfWork<PermitsDbContext> _uow;
        protected readonly IBaseRepository<TEntity, PermitsDbContext> _repository;

        public Type TypeDto { get; set; }

        public BaseController(IMapper mapper, IUnitOfWork<PermitsDbContext> uow, IValidatorFactory validationFactory)
        {
            _mapper = mapper;
            _uow = uow;
            _validationFactory = validationFactory;
            _repository = _uow.GetRepository<TEntity>();
            TypeDto = typeof(List<TEntityDto>);
        }

        /// <summary>
        /// Get all by query options.
        /// </summary>
        /// <returns>A list of records.</returns>
        [HttpGet]
        public virtual IActionResult GetAll([FromQuery] ApiQueryOption queryOption = null)
        {
            IQueryable<TEntity> list = _repository.ApplyApiQueryOption(queryOption, _repository.GetAll());
            IList<TEntityDto> dtoList = _mapper.Map<IList<TEntityDto>>(list);
            return Ok(dtoList);
        }

        /// <summary>
        /// Get a specific record by id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>A specific record.</returns>
        [HttpGet("{id}")]
        public virtual async Task<IActionResult> GetById(int id)
        {
            TEntity entity = _repository.GetByIdAsNoTracking(id);

            if (entity is null)
                return NotFound();

            TEntityDto entityDto = _mapper.Map<TEntityDto>(entity);

            return Ok(await Task.FromResult(entityDto));
        }

        /// <summary>
        /// Get a list of records by ids.
        /// </summary>
        /// <returns>A list of records by ids.</returns>
        [HttpGet("GetAllByIds")]
        public virtual async Task<IActionResult> GetAllByIds([FromQuery] IList<int> ids)
        {
            IQueryable<TEntity> list = _repository.GetAllByIds(ids);
            IList<TEntityDto> dtoList = _mapper.Map<IList<TEntityDto>>(list);
            return Ok(await Task.FromResult(dtoList));
        }

        /// <summary>
        /// Creates a record.
        /// </summary>
        /// <returns>A newly created record.</returns>
        [HttpPost]
        public virtual async Task<IActionResult> Post([FromBody] TEntityDto entityDto)
        {
            TEntity entity = _mapper.Map<TEntity>(entityDto);
            _repository.Add(entity);
            await _uow.Commit();

            entityDto = _mapper.Map<TEntityDto>(entity);

            return Ok(entityDto);
        }

        /// <summary>
        /// Updates a record.
        /// </summary>
        /// <returns>A newly updated record.</returns>
        [HttpPut("{id}")]
        public virtual async Task<IActionResult> Put([FromRoute] int id, [FromBody] TEntityDto entityDto)
        {
            if (entityDto.Id != id)
                return BadRequest();

            TEntity entity = _repository.GetByIdAsNoTracking(id);
            if (entity is null)
                return NotFound();

            entity = _mapper.Map<TEntity>(entityDto);
            _repository.Update(entity);
            await _uow.Commit();

            entityDto = _mapper.Map<TEntityDto>(entity);

            return Ok(entityDto);
        }

        /// <summary>
        /// Deletes a specific record by id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>A deleted record.</returns>
        [HttpDelete("{id}")]
        public virtual async Task<IActionResult> Delete(int id)
        {
            TEntity entity = _repository.GetById(id);

            if (entity is null)
                return NotFound();

            _repository.Delete(entity);
            await _uow.Commit();

            TEntityDto entityDto = _mapper.Map<TEntityDto>(entity);

            return Ok(entityDto);
        }

        /// <summary>
        /// Creates multiple records.
        /// </summary>
        /// <returns>A list of the newly created records.</returns>
        [HttpPost("CreateMultiple")]
        public virtual async Task<IActionResult> CreateMultiple([FromBody] IList<TEntityDto> entityDtoList)
        {
            IList<TEntity> entityList = _mapper.Map<IList<TEntity>>(entityDtoList);

            _repository.Add(entityList);
            await _uow.Commit();

            entityDtoList = _mapper.Map<IList<TEntityDto>>(entityList);

            return Ok(entityDtoList);
        }
    }
}
