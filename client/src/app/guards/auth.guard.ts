import { ActivatedRouteSnapshot, CanActivateFn, Router, RouterStateSnapshot } from '@angular/router';
import { AuthService } from '../core/services/auth.service';
import { inject } from '@angular/core';
import { catchError, from, map, of, switchMap } from 'rxjs';

export const authGuard: CanActivateFn =
  (route: ActivatedRouteSnapshot, state: RouterStateSnapshot) => {
    const authService = inject(AuthService);
    const router = inject(Router);

    // First check if user is logged in
    if (authService.isLoggedIn()) {
      return true;
    }

    // If not logged in, but we have a refresh token in cookie, try to refresh
    return authService.refreshToken().pipe(
      switchMap((token: string) => {
        // After getting a new token, fetch current user
        return authService.getCurrentUser(token);
      }),
      map(() => {
        // If refresh and user fetch succeeded, allow navigation
        return true;
      }),
      catchError(() => {
        console.log('auth error')
        router.navigate(['/login']);
        return of(false);
      })
    );
  };


/**
* return of(authService.isRefreshingToken()).pipe(
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
*/