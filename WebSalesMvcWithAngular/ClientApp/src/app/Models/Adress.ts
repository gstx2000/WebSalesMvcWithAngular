export interface Adress {
  adressId: number;
  cep?: string;
  logradouro?: string;
  complemento?: string;
  bairro?: string;
  localidade?: string;
  uf?: string;
  ddd?: string;
  numero: number;
  supplierId: number;
}
