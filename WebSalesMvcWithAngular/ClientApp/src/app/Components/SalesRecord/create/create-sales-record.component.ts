import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { Department } from '../../../Models/Department';
import { Router } from '@angular/router';
import { Product } from '../../../Models/Product';
import { ProductService } from '../../../Services/ProductService';
import { DepartmentService } from '../../../Services/DepartmentService';
import { Observable, catchError } from 'rxjs';
import { debounceTime, distinctUntilChanged, switchMap } from 'rxjs/operators';
import { SaleStatus } from '../../../Models/enums/SaleStatus';
import { SalesRecord } from '../../../Models/SalesRecord';
import { SalesRecordService } from '../../../Services/SalesRecordService';
import { PaymentMethod } from '../../../Models/enums/PaymentMethod';
import { Seller } from '../../../Models/Seller';
import { CategoryService } from '../../../Services/CategoryService';
import { Category } from '../../../Models/Category';
import { MatTableDataSource } from '@angular/material/table';
import { LoadingService } from '../../../Services/LoadingService';
import { HttpErrorResponse } from '@angular/common/http';

@Component({
  selector: 'app-sales-record/create',
  templateUrl: './create-sales-record.component.html',
  styleUrls: ['./create-sales-record.component.css']
})
export class CreateSalesRecordComponent implements OnInit {
  salesForm!: FormGroup;
  salesRecord: SalesRecord;
  departments$: Observable<Department[]> | undefined;
  products$: Observable<Product[]> | undefined;
  PaymentMethod = PaymentMethod;
  SaleStatus = SaleStatus;
  sellers$: Observable<Seller[]> | undefined;
  categories$: Observable<Category[]> | undefined;
  searchControl: FormControl = new FormControl();
  filteredProducts: Product[] = [];
  searchTerm: string = '';
  selectedProducts: Product[] = [];

  selectedProductsDataSource = new MatTableDataSource<Product>();

  saleStatusValues: (keyof typeof SaleStatus)[] = Object.keys(SaleStatus) as (keyof typeof SaleStatus)[];
  salePayMethodValues: (keyof typeof PaymentMethod)[] = Object.keys(PaymentMethod) as (keyof typeof PaymentMethod)[];

  constructor(
    private SalesService: SalesRecordService,
    private productService: ProductService,
    private departmentService: DepartmentService,
    private fb: FormBuilder,
    private router: Router,
    private categoryService: CategoryService,
    private loadingService: LoadingService

  ) {
    this.salesRecord = {
      amount: 0,
      status: 0,
      paymentMethod: 0,
      date: new Date(),
    };
  }

  ngOnInit(): void {
    this.departments$ = this.departmentService.getDepartments();
    this.products$ = this.productService.getProducts();
    this.categories$ = this.categoryService.getCategories();
    this.setupSearchControl();
    this.initSalesForm();
    this.selectedProductsDataSource = new MatTableDataSource<Product>(this.selectedProducts);

  }

  initSalesForm(): void {
    this.salesForm = this.fb.group({
      amount: [0, [Validators.required, Validators.min(0.01)]],
      status: [0, Validators.required],
      paymentMethod: [0, Validators.required],
      date: [new Date()],
      products: [[]],
      category: [null] 
    });

    this.searchControl.setValue('');

  }

  async onSubmit(): Promise<void> {
    try {
      if (this.salesForm.valid) {
        const formData: SalesRecord = this.salesForm.value;
        formData.date = new Date();
        formData.products = this.selectedProducts;
        const createdSales = await (await this.SalesService.createSalesRecord(formData)).toPromise();
        this.router.navigate(['/salesRecords']);
      }
    } catch (error) {
      console.error('Erro ao criar:', error);
      this.loadingService.hideLoading();
    }
    () => {
      this.loadingService.hideLoading();
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

  cancel(): void {
    this.router.navigate(['/salesRecords']);
  }

  RemoveFromOrder(product: Product): void {
    this.selectedProducts = this.selectedProducts.filter(p => p !== product);
    if (this.selectedProductsDataSource) {
      this.selectedProductsDataSource.data = this.selectedProducts;
      console.log('Filtered Products:', this.selectedProducts);

    }
  }

  private setupSearchControl(): void {
    this.searchControl.valueChanges.pipe(
      debounceTime(300),
      distinctUntilChanged(),
      switchMap((searchTerm: string) => {
        const categoryId = this.salesForm.get('category')?.value;

        if (searchTerm.trim() !== '') {

          return this.productService.searchProductsByName(searchTerm, categoryId).pipe(
            catchError((error: any) => {
              if (error instanceof HttpErrorResponse && error.status === 404) {
                console.warn();
                  return [];
              } else {
                console.error('Error during product search:', error);
                throw error;
              }
            })
          );
        } else {
          return new Observable<Product[]>(); // Return an empty observable for null or empty searchTerm
        }
      })
    ).subscribe((products: Product[]) => {
      console.log('Filtered Products:', products);

      this.filteredProducts = products.filter(p => !this.selectedProducts.includes(p));
    });
  }

  selectProduct(product: Product): void {
    const existingProduct = this.selectedProducts.find(p => p.id === product.id);

    if (!existingProduct) {
      this.selectedProducts.push(product);
      this.salesForm.get('products')!.setValue(this.selectedProducts);

      if (this.selectedProductsDataSource) {
        this.selectedProductsDataSource.data = this.selectedProducts;
        this.searchControl.setValue('');
      }

      this.searchControl.setValue('');
    } else {
      this.searchControl.setValue('');
    }
  }

}
