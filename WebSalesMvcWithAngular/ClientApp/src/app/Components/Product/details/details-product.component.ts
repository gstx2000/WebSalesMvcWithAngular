import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Router } from '@angular/router';
import { LoadingService } from '../../../Services/LoadingService';
import { Product } from '../../../Models/Product';
import { ProductService } from '../../../Services/ProductService';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-products/details',
  templateUrl: './details-product.component.html',
  styleUrls: ['./details-product.component.css']
})
export class DetailsProductComponent implements OnInit {
  product: Product | undefined;
  constructor(
    private activedroute: ActivatedRoute,
    private router: Router,
    private productService: ProductService,
    private loadingService: LoadingService,
    private toastr: ToastrService

  ) { }

  ngOnInit(): void {
    const productId = Number(this.activedroute.snapshot.params['id']);
    this.loadProduct(productId);
  }

  async loadProduct(id: number): Promise<void> {
    this.loadingService.showLoading();
    (await this.productService.getProductById(id)).subscribe((result: Product) => {
      this.product = result;
      this.loadingService.hideLoading();

    },
      (error) => {
        this.loadingService.hideLoading();
        this.toastr.error(error.message || 'Erro interno da aplicação, tente novamente.');
        console.error('Erro ao carregar produto:', error);
      }
    );
  }

  backToIndex() {
    this.router.navigate(['/products']);
  }
}
