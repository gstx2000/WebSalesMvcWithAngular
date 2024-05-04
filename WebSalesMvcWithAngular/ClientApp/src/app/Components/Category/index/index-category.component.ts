import { Component, OnInit } from '@angular/core';
import { CategoryService } from '../../../Services/CategoryService';
import { LoadingService } from '../../../Services/LoadingService';
import { MatDialog } from '@angular/material/dialog';
import { DeleteCategoryComponent } from '../delete/delete-category.component';
import { MatTableDataSource } from '@angular/material/table';
import { ToastrService } from 'ngx-toastr';
import { CategoryDTO } from '../../../DTOs/CategoryDTO';

@Component({
  selector: 'app-categories',
  templateUrl: './index-category.component.html',
  styleUrls: ['./index-category.component.css']
})
export class IndexCategoryComponent implements OnInit {
  categories: CategoryDTO[] = [];
  categoriesDataSource = new MatTableDataSource<CategoryDTO>();

  constructor(
    private categoryService: CategoryService,
    private loadingService: LoadingService,
    private toastr: ToastrService,
    private dialog: MatDialog
  ) {
  }

  ngOnInit(): void {
    this.loadCategories();
  }

  loadCategories(): void {
    this.loadingService.showLoading();
    this.categoryService.getCategoriesDTO().subscribe(
      (result: CategoryDTO[]) => {
        this.categories = result;
        this.categoriesDataSource = new MatTableDataSource(result);
        this.loadingService.hideLoading();
      },
      (error) => {
        this.toastr.error(error.message || 'Erro interno da aplicação, tente novamente.');
        console.error('Erro ao carregar categorias:', error);
        this.loadingService.hideLoading();
      }
    );
  }

  openDeleteDialog(category: CategoryDTO): void {
    const dialogRef = this.dialog.open(DeleteCategoryComponent, {
      data: { category },
      width: '550px',
      height: '350px',

    });

    dialogRef.afterClosed().subscribe(result => {
      if (result && result.deleted) {
        this.categories = this.categories.filter(p => p.id !== category.id);
        this.categoriesDataSource.data = [...this.categories];
        this.loadCategories();
      }
    });
  }
}
