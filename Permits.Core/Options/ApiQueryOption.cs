namespace Permits.Core.Options
{
    public class ApiQueryOption : IApiQueryOption
    {
        public string Where { get; set; }
        public string OrderBy { get; set; }
        public int Page { get; set; }
        public int PerPage { get; set; }

        public ApiQueryOption()
        {
            Page = 1;
            PerPage = 40;
            OrderBy = "-id";
            Where = "";
        }
    }
}
