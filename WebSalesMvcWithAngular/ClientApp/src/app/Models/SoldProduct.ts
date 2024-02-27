import { Product } from "./Product";

export interface SoldProduct {
  id?: number;
  salesRecordId?: number;
  productId: number;
  product?: Product;
  quantity: number;
  name: string;
  price: number;
}
