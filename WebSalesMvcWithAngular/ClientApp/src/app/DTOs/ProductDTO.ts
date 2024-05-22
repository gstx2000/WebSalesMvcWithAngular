import { SupplierDTO } from "./SupplierDTO";

export interface ProductDTO {
  id?: number;
  name?: string;
  price?: number;
  categoryName?: string;
  description?: string;
  categoryId?: number;
  departmentName?: string; 
  departmentId?: number;
  inventoryUnitMeas?: number;
  inventoryCost?: number;
  inventoryQuantity?: number;
  acquisitionCost?: number;
  minimumInventoryQuantity?: number;
  totalInventoryValue?: number;
  totalInventoryCost?: number;
  profit?: number;
  margin?: number;
  suppliers?: SupplierDTO[];
  supplyPrice?: number;
  subCategoryName?: string;
  subCategoryId?: number;
}

