namespace WebSalesMvc.Models
{
    public class CategoryFormViewModel
    {
        public Category? Category { get; set; }
        public ICollection<Department>? Departments { get; set; }

    }
}
