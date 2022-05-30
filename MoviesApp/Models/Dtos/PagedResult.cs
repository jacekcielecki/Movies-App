namespace MoviesApp.Models
{
    public class PagedResult<T>
    {
        public IEnumerable<T> Items { get; set; }
        public int TotalItemsCount { get; set; }
        public int TotalPages { get; set; }
        public int PageNr { get; set; }
        public int ItemFrom { get; set; }
        public int ItemsTo { get; set; }

        public PagedResult(IEnumerable<T> items, int totalCount, int pageSize, int pageNumber)
        {
            Items = items;
            TotalItemsCount = totalCount;
            TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
            PageNr = pageNumber;
            ItemFrom = (pageSize * (pageNumber - 1)) + 1;
            ItemsTo = ItemFrom + pageSize -1;
        }
    }
}
