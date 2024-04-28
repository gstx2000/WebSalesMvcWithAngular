import { Component, OnInit } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { Router } from '@angular/router';
import { AbstractControl, AsyncValidatorFn, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { HttpErrorResponse } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { LoginService } from '../../../Services/LoginService/login.service';
import { LoadingService } from '../../../Services/LoadingService';
import { User } from '../../../Models/User';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  constructor(
    private fb: FormBuilder,
    private router: Router,
    private loadingService: LoadingService,
    private toastr: ToastrService,
    private loginService: LoginService
  ) { }

  registerForm!: FormGroup;

  ngOnInit(): void {
    this.initRegisterForm();
  }

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

  initRegisterForm(): void {
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
