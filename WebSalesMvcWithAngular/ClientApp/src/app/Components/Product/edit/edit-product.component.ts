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
import { LoadingService } from '../../../Services/LoadingService';
import { ToastrService } from 'ngx-toastr';
import { FormControlErrorMessageService } from '../../../Services/FormControlErrorMessage/form-control-error-message.service';


@Component({
  selector: 'app-products/edit',
  templateUrl: './edit-product.component.html',
  styleUrls: ['./edit-product.component.css']
})
export class EditProductComponent {
  departments$: Observable<Department[]> | undefined;
  productForm!: FormGroup;
  categories$: Observable<Category[]> | undefined;

  private fieldLabels: { [key: string]: string } = {
    price: 'Preço',
    name: 'Nome',
    description: 'Descrição',
    categoryId: 'ID Categoria',
    departmentId: 'ID Departamento',
    imageUrl: 'URL de imagem',
    inventoryUnitMeas: 'Unidade de medida'
  };
  constructor(
    private departmentService: DepartmentService,
    private productService: ProductService,
    private categoryService: CategoryService,
    private loadingService: LoadingService,
    private fb: FormBuilder,
    private activedroute: ActivatedRoute,
    private router: Router,
    private toastr: ToastrService,
    private formMessage: FormControlErrorMessageService
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
      id: 0,
      name: ['', Validators.required],
      price: [1, Validators.min(0.1)],
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

    } catch(error: any) {
      console.error('Erro ao buscar produto:', error);
      this.toastr.error(error.message || 'Erro interno da aplicação, tente novamente.');
      this.loadingService.hideLoading();
    }
    this.loadingService.hideLoading();

  }

  async onSubmit(): Promise<void> {
    try {
      if (this.productForm.valid) {
        const formData: Product = this.productForm.getRawValue();
        if (formData.id) {
          const productId = await (await this.productService.getProductById(formData.id)).toPromise();
          if (productId) {
            if (productId.id) {
              const updatedProduct = await (await this.productService.updateProduct(productId.id, formData)).toPromise();
              this.toastr.success(`Produto ${formData.name} alterado com sucesso.`);
              this.router.navigate(['/products']);
            }
          }
        }
      } else {
        this.loadingService.hideLoading();
        Object.keys(this.productForm.controls).forEach(field => {
          const control = this.productForm.get(field);
          if (control) {
            if (control.invalid && control.touched) {
              const label = this.fieldLabels[field] || field;
              const errorMessage = this.formMessage.getErrorMessage(control.errors);
              this.toastr.error(`Campo ${label} está inválido: ${errorMessage}`);
            }
          }
        });
      }
    } catch (error: any) {
      this.toastr.error(error.message || 'Erro interno da aplicação, tente novamente.');
      console.error('Erro ao atualizar produto:', error);
    }
  }
}
