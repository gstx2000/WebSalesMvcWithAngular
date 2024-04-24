import { HttpClient, HttpEvent, HttpHeaders, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, catchError, map, switchMap, throwError } from 'rxjs';
import { environment } from '../../environments/environment';
import { AuthService } from './AuthService';
import { SalesRecord } from '../Models/SalesRecord';
import { PagedResult } from '../Models/PagedResult';
import { SalesRecordDTO } from '../DTOs/SalesRecordDTO';

@Injectable({
  providedIn: 'root',
})
export class SalesRecordService {
  private url = 'SalesRecords';
  applicationUrl = 'https://localhost:7135/api';
  constructor(private http: HttpClient, private auth: AuthService) { }

  getSalesRecords(): Observable<SalesRecord[]> {
    return this.http.get<SalesRecord[]>(`${environment.apiUrl}/${this.url}/get-salesrecords`).pipe(
      catchError((error: any) => {
        console.error('Erro HTTP:', error);
        return throwError(error);
      })
    );
  }

  getSalesRecordsPaginated(page: number = 1, pageSize: number = 10): Observable<PagedResult<SalesRecordDTO>> {
    let params = new HttpParams()
      .set('page', page.toString())
      .set('pageSize', pageSize.toString());
    return this.http.get<PagedResult<SalesRecordDTO>>(`${environment.apiUrl}/${this.url}/get-salesrecords-paginated`, { params })
      .pipe(
        catchError((error: any) => {
          console.error('Erro HTTP:', error);
          return throwError(error);
        })
      );
  }

  getSalesRecordsToInvoice(page: number = 1, pageSize: number = 10): Observable<PagedResult<SalesRecordDTO>> {
    let params = new HttpParams()
      .set('page', page.toString())
      .set('pageSize', pageSize.toString());
    return this.http.get<PagedResult<SalesRecordDTO>>(`${environment.apiUrl}/${this.url}/get-salesrecords-to-invoice`, { params })
      .pipe(
        catchError((error: any) => {
          console.error('Erro HTTP:', error);
          return throwError(error);
        })
      );
  }

  async getSalesRecordById(id: number): Promise<Observable<SalesRecord>> {
    return this.http.get<SalesRecord>(`${environment.apiUrl}/${this.url}/get-salesrecord/${id}`)
      .pipe(
        catchError((error: any) => {
          console.error('Erro HTTP:', error);
          return throwError(error);
        })
      );
  }

  async createSalesRecordAsync(salesRecord: SalesRecord): Promise<Observable<HttpEvent<SalesRecord>>> {
    try {
      const options = await this.auth.getOptions();

      return this.http.post<SalesRecord>(`${environment.apiUrl}/${this.url}/post-salesrecord`, salesRecord, options)
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

  async updateSalesRecord(id: number, salesRecord: SalesRecord): Promise<Observable<HttpEvent<SalesRecord>>> {
    try {
      const options = await this.auth.getOptions();

      return this.http.put<SalesRecord>(`${environment.apiUrl}/${this.url}/edit-salesrecord/${id}`, salesRecord, options)
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

  async deleteSalesRecord(id: number): Promise<Observable<HttpEvent<SalesRecord>>> {
    try {

      const options = await this.auth.getOptions();

      return this.http.delete<SalesRecord>(`${environment.apiUrl}/${this.url}/delete-salesrecord/${id}`, options)
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
