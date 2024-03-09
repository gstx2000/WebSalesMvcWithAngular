import { Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { Product } from '../../../Models/Product';
import { ProductService } from '../../../Services/ProductService';
import { Department } from '../../../Models/Department';
import { DepartmentService } from '../../../Services/DepartmentService';
import { CategoryService } from '../../../Services/CategoryService';
import { Category } from '../../../Models/Category';
import { LoadingService } from '../../../Services/LoadingService';
import { forkJoin } from 'rxjs';
import { MatDialog } from '@angular/material/dialog';
import { DeleteProductComponent } from '../delete/delete-product.component';
import { NgZone } from '@angular/core';
import { finalize } from 'rxjs/operators';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';

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
  loadingCounter: number = 0;

  constructor(
    private productService: ProductService,
    private departmentService: DepartmentService,
    private categoryService: CategoryService,
    private loadingService: LoadingService,
    private dialog: MatDialog,
    private zone: NgZone

  ) {
    this.productsDataSource = new MatTableDataSource<Product>(this.products);
}

  ngOnInit(): void {

    this.loadProducts();
  }

  ngAfterViewInit() {
    this.productsDataSource.sort = this.sort;
    console.log('Sorting state:', this.productsDataSource.sort);

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

  ngOnDestroy() {
    this.productsDataSource.sort = null;
  }
 }

