import { Injectable } from '@angular/core';
import notify from 'devextreme/ui/notify';

@Injectable({
  providedIn: 'root'
})
export class CommonService {
  UI = {
    toastMessage(messages: string, type: string, time: any): void {
      const option = {
        position: {
          my: 'top right',
          at: 'top right',
          of: window,
          offset: '-5 10'
        },
        maxWidth: '400px',
        minHeight: '60px',
        message: messages

      };
      return notify(option, type, time);
    }
  };

  constructor() { }
}
