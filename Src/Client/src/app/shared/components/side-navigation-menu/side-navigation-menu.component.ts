import { Component, NgModule, Output, Input, EventEmitter, ViewChild, ElementRef, AfterViewInit, OnDestroy } from '@angular/core';
import { DxTreeViewModule, DxTreeViewComponent } from 'devextreme-angular/ui/tree-view';
import { navigation } from '../../../app-navigation';

import * as events from 'devextreme/events';
import { Router } from '@angular/router';
import { AuthGuardService } from '../../services';

@Component({
  selector: 'app-side-navigation-menu',
  templateUrl: './side-navigation-menu.component.html',
  styleUrls: ['./side-navigation-menu.component.scss']
})
export class SideNavigationMenuComponent implements AfterViewInit, OnDestroy {
  @ViewChild(DxTreeViewComponent, { static: true })
  menu: DxTreeViewComponent;

  @Output()
  selectedItemChanged = new EventEmitter<string>();

  @Output()
  openMenu = new EventEmitter<any>();

  private _selectedItem: String;
  @Input()
  set selectedItem(value: String) {
    this._selectedItem = value;
    if (!this.menu.instance) {
      return;
    }

    this.menu.instance.selectItem(value);
  }

  private _items;
  get items() {
    if (!this._items) {
      let navigationList = this.getItemMenuByRole();

      this._items = navigationList.map((item) => {
        if(item.path && !(/^\//.test(item.path))){ 
          item.path = `/${item.path}`;
        }
         return { ...item, expanded: !this._compactMode }
        });
    }
    return this._items;
  }

  getItemMenuByRole(){
    let result = navigation.filter(item => {
      //Parent menu have list item
      if(item.path == undefined && item.items){
        item.items = this.getAllowedPages(item.items);
      }

      if(item.path == undefined && item.items.length == 0)
      {
        return true;
      }
      var pathString = item.path.substring(1,item.path.length);
      const itemRouteConfig = this.router.config.filter(function(routeConfig){
        return routeConfig.path == pathString;
      });

      //
      if(itemRouteConfig && itemRouteConfig.length > 0){
        let pathAllowedRoles = itemRouteConfig[0]['data']['allowedRoles'];

        var isAllowedAccess = this.authGuardService.isAuthorized(pathAllowedRoles);
        return isAllowedAccess;
      }
      
      return false;
    })
    return result;
  }

  getAllowedPages(arrayItems: any[]){
    if(arrayItems.length == 0)
      return [];

    let result = arrayItems.filter(item =>{
      const itemRouteConfig = this.router.config.filter(function(routeConfig){
        if(item.path.includes("/")){
          item.path = item.path.substring(1, item.path.length);
        }
        return routeConfig.path == item.path;
      });

      if(itemRouteConfig && itemRouteConfig.length > 0){
        let pathAllowedRoles = itemRouteConfig[0]['data']['allowedRoles'];

        var isAllowedAccess = this.authGuardService.isAuthorized(pathAllowedRoles);
        return isAllowedAccess;
      }
      return false;
    })
    return result;
  }

  private _compactMode = false;
  @Input()
  get compactMode() {
    return this._compactMode;
  }
  set compactMode(val) {
    this._compactMode = val;

    if (!this.menu.instance) {
      return;
    }

    if (val) {
      this.menu.instance.collapseAll();
    } else {
      this.menu.instance.expandItem(this._selectedItem);
    }
  }

  constructor(private elementRef: ElementRef, private router: Router, private authGuardService:AuthGuardService) { }

  onItemClick(event) {
    this.selectedItemChanged.emit(event);
  }

  ngAfterViewInit() {
    events.on(this.elementRef.nativeElement, 'dxclick', (e) => {
      this.openMenu.next(e);
    });
  }

  ngOnDestroy() {
    events.off(this.elementRef.nativeElement, 'dxclick');
  }
}

@NgModule({
  imports: [ DxTreeViewModule ],
  declarations: [ SideNavigationMenuComponent ],
  exports: [ SideNavigationMenuComponent ]
})
export class SideNavigationMenuModule { }
