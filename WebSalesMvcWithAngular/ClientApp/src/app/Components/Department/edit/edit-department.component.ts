import { Component, OnInit } from '@angular/core';
import { DepartmentService } from '../../../Services/DepartmentService';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Department } from '../../../Models/Department';
import { ActivatedRoute } from '@angular/router';
import { Router } from '@angular/router';
import { LoadingService } from '../../../Services/LoadingService';
import { ToastrService } from 'ngx-toastr';


@Component({
  selector: 'app-departments/edit',
  templateUrl: './edit-department.component.html',
  styleUrls: ['./edit-department.component.css']
})
export class EditDepartmentComponent implements OnInit {
  departments: Department[] = [];
  departmentForm!: FormGroup;
  constructor(
    private departmentService: DepartmentService,
    private fb: FormBuilder,
    private activedroute: ActivatedRoute,
    private router: Router,
    private loadingService: LoadingService,
    private toastr: ToastrService

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

    } catch (error: any) {
      this.toastr.error(error.message || 'Erro interno da aplicação, tente novamente.');
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
              this.toastr.success(`Loja ${formData.name} alterada com sucesso.`);
              this.router.navigate(['/departments']);
            }
          }
        }
      }
    } catch (error: any) {
      this.toastr.error(error.message || 'Erro interno da aplicação, tente novamente.');
      console.error('Erro ao atualizar departamento:', error);
      this.loadingService.hideLoading();
    }
  }

  cancel() {
    this.router.navigate(['/departments']);
  }
}

