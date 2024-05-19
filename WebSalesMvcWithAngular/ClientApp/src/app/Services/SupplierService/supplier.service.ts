import { HttpClient, HttpEvent, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { AuthService } from '../AuthService';
import { Observable, catchError, throwError } from 'rxjs';
import { Supplier } from '../../Models/Supplier';
import { environment } from '../../../environments/environment';
import { IndexSupplierResponse } from '../../Components/Supplier/Responses/IndexSupplierResponse';
import { PagedResult } from '../../Models/PagedResult';

@Injectable({
  providedIn: 'root'
})
export class SupplierService {
  private url = 'Suppliers';
  applicationUrl = 'https://localhost:7135/api';
  constructor(private http: HttpClient, private auth: AuthService) { }

  getSuppliers(): Observable<Supplier[]> {
    return this.http.get<Supplier[]>(`${environment.apiUrl}/${this.url}/get-suppliers`);
  }

  getSuppliersPaginated(page: number = 1, pageSize: number = 10): Observable<PagedResult<IndexSupplierResponse>> {
    let params = new HttpParams()
      .set('page', page.toString())
      .set('pageSize', pageSize.toString());
    return this.http.get<PagedResult<IndexSupplierResponse>>(`${environment.apiUrl}/${this.url}/get-suppliers-paginated`, { params })
      .pipe(
        catchError((error: any) => {
          console.error('Erro HTTP:', error);
          return throwError(error);
        }),
      );
  }

  async getSupplierById(id: number): Promise<Observable<Supplier>> {
    return this.http.get<Supplier>(`${environment.apiUrl}/${this.url}/get-supplier/${id}`)
      .pipe(
        catchError((error: any) => {
          console.error('Erro HTTP:', error);
          return throwError(error);
        })
      );
  }

  async createSupplier(supplier: Supplier): Promise<Observable<HttpEvent<Supplier>>> {
    try {
      const options = await this.auth.getOptions();
      return this.http.post<Supplier>(`${environment.apiUrl}/${this.url}/post-supplier`, supplier, options)
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

  async updateSupplier(id: number, supplier: Supplier): Promise<Observable<HttpEvent<Supplier>>> {
    try {
      const options = await this.auth.getOptions();

      return this.http.put<Supplier>(`${environment.apiUrl}/${this.url}/edit-supplier/${id}`, supplier, options)
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

  async deleteSupplier(id: number): Promise<Observable<HttpEvent<Supplier>>> {
    try {

      const options = await this.auth.getOptions();

      return this.http.delete<Supplier>(`${environment.apiUrl}/${this.url}/delete-supplier/${id}`, options)
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
