import { HttpClient, HttpParams } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Router } from "@angular/router";
import { environment } from "../../../environments/environment";
import { ApiResponse } from "../models/api-response.model";

const apiUrl = {
  urlProjectCreate: `/Projects`,
  urlProjectEdit: '/Projects',
  urlProjectDelete: '/Projects',
  urlProjectGetAll: '/Projects',
  urlProjectGetById: '/Projects'
}

@Injectable({
  providedIn: 'root'
})
export class ProjectService {
  constructor(private router: Router, private httpClient: HttpClient) {

  }

  //#region GET

  public getProjects() {
    return this.httpClient.get<ProjectModel[]>(apiUrl.urlProjectGetAll);
  }

  public getProjectById(id: number){
    return this.httpClient.get<ProjectModel>(apiUrl.urlProjectGetById + "/" + id);
  }

  //#endregion

  //#region POST

  public add(entity: ProjectModel) {
    return this.httpClient.post<ProjectModel>(apiUrl.urlProjectCreate, entity);
  }

  //#endregion

  //#region PUT

  public edit(entity: ProjectModel) {
    return this.httpClient.put<ProjectModel>(apiUrl.urlProjectEdit, entity);
  }

  //#endregion

  //#region PUT

  public delete(id: number) {
    return this.httpClient.delete<boolean>(apiUrl.urlProjectDelete + `/${id}`);
  }

  //#endregion
}

export class ProjectModel {
  id: number;
  name: string;
  description: string;
  status: number;
  statusText: string;

  constructor(init?: Partial<ProjectModel>) {
    Object.assign(this, init);
  }
}
