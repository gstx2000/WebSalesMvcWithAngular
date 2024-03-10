import { Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { Product } from '../../../Models/Product';
import { ProductService } from '../../../Services/ProductService';
import { Department } from '../../../Models/Department';
import { DepartmentService } from '../../../Services/DepartmentService';
import { CategoryService } from '../../../Services/CategoryService';
import { Category } from '../../../Models/Category';
import { LoadingService } from '../../../Services/LoadingService';
import { Observable, Subject, forkJoin, of } from 'rxjs';
import { MatDialog } from '@angular/material/dialog';
import { DeleteProductComponent } from '../delete/delete-product.component';
import { NgZone } from '@angular/core';
import { catchError, debounceTime, distinctUntilChanged, finalize, switchMap, takeUntil } from 'rxjs/operators';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { FormControl, FormGroup, FormBuilder } from '@angular/forms';
import { HttpErrorResponse } from '@angular/common/http';
import { AlertService } from '../../../Services/AlertService';

@Component({
  selector: 'app-products',
  templateUrl: './index-product.component.html',
  styleUrls: ['./index-product.component.css']
})
export class IndexProductComponent implements OnInit, OnDestroy {
  @ViewChild(MatSort) sort!: MatSort;

  products: Product[] = []; 
  productsDataSource: MatTableDataSource<Product>;
  departments: Department[] = [];
  categories: Category[] = [];

  filteredProducts: Product[] = [];
  searchControl: FormControl = new FormControl();
  searchForm!: FormGroup;
  searchedProducts: Product[] = [];
  categories$!: Observable<Category[]>;
  private destroy$ = new Subject<void>();
  isMessageVisible: boolean = false;

  constructor(
    private productService: ProductService,
    private departmentService: DepartmentService,
    private categoryService: CategoryService,
    private loadingService: LoadingService,
    private dialog: MatDialog,
    private zone: NgZone,
    private fb: FormBuilder,
    private alertService: AlertService
  ) {
    this.productsDataSource = new MatTableDataSource<Product>(this.products);
}

  ngOnInit(): void {
    this.loadProducts();
    this.initsearchForm();
    this.setupSearchControl();
  }

  ngAfterViewInit() {
    this.productsDataSource.sort = this.sort;
  }

  ngOnDestroy() {
    this.productsDataSource.sort = null;
    this.destroy$.next();
    this.destroy$.complete();
  }


  initsearchForm(): void {
    this.searchForm = this.fb.group({
      category: [null],
      search: this.searchControl,
    });
  }

  loadProducts(): void {
    this.zone.run(() => {
      this.loadingService.showLoading();
    });

    forkJoin([
      this.categoryService.getCategories(),
      this.departmentService.getDepartments(),
      this.productService.getProducts()
    ]).pipe(
      finalize(() => {
        this.zone.run(() => {
          this.loadingService.hideLoading();
        });
      })
    ).subscribe(
      ([categories, departments, products]) => {
        this.categories = categories;
        this.categories$ = this.categoryService.getCategories();
        this.departments = departments;
        this.products = products;
        this.productsDataSource.data = this.products;
        this.filterCategories();
        this.filterDepartments();
      },
      (error) => {
        console.error('Erro:', error);
      }
    );
  }

  filterCategories(): void {
    if (this.categories && this.products) {
      this.categories = this.categories.filter(category =>
        this.products.some(product => product.categoryId === category.id)
      );
      this.productsDataSource.data = this.products; 
    } else {
      console.warn('Departmentos ou produtos estão nulos ou indefinidos.');
    }
  }

  filterDepartments(): void {
    if (this.departments && this.products) {
      this.departments = this.departments.filter(department =>
        this.products.some(product => product.departmentId === department.id)
      );
      this.productsDataSource.data = this.products;
    } else {
      console.warn('Departmentos ou produtos estão nulos ou indefinidos.');
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
            takeUntil(this.destroy$),
            catchError((error: any) => {
              if (error instanceof HttpErrorResponse && error.status === 404) {
                return of([]);
              } else {
                console.error('Erro ao buscar produtos:', error);
                throw error;
              }
            })
          );
        } else {
          return of(this.products);
        }
      })
    ).subscribe((products: Product[]) => {
      this.isMessageVisible = false;
      if (products.length !== 0) {
        this.productsDataSource.data = products;
      } else {
        this.isMessageVisible = true;
        const htmlContent = `<p style="color: red;">Nenhum resultado encontrado para "${this.searchControl.value}".</p>`;
        this.alertService.showHtmlAlert('Resultados', htmlContent);
      }
    });
  }
  resetFilter(): void {
    this.productsDataSource.data = this.products;
    this.searchControl.setValue('');
    this.searchForm.get('category')?.setValue(null);
  }

  openDeleteDialog(product: Product): void {
    const dialogRef = this.dialog.open(DeleteProductComponent, {
      data: { product },
      width: '550px',
      height: '400px'
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result && result.deleted) {
        this.products = this.products.filter(p => p.id !== product.id);
        this.productsDataSource.data = this.products; 
        this.loadProducts();
      }
    });
  }
 }

