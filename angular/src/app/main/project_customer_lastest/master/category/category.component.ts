import { Component, Injector, OnInit, ViewChild } from '@angular/core';
import { CustomColumn, PaginationParamCustom } from '@app/shared/common/models/base.model';
import { AppComponentBase } from '@shared/common/app-component-base';
import { CreateOrEditMstEsignCategoryDto, MstEsignCategoryDto, MstEsignCategoryServiceProxy } from '@shared/service-proxies/service-proxies';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { CreateOrEditCategoryComponent } from './create-or-edit-category-modal.component';

@Component({
  selector: 'esign-category',
  templateUrl: './category.component.html',
  styleUrls: ['./category.component.less']
})
export class CategoryComponent extends AppComponentBase implements OnInit {
  @ViewChild('createOrEditCategory', { static: true }) createOrEditCategory: | CreateOrEditCategoryComponent  | undefined;

  code: string;
  name: string;
  selectionSettings: { type: 'Single' };
  filterSettings: { type: 'FilterBar', mode: 'Immediate', immediateModeDelay: 150 };
  rowData: any[] = [];
  selectionRow: CreateOrEditMstEsignCategoryDto = new CreateOrEditMstEsignCategoryDto();
  pagination: PaginationParamCustom = { totalCount: 0, totalPage: 0, pageNum: 1, pageSize: 10 };

  columnDef: CustomColumn[] =
    [
      {
        header: this.l('Code'),
        field: 'code',
        width: 60,
        textAlign: 'Left',
        isPrimaryKey: true,
      },
      {
        header: this.l('LocalName'),
        field: 'localName',
        width: 50,
        textAlign: 'Left',
      },
      {
        header: this.l('InternationalName'),
        field: 'internationalName',
        width: 60,
        textAlign: 'Left',
      },
      {
        header: this.l('LocalDescription'),
        field: 'localDescription',
        width: 100,
        textAlign: 'Left',
      },
      {
        header: this.l('InternationalDescription'),
        field: 'internationalDescription',
        width: 100,
        textAlign: 'Left',
      },
      {
        header: this.l('IsMadatory'),
        field: 'isMadatory',
        width: 60,
        textAlign: 'Left',
        template: 'CheckBox',
        disabled: true
      },
        {
        header: this.l('DivisionCode'),
        field: 'divisionCode',
        width: 60,
        textAlign: 'Left',
        },
    ]

  constructor(
    injector: Injector,
    private _service: MstEsignCategoryServiceProxy,
    private _fileDownloadService: FileDownloadService
  ) {
    super(injector)
  }

  ngOnInit() {
    this.searchDatas();
  }

  searchDatas() {
    this._service.getAllMstEsignCategories(
      this.code,
      this.name,
      this.pagination?.pageSize,
      this.pagination?.pageSize * (this.pagination?.pageNum - 1)
    ).subscribe(res => {
      this.pagination.totalCount = res.totalCount;
      this.rowData = res.items;
    });
  }

  clearTextSearch() {
    this.code = '';
    this.name = '';
    this.searchDatas();
  }

  changeSelection(e: any) {
    this.selectionRow = new CreateOrEditMstEsignCategoryDto({
        id: e.data.id,
        code: e.data.code,
        localName: e.data.localName,
        internationalName: e.data.internationalName,
        localDescription: e.data.localDescription,
        internationalDescription: e.data.internationalDescription,
        isMadatory: e.data.isMadatory,
        divisionId: e.data.divisionId,
    });
  }

  changePager(event: any) {
    this.pagination = event;
    this.searchDatas();
  }

  deleteRow(system: MstEsignCategoryDto){
    this.message.confirm(this.l('AreYouSure'), 'Delete Row', (isConfirmed) => {
      if (isConfirmed) {
          this._service.delete(system.id).subscribe(() => {
              this.searchDatas();
              this.notify.success(this.l('SuccessfullyDeleted'));
          });
      }
  });
  }

  exportToExcel(): void {
    this._service.getSystem(
        this.code,
        this.name,
        this.pagination?.pageSize,
        this.pagination?.pageSize * (this.pagination?.pageNum - 1)
        )
        .subscribe((result) => {
            this._fileDownloadService.downloadTempFile(result);
        });
  }

}
