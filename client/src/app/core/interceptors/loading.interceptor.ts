import { HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { BusyService } from '../services/busy.service';
import { environment } from 'src/environments/environment';
import { delay, finalize } from 'rxjs';

export const loadingInterceptor: HttpInterceptorFn = (req, next) => {
  const busyService = inject(BusyService);
  const env = environment;

  busyService.busy();

  return next(req).pipe(
    (env.production) ? delay(500) : delay(0),
    finalize(() => busyService.idle())
  );
};
