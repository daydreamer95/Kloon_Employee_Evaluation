import { UserModel } from './../../services/user.service';
import { Component, EventEmitter, Input, NgModule, OnInit, Output, ViewChild } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { DxButtonModule, DxDataGridModule, DxFormComponent, DxFormModule, DxPopupModule } from 'devextreme-angular';
import { UserService } from '../../services/user.service';

@Component({
  selector: 'app-user-form',
  templateUrl: './user-form.component.html',
  styleUrls: ['./user-form.component.scss']
})
export class UserFormComponent implements OnInit {
  @Input() model: UserFormModel;
  @Output() onSubmitForm = new EventEmitter();
  @ViewChild(DxFormComponent, { static: false }) myform: DxFormComponent;
  
  formState = FormState;
  popupVisible = false;
  popupTitle = '';
  currUser:UserModel;

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
    this.currUser = this.model.data;
    this.myform.instance._refresh();
  }

  //#region Options
  closeButtonOptions = {
    text: "Close",
    onClick: (e) => {
        this.popupVisible = false;
    }
  };

  createButtonOptions = {
    icon: 'save',
    text: 'Submit',
    onClick: (e) => {
      this.myform.instance.validate();
      // this.popupVisible = false;
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
    DxPopupModule,
    DxFormModule
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
