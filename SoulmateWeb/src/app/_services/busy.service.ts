import { inject, Injectable } from '@angular/core';
import { NgxSpinnerService } from 'ngx-spinner';

@Injectable({
  providedIn: 'root'
})
export class BusyService {

  bustRequestCount = 0;

  private spinner = inject(NgxSpinnerService);

  busy() {
    this.bustRequestCount++;
    this.spinner.show(undefined, {
      type: 'pacman',
      bdColor: 'rgba(255,255,255,0)',
      color: '#333333'
    })
  }

  idle() {
    this.bustRequestCount--;
    if (this.bustRequestCount <= 0) {
      this.bustRequestCount = 0;
      this.spinner.hide();
    }
  }
}
