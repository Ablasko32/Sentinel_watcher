import { HttpClient } from '@angular/common/http';
import { inject, Injectable, signal } from '@angular/core';
import { environment } from '../../../../environments/environment';
import { Observable } from 'rxjs';
import {
  IAppUser,
  ICreateUserRequest,
  ILoginUserRequest,
  IUpdateUserRequest,
} from '../models/authModels';
import { IApiResponse } from '../models/apiResponse';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private baseUrl = `${environment.apiUrl}/auth`;
  user = signal<IAppUser | null>(null);

  private httpClient = inject(HttpClient);

  loginUser(data: ILoginUserRequest): Observable<IApiResponse<IAppUser>> {
    const url = `${this.baseUrl}/login`;
    return this.httpClient.post<IApiResponse<IAppUser>>(url, data);
  }

  logoutUser(): Observable<IApiResponse<void>> {
    const url = `${this.baseUrl}/logout`;
    return this.httpClient.post<IApiResponse<void>>(url, {});
  }

  deleteUser(userId: string): Observable<IApiResponse<void>> {
    const url = `${this.baseUrl}/${userId}`;
    return this.httpClient.delete<IApiResponse<void>>(url);
  }

  checkAuthStatus(): Observable<IApiResponse<IAppUser>> {
    const url = `${this.baseUrl}/status`;
    return this.httpClient.get<IApiResponse<IAppUser>>(url);
  }

  createUser(data: ICreateUserRequest): Observable<IApiResponse<void>> {
    const url = `${this.baseUrl}/create`;
    return this.httpClient.post<IApiResponse<void>>(url, data);
  }

  getAllUsers(): Observable<IApiResponse<IAppUser[]>> {
    const url = `${this.baseUrl}/users`;
    return this.httpClient.get<IApiResponse<IAppUser[]>>(url);
  }

  updateUser(userId: string, data: IUpdateUserRequest): Observable<IApiResponse<void>> {
    const url = `${this.baseUrl}/update/${userId}`;
    return this.httpClient.put<IApiResponse<void>>(url, data);
  }
}
