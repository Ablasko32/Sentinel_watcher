import { Component } from '@angular/core';

import { ButtonModule } from 'primeng/button';
import { ListLayout } from '../../../../shared/components/layouts/list-layout/list-layout';

@Component({
  selector: 'app-dashboard-page',
  imports: [ButtonModule, ListLayout],
  templateUrl: './dashboard-page.html',
  styleUrl: './dashboard-page.css',
})
export class DashboardPage {}
