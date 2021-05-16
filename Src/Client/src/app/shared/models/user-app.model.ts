export class UserApp {
    firstName: string;
    lastName: string;
    appRole: AppRolesEnum;
    projectRole: ProjectRoleModel;
    isSuccessful: boolean;
    email: string;
    token:string;

    constructor(init?: Partial<UserApp>) {
        Object.assign(this, init);
    }
}

export enum AppRolesEnum {
    ADMINISTRATOR = 1,
    USER = 2,
}

export enum ProjectRolesEnum {
    MEMBER = 1,
    QA = 2,
    PM = 3
}

export class ProjectRoleModel {
    projectId: Int16Array;
    projectRoleId: ProjectRolesEnum;
}