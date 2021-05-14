import { Component, NgModule, OnInit, ViewChild } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { DxButtonModule, DxDataGridModule, DxPopupModule } from 'devextreme-angular';
import { ProjectFormComponent, ProjectFormModel, ProjectFormModule, ProjectFormState } from '../../shared/components/project-form/project-form.component';
import { ProjectModel, ProjectService } from '../../shared/services/project.service';


@Component({
  selector: 'app-project',
  templateUrl: './project.component.html',
  styleUrls: ['./project.component.scss']
})
export class ProjectComponent implements OnInit {

  // //#region  Init variable
  dataSource: ProjectModel[];
  gridColumns = ['name', 'description', 'statusText'];

  @ViewChild(ProjectFormComponent) projectFormComponent: ProjectFormComponent;

  currentProject: ProjectFormModel = new ProjectFormModel();

  //#endregion
  constructor(
    private projectService: ProjectService
  ) {
    this.dataSource = projectService.getProjects();
  }

  ngOnInit(): void {
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
    
    this.projectFormComponent.open();
  }

  onOpenDetailButton(e, data): void {
    this.currentProject.state = ProjectFormState.DETAIL;
    this.currentProject.data = new ProjectModel(data.data);

    this.projectFormComponent.open();
  }

  //#endregion

}


@NgModule({
  imports: [
    BrowserModule,
    DxDataGridModule,
    DxButtonModule,
    DxPopupModule,
    ProjectFormModule
  ],
  declarations: [ProjectComponent],
  bootstrap: [ProjectComponent]
})
export class ProjectModule {

}
