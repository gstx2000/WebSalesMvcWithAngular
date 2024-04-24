import { Component, OnInit } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { Router } from '@angular/router';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { LoadingService } from '../../Services/LoadingService';
import { User } from '../../Models/User';
import { LoginService } from '../../Services/LoginService/login.service';
import { HttpErrorResponse } from '@angular/common/http';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  constructor(
    private fb: FormBuilder,
    private router: Router,
    private loadingService: LoadingService,
    private toastr: ToastrService,
    private loginService: LoginService
  ) { }

  loginForm!: FormGroup;

  ngOnInit(): void {
    this.initLoginForm();
  }

  initLoginForm(): void {
    this.loginForm = this.fb.group({
      email: ['', Validators.required],
      password: ['', Validators.required],
      rememberMe: [false]
    });
  }

  async onSubmit(): Promise<void> {
    this.loadingService.showLoading();

    try {
      if (this.loginForm.valid) {
        const formData: User = this.loginForm.value;
        const loggedUser = await (await this.loginService.login(formData)).toPromise();
        this.toastr.success(`Bem vindo ${formData.email}`);
        this.router.navigate(['/home']);
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
