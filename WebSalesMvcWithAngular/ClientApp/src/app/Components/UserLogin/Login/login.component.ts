import { Component } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { Router } from '@angular/router';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { HttpErrorResponse } from '@angular/common/http';
import { LoginService } from '../../../Services/LoginService/login.service';
import { LoadingService } from '../../../Services/LoadingService';
import { User } from '../../../Models/User';
import { FormControlErrorMessageService } from '../../../Services/FormControlErrorMessage/form-control-error-message.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent {
  constructor(
    private fb: FormBuilder,
    private router: Router,
    private loadingService: LoadingService,
    private toastr: ToastrService,
    private loginService: LoginService,
    private formMessage: FormControlErrorMessageService,
  ) {
    this.loginForm = this.fb.group({
      email: ['', Validators.required],
      password: ['', Validators.required],
      rememberMe: [false]
    });
  }

  loginForm!: FormGroup;
  private fieldLabels: { [key: string]: string } = {
    email: 'E-mail',
    password: 'Senha'
  };

  async onSubmit(): Promise<void> {
    this.loadingService.showLoading();

    try {
      if (this.loginForm.valid) {
        const formData: User = this.loginForm.value;
        const loggedUser = await (await this.loginService.login(formData)).toPromise();
        this.toastr.success(`Bem vindo ${formData.email}`);
        this.router.navigate(['/home']);
        this.loadingService.hideLoading();
      } else {
        this.loadingService.hideLoading();
        Object.keys(this.loginForm.controls).forEach(field => {
          const control = this.loginForm.get(field);
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
