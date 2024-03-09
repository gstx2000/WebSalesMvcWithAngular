import { Component, OnInit } from '@angular/core';
import { Department } from '../../../Models/Department';
import { DepartmentService } from '../../../Services/DepartmentService';
import { LoadingService } from '../../../Services/LoadingService';
import { MatDialog } from '@angular/material/dialog';
import { DeleteDepartmentComponent } from '../delete/delete-department.component';

@Component({
  selector: 'app-departments',
  templateUrl: './index-department.component.html',
  styleUrls: ['./index-department.component.css']
})
export class IndexDepartmentComponent implements OnInit {
  departments: Department[] = [];
  constructor(private departmentService: DepartmentService,
    private loadingService: LoadingService,
    private dialog: MatDialog) { }

  ngOnInit(): void {
    this.loadDepartments();
  }

  loadDepartments(): void {
    this.loadingService.showLoading();

    this.departmentService.getDepartments().subscribe(
      (result: Department[]) => {
        if (Array.isArray(result)) {
          this.departments = result;
        }
      },
      (error) => {
        console.error('Erro ao carregar departamentos:', error);
        this.loadingService.hideLoading();
      },
      () => {
        this.loadingService.hideLoading();
      }
    );
  }

  openDeleteDialog(department: Department): void {
    const dialogRef = this.dialog.open(DeleteDepartmentComponent, {
      data: { department },
      width: '550px',
      height: '350px'
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result && result.deleted) {
        this.departments = this.departments.filter(p => p.id !== department.id);
        this.loadDepartments();
      }
    });
  }
}
