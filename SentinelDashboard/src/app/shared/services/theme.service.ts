import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

type Theme = 'light' | 'dark';
enum Themes {
  Light = 'light',
  Dark = 'dark',
}

@Injectable({
  providedIn: 'root',
})
export class ThemeService {
  private readonly themeKey = 'sentinel-dashboard-theme';
  private currentTheme$ = new BehaviorSubject<Theme>(this.getSavedTheme());
  currentTheme = this.currentTheme$.asObservable();

  constructor() {
    this.applyTheme(this.currentTheme$.value);
  }

  private getSavedTheme() {
    const savedTheme = localStorage.getItem(this.themeKey);
    if (savedTheme === Themes.Dark) {
      return Themes.Dark;
    }
    return window.matchMedia('(prefers-color-scheme: dark)').matches ? Themes.Dark : Themes.Light;
  }

  toggleTheme() {
    const newMode = this.currentTheme$.value === Themes.Dark ? Themes.Light : Themes.Dark;
    this.currentTheme$.next(newMode);
    this.applyTheme(newMode);
    localStorage.setItem(this.themeKey, newMode);
  }

  private applyTheme(theme: Theme) {
    if (theme === Themes.Dark) {
      document.documentElement.classList.add('sentinel-dark');
    } else {
      document.documentElement.classList.remove('sentinel-dark');
    }
  }
}
