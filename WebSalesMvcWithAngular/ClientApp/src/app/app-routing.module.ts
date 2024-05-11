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
import { toInvoiceSalesRecordComponent } from './Components/SalesRecord/toInvoice/toInvoice-sales-record.component';

import { AuthGuard } from './AuthGuard/auth.guard';
import { RegisterComponent } from './Components/UserLogin/Register/register.component';
import { LoginComponent } from './Components/UserLogin/Login/login.component';
import { PasswordRecoveryComponent } from './Components/UserLogin/PasswordRecovery/PasswordRecovery.component';
import { RedefinePasswordComponent } from './Components/UserLogin/PasswordRecovery/RedefinePassword/redefine-password.component';
import { InventoryManagementComponent } from './Components/Product/inventoryManagement/inventory-management.component';

import { ShippingComponent } from './Components/Shipping/shipping.component';
import { CreateSupplierComponent } from './Components/Supplier/create/create-supplier.component';
import { IndexSupplierComponent } from './Components/Supplier/index/index-supplier.component';
import { DetailsSupplierComponent } from './Components/Supplier/details/details-supplier.component';



const routes: Routes = [
  { path: '', redirectTo: '/login', pathMatch: 'full' },

  // Home route
  { path: 'home', component: HomeComponent, canActivate: [AuthGuard] },

  // Department routes
  { path: 'departments', component: IndexDepartmentComponent, canActivate: [AuthGuard] },
  { path: 'departments/create', component: CreateDepartmentComponent, canActivate: [AuthGuard] },
  { path: 'departments/edit/:id', component: EditDepartmentComponent, canActivate: [AuthGuard] },
  { path: 'departments/delete/:id', component: DeleteDepartmentComponent, canActivate: [AuthGuard] },
  { path: 'departments/details/:id', component: DetailsDepartmentComponent, canActivate: [AuthGuard] },

  // Product routes
  { path: 'products', component: IndexProductComponent, canActivate: [AuthGuard] },
  { path: 'products/create', component: CreateProductComponent, canActivate: [AuthGuard] },
  { path: 'products/edit/:id', component: EditProductComponent, canActivate: [AuthGuard] },
  { path: 'products/delete/:id', component: DeleteProductComponent, canActivate: [AuthGuard] },
  { path: 'products/details/:id', component: DetailsProductComponent, canActivate: [AuthGuard] },
  { path: 'products/inventory-management', component: InventoryManagementComponent, canActivate: [AuthGuard] },

  // Category routes
  { path: 'categories', component: IndexCategoryComponent, canActivate: [AuthGuard] },
  { path: 'categories/create', component: CreateCategoryComponent, canActivate: [AuthGuard] },
  { path: 'categories/edit/:id', component: EditCategoryComponent, canActivate: [AuthGuard] },
  { path: 'categories/delete/:id', component: DeleteCategoryComponent, canActivate: [AuthGuard] },

  // Sales record routes
  { path: 'salesRecords', component: IndexSalesRecordComponent, canActivate: [AuthGuard] },
  { path: 'salesRecords/create', component: CreateSalesRecordComponent, canActivate: [AuthGuard] },
  { path: 'salesRecords/edit/:id', component: EditSalesRecordComponent, canActivate: [AuthGuard] },
  { path: 'salesRecords/delete/:id', component: DeleteSalesRecordComponent, canActivate: [AuthGuard] },
  { path: 'salesRecords/details/:id', component: DetailsSalesRecordComponent, canActivate: [AuthGuard] },
  { path: 'salesRecords/toInvoice', component: toInvoiceSalesRecordComponent, canActivate: [AuthGuard] },

  // Login routes
  { path: 'login', component: LoginComponent },
  { path: 'register', component: RegisterComponent },
  { path: 'password-recovery', component: PasswordRecoveryComponent },
  { path: 'redefine-password', component: RedefinePasswordComponent },

  // Shipping routes
  { path: 'shipping', component: ShippingComponent },


  // Supplier routes
  { path: 'suppliers/create', component: CreateSupplierComponent },
  { path: 'suppliers', component: IndexSupplierComponent },
  { path: 'suppliers/details/:id', component: DetailsSupplierComponent }



];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
