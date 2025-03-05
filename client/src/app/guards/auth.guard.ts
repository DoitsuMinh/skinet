import { ActivatedRouteSnapshot, CanActivateFn, Router, RouterStateSnapshot } from '@angular/router';
import { AuthService } from '../core/services/auth.service';
import { inject } from '@angular/core';

export const authGuard: CanActivateFn =
  (route: ActivatedRouteSnapshot, state: RouterStateSnapshot) => {
    const authService = inject(AuthService);
    const router = inject(Router);
    console.log(authService.isLoggedIn())
    if (!authService.isLoggedIn()) {
      router.navigate(['/login']);
      return false;
    } else {
      return true;
    }
  };
