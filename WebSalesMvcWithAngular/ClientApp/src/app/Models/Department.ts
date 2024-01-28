import { Seller } from "./Seller";

export interface Department {
  id?: number;
  name: string;
  numberOfSellers?: number;
  sellers: Seller[];
  address: string;
  cnpj: string;
}
