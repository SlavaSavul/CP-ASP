import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { AccountService } from 'src/app/services/account.service';

@Component({
  selector: 'app-registration',
  templateUrl: './registration.component.html',
  styleUrls: ['./registration.component.scss']
})
export class RegistrationComponent implements OnInit {
  registerForm: FormGroup

  constructor(private formBuilder: FormBuilder, private accountService: AccountService) { }

  ngOnInit() {
    this.registerForm = this.formBuilder.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.pattern('^[a-z0-9]{4,8}')]
    });
  }

  onsubmit(){
    this.accountService.register({ 
      email: this.registerForm.controls['email'].value,
      password: this.registerForm.controls['password'].value  
    });
  }
}
