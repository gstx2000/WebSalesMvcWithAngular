import { Component, Inject, OnInit } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { Product } from '../../../Models/Product';
import { ProductService } from '../../../Services/ProductService';
import { SaleStatus } from '../../../Models/enums/SaleStatus';
import { PaymentMethod } from '../../../Models/enums/PaymentMethod';

@Component({
  selector: 'app-delete-product',
  templateUrl: './delete-product.component.html',
  styleUrls: ['./delete-product.component.css']
})
export class DeleteProductComponent implements OnInit {
  product?: Product;
  constructor(
    public dialogRef: MatDialogRef<DeleteProductComponent>,
    @Inject(MAT_DIALOG_DATA) public data: { product: Product },
    private productService: ProductService,

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
        this.dialogRef.close({ deleted: true });
      }
    } catch (error) {
      console.error('Erro ao deletar produto:', error);
    }
  }

  cancel() {
    this.dialogRef.close({ deleted: false });
  }


}
