namespace Permits.Core.Options
{
    public interface IApiQueryOption
    {
        string Where { get; set; }
        string OrderBy { get; set; }
        int Page { get; set; }
        int PerPage { get; set; }
    }
}
