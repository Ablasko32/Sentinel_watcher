import { Routes } from '@angular/router';

export const LogListRoutes: Routes = [
  {
    path: '',
    loadComponent: () => import('./pages/log-list-page/log-list-page').then((m) => m.LogListPage),
  },
  {
    path: ':logId',
    loadComponent: () =>
      import('./pages/log-details-page/log-details-page').then((m) => m.LogDetailsPage),
  },
];
