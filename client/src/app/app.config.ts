import { ApplicationConfig, inject, provideAppInitializer, provideZoneChangeDetection } from "@angular/core";
import { provideRouter } from "@angular/router";
import { routes } from "./app.routes";
import { provideHttpClient, withInterceptors } from "@angular/common/http";
import { provideAnimationsAsync } from '@angular/platform-browser/animations/async'
import { authenticationInterceptor } from "./core/interceptors/authentication.interceptor";
import { errorInterceptor } from "./core/interceptors/error.interceptor";
import { loadingInterceptor } from "./core/interceptors/loading.interceptor";
import { InitService } from "./core/services/init.service";
import { lastValueFrom } from "rxjs";

export const appConfig: ApplicationConfig = {
  providers: [
    provideZoneChangeDetection({ eventCoalescing: true }),
    provideRouter(routes),
    provideAnimationsAsync(),
    provideHttpClient(
      withInterceptors([
        authenticationInterceptor,
        errorInterceptor,
        loadingInterceptor
      ])
    ),
    provideAppInitializer(() => {
      const initService = inject(InitService);

      return lastValueFrom(initService.init())
        .catch((err) => {
          console.error(err)
        })
        .finally(() => {
          const splash = document.getElementById('initial-splash');
          if (splash) {
            splash.remove();
          }
        })
    })
  ]
};
