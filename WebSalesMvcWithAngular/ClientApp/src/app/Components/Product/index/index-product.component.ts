import { Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { Product } from '../../../Models/Product';
import { ProductService } from '../../../Services/ProductService';
import { CategoryService } from '../../../Services/CategoryService';
import { Category } from '../../../Models/Category';
import { LoadingService } from '../../../Services/LoadingService';
import { Observable, Subject, of } from 'rxjs';
import { MatDialog } from '@angular/material/dialog';
import { DeleteProductComponent } from '../delete/delete-product.component';
import { catchError, debounceTime, distinctUntilChanged, map, startWith, switchMap, takeUntil } from 'rxjs/operators';
import { MatTableDataSource } from '@angular/material/table';
import { FormControl, FormGroup, FormBuilder } from '@angular/forms';
import { HttpErrorResponse } from '@angular/common/http';
import { AlertService } from '../../../Services/AlertService';
import { MatSort } from '@angular/material/sort';
import { MatPaginator } from '@angular/material/paginator';
import { ToastrService } from 'ngx-toastr';
import { ProductDTO } from '../../../DTOs/ProductDTO';

@Component({
  selector: 'app-products',
  templateUrl: './index-product.component.html',
  styleUrls: ['./index-product.component.css']
})
export class IndexProductComponent implements OnInit, OnDestroy {
  private paginator!: MatPaginator;
  private sort!: MatSort;

  productsDataSource: MatTableDataSource<ProductDTO>;
  categories: Category[] = [];

  filteredProducts: ProductDTO[] = [];
  searchControl: FormControl = new FormControl();
  searchForm!: FormGroup;
  searchedProducts: ProductDTO[] = [];
  categories$!: Observable<Category[]>;
  private destroy$ = new Subject<void>();
  isMessageVisible: boolean = false;

  currentPage = 1;
  pageSize = 10;
  totalItems = 0;
  totalPages = 0;
  products: ProductDTO[] = []; 

  @ViewChild(MatSort) set matSort(ms: MatSort) {
    this.sort = ms;
    this.setDataSourceAttributes();
  }
  @ViewChild(MatPaginator) set matPaginator(mp: MatPaginator) {
    this.paginator = mp;
    this.setDataSourceAttributes();
  }
  setDataSourceAttributes() {
    if (this.productsDataSource && this.paginator && this.sort) {
      this.productsDataSource.paginator = this.paginator;
      this.productsDataSource.sort = this.sort;
    }
  }
  constructor(
    private productService: ProductService,
    private categoryService: CategoryService,
    private loadingService: LoadingService,
    private dialog: MatDialog,
    private fb: FormBuilder,
    private alertService: AlertService,
    private toastr: ToastrService

  ) {
    this.productsDataSource = new MatTableDataSource<ProductDTO>;
  }

  ngOnInit(): void {
    this.initsearchForm();
    this.setupSearchControl();
    this.categories$ = this.categoryService.getCategories();
  }

  ngAfterViewInit() {

    this.productsDataSource.paginator = this.paginator;
    this.paginator.page
      .pipe(
        startWith({}),
        switchMap(() => {
          this.loadingService.showLoading();
          return this.productService.getProductsPaginated(
            this.paginator.pageIndex + 1,
            this.paginator.pageSize
          ).pipe(
            catchError((error) => {
              this.loadingService.hideLoading();
              return of(null);
            }),
            takeUntil(this.destroy$) 
          );
        }),
        map((Data) => {
          if (Data == null) return [];
          this.totalItems = Data.totalItems;
          this.loadingService.hideLoading();
          return Data.items;
        })
      )
      .subscribe((empData) => {
        this.products = empData;
        this.productsDataSource = new MatTableDataSource(this.products);
        this.productsDataSource.sort = this.sort;

      }, null, () => {
      });
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

  openDeleteDialog(product: ProductDTO): void {
    const dialogRef = this.dialog.open(DeleteProductComponent, {
      data: { product },
      width: '550px',
      height: '400px'
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result && result.deleted) {
        this.products = this.products.filter(p => p.id !== product.id);
        this.productsDataSource.data = this.products; 
      }
    });
  }
 }

