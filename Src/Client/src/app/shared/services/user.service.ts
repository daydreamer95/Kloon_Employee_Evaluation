import { LoginModel } from './../models/login.model';
import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Router } from "@angular/router";
import { UserModel } from '../models/user.model';

const apiUrl = '/user'

@Injectable({
    providedIn: 'root',
})
export class UserService {
    loginModel = new LoginModel();

    constructor(private router: Router, private httpClient: HttpClient) {
    }

    public login(username, password) {
        this.loginModel.email = username;
        this.loginModel.password = password;
        this.loginModel.rememberMe = false;

        return this.httpClient.post(`/account`, this.loginModel);
    }

    // GET
    public getUsers(stringText) {
        let queryString = !stringText && stringText.length === 0 ? "" : '?searchText=${stringText}';
        return this.httpClient.get<UserModel[]>(apiUrl + queryString);
    }

    public getUserById(id: number) {
        return this.httpClient.get<UserModel>(apiUrl + '/' + id);
    }

    //POST

    public add(entity: UserModel) {
        return this.httpClient.post<UserModel>(apiUrl, entity);
    }

    //PUT

    public edit(entity: UserModel) {
        return this.httpClient.put<UserModel>(apiUrl, entity);
    }

    //DELETE

    public delete(id: number) {
        return this.httpClient.delete<boolean>(apiUrl + `/` + id);
    }

}
