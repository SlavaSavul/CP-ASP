import { Component } from '@angular/core';
import { HttpClient} from '@angular/common/http';
import { HttpHeaders } from '@angular/common/http'; 
import { AuthInterceptor } from '../../services/auth-Interceptor.service';
import { AccountService } from '../../services/account.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {
  title = 'cpf-app';
  token: string;
  
  constructor(
    private accountService: AccountService
  ){}

  // onClick() {
  //   console.log('asdads');
  //   const header = new Headers({
  //     Authorization: `Bearer ${this.token}`
  //   });
  //   const headers = { header };
  //   this.http.get('http://localhost:52281/api/values/getlogin').subscribe((data: any) => console.log(data));
  // }

  // onClick2(){
  //   this.accountService.register(
  //     {
  //       Email: "slaaa@mail.ru",
  //       Password: "123a3"
  //     });
  // }
  // onClick3(){
  //   this.accountService.login(
  //     {
  //       Email: "slaaa@mail.ru",
  //       Password: "12312ad3"
  //     });
  // }
}
