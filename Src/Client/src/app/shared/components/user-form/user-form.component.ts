import { PositionModel } from './../../models/position.model';
import { PositionService } from './../../services/position.service';
import { AppRolesEnum } from './../../models/user-app.model';
import { EnumUserSex } from './../../models/user.model';
import { Component, EventEmitter, Input, NgModule, OnInit, Output, ViewChild } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { DxButtonModule, DxDataGridModule, DxFormComponent, DxFormModule, DxPopupModule } from 'devextreme-angular';
import { UserModel } from '../../models/user.model';
import { UserService } from '../../services/user.service';

@Component({
  selector: 'app-user-form',
  templateUrl: './user-form.component.html',
  styleUrls: ['./user-form.component.scss']
})
export class UserFormComponent implements OnInit {
  @Input() model: UserFormModel;
  @Input() onRefreshGrid = new EventEmitter();
  @ViewChild(DxFormComponent, { static: false }) myform: DxFormComponent;

  formState = FormState;
  popupVisible = false;
  popupTitle = '';
  currUser: UserModel;
  sexDataSource = [
    { caption: 'Male', value: EnumUserSex.MALE },
    { caption: 'Female', value: EnumUserSex.FEMALE }
  ]

  roleDataSource = [
    { caption: 'ADMINISTRATOR', value: AppRolesEnum.ADMINISTRATOR },
    { caption: 'USER', value: AppRolesEnum.USER }
  ]

  positionDateSource: PositionModel[];

  constructor(private userService: UserService, private positionService: PositionService) {
    positionService.getPositions().subscribe(
      next => {
        this.positionDateSource = next;
      },
      error => { }
    );
  }

  open() {
    switch (this.model.state) {
      case FormState.CREATE:
        this.popupTitle = 'CREATE USER';
        break;
      case FormState.DETAIL:
        this.popupTitle = 'DETAIL';
        break;
      case FormState.EDIT:
        this.popupTitle = 'EDIT USER';
        break;
    }
    this.popupVisible = true;
    this.currUser = this.model.data;
    this.myform.instance._refresh();
  }

  //#region Options
  closeButtonOptions = {
    text: "Close",
    icon: 'close',
    onClick: (e) => {
      this.popupVisible = false;
    }
  };

  createButtonOptions = {
    icon: 'save',
    text: 'Submit',
    onClick: (e) => {
      var instance = this.myform.instance.validate();
      if (!instance.isValid) {
        return;
      }

      this.userService.add(this.currUser).subscribe(
        next => {
          //TODO: Call refresh grid
          this.popupVisible = false;
          this.onRefreshGrid;
        },
        error => { }
      )
    }
  };

  deleteButtonOptions = {
    icon: 'trash',
    text: 'Delete',
    onClick: (e) => {
      debugger;
      this.userService.delete(this.currUser.id).subscribe(
        next => {
          //TODO: Call refresh grid
          this.popupVisible = false;
        },
        error => {

        }
      )
    }
  };

  enterEditFormButtonOptions = {
    icon: 'edit',
    text: 'Edit',
    onClick: (e) => {
      this.model.state = FormState.DETAIL;
      this.myform.instance._refresh();
    }
  }

  editButtonOptions = {
    icon: 'save',
    text: 'Edit',
    onClick: (e) => {
      this.userService.edit(this.currUser).subscribe(
        next => {
          debugger;
          //TODO: Call refresh grid
          this.popupVisible = false;
          this.myform.instance._refresh();
        },
        error => {

        }
      )
    }
  }
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
    DxFormModule
  ],
  declarations: [UserFormComponent],
  exports: [UserFormComponent]
})
export class UserFormModule {

}

export class UserFormModel {
  state: FormState;
  data: UserModel

  constructor(init?: Partial<UserFormModel>) {
    Object.assign(this, init);
  }
}

export enum FormState {
  DETAIL,
  CREATE,
  EDIT
}
