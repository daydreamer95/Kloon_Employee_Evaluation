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
  