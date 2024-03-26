import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { SalesRecordService } from '../../../Services/SalesRecordService';
import { LoadingService } from '../../../Services/LoadingService';
import { ToastrService } from 'ngx-toastr';
import { SalesRecord } from '../../../Models/SalesRecord';
import { SaleStatus } from '../../../Models/enums/SaleStatus';
import { PaymentMethod } from '../../../Models/enums/PaymentMethod';
import { MatTableDataSource } from '@angular/material/table';
import { SoldProduct } from '../../../Models/SoldProduct';

@Component({
  selector: 'app-sales-records/details-sales-record',
  templateUrl: './details-sales-record.component.html',
  styleUrls: ['./details-sales-record.component.css']
})
export class DetailsSalesRecordComponent implements OnInit {
  salesRecord: SalesRecord | undefined;
  salesRecordDataSource = new MatTableDataSource<SoldProduct>();

  constructor(
    private activedroute: ActivatedRoute,
    private router: Router,
    private salesService: SalesRecordService,
    private loadingService: LoadingService,
    private toastr: ToastrService


  ) {}

  ngOnInit(): void {
    const salesId = Number(this.activedroute.snapshot.params['id']);
    this.loadSale(salesId);
  }

  async loadSale(id: number): Promise<void> {
    this.loadingService.showLoading();
    (await this.salesService.getSalesRecordById(id)).subscribe((result: SalesRecord) => {
      this.salesRecord = result;
      this.salesRecordDataSource.data = result.soldProducts || [];
      this.loadingService.hideLoading();
    },
      (error: any) => {
        this.loadingService.hideLoading();
        this.toastr.error(error.message || 'Erro interno da aplicação, tente novamente.');
        console.error('Erro ao carregar produto:', error);
      });
  }

  getSaleStatusName(value: number): string {
    return SaleStatus[value] as string;
  }

  getPaymentMethodName(value: number): string {
    return PaymentMethod[value] as string;
  }

  backToIndex() {
    this.router.navigate(['/salesRecords']);
  }
}

