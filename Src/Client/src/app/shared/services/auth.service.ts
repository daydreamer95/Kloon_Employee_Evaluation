import { UserApp } from './../models/user-app.model';
import { UserService } from './user.service';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { CanActivate, Router, ActivatedRouteSnapshot } from '@angular/router';
import { BehaviorSubject, Observable } from 'rxjs';

const defaultPath = '/home';
const defaultUser = {
  email: 'sandra@example.com',
  avatarUrl: 'https://js.devexpress.com/Demos/WidgetsGallery/JSDemos/images/employees/06.png'
};

@Injectable()
export class AuthService {
  // private _user = null;
  private userSubject: BehaviorSubject<UserApp>;
  public user: Observable<UserApp>;

  get loggedIn(): boolean {
    return !!this.userSubject.value;
  }

  private _lastAuthenticatedPath: string = defaultPath;
  set lastAuthenticatedPath(value: string) {
    this._lastAuthenticatedPath = value;
  }

  constructor(private router: Router, private httpClient: HttpClient, private userService: UserService) {
    this.userSubject = new BehaviorSubject<UserApp>(JSON.parse(localStorage.getItem('user')));
    this.user = this.userSubject.asObservable();
  }

  async logIn(email: string, password: string) {

    try {
      return this.userService.login(email, password).toPromise().then(res => {

        let user = new UserApp(res);
        this.userSubject.next(user);
        localStorage.setItem('user', JSON.stringify(res));

        this.router.navigate([this._lastAuthenticatedPath]);
        return {
          isOk: true,
          data: user,
          message: ''
        };
      },
        err => {
          let msgError = typeof (err.error) == 'string' ? err.error : err.message;
          return {
            isOk: false,
            data: this.userSubject.value,
            message: msgError
          };
        });
    }
    catch {
      return {
        isOk: false,
        message: "Authentication failed"
      };
    }
  }

  async getUser() {
    try {
      // Send request
      return {
        isOk: true,
        data: this.userSubject.value
      };
    }
    catch {
      return {
        isOk: false
      };
    }
  }

  async createAccount(email, password) {
    try {
      // Send request
      console.log(email, password);

      this.router.navigate(['/create-account']);
      return {
        isOk: true
      };
    }
    catch {
      return {
        isOk: false,
        message: "Failed to create account"
      };
    }
  }

  async changePassword(email: string, recoveryCode: string) {
    try {
      // Send request
      console.log(email, recoveryCode);

      return {
        isOk: true
      };
    }
    catch {
      return {
        isOk: false,
        message: "Failed to change password"
      }
    };
  }

  async resetPassword(email: string) {
    try {
      // Send request
      console.log(email);

      return {
        isOk: true
      };
    }
    catch {
      return {
        isOk: false,
        message: "Failed to reset password"
      };
    }
  }

  async logOut() {
    localStorage.removeItem('user');
    this.userSubject.next(null);
    this.router.navigate(['/login-form']);
  }
}

@Injectable()
export class AuthGuardService implements CanActivate {
  constructor(private router: Router, private authService: AuthService) { }

  canActivate(route: ActivatedRouteSnapshot): boolean {
    const isLoggedIn = this.authService.loggedIn;
    const isAuthForm = [
      'login-form',
      'reset-password',
      'create-account',
      'change-password/:recoveryCode'
    ].includes(route.routeConfig.path);

    if (isLoggedIn && isAuthForm) {
      this.authService.lastAuthenticatedPath = defaultPath;
      this.router.navigate([defaultPath]);
      return false;
    }

    if (!isLoggedIn && !isAuthForm) {
      this.router.navigate(['/login-form']);
    }

    if (isLoggedIn) {
      this.authService.lastAuthenticatedPath = route.routeConfig.path;
    }

    return isLoggedIn || isAuthForm;
  }
}
