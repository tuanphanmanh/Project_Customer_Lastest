import { Component, Injector, OnInit, ViewChild } from '@angular/core';
import { CustomColumn, PaginationParamCustom } from '@app/shared/common/models/base.model';
import { AppComponentBase } from '@shared/common/app-component-base';
import { MstEsignColorOutputDto, MstEsignColorServiceProxy } from '@shared/service-proxies/service-proxies';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { CreateOrEditColorComponent } from './create-or-edit-color-modal.component';

@Component({
    selector: 'esign-color',
    templateUrl: './color.component.html',
    styleUrls: ['./color.component.less']
})
export class ColorComponent extends AppComponentBase implements OnInit {
    @ViewChild('createOrEditColor', { static: true }) createOrEditColor: | CreateOrEditColorComponent  | undefined;
    selectionSettings: { type: 'Single'};
    filterSettings: { type: 'FilterBar', mode: 'Immediate', immediateModeDelay: 150 };
    rowData: any[] = [];
    selectionRow: MstEsignColorOutputDto = new MstEsignColorOutputDto();
    pagination : PaginationParamCustom = { totalCount: 0, totalPage: 0, pageNum: 1, pageSize: 15 };
    columnDef : CustomColumn[] = [
        {
            header: this.l('Code'),
            field: 'code',
            width: 100,
            textAlign: 'Center',
            isPrimaryKey: true,
        },
        {
            header: this.l('Name'),
            field: 'name',
            width: 100,
            textAlign: 'Left',
        },
        {
            header: this.l('Order'),
            field: 'order',
            width: 100,
            textAlign: 'Center',
        },
        {
            header: this.l('Color'),
            field: 'code',
            width: 100,
            textAlign: 'Center',
            template: "PreviewColor"
        },
    ]
    code: string;
    name: string;
    constructor(
        injector: Injector,
        private _service: MstEsignColorServiceProxy,
        private _fileDownloadService: FileDownloadService
    ) {
        super(injector);
    }

    ngOnInit(): void {
        this.searchDatas();
    }

    searchDatas() {
        this._service.getAllMstEsignColor(
            this.code,
            this.name,
            this.pagination?.pageSize,
            this.pagination?.pageSize * (this.pagination?.pageNum - 1)
            )
            .subscribe((result) => {
                this.pagination.totalCount = result.totalCount;
                this.rowData = result.items;
            });
    }

    clearTextSearch() {
        this.code = '';
        this.name = '';
        this.searchDatas();
    }

    deleteRow(system: MstEsignColorOutputDto): void {
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
        this._service.getColorExcel(
            this.code,
            this.name,
            this.pagination?.pageSize,
            this.pagination?.pageSize * (this.pagination?.pageNum - 1)
            )
            .subscribe((result) => {
                this._fileDownloadService.downloadTempFile(result);
            });
    }

    changeSelection(e: any){
        this.selectionRow = e.data
    }

    changePager(event: any){
        this.pagination = event;
        this.searchDatas();
    }
}
