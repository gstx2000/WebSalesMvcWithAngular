import { Component, Inject, OnInit } from '@angular/core';
import { SalesRecord } from '../../../Models/SalesRecord';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { SalesRecordService } from '../../../Services/SalesRecordService';
import { LoadingService } from '../../../Services/LoadingService';
import { SaleStatus } from '../../../Models/enums/SaleStatus';
import { PaymentMethod } from '../../../Models/enums/PaymentMethod';

@Component({
  selector: 'app-delete-sales-record',
  templateUrl: './delete-sales-record.component.html',
  styleUrls: ['./delete-sales-record.component.css']
})
export class DeleteSalesRecordComponent implements OnInit {
  salesRecord?: SalesRecord;
  constructor(
    public dialogRef: MatDialogRef<DeleteSalesRecordComponent>,
    @Inject(MAT_DIALOG_DATA) public data: { salesRecord: SalesRecord },
    private salesRecordService: SalesRecordService,
    private loadingService: LoadingService,
  ) { }

  ngOnInit(): void {
    if (this.data.salesRecord && this.data.salesRecord.id !== undefined) {
      this.salesRecord = this.data.salesRecord;
    }
  }

  async onDeleteClick(): Promise<void> {

    try {
      if (this.salesRecord && this.salesRecord.id) {
        await (await this.salesRecordService.deleteSalesRecord(this.salesRecord.id)).toPromise();
        this.dialogRef.close({ deleted: true });
      }
    } catch (error) {
      console.error('Erro ao deletar venda:', error);
    }
  }

  getSaleStatusName(value: number): string {
    return SaleStatus[value] as string;
  }

  getPaymentMethodName(value: number): string {
    return PaymentMethod[value] as string;
  }

  cancel() {
    this.dialogRef.close({ deleted: false });
  }

}
