import { Injectable } from '@angular/core';
import { HttpClient} from '@angular/common/http';
import { ExternalService } from './external.service';

@Injectable({
  providedIn: 'root'
})
export class FilmsService {

  constructor( 
    private http: HttpClient,
    private externalService: ExternalService
  ) { }

  getAll(params) {
   return this.http.get(`${this.externalService.getURL()}/api/films`, { params });
  }
}
