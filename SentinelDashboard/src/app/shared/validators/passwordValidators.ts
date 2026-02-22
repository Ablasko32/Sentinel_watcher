import { AbstractControl, ValidatorFn } from '@angular/forms';

/// Custom validator to check if password and confirm password match
//Fields must be named 'password' and 'confirmPassword' for this to work
//Sets error on confirmPassword field if they do not match
export const confirmPasswordValidator: ValidatorFn = (control: AbstractControl) => {
  var password = control.get('password')?.value;
  var confirmPassword = control.get('confirmPassword')?.value;
  if (!password || !confirmPassword) {
    return null;
  }
  if (confirmPassword !== password) {
    control.get('confirmPassword')?.setErrors({ passwordMismatch: true });
    return { passwordMismatch: true };
  }
  return null;
};
