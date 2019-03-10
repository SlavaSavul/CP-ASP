import { Component, OnInit } from '@angular/core';
import { FilmsService } from 'src/app/services/films.service';
import { Film } from '../models/film.model';

@Component({
  selector: 'app-main-page',
  templateUrl: './main-page.component.html',
  styleUrls: ['./main-page.component.scss']
})
export class MainPageComponent implements OnInit {

  constructor(private filmsService: FilmsService) { }
  films: Film[] = [];
  page = 1;
  limit = 2;

  ngOnInit() {
    this.sendRequest({ page: this.page, limit: this.limit });
  }

  nextPage() {
    this.page += 1;
    this.sendRequest({ page: this.page, limit: this.limit });
  }

  previousPage() {
    this.page -= 1;
    this.sendRequest({ page: this.page, limit: this.limit });
  }

  sendRequest(params: { page: number, limit: number }) {
    this.filmsService.getAll(params).subscribe(
      (data: Film[]) => {
        if(data.length !== 0) {
          this.films = data;
        }
    });
  }
}
