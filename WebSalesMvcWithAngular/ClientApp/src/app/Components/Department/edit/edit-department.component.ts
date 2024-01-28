import { Component, OnInit } from '@angular/core';
import { DepartmentService } from '../../../Services/DepartmentService';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Department } from '../../../Models/Department';
import { ActivatedRoute } from '@angular/router';
import { Router } from '@angular/router';
import { LoadingService } from '../../../Services/LoadingService';
import { ErrorStateMatcher } from '@angular/material/core';


@Component({
  selector: 'app-departments/edit',
  templateUrl: './edit-department.component.html',
  styleUrls: ['./edit-department.component.css']
})
export class EditDepartmentComponent implements OnInit {
  departments: Department[] = [];
  departmentForm!: FormGroup;
  matcher = new MyErrorStateMatcher();

  constructor(
    private departmentService: DepartmentService,
    private fb: FormBuilder,
    private activedroute: ActivatedRoute,
    private router: Router,
    private loadingService: LoadingService
  ) { }

  ngOnInit(): void {
    this.initializeForm();
    const departmentId = Number(this.activedroute.snapshot.params['id']);
    if (departmentId) {
      this.fetchDepartment(departmentId);

    }
  }

  initializeForm(): void {
    this.departmentForm = this.fb.group({
      id: '',
      name: ['', Validators.required],
      numberOfSellers: [0, Validators.min(0)],
      address: '',
      cnpj: '',
    });
  }

  async fetchDepartment(id: number): Promise<void> {
    this.loadingService.showLoading();
    try {
      const fetchedDepartment = await (await this.departmentService.getDepartmentById(id)).toPromise();
      if (fetchedDepartment)
        this.departmentForm.patchValue({
          id: fetchedDepartment.id,
          name: fetchedDepartment.name,
          numberOfSellers: fetchedDepartment.sellers.length,
          address: fetchedDepartment.address,
          cnpj: fetchedDepartment.cnpj

        });
      this.departmentForm.get('id')!.disable();
      this.loadingService.hideLoading();

    } catch (error) {
      console.error('Erro ao buscar departamento', error);
      this.loadingService.hideLoading();
    }
    this.loadingService.hideLoading();

  }

  async onSubmit(): Promise<void> {
    this.loadingService.showLoading();
    try {
      if (this.departmentForm.valid) {
        const formData: Department = this.departmentForm.getRawValue();
        if (formData.id) {
          const departmentId = await (await this.departmentService.getDepartmentById(formData.id)).toPromise(); 
          if (departmentId) {
            if (departmentId.id) {
              const updatedDepartment = await (await this.departmentService.updateDepartment(departmentId.id, formData)).toPromise();
              this.loadingService.hideLoading();

            this.router.navigate(['/departments']);

            }
          }
        }
      }
    } catch (error) {
      console.error('Erro ao atualizar departamento:', error);
      this.loadingService.hideLoading();
    }
  }

  cancel() {
    this.router.navigate(['/departments']);
  }
}
export class MyErrorStateMatcher implements ErrorStateMatcher {
  isErrorState(control: any, form: any): boolean {
    const isSubmitted = form && form.submitted;

    if (control && control.invalid) {
      if (control.hasError('required')) {
        return isSubmitted || (control.dirty || control.touched);
      }

      // Default behavior for other errors
      return control.dirty || control.touched || isSubmitted;
    }
    return false;
  }
}

