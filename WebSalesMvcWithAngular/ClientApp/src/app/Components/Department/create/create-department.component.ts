import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Department } from '../../../Models/Department';
import { DepartmentService } from '../../../Services/DepartmentService';
import { Router } from '@angular/router';
import { LoadingService } from '../../../Services/LoadingService';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-departments/create',
  templateUrl: './create-department.component.html',
  styleUrls: ['./create-department.component.css']
})
export class CreateDepartmentComponent implements OnInit {
  departments: Department[] = [];
  departmentForm!: FormGroup;
  department: Department;
  constructor(
    private departmentService: DepartmentService,
    private fb: FormBuilder,
    private router: Router,
    private loadingService: LoadingService,
    private toastr: ToastrService
  ) {
    this.department = {
      name: '',
      sellers: [],
      address: '',
      cnpj: ''
    };
  }

  ngOnInit(): void {
    this.initDepartmentForm();
  }

  initDepartmentForm(): void {
    this.departmentForm = this.fb.group({
      name: ['', Validators.required],
      numberOfSellers: [0, Validators.min(0)],
      address: '',
      cnpj: ''
    });
  }

  async onSubmit(): Promise<void> {
    this.loadingService.showLoading();

    try {
      if (this.departmentForm.valid) {
        const formData: Department = this.departmentForm.value;
        const createdDepartment = await (await this.departmentService.createDepartment(formData)).toPromise();
        this.toastr.success(`Loja ${formData.name} criada com sucesso.`);
        this.router.navigate(['/departments']);
      }
    } catch (error: any) {
      this.toastr.error(error.message || 'Erro interno da aplicação, tente novamente.');
      console.error('Erro ao criar:', error);
      this.loadingService.hideLoading();
    }
    () => {
      this.loadingService.hideLoading();
    }
  }

  cancel() {
    this.router.navigate(['/departments']);
  }
}
