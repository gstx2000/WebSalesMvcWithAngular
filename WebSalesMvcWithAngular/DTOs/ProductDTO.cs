using WebSalesMvcWithAngular.Models.Enums;

namespace WebSalesMvcWithAngular.DTOs
{
    [Help("SupplierID é usada apenas para dar entrada no estoque associada com o fornecedor, pois os produtos podem possuir" +
        "muitos fornecedores, e por isso que contém a tabela de junção ProductSupplier. A lista de fornecedores utilizada " +
        "aqui é apenas para transporte dos dados para o Client, o Controller de Product utiliza apenas para extrair " +
        "APENAS nome dos fornecedores associados ao produto da classe Supplier. Dessa forma, menos dados são enviados na" +
        "requisição do Client.")]

    public class ProductDTO 
    {
        public int? Id { get; set; }
        public string? Name { get; set; }    
        public decimal? Price { get; set; }
        public string? BarCode { get; set; }
        public string? CategoryName { get; set; } 
        public string? DepartmentName { get; set; }
        public int? DepartmentId { get; set; }
        public int? CategoryId { get; set; }
        public int? SubCategoryId { get; set; }
        public string? SubCategoryName { get; set; }
        public string? ImageUrl { get; set; }
        public InventoryUnitMeas? InventoryUnitMeas { get; set; }
        public decimal? InventoryCost { get; set; }
        public decimal? InventoryQuantity { get; set; }
        public decimal? AcquisitionCost { get; set; }
        public decimal? MinimumInventoryQuantity { get; set; }
        public decimal? TotalInventoryValue { get; set; }
        public decimal? TotalInventoryCost { get; set; }
        public decimal? CMV { get; set; }
        public decimal? Margin { get; set; }
        public int? SupplierId { get; set; }
        public string? NCM { get; set; }
        public List<SupplierDTO>? Suppliers { get; set; }
    }
}
