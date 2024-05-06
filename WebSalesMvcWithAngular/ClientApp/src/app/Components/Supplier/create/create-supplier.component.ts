import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { LoadingService } from '../../../Services/LoadingService';
import { ToastrService } from 'ngx-toastr';
import { SupplierType } from '../../../Models/enums/SupplierType';
import { Supplier } from '../../../Models/Supplier';
import { SupplierService } from '../../../Services/SupplierService/supplier.service';
import { FormControlErrorMessageService } from '../../../Services/FormControlErrorMessage/form-control-error-message.service';

@Component({
  selector: 'app-create-supplier',
  templateUrl: './create-supplier.component.html',
  styleUrls: ['./create-supplier.component.css']
})
export class CreateSupplierComponent {
  supplierForm!: FormGroup;
  SupplierType!: SupplierType;
  supplier!: Supplier;

  private fieldLabels: { [key: string]: string } = {
    name: 'Nome',
    phone: 'Telefone',
    CNPJ: 'CNPJ',
    dayToPay: 'Dia para Pagar',
    contactPerson: 'Pessoa de Contato',
    supplierType: 'Tipo de Fornecedor',
    website: 'Website',
    shippingValue: 'Valor de Envio',
    adresses: 'Endereços', 
  };

  constructor(
    private fb: FormBuilder,
    private router: Router,
    private loadingService: LoadingService,
    private toastr: ToastrService,
    private supplierService: SupplierService,
    private formMessage: FormControlErrorMessageService
  ) {
    this.supplierForm = this.fb.group({
      name: ['', [Validators.required, Validators.minLength(2)]],
      email: ['', Validators.email],
      phone: ['', [Validators.maxLength(11), Validators.minLength(10)]],
      CNPJ: ['', [ Validators.maxLength(18), Validators.minLength(14)]],
      dayToPay: [],
      contactPerson: [''],
      supplierType: [0],
      website: [''],
      shippingValue: [0],
      adresses: [[]],
    });
  }

  async onSubmit(): Promise<void> {
    this.loadingService.showLoading();
    try {
      console.log(this.supplierForm.value)
      if (this.supplierForm.valid) {
        const formData: Supplier = this.supplierForm.value;
        const createdSupplier = await (await this.supplierService.createSupplier(formData)).toPromise();
        this.toastr.success(`Fornecedor ${formData.name} criado com sucesso.`);
        this.loadingService.hideLoading();
        this.router.navigate(['/suppliers']);
      } else {
        this.loadingService.hideLoading();
        Object.keys(this.supplierForm.controls).forEach(field => {
          const control = this.supplierForm.get(field);
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
    }
    () => {
      this.loadingService.hideLoading();
    }
  }

  getSupplierTypeValues(): number[] {
    return Object.values(SupplierType).filter(value => typeof value === 'number') as number[];
  }

  getSupplierTypeName(value: number): string {
    return SupplierType[value] as string;
  }
}
