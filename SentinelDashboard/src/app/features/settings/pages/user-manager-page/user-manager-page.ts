import { Component, inject, OnInit, signal } from '@angular/core';
import { Button } from 'primeng/button';
import { DialogService, DynamicDialogRef } from 'primeng/dynamicdialog';
import { DynamicDialogModule } from 'primeng/dynamicdialog';
import { CreateUserModal } from '../../components/create-user-modal/create-user-modal';
import { IAppUser } from '../../../../core/api/models/authModels';
import { AuthService } from '../../../../core/api/services/auth-service';
import { TableModule } from 'primeng/table';
import { ConfirmationService, MessageService } from 'primeng/api';
import { Dialog } from 'primeng/dialog';
import { ConfirmDialogModule } from 'primeng/confirmdialog';

@Component({
  selector: 'app-user-manager-page',
  imports: [Button, DynamicDialogModule, TableModule, Dialog, ConfirmDialogModule],
  providers: [DialogService, ConfirmationService],
  templateUrl: './user-manager-page.html',
  styleUrl: './user-manager-page.css',
})
export class UserManagerPage implements OnInit {
  createUserDialogRef: DynamicDialogRef<CreateUserModal> | null = null;
  private dialogService = inject(DialogService);
  private authService = inject(AuthService);
  private confirmationService = inject(ConfirmationService);
  private messageService = inject(MessageService);
  //state
  users = signal<IAppUser[]>([]);
  isLoading = signal(false);
  error = signal<string | null>(null);

  ngOnInit() {
    this.fetchUsers();
  }

  //API
  private fetchUsers() {
    this.isLoading.set(true);
    this.error.set(null);
    this.authService.getAllUsers().subscribe({
      next: (res) => {
        if (res.success) {
          this.users.set(res.data);
          this.isLoading.set(false);
        }
      },
      error: (err) => {
        this.error.set(err.message);
        this.isLoading.set(false);
      },
    });
  }

  //mutations
  private deleteUser(user: IAppUser) {
    this.authService.deleteUser(user.id).subscribe({
      next: (res) => {
        if (res.success) {
          this.messageService.add({
            severity: 'success',
            summary: 'Success',
            detail: 'User deleted successfully',
          });
          this.fetchUsers();
        }
      },
      error: (err) => {
        this.messageService.add({
          severity: 'error',
          summary: 'Error',
          detail: 'Failed to delete user',
        });
      },
    });
  }

  show() {
    this.createUserDialogRef = this.dialogService.open(CreateUserModal, {
      header: 'Create New User',
      closable: true,
    });

    this.createUserDialogRef?.onClose.subscribe(({ created }) => {
      if (created) {
        this.fetchUsers();
      }
    });
  }

  confirmDelete(user: IAppUser) {
    this.confirmationService.confirm({
      message: `Are you sure you want to delete ${user.username}?`,
      header: 'Confirm Deletion',
      icon: 'pi pi-exclamation-triangle',
      acceptButtonStyleClass: 'p-button-danger',
      rejectButtonStyleClass: 'p-button-secondary',
      accept: () => {
        this.deleteUser(user);
      },
    });
  }

  openEditDialog(user: IAppUser) {
    this.createUserDialogRef = this.dialogService.open(CreateUserModal, {
      header: 'Edit User',
      closable: true,
      data: {
        user: user,
      },
    });
    this.createUserDialogRef?.onClose.subscribe(({ created }) => {
      if (created) {
        this.fetchUsers();
      }
    });
  }
}
