import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Department } from '../../../Models/Department';
import { DepartmentService } from '../../../Services/DepartmentService';
import { Router } from '@angular/router';
import { Category } from '../../../Models/Category';
import { Observable } from 'rxjs';
import { CategoryService } from '../../../Services/CategoryService';

@Component({
  selector: 'app-create-category',
  templateUrl: './create-category.component.html',
  styleUrls: ['./create-category.component.css']
})
export class CreateCategoryComponent implements OnInit {
  categoryForm!: FormGroup;
  departments$: Observable<Department[]> | undefined;
  category: Category;
  constructor(
    private categoryService: CategoryService,
    private departmentService: DepartmentService,
    private fb: FormBuilder,
    private router: Router,

  ) {
    this.category = {
      name: '',
      description: '',
      departmentId: 0
    };
  }

  ngOnInit(): void {
    this.departments$ = this.departmentService.getDepartments();
    this.initCategoryForm();
  }

  initCategoryForm(): void {
    this.categoryForm = this.fb.group({
      name: ['', Validators.required],
      description: ['', ],
      departmentId: [0]
    });
  }

  async onSubmit(): Promise<void> {

    try {
      if (this.categoryForm.valid) {
        const formData: Category = this.categoryForm.value;
        const createdCategory = await (await this.categoryService.createCategory(formData)).toPromise();
        this.router.navigate(['/categories']);
      }
    } catch (error) {
      console.error('Erro ao criar:', error);
    }
  }

  cancel() {
    this.router.navigate(['/categories']);
  }
}
