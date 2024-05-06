import { Component } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { Router } from '@angular/router';
import { AbstractControl, AsyncValidatorFn, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { HttpErrorResponse } from '@angular/common/http';
import { Observable } from 'rxjs';
import { LoginService } from '../../../Services/LoginService/login.service';
import { LoadingService } from '../../../Services/LoadingService';
import { User } from '../../../Models/User';
import { FormControlErrorMessageService } from '../../../Services/FormControlErrorMessage/form-control-error-message.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent {
  constructor(
    private fb: FormBuilder,
    private router: Router,
    private loadingService: LoadingService,
    private toastr: ToastrService,
    private loginService: LoginService,
    private formMessage: FormControlErrorMessageService,

  ) {
    this.registerForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', [
        Validators.required,
        Validators.minLength(6),
        Validators.maxLength(10),
        Validators.pattern(/^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]+$/)
      ]],
      confirmPassword: ['', Validators.required]
    }, { asyncValidators: this.passwordMatchValidator() });
  }

  registerForm!: FormGroup;
  private fieldLabels: { [key: string]: string } = {
    email: 'E-mail',
    password: 'Senha',
    confirmPassword: 'Confirmação de senha'
  };

  passwordMatchValidator(): AsyncValidatorFn {
    return (control: AbstractControl): Observable<{ [key: string]: boolean } | null> => {
      return new Observable((observer) => {
        const password = control.get('password');
        const confirmPassword = control.get('confirmPassword');
        if (password!.pristine || confirmPassword!.pristine) {
          observer.next(null);
          observer.complete();
          return;
        }
        if (password && confirmPassword && password.value !== confirmPassword.value) {
          observer.next({ 'passwordMismatch': true });
        } else {
          observer.next(null);
        }
        observer.complete();
      });
    };
  }

  async onSubmit(): Promise<void> {
    this.loadingService.showLoading();
    try {
      if (this.registerForm.errors && this.registerForm.errors.passwordMismatch) {
        this.toastr.error('Senhas diferentes');
        return;
      }
      if (this.registerForm.valid) {
        const formData: User = this.registerForm.value;
        const registeredUser = await (await this.loginService.register(formData)).toPromise();
        this.toastr.success(`Bem vindo ${formData.email}`);
        this.router.navigate(['/home']);
      } else {
        this.loadingService.hideLoading();
        Object.keys(this.registerForm.controls).forEach(field => {
          const control = this.registerForm.get(field);
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
    } finally {
      this.loadingService.hideLoading();
    }
  }
}
