import { Component } from '@angular/core';
import { ListLayout } from '../../../../shared/components/layouts/list-layout/list-layout';
import { ButtonModule } from 'primeng/button';
import { MainLogTable } from '../../components/main-log-table/main-log-table';

@Component({
  selector: 'app-log-list-page',
  imports: [ListLayout, ButtonModule, MainLogTable],
  templateUrl: './log-list-page.html',
  styleUrl: './log-list-page.css',
})
export class LogListPage {}
