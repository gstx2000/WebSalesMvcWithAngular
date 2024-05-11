import { Component } from '@angular/core';
import { DepartmentService } from '../../../Services/DepartmentService';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Department } from '../../../Models/Department';
import { ActivatedRoute } from '@angular/router';
import { Router } from '@angular/router';
import { LoadingService } from '../../../Services/LoadingService';
import { CategoryService } from '../../../Services/CategoryService';
import { Observable } from 'rxjs';
import { Category } from '../../../Models/Category';
import { ToastrService } from 'ngx-toastr';
import { FormControlErrorMessageService } from '../../../Services/FormControlErrorMessage/form-control-error-message.service';

@Component({
  selector: 'app-categories/edit',
  templateUrl: './edit-category.component.html',
  styleUrls: ['./edit-category.component.css']
})
export class EditCategoryComponent  {
  categoryForm!: FormGroup;
  departments$: Observable<Department[]> | undefined;

  private fieldLabels: { [key: string]: string } = {
    name: 'Nome',
    description: 'Descrição',
    departmentId: 'ID Departamento'
  };
  constructor(
    private departmentService: DepartmentService,
    private fb: FormBuilder,
    private activedroute: ActivatedRoute,
    private router: Router,
    private loadingService: LoadingService,
    private categoryService: CategoryService,
    private toastr: ToastrService,
    private formMessage: FormControlErrorMessageService
  ) {
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

    } catch (error: any) {
      this.toastr.error(error.message || 'Erro interno da aplicação, tente novamente.');
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
              this.toastr.success(`Categoria ${formData.name} alterada com sucesso.`);
              this.router.navigate(['/categories']);

            }
          }
        }
      } else {
        this.loadingService.hideLoading();
        Object.keys(this.categoryForm.controls).forEach(field => {
          const control = this.categoryForm.get(field);
          if (control) {
            if (control.invalid && control.touched) {
              const label = this.fieldLabels[field] || field;
              const errorMessage = this.formMessage.getErrorMessage(control.errors);
              this.toastr.error(`Campo ${label} está inválido: ${errorMessage}`);
            }
          }
        });
      }
    } catch (error: any) {
      this.toastr.error(error.message || 'Erro interno da aplicação, tente novamente.');
      console.error('Erro ao atualizar categoria:', error);
      this.loadingService.hideLoading();
    }
  }
}
