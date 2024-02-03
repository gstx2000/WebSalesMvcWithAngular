import { ComponentFixture, TestBed } from '@angular/core/testing';

import { EditSalesRecordComponent } from './edit-sales-record.component';

describe('EditSalesRecordComponent', () => {
  let component: EditSalesRecordComponent;
  let fixture: ComponentFixture<EditSalesRecordComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [EditSalesRecordComponent]
    });
    fixture = TestBed.createComponent(EditSalesRecordComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
