import { Component, Injector, OnInit, ViewChild } from '@angular/core';
import { CustomColumn, PaginationParamCustom } from '@app/shared/common/models/base.model';
import { AppComponentBase } from '@shared/common/app-component-base';
import { MstEsignCategoryDto, MstEsignConfigServiceProxy } from '@shared/service-proxies/service-proxies';
import { CreateOrEditEsignConfigComponent } from './create-or-edit-esign-config-modal.component';
import { CreateOrEditMstEsignConfigDto } from '@shared/service-proxies/service-proxies';
import { MstEsignConfigOutputDto } from '@shared/service-proxies/service-proxies';

@Component({
    selector: 'esign-config',
    templateUrl: './esign-config.component.html',
    styleUrls: ['./esign-config.component.less']
})
export class EsignConfigComponent extends AppComponentBase implements OnInit {
    @ViewChild('createOrEditConfig', { static: true }) createOrEditConfig: | CreateOrEditEsignConfigComponent | undefined;
    code: string;
    description: string;
    selectionSettings: { type: 'Single' };
    filterSettings: { type: 'FilterBar', mode: 'Immediate', immediateModeDelay: 150 };
    rowData: any[] = [];
    selectionRow: CreateOrEditMstEsignConfigDto = new CreateOrEditMstEsignConfigDto();
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
                header: this.l('Value'),
                field: 'value',
                width: 50,
                textAlign: 'Left',
            },
            {
                header: this.l('StringValue'),
                field: 'stringValue',
                width: 60,
                textAlign: 'Left',
            },
            {
                header: this.l('Description'),
                field: 'description',
                width: 100,
                textAlign: 'Left',
            },
        ]

    constructor(
        injector: Injector,
        private _service: MstEsignConfigServiceProxy,
    ) {
        super(injector)
    }

    ngOnInit() {
        this.searchDatas();
    }

    searchDatas() {
        this._service.getAllConfig(
            this.code,
            this.description,
            this.pagination?.pageSize,
            this.pagination?.pageSize * (this.pagination?.pageNum - 1)
        ).subscribe(res => {
            this.pagination.totalCount = res.totalCount;
            this.rowData = res.items;
        });
    }

    clearTextSearch() {
        this.code = '';
        this.description = '';
        this.searchDatas();
    }

    changeSelection(e: any) {
        this.selectionRow = new CreateOrEditMstEsignConfigDto({
            id: e.data.id,
            code: e.data.code,
            description: e.data.description,
            stringValue: e.data.stringValue,
            value: e.data.value,
            tenantId: abp.session.tenantId,
        });
    }

    changePager(event: any) {
        this.pagination = event;
        this.searchDatas();
    }

    deleteRow(system: MstEsignConfigOutputDto) {
        this.message.confirm(this.l('Are You Sure To Delete', system.code), 'Delete Row', (isConfirmed) => {
            if (isConfirmed) {
                this._service.delete(system.id).subscribe(() => {
                    this.searchDatas();
                    this.notify.success(this.l('SuccessfullyDeleted'));
                });
            }
        });
    }
}
