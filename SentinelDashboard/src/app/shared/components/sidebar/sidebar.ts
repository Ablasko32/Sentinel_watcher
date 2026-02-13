import { Component } from '@angular/core';
import { RouterLink, RouterLinkActive } from '@angular/router';
import { ThemeSwitch } from '../theme-switch/theme-switch';
import { TooltipModule } from 'primeng/tooltip';
import { AvatarModule } from 'primeng/avatar';

@Component({
  selector: 'app-sidebar',
  imports: [RouterLink, RouterLinkActive, ThemeSwitch, TooltipModule, AvatarModule],
  templateUrl: './sidebar.html',
  styleUrl: './sidebar.css',
})
export class Sidebar {
  switchTheme() {
    document.documentElement.classList.toggle('sentinel-dark');
  }

  getCurrentTheme() {
    return document.documentElement.classList.contains('sentinel-dark')
      ? 'pi pi-moon text-gray-500'
      : 'pi pi-sun text-yellow-500';
  }
}
