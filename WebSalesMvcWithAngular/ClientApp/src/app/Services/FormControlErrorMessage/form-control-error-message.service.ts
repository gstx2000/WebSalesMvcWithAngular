import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
//This service is for providing the feedback when validating a form, when there are form control errors,
// the adequate message is displayed from her, across the application.
export class FormControlErrorMessageService {
  constructor() { }

  getErrorMessage(errors: any): string {
    let errorMessage = '';
    if (errors.required) {
      errorMessage = 'Este campo é obrigatório.';
    } else if (errors.email) {
      errorMessage = 'Por favor, insira um endereço de e-mail válido.';
    } else if (errors.minlength) {
      errorMessage = `O campo deve ter no mínimo ${errors.minlength.requiredLength} caracteres.`;
    } else if (errors.maxlength) {
      errorMessage = `O campo deve ter no máximo ${errors.maxlength.requiredLength} caracteres.`;
    } else if (errors.pattern) {
      errorMessage = 'Padrão inválido.';
    } else if (errors.equalto) {
      errorMessage = `Os campos estão diferentes.`;
    } else if (errors.min) {
      errorMessage = `O campo não possui a quantidade mínima de ${errors.min.min}`;
    } else if (errors.max) {
      errorMessage = `O campo não deve ultrapassar a quantidade máxima de ${errors.max.max}`;
    } else {
      errorMessage = 'Ocorreu um erro inesperado ao validar o campo.';
    }

    return errorMessage;
  }
}
