import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { environment } from '../../../../environments/environment';
import { Observable } from 'rxjs';
import { ILoginUserRequest } from '../models/authModels';
import { ApiResponse } from '../models/apiResponse';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private baseUrl = `${environment.apiUrl}/auth`;

  private httpClient = inject(HttpClient);

  loginUser(data: ILoginUserRequest): Observable<ApiResponse<void>> {
    const url = `${this.baseUrl}/login`;
    return this.httpClient.post<ApiResponse<void>>(url, data);
  }

  logoutUser(): Observable<ApiResponse<void>> {
    const url = `${this.baseUrl}/logout`;
    return this.httpClient.post<ApiResponse<void>>(url, {});
  }

  deleteUser(userId: string): Observable<ApiResponse<void>> {
    const url = `${this.baseUrl}/${userId}`;
    return this.httpClient.delete<ApiResponse<void>>(url);
  }

  checkAuthStatus(): Observable<ApiResponse<void>> {
    const url = `${this.baseUrl}/check`;
    return this.httpClient.get<ApiResponse<void>>(url);
  }
}
