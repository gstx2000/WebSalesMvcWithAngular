import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { CepModel } from '../../Models/CepModel';

@Injectable({
  providedIn: 'root'
})

//This service consumes a extern API 
export class ViaCepService {
  urlcep!: string;
  private searchResults: CepModel[] = [];

  constructor(
    private http: HttpClient
  ) { }

  fetchCep(cep: string): Observable<CepModel> {
    this.urlcep = `https://viacep.com.br/ws/${cep}/json/`;
    return this.http.get(this.urlcep);
  }

}
