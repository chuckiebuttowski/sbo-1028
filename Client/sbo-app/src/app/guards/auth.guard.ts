import { Injectable } from '@angular/core';
import { CanActivate, Router } from '@angular/router';
import { Observable } from 'rxjs/Observable';

import { AuthService } from '../services/auth.service';

@Injectable()
export class AuthGuard implements CanActivate {

  constructor(private authService: AuthService, private router: Router) { }

  canActivate(): Observable<boolean> {
    if (this.authService.isUserLoggedIn()) {
      return Observable.of(true);
    }
    else {
      this.router.navigate(['/login']);
      return Observable.of(false);
    }
  }

}
