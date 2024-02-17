import { Product } from "./Product";
import { Seller } from "./Seller";


export interface SalesRecord {
  id?: number;
  amount: number;
  status: number;
  seller?: Seller;
  sellerid?: number;
  products?: Product[];
  paymentMethod: number;


}
