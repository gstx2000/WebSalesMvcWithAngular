import { Component, Inject, OnInit } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { Category } from '../../../Models/Category';
import { LoadingService } from '../../../Services/LoadingService';
import { CategoryService } from '../../../Services/CategoryService';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-categories/delete-category',
  templateUrl: './delete-category.component.html',
  styleUrls: ['./delete-category.component.css']
})
export class DeleteCategoryComponent implements OnInit {
  category?: Category;
  constructor(
    public dialogRef: MatDialogRef<DeleteCategoryComponent>,
    @Inject(MAT_DIALOG_DATA) public data: { category: Category },
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
        if (this.category.products?.length === 0) {
          await (await this.categoryService.deleteCategory(this.category.id)).toPromise();
          this.toastr.success(`Categoria ${this.category.name} deletada com sucesso.`);
          this.dialogRef.close({ deleted: true });
        } else {
          this.loadingService.hideLoading();
          this.toastr.error(`Esta categoria possui ${this.category.products?.length} produto(s) cadastrados. Delete o(s) produto(s) associado(s) primeiro.`);
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
