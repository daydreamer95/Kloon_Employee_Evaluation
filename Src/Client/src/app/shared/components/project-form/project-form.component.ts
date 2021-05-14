import { Component, EventEmitter, Input, NgModule, OnInit, Output, ViewChild } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { DxButtonModule, DxDataGridModule, DxFormComponent, DxFormModule, DxPopupModule, DxSelectBoxModule, DxTabPanelModule } from 'devextreme-angular';
import { ProjectModel, ProjectService } from '../../services/project.service';


import { confirm } from "devextreme/ui/dialog";

@Component({
    selector: 'app-project-form',
    templateUrl: './project-form.component.html',
    styleUrls: ['./project-form.component.scss']
})
export class ProjectFormComponent implements OnInit {

    @Input() model: ProjectFormModel;
    @Output() onSubmitForm = new EventEmitter();
    @ViewChild(DxFormComponent, { static: false }) myForm: DxFormComponent;

    formState = ProjectFormState;
    popupVisible = false;
    popupTitle = '';
    currentProject: ProjectModel;
    selectedIndex: number = 0;

    constructor() {

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
            status: "Close"
        }
    ];

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
        this.myForm.instance._refresh();
        this.selectedIndex = 0;

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
        text: 'Submit',
        onClick: (e) => {
            this.myForm.instance.validate();
        }
    }

    editButtonOptions = {
        icon: 'save',
        text: 'Submit',
        onClick: (e) => {
            this.myForm.instance.validate();
        }
    }

    editButtonOnDetailOptions = {
        icon: 'save',
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
            //result.then(function (dialogResult) {
            //    alert(dialogResult ? "Yes" : "No");
            //});
        }
    }

    // #endregion

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
        DxSelectBoxModule,
        DxTabPanelModule
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