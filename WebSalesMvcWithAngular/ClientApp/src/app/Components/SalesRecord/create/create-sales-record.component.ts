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
import { SoldProduct } from '../../../Models/SoldProduct';

@Component({
  selector: 'app-sales-record/create',
  templateUrl: './create-sales-record.component.html',
  styleUrls: ['./create-sales-record.component.css']
})
export class CreateSalesRecordComponent implements OnInit {
  salesForm!: FormGroup;
  searchForm!: FormGroup;
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
  selectedProducts: SoldProduct[] = [];
  quantity!: number;

  selectedProductsDataSource = new MatTableDataSource<SoldProduct>(this.selectedProducts);

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
      sellerid: 1
    };
  }

  ngOnInit(): void {
    this.loadingService.showLoading();

    this.departments$ = this.departmentService.getDepartments();
    this.products$ = this.productService.getProducts();
    this.categories$ = this.categoryService.getCategories();
    this.setupSearchControl();
    this.initSalesForm();
    this.initsearchForm();
    this.selectedProductsDataSource = new MatTableDataSource<SoldProduct>(this.selectedProducts);
    this.loadingService.hideLoading();

  }

  initSalesForm(): void {
    this.salesForm = this.fb.group({
      amount: [0, [Validators.required, Validators.min(0.01)]],
      status: [0, Validators.required],
      paymentMethod: [0, Validators.required],
    });
  }

  initsearchForm(): void {
    this.searchForm = this.fb.group({
      category: [null],
      search: this.searchControl,
      quantity: [1, [Validators.required, Validators.min(1)]]

    });

    this.searchControl?.setValue('');

    this.searchControl?.disable({ onlySelf: true, emitEvent: false });

    this.searchForm.get('category')?.valueChanges.subscribe(value => {
      if (value === null) {
        this.searchControl?.disable({ onlySelf: true, emitEvent: false });
      } else {
        this.searchControl.enable({ onlySelf: true, emitEvent: false });
      }
    });
  }
  
  async onSubmit(): Promise<void> {
    try {
      this.loadingService.showLoading();

      if (this.salesForm.valid) {

        const formData: SalesRecord = this.salesForm.value;
        formData.sellerid = 1;
        formData.soldProducts = this.selectedProducts;
        console.log('selected products:', this.selectedProducts);

        const createdSales = await (await this.SalesService.createSalesRecordAsync(formData)).toPromise();
        console.log('venda:', createdSales);
        console.log('selected products:', this.selectedProducts);


        this.router.navigate(['/salesRecords']);
      }
    } catch (error) {
      console.log('venda:', this.salesForm.value);
      console.error('Erro ao criar:', error);
      this.loadingService.hideLoading();
    } finally {
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

  removeFromOrder(soldProduct: SoldProduct): void {
    this.selectedProducts = this.selectedProducts.filter(p => p !== soldProduct);

    if (this.selectedProductsDataSource) {
      this.selectedProductsDataSource.data = this.selectedProducts;

      const totalAmount = this.selectedProducts.reduce((sum, sp) => sum + sp.price * sp.quantity, 0);

      this.salesForm.get('amount')?.setValue(totalAmount);
    }
  }

  private setupSearchControl(): void {
    this.searchControl.valueChanges.pipe(
      debounceTime(300),
      distinctUntilChanged(),
      switchMap((searchTerm: string) => {
        const categoryId = this.searchForm.get('category')?.value;

        if (typeof searchTerm === 'string' && searchTerm.trim() !== '') {
          return this.productService.searchProductsByName(searchTerm, categoryId).pipe(
            catchError((error: any) => {
              if (error instanceof HttpErrorResponse && error.status === 404) {
                return [];
              } else {
                console.error('Error during product search:', error);
                throw error;
              }
            })
          );
        } else {
          return new Observable<Product[]>();
        }
      })
    ).subscribe((products: Product[]) => {
      this.filteredProducts = products.filter(p => !this.isProductSelected(p));
    });
  }

  private isProductSelected(product: Product): boolean {
    return this.selectedProducts.some(soldProduct => soldProduct.productId === product.id);
  }

  selectProduct(product: Product): void {

    const existingProduct = this.selectedProducts.find(sp => sp.productId === product.id);

    if (!existingProduct) {

      var price = product.price;
      var name = product.name;
      var id = product.id;

      const soldProduct: SoldProduct = {
        productId: id!,
        quantity: this.searchForm.get('quantity')?.value,
        name: name,
        price: price
      };

      this.selectedProducts.push(soldProduct);
      this.salesForm.get('products')?.setValue(this.selectedProducts);

      const totalAmount = this.selectedProducts.reduce((sum, sp) => sum + sp.price * sp.quantity, 0);
      this.salesForm.get('amount')?.setValue(totalAmount);

      if (this.selectedProductsDataSource) {
        this.selectedProductsDataSource.data = this.selectedProducts;
      }

      this.searchControl?.setValue('');
    }
  }

}
