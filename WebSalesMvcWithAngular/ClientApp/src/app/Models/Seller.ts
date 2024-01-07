import { Department } from "./Department";
import { SalesRecord } from "./SalesRecord";

export interface Seller {
  id: number;
  name: string;
  department: Department;
  phone: string;
  email: string;
  salary: number;
  birthdate: Date;
  departmentid: number;
  sales: SalesRecord[];

}
