import { Component, Injector, OnInit, ViewChild } from '@angular/core';
import { AppComponentBase } from '@shared/common/app-component-base';
import { MstEsignDepartmentOutputDto, MstEsignDepartmentServiceProxy } from '@shared/service-proxies/service-proxies';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { SelectionService } from '@syncfusion/ej2-angular-grids';
import { CreateOrEditDepartmentComponent } from './create-or-edit-department-modal.component';
import { CustomColumn, PaginationParamCustom } from '@app/shared/common/models/base.model';
@Component({
    selector: 'esign-department',
    templateUrl: './department.component.html',
    styleUrls: ['./department.component.less'],
    providers: [SelectionService],
})


export class DepartmentComponent extends AppComponentBase implements OnInit {
    @ViewChild('createOrEditDivision', { static: true }) createOrEditDivision: | CreateOrEditDepartmentComponent  | undefined;
    selectionSettings: { type: 'Single'};
    filterSettings: { type: 'FilterBar', mode: 'Immediate', immediateModeDelay: 150 };
    rowDataDepartment: any[] = [];
    selectionRow: MstEsignDepartmentOutputDto = new MstEsignDepartmentOutputDto();
    pagination : PaginationParamCustom = { totalCount: 0, totalPage: 0, pageNum: 1, pageSize: 10 };
    columnDeftDepartment : CustomColumn[] = [
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
        private _service: MstEsignDepartmentServiceProxy,
        private _fileDownloadService: FileDownloadService
    ) {
        super(injector);
    }

    ngOnInit(): void {
        this.searchDatas();
    }

    searchDatas() {
        this._service.getAllDepartment(
            this.code,
            this.localName,
            this.pagination?.pageSize,
            this.pagination?.pageSize * (this.pagination?.pageNum - 1)
            )
            .subscribe((result) => {
                this.pagination.totalCount = result.totalCount;
                this.rowDataDepartment = result.items;
            });
    }

    clearTextSearch() {
        this.code = '';
        this.localName = '';
        this.searchDatas();
    }

    deleteRow(system: MstEsignDepartmentOutputDto): void {
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
        this._service.getDepartmentExcel(
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
