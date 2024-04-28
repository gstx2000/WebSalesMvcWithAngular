import { Component, OnInit } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { Router } from '@angular/router';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { HttpErrorResponse } from '@angular/common/http';
import { LoginService } from '../../../Services/LoginService/login.service';
import { LoadingService } from '../../../Services/LoadingService';

@Component({
  selector: 'app-password-recovery',
  templateUrl: './PasswordRecovery.component.html',
  styleUrls: ['./PasswordRecovery.component.css']
})
export class PasswordRecoveryComponent {
  constructor(
    private fb: FormBuilder,
    private router: Router,
    private loadingService: LoadingService,
    private toastr: ToastrService,
    private loginService: LoginService
  ) { }

  recoveryForm!: FormGroup;
  email: string | undefined;
  ngOnInit(): void {
    this.initRecoveryForm();
  }

  initRecoveryForm(): void {
    this.recoveryForm = this.fb.group({
      email: ['', Validators.required]
    });
  }

  async onSubmit(): Promise<void> {
    this.loadingService.showLoading();

    try {
      if (this.recoveryForm.valid) {
        const formData = this.recoveryForm.value;
        const loggedUser = await (await this.loginService.passwordRecovery(formData.email)).toPromise();
        this.toastr.success(`Seu link de recuperação foi enviado para ${formData.email}`);
        this.router.navigate(['/login']);
        this.loadingService.hideLoading();
      }
    } catch (error: any) {
      if (error instanceof HttpErrorResponse) {
        const errorBody = error.error;
        if (errorBody && errorBody.errors) {
          errorBody.errors.forEach((errorMessage: string | undefined) => {
            this.toastr.error(errorMessage);
          });
        } else {
          this.toastr.error('Ocorreu um erro, tente novamente.');
        }
      } else {
        this.toastr.error('Ocorreu um erro, tente novamente.');
      }
      console.error('Erro ao logar:', error);
      this.loadingService.hideLoading();
    }
    () => {
      this.loadingService.hideLoading();
    }
  }
}
