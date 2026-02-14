import { Routes } from '@angular/router';
import { authGuard } from './core/api/guards/authGuard';

export const routes: Routes = [
  { path: '', pathMatch: 'full', redirectTo: 'login' },
  {
    path: 'login',
    loadComponent: () =>
      import('./features/auth/pages/login-page/login-page').then((m) => m.LoginPage),
  },
  {
    path: 'app',
    canActivate: [authGuard],
    loadComponent: () =>
      import('./shared/components/layouts/main-layout/main-layout').then((m) => m.MainLayout),
    children: [
      {
        path: 'dashboard',
        loadChildren: () =>
          import('./features/dashboard/dashboard.routes').then((m) => m.DashboardRoutes),
      },

      {
        path: 'logs',
        loadChildren: () =>
          import('./features/log-list/log-list.routes').then((m) => m.LogListRoutes),
      },
    ],
  },
];
