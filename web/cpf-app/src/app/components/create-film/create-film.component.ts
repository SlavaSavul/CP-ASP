import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { FilmsService } from 'src/app/services/films.service';
import { Film } from '../models/film.model';
import { ToastrService } from 'ngx-toastr';
import { Observable, Subject } from 'rxjs';
import { debounceTime, map } from 'rxjs/operators';

@Component({
  selector: 'app-create-film',
  templateUrl: './create-film.component.html',
  styleUrls: ['./create-film.component.scss']
})
export class CreateFilmComponent implements OnInit {
  createFimlForm: FormGroup;
  eventEmiter = new Subject();

  constructor(
    private formBuilder: FormBuilder, 
    private filmService: FilmsService,
    private toastr: ToastrService
    ) { }

  ngOnInit() {
    this.createFimlForm = this.formBuilder.group({
      name: ['', [Validators.required]],
      description: ['', [Validators.required]],
      posterURL: ['', [Validators.required]],
    });

    this.eventEmiter
      .pipe(debounceTime(500))
      .subscribe(
        (film: Film) => {
          this.filmService.createFilm(film)
          .subscribe((response: any) => {
              if(response.data){
                this.toastr.success('Created');
              }
              else if(response.error){
                this.toastr.error(response.error.message);
              }
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

    this.eventEmiter.next(film);
  }
}
