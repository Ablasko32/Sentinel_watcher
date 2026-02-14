import { HttpEvent, HttpHandlerFn, HttpRequest } from '@angular/common/http';
import { Observable } from 'rxjs';

// This interceptor adds the withCredentials flag to all outgoing HTTP requests, allowing cookies to be sent with requests to the backend.
export function cookieInterceptor(
  req: HttpRequest<unknown>,
  next: HttpHandlerFn,
): Observable<HttpEvent<unknown>> {
  const clonedRequest = req.clone({
    withCredentials: true,
  });

  return next(clonedRequest);
}
