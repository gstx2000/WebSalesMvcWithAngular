import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DetailsSalesRecordComponent } from './details-sales-record.component';

describe('DetailsSalesRecordComponent', () => {
  let component: DetailsSalesRecordComponent;
  let fixture: ComponentFixture<DetailsSalesRecordComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [DetailsSalesRecordComponent]
    });
    fixture = TestBed.createComponent(DetailsSalesRecordComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
