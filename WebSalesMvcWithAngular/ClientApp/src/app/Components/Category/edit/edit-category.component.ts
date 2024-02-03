import { Component, OnInit } from '@angular/core';
import { DepartmentService } from '../../../Services/DepartmentService';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Department } from '../../../Models/Department';
import { ActivatedRoute } from '@angular/router';
import { Router } from '@angular/router';
import { LoadingService } from '../../../Services/LoadingService';
import { CategoryService } from '../../../Services/CategoryService';
import { Observable } from 'rxjs';
import { Category } from '../../../Models/Category';

@Component({
  selector: 'app-categories/edit',
  templateUrl: './edit-category.component.html',
  styleUrls: ['./edit-category.component.css']
})
export class EditCategoryComponent implements OnInit {
  categoryForm!: FormGroup;
  departments$: Observable<Department[]> | undefined;
  constructor(
    private departmentService: DepartmentService,
    private fb: FormBuilder,
    private activedroute: ActivatedRoute,
    private router: Router,
    private loadingService: LoadingService,
    private categoryService: CategoryService
  ) { }

  ngOnInit(): void {
    this.initializeForm();
    this.departments$ = this.departmentService.getDepartments();
    const categoryId = Number(this.activedroute.snapshot.params['id']);
    if (categoryId) {
      this.fetchCategory(categoryId);

    }
  }

  initializeForm(): void {
    this.categoryForm = this.fb.group({
      id: 0,
      name: ['', Validators.required],
      description: '',
      categoryId: 0,
      departmentId: 0
    });
  }

  async fetchCategory(id: number): Promise<void> {
    this.loadingService.showLoading();
    try {
      const fetchedCategory = await (await this.categoryService.getCategoryById(id)).toPromise();
      if (fetchedCategory)
        this.categoryForm.patchValue({
          id: fetchedCategory.id,
          name: fetchedCategory.name,
          description: fetchedCategory.description,
          departmentId: fetchedCategory.departmentId
        });

      this.categoryForm.get('id')!.disable();
      this.loadingService.hideLoading();

    } catch (error) {
      console.error('Erro ao buscar categoria:', error);
      this.loadingService.hideLoading();
    }
    this.loadingService.hideLoading();

  }

  async onSubmit(): Promise<void> {
    this.loadingService.showLoading();
    try {
      if (this.categoryForm.valid) {
        const formData: Category = this.categoryForm.getRawValue();
        if (formData.id) {
          const categoryId = await (await this.categoryService.getCategoryById(formData.id)).toPromise();
          if (categoryId) {
            if (categoryId.id) {
              const updatedCategory = await (await this.categoryService.updateCategory(categoryId.id, formData)).toPromise();
              this.loadingService.hideLoading();

              this.router.navigate(['/categories']);

            }
          }
        }
      }
    } catch (error) {
      console.error('Erro ao atualizar categoria:', error);
      this.loadingService.hideLoading();
    }
  }

  cancel() {
    this.router.navigate(['/categories']);
  }
}
