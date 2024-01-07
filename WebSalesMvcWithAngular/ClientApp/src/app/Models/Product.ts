import { Category } from "./Category";
import { Department } from "./Department";

export interface Product {
  id: number;
  price: number;
  name: string;
  description: string;
  category: Category;
  categoryId: number;
  department: Department;
  imageUrl: string;

}
