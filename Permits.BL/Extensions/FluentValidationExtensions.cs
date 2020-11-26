using Admin.Core.IoC;
using FluentValidation;
using FluentValidation.Validators;
using Permits.Core.Models;
using Permits.Model.Contexts.Permits;
using Permits.Model.UnitOfWorks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Permits.BL.Extensions
{
    public static class FluentValidationExtensions
    {
        public static IRuleBuilderOptions<TItem, TProperty> IsUnique<TItem, TProperty>(this IRuleBuilder<TItem, TProperty> ruleBuilder) where TItem : class, IBaseEntity
        {
            var uow = Dependency.ServiceProvider.GetService(typeof(IUnitOfWork<PermitsDbContext>)) as IUnitOfWork<PermitsDbContext>;
            return ruleBuilder.SetValidator(new UniqueValidator<TItem>(uow));
        }
    }
    public class UniqueValidator<T> : PropertyValidator where T : class, IBaseEntity
    {
        public IUnitOfWork<PermitsDbContext> _uow { get; set; }

        public UniqueValidator(IUnitOfWork<PermitsDbContext> uow)
          : base("{PropertyName} debe ser único")
        {
            _uow = uow;
        }

        protected override bool IsValid(PropertyValidatorContext context)
        {
            T editedItem = context.InstanceToValidate as T;
            string newValue = context.PropertyValue?.ToString();
            string propertyName = context.PropertyName.GetPropertyName();
            PropertyInfo property = typeof(T).GetTypeInfo().GetDeclaredProperty(propertyName);

            IQueryable<string> _items = _uow.GetRepository<T>()
                .GetAllAsNoTracking(x => x.Id != editedItem.Id)
                .Where(x => property.GetValue(x) != null)
                .Select(x => property.GetValue(x).ToString());

            bool result = _items.Contains(newValue);

            return result is false;
        }


    }
}
