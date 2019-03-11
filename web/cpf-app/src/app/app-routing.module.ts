import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { RegistrationComponent } from './components/registration/registration.component';
import { LoginComponent } from './components/login/login.component';
import { MainPageComponent } from './components/main-page/main-page.component';
import { PageNotFoundComponent } from './components/page-not-found/page-not-found.component';
import { FilmComponent } from './components/film/film.component';
import { CreateFilmComponent } from './components/create-film/create-film.component';
import { EditFilmComponent } from './components/edit-film/edit-film.component';

const routes: Routes = [
  { path: 'signup', component: RegistrationComponent },
  { path: 'signin', component: LoginComponent },
  { path: 'film/:id', component: FilmComponent },
  { path: 'createfilm', component: CreateFilmComponent },
  { path: 'editfilm/:id', component: EditFilmComponent },
  { path: '', component: MainPageComponent },
  { path: '**', component: PageNotFoundComponent },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
