import { HttpParams } from '@angular/common/http';

export const buildHttpParams = (filters: Record<string, any>) => {
  let params = new HttpParams();
  Object.entries(filters).forEach(([key, value]) => {
    if (value !== null && value !== undefined) {
      params = params.set(key, value.toString());
    }
  });
  return params;
};
