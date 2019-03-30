import { Injectable } from '@angular/core';
import { HttpClient} from '@angular/common/http';
import { ExternalService } from './external.service';
import { Film } from '../models/film.model';

@Injectable({
  providedIn: 'root'
})
export class FilmsService {

  constructor( 
    private http: HttpClient,
    private externalService: ExternalService
  ) { }

  getAll(params) {
   return this.http.get(`${this.externalService.getURL()}/api/films`, { params, observe: 'response' });
  }

  get(id: string){
    return this.http.get(`${this.externalService.getURL()}/api/films/${id}`);
  }

  createFilm(film: Film){
    return this.http.post(`${this.externalService.getURL()}/api/films/`, film, { observe: 'response' });
  }

  updateFilm(film: Film){
    return this.http.put(`${this.externalService.getURL()}/api/films/`, film, { observe: 'response' });
  }
}
