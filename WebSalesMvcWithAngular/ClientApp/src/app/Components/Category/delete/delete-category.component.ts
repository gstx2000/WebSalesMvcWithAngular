import { Component, Inject, OnInit } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { LoadingService } from '../../../Services/LoadingService';
import { CategoryService } from '../../../Services/CategoryService';
import { ToastrService } from 'ngx-toastr';
import { CategoryDTO } from '../../../DTOs/CategoryDTO';

@Component({
  selector: 'app-categories/delete-category',
  templateUrl: './delete-category.component.html',
  styleUrls: ['./delete-category.component.css']
})
export class DeleteCategoryComponent implements OnInit {
  category?: CategoryDTO;
  constructor(
    public dialogRef: MatDialogRef<DeleteCategoryComponent>,
    @Inject(MAT_DIALOG_DATA) public data: { category: CategoryDTO },
    private categoryService: CategoryService,
    private loadingService: LoadingService,
    private toastr: ToastrService
  ) { }

  ngOnInit(): void {
    if (this.data.category && this.data.category.id !== undefined) {
      this.category = this.data.category;
    }
  }

  async onDeleteClick(): Promise<void> {
    try {
      this.loadingService.showLoading();
      if (this.category && this.category.id) {
        if (this.category.productCount === 0) {
          await (await this.categoryService.deleteCategory(this.category.id)).toPromise();
          this.toastr.success(`Categoria ${this.category.name} deletada com sucesso.`);
          this.dialogRef.close({ deleted: true });
        } else {
          this.loadingService.hideLoading();
          this.toastr.error(`Esta categoria possui ${this.category.productCount} produto(s) cadastrados. Delete o(s) produto(s) associado(s) primeiro.`);
        }
      } 
        
    } catch (error: any) {
      this.toastr.error(error.message || 'Erro interno da aplicação, tente novamente.');
      console.error('Erro ao deletar categoria:', error);
    }
  }

  cancel() {
    this.dialogRef.close({ deleted: false });
  }

}
