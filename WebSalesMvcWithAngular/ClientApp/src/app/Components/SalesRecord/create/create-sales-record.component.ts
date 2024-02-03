import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { Department } from '../../../Models/Department';
import { Router } from '@angular/router';
import { Product } from '../../../Models/Product';
import { ProductService } from '../../../Services/ProductService';
import { DepartmentService } from '../../../Services/DepartmentService';
import { Observable } from 'rxjs';
import { SaleStatus } from '../../../Models/enums/SaleStatus';
import { SalesRecord } from '../../../Models/SalesRecord';
import { SalesRecordService } from '../../../Services/SalesRecordService';
import { PaymentMethod } from '../../../Models/enums/PaymentMethod';
import { Seller } from '../../../Models/Seller';

@Component({
  selector: 'app-sales-record/create',
  templateUrl: './create-sales-record.component.html',
  styleUrls: ['./create-sales-record.component.css']
})
export class CreateSalesRecordComponent  implements OnInit {
  products: Product[] = [];
  salesForm!: FormGroup;
  salesRecord: SalesRecord;
  departments$: Observable<Department[]> | undefined;
  products$: Observable<Product[]> | undefined;
  PaymentMethod = PaymentMethod;
  SaleStatus = SaleStatus;
  sellers$: Observable<Seller[]> | undefined;

  saleStatusValues: (keyof typeof SaleStatus)[] = Object.keys(SaleStatus) as (keyof typeof SaleStatus)[];

  salePayMethodValues: (keyof typeof PaymentMethod)[] = Object.keys(PaymentMethod) as (keyof typeof PaymentMethod)[];
  constructor(
    private SalesService: SalesRecordService,
    private productService: ProductService,
    private departmentService: DepartmentService,
    private fb: FormBuilder,
    private router: Router
  ) {
    this.salesRecord = {
      amount: 0,
      status: 0,
      paymentMethod: 0,
      date: new Date()
    };
  }

  ngOnInit(): void {
    this.initSalesForm();
    this.departments$ = this.departmentService.getDepartments();
    this.products$ = this.productService.getProducts();
  }

  initSalesForm(): void {
    this.salesForm = this.fb.group({
      amount: [0, [Validators.required, Validators.min(0.01)]],
      status: [0, Validators.required],
      paymentMethod: [0, Validators.required],
      date: [new Date()]
    });
  }

  async onSubmit(): Promise<void> {
    try {
      if (this.salesForm.valid) {
        const formData: SalesRecord = this.salesForm.value;
        formData.date = new Date();
        const createdSales = await (await this.SalesService.createSalesRecord(formData)).toPromise();
        this.router.navigate(['/salesRecords']);
      }
    } catch (error) {
      console.error('Erro ao criar:', error);
    }
  }

  getPaymentMethodValues(): number[] {
    return Object.values(PaymentMethod).filter(value => typeof value === 'number') as number[];
  }

  getPaymentMethodName(value: number): string {
    return PaymentMethod[value] as string;
  }
  getSaleStatusValues(): number[] {
    return Object.values(SaleStatus).filter(value => typeof value === 'number') as number[];
  }

  getSaleStatusName(value: number): string {
    return SaleStatus[value] as string;
  }

  cancel() {
    this.router.navigate(['/salesRecords']);
  }
}

