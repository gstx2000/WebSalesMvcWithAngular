import { Component, Inject, OnInit } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { Category } from '../../../Models/Category';
import { LoadingService } from '../../../Services/LoadingService';
import { CategoryService } from '../../../Services/CategoryService';

@Component({
  selector: 'app-categories/delete',
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
  ) { }

  ngOnInit(): void {
    if (this.data.category && this.data.category.id !== undefined) {
      this.category = this.data.category;
    }
  }

  async onDeleteClick(): Promise<void> {

    try {
      if (this.category && this.category.id) {
        await (await this.categoryService.deleteCategory(this.category.id)).toPromise();
        this.dialogRef.close({ deleted: true });
      }
    } catch (error) {
      console.error('Erro ao deletar categoria:', error);
    }
  }

  cancel() {
    this.dialogRef.close({ deleted: false });
  }

}
