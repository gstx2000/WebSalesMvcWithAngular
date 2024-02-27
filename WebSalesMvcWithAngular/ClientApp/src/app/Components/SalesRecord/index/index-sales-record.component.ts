import { Component, OnInit } from '@angular/core';
import { Department } from '../../../Models/Department';
import { DepartmentService } from '../../../Services/DepartmentService';
import { LoadingService } from '../../../Services/LoadingService';
import { MatDialog } from '@angular/material/dialog';
import { SalesRecord } from '../../../Models/SalesRecord';
import { SalesRecordService } from '../../../Services/SalesRecordService';
import { DeleteSalesRecordComponent } from '../delete/delete-sales-record.component';
import { SaleStatus } from '../../../Models/enums/SaleStatus';

@Component({
  selector: 'app-sales-records',
  templateUrl: './index-sales-record.component.html',
  styleUrls: ['./index-sales-record.component.css']
})
export class IndexSalesRecordComponent implements OnInit {
  salesRecords: SalesRecord[] = [];
  departments: Department[] = [];

  constructor(private departmentService: DepartmentService,
    private loadingService: LoadingService,
    private dialog: MatDialog,
    private salesService: SalesRecordService) { }

  ngOnInit(): void {
    this.loadSalesRecords();
  }

  loadSalesRecords(): void {
    this.loadingService.showLoading();

    this.salesService.getSalesRecords().subscribe(
      (result: SalesRecord[]) => {
        if (Array.isArray(result)) {
          this.salesRecords = result;
        }
      },
      (error) => {
        console.error('Erro ao carregar departamentos:', error);
        this.loadingService.hideLoading();
      },
      () => {
        this.loadingService.hideLoading();
      }
    );
  }

  getSaleStatusName(value: number): string {
    return SaleStatus[value] as string;
  }

  openDeleteDialog(salesRecord: SalesRecord): void {
    const dialogRef = this.dialog.open(DeleteSalesRecordComponent, {
      data: { salesRecord },
      width: '350px',

    });

    dialogRef.afterClosed().subscribe(result => {
      if (result && result.deleted) {
        this.salesRecords = this.salesRecords.filter(p => p.id !== salesRecord.id);
        this.loadSalesRecords();
      }
    });
  }
}
