import { Department } from "./Department";
import { Product } from "./Product";
export interface Category {
  id?: number;
  name: string;
  description: string;
  department?: Department;
  departmentId: number;
  products?: Product[];
  productCount?: number;
}
