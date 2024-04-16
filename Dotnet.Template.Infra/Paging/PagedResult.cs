namespace Dotnet.Template.Infra.Paging
{
    public class PagedResult<T> where T : class
    {
        public PagedResult(int totalSize, int pageSize, IEnumerable<T> data)
        {
            TotalSize = totalSize;
            PageSize = pageSize;
            Data = data;
        }

        public int TotalSize { get; private set; }
        public int PageSize { get; private set; }
        public IEnumerable<T> Data { get; private set; }
    }
}
