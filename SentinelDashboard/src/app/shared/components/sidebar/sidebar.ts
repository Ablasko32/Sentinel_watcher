import { Component } from '@angular/core';
import { Button } from 'primeng/button';
import { RouterLink, RouterLinkActive } from '@angular/router';

@Component({
  selector: 'app-sidebar',
  imports: [Button, RouterLink, RouterLinkActive],
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
