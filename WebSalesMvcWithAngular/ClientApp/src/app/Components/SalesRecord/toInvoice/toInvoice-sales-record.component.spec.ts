import { ComponentFixture, TestBed } from '@angular/core/testing';

import { toInvoiceSalesRecordComponent } from './toInvoice-sales-record.component';

describe('IndexSalesRecordComponent', () => {
  let component: toInvoiceSalesRecordComponent;
  let fixture: ComponentFixture<toInvoiceSalesRecordComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [toInvoiceSalesRecordComponent]
    });
    fixture = TestBed.createComponent(toInvoiceSalesRecordComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
