import { Component } from '@angular/core';
import { TabsModule } from 'primeng/tabs';
import { RouterLink, RouterOutlet } from '@angular/router';
import { PageLayout } from '../../../../../shared/components/layouts/page-layout/page-layout';

@Component({
  selector: 'app-settings-layout',
  imports: [TabsModule, RouterOutlet, PageLayout, RouterLink],
  templateUrl: './settings-layout.html',
  styleUrl: './settings-layout.css',
})
export class SettingsLayout {
  readonly tabs = [
    { label: 'Api Keys', route: 'api-keys', icon: 'pi pi-key' },
    { label: 'Users', route: 'users', icon: 'pi pi-user' },
  ];
}
