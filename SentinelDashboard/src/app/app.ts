import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { ToastModule } from 'primeng/toast';
import { ConfirmationService, MessageService } from 'primeng/api';
import { ConfirmDialog } from 'primeng/confirmdialog';
import { DialogService } from 'primeng/dynamicdialog';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, ToastModule, ConfirmDialog],
  providers: [MessageService, ConfirmationService, DialogService],
  templateUrl: './app.html',
  styleUrl: './app.css',
})
export class App {}
