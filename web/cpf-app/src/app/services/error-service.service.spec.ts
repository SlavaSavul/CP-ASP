import { TestBed } from '@angular/core/testing';

import { ErrorServiceService } from './error-message.service';

describe('ErrorServiceService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: ErrorServiceService = TestBed.get(ErrorServiceService);
    expect(service).toBeTruthy();
  });
});
