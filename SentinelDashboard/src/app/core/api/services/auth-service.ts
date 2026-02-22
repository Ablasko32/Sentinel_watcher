import { HttpClient, HttpParams } from '@angular/common/http';
import { inject, Injectable, signal } from '@angular/core';
import { environment } from '../../../../environments/environment';
import { Observable } from 'rxjs';
import {
  IAppUser,
  ICreateUserRequest,
  ILoginUserRequest,
  IUpdateUserRequest,
} from '../models/authModels';
import { IApiResponse, IPaginatedApiResponse } from '../models/apiResponse';
import { IPaginatedRequest } from '../models/apiRequest';
import { buildHttpParams } from '../helpers/httpParamBuilder';

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

  getAllUsers(filters: IPaginatedRequest): Observable<IPaginatedApiResponse<IAppUser>> {
    const url = `${this.baseUrl}/users`;
    let params = buildHttpParams(filters);
    return this.httpClient.get<IPaginatedApiResponse<IAppUser>>(url, { params });
  }

  updateUser(userId: string, data: IUpdateUserRequest): Observable<IApiResponse<void>> {
    const url = `${this.baseUrl}/update/${userId}`;
    return this.httpClient.put<IApiResponse<void>>(url, data);
  }

  checkEmailExists(email: string): Observable<IApiResponse<{ exists: boolean }>> {
    const url = `${this.baseUrl}/email-exists`;
    const params = new HttpParams().set('email', email);
    return this.httpClient.get<IApiResponse<{ exists: boolean }>>(url, { params });
  }
}
