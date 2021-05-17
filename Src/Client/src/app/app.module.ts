import { ProjectService } from './shared/services/project.service';
import { PositionService } from './shared/services/position.service';
import { UserService } from './shared/services/user.service';
import { HttpClient, HttpClientModule, HttpHandler, HTTP_INTERCEPTORS } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppComponent } from './app.component';
import { SideNavOuterToolbarModule, SideNavInnerToolbarModule, SingleCardModule } from './layouts';
import { FooterModule, ResetPasswordFormModule, CreateAccountFormModule, ChangePasswordFormModule, LoginFormModule } from './shared/components';
import { AuthService, ScreenService, AppInfoService } from './shared/services';
import { UnauthenticatedContentModule } from './unauthenticated-content';
import { AppRoutingModule } from './app-routing.module';
import { UserModule } from './pages/user/user.component';
import { ProjectModule } from './pages/project/project.component';
import { ErrorComponent } from './pages/error/error.component';
import { ApiInterceptor } from './http-interceptor/api-interceptor';
import { UserFormModule } from './shared/components/user-form/user-form.component';
import { CriteriasModule } from './pages/criterias/criterias.component';
import { JwtHelperService, JwtModule } from '@auth0/angular-jwt';

@NgModule({
  declarations: [
    AppComponent,
    ErrorComponent
  ],
  imports: [
    BrowserModule,
    SideNavOuterToolbarModule,
    SideNavInnerToolbarModule,
    SingleCardModule,
    FooterModule,
    ResetPasswordFormModule,
    CreateAccountFormModule,
    ChangePasswordFormModule,
    LoginFormModule,
    UnauthenticatedContentModule,
    AppRoutingModule,
    UserModule,
    ProjectModule,
    HttpClientModule,
    UserFormModule,
    CriteriasModule,
    JwtModule.forRoot({
      config: {
        tokenGetter: function tokenGetter() {
          if (localStorage.getItem('user') == null || localStorage.getItem('user') == undefined) {
            return 'a';
          }
          return JSON.parse(localStorage.getItem('user'))['token'];
        }
      }
    })
  ],
  providers: [
    AuthService, ScreenService, AppInfoService, JwtHelperService, UserService, PositionService, ProjectService,
    { provide: HTTP_INTERCEPTORS, useClass: ApiInterceptor, multi: true },
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
