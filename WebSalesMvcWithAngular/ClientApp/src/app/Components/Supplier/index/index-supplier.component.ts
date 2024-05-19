import { Component, ViewChild } from '@angular/core';
import { LoadingService } from '../../../Services/LoadingService';
import { MatDialog } from '@angular/material/dialog';
import { ToastrService } from 'ngx-toastr';
import { SupplierService } from '../../../Services/SupplierService/supplier.service';
import { MatTableDataSource } from '@angular/material/table';
import { SupplierType } from '../../../Models/enums/SupplierType';
import { IndexSupplierResponse } from '../Responses/IndexSupplierResponse';
import { MatSort } from '@angular/material/sort';
import { MatPaginator } from '@angular/material/paginator';
import { Subject, catchError, map, of, startWith, switchMap, takeUntil } from 'rxjs';

@Component({
  selector: 'app-index-supplier',
  templateUrl: './index-supplier.component.html',
  styleUrls: ['./index-supplier.component.css']
})
export class IndexSupplierComponent {
  suppliers: IndexSupplierResponse[] = [];
  suppliersDataSource = new MatTableDataSource<IndexSupplierResponse>();
  private paginator!: MatPaginator;
  private sort!: MatSort;
  private destroy$ = new Subject<void>();

  currentPage = 1;
  pageSize = 10;
  totalItems = 0;
  totalPages = 0;

  @ViewChild(MatSort) set matSort(ms: MatSort) {
    this.sort = ms;
    this.setDataSourceAttributes();
  }
  @ViewChild(MatPaginator) set matPaginator(mp: MatPaginator) {
    this.paginator = mp;
    this.setDataSourceAttributes();
  }
  setDataSourceAttributes() {
    if (this.suppliersDataSource && this.paginator && this.sort) {
      this.suppliersDataSource.paginator = this.paginator;
      this.suppliersDataSource.sort = this.sort;
    }
  }
  constructor(private supplierService: SupplierService,
    private loadingService: LoadingService,
    private dialog: MatDialog,
    private toastr: ToastrService
  ) {
    }

  ngAfterViewInit() {

    this.suppliersDataSource.paginator = this.paginator;
    this.paginator.page
      .pipe(
        startWith({}),
        switchMap(() => {
          this.loadingService.showLoading();
          return this.supplierService.getSuppliersPaginated(
            this.paginator.pageIndex + 1,
            this.paginator.pageSize
          ).pipe(
            catchError((error) => {
              this.loadingService.hideLoading();
              return of(null);
            }),
            takeUntil(this.destroy$)
          );
        }),
        map((Data) => {
          if (Data == null) return [];
          this.totalItems = Data.totalItems;
          this.loadingService.hideLoading();
          return Data.items;
        })
      )
      .subscribe((empData) => {
        this.suppliers = empData;
        this.suppliersDataSource = new MatTableDataSource(this.suppliers);
        this.suppliersDataSource.sort = this.sort;
        this.loadingService.hideLoading();
      }, null, () => {
        this.loadingService.hideLoading();
      });
  }

  getSupplierTypeName(value: number): string {
    return SupplierType[value] as string;
  }

  ngOnDestroy() {
    this.suppliersDataSource.sort = null;
    this.destroy$.next();
    this.destroy$.complete();
  }


  //openDeleteDialog(department: Department): void {
  //  const dialogRef = this.dialog.open(DeleteDepartmentComponent, {
  //    data: { department },
  //    width: '550px',
  //    height: '350px'
  //  });

  //  dialogRef.afterClosed().subscribe(result => {
  //    if (result && result.deleted) {
  //      this.departments = this.departments.filter(p => p.id !== department.id);
  //      this.loadSuppliers();
  //    }
  //  });
  //}
}

