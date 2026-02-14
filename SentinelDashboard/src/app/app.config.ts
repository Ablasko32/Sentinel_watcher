import { ApplicationConfig, provideBrowserGlobalErrorListeners } from '@angular/core';
import { provideRouter } from '@angular/router';
import { providePrimeNG } from 'primeng/config';
import Aura from '@primeuix/themes/Aura';

import { routes } from './app.routes';
import { provideHttpClient, withInterceptors } from '@angular/common/http';
import { SentinelPreset } from './lib/sentinel.preset';
import { cookieInterceptor } from './core/api/intreceptors/cookieIntreceptor';

export const appConfig: ApplicationConfig = {
  providers: [
    provideBrowserGlobalErrorListeners(),
    provideHttpClient(withInterceptors([cookieInterceptor])),
    provideRouter(routes),
    providePrimeNG({
      theme: {
        preset: SentinelPreset,
        options: {
          darkModeSelector: '.sentinel-dark',
          cssLayer: {
            name: 'primeng',
            order: 'theme, base, primeng',
          },
        },
      },
    }),
  ],
};
