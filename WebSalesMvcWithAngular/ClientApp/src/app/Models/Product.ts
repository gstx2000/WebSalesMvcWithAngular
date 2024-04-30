import { Category } from "./Category";
import { Department } from "./Department";
export interface Product {
  id?: number;
  name: string;
  price: number;
  description: string;
  category?: Category;
  categoryId: number;
  department?: Department;
  departmentId: number;
  imageUrl: string;
  inventoryUnitMeas: number;
  inventoryCost?: number;
  inventoryQuantity?: number;
  acquisitionCost?: number;
  minimumInventoryQuantity?: number;
  totalInventoryValue?: number;
  totalInventoryCost?: number;

}
