import { UserModel } from './../../services/user.service';
import { Component, EventEmitter, Input, NgModule, OnInit, Output } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { DxButtonModule, DxDataGridModule, DxPopupModule } from 'devextreme-angular';
import { UserService } from '../../services/user.service';

@Component({
  selector: 'app-user-form',
  templateUrl: './user-form.component.html',
  styleUrls: ['./user-form.component.scss']
})
export class UserFormComponent implements OnInit {
  @Input() model: UserFormModel;
  @Output() onSubmitForm = new EventEmitter();

  formState = FormState;
  popupVisible = false;
  popupTitle = 'haha';

  constructor(userService: UserService) { 
    
  }

  open(){
    switch(this.model.state){
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
  }

  //#region Options
  closeButtonOptions = {
    text: "Close",
    onClick: (e) => {
        this.popupVisible = false;
    }
  };

  submitButtonOptions = {
    icon: 'save',
    text: 'Submit',
    onClick: (e) => {
      this.popupVisible = false;
    }
  };

  editButtonOptions = {
    icon: 'save',
    text: 'Edit',
    onClick: (e) => {
      this.popupVisible = false;
    }
  }
  ////#endregion

  ngOnInit(): void {
  }

}


@NgModule({
  imports:[
    BrowserModule,
    DxDataGridModule,
    DxButtonModule,
    DxPopupModule
  ],
  declarations: [UserFormComponent],
  exports: [UserFormComponent]
})
export class UserFormModule{

}

export class UserFormModel{
  state: FormState;
  data: UserModel

  constructor(init?:Partial<UserFormModel>) {
    Object.assign(this, init);
}
}

export enum FormState {
  DETAIL,
  CREATE,
  EDIT
}
