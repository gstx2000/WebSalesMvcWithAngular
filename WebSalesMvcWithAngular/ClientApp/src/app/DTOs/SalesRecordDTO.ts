import { Seller } from "../Models/Seller";
import { SoldProduct } from "../Models/SoldProduct";

export interface SalesRecordDTO {
  id?: number;
  amount?: number;
  status?: number;
  paymentMethod?: number;
  seller?: Seller;
  sellerid?: number;
  soldProducts?: SoldProduct[];
}
