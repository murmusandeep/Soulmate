import { Component, inject, OnInit } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet],
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss'
})
export class AppComponent implements OnInit {

    users: any;
    private httpClient = inject(HttpClient);

    ngOnInit() {
      this.httpClient.get('https://localhost:5001/api/users').subscribe((response) => {
        this.users = response;
      });
    }
}
