import { Component, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { LoadingService } from '../../../Services/LoadingService';
import { ToastrService } from 'ngx-toastr';
import { ProductService } from '../../../Services/ProductService';
import { Observable, Subject } from 'rxjs';
import { CategoryService } from '../../../Services/CategoryService';
import { Category } from '../../../Models/Category';
import { MatTableDataSource } from '@angular/material/table';
import { ProductDTO } from '../../../DTOs/ProductDTO';
import { MatSort } from '@angular/material/sort';
import { MatPaginator } from '@angular/material/paginator';
import { SupplierService } from '../../../Services/SupplierService/supplier.service';
import { Supplier } from '../../../Models/Supplier';
import { SupplierType } from '../../../Models/enums/SupplierType';

@Component({
  selector: 'app-details-supplier',
  templateUrl: './details-supplier.component.html',
  styleUrls: ['./details-supplier.component.css']
})
export class DetailsSupplierComponent {

  selectedProducts: ProductDTO[] = [];
  filteredProducts: ProductDTO[] = [];
  categories$!: Observable<Category[]>;
  productsDataSource = new MatTableDataSource<ProductDTO>(this.selectedProducts);
  supplier!: Supplier;

  private destroy$ = new Subject<void>();
  private paginator!: MatPaginator;
  private sort!: MatSort;
  @ViewChild(MatSort) set matSort(ms: MatSort) {
    this.sort = ms;
  }
  @ViewChild(MatPaginator) set matPaginator(mp: MatPaginator) {
    this.paginator = mp;
  }
  constructor(
    private router: Router,
    private loadingService: LoadingService,
    private toastr: ToastrService,
    private categoryService: CategoryService,
    private productService: ProductService,
    private supplierService: SupplierService,
    private activedroute: ActivatedRoute

  ) {
    const supplierId = Number(this.activedroute.snapshot.params['id']);
    if (supplierId) {
      this.loadSupplier(supplierId);
    }
  }

  async loadSupplier(id: number): Promise<void> {
    this.loadingService.showLoading;
    (await this.supplierService.getSupplierById(id)).subscribe(
      (result: Supplier) => {
        if (result){
          this.supplier = result;
          this.productsDataSource = new MatTableDataSource(result.products);
          this.loadingService.hideLoading();
        }
      },
      (error) => {
        console.error('Erro ao carregar fornecedor:', error);
        this.loadingService.hideLoading();
      },
      () => {
        this.loadingService.hideLoading();
      }
    );
  }

  removeFromSupplier(id: number) {
    return;
  }

  getSupplierTypeValues(): number[] {
    return Object.values(SupplierType).filter(value => typeof value === 'number') as number[];
  }

  getSupplierTypeName(value: number): string {
    return SupplierType[value] as string;
  }

  ngOnDestroy() {
    this.destroy$.next();
    this.destroy$.complete();
  }
}
