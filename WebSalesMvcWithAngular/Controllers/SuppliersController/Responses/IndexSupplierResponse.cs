using WebSalesMvcWithAngular.Models.Enums;

namespace WebSalesMvcWithAngular.Controllers.SuppliersController.Responses
{
    public class IndexSupplierResponse
    {
        public int? SupplierId { get; set; }
        public string? Name { get; set; }
        public SupplierType? SupplierType { get; set; }

        public int? ProductCount { get; set; }
    }
}
