import { HttpErrorResponse, HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { NavigationExtras, Router } from '@angular/router';
import { SnackbarService } from '../services/snackbar.service';
import { catchError, throwError } from 'rxjs';
import { AuthService } from '../services/auth.service';

export const errorInterceptor: HttpInterceptorFn = (req, next) => {
  const router = inject(Router);
  const snackbar = inject(SnackbarService);
  const authService = inject(AuthService);

  return next(req).pipe(
    catchError((error: HttpErrorResponse) => {
      if (error.status === 400) {
        if (error.error.errors) {
          const modelStateErrors = [];
          for (const key in error.error.errors) {
            if (error.error.errors[key]) {
              modelStateErrors.push(error.error.errors[key])
            }
          }
          throw modelStateErrors.flat();
        } else {
          snackbar.error(error.error.message || error.error)
        }
      }
      else if (error.status === 401) {
        snackbar.error(error.error.message || error.error);
        if (!req.url.includes('login')) {
          authService.refreshToken();
          router.navigateByUrl('/login');
        }
      }
      else if (error.status === 404) {
        router.navigateByUrl('/not-found');
      }
      else if (error.status === 500) {
        const navigationExtras: NavigationExtras = { state: { error: error.error } }
        router.navigateByUrl('/server-error', navigationExtras);
      }
      return throwError(() => error);
    })
  );
};
