import { Component, OnInit } from '@angular/core';
import { FilmsService } from 'src/app/services/films.service';
import { Film } from '../../models/film.model';
import { RouterStateSnapshot, ActivatedRoute, Router } from '@angular/router';
import { AccountService } from 'src/app/services/account.service';
import { HttpErrorResponse, HttpResponse } from '@angular/common/http';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { Genre } from 'src/app/models/genre.model';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-films',
  templateUrl: './films.component.html',
  styleUrls: ['./films.component.scss']
})
export class FilmsComponent implements OnInit {
  form: FormGroup;
  films: Film[] = [];
  page = 1;
  limit = 2;
  metaData: any;
  genres: string[] = [];
  selectedGenres:  string[] = [];

  constructor(
    private filmsService: FilmsService, 
    private activatedRoute: ActivatedRoute,
    private router: Router,
    private formBuilder: FormBuilder,
    private accountService: AccountService,
    private toastr: ToastrService,
    ) { }

  ngOnInit() {
   this.filmsService.gerGenres().subscribe( 
     (response: HttpResponse<string[]>) => {
      this.genres = response.body;
     }
   );
   
   this.createForm();

    this.activatedRoute.params.subscribe(
      (params) => {
        this.page = +params['page'];
        if(!this.page) {
          this.page = 1;
        }
        this.sendWithForm(this.page, this.limit);
    });    
  }

  createForm() {
    this.form = this.formBuilder.group({
      name: ['', [
        Validators.maxLength(100)
      ]],
      raiting: ['', [
        Validators.max(10),
        Validators.min(0),
        Validators.pattern('^[0-9]*[.]?[0-9]+$'),
        Validators.maxLength(3)
      ]],
      year: ['', [
        Validators.min(1960),
        Validators.max((new Date()).getFullYear()),
        Validators.pattern('^[0-9]{4}$')
      ]],
    });
  }

  onSubmit() {
    this.sendWithForm(1, this.limit);
  }

  sendWithForm(page: number, limit: number) {
    if(this.form.valid){
      this.sendRequest(page, limit, { 
        year: this.form.controls["year"].value,
        name: this.form.controls["name"].value.trim(),
        raiting: this.form.controls["raiting"].value,
        genres: this.selectedGenres
      });
    }
    else {
    }
  }

  sendRequest(page: number, limit: number, 
    data?: {
      year: number, 
      name: string, 
      raiting: number,
      genres: string[]  
  }) {
    this.searchFilms({ page, limit, ...data });
  }

  searchFilms(params: { 
    page: number, 
    limit: number, 
    year?: number, 
    name?: string, 
    raiting?: number,
    genres?: string[]
  }) {
  this.filmsService.getAll(params).subscribe(
      (response: HttpResponse<any>) => {
        console.log(response);
        if(response.body.films.length !== 0) {
          this.films = response.body.films;
          this.metaData = response.body.metaData;
        }
      },
      (error: HttpErrorResponse) => {
        this.films = [];
        this.metaData = { count: 0, page: 0, limit: 0};
      }
    );
  }

  paginate(event) {
    this.router.navigate([`/films/${event.page + 1}`]);
  }

  getLimit() {
    return this.metaData ? this.metaData.limit : 0;
  }

  getCount() {
    return this.metaData ? this.metaData.count : 0;
  }

  selectGenre(genre: string) {
    if(this.selectedGenres.indexOf(genre) < 0) {
      this.selectedGenres.push(genre);
    }
    else {
      this.selectedGenres.splice(this.selectedGenres.indexOf(genre), 1);
    }
  }

  ifgenreSelected(genre: string) {
    return this.selectedGenres.indexOf(genre) > -1;
  }

  isRaitingValid() {
    return !this.form.controls['raiting'].errors;
  }

  isYearValid() {
    return !this.form.controls['year'].errors;
  }

  delete(id: string) {
    this.filmsService.delete(id).subscribe(
      (response: HttpResponse<any>) => {
        this.sendWithForm(1, this.limit);
      }
    );
  }
}
