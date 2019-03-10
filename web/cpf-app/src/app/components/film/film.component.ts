import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { FilmsService } from 'src/app/services/films.service';
import { Film } from '../models/film.model';

@Component({
  selector: 'app-film',
  templateUrl: './film.component.html',
  styleUrls: ['./film.component.scss']
})
export class FilmComponent implements OnInit {
  film = new Film();

  constructor(
    private route: ActivatedRoute,
    private filmsService: FilmsService) { }

  ngOnInit() {    
    this.filmsService.get(this.route.snapshot.params['id']).subscribe( 
      (data: Film) => this.film = data
    );
  }

}
