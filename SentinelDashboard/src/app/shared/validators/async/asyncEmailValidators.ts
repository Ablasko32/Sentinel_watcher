import { AbstractControl, AsyncValidatorFn, ValidationErrors } from '@angular/forms';
import { catchError, map, Observable, of, timer, switchMap } from 'rxjs';
import { AuthService } from '../../../core/api/services/auth-service';

export function validateEmailAsync(authService: AuthService): AsyncValidatorFn {
  return (control: AbstractControl): Observable<ValidationErrors | null> => {
    if (!control.value) {
      return of(null);
    }

    return timer(500).pipe(
      switchMap(() => authService.checkEmailExists(control.value)),
      map((res) => {
        return res.data ? { emailExists: true } : null;
      }),
      catchError(() => of(null)),
    );
  };
}
