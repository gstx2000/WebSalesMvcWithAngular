import { Component, OnDestroy, ViewChild } from '@angular/core';
import { DepartmentService } from '../../../Services/DepartmentService';
import { LoadingService } from '../../../Services/LoadingService';
import { MatDialog } from '@angular/material/dialog';
import { SalesRecordService } from '../../../Services/SalesRecordService';
import { DeleteSalesRecordComponent } from '../delete/delete-sales-record.component';
import { SaleStatus } from '../../../Models/enums/SaleStatus';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { catchError, map, startWith, switchMap, takeUntil } from 'rxjs/operators';
import { MatPaginator } from '@angular/material/paginator';
import { Subject, of } from 'rxjs';
import { SalesRecordDTO } from '../../../DTOs/SalesRecordDTO';

@Component({
  selector: 'app-sales-records',
  templateUrl: './index-sales-record.component.html',
  styleUrls: ['./index-sales-record.component.css']
})
export class IndexSalesRecordComponent implements OnDestroy {
  private paginator!: MatPaginator;
  private sort!: MatSort;

  salesRecords: SalesRecordDTO[] = [];
  salesRecordsDataSource: MatTableDataSource<SalesRecordDTO>;
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
    if (this.salesRecordsDataSource && this.paginator && this.sort) {
      this.salesRecordsDataSource.paginator = this.paginator;
      this.salesRecordsDataSource.sort = this.sort;
    }
  }
  constructor(private departmentService: DepartmentService,
    private loadingService: LoadingService,
    private dialog: MatDialog,
    private salesService: SalesRecordService,

  ){
    this.salesRecordsDataSource = new MatTableDataSource<SalesRecordDTO>(this.salesRecords);
  }

  ngAfterViewInit() {

    this.salesRecordsDataSource.paginator = this.paginator;
    this.paginator.page
      .pipe(
        startWith({}),
        switchMap(() => {
          this.loadingService.showLoading();
          return this.salesService.getSalesRecordsPaginated(
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
        this.salesRecords = empData;
        this.salesRecordsDataSource = new MatTableDataSource(this.salesRecords);
        this.salesRecordsDataSource.sort = this.sort;
        this.loadingService.hideLoading();

      }, null, () => {
        this.loadingService.hideLoading();

      });
  }

  ngOnDestroy() {
    this.salesRecordsDataSource.sort = null;
    this.destroy$.next();
    this.destroy$.complete();
  }

  getSaleStatusName(value: number): string {
    return SaleStatus[value] as string;
  }

  openDeleteDialog(salesRecord: SalesRecordDTO): void {
    const dialogRef = this.dialog.open(DeleteSalesRecordComponent, {
      data: { salesRecord },
      width: '550px',
      height: '350px'

    });

    dialogRef.afterClosed().subscribe(result => {
      if (result && result.deleted) {
        this.salesRecords = this.salesRecords.filter(p => p.id !== salesRecord.id);
        this.salesRecordsDataSource.data = this.salesRecords; 
      }
    });


  }
}
