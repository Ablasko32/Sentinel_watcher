import { Injectable } from '@angular/core';
import { environment } from '../../../../environments/environment';

@Injectable({
  providedIn: 'root',
})
export class LogEntryService {
  private baseUrl = environment.apiUrl;
  private apiUrl = `${this.baseUrl}/log-entries`;
}
