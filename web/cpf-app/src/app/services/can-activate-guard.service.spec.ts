import { TestBed } from '@angular/core/testing';

import { CanActivateGuard } from './can-activate-guard.service';

describe('CanActivateGuard', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: CanActivateGuard = TestBed.get(CanActivateGuard);
    expect(service).toBeTruthy();
  });
});
