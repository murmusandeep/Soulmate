import { Component } from '@angular/core';
import { ImportsModule } from '../../_shared/imports';
import { RegisterComponent } from '../register/register.component';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [ImportsModule, RegisterComponent],
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss'] // Fixed typo: styleUrl -> styleUrls
})
export class HomeComponent {
  name: string = '';
  email: string = '';
  password: string = '';
  confirmPassword: string = '';
  accept: boolean = false;
  registerMode: boolean = false; // Added registerToggle property

  registerToggle() {
    this.registerMode = !this.registerMode;
  }

  cancelRegisterMode(event: boolean) {
    this.registerMode = event;
  }

  signUp() {}
}
