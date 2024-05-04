import { HttpClient, HttpEvent, HttpParams, HttpResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, catchError, map, shareReplay, throwError } from 'rxjs';
import { environment } from '../../environments/environment';
import { AuthService } from './AuthService';
import { Product } from '../Models/Product';
import { PagedResult } from '../Models/PagedResult';
import { ProductDTO } from '../DTOs/ProductDTO';
import { ToastrService } from 'ngx-toastr';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root',
})
export class ProductService {
  private url = 'Products';
  applicationUrl = 'https://localhost:7135/api';
  constructor(private http: HttpClient, private auth: AuthService,
    private toastr: ToastrService, private router: Router) { }


  getProducts(): Observable<Product[]> {
    return this.http.get<Product[]>(`${environment.apiUrl}/${this.url}/get-products`)
      .pipe(
        catchError((error: any) => {
          console.error('Erro HTTP:', error);
          return throwError(error);
        })
      );
  }

  getProductsPaginated(page: number = 1, pageSize: number = 10): Observable<PagedResult<ProductDTO>> {
    let params = new HttpParams()
      .set('page', page.toString())
      .set('pageSize', pageSize.toString());
    return this.http.get<PagedResult<ProductDTO>>(`${environment.apiUrl}/${this.url}/get-products-paginated`, { params })
      .pipe(
        catchError((error: any) => {
          console.error('Erro HTTP:', error);
          return throwError(error);
        }),
        shareReplay(1) 
      );
  }
  async getProductById(id: number): Promise<Observable<Product>> {
    return this.http.get<Product>(`${environment.apiUrl}/${this.url}/get-product/${id}`)
      .pipe(
        catchError((error: any) => {
          console.error('Erro HTTP:', error);
          return throwError(error);
        })
      );
  }

  async createProduct(product: ProductDTO): Promise<Observable<HttpEvent<Product>>> {
    try {

      const options = await this.auth.getOptions();

      return this.http.post<Product>(`${environment.apiUrl}/${this.url}/post-product`, product, options)
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

  async updateProduct(id: number, product: ProductDTO): Promise<Observable<HttpEvent<ProductDTO>>> {
    try {
      const options = await this.auth.getOptions();

      return this.http.put<ProductDTO>(`${environment.apiUrl}/${this.url}/edit-product/${id}`, product, options)
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

  async editInventory(id: number, product: ProductDTO): Promise<Observable<HttpEvent<ProductDTO>>> {
    try {
      const options = await this.auth.getOptions();

      return this.http.put<ProductDTO>(`${environment.apiUrl}/${this.url}/edit-inventory/${id}`, product, options)
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

  async deleteProduct(id: number): Promise<Observable<HttpEvent<ProductDTO>>> {
    try {

      const options = await this.auth.getOptions();

      return this.http.delete<ProductDTO>(`${environment.apiUrl}/${this.url}/delete-product/${id}`, options)
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

  searchProductsByNameDTO(searchTerm: string, categoryId?: number | null): Observable<ProductDTO[]> {
    const url = `${environment.apiUrl}/${this.url}/get-product-by-name/${searchTerm}${categoryId !== null && categoryId !== undefined ? `/${categoryId}` : ''}`;

    return this.http.get<ProductDTO[]>(url)
      .pipe(
        catchError((error: any) => {
          console.error('HTTP Error:', error);
          return throwError(error);
        })
      );
  }

  searchProductsByName(searchTerm: string, categoryId?: number | null): Observable<Product[]> {
    const url = `${environment.apiUrl}/${this.url}/get-full-product-by-name/${searchTerm}${categoryId !== null && categoryId !== undefined ? `/${categoryId}` : ''}`;

    return this.http.get<Product[]>(url)
      .pipe(
        catchError((error: any) => {
          console.error('HTTP Error:', error);
          return throwError(error);
        })
      );
  }
}
