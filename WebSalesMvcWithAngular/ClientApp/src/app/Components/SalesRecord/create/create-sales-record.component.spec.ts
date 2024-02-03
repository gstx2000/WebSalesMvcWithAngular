import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CreateSalesRecordComponent } from './create-sales-record.component';

describe('CreateSalesRecordComponent', () => {
  let component: CreateSalesRecordComponent;
  let fixture: ComponentFixture<CreateSalesRecordComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [CreateSalesRecordComponent]
    });
    fixture = TestBed.createComponent(CreateSalesRecordComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
