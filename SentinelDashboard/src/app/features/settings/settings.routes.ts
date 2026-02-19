import { Routes } from '@angular/router';

export const SettingsRoutes: Routes = [
  {
    path: 'api-keys',
    loadComponent: () => import('./pages/api-keys-page/api-keys-page').then((m) => m.ApiKeysPage),
  },
  {
    path: 'users',
    loadComponent: () =>
      import('./pages/user-manager-page/user-manager-page').then((m) => m.UserManagerPage),
  },
];
