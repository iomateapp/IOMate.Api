namespace IOMate.Application.Shared.Dtos
{
    public class PagedResponseDto<T>
    {
        public int TotalCount { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public List<T> Results { get; set; } = new();
    }
}