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
  constructor(
    private productService: ProductService,
    private departmentService: DepartmentService,
    private categoryService: CategoryService,
    private fb: FormBuilder,
    private router: Router
  ) {
    this.product = {
      price: 0,
      name: '',
      description: '',
      categoryId: 0,
      departmentId: 0,
      imageUrl: ''
    };
  }

  ngOnInit(): void {
    this.initProductForm();
    this.departments$ = this.departmentService.getDepartments();
    this.categories$ = this.categoryService.getCategories();
  }

  initProductForm(): void {
    this.productForm = this.fb.group({
      price: [0, Validators.min(0)],
      name: ['', Validators.required],
      description: '',
      categoryId: [0],
      departmentId: [0],
      imageUrl: ''
    });
  }

  async onSubmit(): Promise<void> {

    try {
      if (this.productForm.valid) {
        const formData: Product = this.productForm.value;
        const createdProduct = await (await this.productService.createProduct(formData)).toPromise();
        this.router.navigate(['/products']);
      }
    } catch (error) {
      console.error('Erro ao criar:', error);
    }
  }

  cancel() {
    this.router.navigate(['/products']);
  }
}

