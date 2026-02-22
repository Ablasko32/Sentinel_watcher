import { Component, inject, signal } from '@angular/core';
import { Button } from 'primeng/button';
import { DialogService, DynamicDialogRef } from 'primeng/dynamicdialog';
import { DynamicDialogModule } from 'primeng/dynamicdialog';
import { CreateUserModal } from '../../components/create-user-modal/create-user-modal';
import { IAppUser } from '../../../../core/api/models/authModels';
import { AuthService } from '../../../../core/api/services/auth-service';
import { TableLazyLoadEvent, TableModule } from 'primeng/table';
import { ConfirmDialogModule } from 'primeng/confirmdialog';
import { paginationSignal } from '../../../../core/api/helpers/paginationSignal';
import { IconField } from 'primeng/iconfield';
import { InputText } from 'primeng/inputtext';
import { InputIcon } from 'primeng/inputicon';
import { BadgeModule } from 'primeng/badge';
import { ConfirmationService, MessageService } from 'primeng/api';

@Component({
  selector: 'app-user-manager-page',
  imports: [
    Button,
    DynamicDialogModule,
    TableModule,
    ConfirmDialogModule,
    IconField,
    InputText,
    InputIcon,
    BadgeModule,
  ],
  templateUrl: './user-manager-page.html',
  styleUrl: './user-manager-page.css',
})
export class UserManagerPage {
  createUserDialogRef: DynamicDialogRef<CreateUserModal> | null = null;
  private dialogService = inject(DialogService);
  private authService = inject(AuthService);
  private confirmationService = inject(ConfirmationService);
  private messageService = inject(MessageService);
  //state
  users = signal<IAppUser[]>([]);
  pagination = paginationSignal();
  isLoading = signal(false);
  error = signal<string | null>(null);
  private lastTableEvent: TableLazyLoadEvent = { first: 0, rows: 10 };

  //API
  private fetchUsers(event: TableLazyLoadEvent) {
    this.isLoading.set(true);

    const first = event.first ?? 0; //offset
    const rows = event.rows ?? 10; //limit
    const sortField = event.sortField as string;
    const sortOrder = event.sortOrder === 1 ? 'asc' : 'desc';
    const globalFilter = event.filters?.['global'];
    const searchTerm =
      globalFilter && !Array.isArray(globalFilter) ? (globalFilter.value as string) : '';

    this.error.set(null);
    this.authService
      .getAllUsers({
        skip: first,
        take: rows,
        sortField: sortField,
        sortOrder: sortOrder,
        searchTerm: searchTerm,
      })
      .subscribe({
        next: (res) => {
          if (res.success) {
            this.users.set(res.data.items);
            this.pagination.set({
              page: res.data.page,
              pageSize: res.data.pageSize,
              totalPages: res.data.totalPages,
              hasNextPage: res.data.hasNextPage,
              hasPreviousPage: res.data.hasPreviousPage,
              totalCount: res.data.totalCount,
            });
            this.isLoading.set(false);
          }
        },
        error: (err) => {
          this.error.set(err.message);
          this.isLoading.set(false);
        },
      });
  }

  private refetchUsers() {
    this.fetchUsers(this.lastTableEvent);
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
          this.refetchUsers();
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
        this.refetchUsers();
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
        this.refetchUsers();
      }
    });
  }

  onLazyLoad(event: TableLazyLoadEvent) {
    this.lastTableEvent = event;
    this.fetchUsers(event);
  }
}
