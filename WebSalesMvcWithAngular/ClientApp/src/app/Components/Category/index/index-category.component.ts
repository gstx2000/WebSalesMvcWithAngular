import { Component, OnDestroy, ViewChild } from '@angular/core';
import { CategoryService } from '../../../Services/CategoryService';
import { LoadingService } from '../../../Services/LoadingService';
import { MatDialog } from '@angular/material/dialog';
import { DeleteCategoryComponent } from '../delete/delete-category.component';
import { MatTableDataSource } from '@angular/material/table';
import { ToastrService } from 'ngx-toastr';
import { CategoryDTO } from '../../../DTOs/CategoryDTO';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { Subject, takeUntil } from 'rxjs';
import { MatButtonToggleGroup } from '@angular/material/button-toggle';

@Component({
  selector: 'app-categories',
  templateUrl: './index-category.component.html',
  styleUrls: ['./index-category.component.css']
})
export class IndexCategoryComponent implements OnDestroy {
  private paginator!: MatPaginator;
  private sort!: MatSort;
  private destroy$ = new Subject<void>();

  categories: CategoryDTO[] = [];
  categoriesDataSource = new MatTableDataSource<CategoryDTO>();
  totalItems = 0;
  pageSize = 10;
  pageIndex = 0;
  selectedFilter = '';

  @ViewChild(MatButtonToggleGroup) filterGroup!: MatButtonToggleGroup;


  constructor(
    private categoryService: CategoryService,
    private loadingService: LoadingService,
    private toastr: ToastrService,
    private dialog: MatDialog
  ) {
    this.loadCategories();
  }

  ngAfterViewInit() {
    this.categoriesDataSource.paginator = this.paginator;
    this.categoriesDataSource.sort = this.sort;
    this.filterGroup.change.subscribe(() => this.filterCategories());
  }

  loadCategories(): void {
    this.loadingService.showLoading();
    this.categoryService.getCategoriesDTO()
      .pipe(takeUntil(this.destroy$))
      .subscribe(
        (result: CategoryDTO[]) => {
          this.categories = result;
          this.totalItems = result.length;
          this.updateDataSource();
          this.loadingService.hideLoading();
        },
        (error) => {
          this.toastr.error(error.message || 'Erro interno da aplicação, tente novamente.');
          console.error('Erro ao carregar categorias:', error);
          this.loadingService.hideLoading();
        }
      );
  }

  updateDataSource(): void {
    this.categoriesDataSource.data = this.categories.slice(
      this.pageIndex * this.pageSize,
      (this.pageIndex + 1) * this.pageSize
    );
  }

  onPageChange(event: PageEvent) {
    this.pageSize = event.pageSize;
    this.pageIndex = event.pageIndex;
    this.updateDataSource();
  }

  filterCategories() {
    const filterValue = this.filterGroup.value;
    let filteredData: CategoryDTO[];

    if (filterValue === 'categories') {
      filteredData = this.categories.filter(category => !category.isSubCategory);
    } else if (filterValue === 'all') {
      filteredData = this.categories;
    } else if (filterValue === 'subcategories') {
      filteredData = this.categories.filter(category => category.isSubCategory);
    }
    this.categoriesDataSource.data = filteredData!;
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

  ngOnDestroy() {
    this.categoriesDataSource.sort = null;
    this.destroy$.next();
    this.destroy$.complete();
  }
}
