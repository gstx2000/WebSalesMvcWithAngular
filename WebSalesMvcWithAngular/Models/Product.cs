using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebSalesMvcWithAngular.Models.Enums;
namespace WebSalesMvc.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Nome")]
        public required string Name { get; set; }

        [Display(Name = "Descrição")]
        public string? Description { get; set; }
        public int CategoryId { get; set; }

        [ForeignKey("CategoryId")]

        [Display(Name = "Categoria")]
        public Category? Category { get; set; }

        [Display(Name = "Preço")]
        public double Price { get; set; }

        [Display(Name = "Departamento")]
        public int DepartmentId { get; set; }

        [ForeignKey("DepartmentId")]
        public Department? Department { get; set; }

        [Display(Name = "Imagem")]
        public string? ImageUrl { get; set; }
        [Display(Name = "Unidade de medida")]
        public InventoryUnitMeas InventoryUnitMeas { get; set; } = InventoryUnitMeas.Unidades;
        //THIS IS USED TO CALCULATE THE CMV, IT SHOULD HAVE ALL THE COSTS INCLUDED LIKE:
        //TRANSPORT COSTS, STORAGE, MAINTENANCE, OBSOLENSCE, DEPRECIATION AND ETC.
        [Display(Name = "Custo de estoque")]
        public double? InventoryCost { get; set; }
        [Display(Name = "Quantidade em estoque")]
        public double? InventoryQuantity { get; set; }

        [Display(Name = "Custo de aquisição")]
        public double? AcquisitionCost { get; set; }

        //THIS QUANTITY IS USED TO ALERT WHEN PRODUCT IS BELOW THE MINIMUM LIMIT TO AVOID LACK OF STOCK

        [Display(Name = "Quantidade mínima de estoque")]
        public double? MinimumInventoryQuantity { get; set; }

        [Display(Name = "Valor total de estoque")]
        public double? TotalInventoryValue
        {
            get { return InventoryQuantity * Price; }
        }

        [Display(Name = "Valor total de estoque")]
        public double? TotalInventoryCost
        {
            get
            {
                if (InventoryCost == null)
                {
                    return null; 
                }
                return InventoryQuantity * InventoryCost.Value;
            }
        }

        //THIS CALCULATES THE CMV WITH THE STATE OF THE CLASS ITSELF, THE CALCULATION IS ALWAYS PERFORMED WITH THE
        //CURRENT INVENTORY DATA
        public double CalculateCMV()
        {
            if (InventoryQuantity == 0 || InventoryCost == null)
            {
                throw new InvalidOperationException("Não é possível calcular o CMV com quantidade de inventário igual a zero ou custo de inventário não definido.");
            }
            return (double)(InventoryCost.Value / InventoryQuantity);
        }
        //THIS CALCULATES THE COST REGARDLESS OF THE STATE OF THE CLASS
        private double CalculateCMV(double inventoryQuantity, double inventoryCost)
        {
            if (inventoryQuantity == 0)
            {
                throw new InvalidOperationException("Não é possível calcular o CMV com quantidade de inventário igual a zero.");
            }
            return inventoryCost / inventoryQuantity;
        }
        public void AddInventoryQuantity(double quantity)
        {
            InventoryQuantity += quantity;
        }
        public void RemoveInventoryQuantity(double quantity)
        {
            InventoryQuantity -= quantity;
        }
        public double CalculateProfitCMV()
        {
            if (InventoryQuantity == 0 || InventoryCost == null)
            {
                throw new InvalidOperationException("Não é possível calcular o lucro com CMV com quantidade de inventário igual a zero ou custo de inventário não definido.");
            }
            return Price - CalculateCMV();
        }


        public bool IsBelowMinimum(double inventoryQuantity, double minimumInventoryQuantity)
        {
            if(inventoryQuantity == 0)
            {
                throw new InvalidOperationException("O estoque está vazio.");
            }

            if (minimumInventoryQuantity == 0)
            {
                throw new InvalidOperationException("Esse produto não tem quantidade mínima definida.");
            }

            if (minimumInventoryQuantity > inventoryQuantity)
            {
                return true;   
            } else
            {
                return false;
            }

        }
        public bool IsBelowMinimum()
        {
            if (InventoryQuantity == 0 || MinimumInventoryQuantity == null)
            {
                throw new InvalidOperationException("Condições de estoque inválidas.");
            }

            if (MinimumInventoryQuantity > InventoryQuantity)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
