import { Component, Inject, OnInit } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { DepartmentService } from '../../../Services/DepartmentService';
import { Department } from '../../../Models/Department';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-departments/delete-department',
  templateUrl: './delete-department.component.html',
  styleUrls: ['./delete-department.component.css']
})
export class DeleteDepartmentComponent implements OnInit {
  department?: Department
  constructor(
    public dialogRef: MatDialogRef<DeleteDepartmentComponent>,
    @Inject(MAT_DIALOG_DATA) public data: { department: Department },
    private departmentService: DepartmentService,
    private toastr: ToastrService
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
        this.toastr.success(`Loja ${this.department.name} deletada com sucesso.`);
        this.dialogRef.close({ deleted: true });
      }
    } catch (error: any) {
      this.toastr.error(error.message || 'Erro interno da aplicação, tente novamente.');
      console.error('Erro ao deletar departamento:', error);
    }
  }

  cancel() {
    this.dialogRef.close({ deleted: false });
  }
}
