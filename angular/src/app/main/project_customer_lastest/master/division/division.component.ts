import { Component, Injector, OnInit, ViewChild } from '@angular/core';
import { CustomColumn, PaginationParamCustom } from '@app/shared/common/models/base.model';
import { AppComponentBase } from '@shared/common/app-component-base';
import { MstEsignDivisionOutputDto, MstEsignDivisionServiceProxy } from '@shared/service-proxies/service-proxies';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { CreateOrEditDivisionComponent } from './create-or-edit-division-modal.component';
@Component({
    selector: 'esign-division',
    templateUrl: './division.component.html',
    styleUrls: ['./division.component.less'],
})
export class DivisionComponent extends AppComponentBase implements OnInit {
    @ViewChild('createOrEditDivision', { static: true }) createOrEditDivision: | CreateOrEditDivisionComponent  | undefined;
    selectionSettings: { type: 'Single'};
    filterSettings: { type: 'FilterBar', mode: 'Immediate', immediateModeDelay: 150 };
    rowDataDivision: any[] = [];
    selectionRow: MstEsignDivisionOutputDto = new MstEsignDivisionOutputDto();
    pagination : PaginationParamCustom = { totalCount: 0, totalPage: 0, pageNum: 1, pageSize: 10 };
    columnDeftDivision : CustomColumn[] = [
        {
            header: this.l('Code'),
            field: 'code',
            width: 100,
            textAlign: 'Left',
            isPrimaryKey: true,
        },
        {
            header: this.l('LocalName'),
            field: 'localName',
            width: 100,
            textAlign: 'Left',
        },
        {
            header: this.l('InternationalName'),
            field: 'internationalName',
            width: 100,
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
    ]
    code: string;
    localName: string;
    constructor(
        injector: Injector,
        private _service: MstEsignDivisionServiceProxy,
        private _fileDownloadService: FileDownloadService
    ) {
        super(injector);
    }

    ngOnInit(): void {
        this.searchDatas();
    }

    searchDatas() {
        this._service.getAllDivision(
            this.code,
            this.localName,
            this.pagination?.pageSize,
            this.pagination?.pageSize * (this.pagination?.pageNum - 1)
            )
            .subscribe((result) => {
                this.pagination.totalCount = result.totalCount;
                this.rowDataDivision = result.items;
            });
    }

    clearTextSearch() {
        this.code = '';
        this.localName = '';
        this.searchDatas();
    }

    deleteRow(system: MstEsignDivisionOutputDto): void {
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
        this._service.getDivisionExcel(
            this.code,
            this.localName,
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
