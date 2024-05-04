import { HttpClient, HttpEvent, HttpEventType, HttpResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { AuthService } from '../AuthService';
import { Observable, catchError, tap, throwError } from 'rxjs';
import { environment } from '../../../environments/environment';
import { User } from '../../Models/User';

@Injectable({
  providedIn: 'root'
})
export class LoginService {
  private url = 'Users';
  applicationUrl = 'https://localhost:7135/api';
  constructor(private http: HttpClient, private auth: AuthService) { }

  async login(user: User): Promise<Observable<HttpEvent<User>>> {
    user.username = user.email;
    try {
      const options = await this.auth.getOptions();
      return this.http.post<User>(`${environment.apiUrl}/${this.url}/login`, user, options).pipe(
        tap((response: any) => {
          if (response) {
            const token = response.token; 
            if (token) {
              this.auth.saveToken(token);
            }
          }
        }),
          catchError((error: any) => {
            console.error('Erro HTTP:', error);
            return throwError(error);
          })
        );
    } catch (error) {
      return throwError(error);
    }
  }

  async register(user: User): Promise<Observable<HttpEvent<User>>> {
    try {

      const options = await this.auth.getOptions();
      user.username = user.email;

      return await this.http.post<User>(`${environment.apiUrl}/${this.url}/register`, user, options)
        .pipe(
          catchError((error: any) => {
            console.error('Erro HTTP:', error);
            return throwError(error);
          })
        );
    } catch (error) {
      return throwError(error);
    }
  }
  async passwordRecovery(email: string): Promise<Observable<HttpEvent<string>>> { 
    try {
      const options = await this.auth.getOptions();
      return await this.http.post<string>(`${environment.apiUrl}/${this.url}/password-recovery`, { email: email }, options)
        .pipe(
          catchError((error: any) => {
            console.error('Erro HTTP:', error);
            return throwError(error);
          })
        );
    } catch (error) {
      return throwError(error);
    }
  }

  async redefinePassword(user: User): Promise<Observable<HttpEvent<string>>> {
    try {
      user.username = user.email;
      const options = await this.auth.getOptions();
      return await this.http.post<string>(`${environment.apiUrl}/${this.url}/reset-password`, user, options)
        .pipe(
          catchError((error: any) => {
            console.error('Erro HTTP:', error);
            return throwError(error);
          })
        );
    } catch (error) {
      return throwError(error);
    }
  }
}
