import { Component, inject, OnInit, output } from '@angular/core';
import { ImportsModule } from '../../_shared/imports';
import { AccountService } from '../../_services/account.service';
import { AbstractControl, FormBuilder, FormControl, FormGroup, ValidatorFn, Validators } from '@angular/forms';
import { TextInputComponent } from '../../_forms/text-input/text-input.component';
import { DatePickerComponent } from '../../_forms/date-picker/date-picker.component';
import { Router } from '@angular/router';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [ImportsModule, TextInputComponent, DatePickerComponent],
  templateUrl: './register.component.html',
  styleUrl: './register.component.scss'
})
export class RegisterComponent implements OnInit {

  private accountService = inject(AccountService);
  private fb = inject(FormBuilder);
  private router = inject(Router);
  cancelRegister = output<boolean>();
  maxDate: Date = new Date();
  validationErrors: string[] | undefined;
  registerForm: FormGroup = new FormGroup({});

  ngOnInit() {
    this.initializeForm();
    this.maxDate.setFullYear(this.maxDate.getFullYear() - 18);
  }

  register() {
    const dob = this.getDateOnly(this.registerForm.controls['dateOfBirth'].value);
    this.registerForm.patchValue({ dateOfBirth: dob });
    this.accountService.register(this.registerForm.value).subscribe({
      next: (_) => {
        this.router.navigateByUrl('/members');
      },
      error: (error) => {
        this.validationErrors = error;
      },
    });
  }

  cancel() {
    this.cancelRegister.emit(false);
  }

  initializeForm() {
    this.registerForm = this.fb.group({
      gender: ['male'],
      username: ['', Validators.required],
      knownAs: ['', Validators.required],
      dateOfBirth: ['', Validators.required],
      city: ['', Validators.required],
      country: ['', Validators.required],
      password: ['', [Validators.required, Validators.minLength(4), Validators.maxLength(30)]],
      confirmPassword: ['', [Validators.required, this.matchValues('password')]]
    });
    this.registerForm.controls['password'].valueChanges.subscribe({
      next: () => this.registerForm.controls['confirmPassword'].updateValueAndValidity()
    });
  }

  matchValues(matchTo: string): ValidatorFn {
    return (control: AbstractControl) => {
      return control.value === control.parent?.get(matchTo)?.value ? null : { notMatching: true };
    }
  }

  private getDateOnly(dob: string | undefined) {
    if (!dob) return;
    // The below line converts the date to ISO format and slices it to get only the date part (YYYY-MM-DD).
    return new Date(dob).toISOString().slice(0, 10);
    // let newDob = new Date(dob);
    // return new Date(newDob.setMinutes(newDob.getMinutes() - newDob.getTimezoneOffset())).toISOString().slice(0, 10);
  }
}
