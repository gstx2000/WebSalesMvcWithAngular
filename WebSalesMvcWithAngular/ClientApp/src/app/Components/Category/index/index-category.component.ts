import { Component, OnInit } from '@angular/core';
import { Department } from '../../../Models/Department';
import { DepartmentService } from '../../../Services/DepartmentService';
import { Category } from '../../../Models/Category';
import { CategoryService } from '../../../Services/CategoryService';
import { LoadingService } from '../../../Services/LoadingService';
import { MatDialog } from '@angular/material/dialog';
import { DeleteCategoryComponent } from '../delete/delete-category.component';
import { Product } from '../../../Models/Product';
import { ProductService } from '../../../Services/ProductService';
import { forkJoin } from 'rxjs';

@Component({
  selector: 'app-categories',
  templateUrl: './index-category.component.html',
  styleUrls: ['./index-category.component.css']
})
export class IndexCategoryComponent implements OnInit {
  departments: Department[] = [];
  categories: Category[] = [];
  products: Product[] = [];
  constructor(
    private departmentService: DepartmentService,
    private categoryService: CategoryService,
    private loadingService: LoadingService,
    private productService: ProductService,

    private dialog: MatDialog
  ) { }
    
  ngOnInit(): void {
    this.loadingService.showLoading();

    forkJoin([
      this.departmentService.getDepartments(),
      this.productService.getProducts(),
      this.categoryService.getCategories()
    ]).subscribe(
      ([departments, products, categories]) => {
        this.departments = departments;
        this.products = products;
        this.categories = categories;

        this.categories.forEach(category => {
          category.products = this.products.filter(product => product.categoryId === category.id);
        });

        this.departments = this.departments.filter(department =>
          this.categories.some(category => category.departmentId === department.id)
        );

        this.loadingService.hideLoading();
      },
      (error) => {
        console.error('Erro ao carregar os dados:', error);
        this.loadingService.hideLoading();
      }
    );
  }


  loadProducts(): void {

    this.productService.getProducts().subscribe(
      (result: Product[]) => {
        this.products = result;

        this.loadCategories();
        this.loadingService.hideLoading();

      },
      (error) => {
        console.error('Erro ao carregar produtos:', error);
      }
    );
  }

  loadCategories(): void {

    this.categoryService.getCategories().subscribe(
      (result: Category[]) => {
        this.categories = result;

        this.departments = this.departments.filter(department =>
          this.categories.some(category => category.departmentId === department.id)
        );

        this.products = this.products.filter(product =>
          this.categories.some(category => category.id === product.categoryId)
        );

        this.loadingService.hideLoading(); 
      },
      (error) => {
        console.error('Erro ao carregar categorias:', error);
        this.loadingService.hideLoading(); 
      }
    );
  }

  openDeleteDialog(category: Category): void {
    const dialogRef = this.dialog.open(DeleteCategoryComponent, {
      data: { category },
      width: '550px',
      height: '350px', 

    });

    dialogRef.afterClosed().subscribe(result => {
      if (result && result.deleted) {
        this.categories = this.categories.filter(p => p.id !== category.id).slice();
        this.loadCategories();
        this.loadProducts();
      }
    });
  }
}
