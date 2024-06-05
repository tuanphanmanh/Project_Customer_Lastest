import { Component, Injector, OnInit, ViewChild } from '@angular/core';
import { CustomColumn, PaginationParamCustom } from '@app/shared/common/models/base.model';
import { AppComponentBase } from '@shared/common/app-component-base';
import { MstActivityHistoryDto, MstActivityHistoryServiceProxy } from '@shared/service-proxies/service-proxies';
import { FileDownloadService } from '@shared/utils/file-download.service';

@Component({
    selector: 'esign-activity-history',
    templateUrl: './activity-history.component.html',
    styleUrls: ['./activity-history.component.less']
})
export class ActivityHistoryComponent extends AppComponentBase implements OnInit {
    @ViewChild('createOrEditActivityHistory', { static: true }) createOrEditActivityHistory: | undefined;

    code: string;
    name: string;
    selectionSettings: { type: 'Single' };
    filterSettings: { type: 'FilterBar', mode: 'Immediate', immediateModeDelay: 150 };
    rowData: any[] = [];
    selectionRow: MstActivityHistoryDto = new MstActivityHistoryDto();
    pagination: PaginationParamCustom = { totalCount: 0, totalPage: 0, pageNum: 1, pageSize: 10 };

    columnDef: CustomColumn[] = [
        {
            header: this.l('Code'),
            field: 'code',
            width: 50,
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
            header: this.l('Description'),
            field: 'description',
            width: 200,
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
        private _service: MstActivityHistoryServiceProxy,
        private _fileDownloadService: FileDownloadService
    ) {
        super(injector)
    }

    ngOnInit() {
        this.searchDatas();
    }

    searchDatas() {
        this._service.getAllMstEsignActivityHistory(
            this.code,
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

    deleteRow(system: MstActivityHistoryDto) {
        this.message.confirm(this.l('AreYouSure'), this.l('Delete'), (isConfirmed) => {
            if (isConfirmed) {
                this._service.delete(system.id).subscribe(() => {
                    this.searchDatas();
                    this.notify.success(this.l('SuccessfullyDeleted'));
                });
            }
        });
    }

    exportToExcel(): void {
        this._service.getActivityHistory(
            this.code,
            this.pagination?.pageSize,
            this.pagination?.pageSize * (this.pagination?.pageNum - 1)
        )
            .subscribe((result) => {
                this._fileDownloadService.downloadTempFile(result);
            });
    }

}
