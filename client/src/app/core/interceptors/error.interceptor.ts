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
        if (!req.url.includes('login') && !req.url.includes('/token/refresh')) {
          // CALL REFRESH TOKEN API
          return of(authService.isRefreshingToken()).pipe(
            map(() => {
              authService.startRefreshingToken();
              return authService.isRefreshingToken()
            }),
            tap(() => { console.log(authService.isRefreshingToken()) }),
            filter((isRefreshing: boolean) => isRefreshing),
            switchMap(() => authService.refreshToken()),
            switchMap((token: string) => authService.getCurrentUser(token)),
            switchMap(() => {
              // clone the original request with the new token
              const updatedReq = req.clone({
                setHeaders: {
                  Authorization: `Bearer ${authService.user().token}`
                }
              })
              authService.completeRefreshToken();
              // return the original request
              return next(updatedReq);
            })
          )
        } else if (req.url.includes('/token/refresh')) {
          snackbar.error(error.error.message || error.error);
          authService.logout();
          router.navigateByUrl('/login');
        } else {
          snackbar.error(error.error.message || error.error);
        }
      }
      else if (error.status === 404) {
        router.navigateByUrl('/not-found');
      }
      if (error.status === 500) {
        const navigationExtras: NavigationExtras = { state: { error: error.error } }
        router.navigateByUrl('/server-error', navigationExtras);
      }
      return throwError(() => error);
    })
  );
};
