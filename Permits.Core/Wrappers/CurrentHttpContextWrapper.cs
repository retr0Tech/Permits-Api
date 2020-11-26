using Microsoft.ApplicationInsights.AspNetCore.Extensions;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Permits.Core.Wrappers
{
    public class CurrentHttpContextWrapper : ICurrentHttpContextWrapper
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentHttpContextWrapper(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public Uri GetRequestUrl()
        {
            return _httpContextAccessor.HttpContext.Request.HttpContext.Request.GetUri();
        }
        public bool HasHeader(string key)
        {
            return _httpContextAccessor.HttpContext.Request.HttpContext.Response.Headers.ContainsKey(key);
        }
        public void AddHeader(string key, object value)
        {
            if (!HasHeader(key))
                _httpContextAccessor.HttpContext.Request.HttpContext.Response.Headers.Add(key, value.ToString());
        }

        public void SetPaginationHeader(object value)
        {
            AddHeader("X-Pagination", JsonConvert.SerializeObject(value));
            AddHeader("Access-Control-Allow-Headers", "User-Agent, Content-Type, *");
            AddHeader("Access-Control-Allow-Origin", "*");
            AddHeader("Access-Control-Expose-Headers", "X-Pagination");
        }
    }
}
