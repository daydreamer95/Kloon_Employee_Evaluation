import { UserFormComponent, UserFormModule, UserFormModel, FormState } from './../../shared/components/user-form/user-form.component';
import { UserService } from './../../shared/services/user.service';
import { Component, NgModule, OnInit, ViewChild } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { DxButtonModule, DxDataGridModule, DxPopupModule} from 'devextreme-angular';
import { UserModel } from 'src/app/shared/models/user.model';
import { PositionModel } from 'src/app/shared/models/position.model';
import { PositionService } from 'src/app/shared/services/position.service';

@Component({
  selector: 'app-user',
  templateUrl: './user.component.html',
  styleUrls: ['./user.component.scss']
})
export class UserComponent implements OnInit {
  //#region Init variable
  dataSource: UserModel[]
  positionDataSource: PositionModel[];
 
  
  gridColumns: ['email', 'firstName', 'lastName', 'position', 'phoneNo'];

  @ViewChild(UserFormComponent) userFormComponent: UserFormComponent;
  currUser: UserFormModel = new UserFormModel();
  //#endregion

  constructor(private userService: UserService,private positionService: PositionService) {
    userService.getUsers("").subscribe(
      next => {
        this.dataSource = next;
        console.log(this.positionDataSource)  
      },
      error => {

      }
    );

    this.positionService.getPositions().subscribe(
      next => {
        this.positionDataSource = next;
        console.log(this.positionDataSource)
      },
      error => { }
    );
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

  onOpenDetailButton(e, data): void {
    this.currUser.state = FormState.DETAIL;
    this.currUser.data = new UserModel(data.data);

    this.userFormComponent.open();
  }

  onRefreshGrid() {
    console.log('haha');
    this.userService.getUsers("").subscribe(
      next => {
        this.dataSource = next;
      },
      error => {

      }
    );
  }
  //#endregion
 
  ngOnInit(): void {
    
  }

}

@NgModule({
  imports: [
    BrowserModule,
    DxDataGridModule,
    DxButtonModule,
    DxPopupModule,
  
    UserFormModule
  ],
  declarations: [UserComponent],
  bootstrap: [UserComponent]
})
export class UserModule {
 
}
