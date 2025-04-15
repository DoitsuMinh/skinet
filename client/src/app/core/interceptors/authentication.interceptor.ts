import { HttpErrorResponse, HttpHandler, HttpInterceptorFn, HttpRequest } from '@angular/common/http';
import { inject } from '@angular/core';
import { AuthService } from '../services/auth.service';

export const authenticationInterceptor: HttpInterceptorFn = (req, next) => {
  const authService = inject(AuthService);
  console.log(req)
  // Add token to request if user is logged in
  if (authService.isLoggedIn()) {
    req = req.clone({
      withCredentials: true,
      setHeaders: {
        Authorization: `Bearer ${authService.user().token}`,
      },
    });
  } else {
    req = req.clone({
      withCredentials: true,
    });
  }
  return next(req);
};