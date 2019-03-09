import { Injectable } from '@angular/core';
import { HttpHeaders, HttpErrorResponse } from '@angular/common/http';
import { HttpClient} from '@angular/common/http';
import { ExternalService } from './external.service';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';


@Injectable()
export class AccountService {

  httpHeaders = new HttpHeaders({
    'Content-Type' : 'application/json; charset=utf-8'
  }); 
  options = { headers: this.httpHeaders };
  
  constructor(
    private http: HttpClient,
    private externalService: ExternalService,
    private router: Router,
    private toastr: ToastrService
  ){}

  register(data){
    this.http.post(`${this.externalService.getURL()}/registration`, data, this.options)
    .subscribe(
      (response: any) => {
        if(response.error) {
          this.toastr.error(response.error.message, 'Authenticate error');
        } else if(response.data){          
          localStorage.setItem('token', response.data.access_token);
          localStorage.setItem('user', response.data.username);
          this.router.navigate(['/']);
        }
      },
      (error: any) => {
        this.toastr.error(error.message, 'Authenticate error');
    });
  }

  login(data){
    this.http.post(`${this.externalService.getURL()}/login`, data, this.options)
    .subscribe(
      (response: any) => {
        if(response.error) {
          this.toastr.error(response.error.message, 'Authenticate error');
        } else if(response.data){          
          localStorage.setItem('token', response.data.access_token);
          localStorage.setItem('user', response.data.username);
          this.router.navigate(['/']);
        }
      },
      (error: any) => {
        this.toastr.error(error.message, 'Authenticate error');
    });
  }

  isAuthenticated(){
    if(localStorage.getItem('token') !== null){
      return true;
    }
     return false;
  }

  logout(){
    localStorage.removeItem('token');
    localStorage.removeItem('user');
  }

  getEmail() {
    return localStorage.getItem('user');
  }

}