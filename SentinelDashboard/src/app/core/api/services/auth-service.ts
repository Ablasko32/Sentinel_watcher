import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { environment } from '../../../../environments/environment';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private baseUrl = `${environment.apiUrl}/auth`;

  private _httpClient = inject(HttpClient);

  loginUser(email: string, password: string) {
    console.log('Login attempt:', { email, password });
  }
}
