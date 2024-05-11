import { Component } from '@angular/core';
import { LoadingService } from '../../../Services/LoadingService';
import { MatDialog } from '@angular/material/dialog';
import { ToastrService } from 'ngx-toastr';
import { Supplier } from '../../../Models/Supplier';
import { SupplierService } from '../../../Services/SupplierService/supplier.service';
import { MatTableDataSource } from '@angular/material/table';
import { SupplierType } from '../../../Models/enums/SupplierType';

@Component({
  selector: 'app-index-supplier',
  templateUrl: './index-supplier.component.html',
  styleUrls: ['./index-supplier.component.css']
})
export class IndexSupplierComponent {
  suppliers: Supplier[] = [];
  suppliersDataSource = new MatTableDataSource<Supplier>();
  constructor(private supplierService: SupplierService,
    private loadingService: LoadingService,
    private dialog: MatDialog,
    private toastr: ToastrService
  ) {
    this.loadSuppliers();
  }

  loadSuppliers(): void {
    this.loadingService.showLoading();

    this.supplierService.getSuppliers().subscribe(
      (result: Supplier[]) => {
        if (Array.isArray(result)) {
          this.suppliers = result;
          this.suppliersDataSource = new MatTableDataSource(result);
          this.loadingService.hideLoading();
        }
      },
      (error) => {
        console.error('Erro ao carregar fornecedores:', error);
        this.loadingService.hideLoading();
      },
      () => {
        this.loadingService.hideLoading();
      }
    );
  }

  getSupplierTypeName(value: number): string {
    return SupplierType[value] as string;
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

