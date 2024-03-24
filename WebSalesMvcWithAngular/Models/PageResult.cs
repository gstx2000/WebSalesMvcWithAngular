namespace WebSalesMvcWithAngular.Models
{
    public class PagedResult<T>
    {
        public List<T> items { get; set; }
        public int page { get; set; }
        public int pageSize { get; set; }
        public int totalItems { get; set; }
        public int totalPages { get; set; }
    }

}
