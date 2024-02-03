import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { HttpClientXsrfModule } from '@angular/common/http';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatSelectModule } from '@angular/material/select';

import { AppRoutingModule } from './app-routing.module';
import { DepartmentService } from './Services/DepartmentService';

import { AppComponent } from './app.component';
import { HomeComponent } from './Components/home/home.component';
import { NavMenuComponent } from './Components/nav-menu/nav-menu.component';
import { GlobalLoadingComponent } from './Components/GlobalLoading/global-loading.component';

import { IndexDepartmentComponent } from './Components/Department/index/index-department.component';
import { CreateDepartmentComponent } from './Components/Department/create/create-department.component';
import { DeleteDepartmentComponent } from './Components/Department/delete/delete-department.component';
import { EditDepartmentComponent } from './Components/Department/edit/edit-department.component';
import { DetailsDepartmentComponent } from './Components/Department/details/details-department.component';

import { IndexProductComponent } from './Components/Product/index/index-product.component';
import { CreateProductComponent } from './Components/Product/create/create-product.component';
import { EditProductComponent } from './Components/Product/edit/edit-product.component';
import { DeleteProductComponent } from './Components/Product/delete/delete-product.component';
import { DetailsProductComponent } from './Components/Product/details/details-product.component';

import { IndexCategoryComponent } from './Components/Category/index/index-category.component';
import { CreateCategoryComponent } from './Components/Category/create/create-category.component';
import { EditCategoryComponent } from './Components/Category/edit/edit-category.component';
import { DeleteCategoryComponent } from './Components/Category/delete/delete-category.component';

import { CreateSalesRecordComponent } from './Components/SalesRecord/create/create-sales-record.component';
import { IndexSalesRecordComponent } from './Components/SalesRecord/index/index-sales-record.component';
import { DeleteSalesRecordComponent } from './Components/SalesRecord/delete/delete-sales-record.component';
import { EditSalesRecordComponent } from './Components/SalesRecord/edit/edit-sales-record.component';

import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatTableModule } from '@angular/material/table';
import { MatDialogModule } from '@angular/material/dialog';
import { FlexLayoutModule } from '@angular/flex-layout';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
import { MatCardModule } from '@angular/material/card';


import { LoadingService } from './Services/LoadingService';
import { ProductService } from './Services/ProductService';
import { CategoryService } from './Services/CategoryService';
import { AuthService } from './Services/AuthService';
import { SalesRecordService } from './Services/SalesRecordService';

//import { MatMomentDateModule, MAT_MOMENT_DATE_ADAPTER_OPTIONS } from '@angular/material-moment-adapter';

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
    EditCategoryComponent,
    DeleteCategoryComponent,
    CreateSalesRecordComponent,
    IndexSalesRecordComponent,
    DeleteSalesRecordComponent,
    EditSalesRecordComponent,
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
    FlexLayoutModule,
    MatDatepickerModule,
    MatNativeDateModule,
    MatCardModule

  ],
  providers: [DepartmentService, AuthService, LoadingService, ProductService, CategoryService, SalesRecordService],
    /*  { provide: MAT_MOMENT_DATE_ADAPTER_OPTIONS, useValue: { useUtc: true } }*/

  bootstrap: [AppComponent]
})
export class AppModule { }
