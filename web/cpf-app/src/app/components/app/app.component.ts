import { Component, OnInit } from '@angular/core';
import { HttpClient} from '@angular/common/http';
import { HttpHeaders } from '@angular/common/http'; 
import { AuthInterceptor } from '../../services/auth-Interceptor.service';
import { AccountService } from '../../services/account.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {
  constructor(
    public accountService: AccountService
  ){}

  ngOnInit() {
    this.accountService.checkLogin();
  }
}
