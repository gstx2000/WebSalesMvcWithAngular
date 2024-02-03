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
    this.loadCategories();
  }

  loadDepartments(): void {
    this.loadingService.showLoading();

    this.departmentService.getDepartments().subscribe(
      (result: Department[]) => {
        this.departments = result;

        this.loadCategories();
        this.loadingService.hideLoading();

      },
      (error) => {
        console.error('Erro ao carregar departamentos:', error);
        this.loadingService.hideLoading();
      }
    );
  }

  loadProducts(): void {
    this.loadingService.showLoading();

    this.productService.getProducts().subscribe(
      (result: Product[]) => {
        this.products = result;

        this.loadCategories();
        this.loadingService.hideLoading();

      },
      (error) => {
        console.error('Erro ao carregar departamentos:', error);
        this.loadingService.hideLoading();
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
      width: '350px',

    });

    dialogRef.afterClosed().subscribe(result => {
      if (result && result.deleted) {
        this.categories = this.categories.filter(p => p.id !== category.id);
        this.loadCategories();
      }
    });
  }
}
