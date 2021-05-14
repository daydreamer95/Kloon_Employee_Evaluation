import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';

const mainUrl = '/Criteria';
const gettUrl = '/Criteria';
export class Criteria {
  id: number;
  typeId: number;
  name: string;
  description: string;
  orderNo: number;
}
@Injectable({
  providedIn: 'root'
})
export class Service {
  getCriterias(params: HttpParams): any {
    return this.http.get(gettUrl, { params });
  }
  // getEmployees(): Criteria[] {
  //   return datas;
  // }
  addCriteria(data: any): any {
    return this.http.post(mainUrl, data);
  }
  editCriteria(data: any): any {
    return this.http.put(mainUrl, data);
  }
  orderCriteria(data: any): any {
    const url = mainUrl + '/Order';
    return this.http.post(url, data);
  }
  constructor(private http: HttpClient) { }
}
