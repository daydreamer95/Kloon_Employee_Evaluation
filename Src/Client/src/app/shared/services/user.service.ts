import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Router } from "@angular/router";

const apiUrl = {
    urlCreateUser: '',
    urlDeleteUser: '',
    urlEditUser: ''
};

@Injectable({
    providedIn: 'root',
  })
export class UserService{
    constructor(private router: Router,private httpClient: HttpClient) { 
        
    }

    //#region GET
    public getUsers()
    {
        let tempListUser: UserModel[] = [
            {
                email: 'lehuy1@gmail.com',
                firstName: 'Le',
                lastName: 'Huy',
                position: 'CEO',
                sex: 'male',
                doB: new Date(),
                phoneNo: '123456789'
            },
            {
                email: 'lehuy2@gmail.com',
                firstName: 'Le',
                lastName: 'Huy 2',
                position: 'CEO',
                sex: 'male',
                doB: new Date(),
                phoneNo: '123456789'
            },
            {
                email: 'lehuy3@gmail.com',
                firstName: 'Le',
                lastName: 'Huy 3',
                position: 'CEO',
                sex: 'male',
                doB: new Date(),
                phoneNo: '123456789'
            }
        ];
        return tempListUser;
    }

    //#endregion

}

export class UserModel{
    email: string;
    firstName: string;
    lastName: string;
    position: any;
    sex: any;
    doB: Date;
    phoneNo: any;

    constructor(init?:Partial<UserModel>) {
        Object.assign(this, init);
    }
}