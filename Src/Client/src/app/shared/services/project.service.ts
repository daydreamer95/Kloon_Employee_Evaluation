import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Router } from "@angular/router";
import { ProjectModel } from "../models/project.model";

const apiUrl = {
  urlProjectCreate: `/Project`,
  urlProjectEdit: '/Project',
  urlProjectDelete: '/Project',
  urlProjectGetAll: '/Project',
  urlProjectGetById: '/Project'
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

  public getProjectById(id: number) {
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

  //#region DELETE

  public delete(id: number) {
    return this.httpClient.delete<boolean>(apiUrl.urlProjectDelete + `/${id}`);
  }

  //#endregion
}

