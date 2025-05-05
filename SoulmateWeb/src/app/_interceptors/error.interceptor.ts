import { HttpErrorResponse, HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { MessageService } from 'primeng/api';
import { NavigationExtras, Router } from '@angular/router';
import { catchError, throwError } from 'rxjs';

export const ErrorInterceptor: HttpInterceptorFn = (req, next) => {
  const router = inject(Router);
  const messageService = inject(MessageService);

  return next(req).pipe(
    catchError((error: HttpErrorResponse) => {
      if (error) {
        switch (error.status) {
          case 400:
            if (error.error.errors) {
              const modelStateErrors = [];
              for (const key in error.error.errors) {
                if (error.error.errors[key]) {
                  modelStateErrors.push(error.error.errors[key]);
                }
              }
              throw modelStateErrors.flat();
            } else {
              messageService.add({ severity: 'error', summary: 'Error', detail: error.error });
            }
            break;
          case 401:
            messageService.add({ severity: 'error', summary: error.error.Message, detail: 'Unauthorized' });
            break;
          case 404:
            router.navigateByUrl('/not-found');
            break;
          case 500:
            const navigationExtras: NavigationExtras = { state: { error: error.error } };
            router.navigateByUrl('/server-error', navigationExtras);
            break;
          default:
            messageService.add({ severity: 'error', summary: 'Error', detail: 'Something unexpected went wrong' });
            break;
        }
      }
      return throwError(() => error);
    })
  );
};