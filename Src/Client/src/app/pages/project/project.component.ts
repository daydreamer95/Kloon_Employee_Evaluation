import { Component, NgModule, OnInit, ViewChild } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { DxButtonModule, DxDataGridModule, DxPopupModule, DxLoadPanelModule } from 'devextreme-angular';
import { ProjectFormComponent, ProjectFormModel, ProjectFormModule, ProjectFormState } from '../../shared/components/project-form/project-form.component';
import { ProjectModel, ProjectService } from '../../shared/services/project.service';
import notify from 'devextreme/ui/notify';


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

  //#endregion
  constructor(
    private projectService: ProjectService
  ) {

  }

  ngOnInit(): void {
    this.loading = true;
    this.getProjects();
  }

  getProjects() {
    this.projectService.getProjects().toPromise().then(
      ((responeseData: ProjectModel[]) => {
        setTimeout(() => {
          if (responeseData.length > 0) {
            this.dataSource = responeseData;
          }
          this.loading = false;
        }, 1000);
      }),
      (
        error => {
          setTimeout(() => {
            this.loading = false;
            notify(error.message, 'error', 5000);
          }, 1000);
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
        onClick: this.onOpenAddProjectPopup.bind(this)
      }
    });
  }

  //#region Popup

  onOpenAddProjectPopup(e): void {
    this.currentProject.state = ProjectFormState.CREATE;
    this.currentProject.data = new ProjectModel();
    this.currentProject.data.id = 0;

    this.projectFormComponent.open();

  }

  onOpenDetailButton(e, data): void {
    this.projectService.getProjectById(data.data.id).toPromise().then(
      ((responeseData: ProjectModel) => {
        this.currentProject.state = ProjectFormState.DETAIL;
        this.currentProject.data = new ProjectModel(responeseData);
        this.projectFormComponent.open();
        this.selectedIndex = 0;
      }),
      (
        error => {
          setTimeout(() => {
            this.loading = false;
            notify(error.message, 'error', 5000);
          }, 1000);
        }
      )
    )


  }

  saveProjectForm(data: ProjectModel) {
    if (data.id === 0) {
      this.projectService.add(data).toPromise().then(
        ((responeseData: ProjectModel) => {
          setTimeout(() => {
            notify("Add Project Success", "success", 5000);

            this.currentProject.state = ProjectFormState.DETAIL;
            this.currentProject.data = responeseData;
            this.selectedIndex = 1;
            this.getProjects();
            this.projectFormComponent.open();
          }, 1000)

        }),
        (
          error => {
            setTimeout(() => {
              this.loading = false;
              notify(error.error, 'error', 5000);
            }, 1000);
          }
        )
      )
    }
    else {
      this.projectService.edit(data).toPromise().then(
        ((responeseData: ProjectModel) => {
          setTimeout(() => {
            notify("Update Project Success", "success", 5000);

            this.currentProject.state = ProjectFormState.DETAIL;
            this.currentProject.data = responeseData;
            this.selectedIndex = 1;
            this.getProjects();
            this.projectFormComponent.open();
          }, 1000)

        }),
        (
          error => {
            setTimeout(() => {
              this.loading = false;
              notify(error.error, 'error', 5000);
            }, 1000);
          }
        )
      )
    }
  }

  deleteProject(data: boolean) {
    if (data) {
      this.projectService.delete(this.currentProject.data.id).toPromise().then(
        (() => {
          setTimeout(() => {
            notify("Delete Project Success", "success", 5000);
            this.projectFormComponent.close();
            this.getProjects();
          }, 1000)

        }),
        (
          error => {
            setTimeout(() => {
              this.loading = false;
              notify(error.error, 'error', 5000);
            }, 1000);
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
