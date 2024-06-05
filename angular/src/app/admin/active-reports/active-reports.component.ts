import { Component, Injector, OnInit } from '@angular/core';
import { CustomColumn, PaginationParamCustom } from '@app/shared/common/models/base.model';
import { AppComponentBase } from '@shared/common/app-component-base';
import { EsignActivityReportDto, EsignActivityReportServiceProxy } from '@shared/service-proxies/service-proxies';
import { FileDownloadService } from '@shared/utils/file-download.service';

@Component({
    selector: 'esign-active-reports',
    templateUrl: './active-reports.component.html',
    styleUrls: ['./active-reports.component.less']
})
export class ActiveReportsComponent extends AppComponentBase implements OnInit {

    filterName: string;
    filterEmail: string;
    selectionSettings: { type: 'Single' };
    filterSettings: { type: 'FilterBar', mode: 'Immediate', immediateModeDelay: 150 };
    rowData: any[] = [];
    selectionRow: EsignActivityReportDto = new EsignActivityReportDto();
    pagination: PaginationParamCustom = { totalCount: 0, totalPage: 0, pageNum: 1, pageSize: 50 };

    columnDef: CustomColumn[] = [
        {
            header: this.l('Name'),
            field: 'name',
            width: 180,
            textAlign: 'Left',
        },
        {
            header: this.l('Username'),
            field: 'username',
            width: 200,
            textAlign: 'Left',
        },
        {
            header: this.l('EmailAddress'),
            field: 'emailAddress',
            width: 250,
            textAlign: 'Left',
        },
        {
            header: this.l('Request'),
            field: 'request',
            width: 110,

            textAlign: 'Left',
        },
        {
            header: this.l('Rejected'),
            field: 'rejected',
            width: 110,
            textAlign: 'Center',
        },
        {
            header: this.l('Reassigned'),
            field: 'reassigned',
            width: 110,
            textAlign: 'Center',
        },
        {
            header: this.l('Viewed'),
            field: 'viewed',
            width: 110,
            textAlign: 'Center',
        },
        {
            header: this.l('Shared'),
            field: 'shared',
            width: 110,
            textAlign: 'Center',
        },
        {
            header: this.l('Signed'),
            field: 'signed',
            width: 110,
            textAlign: 'Center',
        }, {
            header: this.l('Transferred'),
            field: 'transferred',
            width: 110,
            textAlign: 'Center',
        },
        {
            header: this.l('AdditionalRefDoc'),
            field: 'additionalRefDoc',
            width: 135,
            textAlign: 'Center',
        },
        {
            header: this.l('Reminded'),
            field: 'reminded',
            width: 110,
            textAlign: 'Center',
        },
        {
            header: this.l('Commented'),
            field: 'commented',
            width: 110,
            textAlign: 'Center',
        },
        {
            header: this.l('Revoked'),
            field: 'revoked',
            width: 110,
            textAlign: 'Center',
        },
        {
            header: this.l('Total'),
            field: 'total',
            width: 110,
            textAlign: 'Center',
        },
    ]

    constructor(
        injector: Injector,
        private _service: EsignActivityReportServiceProxy,
        private _fileDownloadService: FileDownloadService
    ) {
        super(injector)
    }

    ngOnInit() {
        this.searchDatas();
    }

    searchDatas() {
        this._service.getAllEsignActivityReport(
            this.filterName,
            this.filterEmail,
            this.pagination?.pageSize,
            this.pagination?.pageSize * (this.pagination?.pageNum - 1)
        ).subscribe(res => {
            this.pagination.totalCount = res.totalCount;
            this.rowData = res.items;
        });
    }

    clearTextSearch() {
        this.filterName = '';
        this.filterEmail = '';
        this.searchDatas();
    }

    changeSelection(e: any) {
        this.selectionRow = e.data
    }

    changePager(event: any) {
        this.pagination = event;
        this.searchDatas();
    }

    exportToExcel(): void {
        this._service.getAllEsignActivityReportExcel(
            this.filterName,
            this.filterEmail,
            this.pagination?.pageSize,
            this.pagination?.pageSize * (this.pagination?.pageNum - 1)
        )
            .subscribe((result) => {
                this._fileDownloadService.downloadTempFile(result);
            });
    }

}
