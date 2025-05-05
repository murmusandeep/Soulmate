import { Component, inject, OnInit } from '@angular/core';
import { NavigationStart, Router, RouterOutlet } from '@angular/router';
import { NavComponent } from "./components/nav/nav.component";
import { AccountService } from './_services/account.service';
import { ToastModule } from 'primeng/toast';
import { NgxSpinnerComponent } from 'ngx-spinner';
import { filter } from 'rxjs';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, NavComponent, ToastModule, NgxSpinnerComponent],
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss'
})
export class AppComponent implements OnInit {

  private accountService = inject(AccountService);
  private router = inject(Router);

  ngOnInit() {
    this.setCurrentUser();
  }

  setCurrentUser() {
    const userString = localStorage.getItem('user');
    if (!userString) return;
    const user = JSON.parse(userString);
    this.accountService.setCurrentUser(user);
    if (this.router.url === '/' && window.location.pathname === '/') {
      this.router.navigateByUrl('/members');
    }
  }
}
