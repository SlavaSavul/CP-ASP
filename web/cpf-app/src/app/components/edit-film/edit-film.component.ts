import { Component, OnInit, OnDestroy, ViewChild, ElementRef } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { FilmsService } from 'src/app/services/films.service';
import { Film } from '../../models/film.model';
import { Genre } from '../../models/genre.model';
import { FormGroup, Validators, FormBuilder, FormArray, FormControl } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { debounceTime } from 'rxjs/operators';
import { Subject, Observable } from 'rxjs';
import { CanComponentDeactivate } from '../../services/can-deactivate-guard.service';
import { ErrorMessageService } from '../../services/error-message.service';
import { HttpErrorResponse, HttpResponse } from '@angular/common/http';
import { FilmFormComponent } from '../film-form/film-form.component';

@Component({
  selector: 'app-edit-film',
  templateUrl: './edit-film.component.html',
  styleUrls: ['./edit-film.component.scss']
})
export class EditFilmComponent implements OnInit, CanComponentDeactivate, OnDestroy {
  film: Film = new Film();
  eventEmitter = new Subject();
  film$: Observable<Object>;
  @ViewChild('filmForm') filmForm: FilmFormComponent;
  
  constructor(
    private route: ActivatedRoute,
    private filmsService: FilmsService,
    private toastr: ToastrService,
    private errorMessageService: ErrorMessageService
    ) { }

  ngOnInit() {
    this.film$ = this.filmsService.get(this.route.snapshot.params['id']);
  }

  canDeactivate() {
   return this.filmForm.canDeactivate();
  }

  onSave(event) {
    this.filmsService.updateFilm(event)
          .subscribe(
            (response: HttpResponse<any>) => {
              console.log(response);
              this.toastr.success(`${response.body.data.name} updated!`);
              this.filmForm.markAsPristine();
            },
            (error: HttpErrorResponse) => {
              console.log(error);
              this.errorMessageService.sendError(error, 'Update film error');
            });
    }

  ngOnDestroy(){
    this.eventEmitter.unsubscribe();
  }
}
