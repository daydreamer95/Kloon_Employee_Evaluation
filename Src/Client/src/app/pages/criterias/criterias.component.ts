import { Component, OnInit, NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { Service, Criteria } from './service';
import {
  DxTreeListModule,
  DxButtonModule,
  DxTextBoxModule,
  DxCheckBoxModule,
  DxPopupModule,
  DxTextAreaModule,
  DxSelectBoxModule,
  DxValidatorModule,
  DxValidationSummaryModule
} from 'devextreme-angular';
// import { Console } from 'console';

@Component({
  selector: 'app-criterias',
  templateUrl: './criterias.component.html',
  styleUrls: ['./criterias.component.scss'],
  providers: [Service],
})
export class CriteriasComponent implements OnInit {
  employees: Array<Criteria>;
  criteriaModel = new Criteria();
  lookupData: any;
  isEnableDrag = false;
  isAddOrEditType = false;
  popupVisible = false;
  searchValue: any;
  treeListComp: any;
  popupComp: any;
  closeButtonOptions = {
    text: 'Close', icon: 'remove',
    onClick: (e: any) => {
      e.validationGroup.reset();
      this.popupComp.hide();
    }
  };
  saveButtonOptions = {
    text: 'Save', type: 'success', icon: 'save',
    onClick: (e: any) => { this.onSave(e); }
  };

  expandedRowKeys: Array<number> = [1, 2];
  constructor(private service: Service) {
    this.employees = service.getEmployees();
  }

  ngOnInit(): void {
    this.lookupData = this.employees.filter(x => x.TypeId == null);
  }

  onDragChange(e: any): any {
    const visibleRows = e.component.getVisibleRows();
    const sourceNode = e.component.getNodeByKey(e.itemData.Id);
    let targetNode = visibleRows[e.toIndex].node;

    while (targetNode && targetNode.data) {
      if (targetNode.data.Id === sourceNode.data.Id) {
        e.cancel = true;
        break;
      }
      targetNode = targetNode.parent;
    }
  }
  onSave = (e: any) => {
    const validateData = e.validationGroup.validate();
    if (validateData && validateData.brokenRules && validateData.brokenRules.length > 0) {
      validateData.brokenRules[0].validator.focus();
    }
    if (validateData.isValid) {
      const data = new Criteria();
      Object.assign(data, this.criteriaModel);
      this.service.saveCriteria(data)
        .subscribe((result: any) => {
          this.treeListComp.refresh();
        },
          (err: any) => { }
        );
    }
  }
  onInitTreeList = (e: any) => {
    this.treeListComp = e.component;
  }
  onInitPopup = (e: any) => {
    this.popupComp = e.component;
  }
  onHidingPopup = (e: any) => {
    this.criteriaModel = new Criteria();
    // this.criteriaModel.Id = null;
    // this.criteriaModel.Name = '';
    // this.criteriaModel.Description = '';
    // this.criteriaModel.TypeId = null;
    // this.criteriaModel.OrderNo = null;
  }
  onChangeSelect = (e: any) => {
    const a = 1;
  }
  onClickAdd = (e: any, data: any) => {
    this.isAddOrEditType = false;
    this.onShowPopup('add', false);
    this.criteriaModel.TypeId = data.Id;
  }
  onClickAddType = (e: any) => {
    this.isAddOrEditType = true;
    this.onShowPopup('add', true);
  }
  onClickEdit = (e: any, data: any) => {
    const isType = data.TypeId === null;
    this.isAddOrEditType = isType;
    this.onBindingModel(data);
    this.onShowPopup('edit', isType);
  }
  onBindingModel = (data: Criteria) => {
    this.criteriaModel.Id = data.Id;
    this.criteriaModel.TypeId = data.TypeId;
    this.criteriaModel.Name = data.Name;
    this.criteriaModel.Description = data.Description;
    this.criteriaModel.OrderNo = data.OrderNo;
  }
  onShowPopup = (mode: any, isType: boolean) => {
    let strTitle = 'Criteria';
    const titleMode = mode === 'add' ? 'Add' : 'Edit';
    const strType = !isType ? '' : 'Type';
    strTitle = titleMode + ' ' + strTitle + ' ' + strType;
    this.popupComp.option('title', strTitle);
    this.popupComp.show();
  }
  onCheckBoxDragChange = (e: any) => {
    this.isEnableDrag = e.value;
    if (!e.value && e.previousValue) {
      // save change
      // const data = this.treeListComp !== null ? this.treeListComp.getDataSource()._store._array : [];
      return;
    }
  }
  onRowPrepared = (e: any) => {
    if (e.rowType === 'data' && e.data.TypeId === null) {
      e.rowElement.style.backgroundColor = '#d1bdbd';
      e.rowElement.style.fontWeight = 'bold';
    }
  }
  onSearch = (e: any) => {
    this.searchValue = e.component.option('value');
    this.employees = this.service.getEmployees().filter(x => x.Name.toLocaleLowerCase().indexOf(this.searchValue.toLocaleLowerCase()) > -1);
    // reload dataSource
  }
  onReorder = (e: any): any => {
    const visibleRows = e.component.getVisibleRows();
    const sourceData = e.itemData;
    const targetData = visibleRows[e.toIndex].data;
    if (e.dropInsideItem) {
      if (targetData.TypeId !== (null) || e.itemData.TypeId === null) { return; }
      e.itemData.TypeId = targetData.Id;
      e.component.refresh();
    } else {
      const sourceIndex = this.employees.indexOf(sourceData);
      let targetIndex = this.employees.indexOf(targetData);
      // cannot drop to chill
      if (sourceData.TypeId === null && targetData.TypeId !== null) {
        return;
      }
      // cannot drop chill to parrent
      if (sourceData.TypeId !== null && targetData.TypeId === null) {
        return;
      }
      if (sourceData.TypeId !== targetData.TypeId) {
        sourceData.TypeId = targetData.TypeId;
        if (e.toIndex > e.fromIndex) {
          targetIndex++;
        }
      }

      this.employees.splice(sourceIndex, 1);
      this.employees.splice(targetIndex, 0, sourceData);
    }
  }
  getDataSource = (comp: any) => {
    return comp.getDataSource()._store._array;
  }
}
@NgModule({
  imports: [
    DxTreeListModule,
    DxButtonModule,
    DxTextBoxModule,
    DxCheckBoxModule,
    DxPopupModule,
    BrowserModule,
    DxTextAreaModule,
    DxSelectBoxModule,
    DxValidatorModule,
    DxValidationSummaryModule
  ],
  declarations: [CriteriasComponent],
  bootstrap: [CriteriasComponent]
})
export class CriteriasModule { }
