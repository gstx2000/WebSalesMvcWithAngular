import { Component, OnDestroy, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { Product } from '../../../Models/Product';
import { ProductService } from '../../../Services/ProductService';
import { Observable, Subject, catchError } from 'rxjs';
import { debounceTime, distinctUntilChanged, switchMap, takeUntil } from 'rxjs/operators';
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
import { ToastrService } from 'ngx-toastr';
import { ProductDTO } from '../../../DTOs/ProductDTO';

@Component({
  selector: 'app-sales-records/create-salesrecord',
  templateUrl: './create-sales-record.component.html',
  styleUrls: ['./create-sales-record.component.css']
})
export class CreateSalesRecordComponent implements OnInit, OnDestroy {
  salesForm!: FormGroup;
  searchForm!: FormGroup;

  selectedProducts: SoldProduct[] = [];

  filteredProducts: ProductDTO[] = [];
  salesRecord: SalesRecord;
  PaymentMethod = PaymentMethod;
  SaleStatus = SaleStatus;

  searchControl: FormControl = new FormControl();
  searchTerm: string = '';
  quantity = new FormControl();

  sellers$!: Observable<Seller[]>;
  products$!: Observable<Product[]>;
  categories$!: Observable<Category[]>;

  selectedProductsDataSource = new MatTableDataSource<SoldProduct>(this.selectedProducts);

  saleStatusValues: (keyof typeof SaleStatus)[] = Object.keys(SaleStatus) as (keyof typeof SaleStatus)[];
  salePayMethodValues: (keyof typeof PaymentMethod)[] = Object.keys(PaymentMethod) as (keyof typeof PaymentMethod)[];

  private destroy$ = new Subject<void>();

  constructor(
    private SalesService: SalesRecordService,
    private productService: ProductService,
    private fb: FormBuilder,
    private router: Router,
    private categoryService: CategoryService,
    private loadingService: LoadingService,
    private toastr: ToastrService

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
    this.setupSearchControl();
    this.initSalesForm();
    this.initsearchForm();
    this.products$ = this.productService.getProducts();
    this.categories$ = this.categoryService.getCategories();
    this.loadingService.hideLoading();
  }

  ngOnDestroy() {
    this.destroy$.next();
    this.destroy$.complete();
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
      quantity: 1

    });

    this.searchForm.get('quantity')?.valueChanges.subscribe(value => {
      if (value === null || value == 0) {
        this.searchControl?.disable({ onlySelf: true, emitEvent: false });
      } else {
        this.searchControl.enable({ onlySelf: true, emitEvent: false });
      }
    });
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
          return this.productService.searchProductsByNameDTO(searchTerm, categoryId).pipe(
            takeUntil(this.destroy$),
            catchError((error: any) => {
              if (error instanceof HttpErrorResponse && error.status === 404) {
                return [];
              } else {
                console.error('Erro ao buscar produtos:', error);
                this.toastr.error(error.message || 'Erro interno da aplicação, tente novamente.');
                throw error;
              }
            })
          );
        } else {
          return new Observable<ProductDTO[]>();
        }
      })
    ).subscribe((products: ProductDTO[]) => {
      this.filteredProducts = products.filter(p => !this.isProductSelected(p));
    });
  }

  private isProductSelected(product: ProductDTO): boolean {
    return this.selectedProducts.some(soldProduct => soldProduct.productId === product.id);
  }

  selectProduct(product: ProductDTO): void {

    const existingProduct = this.selectedProducts.find(sp => sp.productId === product.id);

    if (!existingProduct) {

      var price = product.price;
      var name = product.name;
      var id = product.id;

      const soldProduct: SoldProduct = {
        productId: id!,
        quantity: this.searchForm.get('quantity')?.value,
        name: name!,
        price: price!
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

  async onSubmit(): Promise<void> {
    try {
      this.loadingService.showLoading();

      if (this.salesForm.valid) {

        const formData: SalesRecord = this.salesForm.value;
        formData.sellerid = 1;
        formData.soldProducts = this.selectedProducts
        const createdSales = await (await this.SalesService.createSalesRecordAsync(formData)).toPromise();
        this.toastr.success('A venda foi registrada com sucesso.')
        this.router.navigate(['/salesRecords']);
      }
    } catch (error: any) {
      if (error instanceof HttpErrorResponse) {
        const errorBody = error.error;
        if (errorBody) {
          this.toastr.error(errorBody);
        } else {
          this.toastr.error('Ocorreu um erro, tente novamente.');
        }
      } else {
        this.toastr.error('Ocorreu um erro, tente novamente.');
      }
      console.error('Erro ao realizar venda:', error);
      this.loadingService.hideLoading();
    }
  }
}
