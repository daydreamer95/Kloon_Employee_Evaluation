import { UserFormComponent, UserFormModule, UserFormModel, FormState } from './../../shared/components/user-form/user-form.component';
import { element } from 'protractor';
import { UserModel, UserService } from './../../shared/services/user.service';
import { Component, NgModule, OnInit, ViewChild } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { DxButtonModule, DxDataGridModule, DxPopupModule } from 'devextreme-angular';

@Component({
  selector: 'app-user',
  templateUrl: './user.component.html',
  styleUrls: ['./user.component.scss']
})
export class UserComponent implements OnInit {
  //#region Init variable
  dataSource: UserModel[]
  gridColumns: ['email', 'firstName', 'lastName', 'position', 'phoneNo'];

  @ViewChild(UserFormComponent) userFormComponent:UserFormComponent;
  currUser: UserFormModel = new UserFormModel();
  //#endregion

  constructor(userService: UserService) { 
    this.dataSource = userService.getUsers();

  }

  onToolbarPreparing(e) {
    e.toolbarOptions.items.unshift({
            location: 'after',
            widget: 'dxButton',
            options: {
                icon: 'add',
                width: 'auto',
                text: 'Add',
                onClick: this.onOpenAddUserPopup.bind(this)
            }
        });
  }

  //#region POPUP

  onOpenAddUserPopup(): void {
    this.currUser.state = FormState.CREATE;
    this.currUser.data = new UserModel();
    
    this.userFormComponent.open();
  }

  onOpenDetailButton(e, data): void{
    this.currUser.state = FormState.DETAIL;
    this.currUser.data = new UserModel(data.data);

    this.userFormComponent.open();
  }

  
  //#endregion

  ngOnInit(): void {
  }

}

@NgModule({
  imports:[
    BrowserModule,
    DxDataGridModule,
    DxButtonModule,
    DxPopupModule,
    UserFormModule
  ],
  declarations: [UserComponent],
  bootstrap: [UserComponent]
})
export class UserModule{

}
