import { Routes } from '@angular/router';

export const routes: Routes = [
  {
    path: '',
    loadComponent: () =>
      import('./features/dashboard/pages/dashboard-page/dashboard-page').then(
        (m) => m.DashboardPage,
      ),
  },
  {
    path: 'logs-list',
    loadComponent: () =>
      import('./features/log-list/pages/log-list-page/log-list-page').then((m) => m.LogListPage),
  },
  {
    path: 'logs-list/:logId',
    loadComponent: () =>
      import('./features/log-list/pages/log-details-page/log-details-page').then(
        (m) => m.LogDetailsPage,
      ),
  },
];
