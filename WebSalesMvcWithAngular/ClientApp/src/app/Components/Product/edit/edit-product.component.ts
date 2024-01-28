import { Component, OnInit } from '@angular/core';
import { DepartmentService } from '../../../Services/DepartmentService';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Department } from '../../../Models/Department';
import { ActivatedRoute } from '@angular/router';
import { Router } from '@angular/router';
import { ProductService } from '../../../Services/ProductService';
import { Observable } from 'rxjs';
import { Category } from '../../../Models/Category';
import { CategoryService } from '../../../Services/CategoryService';
import { Product } from '../../../Models/Product';
import { MyErrorStateMatcher } from '../../Department/edit/edit-department.component';
import { LoadingService } from '../../../Services/LoadingService';

@Component({
  selector: 'app-edit-product',
  templateUrl: './edit-product.component.html',
  styleUrls: ['./edit-product.component.css']
})
export class EditProductComponent {
  departments$: Observable<Department[]> | undefined;
  productForm!: FormGroup;
  categories$: Observable<Category[]> | undefined;
  matcher = new MyErrorStateMatcher();
  constructor(
    private departmentService: DepartmentService,
    private productService: ProductService,
    private categoryService: CategoryService,
    private loadingService: LoadingService,

    private fb: FormBuilder,
    private activedroute: ActivatedRoute,
    private router: Router
  ) { }

  ngOnInit(): void {
    this.initializeForm();
    const productId = Number(this.activedroute.snapshot.params['id']);
    this.departments$ = this.departmentService.getDepartments();
    this.categories$ = this.categoryService.getCategories();

    if (productId) {
      this.fetchProduct(productId);
    }
  }

  initializeForm(): void {
    this.productForm = this.fb.group({
      id: '',
      name: ['', Validators.required],
      price: [0, Validators.min(0)],
      description: '',
      categoryId: 0,
      departmentId: 0,
      imageUrl: ''
    });
  }

  async fetchProduct(id: number): Promise < void> {
    this.loadingService.showLoading();

    try {
      const fetchedProduct = await (await this.productService.getProductById(id)).toPromise();
      if (fetchedProduct)
        this.productForm.patchValue({
          id: fetchedProduct.id,
          name: fetchedProduct.name,
          price: fetchedProduct.price,
          description: fetchedProduct.description,
          categoryId: fetchedProduct.categoryId,
          departmentId: fetchedProduct.departmentId,
          imageUrl: fetchedProduct.imageUrl

      });

      this.productForm.get('id')!.disable();

    } catch(error) {
      console.error('Erro ao buscar produto:', error);
      this.loadingService.hideLoading();
    }
    this.loadingService.hideLoading();

  }

  async onSubmit(): Promise < void> {
    try {
      if(this.productForm.valid) {
    const formData: Product = this.productForm.getRawValue();
    if (formData.id) {
      const productId = await (await this.productService.getProductById(formData.id)).toPromise();
      if (productId) {
        if (productId.id) {
          const updatedDepartment = await (await this.productService.updateProduct(productId.id, formData)).toPromise();
          this.router.navigate(['/products']);

        }
      }
    }
  }
} catch (error) {
  console.error('Erro ao atualizar produto:', error);
}
  }

cancel() {
  this.router.navigate(['/products']);
}
}
