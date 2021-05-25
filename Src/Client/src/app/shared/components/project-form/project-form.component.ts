import { Component, EventEmitter, Input, NgModule, OnInit, Output, ViewChild } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import {
    DxButtonModule,
    DxDataGridModule,
    DxFormComponent,
    DxFormModule,
    DxPopupModule,
    DxSelectBoxModule,
    DxTabPanelModule,
    DxScrollViewModule,
    DxTemplateModule,
    DxNumberBoxModule
} from 'devextreme-angular';
import CustomStore from 'devextreme/data/custom_store';
import DataSource from 'devextreme/data/data_source';


import { confirm } from "devextreme/ui/dialog";
import { ProjectUserModel } from '../../models/project-user.model';
import { ProjectModel } from '../../models/project.model';
import { ProjectRolesEnum, UserApp } from '../../models/user-app.model';
import { UserModel } from '../../models/user.model';
import { AuthService } from '../../services';
import { CommonService } from '../../services/common.service';
import { ProjectUserService } from '../../services/project-user.service';

@Component({
    selector: 'app-project-form',
    templateUrl: './project-form.component.html',
    styleUrls: ['./project-form.component.scss']
})
export class ProjectFormComponent implements OnInit {

    @Input() model: ProjectFormModel;
    projectUserFormModel: ProjectUserFormModel = new ProjectUserFormModel();
    @Input() selectedIndex: number = 0;
    @Output() onSubmitForm: EventEmitter<ProjectModel> = new EventEmitter<ProjectModel>();
    @Output() onConfirmDelete: EventEmitter<boolean> = new EventEmitter<boolean>();
    @ViewChild(DxFormComponent, { static: false }) myForm: DxFormComponent;

    selectBoxListUsers: any = {};
    dataSource: ProjectUserModel[];
    gridColumns = ['no', 'email', 'firstName', 'lastName', 'projectRole'];
    loading = false;
    disabledCreateUser = false;
    listUserNotInProject: UserModel[];
    userSelect: UserModel;
    userSelectId: number = 0;

    formState = ProjectFormState;
    popupVisible = false;
    popupTitle = '';
    currentProject: ProjectModel;

    isAdminRole: boolean = false;

    popupVisibleProjectUser = false;
    popupTitleProjectUser = '';
    userCurrent: UserApp = null;

    selectBoxUserComp: any;
    buttonAddUserComp: any;
    initTextboxNameComp: any;

    constructor(
        private projectMemberService: ProjectUserService,
        private authService: AuthService,
        private commonService: CommonService
    ) {
        this.isAdminRole = this.authService.isRoleAdministrator;
        this.userCurrent = this.authService.getUser;
    }
    ngOnInit() {
    }

    listStatus: ProjectStatus[] = [
        {
            id: 1,
            status: "Open"
        },
        {
            id: 2,
            status: "Pending"
        },
        {
            id: 3,
            status: "Closed"
        }
    ];

    listProjectUserRole: ProjectRoleStatus[] = [
        {
            id: 1,
            status: "Member"
        },
        {
            id: 2,
            status: "QA"
        },
        {
            id: 3,
            status: "Project Manager"
        }
    ]

    open() {
        switch (this.model.state) {
            case ProjectFormState.CREATE:
                this.popupTitle = 'Create Project';
                break;

            case ProjectFormState.EDIT:
                this.popupTitle = 'Update Project';
                break;
            case ProjectFormState.DETAIL:
                this.popupTitle = 'Detail Project';
                break;
        }
        this.popupVisible = true;
        this.currentProject = this.model.data;
        this.selectedIndex = 0;
        this.getProjectMember();

        this.OnInitDataUser();

        this.selectBoxListUsers.load();
        this.userSelectId = 0;
    }

    close() {
        this.popupVisible = false;
    }

    // #region Options

    closeButtonOption = {
        text: 'Close',
        onClick: (e) => {
            this.popupVisible = false;
        }
    }

    createButtonOptions = {
        icon: 'save',
        text: 'Add',
        onClick: (e) => {
            this.disabledCreateUser = true;
            if (this.myForm.instance.validate().isValid) {
                this.onSubmitForm.emit(this.currentProject);
            };
            this.disabledCreateUser = false;
        },
    }

    editButtonOptions = {
        icon: 'edit',
        text: 'Update',
        onClick: (e) => {
            if (this.myForm.instance.validate().isValid) {
                this.onSubmitForm.emit(this.currentProject);
            };
        }
    }

    editButtonOnDetailOptions = {
        icon: 'edit',
        text: 'Edit',
        onClick: (e) => {
            this.popupTitle = 'Update Project';
            this.model.state = ProjectFormState.EDIT;
            this.selectedIndex = 0;
        }
    }

    deleteButtonOnDetailOptions = {
        icon: 'remove',
        text: 'Delete',
        onClick: (e) => {
            var result = confirm("Are you want to delete this record?", "Delete");
            result.then((dialogResult: boolean) => {
                this.onConfirmDelete.emit(dialogResult);
            });
        }
    }

    setFocus(e) {
        if (this.model.state == ProjectFormState.CREATE) {
            this.myForm.instance.getEditor("name").focus();
        }
    }

    // #endregion

    //#region tab 2

    OnInitDataUser() {
        this.selectBoxListUsers = new DataSource({
            store: new CustomStore({
                key: "ID",
                loadMode: "raw",
                load: async () => {
                    const data = await this.projectMemberService.GetTopFiveUserNotInProject(this.currentProject.id, "").toPromise();
                    this.selectBoxUserComp.option('items', data);
                    this.selectBoxUserComp.option('value', null);

                    this.selectBoxUserComp.option('visible', this.isProjectleaderProject());
                    this.buttonAddUserComp.option('visible', this.isProjectleaderProject());
                    return data;
                }
            })
        });
    }

    getProjectMember() {
        if (this.currentProject.id == 0) {
            return;
        }
        this.projectMemberService.GetProjectMember(this.currentProject.id).subscribe(
            ((responeseData: ProjectUserModel[]) => {
                this.dataSource = [];
                if (responeseData.length > 0) {
                    this.dataSource = responeseData;
                    this.dataSource.forEach((element, index) => {
                        element.no = index + 1;
                    });
                }
                this.loading = false;
            }),
            (
                error => {
                    this.loading = false;
                    this.commonService.UI.multipleNotify(error.error, 'error', 2000);
                }
            )
        )
    }

    onToolbarPreparing(e) {
        e.toolbarOptions.items.unshift(
            {
                location: 'before',
                widget: 'dxButton',
                options: {
                    icon: 'add',
                    text: 'Add',
                    //visible: this.isProjectleaderProject(),
                    onClick: this.onAddProjectMember.bind(this),
                    onInitialized: this.onInitButtonAddUser.bind(this)
                }
            },
            {
                location: 'before',
                widget: 'dxSelectBox',
                options: {
                    width: '150%',
                    dataSource: this.selectBoxListUsers,
                    showClearButton: true,
                    valueExpr: (t: UserModel) => {
                        if (t == null) {
                            return 0;
                        }
                        return t.id;
                    },
                    displayExpr: (t: UserModel) => {
                        if (t == null) {
                            return "";
                        }
                        return `${t.firstName} ${t.lastName}(${t.email})`
                    },
                    value: "id",
                    searchEnabled: true,
                    placeholder: "Search User",
                    onValueChanged: (e) => {
                        this.userSelectId = e.value;
                    },
                    onInitialized: this.onInitSelectBoxUser.bind(this),
                }
            }
        );
    }

    onInitSelectBoxUser(e) {
        this.selectBoxUserComp = e.component;
    }

    onInitButtonAddUser(e) {
        this.buttonAddUserComp = e.component;
    }


    onAddProjectMember(e): void {
        var projectId = this.model.data.id;
        var userId = this.userSelectId;
        if (userId === null) {
            this.commonService.UI.multipleNotify("Please choose the user to add to the Project.", "error", 2000);
            return;
        }
        this.projectMemberService.add(projectId, userId).subscribe(
            ((responeseData: ProjectUserModel) => {
                this.commonService.UI.multipleNotify("Add a member to project success.", "success", 2000);
                this.getProjectMember();
                this.selectBoxUserComp.option('value', null);
                this.OnInitDataUser();
                this.selectBoxListUsers.load();
            }),
            (
                error => {
                    this.loading = false;
                    this.commonService.UI.multipleNotify(error.error, 'error', 2000);
                }
            )
        )

    }

    onOpenDetailProjectUserButton(e, data): void {
        this.projectMemberService.GetProjectMemberById(this.currentProject.id, data.data.id).subscribe(
            ((responeseData: ProjectUserModel) => {
                this.projectUserFormModel.state = ProjectUserState.DETAIL;
                this.projectUserFormModel.data = new ProjectUserModel(responeseData);
                this.openPopupUserModel();

            }),
            (
                error => {
                    this.loading = false;
                    this.commonService.UI.multipleNotify(error.error, 'error', 2000);
                }
            )
        )
    }

    openPopupUserModel() {
        switch (this.projectUserFormModel.state) {

            case ProjectUserState.EDIT:
                this.popupTitleProjectUser = 'Update Project Member';
                break;
            case ProjectUserState.DETAIL:
                this.popupTitleProjectUser = 'Detail Project Member';
                break;
        }
        this.popupVisibleProjectUser = true;
    }

    editButtonProjectUserOptions = {
        icon: 'save',
        text: 'Submit',
        onClick: (e) => {
            this.projectMemberService.edit(this.projectUserFormModel.data.projectId, this.projectUserFormModel.data.id, this.projectUserFormModel.data.projectRoleId).subscribe(
                ((responeseData: ProjectUserModel) => {
                    this.popupVisibleProjectUser = false;
                    this.getProjectMember();
                }),
                (
                    error => {
                        this.loading = false;
                        this.commonService.UI.multipleNotify(error.error, 'error', 2000);
                    }
                )
            )
        }
    }

    isProjectleaderProject(): boolean {
        if (this.userCurrent == null) {
            return false;
        }

        return this.isAdminRole == true;
    }

    editButtonOnDetailProjectUserOptions = {
        icon: 'edit',
        text: 'Edit',
        onClick: (e) => {
            this.popupTitleProjectUser = 'Update Project Member';
            this.projectUserFormModel.state = ProjectUserState.EDIT;
        },
    }

    deleteButtonOnDetailProjectUserOptions = {
        icon: 'remove',
        text: 'Delete',
        onClick: (e) => {
            var result = confirm("Are you want to delete this record?", "Delete");
            result.then((dialogResult: boolean) => {
                if (dialogResult) {
                    this.projectMemberService.delete(this.projectUserFormModel.data.projectId, this.projectUserFormModel.data.id).subscribe(
                        (() => {
                            this.commonService.UI.multipleNotify("Delete Project Member Success", "success", 2000);
                            this.popupVisibleProjectUser = false;
                            this.getProjectMember();

                            this.OnInitDataUser();

                            this.selectBoxListUsers.load();

                        }),
                        (
                            error => {
                                this.loading = false;
                                this.commonService.UI.multipleNotify(error.error, 'error', 5000);
                            }
                        )
                    )
                }
            });
        }
    }

    closeButtonProjectUserOption = {
        text: 'Close',
        onClick: (e) => {
            this.popupVisibleProjectUser = false;
        }
    }
    //#endregion

}

@NgModule({
    imports: [
        BrowserModule,
        DxDataGridModule,
        DxButtonModule,
        DxPopupModule,
        DxFormModule,
        DxSelectBoxModule,
        DxTabPanelModule,
        DxScrollViewModule
    ],
    declarations: [ProjectFormComponent],
    exports: [ProjectFormComponent]
})
export class ProjectFormModule {

}


export class ProjectFormModel {
    state: ProjectFormState;
    data: ProjectModel;

    constructor(init?: Partial<ProjectFormModel>) {
        Object.assign(this, init);
    }
}


export enum ProjectFormState {
    CREATE,
    EDIT,
    DETAIL
}

export class ProjectStatus {
    id: number;
    status: string;
}


export class ProjectUserFormModel {
    state: ProjectUserState;
    data: ProjectUserModel;

    constructor(init?: Partial<ProjectUserFormModel>) {
        Object.assign(this, init);
    }
}

export enum ProjectUserState {
    EDIT,
    DETAIL
}
export class ProjectRoleStatus {
    id: number;
    status: string;
}
