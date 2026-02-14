import { Component, inject, signal } from '@angular/core';
import { FormBuilder, FormControl, ReactiveFormsModule, Validators } from '@angular/forms';
import { PasswordModule } from 'primeng/password';
import { MessageModule } from 'primeng/message';
import { Button } from 'primeng/button';
import { AuthService } from '../../../../core/api/services/auth-service';
import { FormTextInput } from '../../../../shared/components/forms/form-text-input/form-text-input';
import { Router } from '@angular/router';

interface ILoginForm {
  email: FormControl<string>;
  password: FormControl<string>;
}

@Component({
  selector: 'app-login-card',
  imports: [PasswordModule, ReactiveFormsModule, Button, FormTextInput, MessageModule],
  templateUrl: './login-card.html',
  styleUrl: './login-card.css',
})
export class LoginCard {
  isLoading = signal(false);
  errorMessage = signal<string | null>(null);
  private fb = inject(FormBuilder);
  private authService = inject(AuthService);
  private router = inject(Router);

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
    const data = this.loginForm.getRawValue();

    this.isLoading.set(true);
    this.errorMessage.set(null);
    this.authService.loginUser(data).subscribe({
      next: (res) => {
        if (res.success) {
          this.authService.user.set(res.data);
          this.router.navigate(['app', 'dashboard']);
        } else {
          this.errorMessage.set(res.message ?? 'Login failed. Please try again.');
          this.isLoading.set(false);
        }
      },
      error: (err) => {
        this.errorMessage.set(err.error?.message ?? 'Login failed. Please try again.');
        this.isLoading.set(false);
      },
    });
  }
}
