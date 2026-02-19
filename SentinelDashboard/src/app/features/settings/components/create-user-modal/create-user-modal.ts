import { Component, inject, OnInit, signal } from '@angular/core';
import {
  FormBuilder,
  FormControl,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { FormTextInput } from '../../../../shared/components/forms/form-text-input/form-text-input';
import { Button } from 'primeng/button';
import { AuthService } from '../../../../core/api/services/auth-service';
import { MessageService } from 'primeng/api';
import { DynamicDialogConfig, DynamicDialogRef } from 'primeng/dynamicdialog';
import { IAppUser } from '../../../../core/api/models/authModels';

interface ICreateUserForm {
  username: FormControl<string>;
  password: FormControl<string>;
  confirmPassword: FormControl<string>;
  email: FormControl<string>;
  newPassword: FormControl<string>;
}

@Component({
  selector: 'app-create-user-modal',
  imports: [ReactiveFormsModule, FormTextInput, Button],
  templateUrl: './create-user-modal.html',
  styleUrl: './create-user-modal.css',
})
export class CreateUserModal implements OnInit {
  editMode = signal(false);
  private fb = inject(FormBuilder);
  private authService = inject(AuthService);
  private messageService = inject(MessageService);
  private dialogRef = inject(DynamicDialogRef);
  private config = inject(DynamicDialogConfig<{ user?: IAppUser }>);

  form!: FormGroup<ICreateUserForm>;

  ngOnInit() {
    if (this.config.data?.user) {
      this.editMode.set(true);
      this.initForm(true);
      return;
    }
    this.initForm(false);
  }

  private initForm(editMode: boolean) {
    const user = this.config.data?.user;

    this.form = this.fb.group<ICreateUserForm>({
      username: this.fb.control(user?.username ?? '', {
        validators: editMode ? [] : [Validators.required],
        nonNullable: true,
      }),
      email: this.fb.control(user?.email ?? '', {
        validators: editMode ? [Validators.email] : [Validators.required, Validators.email],
        nonNullable: true,
      }),
      // Create mode fields
      password: this.fb.control('', {
        validators: editMode ? [] : [Validators.required, Validators.minLength(6)],
        nonNullable: true,
      }),
      confirmPassword: this.fb.control('', {
        validators: editMode ? [] : [Validators.required, Validators.minLength(6)],
        nonNullable: true,
      }),
      // Edit mode fields
      newPassword: this.fb.control('', {
        validators: editMode ? [Validators.minLength(6)] : [],
        nonNullable: true,
      }),
    });
  }

  onSubmit() {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    const { username, password, email, newPassword } = this.form.getRawValue();

    if (this.editMode()) {
      const userId = this.config.data.user.id;
      this.authService.updateUser(userId, { username, newPassword, email }).subscribe({
        next: (res) => {
          if (res.success) {
            this.messageService.add({
              severity: 'success',
              summary: 'Success',
              detail: 'User updated successfully',
            });
            this.dialogRef.close({ created: true });
          }
        },
        error: (err) => {
          this.messageService.add({
            severity: 'error',
            summary: 'Error',
            detail: err?.error?.message || 'Failed to update user',
          });
        },
      });
      return;
    }
    this.authService.createUser({ username, password, email }).subscribe({
      next: (res) => {
        if (res.success) {
          this.messageService.add({
            severity: 'success',
            summary: 'Success',
            detail: 'User created successfully',
          });
          this.dialogRef.close({ created: true });
        }
      },
      error: (err) => {
        this.messageService.add({
          severity: 'error',
          summary: 'Error',
          detail: 'Failed to create user',
        });
      },
    });
  }

  reset() {
    if (this.editMode()) {
      const user = this.config.data.user;
      this.form.reset({
        username: user.username,
        email: user.email,
        password: '',
        confirmPassword: '',
        newPassword: '',
      });
    } else {
      this.form.reset();
    }
  }
}
