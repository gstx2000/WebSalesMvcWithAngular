import { Component, OnInit } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { Router } from '@angular/router';
import { AbstractControl, AsyncValidatorFn, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { HttpErrorResponse } from '@angular/common/http';
import { LoadingService } from '../../../../Services/LoadingService';
import { LoginService } from '../../../../Services/LoginService/login.service';
import { Observable } from 'rxjs';
import { User } from '../../../../Models/User';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-redefine-password',
  templateUrl: './redefine-password.component.html',
  styleUrls: ['./redefine-password.component.css']
})
export class RedefinePasswordComponent {
  constructor(
    private fb: FormBuilder,
    private router: Router,
    private loadingService: LoadingService,
    private toastr: ToastrService,
    private loginService: LoginService,
    private activeroute: ActivatedRoute
  ) { }

  redefineForm!: FormGroup;
  email!: string;
  token!: string;

  ngOnInit(): void {
    this.activeroute.queryParams.subscribe(params => {
      this.email = params['email'];
      this.token = params['token'];
    });
    this.initRedefineForm();
    this.redefineForm.get('email')!.disable();

  }

  initRedefineForm(): void {
    this.redefineForm = this.fb.group({
      email: [this.email, Validators.required],
      password: ['', [
        Validators.required,
        Validators.minLength(6),
        Validators.maxLength(10),
        Validators.pattern(/^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]+$/)
      ]],
      confirmPassword: ['', Validators.required]
    }, { asyncValidators: this.passwordMatchValidator() });
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

  async onSubmit(): Promise<void> {
    this.loadingService.showLoading();
    try {
      if (this.redefineForm.errors && this.redefineForm.errors.passwordMismatch) {
        this.toastr.error('Senhas diferentes');
        return;
      }
      if (this.redefineForm.valid) {
        const formData: User = this.redefineForm.value;
        formData.email = this.email;
        formData.recoveryToken = this.token;
        const registeredUser = await (await this.loginService.redefinePassword(formData)).toPromise();
        this.toastr.success(`${formData.email} sua senha foi redefinida, faça login com a nova senha.`);
        this.router.navigate(['/login']);
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

