namespace WebSalesMvcWithAngular.DTOs
{
    public class CategoryDTO
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? DepartmentName { get; set; }
        public int? ProductCount { get; set; }
        public bool IsSubCategory { get; set; } = false;

    }
}
