import { Component } from '@angular/core';
import { ListLayout } from '../../../../shared/components/layouts/list-layout/list-layout';
import { ButtonModule } from 'primeng/button';

@Component({
  selector: 'app-log-list-page',
  imports: [ListLayout, ButtonModule],
  templateUrl: './log-list-page.html',
  styleUrl: './log-list-page.css',
})
export class LogListPage {}
