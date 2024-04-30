import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Department } from '../../../Models/Department';
import { Router } from '@angular/router';
import { Product } from '../../../Models/Product';
import { ProductService } from '../../../Services/ProductService';
import { Category } from '../../../Models/Category';
import { DepartmentService } from '../../../Services/DepartmentService';
import { Observable } from 'rxjs';
import { CategoryService } from '../../../Services/CategoryService';
import { LoadingService } from '../../../Services/LoadingService';
import { ToastrService } from 'ngx-toastr';
import { InventoryUnitMeas } from '../../../Models/enums/InventoryUnitMeas';

@Component({
  selector: 'app-products/create',
  templateUrl: './create-product.component.html',
  styleUrls: ['./create-product.component.css']
})
export class CreateProductComponent implements OnInit {
  products: Product[] = [];
  productForm!: FormGroup;
  product: Product;
  departments$: Observable<Department[]> | undefined;
  categories$: Observable<Category[]> | undefined;
  InventoryUnitMeas!: InventoryUnitMeas;
  constructor(
    private productService: ProductService,
    private departmentService: DepartmentService,
    private categoryService: CategoryService,
    private fb: FormBuilder,
    private router: Router,
    private loadingService: LoadingService,
    private toastr: ToastrService

  ) {
    this.product = {
      price: 0,
      name: '',
      description: '',
      categoryId: 0,
      departmentId: 0,
      imageUrl: '',
      inventoryUnitMeas: 0
    };
  }

  ngOnInit(): void {
    this.initProductForm();
    this.departments$ = this.departmentService.getDepartments();
    this.categories$ = this.categoryService.getCategories();
  }

  initProductForm(): void {
    this.productForm = this.fb.group({
      price: [1, Validators.min(0.1)],
      name: ['', Validators.required],
      description: '',
      categoryId: [0],
      departmentId: [0],
      imageUrl: '',
      inventoryUnitMeas: 0
    });
  }

  async onSubmit(): Promise<void> {
    this.loadingService.showLoading();
    try {
      console.log(this.productForm.value)
      if (this.productForm.valid) {
        const formData: Product = this.productForm.value;
        const createdProduct = await (await this.productService.createProduct(formData)).toPromise();
        this.toastr.success(`Produto ${formData.name} criado com sucesso.`);
        this.router.navigate(['/products']);
      }
    } catch (error: any) {
      this.toastr.error(error.message || 'Erro interno da aplicação, tente novamente.');
      console.error('Erro ao criar:', error);
      this.loadingService.hideLoading();
    }
    () => {
      this.loadingService.hideLoading();
    }
  }

  getUnitMeasValues(): number[] {
    return Object.values(InventoryUnitMeas).filter(value => typeof value === 'number') as number[];
  }

  getUnitMeasName(value: number): string {
    return InventoryUnitMeas[value] as string;
  }

  cancel() {
    this.router.navigate(['/products']);
  }
}

