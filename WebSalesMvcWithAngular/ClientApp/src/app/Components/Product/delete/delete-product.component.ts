import { Component, Inject, OnInit } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { Product } from '../../../Models/Product';
import { ProductService } from '../../../Services/ProductService';
import { ToastrService } from 'ngx-toastr';
import { ProductDTO } from '../../../DTOs/ProductDTO';

@Component({
  selector: 'app-products/delete-product',
  templateUrl: './delete-product.component.html',
  styleUrls: ['./delete-product.component.css']
})
export class DeleteProductComponent implements OnInit {
  product?: ProductDTO;
  constructor(
    public dialogRef: MatDialogRef<DeleteProductComponent>,
    @Inject(MAT_DIALOG_DATA) public data: { product: ProductDTO },
    private productService: ProductService,
    private toastr: ToastrService
    ) { }

  ngOnInit(): void {
    if (this.data.product && this.data.product.id !== undefined) {
      this.product = this.data.product;
    }
  }

  async onDeleteClick(): Promise<void> {

    try {
      if (this.product && this.product.id) {
        await (await this.productService.deleteProduct(this.product.id)).toPromise();
        this.toastr.success(`Produto ${this.product.name} deletado com sucesso.`);
        this.dialogRef.close({ deleted: true });
      }
    } catch (error: any) {
      this.toastr.error(error.message || 'Erro interno da aplicação, tente novamente.');
      console.error('Erro ao deletar produto:', error);
    }
  }

  cancel() {
    this.dialogRef.close({ deleted: false });
  }


}
