import { HttpClient, HttpEvent, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, catchError, map, switchMap, throwError } from 'rxjs';
import { environment } from '../../environments/environment';
import { AuthService } from './AuthService';
import { Category } from '../Models/Category';

@Injectable({
  providedIn: 'root',
})
export class CategoryService {
  private url = 'Categories';
  applicationUrl = 'https://localhost:7135/api';
  constructor(private http: HttpClient, private auth: AuthService) { }

  getCategories(): Observable<Category[]> {
    return this.http.get<Category[]>(`${environment.apiUrl}/${this.url}/get-categories`);
  }

  async getCategoryById(id: number): Promise<Observable<Category>> {
    return this.http.get<Category>(`${environment.apiUrl}/${this.url}/get-category/${id}`)
      .pipe(
        catchError((error: any) => {
          console.error('Erro HTTP:', error);
          return throwError(error);
        })
      );
  }

  async createCategory(category: Category): Promise<Observable<HttpEvent<Category>>> {
    try {

      const options = await this.auth.getOptions();
    
      return this.http.post<Category>(`${environment.apiUrl}/${this.url}/post-category`, category, options)
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

  async updateCategory(id: number, category: Category): Promise<Observable<HttpEvent<Category>>> {
    try {
      const options = await this.auth.getOptions();

      return this.http.put<Category>(`${environment.apiUrl}/${this.url}/edit-category/${id}`, category, options)
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

  async deleteCategory(id: number): Promise<Observable<HttpEvent<Category>>> {
    try {

      const options = await this.auth.getOptions();

      return this.http.delete<Category>(`${environment.apiUrl}/${this.url}/confirm-delete/${id}`, options)
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
