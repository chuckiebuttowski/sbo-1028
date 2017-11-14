import { Injectable } from '@angular/core';
import { Http, Response } from '@angular/http';
import {HttpEvent, HttpInterceptor, HttpHandler, HttpRequest, HttpResponse, HttpErrorResponse} from '@angular/common/http';
import {Observable} from 'rxjs/Observable'
import 'rxjs/Rx';
import { AuthService } from '../services/auth.service'

@Injectable()
export class TokenInterceptor implements HttpInterceptor {

    intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        const token = sessionStorage.getItem("token"); //this.auth.GetToken();
        
        if(token != null && token != "undefined" && token != undefined && token != "") {
             const authReq = req.clone({headers: req.headers.set('Authorization', 'Bearer ' + token)});
             //req.headers.append("Authorization", 'Bearer ' + token);
            return next.handle(authReq);
          //   .map((event: HttpEvent<any>) => {
          //   if (event instanceof HttpResponse) {
          //     return event.clone({
          //       body: JSON.parse(event.body),
          //     });
          //   }
          // })
        // .catch((error: HttpErrorResponse) => {
        //       const parsedError = Object.assign({}, error, { error: JSON.parse(error.error) });
        //       return Observable.throw(new HttpErrorResponse(parsedError));
        //   });
        }
        else{
            return next.handle(req)
            .map((event: HttpEvent<any>) => {
            if (event instanceof HttpResponse) {
              return event.clone({
                body: JSON.parse(event.body),
              });
            }
          })
        // .catch((error: HttpErrorResponse) => {
        //       const parsedError = Object.assign({}, error, { error: JSON.parse(error.error) });
        //       return Observable.throw(new HttpErrorResponse(parsedError));
        //   });
        }
      }

}
