import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DeleteSalesRecordComponent } from './delete-sales-record.component';

describe('DeleteSalesRecordComponent', () => {
  let component: DeleteSalesRecordComponent;
  let fixture: ComponentFixture<DeleteSalesRecordComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [DeleteSalesRecordComponent]
    });
    fixture = TestBed.createComponent(DeleteSalesRecordComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
