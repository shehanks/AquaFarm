namespace AquaFarm.Application.DTOs
{
    public class PaginatedResponse<T>
    {
        public IEnumerable<T> Items { get; set; } = Enumerable.Empty<T>();
        public MetaData Meta { get; set; } = new();
    }

    public class MetaData
    {
        public int TotalCount { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
    }
}
