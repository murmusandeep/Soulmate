import { ApplicationConfig, importProvidersFrom, provideZoneChangeDetection } from '@angular/core';
import { provideRouter } from '@angular/router';
import { provideAnimationsAsync } from '@angular/platform-browser/animations/async';
import { provideHttpClient, withFetch, withInterceptors } from '@angular/common/http';
import { providePrimeNG } from 'primeng/config';
import Aura from '@primeng/themes/aura';
import { routes } from './app.routes';
import { MessageService } from 'primeng/api';
import { ErrorInterceptor } from './_interceptors/error.interceptor';
import { jwtInterceptor } from './_interceptors/jwt.interceptor';
import { NgxSpinnerModule } from "ngx-spinner";
import { loadingInterceptor } from './_interceptors/loading.interceptor';
import { TimeagoModule } from "ngx-timeago";

export const appConfig: ApplicationConfig = {
  providers: [
    provideHttpClient(
      withFetch(),
      withInterceptors([ErrorInterceptor, jwtInterceptor, loadingInterceptor])
    ),
    provideAnimationsAsync(),
    providePrimeNG({
      theme: { preset: Aura },
    }),
    provideZoneChangeDetection({ eventCoalescing: true }),
    provideRouter(routes),
    MessageService,
    importProvidersFrom(NgxSpinnerModule, TimeagoModule.forRoot())
  ]
};
