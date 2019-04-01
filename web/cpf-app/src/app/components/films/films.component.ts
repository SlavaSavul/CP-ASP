import { Component, OnInit } from '@angular/core';
import { FilmsService } from 'src/app/services/films.service';
import { Film } from '../../models/film.model';
import { RouterStateSnapshot, ActivatedRoute, Router } from '@angular/router';
import { AccountService } from 'src/app/services/account.service';
import { HttpErrorResponse, HttpResponse } from '@angular/common/http';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';

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

  constructor(
    private filmsService: FilmsService, 
    private activatedRoute: ActivatedRoute,
    private router: Router,
    private formBuilder: FormBuilder,
    private accountService: AccountService
    ) { }

  ngOnInit() {
    this.form = this.formBuilder.group({
      name: ['', [
      //  Validators.maxLength(100)
      ]],
      raiting: ['', [
      //  Validators.max(10),
      //  Validators.min(0)
      ]],
      year: ['', [
       // Validators.min(1960),
      ]],
    });

    this.activatedRoute.params.subscribe(
      (params) => {
        this.page = +params['page'];
        if(!this.page) {
          this.page = 1;
        }
        this.onSubmit(this.page);
    });    
  }

  sendRequest(params: { 
      page: number, 
      limit: number
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
        this.router.navigate(['/notfound']);
      }
    );
  }

  searchFilms(params: { 
    page: number, 
    limit: number, 
    year?: number, 
    name?: string, 
    raiting?: number
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

  onSubmit(page) {
    if(this.form.valid){
      this.searchFilms({ 
        page: page ? this.page : 1, 
        limit: this.limit, 
        year: this.form.controls["year"].value,
        name: this.form.controls["name"].value,
        raiting: this.form.controls["raiting"].value
      });
    }
  }
}
