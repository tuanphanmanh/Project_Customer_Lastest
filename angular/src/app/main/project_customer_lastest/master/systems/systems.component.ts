import { Component, Injector, OnInit, ViewChild } from '@angular/core';
import { CustomColumn, PaginationParamCustom } from '@app/shared/common/models/base.model';
import { AppComponentBase } from '@shared/common/app-component-base';
import { MstEsignSystemsDto, MstEsignSystemsServiceProxy } from '@shared/service-proxies/service-proxies';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { CreateOrEditSystemsComponent } from './create-or-edit-systems-modal.component';

@Component({
  selector: 'esign-systems',
  templateUrl: './systems.component.html',
  styleUrls: ['./systems.component.less']
})
export class SystemsComponent extends AppComponentBase implements OnInit {
  @ViewChild('createOrEditSystems', { static: true }) createOrEditSystems: | CreateOrEditSystemsComponent  | undefined;

  code: string;
  name: string;
  selectionSettings: { type: 'Single' };
  filterSettings: { type: 'FilterBar', mode: 'Immediate', immediateModeDelay: 150 };
  rowData: any[] = [];
  selectionRow: MstEsignSystemsDto = new MstEsignSystemsDto();
  pagination: PaginationParamCustom = { totalCount: 0, totalPage: 0, pageNum: 1, pageSize: 10 };
  columnDef: CustomColumn[] = [
    {
      header: this.l('Code'),
      field: 'code',
      width: 20,
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
      width: 50,
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
    header: this.l('Image'),
    field: 'imgUrl',
    width: 30,
    textAlign: 'Center',
    template: 'PreviewImage'
},
  ]


  constructor(
    injector: Injector,
    private _service: MstEsignSystemsServiceProxy,
    private _fileDownloadService: FileDownloadService
  ) {
    super(injector)
  }

  ngOnInit() {
    this.searchDatas();
  }

  searchDatas() {
    this._service.getAllSystems(
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
    this.selectionRow = e.data
  }

  changePager(event: any) {
    this.pagination = event;
    this.searchDatas();
  }

  deleteRow(system: MstEsignSystemsDto){
    this.message.confirm(this.l('Are You Sure To Delete', system.code), 'Delete Row', (isConfirmed) => {
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
