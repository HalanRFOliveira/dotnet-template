using Microsoft.AspNetCore.Mvc;

namespace Dotnet.Template.Infra.Paging
{
    public class Filter<T> where T : class, new()
    {
		public Filter()
        {
            Data = new T();
        }

        public T Data { get; set; }

        public int Page { get; set; } = 1;

        [FromQuery]
        public int Limit { get; set; } = 10;

        public string SortBy { get; set; } = "id";

        public bool? Descending { get; set; } = null;

        public bool HasSorter
        {
            get
            {
                return Descending.HasValue && !string.IsNullOrWhiteSpace(SortBy);
            }
        }

        public int SkipCount
        {
            get
            {
                return (Page - 1) * Limit;
            }
        }
    }
}
