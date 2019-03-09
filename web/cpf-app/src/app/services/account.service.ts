import { Injectable } from '@angular/core';
import { HttpHeaders } from '@angular/common/http';
import { HttpClient} from '@angular/common/http';
import { ExternalService } from './external.service';
import { Router } from '@angular/router';


@Injectable()
export class AccountService {

  httpHeaders = new HttpHeaders({
    'Content-Type' : 'application/json; charset=utf-8'
  }); 
  options = { headers: this.httpHeaders };
  
  constructor(private http: HttpClient,private externalService: ExternalService ,private router: Router){}

  register(data){
    this.http.post(`${this.externalService.getURL()}/registration`, data, this.options)
    .subscribe(
      (data: any) => {
        localStorage.setItem('token', data.access_token);
        localStorage.setItem('user', data.username);
        this.router.navigate(['/']);
      },
      (error: any) => {
        console.log(error);
    });
  }

  login(data){
    this.http.post(`${this.externalService.getURL()}/login`, data, this.options)
    .subscribe(
      (data: any) => {
        localStorage.setItem('token', data.access_token);
        localStorage.setItem('user', data.username);
        this.router.navigate(['/']);
      },
      (error: any) => {
        console.log(error);
    });
  }
}