import { HttpErrorResponse, HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { Observable , catchError, throwError} from 'rxjs';
import { LoadingService } from '../../Services/LoadingService';

@Injectable({
  providedIn: 'root'
})
export class HttpInterceptorService implements HttpInterceptor {

  constructor(private router: Router, private toastr: ToastrService, private loadingService: LoadingService) { }
  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    const JWTtoken = localStorage.getItem('JWTtoken');
    if (JWTtoken) {
      req = req.clone({ headers: req.headers.set('Authorization', `Bearer ${JWTtoken}`) });
    }
    return next.handle(req).pipe(
      catchError((error: HttpErrorResponse) => {
        if (error.status && error.status === 401) {
          this.toastr.error('Sua sessão expirou, faça login novamente.');
          this.router.navigate(['/login']);
          this.loadingService.hideLoading();
        }
        return throwError(error);
      })
    );
  }
}
