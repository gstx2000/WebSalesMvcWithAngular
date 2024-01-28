import { Component, OnInit } from '@angular/core';
import { Department } from '../../../Models/Department';
import { DepartmentService } from '../../../Services/DepartmentService';
import { Category } from '../../../Models/Category';
import { CategoryService } from '../../../Services/CategoryService';
import { LoadingService } from '../../../Services/LoadingService';

@Component({
  selector: 'app-categories',
  templateUrl: './index-category.component.html',
  styleUrls: ['./index-category.component.css']
})
export class IndexCategoryComponent implements OnInit {
  departments: Department[] = [];
  categories: Category[] = [];

  constructor(
    private departmentService: DepartmentService,
    private categoryService: CategoryService,
    private loadingService: LoadingService
  ) { }

  ngOnInit(): void {
    this.loadDepartments();
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

  loadCategories(): void {
    this.categoryService.getCategories().subscribe(
      (result: Category[]) => {
        this.categories = result;

        this.departments = this.departments.filter(department =>
          this.categories.some(category => category.departmentId === department.id)
        );

        this.loadingService.hideLoading(); 
      },
      (error) => {
        console.error('Erro ao carregar categorias:', error);
        this.loadingService.hideLoading(); 
      }
    );
  }
}
