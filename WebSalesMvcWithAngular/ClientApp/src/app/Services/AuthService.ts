import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private apiUrl = 'https://localhost:7135/api';
  constructor(private http: HttpClient ) { }

  setAntiforgeryCookie(): Promise<{ token: string, cookie: string }> {
    return new Promise((resolve, reject) => {
      this.http.get(`${this.apiUrl}/Tokens/antiforgery-token`, {
        withCredentials: true,
        responseType: 'text'
      }).subscribe(
        (token: string) => {
          if (token) {
            const cookieName = '.AspNetCore.Antiforgery.yQh38VJV-uk';
            const cookieValue = `${cookieName}=${token};`;
            document.cookie = cookieValue;
            resolve({ token, cookie: cookieValue });
          } else {
            reject('Token antiforgery não encontrado.');
          }
        },
        error => {
          console.error('Erro ao validar cookie antiforgery:', error);
          reject(error);
        }
      );
    });
  }

  public async getOptions(): Promise<any> {
    const { token } = await this.setAntiforgeryCookie();
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
      'X-XSRF-TOKEN': token,
    });

    return {
      headers: headers,
      withCredentials: true,
    };
  }

}
