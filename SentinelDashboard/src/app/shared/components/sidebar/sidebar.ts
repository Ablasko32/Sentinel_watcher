import { Component, inject, signal } from '@angular/core';
import { Router, RouterLink, RouterLinkActive } from '@angular/router';
import { ThemeSwitch } from '../theme-switch/theme-switch';
import { TooltipModule } from 'primeng/tooltip';
import { AvatarModule } from 'primeng/avatar';
import { PopoverModule } from 'primeng/popover';
import { AuthService } from '../../../core/api/services/auth-service';
import { ButtonModule } from 'primeng/button';

@Component({
  selector: 'app-sidebar',
  imports: [
    RouterLink,
    RouterLinkActive,
    ThemeSwitch,
    TooltipModule,
    AvatarModule,
    PopoverModule,
    ButtonModule,
  ],
  templateUrl: './sidebar.html',
  styleUrl: './sidebar.css',
})
export class Sidebar {
  isLoading = signal(false);

  authService = inject(AuthService);
  private router = inject(Router);
  readonly menuItems = [
    {
      label: 'Dashboard',
      icon: 'pi pi-home',
      route: 'dashboard',
    },

    {
      label: 'Logs',
      icon: 'pi pi-file',
      route: 'logs',
    },

    {
      label: 'Settings',
      icon: 'pi pi-cog',
      route: 'settings',
    },
  ];

  switchTheme() {
    document.documentElement.classList.toggle('sentinel-dark');
  }

  getCurrentTheme() {
    return document.documentElement.classList.contains('sentinel-dark')
      ? 'pi pi-moon text-gray-500'
      : 'pi pi-sun text-yellow-500';
  }

  logout() {
    this.isLoading.set(true);
    this.authService.logoutUser().subscribe({
      next: () => {
        this.isLoading.set(false);
        this.router.navigate(['/login']);
      },
      error: () => {
        this.isLoading.set(false);
      },
    });
  }
}
