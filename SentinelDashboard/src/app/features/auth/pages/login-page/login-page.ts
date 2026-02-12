import { Component } from '@angular/core';
import { LoginCard } from '../../components/login-card/login-card';
import { ThemeSwitch } from '../../../../shared/components/theme-switch/theme-switch';

@Component({
  selector: 'app-login-page',
  imports: [LoginCard, ThemeSwitch],
  templateUrl: './login-page.html',
  styleUrl: './login-page.css',
})
export class LoginPage {}
