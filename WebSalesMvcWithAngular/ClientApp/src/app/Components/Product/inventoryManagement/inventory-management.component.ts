import { Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { ProductService } from '../../../Services/ProductService';
import { CategoryService } from '../../../Services/CategoryService';
import { Category } from '../../../Models/Category';
import { LoadingService } from '../../../Services/LoadingService';
import { Observable, Subject, of } from 'rxjs';
import { catchError, debounceTime, distinctUntilChanged, map, startWith, switchMap, takeUntil } from 'rxjs/operators';
import { FormControl, FormGroup, FormBuilder } from '@angular/forms';
import { HttpErrorResponse } from '@angular/common/http';
import { AlertService } from '../../../Services/AlertService';
import { ToastrService } from 'ngx-toastr';
import { Router } from '@angular/router';
import { InventoryUnitMeas } from '../../../Models/enums/InventoryUnitMeas';
import { ProductDTO } from '../../../DTOs/ProductDTO';

@Component({
  selector: 'app-inventory-management',
  templateUrl: './inventory-management.component.html',
  styleUrls: ['./inventory-management.component.css']
})
export class InventoryManagementComponent implements OnInit, OnDestroy {

  searchControl: FormControl = new FormControl();
  searchForm!: FormGroup;
  categories$!: Observable<Category[]>;
  isMessageVisible: boolean = false;
  private destroy$ = new Subject<void>();
  products: ProductDTO[] = []; 
  selectedProduct!: ProductDTO;
  filteredProducts: ProductDTO[] = [];
  productForm!: FormGroup;
  constructor(
    private productService: ProductService,
    private categoryService: CategoryService,
    private loadingService: LoadingService,
    private alertService: AlertService,
    private fb: FormBuilder,
    private toastr: ToastrService,
    private router: Router,

  ) { }

  ngOnInit(): void {
    this.initsearchForm();
    this.initProductForm();
    this.setupSearchControl();
    this.categories$ = this.categoryService.getCategories();
  }
  
  ngOnDestroy() {
    this.destroy$.next();
    this.destroy$.complete();
  }

  initsearchForm(): void {
    this.searchForm = this.fb.group({
      category: [null],
      search: this.searchControl
    });
  }

  initProductForm(): void {
    this.productForm = this.fb.group({
      inventoryQuantity: 0,
      acquisitionCost: 0,
      minimumInventoryQuantity: 0
    });
  }

  resetFilter(): void {
    this.searchControl.setValue('');
    this.searchForm.get('category')?.setValue(null);
  }

  private setupSearchControl(): void {
    this.searchControl.valueChanges.pipe(
      debounceTime(300),
      distinctUntilChanged(),
      switchMap((searchTerm: string) => {
        var categoryId = this.searchForm.get('category')?.value;
        if (typeof searchTerm === 'string' && searchTerm.trim() !== '') {
          return this.productService.searchProductsByNameDTO(searchTerm, categoryId).pipe(
            takeUntil(this.destroy$),
            catchError((error: any) => {
              if (error instanceof HttpErrorResponse && error.status === 404) {
                return of([]);
              } else {
                console.error('Erro ao buscar produtos:', error);
                this.toastr.error(error.message || 'Erro interno da aplicação, tente novamente.');
                throw error;
              }
            })
          );
        } else {
          return of(this.products);
        }
      })
    ).subscribe((products: ProductDTO[]) => {
      this.isMessageVisible = false;
      if (products.length !== 0) {
        this.filteredProducts = products.filter(p => !this.isProductSelected(p));

      } else {
        this.isMessageVisible = true;
        const htmlContent = `<p style="color: red;">Nenhum resultado encontrado para "${this.searchControl.value}".</p>`;
        this.alertService.showHtmlAlert('Resultados', htmlContent);
      }
    });
  }

  async onSubmit(): Promise<void> {
    try {
      if (this.productForm.valid) {
        const formData: ProductDTO = this.productForm.getRawValue();
        if (this.selectedProduct.id) {
          const productId = await (await this.productService.getProductById(this.selectedProduct.id)).toPromise();
          if (productId) {
            if (productId.id) {
              const updatedProduct = await (await this.productService.editInventory(productId.id, formData)).toPromise();
              this.toastr.success(`Produto ${this.selectedProduct?.name} alterado com sucesso.`);
            }
          }
        }
      }
    } catch (error: any) {
      this.toastr.error(error.message || 'Erro interno da aplicação, tente novamente.');
      console.error('Erro ao atualizar produto:', error);
    }
  }

  private isProductSelected(product: ProductDTO): boolean {
    return product.id === this.selectedProduct?.id;
  }

  selectProduct(product: ProductDTO): void {
    this.selectedProduct = product;
    this.productForm.patchValue({
      acquisitionCost: this.selectedProduct?.acquisitionCost,
      minimumInventoryQuantity: this.selectedProduct?.minimumInventoryQuantity 
      })

    this.searchControl?.setValue('');
  }

  getUnitMeasValues(): number[] {
    return Object.values(InventoryUnitMeas).filter(value => typeof value === 'number') as number[];
  }

  getUnitMeasName(value?: number): string {
    if (value === undefined) {
      return 'Indefinido';
    }
    return InventoryUnitMeas[value];
  }
}
