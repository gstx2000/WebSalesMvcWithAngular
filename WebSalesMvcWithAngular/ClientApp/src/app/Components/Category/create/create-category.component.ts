import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Department } from '../../../Models/Department';
import { DepartmentService } from '../../../Services/DepartmentService';
import { Router } from '@angular/router';
import { Category } from '../../../Models/Category';
import { Observable } from 'rxjs';
import { CategoryService } from '../../../Services/CategoryService';
import { LoadingService } from '../../../Services/LoadingService';
import { ToastrService } from 'ngx-toastr';
import { FormControlErrorMessageService } from '../../../Services/FormControlErrorMessage/form-control-error-message.service';

@Component({
  selector: 'app-create-category',
  templateUrl: './create-category.component.html',
  styleUrls: ['./create-category.component.css']
})
export class CreateCategoryComponent implements OnInit {
  categoryForm!: FormGroup;
  departments$: Observable<Department[]> | undefined;

  private fieldLabels: { [key: string]: string } = {
    name: 'Nome',
    description: 'Descrição',
    departmentId: 'ID Departamento'
  };
  constructor(
    private categoryService: CategoryService,
    private departmentService: DepartmentService,
    private fb: FormBuilder,
    private router: Router,
    private loadingService: LoadingService,
    private toastr: ToastrService,
    private formMessage: FormControlErrorMessageService
  ) {
    this.categoryForm = this.fb.group({
      name: ['', Validators.required],
      description: ['',],
      departmentId: [0]
    });
   
  }

  ngOnInit(): void {
    this.departments$ = this.departmentService.getDepartments();
  }

  async onSubmit(): Promise<void> {
    try {
      if (this.categoryForm.valid) {
        const formData: Category = this.categoryForm.value;
        const createdCategory = await (await this.categoryService.createCategory(formData)).toPromise();
        this.toastr.success(`Categoria ${formData.name} criada com sucesso.`);
        this.router.navigate(['/categories']);

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
      console.error('Erro ao criar:', error);
      this.loadingService.hideLoading();
    } finally {
      this.loadingService.hideLoading();
    }
  }
}
