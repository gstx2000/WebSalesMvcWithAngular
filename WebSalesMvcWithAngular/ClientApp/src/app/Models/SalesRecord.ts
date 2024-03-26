import { SoldProduct } from "./SoldProduct";
import { Seller } from "./Seller";


export interface SalesRecord {
  id?: number;
  amount: number;
  status: number;
  seller?: Seller;
  sellerid?: number;
  soldProducts?: SoldProduct[];
  paymentMethod: number;
}
