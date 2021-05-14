import { Injectable } from '@angular/core';
import { HttpEvent, HttpInterceptor, HttpHandler, HttpRequest } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';

export const rootApi = `${environment.apiUrl}`;

@Injectable()
export class ApiInterceptor implements HttpInterceptor {
  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    const apiReq = req.clone({ 
        url: `${rootApi}${req.url}`
        ,setHeaders: {
            Authorization: `Bearer ${'token here'}`
          } 
    });
    return next.handle(apiReq);
  }
}

