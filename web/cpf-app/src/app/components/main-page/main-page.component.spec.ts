import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { MainPageComponent } from './main-page.component';
import { ActivatedRoute, RouterModule, Router } from '@angular/router';
import { FilmsService } from 'src/app/services/films.service';
import { FormBuilder, FormsModule, ReactiveFormsModule } from '@angular/forms';
import { Observable, of, throwError } from 'rxjs'; 
import { PaginatorModule } from 'primeng/paginator';
import {PanelModule} from 'primeng/panel';

class FakeFilmsService {
  get(){
    return new Observable();
  };
  updateFilm() {
    return new Observable();
  }
  getComments(){
    return new Observable();
  }
  createComment(value) {
    return new Observable();
  }
  getGenres() {
    return new Observable();
  }
  getAll() {
    return new Observable();
  }
  delete() {
    return new Observable();
  };
}

describe('MainPageComponent', () => {
  let component: MainPageComponent;
  let fixture: ComponentFixture<MainPageComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      imports: [ReactiveFormsModule, PaginatorModule, RouterModule],
      declarations: [ MainPageComponent ],
      providers: [
        FormBuilder,
        { provide: FilmsService, useClass: FakeFilmsService },
        {
          provide: ActivatedRoute,
          useValue: { params: of({}) }
        },
        {
          provide: Router,
          useValue: { navigate: () => {} }
        }
      ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(MainPageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('', () => {
    spyOn(TestBed.get(FilmsService), 'delete').and.returnValue(of({}));
    spyOn(component, 'sendRequest');

    component.delete('id1');
    
    expect(component.sendRequest).toHaveBeenCalled();
  });

  it('', () => {
    component.metaData = {limit: 1};

    expect(component.getLimit()).toEqual(1);
  });

  it('', () => {
    component.metaData = {count: 1};
    
    expect(component.getCount()).toEqual(1);
  });
  it('', () => {
    spyOn(TestBed.get(Router), 'navigate');

    component.paginate({});

    expect(TestBed.get(Router).navigate).toHaveBeenCalled();
  });
});
