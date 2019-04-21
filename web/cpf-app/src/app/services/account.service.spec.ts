import { TestBed } from '@angular/core/testing';
import { AccountService } from './account.service';
import { HttpClient} from '@angular/common/http';
import { ExternalService } from './external.service';
import { Router } from '@angular/router';
import { ErrorMessageService } from './error-message.service';

class FakeRouter {
}

class FakeHttpClient {
}

class FakeExternalService {
}

class FakeErrorMessageService {
}

describe('AccountService', () => {
  let service: AccountService;

  beforeEach(() => {
  TestBed.configureTestingModule({
    imports: [],
    providers: [
      AccountService, 
      HttpClient, 
      ExternalService, 
      ErrorMessageService,
      { provide: Router, useValue: FakeRouter },
      { provide: ErrorMessageService, useValue: FakeExternalService },
      { provide: HttpClient, useValue: FakeHttpClient },
      { provide: ExternalService, useValue: FakeErrorMessageService },
    ]
  })
  service = TestBed.get(AccountService);
});

  it('should be created', () => {
    expect(service).toBeTruthy();
  });


  describe("", () => {

  });
});
