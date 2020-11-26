using AutoMapper;
using Microsoft.AspNet.OData.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Permits.Api.Controllers;
using System;

namespace Permits.Api.Filters
{
    public class ODataAttribute : ActionFilterAttribute
    {
        public IMapper _mapper;
        public Type _type;

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            var controller = context.Controller as IBaseController;

            var odataFeature = context.HttpContext.ODataFeature();
            var result = context.Result;
            if (odataFeature != null && result is OkObjectResult objectResult)
            {
                if (odataFeature.TotalCount != null)
                {
                    var dto = controller._mapper.Map(objectResult.Value, objectResult.Value.GetType(), controller.TypeDto);
                    objectResult.Value = new { count = odataFeature.TotalCount, Data = dto };
                    context.Result = objectResult;
                    context.HttpContext.Response.Headers.Add("$odatacount", odataFeature.TotalCount.ToString());
                }
            }
            base.OnActionExecuted(context);
        }
    }
}
