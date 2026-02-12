import { Component, inject } from '@angular/core';
import { Button } from 'primeng/button';
import { ThemeService } from '../../services/theme.service';

@Component({
  selector: 'app-theme-switch',
  imports: [Button],
  templateUrl: './theme-switch.html',
  styleUrl: './theme-switch.css',
})
export class ThemeSwitch {
  private themeService = inject(ThemeService);

  switchTheme() {
    this.themeService.toggleTheme();
  }

  getCurrentTheme() {
    return document.documentElement.classList.contains('sentinel-dark')
      ? 'pi pi-moon '
      : 'pi pi-sun ';
  }
}
