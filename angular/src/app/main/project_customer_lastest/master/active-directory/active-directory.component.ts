import { Component, Injector, OnInit } from '@angular/core';
import { CustomColumn, PaginationParamCustom } from '@app/shared/common/models/base.model';
import { AppComponentBase } from '@shared/common/app-component-base';
import { MstEsignActiveDirectoryDto, MstEsignActiveDirectoryServiceProxy } from '@shared/service-proxies/service-proxies';
import { FileDownloadService } from '@shared/utils/file-download.service';

@Component({
    selector: 'esign-active-directory',
    templateUrl: './active-directory.component.html',
    styleUrls: ['./active-directory.component.less']
})
export class ActiveDirectoryComponent extends AppComponentBase implements OnInit {

    textFilter: string;
    selectionSettings: { type: 'Single' };
    filterSettings: { type: 'FilterBar', mode: 'Immediate', immediateModeDelay: 150 };
    rowData: any[] = [];
    selectionRow: MstEsignActiveDirectoryDto = new MstEsignActiveDirectoryDto();
    pagination: PaginationParamCustom = { totalCount: 0, totalPage: 0, pageNum: 1, pageSize: 50 };

    columnDef: CustomColumn[] = [
        {
            header: this.l('Email'),
            field: 'email',
            width: 100,
            textAlign: 'Left',
            isPrimaryKey: true,
        },
        {
            header: this.l('Title'),
            field: 'title',
            width: 100,
            textAlign: 'Left',
        },
        {
            header: this.l('Department'),
            field: 'department',
            width: 100,
            textAlign: 'Left',
            isPrimaryKey: true,
        },
        {
            header: this.l('FullName'),
            field: 'fullName',
            width: 100,
            textAlign: 'Left',
            isPrimaryKey: true,
        },
        {
            header: this.l('Image'),
            field: 'imageUrl',
            width: 70,
            textAlign: 'Center',
            template: 'PreviewImage'
        },
    ]

    constructor(
        injector: Injector,
        private _service: MstEsignActiveDirectoryServiceProxy,
        private _fileDownloadService: FileDownloadService
    ) {
        super(injector)
    }

    ngOnInit() {
        this.searchDatas();
    }

    searchDatas() {
        this._service.getAllActiveDirectory(
            this.textFilter,
            this.pagination?.pageSize,
            this.pagination?.pageSize * (this.pagination?.pageNum - 1)
        ).subscribe(res => {
            this.pagination.totalCount = res.totalCount;
            this.rowData = res.items;
        });
    }

    clearTextSearch() {
        this.textFilter = '';
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
        this._service.getActiveDirectory(
            this.textFilter,
            this.pagination?.pageSize,
            this.pagination?.pageSize * (this.pagination?.pageNum - 1)
        )
            .subscribe((result) => {
                this._fileDownloadService.downloadTempFile(result);
            });
    }

}
