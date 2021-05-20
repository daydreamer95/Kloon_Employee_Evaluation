import { Component, NgModule, OnInit, ViewChild } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { DxButtonModule, DxDataGridModule, DxPopupModule, DxLoadPanelModule } from 'devextreme-angular';
import { ProjectFormComponent, ProjectFormModel, ProjectFormModule, ProjectFormState } from '../../shared/components/project-form/project-form.component';
import { ProjectService } from '../../shared/services/project.service';
import { ProjectModel } from 'src/app/shared/models/project.model';
import { AuthService } from 'src/app/shared/services';
import { CommonService } from 'src/app/shared/services/common.service';


@Component({
  selector: 'app-project',
  templateUrl: './project.component.html',
  styleUrls: ['./project.component.scss']
})
export class ProjectComponent implements OnInit {

  // //#region  Init variable
  dataSource: ProjectModel[];
  gridColumns = ['name', 'description', 'statusText'];
  loading = false;

  @ViewChild(ProjectFormComponent) projectFormComponent: ProjectFormComponent;

  currentProject: ProjectFormModel = new ProjectFormModel();
  selectedIndex: number = 0;
  isAdminRole: boolean = false;

  //#endregion
  constructor(
    private projectService: ProjectService,
    private authService: AuthService,
    private commonService: CommonService
  ) {
    this.isAdminRole = this.authService.isRoleAdministrator;
  }

  ngOnInit(): void {
    this.loading = true;
    this.getProjects();
  }

  getProjects() {
    this.projectService.getProjects().subscribe(
      ((responeseData: ProjectModel[]) => {
        this.dataSource = [];
        if (responeseData.length > 0) {
          this.dataSource = responeseData;
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
    e.toolbarOptions.items.unshift({
      location: 'after',
      widget: 'dxButton',
      options: {
        icon: 'add',
        width: 'auto',
        text: 'Add',
        visible: this.isAdminRole,
        onClick: this.onOpenAddProjectPopup.bind(this)
      }
    });
  }

  //#region Popup

  onOpenAddProjectPopup(e): void {
    this.currentProject.state = ProjectFormState.CREATE;
    this.currentProject.data = new ProjectModel();
    this.currentProject.data.id = 0;
    this.selectedIndex = 0;

    this.projectFormComponent.open();

  }

  onOpenDetailButton(e, data): void {
    this.projectService.getProjectById(data.data.id).subscribe(
      ((responeseData: ProjectModel) => {
        this.currentProject.state = ProjectFormState.DETAIL;
        this.currentProject.data = new ProjectModel(responeseData);
        this.projectFormComponent.open();
        this.selectedIndex = 0;
      }),
      (
        error => {
          this.loading = false;
          this.commonService.UI.multipleNotify(error.error, 'error', 2000);
        }
      )
    )


  }

  saveProjectForm(data: ProjectModel) {
    if (data.id === 0) {
      this.projectService.add(data).subscribe(
        ((responeseData: ProjectModel) => {
          this.commonService.UI.multipleNotify("Add Project Success", 'success', 2000);
          this.currentProject.state = ProjectFormState.DETAIL;
          this.currentProject.data = responeseData;
          this.selectedIndex = 1;
          this.getProjects();
          this.projectFormComponent.open();

        }),
        (
          error => {
            this.loading = false;
            this.commonService.UI.multipleNotify(error.error, 'error', 2000);
          }
        )
      )
    }
    else {
      this.projectService.edit(data).subscribe(
        ((responeseData: ProjectModel) => {
          this.commonService.UI.multipleNotify("Update Project Success", "success", 2000);

          this.currentProject.state = ProjectFormState.DETAIL;
          this.currentProject.data = responeseData;
          this.selectedIndex = 1;
          this.getProjects();
          this.projectFormComponent.open();

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

  deleteProject(data: boolean) {
    if (data) {
      this.projectService.delete(this.currentProject.data.id).subscribe(
        (() => {
          this.commonService.UI.multipleNotify("Delete Project Success", "success", 2000);
          this.projectFormComponent.close();
          this.getProjects();

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

  //#endregion

}


@NgModule({
  imports: [
    BrowserModule,
    DxDataGridModule,
    DxButtonModule,
    DxPopupModule,
    ProjectFormModule,
    DxLoadPanelModule
  ],
  declarations: [ProjectComponent],
  bootstrap: [ProjectComponent]
})
export class ProjectModule {

}
