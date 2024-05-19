﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebSalesMvcWithAngular.Models;
using WebSalesMvcWithAngular.Models.Enums;
namespace WebSalesMvc.Models
{
    [Help("")]

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
        public decimal Price { get; set; }

        [Display(Name = "Departamento")]
        public int DepartmentId { get; set; }

        [ForeignKey("DepartmentId")]
        public Department? Department { get; set; }
        public List<ProductSupplier> Suppliers { get; set; }

        [Display(Name = "Imagem")]
        public string? ImageUrl { get; set; }
        [Display(Name = "Unidade de medida")]
        public InventoryUnitMeas InventoryUnitMeas { get; set; } = InventoryUnitMeas.Unidades;
        //THIS IS USED TO CALCULATE THE CMV, IT SHOULD HAVE ALL THE COSTS INCLUDED LIKE:
        //TRANSPORT COSTS, STORAGE, MAINTENANCE, OBSOLENSCE, DEPRECIATION AND ETC.
        [Display(Name = "Custo de estoque")]
        public decimal? InventoryCost { get; set; }
        [Display(Name = "Quantidade em estoque")]
        public decimal? InventoryQuantity{ get; set; }

        [Display(Name = "Custo de aquisição")]
        public decimal? AcquisitionCost { get; set; }

        //THIS QUANTITY IS USED TO ALERT WHEN PRODUCT IS BELOW THE MINIMUM LIMIT TO AVOID LACK OF STOCK

        [Display(Name = "Quantidade mínima de estoque")]
        public decimal? MinimumInventoryQuantity { get; set; }
        
        [Display(Name = "Margem")]
        public decimal? Margin { get; set; }

        [Display(Name = "Código de barras")]
        public string? BarCode { get; set; }

        [Display(Name = "Valor total de estoque")]
        public decimal? TotalInventoryValue
        {
            get { return InventoryQuantity * Price; }
        }

        [Display(Name = "Valor total de estoque")]
        public decimal? TotalInventoryCost
        {
            get
            {
                if (InventoryCost == null)
                {
                    return 0;
                }
                return InventoryQuantity * InventoryCost.Value;
            }
        }

        [Display(Name = "Valor total de estoque")]
        public decimal? CMV
        {
            get
            {
               return CalculateCMV();
            }
        }

        //THIS CALCULATES THE CMV WITH THE STATE OF THE CLASS ITSELF, THE CALCULATION IS ALWAYS PERFORMED WITH THE
        //CURRENT INVENTORY DATA
        public decimal CalculateCMV()
        {
            if (InventoryQuantity == 0 || InventoryQuantity == null || InventoryCost == null || InventoryCost == 0)
            {
                return 0;
            }
            return (decimal)(InventoryCost.Value / InventoryQuantity);
        }
        //THIS CALCULATES THE COST REGARDLESS OF THE STATE OF THE CLASS
        private decimal CalculateCMV(decimal inventoryQuantity, decimal inventoryCost)
        {
            if (inventoryQuantity == 0)
            {
                throw new InvalidOperationException("Não é possível calcular o CMV com quantidade de inventário igual a zero.");
            }
            return inventoryCost / inventoryQuantity;
        }

        public decimal CalculateProfit() 
        {
            if (InventoryQuantity == 0 || InventoryQuantity == null|| AcquisitionCost == null || AcquisitionCost == 0)
            {
                return 0;
            }
            return (decimal)(Price - (AcquisitionCost ?? 0));
        }
        public bool IsBelowMinimum(decimal inventoryQuantity, decimal minimumInventoryQuantity)
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
