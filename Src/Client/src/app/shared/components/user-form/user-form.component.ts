import { PositionModel } from './../../models/position.model';
import { PositionService } from './../../services/position.service';
import { AppRolesEnum } from './../../models/user-app.model';
import { EnumUserSex } from './../../models/user.model';
import {
  Component,
  EventEmitter,
  Input,
  NgModule,
  OnInit,
  Output,
  ViewChild,
} from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import {
  DxButtonModule,
  DxDataGridModule,
  DxFormComponent,
  DxFormModule,
  DxPopupModule,
  DxValidatorModule,
  DxTextBoxModule,
} from 'devextreme-angular';
import { UserModel } from '../../models/user.model';
import { UserService } from '../../services/user.service';
import { AuthService } from '../../services';
import { CommonService } from '../../services/common.service';
import { JwtHelperService } from '@auth0/angular-jwt';

@Component({
  selector: 'app-user-form',
  templateUrl: './user-form.component.html',
  styleUrls: ['./user-form.component.scss'],
})
export class UserFormComponent implements OnInit {
  @Input() model: UserFormModel;
  @Input() position: PositionModel[];
  @Output() onRefreshGrid = new EventEmitter<void>();
  @ViewChild(DxFormComponent, { static: false }) myform: DxFormComponent;

  emailPattern: any = /^\s*[A-Za-z0-9-.\\+]+(\\.[_A-Za-z0-9-]+)*@kloon.vn\s*$/;
  isAdminRole = false;
  formState = FormState;
  popupVisible = false;
  popupConfirmDeleteVisible = false;
  popupTitle = '';
  buttonCloseTitle = 'Close';
  currUser: UserModel;
  titleChange: any;
  sexDataSource = [
    { caption: 'Male', value: EnumUserSex.MALE },
    { caption: 'Female', value: EnumUserSex.FEMALE },
  ];

  roleDataSource = [
    { caption: 'ADMINISTRATOR', value: AppRolesEnum.ADMINISTRATOR },
    { caption: 'USER', value: AppRolesEnum.USER },
  ];

  constructor(
    private userService: UserService,
    private authService: AuthService,
    private common: CommonService,
    private jwtHelperService: JwtHelperService
  ) {
    this.isAdminRole = this.authService.isRoleAdministrator;

  }

  open() {
    switch (this.model.state) {
      case FormState.CREATE:
        this.popupTitle = 'CREATE USER';
        this.buttonCloseTitle = 'Cancel';
        break;
      case FormState.DETAIL:
        this.popupTitle = 'DETAIL';
        this.buttonCloseTitle = 'Exit';
        break;
      case FormState.EDIT:
        this.popupTitle = 'EDIT USER';
        this.buttonCloseTitle = 'Cancel';
        break;
    }
    this.popupVisible = true;
    this.currUser = this.model.data;
  }

  onContentReady(e) {
    this.titleChange = e.component;
  }

  //#region Options
  closeButtonOptions = {
    text: 'Cancel', //() => {return this.buttonCloseTitle},
    icon: 'close',
    onClick: (e) => {
      this.popupVisible = false;
    },
  };

  createButtonOptions = {
    icon: 'save',
    text: 'Save',
    onClick: (e) => {
      var instance = this.myform.instance.validate();
      if (!instance.isValid) {
        return;
      }
      this.authService.isRoleAdministrator;
      this.userService.add(this.currUser).subscribe(
        (next) => {
          this.common.UI.multipleNotify("Add Success", "Success", 2000);
          this.popupVisible = false;
          this.onRefreshGrid.emit();
        },
        (err) => {

          if (err.error === 'INVALID_MODEL_DUPLICATED_EMAIL') {
            this.common.UI.multipleNotify('Email is existed !', 'error', 2000);
          }
          else if (err.error === 'INVALID_MODEL_FIRST_NAME_MIN_LENGTH') {
            this.common.UI.multipleNotify('First Name must have more than 2 character', 'error', 2000);
          }
          else if (err.error === 'INVALID_MODEL_FIRST_NAME_MAX_LENGTH') {
            this.common.UI.multipleNotify('First Name cannot exceed 20 characters', 'Error', 2000);
          }
        }

      );
    },
  };

  deleteButtonOptions = {
    icon: 'trash',
    text: 'Delete',
    onClick: (e) => {
      this.popupConfirmDeleteVisible = true;
    },
  };

  enterEditFormButtonOptions = {
    icon: 'edit',
    text: 'Edit',
    onClick: (e) => {
      this.model.state = FormState.EDIT;
      this.titleChange._options._optionManager._options.toolbarItems.filter(
        (t) => t.options.icon == 'close'
      )[0].options.text = 'Cancel';
      this.myform.instance._refresh();
      this.myform.instance.repaint();
    },
  };

  editButtonOptions = {
    icon: 'save',
    text: 'Save',
    onClick: (e) => {
      var instance = this.myform.instance.validate();
      if (!instance.isValid) {
        return;
      }

      this.userService.edit(this.currUser).subscribe(
        (next) => {
          //#region Temp solution for 
          let decodedToken = this.jwtHelperService.decodeToken(this.authService.getUserValue.token);
          const currentLoggedInUserId = decodedToken["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/sid"];
          const currentLoggedInUserRoleId = decodedToken["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"];

          if (currentLoggedInUserId == this.currUser.id) {
            this.authService.onChangeUserValue(this.currUser);
          }
          if (this.currUser.roleId != currentLoggedInUserRoleId) {
            this.authService.logOut();
          }
          //#endregion

          //TODO: Call refresh grid
          this.popupVisible = false;
          this.onRefreshGrid.emit();
        },
        (e: any) => {
          if (e.error === 'INVALID_MODEL_DUPLICATED_EMAIL') {
            this.common.UI.multipleNotify('Email is existed !', 'error', 2000);
          }
        }
      );
    },
  };

  closeDeletePopupButtonOptions = {
    text: 'Cancel',
    icon: 'close',
    onClick: (e) => {
      this.popupConfirmDeleteVisible = false;
    },
  };

  confirmDeleteButtonOptions = {
    icon: 'save',
    text: 'Ok',
    onClick: (e) => {
      this.userService.delete(this.currUser.id).subscribe(
        (next) => {
          this.popupConfirmDeleteVisible = false;
          this.popupVisible = false;
          this.onRefreshGrid.emit();
        },
        (error) => { }
      );
    },
  };
  ////#endregion

  ngOnInit(): void { }
}

@NgModule({
  imports: [
    BrowserModule,
    DxDataGridModule,
    DxButtonModule,
    DxPopupModule,
    DxFormModule,
    DxTextBoxModule,
    DxValidatorModule,
  ],
  declarations: [UserFormComponent],
  exports: [UserFormComponent],
})
export class UserFormModule { }

export class UserFormModel {
  state: FormState;
  data: UserModel;

  constructor(init?: Partial<UserFormModel>) {
    Object.assign(this, init);
  }
}

export enum FormState {
  DETAIL,
  CREATE,
  EDIT,
}
