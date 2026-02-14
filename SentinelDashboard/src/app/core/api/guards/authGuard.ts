import { CanActivateFn, Router } from '@angular/router';
import { AuthService } from '../services/auth-service';
import { inject } from '@angular/core';
import { catchError, map, of } from 'rxjs';

export const authGuard: CanActivateFn = (route, state) => {
  const authService = inject(AuthService);
  const router = inject(Router);

  return authService.checkAuthStatus().pipe(
    map((res) => {
      if (res.success) {
        authService.user.set(res.data);
        return true;
      } else {
        router.navigate(['login']);
        return false;
      }
    }),
    catchError(() => {
      router.navigate(['login']);
      return of(false);
    }),
  );
};
