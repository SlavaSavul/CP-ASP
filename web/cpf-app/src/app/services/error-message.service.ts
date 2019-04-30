import { Injectable } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { HttpErrorResponse } from '@angular/common/http';

const errors = {'401' : 'Please autenticate'};

@Injectable()
export class ErrorMessageService {

  constructor(private toastr: ToastrService) { }

  sendError(response: HttpErrorResponse, title: string) {
    if(errors[response.status]) {
      this.toastr.error(errors[response.status], title);
    }
    else if(response.error) {
      this.toastr.error(response.error.message, title);
    }
    else {
      this.toastr.error(response.message, title);
    }
  }
}
