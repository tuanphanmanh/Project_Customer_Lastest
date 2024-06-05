import { Component, Injector, OnInit, ViewChild } from '@angular/core';
import { CustomColumn, PaginationParamCustom } from '@app/shared/common/models/base.model';
import { AppComponentBase } from '@shared/common/app-component-base';
import { CreateOrEditMstEsignStatusInputDto, MstEsignStatusOutputDto, MstEsignStatusServiceProxy } from '@shared/service-proxies/service-proxies';
import { CreateOrEditStatusComponent } from './create-or-edit-status-modal.component';

@Component({
    selector: 'esign-status',
    templateUrl: './status.component.html',
    styleUrls: ['./status.component.less']
})
export class StatusComponent extends AppComponentBase implements OnInit {
    @ViewChild('createOrEditStatus', { static: true }) createOrEditStatus: | CreateOrEditStatusComponent | undefined;
    selectionSettings: { type: 'Single' };
    filterSettings: { type: 'FilterBar', mode: 'Immediate', immediateModeDelay: 150 };
    rowDataStatus: any[] = [];
    selectionRow: CreateOrEditMstEsignStatusInputDto = new CreateOrEditMstEsignStatusInputDto();
    pagination: PaginationParamCustom = { totalCount: 0, totalPage: 0, pageNum: 1, pageSize: 10 };
    columnDeftStatus: CustomColumn[] = [
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
        {
            header: this.l('Type'),
            field: 'type',
            width: 100,
            textAlign: 'Center',
        },
    ]
    code: string;
    localName: string;
    typeId: number = -1;
    typeFilter: any[] = [
        { value: -1, label: this.l('All') },
        { value: 0, label: this.l('Mobile') },
        { value: 1, label: this.l('Website') },
    ];
    constructor(
        injector: Injector,
        private _service: MstEsignStatusServiceProxy,
    ) {
        super(injector);
    }

    ngOnInit(): void {
        this.searchDatas();
    }

    searchDatas() {
        this._service.getAllStatus(
            this.code,
            this.localName,
            this.typeId,
            this.pagination?.pageSize,
            this.pagination?.pageSize * (this.pagination?.pageNum - 1)
        )
            .subscribe((result) => {
                this.pagination.totalCount = result.totalCount;
                this.rowDataStatus = result.items;
            });
    }

    clearTextSearch() {
        this.code = '';
        this.localName = '';
        this.typeId = -1;
        this.searchDatas();
    }

    deleteRow(system: CreateOrEditMstEsignStatusInputDto): void {
        this.message.confirm(this.l('Are You Sure To Delete', system.code), 'Delete Row', (isConfirmed) => {
            if (isConfirmed) {
                this._service.delete(system.id).subscribe(() => {
                    this.searchDatas();
                    this.notify.success(this.l('SuccessfullyDeleted'));
                });
            }
        });
    }

    changeSelection(e: any) {
        this.selectionRow = e.data
    }

    changePager(event: any) {
        this.pagination = event;
        this.searchDatas();
    }
}
