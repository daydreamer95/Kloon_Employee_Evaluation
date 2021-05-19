import { CommonService } from './../../shared/services/common.service';
import { Component, OnInit, NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { Service, Criteria } from './service';
import { HttpParams } from '@angular/common/http';
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

@Component({
  selector: 'app-criterias',
  templateUrl: './criterias.component.html',
  styleUrls: ['./criterias.component.scss'],
  providers: [Service],
})
export class CriteriasComponent implements OnInit {
  employees: any;
  criteriaModel = new Criteria();
  lookupData: any;
  mode: any;
  expanded = true;
  validationGR: any;
  isEnableDrag = false;
  isAddOrEditType = false;
  isViewDetail = false;
  popupVisible = false;
  searchValue = '';
  treeListComp: any;
  btnAddTypeComp: any;
  popupComp: any;
  closeButtonOptions = {
    text: 'Close', icon: 'remove',
    onClick: (e: any) => {
      this.popupComp.hide();
      e.validationGroup.reset();
    }
  };
  saveButtonOptions = {
    text: 'Save', type: 'success', icon: 'save',
    onClick: (e: any) => { this.onSave(e); }
  };

  expandedRowKeys: Array<number> = [1, 2];
  constructor(private service: Service, private common: CommonService) {
  }

  ngOnInit(): void {
    this.init();
  }

  init(): any {
    const params: HttpParams = new HttpParams()
      .set('key', this.searchValue);
    this.service.getCriterias(params)
      .subscribe((result: any) => {
        this.employees = result;
        this.lookupData = result.filter(x => x.typeId === null);
      },
        (err: any) => {
          this.common.UI.multipleNotify('Load data fail!!!', 'error', 2000);
        }
      );
  }
  onDragChange(e: any): any {
    const visibleRows = e.component.getVisibleRows();
    const sourceNode = e.component.getNodeByKey(e.itemData.id);
    let targetNode = visibleRows[e.toIndex].node;

    while (targetNode && targetNode.data) {
      if (targetNode.data.id === sourceNode.data.id) {
        e.cancel = true;
        break;
      }
      targetNode = targetNode.parent;
    }
  }
  onSave = (e: any) => {
    this.validationGR = e.validationGroup;
    const validateData = e.validationGroup.validate();
    if (validateData && validateData.brokenRules && validateData.brokenRules.length > 0) {
      validateData.brokenRules[0].validator.focus();
    }
    if (validateData.isValid) {
      const data = new Criteria();
      Object.assign(data, this.criteriaModel);
      if (this.mode === 'Add') {
        this.service.addCriteria(data)
          .subscribe((result: any) => {
            this.init();
            this.popupComp.hide();
          },
            (err: any) => {
              this.popupComp.hide();
              this.common.UI.multipleNotify('Add Error', 'error', 2000);
            }
          );
      }
      else {
        this.service.editCriteria(data)
          .subscribe((result: any) => {
            this.init();
            this.popupComp.hide();
          },
            (err: any) => {
              this.popupComp.hide();
              this.common.UI.multipleNotify('Edit Error!', 'error', 2000);
            }
          );
      }

    }
  }
  onInitTreeList = (e: any) => {
    this.treeListComp = e.component;
  }
  onInitPopup = (e: any) => {
    this.popupComp = e.component;
  }
  onHidingPopup = (e: any) => {
    if (this.validationGR != null) {
      // this.criteriaModel = new Criteria();
      this.validationGR.reset();
    }
    this.isViewDetail = false;
  }
  onClickAdd = (e: any, data: any) => {
    this.isAddOrEditType = false;
    this.onShowPopup('add', false);
    this.criteriaModel.typeId = data.id;
  }
  onClickAddType = (e: any) => {
    this.isAddOrEditType = true;
    this.onShowPopup('add', true);
  }
  onClickEdit = (e: any, data: any) => {
    const isType = data.typeId === null;
    this.isAddOrEditType = isType;
    this.onBindingModel(data);
    this.onShowPopup('edit', isType);
  }
  onBindingModel = (data: Criteria) => {
    this.criteriaModel.id = data.id;
    this.criteriaModel.typeId = data.typeId;
    this.criteriaModel.name = data.name;
    this.criteriaModel.description = data.description;
    this.criteriaModel.orderNo = data.orderNo;
  }
  onShowPopup = (mode: any, isType: boolean) => {
    let titleMode = '';
    switch (mode) {
      case 'add':
        titleMode = 'Add';
        break;
      case 'edit':
        titleMode = 'Edit';
        break;
      case 'detail':
        titleMode = 'Detail';
        break;
      default:
      // code block
    }
    let strTitle = 'Criteria';
    const strType = !isType ? '' : 'Type';
    strTitle = titleMode + ' ' + strTitle + ' ' + strType;
    this.popupComp.option('title', strTitle);
    this.popupComp.show();
    this.mode = mode === 'add' ? 'Add' : 'Edit';
  }
  onCheckBoxDragChange = (e: any) => {
    this.isEnableDrag = e.value;
    this.btnAddTypeComp.option('disabled', this.isEnableDrag);
    if (!e.value && e.previousValue) {
      // save change
      const data = this.treeListComp !== null ? this.treeListComp.getDataSource()._store._array : [];
      this.service.orderCriteria(data)
        .subscribe((result: any) => {
          this.init();
        },
          (err: any) => { }
        );
    }
    return;
  }
  onRowPrepared = (e: any) => {
    if (e.rowType === 'data' && e.data.typeId === null) {
      e.rowElement.style.backgroundColor = '#d1bdbd';
      e.rowElement.style.fontWeight = 'bold';
    }
  }
  onToolbarPreparing = (e: any) => {
    e.toolbarOptions.items.unshift({
      location: 'before',
      widget: 'dxCheckBox',
      options: {
        width: 200,
        text: ' Re-arrange (Drag & Drop)',
        value: this.isEnableDrag,
        onValueChanged: this.onCheckBoxDragChange.bind(this)
      }
    },
      {
        location: 'after',
        widget: 'dxButton',
        options: {
          width: 136,
          text: 'Collapse All',
          onClick: this.collapseAllClick.bind(this)
        }
      }, {
      location: 'after',
      widget: 'dxButton',
      options: {
        text: 'Add',
        icon: 'plus',
        type: 'success',
        onInitialized: this.onInitBtnAddType.bind(this),
        onClick: () => { this.onClickAddType(e); }
      }
    });
  }
  onInitBtnAddType = (e: any) => {
    this.btnAddTypeComp = e.component;
  }
  collapseAllClick = (e: any) => {
    this.expanded = !this.expanded;
    e.component.option({
      text: this.expanded ? 'Collapse All' : 'Expand All'
    });
    if (this.expanded) {
      this.treeListComp.option('expandedRowKeys', []);
    } else {
      const expanItem = [];
      for (let i = 0; i < this.lookupData.length; i++) {
        expanItem.push(i);
      }
      this.treeListComp.option('expandedRowKeys', expanItem);
    }
  }
  oncellDblClick = (e: any) => {
    const isType = e.data.typeId === null;
    this.isViewDetail = true;
    this.onBindingModel(e.data);
    this.isAddOrEditType = isType;
    this.onShowPopup('detail', isType);
  }
  onSearch = (e: any) => {
    this.searchValue = e.component.option('value');
    this.init();
  }
  onReorder = (e: any) => {
    const visibleRows = e.component.getVisibleRows();
    const sourceData = e.itemData;
    const targetData = visibleRows[e.toIndex].data;
    if (e.dropInsideItem) {
      if (targetData.typeId !== (null) || e.itemData.typeId === null) { return; }
      e.itemData.typeId = targetData.id;
      e.component.refresh();
    } else {
      const sourceIndex = this.employees.indexOf(sourceData);
      let targetIndex = this.employees.indexOf(targetData);
      // cannot drop to chill
      if (sourceData.typeId === null && targetData.typeId !== null) {
        return;
      }
      // cannot drop chill to parrent
      if (sourceData.typeId !== null && targetData.typeId === null) {
        return;
      }
      if (sourceData.typeId !== targetData.typeId) {
        sourceData.typeId = targetData.typeId;
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
