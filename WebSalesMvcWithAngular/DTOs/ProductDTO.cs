using WebSalesMvcWithAngular.Models.Enums;

namespace WebSalesMvcWithAngular.DTOs
{
    public class ProductDTO 
    {
        public int? Id { get; set; }
        public string? Name { get; set; }    
        public double? Price { get; set; }
        public string? CategoryName { get; set; } 
        public string? DepartmentName { get; set; }
        public int? DepartmentId { get; set; }
        public int? CategoryId { get; set; }
        public string? ImageUrl { get; set; }
        public InventoryUnitMeas? InventoryUnitMeas { get; set; }
        public double? InventoryCost { get; set; }
        public double? InventoryQuantity { get; set; }
        public double? AcquisitionCost { get; set; }
        public double? MinimumInventoryQuantity { get; set; }
        public double? TotalInventoryValue { get; set; }
        public double? TotalInventoryCost { get; set; }
        public double? CMV { get; set; }
        public double? Profit { get; set; }
        public double? Margin { get; set; }
    }
}
