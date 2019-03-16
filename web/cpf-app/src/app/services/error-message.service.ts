import { Injectable } from '@angular/core';
import { ToastrService } from 'ngx-toastr';

@Injectable()
export class ErrorMessageService {

  constructor(private toastr: ToastrService) { }

  sendError(response: any, title: string) {
    if(response.error) {
      this.toastr.error(response.error.message, title);
    }
    else {
      this.toastr.error(response.message, title);
    }
  }
}
