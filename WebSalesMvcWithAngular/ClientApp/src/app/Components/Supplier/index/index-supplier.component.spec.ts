import { ComponentFixture, TestBed } from '@angular/core/testing';

import { IndexSupplierComponent } from './index-supplier.component';

describe('IndexSupplierComponent', () => {
  let component: IndexSupplierComponent;
  let fixture: ComponentFixture<IndexSupplierComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [IndexSupplierComponent]
    });
    fixture = TestBed.createComponent(IndexSupplierComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
