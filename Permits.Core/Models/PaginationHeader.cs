namespace Permits.Core.Models
{
    public class PaginationHeader
    {
        public int currentPage { get; set; }
        public int perPage { get; set; }
        public int totalCount { get; set; }
        public int totalPages { get; set; }
        public string previousPageLink { get; set; }
        public string nextPageLink { get; set; }
    }
}
