import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { ToastModule } from 'primeng/toast';
import { MessageService } from 'primeng/api';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, ToastModule],
  providers: [MessageService],
  templateUrl: './app.html',
  styleUrl: './app.css',
})
export class App {}
