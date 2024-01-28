import { ComponentFixture, TestBed } from '@angular/core/testing';

import { IndexDepartmentComponent } from './index-department.component';

describe('DepartmentComponentComponent', () => {
  let component: IndexDepartmentComponent;
  let fixture: ComponentFixture<IndexDepartmentComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ IndexDepartmentComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(IndexDepartmentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
