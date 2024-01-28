import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { HttpClientXsrfModule } from '@angular/common/http';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatSelectModule } from '@angular/material/select';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { NavMenuComponent } from './Components/nav-menu/nav-menu.component';
import { DepartmentService } from './Services/DepartmentService';
import { AuthService } from './Services/AuthService';

import { HomeComponent } from './Components/home/home.component';
import { IndexDepartmentComponent } from './Components/Department/index/index-department.component';
import { CreateDepartmentComponent } from './Components/Department/create/create-department.component';
import { DeleteDepartmentComponent } from './Components/Department/delete/delete-department.component';
import { EditDepartmentComponent } from './Components/Department/edit/edit-department.component';
import { DetailsDepartmentComponent } from './Components/Department/details/details-department.component';
import { IndexProductComponent } from './Components/Product/index/index-product.component';
import { CreateProductComponent } from './Components/Product/create/create-product.component';
import { IndexCategoryComponent } from './Components/Category/index/index-category.component';
import { CreateCategoryComponent } from './Components/Category/create/create-category.component';
import { EditProductComponent } from './Components/Product/edit/edit-product.component';
import { DeleteProductComponent } from './Components/Product/delete/delete-product.component';
import { GlobalLoadingComponent } from './Components/GlobalLoading/global-loading.component';


import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatTableModule } from '@angular/material/table';
import { MatDialogModule } from '@angular/material/dialog';
import { FlexLayoutModule } from '@angular/flex-layout';



import { LoadingService } from './Services/LoadingService';
import { ProductService } from './Services/ProductService';
import { CategoryService } from './Services/CategoryService';
import { DetailsProductComponent } from './Components/Product/details/details-product.component';

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    HomeComponent,
    IndexDepartmentComponent,
    CreateDepartmentComponent,
    EditDepartmentComponent,
    DeleteDepartmentComponent,
    DetailsDepartmentComponent,
    IndexProductComponent,
    CreateProductComponent,
    IndexCategoryComponent,
    CreateCategoryComponent,
    EditProductComponent,
    DeleteProductComponent,
    GlobalLoadingComponent,
    DetailsProductComponent,
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientXsrfModule.withOptions({
      cookieName: 'XSRF-TOKEN',
      headerName: 'X-XSRF-TOKEN',
    }),
    HttpClientModule,
    ReactiveFormsModule,
    FormsModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatProgressSpinnerModule,
    MatIconModule,
    MatTableModule,
    MatDialogModule,
    MatSelectModule,
    FlexLayoutModule

  ],
  providers: [DepartmentService, AuthService, LoadingService, ProductService, CategoryService],
  bootstrap: [AppComponent]
})
export class AppModule { }
