import { Component, inject } from '@angular/core';
import { TableModule } from 'primeng/table';
import logData from './sampleData';
import { DatePipe } from '@angular/common';
import { Button } from 'primeng/button';
import { SplitterModule } from 'primeng/splitter';
import { Tag } from 'primeng/tag';
import { ActivatedRoute, Router } from '@angular/router';
import { RippleModule } from 'primeng/ripple';

@Component({
  selector: 'app-main-log-table',
  imports: [TableModule, DatePipe, Button, Tag, RippleModule, SplitterModule],
  templateUrl: './main-log-table.html',
  styleUrl: './main-log-table.css',
})
export class MainLogTable {
  logData = logData;

  private router = inject(Router);
  private route = inject(ActivatedRoute);

  goToLogDetails(logId: string) {
    this.router.navigate([logId], { relativeTo: this.route });
  }

  getBadgeSeverity(
    severity: string,
  ): 'success' | 'secondary' | 'info' | 'warn' | 'danger' | 'contrast' | null | undefined {
    switch (severity) {
      case 'warning':
        return 'warn';
      case 'fatal':
        return 'danger';
      case 'error':
        return 'danger';
      case 'critical':
        return 'danger';
      default:
        return 'warn';
    }
  }
}
