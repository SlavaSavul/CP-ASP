import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { FilmsService } from 'src/app/services/films.service';
import { Film } from '../models/film.model';
import { FormGroup, Validators, FormBuilder } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { debounceTime } from 'rxjs/operators';
import { Subject } from 'rxjs';

@Component({
  selector: 'app-edit-film',
  templateUrl: './edit-film.component.html',
  styleUrls: ['./edit-film.component.scss']
})
export class EditFilmComponent implements OnInit {
  film: Film = new Film();
  editFimlForm: FormGroup;
  eventEmiter = new Subject();

  constructor(
    private route: ActivatedRoute,
    private filmsService: FilmsService,
    private formBuilder: FormBuilder,
    private toastr: ToastrService
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

    this.eventEmiter
    .pipe(debounceTime(500))
    .subscribe(
      (film: Film) => {
        this.filmsService.updateFilm(film)
        .subscribe(
          (response: any) => {
            this.toastr.success(`${response.data.name} updated!`);
          },
          (response) => {
            this.toastr.error(`${response.error.message}`);
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

    this.eventEmiter.next(film);
  }

}
