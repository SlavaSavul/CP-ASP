import { TestBed } from '@angular/core/testing';

import { ErrorMessageService } from './error-message.service';
import { ToastrService, ToastrModule } from 'ngx-toastr';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { BrowserModule } from '@angular/platform-browser';

describe('ErrorMessageService', () => {
  beforeEach(() => TestBed.configureTestingModule({
    providers: [ToastrService, ErrorMessageService],
    imports: [ BrowserModule, ToastrModule.forRoot(), BrowserAnimationsModule]
  }));

  it('should be created', () => {
    const service: ErrorMessageService = TestBed.get(ErrorMessageService);
    expect(service).toBeTruthy();
  });
});
