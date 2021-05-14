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
import DataSource from 'devextreme/data/data_source';

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
  validationGR: any;
  isEnableDrag = false;
  isAddOrEditType = false;
  popupVisible = false;
  searchValue: any;
  treeListComp: any;
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
    const params = new HttpParams();
    this.service.getCriterias(params)
      .subscribe((result: any) => {
        this.employees = result;
      },
        (err: any) => {
          this.common.UI.toastMessage('Load data fail!!!', 'error', 2000);
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
            this.treeListComp.refresh();
            this.popupComp.hide();
          },
            (err: any) => {
              this.popupComp.hide();
              this.common.UI.toastMessage('Add Error', 'error', 2000);
            }
          );
      }
      else {
        this.service.editCriteria(data)
          .subscribe((result: any) => {
            this.treeListComp.refresh();
            this.popupComp.hide();
          },
            (err: any) => {
              this.popupComp.hide();
              this.common.UI.toastMessage('Edit Error!', 'error', 2000);
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
    this.criteriaModel = new Criteria();
    this.validationGR.reset();
  }
  onChangeSelect = (e: any) => {
    const a = 1;
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
    let strTitle = 'Criteria';
    const titleMode = mode === 'add' ? 'Add' : 'Edit';
    const strType = !isType ? '' : 'Type';
    strTitle = titleMode + ' ' + strTitle + ' ' + strType;
    this.popupComp.option('title', strTitle);
    this.popupComp.show();
    this.mode = mode === 'add' ? 'Add' : 'Edit';
  }
  onCheckBoxDragChange = (e: any) => {
    this.isEnableDrag = e.value;
    if (!e.value && e.previousValue) {
      // save change
      const data = this.treeListComp !== null ? this.treeListComp.getDataSource()._store._array : [];
      this.service.orderCriteria(data)
        .subscribe((result: any) => {
          this.treeListComp.refresh();
        },
          (err: any) => { }
        );
      return;
    }
  }
  onRowPrepared = (e: any) => {
    if (e.rowType === 'data' && e.data.typeId === null) {
      e.rowElement.style.backgroundColor = '#d1bdbd';
      e.rowElement.style.fontWeight = 'bold';
    }
  }
  onSearch = (e: any) => {
    this.searchValue = e.component.option('value');
    // reload dataSource
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
