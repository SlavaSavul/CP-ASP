import { Component, OnInit } from '@angular/core';
import { FilmsService } from 'src/app/services/films.service';
import { Film } from '../../models/film.model';
import { RouterStateSnapshot, ActivatedRoute, Router } from '@angular/router';
import { AccountService } from 'src/app/services/account.service';

@Component({
  selector: 'app-main-page',
  templateUrl: './main-page.component.html',
  styleUrls: ['./main-page.component.scss']
})
export class MainPageComponent implements OnInit {

  constructor(
    private filmsService: FilmsService, 
    private activatedRoute: ActivatedRoute,
    private router: Router,
    private accountService: AccountService
    ) { }
  films: Film[] = [];
  page = 1;
  limit = 2;
  metaData: any;

  counterPage() {
    if(this.metaData){
      return new Array(Math.round(this.lastPage));
    }
    return new Array();
  }

  get nextPage() {
    return +this.page + 1;
  }

  get previousPage() {
    return +this.page - 1;
  }

  get lastPage() {
    return this.metaData ? this.metaData.count / this.metaData.limit : 0;
  }

  ngOnInit() {
    this.activatedRoute.params.subscribe(
      (params) => {
        this.page = +params['page'];
        this.sendRequest({ page: this.page, limit: this.limit });
    });    
  }

  sendRequest(params: { page: number, limit: number }) {
    this.filmsService.getAll(params).subscribe(
      (data: any) => {
        if(data.films.length !== 0) {
          this.films = data.films;
          this.metaData = data.metaData;
        }
      },
      (error: any) => {
        this.router.navigate(['/notfound']);
      }
    );
  }
}
