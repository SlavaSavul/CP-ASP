import { Component, OnInit, OnDestroy } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { FilmsService } from 'src/app/services/films.service';
import { Film } from '../../models/film.model';
import { FormGroup, Validators, FormBuilder } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { debounceTime } from 'rxjs/operators';
import { Subject } from 'rxjs';
import { CanComponentDeactivate } from 'src/app/services/can-deactivate-guard.service';
import { ErrorMessageService } from 'src/app/services/error-message.service';

@Component({
  selector: 'app-edit-film',
  templateUrl: './edit-film.component.html',
  styleUrls: ['./edit-film.component.scss']
})
export class EditFilmComponent implements OnInit, CanComponentDeactivate, OnDestroy {
  film: Film = new Film();
  editFimlForm: FormGroup;
  eventEmitter = new Subject();

  constructor(
    private route: ActivatedRoute,
    private filmsService: FilmsService,
    private formBuilder: FormBuilder,
    private toastr: ToastrService,
    private errorMessageService: ErrorMessageService
    ) { }
    
  ngOnInit() {
    this.film.id = this.route.snapshot.params['id'];

    this.filmsService.get(this.route.snapshot.params['id']).subscribe( 
      (data: Film) => {
        this.editFimlForm = this.formBuilder.group({
          name: [data.name, [Validators.required]],
          description: [data.description, [Validators.required]],
          posterURL: [data.posterURL, [Validators.required]],
        });
    });

    this.eventEmitter
      .pipe(debounceTime(500))
      .subscribe(
        (film: Film) => {
          this.filmsService.updateFilm(film)
          .subscribe(
            (response: any) => {
              this.toastr.success(`${response.data.name} updated!`);
              this.editFimlForm.markAsPristine();
            },
            (response) => {
              this.errorMessageService.sendError(response, 'Update film error');
            });
        }
    );
  }

  onSubmit() {
    const film = {
      id: this.film.id,
      name: this.editFimlForm.controls['name'].value,
      description: this.editFimlForm.controls['description'].value,
      posterURL: this.editFimlForm.controls['posterURL'].value
    } as Film;

    this.eventEmitter.next(film);
  }

  canDeactivate() {
    if(this.editFimlForm.dirty) {

      return confirm('Discard changes for Film?');
    }

    return true;
  }

  ngOnDestroy(){
    this.eventEmitter.unsubscribe();
  }
}
