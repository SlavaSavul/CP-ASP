import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './components/app/app.component';
import { HttpClientModule, HTTP_INTERCEPTORS }   from '@angular/common/http';
import { AuthInterceptor } from './services/auth-Interceptor.service';
import { AccountService } from './services/account.service';
import { RegistrationComponent } from './components/registration/registration.component';
import { LoginComponent } from './components/login/login.component';
import { MainPageComponent } from './components/main-page/main-page.component';
import { ReactiveFormsModule }   from '@angular/forms';
import { ExternalService } from './services/external.service';
import { ToastrModule } from 'ngx-toastr';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { CommonModule } from '@angular/common';
import { PageNotFoundComponent } from './components/page-not-found/page-not-found.component';
import { FilmsService } from './services/films.service';
import { FilmComponent } from './components/film/film.component';
import { CreateFilmComponent } from './components/create-film/create-film.component';
import { EditFilmComponent } from './components/edit-film/edit-film.component';
import { CanDeactivateGuard } from './services/can-deactivate-guard.service';
import { CanActivateGuard } from './services/can-activate-guard.service';
import { ErrorMessageService } from './services/error-message.service';

export const httpInterceptorProviders = [
  { provide: HTTP_INTERCEPTORS, useClass: AuthInterceptor, multi: true },
];

@NgModule({
  declarations: [
    AppComponent,
    RegistrationComponent,
    LoginComponent,
    MainPageComponent,
    PageNotFoundComponent,
    FilmComponent,
    CreateFilmComponent,
    EditFilmComponent,
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    ReactiveFormsModule,
    BrowserAnimationsModule,
    CommonModule,
    ToastrModule.forRoot(),
  ],
  providers: [
    httpInterceptorProviders,
    AuthInterceptor,
    AccountService,
    ExternalService,
    FilmsService,
    CanDeactivateGuard,
    CanActivateGuard,
    ErrorMessageService
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
