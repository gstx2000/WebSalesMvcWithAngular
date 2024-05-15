using System.ComponentModel.DataAnnotations;
using WebSalesMvc.Models;

namespace WebSalesMvcWithAngular.Models
{
    [Help("Essa classe é similar a ProductSupplier, mas é usada para registrar e armazenar entrada de estoque com" +
        "base no fornecedor e no preço agrupando por um LOTE. Um fornecedor junto com o preço formam um lote," +
        "se o preço de aquisição e o fornecedor forem diferentes, deve ser criado um novo lote no estoque. " +
        "Dessa forma, quando forem efetuadas vendas, será mais fácil calcular o lucro, o custo a margem e etc. ")]
    public class InventoryReceipt
    {

        [Key]
        public int LotId { get; set; }
        public int? SupplierId { get; set; }
        public Supplier? Supplier { get; set; }
        public int? ProductId { get; set; }
        public Product? Product { get; set; }
        public DateTime? Date { get; set; }
        public decimal? SupplyPrice { get; set; }
        public decimal? InventoryQuantity { get; set; }

        public void RemoveInventoryQuantity(decimal quantity)
        {
            InventoryQuantity -= quantity;
        }
        public void AddInventoryQuantity(decimal quantity)
        {
            InventoryQuantity += quantity;
        }


    }
}
