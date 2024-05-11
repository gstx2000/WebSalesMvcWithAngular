import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SupplierIndexComponent } from './index-supplier.component';

describe('IndexSupplierComponent', () => {
  let component: SupplierIndexComponent;
  let fixture: ComponentFixture<SupplierIndexComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [SupplierIndexComponent]
    });
    fixture = TestBed.createComponent(SupplierIndexComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
