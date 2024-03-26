import { Component, Inject, OnInit } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { SalesRecordService } from '../../../Services/SalesRecordService';
import { LoadingService } from '../../../Services/LoadingService';
import { SaleStatus } from '../../../Models/enums/SaleStatus';
import { PaymentMethod } from '../../../Models/enums/PaymentMethod';
import { ToastrService } from 'ngx-toastr';
import { SalesRecordDTO } from '../../../DTOs/SalesRecordDTO';

@Component({
  selector: 'app-salesrecords/delete-sales-record',
  templateUrl: './delete-sales-record.component.html',
  styleUrls: ['./delete-sales-record.component.css']
})
export class DeleteSalesRecordComponent implements OnInit {
  salesRecord?: SalesRecordDTO;
  constructor(
    public dialogRef: MatDialogRef<DeleteSalesRecordComponent>,
    @Inject(MAT_DIALOG_DATA) public data: { salesRecord: SalesRecordDTO },
    private salesRecordService: SalesRecordService,
    private loadingService: LoadingService,
    private toastr: ToastrService
  ) { }

  ngOnInit(): void {
    if (this.data.salesRecord && this.data.salesRecord.id !== undefined) {
      this.salesRecord = this.data.salesRecord;
    }
  }

  async onDeleteClick(): Promise<void> {
    try {
      if (this.salesRecord && this.salesRecord.id) {
        this.loadingService.showLoading();
        if (this.salesRecord.status !== 1) {
          await (await this.salesRecordService.deleteSalesRecord(this.salesRecord.id)).toPromise();
          this.toastr.success(`Venda deletada com sucesso.`);
          this.dialogRef.close({ deleted: true });
        } else {
          this.toastr.error('Vendas faturadas não podem ser deletadas, somente canceladas.');
        }
      }
    } catch (error: any) {
      this.toastr.error(error.message || 'Erro interno da aplicação, tente novamente.');
      console.error('Erro ao deletar venda:', error);
    } finally {
      this.loadingService.hideLoading();
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
