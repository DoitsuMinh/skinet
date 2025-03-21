import { ActivatedRouteSnapshot, CanActivateFn, Router, RouterStateSnapshot } from '@angular/router';
import { AuthService } from '../services/auth.service';
import { inject } from '@angular/core';

export const authGuard: CanActivateFn =
  (route: ActivatedRouteSnapshot, state: RouterStateSnapshot) => {
    const authService = inject(AuthService);
    const router = inject(Router);

    // First check if user is logged in
    if (authService.isLoggedIn()) {
      return true;
    }

    router.navigate(['account/login']);
    return false;

    // // If not logged in, but we have a refresh token in cookie, try to refresh
    // return authService.refreshToken().pipe(
    //   switchMap((token: string) => {
    //     // After getting a new token, fetch current user
    //     return authService.getCurrentUser(token);
    //   }),
    //   map(() => {
    //     // If refresh and user fetch succeeded, allow navigation
    //     return true;
    //   }),
    //   catchError(() => {
    //     console.log('auth error')
    //     router.navigate(['/login']);
    //     return of(false);
    //   })
    // );
  };
