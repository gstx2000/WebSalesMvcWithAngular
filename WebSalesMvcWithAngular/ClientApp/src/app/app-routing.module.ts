import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { HomeComponent } from './Components/home/home.component';

import { IndexDepartmentComponent } from './Components/Department/index/index-department.component';
import { CreateDepartmentComponent } from './Components/Department/create/create-department.component';
import { EditDepartmentComponent } from './Components/Department/edit/edit-department.component';
import { DeleteDepartmentComponent } from './Components/Department/delete/delete-department.component';
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

import { IndexSalesRecordComponent } from './Components/SalesRecord/index/index-sales-record.component';
import { DeleteSalesRecordComponent } from './Components/SalesRecord/delete/delete-sales-record.component';
import { CreateSalesRecordComponent } from './Components/SalesRecord/create/create-sales-record.component';
import { EditSalesRecordComponent } from './Components/SalesRecord/edit/edit-sales-record.component';
import { DetailsSalesRecordComponent } from './Components/SalesRecord/details/details-sales-record.component';


const routes: Routes = [

  { path: '', component: HomeComponent, pathMatch: 'full' },

  //DEPARTMENT ROUTES *----------------------------------------------------------*
  { path: 'departments', component: IndexDepartmentComponent },
  { path: 'departments/create', component: CreateDepartmentComponent },
  { path: 'departments/edit/:id', component: EditDepartmentComponent },
  { path: 'departments/delete/:id', component: DeleteDepartmentComponent },
  { path: 'departments/details/:id', component: DetailsDepartmentComponent },
  // *----------------------------------------------------------------------------*

  //PRODUCT ROUTES *--------------------------------------------------------------*
  { path: 'products', component: IndexProductComponent },
  { path: 'products/create', component: CreateProductComponent },
  { path: 'products/edit/:id', component: EditProductComponent },
  { path: 'products/delete/:id', component: DeleteProductComponent },
  { path: 'products/details/:id', component: DetailsProductComponent },
  // *----------------------------------------------------------------------------*

  //CATEGORY ROUTES *-------------------------------------------------------------*
  { path: 'categories', component: IndexCategoryComponent },
  { path: 'categories/create', component: CreateCategoryComponent },
  { path: 'categories/edit/:id', component: EditCategoryComponent },
  { path: 'categories/delete/:id', component: DeleteCategoryComponent },

  // *------------------------------------------------------------------------------*

  //SALES RECORDS ROUTES *----------------------------------------------------------*
  { path: 'salesRecords', component: IndexSalesRecordComponent },
  { path: 'salesRecords/create', component: CreateSalesRecordComponent },
  { path: 'salesRecords/edit/:id', component: EditSalesRecordComponent },
  { path: 'salesRecords/delete/:id', component: DeleteSalesRecordComponent },
  { path: 'salesRecords/details/:id', component: DetailsSalesRecordComponent }
  // *------------------------------------------------------------------------------*

];

@NgModule({
  imports: [RouterModule.forRoot(routes, { useHash: true }),],
  exports: [RouterModule],
})
export class AppRoutingModule { }
