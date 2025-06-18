namespace JobTracking.Domain.Filters
{
    public abstract class BaseFilter : IFilter
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}