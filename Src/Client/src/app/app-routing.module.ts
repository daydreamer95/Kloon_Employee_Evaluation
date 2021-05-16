import { ErrorComponent } from './pages/error/error.component';
import { ProjectComponent } from './pages/project/project.component';
import { UserComponent } from './pages/user/user.component';
import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { LoginFormComponent, ResetPasswordFormComponent, CreateAccountFormComponent, ChangePasswordFormComponent } from './shared/components';
import { AuthGuardService } from './shared/services';
import { HomeComponent } from './pages/home/home.component';
import { ProfileComponent } from './pages/profile/profile.component';
import { DxDataGridModule, DxFormModule } from 'devextreme-angular';
import { CriteriasComponent } from './pages/criterias/criterias.component';
import { AppRoutingRole } from './shared/common/enum-app-routing-role';

const routes: Routes = [
  {
    path: 'profile',
    component: ProfileComponent,
    canActivate: [ AuthGuardService ],
    data: {
      allowedRoles: [AppRoutingRole.ADMINISTRATOR, AppRoutingRole.USER]
    }
  },
  {
    path: 'criteria',
    component: CriteriasComponent,
    canActivate: [ AuthGuardService ],
    data: {
      allowedRoles: [AppRoutingRole.ADMINISTRATOR]
    }
  },
  {
    path: 'home',
    component: HomeComponent,
    canActivate: [ AuthGuardService ],
    data: {
      allowedRoles: [AppRoutingRole.ADMINISTRATOR, AppRoutingRole.USER]
    }
  },
  {
    path: 'login-form',
    component: LoginFormComponent,
    canActivate: [ AuthGuardService ]
  },
  {
    path: 'reset-password',
    component: ResetPasswordFormComponent,
    canActivate: [ AuthGuardService ]
  },
  {
    path: 'create-account',
    component: CreateAccountFormComponent,
    canActivate: [ AuthGuardService ]
  },
  {
    path: 'change-password/:recoveryCode',
    component: ChangePasswordFormComponent,
    canActivate: [ AuthGuardService ]
  },
  {
    path: 'user',
    component: UserComponent,
    canActivate: [ AuthGuardService ],
    data: {
      allowedRoles: [AppRoutingRole.ADMINISTRATOR, AppRoutingRole.USER]
    }
  },
  {
    path: 'project',
    component: ProjectComponent,
    canActivate: [ AuthGuardService ],
    data: {
      allowedRoles: [AppRoutingRole.ADMINISTRATOR]
    }
  },
  {
    path: 'error',
    component: ErrorComponent,
    canActivate: [ AuthGuardService ]
  },
  {
    path: '**',
    redirectTo: 'error'
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes, { useHash: false }), DxDataGridModule, DxFormModule],
  providers: [AuthGuardService],
  exports: [RouterModule],
  declarations: [HomeComponent, ProfileComponent]
})
export class AppRoutingModule { }
