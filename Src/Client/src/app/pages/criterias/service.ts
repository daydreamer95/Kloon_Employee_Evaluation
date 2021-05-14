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

// const datas = [{
//   Id: 1,
//   TypeId: null,
//   Name: 'Bo 1',
//   Description: 'Dr.',
//   OrderNo: 1
// },
// {
//   Id: 3,
//   TypeId: 1,
//   Name: 'Con 1-1',
//   Description: 'Dr.',
//   OrderNo: 2
// },
// {
//   Id: 4,
//   TypeId: 1,
//   Name: 'Con 1-2',
//   Description: 'Dr.',
//   OrderNo: 3
// },
// {
//   Id: 5,
//   TypeId: null,
//   Name: 'Bo 2',
//   Description: 'Dr.',
//   OrderNo: 4
// },
// {
//   Id: 6,
//   TypeId: 5,
//   Name: 'Con 2-1',
//   Description: 'Dr.',
//   OrderNo: 5
// }];
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
    return this.http.post(mainUrl, data);
  }
  constructor(private http: HttpClient) { }
}
