import { Component, inject, signal } from '@angular/core';
import { FormBuilder, FormControl, ReactiveFormsModule, Validators } from '@angular/forms';
import { PasswordModule } from 'primeng/password';
import { Button } from 'primeng/button';
import { AuthService } from '../../../../core/api/services/auth-service';
import { FormTextInput } from '../../../../shared/components/forms/form-text-input/form-text-input';

interface ILoginForm {
  email: FormControl<string>;
  password: FormControl<string>;
}

@Component({
  selector: 'app-login-card',
  imports: [PasswordModule, ReactiveFormsModule, Button, FormTextInput],
  templateUrl: './login-card.html',
  styleUrl: './login-card.css',
})
export class LoginCard {
  isLoading = signal(false);
  private fb = inject(FormBuilder);
  private authService = inject(AuthService);

  loginForm = this.fb.group<ILoginForm>({
    email: this.fb.control('', {
      validators: [Validators.required, Validators.email],
      nonNullable: true,
    }),
    password: this.fb.control('', { validators: [Validators.required], nonNullable: true }),
  });

  onSubmit() {
    if (this.loginForm.invalid) {
      this.loginForm.markAllAsTouched();
      return;
    }
    this.isLoading.set(true);
    const { email, password } = this.loginForm.getRawValue();
    this.authService.loginUser(email, password);
    this.isLoading.set(false);
  }

  isInvalid(controlName: keyof ILoginForm) {
    const control = this.loginForm.get(controlName);
    return control?.invalid && (control.dirty || control.touched);
  }
}
