import { TestBed } from '@angular/core/testing';
import { HttpClient} from '@angular/common/http';
import { FilmsService } from './films.service';
import { ExternalService } from './external.service';

class FakeHttpClient {

}

class FakeExternalService {

}

describe('FilmsService', () => {
  beforeEach(() => TestBed.configureTestingModule({
    providers: [
      FilmsService,
      ExternalService,
      { provide: HttpClient, useClass: FakeHttpClient },
    ]
  }));

  it('should be created', () => {
    const service: FilmsService = TestBed.get(FilmsService);
    expect(service).toBeTruthy();
  });
});
