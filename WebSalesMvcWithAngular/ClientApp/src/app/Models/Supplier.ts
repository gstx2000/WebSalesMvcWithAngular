import { ProductDTO } from '../DTOs/ProductDTO';
import { Adress } from './Adress';
import { SupplierType } from './enums/SupplierType';

export interface Supplier {
  supplierId?: number;
  name: string;
  email?: string;
  phone?: string;
  CNPJ?: string;
  dayToPay?: number;
  contactPerson?: string;
  supplierType?: SupplierType;
  website?: string;
  shippingValue?: number;
  adresses?: Adress[];
  products?: ProductDTO[];
  productCount?: number;
}
