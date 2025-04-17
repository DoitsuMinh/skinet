import { HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { BusyService } from '../services/busy.service';
import { environment } from 'src/environments/environment';
import { delay, finalize, identity } from 'rxjs';

export const loadingInterceptor: HttpInterceptorFn = (req, next) => {
  const busyService = inject(BusyService);
  const env = environment;

  busyService.busy();

  return next(req).pipe(
    (env.production) ? identity : delay(200),
    finalize(() => busyService.idle())
  );
};
