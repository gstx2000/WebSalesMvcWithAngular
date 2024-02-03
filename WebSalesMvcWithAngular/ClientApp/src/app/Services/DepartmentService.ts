import { HttpClient, HttpEvent, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, catchError, map, switchMap, throwError } from 'rxjs';
import { Department } from '../Models/Department';
import { environment } from '../../environments/environment';
import { AuthService } from './AuthService';

@Injectable({
  providedIn: 'root',
})
export class DepartmentService {
  private url = 'Departments';
  applicationUrl = 'https://localhost:7135/api';
  constructor(private http: HttpClient, private auth: AuthService) { }

  getDepartments(): Observable<Department[]> {
    return this.http.get<Department[]>(`${environment.apiUrl}/${this.url}/get-departments`);
  }

  async getDepartmentById(id: number): Promise<Observable<Department>> {
    return this.http.get<Department>(`${environment.apiUrl}/${this.url}/get-department/${id}`)
      .pipe(
        catchError((error: any) => {
          console.error('Erro HTTP:', error);
          return throwError(error);
        })
      );
  }

  async createDepartment(department: Department): Promise<Observable<HttpEvent<Department>>> {
    try {

      const options = await this.auth.getOptions();

      return this.http.post<Department>(`${environment.apiUrl}/${this.url}/post-department`, department, options)
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

  async updateDepartment(id: number, department: Department): Promise<Observable<HttpEvent<Department>>> {
    try {
      const options = await this.auth.getOptions();

      return this.http.put<Department>(`${environment.apiUrl}/${this.url}/edit-department/${id}`, department, options)
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

  async deleteDepartment(id: number): Promise<Observable<HttpEvent<Department>>> {
    try {
    
      const options = await this.auth.getOptions();

      return this.http.delete<Department>(`${environment.apiUrl}/${this.url}/delete-department/${id}`, options)
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
