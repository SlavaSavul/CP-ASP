import { Injectable } from '@angular/core';
import { HttpHeaders, HttpErrorResponse } from '@angular/common/http';
import { HttpClient} from '@angular/common/http';
import { ExternalService } from './external.service';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { ErrorMessageService } from './error-message.service';


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
    private toastr: ToastrService,
    private errorMessageService: ErrorMessageService
  ){}

  register(data){
    this.http.post(`${this.externalService.getURL()}/registration`, data, this.options)
    .subscribe(
      (response: any) => {
        localStorage.setItem('token', response.data.access_token);
        localStorage.setItem('user', response.data.username);
        localStorage.setItem('role', response.data.role);
        this.router.navigate(['/']);
      },
      (response) => {
        this.errorMessageService.sendError(response, 'Authenticate error');
    });
  }

  login(data){
    this.http.post(`${this.externalService.getURL()}/login`, data, this.options)
    .subscribe(
    (response: any) => {
      localStorage.setItem('token', response.data.access_token);
      localStorage.setItem('user', response.data.username);
      localStorage.setItem('role', response.data.role);
      this.router.navigate(['/']);
    },
    (response) => {
      this.errorMessageService.sendError(response, 'Authenticate error');
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
    localStorage.removeItem('role');
  }

  getEmail() {
    return localStorage.getItem('user');
  }

  getRole() {
    return localStorage.getItem('role');
  }
}