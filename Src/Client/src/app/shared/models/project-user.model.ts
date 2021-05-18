export class ProjectUserModel {
    id: string;
    userId: number;
    projectId: number;
    projectName: string;
    email: string;
    firstName: string;
    lastName: string;
    projectRoleId: number;
    projectRole: string;
  
    constructor(init?: Partial<ProjectUserModel>) {
      Object.assign(this, init);
    }
  }
  