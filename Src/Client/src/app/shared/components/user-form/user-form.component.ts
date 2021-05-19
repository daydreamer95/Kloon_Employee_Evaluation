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

  emailPattern: any = /^[_A-Za-z0-9-\\+]+(\\.[_A-Za-z0-9-]+)*@kloon.vn$/;
  isAdminRole = false;
  formState = FormState;
  popupVisible = false;
  popupConfirmDeleteVisible = false;
  popupTitle = '';
  buttonCloseTitle='Close';
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
    private authService: AuthService
  ) {
    this.isAdminRole = this.authService.isRoleAdministrator;
  }

  open() {
    switch (this.model.state) {
      case FormState.CREATE:
        this.popupTitle = 'CREATE USER';
        this.buttonCloseTitle = "Cancel"
        break;
      case FormState.DETAIL:
        this.popupTitle = 'DETAIL';
        this.buttonCloseTitle = "Exit"
        break;
      case FormState.EDIT:
        this.popupTitle = 'EDIT USER';
        this.buttonCloseTitle = "Cancel"
        break;
    }
    this.popupVisible = true;
    this.currUser = this.model.data;
    // this.myform.instance._refresh();
    this.onTitleButtonChange();
    this.titleChange._options._optionManager._options.toolbarItems[4].options.text = this.buttonCloseTitle;

  }
  
  onContentReady(e){

    this.titleChange = e.component;
  }

  onTitleButtonChange(){

    if(this.model == null)
      return this.buttonCloseTitle;
      switch (this.model.state) {
        case FormState.CREATE:
          this.buttonCloseTitle = "Cancel"
          break;
        case FormState.DETAIL:
          this.buttonCloseTitle = "Exit"
          break;
        case FormState.EDIT:
          this.buttonCloseTitle = "Cancel"
          break;
      }
    return this.buttonCloseTitle;
  }

  //#region Options
  closeButtonOptions = {
    text: this.buttonCloseTitle,
    icon: 'close',
    onClick: (e) => {
      this.popupVisible = false;
    }
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
          //TODO: Call refresh grid
          this.popupVisible = false;
          this.onRefreshGrid.emit();
        },
        (error) => {
          alert(error);
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
      this.titleChange._options._optionManager._options.toolbarItems.filter(t => t.options.icon == 'close')[0].options.text = "Cancel";
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
          //TODO: Call refresh grid
          this.popupVisible = false;
          this.onRefreshGrid.emit();
        },
        (error) => {

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
    text: 'Submit',
    onClick: (e) => {
      this.userService.delete(this.currUser.id).subscribe(
        (next) => {
          this.popupConfirmDeleteVisible = false;
          this.popupVisible = false;
          this.onRefreshGrid.emit();
        },
        (error) => {

        }
      );
    },
  };
  ////#endregion

  ngOnInit(): void {
   
  }
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
export class UserFormModule {}

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
