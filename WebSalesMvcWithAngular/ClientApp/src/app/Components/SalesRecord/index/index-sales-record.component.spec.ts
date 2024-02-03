import { ComponentFixture, TestBed } from '@angular/core/testing';

import { IndexSalesRecordComponent } from './index-sales-record.component';

describe('IndexSalesRecordComponent', () => {
  let component: IndexSalesRecordComponent;
  let fixture: ComponentFixture<IndexSalesRecordComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [IndexSalesRecordComponent]
    });
    fixture = TestBed.createComponent(IndexSalesRecordComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
