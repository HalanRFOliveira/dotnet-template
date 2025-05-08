namespace Dotnet.Template.Infra.Paging
{
    public class PagedFilter<T>
    {
        public string Search { get; set; }
        public DateTime? PeriodStartAt { get; set; }
        public DateTime? PeriodEndAt { get; set; }

    }
}
