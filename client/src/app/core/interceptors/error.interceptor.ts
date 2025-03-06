import { HttpErrorResponse, HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { NavigationExtras, Router } from '@angular/router';
import { SnackbarService } from '../services/snackbar.service';
import { catchError, filter, map, of, switchMap, tap, throwError } from 'rxjs';
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
        if (req.url.includes('/token/refresh')) {
          snackbar.error(error.error.message);

          authService.logout();
          router.navigateByUrl('/login');
        } else {
          snackbar.error(error.error.message);
          // router.navigateByUrl('/login');
        }
      }
      else if (error.status === 404) {
        // router.navigateByUrl('/not-found');
        snackbar.error(error.error.message);
      }
      if (error.status === 500) {
        const navigationExtras: NavigationExtras = { state: { error: error.error } }
        router.navigateByUrl('/server-error', navigationExtras);
      }
      throw error;
    })
  );
};
