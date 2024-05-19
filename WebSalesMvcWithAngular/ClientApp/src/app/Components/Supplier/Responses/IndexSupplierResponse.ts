import { SupplierType } from "../../../Models/enums/SupplierType";

export interface IndexSupplierResponse {
  id?: number;
  name?: string;
  productCount?: number;
  supplierType?: SupplierType;
}

