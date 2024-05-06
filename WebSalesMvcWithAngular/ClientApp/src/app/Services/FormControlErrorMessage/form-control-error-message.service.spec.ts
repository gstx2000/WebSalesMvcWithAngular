import { TestBed } from '@angular/core/testing';

import { FormControlErrorMessageService } from './form-control-error-message.service';

describe('FormControlErrorMessageService', () => {
  let service: FormControlErrorMessageService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(FormControlErrorMessageService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
