using System;

namespace Permits.Core.Wrappers
{
    public interface ICurrentHttpContextWrapper
    {
        Uri GetRequestUrl();
        void AddHeader(string key, object value);
        void SetPaginationHeader(object value);
    }
}
