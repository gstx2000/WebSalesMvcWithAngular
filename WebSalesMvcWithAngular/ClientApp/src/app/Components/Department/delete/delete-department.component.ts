import { Component, Inject, OnInit } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { DepartmentService } from '../../../Services/DepartmentService';
import { Department } from '../../../Models/Department';

@Component({
  selector: 'app-departments/delete',
  templateUrl: './delete-department.component.html',
  styleUrls: ['./delete-department.component.css']
})
export class DeleteDepartmentComponent implements OnInit {
  department?: Department
  constructor(
    public dialogRef: MatDialogRef<DeleteDepartmentComponent>,
    @Inject(MAT_DIALOG_DATA) public data: { department: Department },
    private departmentService: DepartmentService,
    ) { }

  ngOnInit(): void {
    if (this.data.department && this.data.department.id !== undefined) {
      this.department = this.data.department;
    } 
  }

  async onDeleteClick(): Promise<void> {

    try {
      if (this.department && this.department.id) {
        await (await this.departmentService.deleteDepartment(this.department.id)).toPromise();
        this.dialogRef.close({ deleted: true });
      }
    } catch (error) {
      console.error('Erro ao deletar departamento:', error);
    }
  }

  cancel() {
    this.dialogRef.close({ deleted: false });
  }
}
