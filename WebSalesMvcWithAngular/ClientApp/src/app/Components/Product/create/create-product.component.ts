import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Department } from '../../../Models/Department';
import { Router } from '@angular/router';
import { Product } from '../../../Models/Product';
import { ProductService } from '../../../Services/ProductService';
import { DepartmentService } from '../../../Services/DepartmentService';
import { Observable, map } from 'rxjs';
import { CategoryService } from '../../../Services/CategoryService';
import { LoadingService } from '../../../Services/LoadingService';
import { ToastrService } from 'ngx-toastr';
import { InventoryUnitMeas } from '../../../Models/enums/InventoryUnitMeas';
import { ProductDTO } from '../../../DTOs/ProductDTO';
import { CategoryDTO } from '../../../DTOs/CategoryDTO';
import { FormControlErrorMessageService } from '../../../Services/FormControlErrorMessage/form-control-error-message.service';

@Component({
  selector: 'app-products/create',
  templateUrl: './create-product.component.html',
  styleUrls: ['./create-product.component.css']
})
export class CreateProductComponent implements OnInit {
  products: ProductDTO[] = [];
  productForm!: FormGroup;
  product: Product;
  departments$: Observable<Department[]> | undefined;
  categories$: Observable<CategoryDTO[]> | undefined;
  subcategories$: Observable<CategoryDTO[]> | undefined;
  InventoryUnitMeas!: InventoryUnitMeas;
  selectedFile: File | string = '';
  selectedFileName: string = '';
  filePreview: string | ArrayBuffer | null = null;


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
    private productService: ProductService,
    private departmentService: DepartmentService,
    private categoryService: CategoryService,
    private fb: FormBuilder,
    private router: Router,
    private loadingService: LoadingService,
    private toastr: ToastrService,
    private formMessage: FormControlErrorMessageService
  ) {
    this.product = {
      price: 0,
      name: '',
      description: '',
      categoryId: 0,
      departmentId: 0,
      imageUrl: '',
      inventoryUnitMeas: 0,
      supplierId: 0
    };
  }

  ngOnInit(): void {
    this.initProductForm();
    this.departments$ = this.departmentService.getDepartments();
    this.categories$ = this.categoryService.getCategoriesDTO();
    this.subcategories$ = this.categories$.pipe(
      map(categories => categories.filter(category => category.isSubCategory == true))
    );
    this.categories$ = this.categories$.pipe(
      map(categories => categories.filter(category => category.isSubCategory == false))
    );
  }

  initProductForm(): void {
    this.productForm = this.fb.group({
      price: [0, [Validators.required, Validators.min(1)]],
      name: ['', Validators.required],
      description: '',
      categoryId: [null, Validators.required],
      subCategoryId: [null], 
      departmentId: [null, Validators.required],
      imageUrl: '',
      inventoryUnitMeas: 0,
      subcategoryId: [null],
    });
  }

  onFileSelected(event: any): void {
    const file = event.target.files[0];
    this.selectedFile = file;
    this.selectedFileName = file ? file.name : '';

    if (file) {
      const reader = new FileReader();
      reader.onload = (e) => {
        this.selectedFile = reader.result as string;
      };
      reader.readAsDataURL(file);
    }
  }

  triggerFileInput() {
    document.getElementById('imageFile')?.click();
  }


  async onSubmit(): Promise<void> {
    this.loadingService.showLoading();
    try {
      if (this.productForm.valid) {
        const formData = new FormData();

        Object.keys(this.productForm.value).forEach(key => {
          if (this.productForm.value[key] !== null && this.productForm.value[key] !== undefined) {
            formData.append(key, this.productForm.value[key]);
          }
        });

        if (this.selectedFile) {
          formData.append('imageFile', this.selectedFile);
        }

        const createdProduct = await (await this.productService.createProduct(formData)).toPromise();

        this.toastr.success(`Produto ${this.productForm.value.name} criado com sucesso.`);
        this.router.navigate(['/products']);
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
      console.error('Erro ao criar:', error);
      this.loadingService.hideLoading();
    }
  }

  getUnitMeasValues(): number[] {
    return Object.values(InventoryUnitMeas).filter(value => typeof value === 'number') as number[];
  }

  getUnitMeasName(value: number): string {
    return InventoryUnitMeas[value] as string;
  }
}

