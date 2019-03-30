import { Component, OnInit, OnDestroy, ViewChild, ElementRef } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { FilmsService } from 'src/app/services/films.service';
import { Film } from '../../models/film.model';
import { Genre } from '../../models/genre.model';
import { FormGroup, Validators, FormBuilder, FormArray, FormControl } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { debounceTime } from 'rxjs/operators';
import { Subject } from 'rxjs';
import { CanComponentDeactivate } from 'src/app/services/can-deactivate-guard.service';
import { ErrorMessageService } from 'src/app/services/error-message.service';
import { HttpErrorResponse, HttpResponse } from '@angular/common/http';

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
    
    get genresFormArray(): FormArray{
      return this.editFimlForm.get('genres') as FormArray;
  }

  ngOnInit() {
    this.film.id = this.route.snapshot.params['id'];

    this.editFimlForm = this.formBuilder.group({
      name: ['', [Validators.required]],
      description: ['', [Validators.required]],
      posterURL: ['', [Validators.required]],
      imDbRaiting: ['', [Validators.required]],
      date: ['', [Validators.required]],
      genres: this.formBuilder.array([])
    });

    this.filmsService.get(this.route.snapshot.params['id']).subscribe( 
      (film: Film) => {
        console.log(film);
        this.editFimlForm.controls['name'].setValue(film.name);
        this.editFimlForm.controls['description'].setValue(film.description);
        this.editFimlForm.controls['posterURL'].setValue(film.posterURL);
        this.editFimlForm.controls['imDbRaiting'].setValue(film.imDbRaiting);
        this.editFimlForm.controls['date'].setValue(film.date.substring(0,10));

        film.genres.forEach(genre => {
          this.addGenre(genre);
        });
    });

    this.eventEmitter
      .pipe(debounceTime(500))
      .subscribe(
        (film: Film) => {
          this.filmsService.updateFilm(film)
          .subscribe(
            (response: HttpResponse<any>) => {
              console.log(response);
              this.toastr.success(`${response.body.data.name} updated!`);
              this.editFimlForm.markAsPristine();
            },
            (error: HttpErrorResponse) => {
              console.log(error);
              this.errorMessageService.sendError(error, 'Update film error');
            });
        }
    );
  }

  addGenre(genre: Genre){
    let fg = this.formBuilder.group(genre);
    this.genresFormArray.push(fg);	  
  }

  onGenreDelete(event) {
    const index = this.genresFormArray.controls.indexOf(event);
    this.genresFormArray.removeAt(index);
  }

  onGenreAdd(value) {
    let flag = true;
    this.genresFormArray.controls.forEach((elem: FormGroup) => {
      if(elem.controls['genre'].value == value){
        this.toastr.error('Already exists', 'Genre');
        flag = false;
      }
    });

    if(flag){
      this.addGenre({genre: value} as Genre);
    }
  }
  
  onSubmit() {
    const film = {
      id: this.film.id,
      name: this.editFimlForm.controls['name'].value,
      description: this.editFimlForm.controls['description'].value,
      posterURL: this.editFimlForm.controls['posterURL'].value,
      imDbRaiting: this.editFimlForm.controls['imDbRaiting'].value,
      date: this.editFimlForm.controls['date'].value,
      genres: this.genresFormArray.value
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
