namespace WebSalesMvcWithAngular.DTOs
{
    public class ProductDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }    
        public double Price { get; set; } 
        public string CategoryName { get; set; } 
        public string DepartmentName { get; set; }
        public int DepartmentId { get; set; }
        public int CategoryId { get; set; }

    }
}
