import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Router } from '@angular/router';
import { Department } from '../../../Models/Department';
import { DepartmentService } from '../../../Services/DepartmentService';
import { LoadingService } from '../../../Services/LoadingService';

@Component({
  selector: 'app-departments/details',
  templateUrl: './details-department.component.html',
  styleUrls: ['./details-department.component.css']
})
export class DetailsDepartmentComponent implements OnInit{
  department: Department | undefined;

  constructor(
    private activedroute: ActivatedRoute,
    private router: Router,
    private departmentService: DepartmentService,
    private loadingService: LoadingService
  ) { }

  ngOnInit(): void {
    const departmentId = Number(this.activedroute.snapshot.params['id']);
    this.loadDepartment(departmentId);
  }

  async loadDepartment(id: number): Promise<void> {
    this.loadingService.showLoading();
    const departmentId = Number(this.activedroute.snapshot.params['id']);
    (await this.departmentService.getDepartmentById(departmentId)).subscribe((result: Department) => {
      this.department = result;
      this.loadingService.hideLoading();

    },
      (error) => {
        console.error('Erro ao carregar departamento:', error);
      }
    );
  }

  backToIndex() {
    this.router.navigate(['/departments']);
  }
}
