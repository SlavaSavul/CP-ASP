import { Component, OnInit, OnDestroy } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { FilmsService } from 'src/app/services/films.service';
import { Film } from '../../models/film.model';
import { ToastrService } from 'ngx-toastr';
import { Observable, Subject } from 'rxjs';
import { debounceTime, map } from 'rxjs/operators';
import { CanComponentDeactivate } from 'src/app/services/can-deactivate-guard.service';
import { ErrorMessageService } from 'src/app/services/error-message.service';

@Component({
  selector: 'app-create-film',
  templateUrl: './create-film.component.html',
  styleUrls: ['./create-film.component.scss']
})
export class CreateFilmComponent implements OnInit, CanComponentDeactivate, OnDestroy {
  createFimlForm: FormGroup;
  eventEmitter = new Subject();

  constructor(
    private formBuilder: FormBuilder, 
    private filmService: FilmsService,
    private toastr: ToastrService,
    private errorMessageService: ErrorMessageService
    ) { }

  ngOnInit() {
    this.createFimlForm = this.formBuilder.group({
      name: ['', [Validators.required]],
      description: ['', [Validators.required]],
      posterURL: ['', [Validators.required]],
    });

    this.eventEmitter
      .pipe(debounceTime(500))
      .subscribe(
        (film: Film) => {
          this.filmService.createFilm(film)
          .subscribe(
            (response: any) => {
              this.toastr.success(`${response.data.name} created!`);
              this.createFimlForm.markAsPristine();
            },
            (response) => {
              this.errorMessageService.sendError(response, 'Create film error');
            });
        }
      );
  }

  onSubmit() {
    const film = {
      name: this.createFimlForm.controls['name'].value,
      description: this.createFimlForm.controls['description'].value,
      posterURL: this.createFimlForm.controls['posterURL'].value
    } as Film;

    this.eventEmitter.next(film);
  }

  canDeactivate() {
    if (this.createFimlForm.dirty) {

      return confirm('Discard changes for Film?');
    }
    return true; 
  }

  ngOnDestroy() {
    this.eventEmitter.unsubscribe();
  }
}
