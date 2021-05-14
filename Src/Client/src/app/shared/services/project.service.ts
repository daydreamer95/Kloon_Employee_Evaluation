import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Router } from "@angular/router";

const apiUrl = {
  urlProjectCreate: '',
  urlProjectEdit: '',
  urlProjectDelete: '',
  urlProjectGetAll: '',
  urlProjectGetById: ''
}

@Injectable({
  providedIn: 'root'
})
export class ProjectService {
  constructor(private router: Router, private httpClient: HttpClient) {

  }

  //#region GET

  public getProjects() {
    let data: ProjectModel[] = [
      {
        id: 1,
        name: 'Rethink',
        description: 'Dự án của Agenda4',
        status: 1,
        statusText: "Open"
      },
      {
        id: 2,
        name: 'Abacus',
        description: 'Dự án của Abacus',
        status: 2,
        statusText: "Pending"
      },
      {
        id: 3,
        name: 'Internal',
        description: 'Dự án nội bộ của Kloon',
        status: 3,
        statusText: "Closed"
      },
      {
        id: 4,
        name: 'OSE',
        description: 'Thi Tiếng anh trực tuyến',
        status: 3,
        statusText: 'Closed'
      }
    ];
    return data;
  }
  //#endregion
}

export class ProjectModel {
  id: number;
  name: string;
  description: string;
  status: number;
  statusText: string;

  constructor(init?:Partial<ProjectModel>){
    Object.assign(this, init);
  }
}
