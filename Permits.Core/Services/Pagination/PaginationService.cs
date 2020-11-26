using Permits.Core.Models;
using Permits.Core.Options;
using Permits.Core.Services.Parser;
using Permits.Core.Wrappers;
using System;
using System.Linq;
using System.Linq.Dynamic.Core;

namespace Permits.Core.Services.Pagination
{
    public class PaginationService : IPaginationService
    {
        private readonly ICurrentHttpContextWrapper _currentHttpContextWrapper;
        private readonly IUrlFilterToDynamicLinqParser _urlFilterToDynamicLinqParser;
        public PaginationService(IUrlFilterToDynamicLinqParser urlFilterToDynamicLinqParser, ICurrentHttpContextWrapper currentHttpContextWrapper)
        {
            _urlFilterToDynamicLinqParser = urlFilterToDynamicLinqParser;
            _currentHttpContextWrapper = currentHttpContextWrapper;
        }

        public IQueryable<TEntity> SetHeader<TEntity>(IQueryable<TEntity> entitiesQuery, IApiQueryOption paginationInfo)
        {
            SetHeader(entitiesQuery.Count(), paginationInfo);
            return entitiesQuery;
        }
        public IQueryable<TEntity> Filter<TEntity>(IQueryable<TEntity> entitiesQuery, string where)
        {
            var query = entitiesQuery;
            var filterString = _urlFilterToDynamicLinqParser.Parse(where);
            if (!string.IsNullOrWhiteSpace(filterString))
                query = entitiesQuery.Where(filterString);
            return query;
        }
        public IQueryable<TEntity> Order<TEntity>(IQueryable<TEntity> entitiesQuery, IApiQueryOption paginationInfo)
        {
            string[] sortFields = string.IsNullOrWhiteSpace(paginationInfo.OrderBy) ? new string[] { "Id" } : paginationInfo.OrderBy.Replace(" ", "").Split(',');

            string completeSortExpression = "";
            foreach (string sortField in sortFields)
            {
                if (sortField.StartsWith("-"))
                    completeSortExpression = completeSortExpression + sortField.Remove(0, 1) + " descending,";
                else
                    completeSortExpression = completeSortExpression + sortField + ",";
            }

            if (!string.IsNullOrWhiteSpace(completeSortExpression))
                entitiesQuery = entitiesQuery.OrderBy(completeSortExpression.Remove(completeSortExpression.Count() - 1));

            return entitiesQuery;
        }

        public IQueryable<TEntity> Pagination<TEntity>(IQueryable<TEntity> entitiesQuery, IApiQueryOption paginationInfo)
        {
            return entitiesQuery.Skip(paginationInfo.PerPage * (paginationInfo.Page - 1)).Take(paginationInfo.PerPage);
        }

        private void SetHeader(int entitiesCount, IApiQueryOption paginationInfo)
        {
            int totalPages = (int)Math.Ceiling((double)entitiesCount / paginationInfo.PerPage);

            string baseUrl = _currentHttpContextWrapper.GetRequestUrl().AbsolutePath + "?page={0}&perPage={1}&orderBy={2}&where={3}";

            string previousLink =
                paginationInfo.Page > 1 ?
                string.Format(baseUrl, paginationInfo.Page - 1, paginationInfo.PerPage, paginationInfo.OrderBy, paginationInfo.Where) :
                "";

            string nextLink =
                paginationInfo.Page < totalPages ?
                string.Format(baseUrl, paginationInfo.Page + 1, paginationInfo.PerPage, paginationInfo.OrderBy, paginationInfo.Where) :
                "";

            _currentHttpContextWrapper.SetPaginationHeader(new PaginationHeader
            {
                currentPage = paginationInfo.Page,
                perPage = paginationInfo.PerPage,
                totalCount = entitiesCount,
                totalPages = totalPages,
                previousPageLink = previousLink,
                nextPageLink = nextLink
            });
        }
    }
}
