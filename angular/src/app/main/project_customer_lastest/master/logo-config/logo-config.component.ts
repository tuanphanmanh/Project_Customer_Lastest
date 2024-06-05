import { Component, Injector, OnInit, ViewChild } from '@angular/core';
import { CustomColumn, PaginationParamCustom } from '@app/shared/common/models/base.model';
import { AppComponentBase } from '@shared/common/app-component-base';
import { MstEsignLogoServiceProxy, MstEsignLogoDto } from '@shared/service-proxies/service-proxies';
import { CreateOrEditLogoComponent } from './create-or-edit-logo-modal.component';
import { API_BASE_URL } from '@shared/service-proxies/service-proxies';
import { AppConsts } from '@shared/AppConsts';

@Component({
    selector: 'esign-logo-config',
    templateUrl: './logo-config.component.html',
    styleUrls: ['./logo-config.component.less']
})
export class LogoConfigComponent extends AppComponentBase implements OnInit {
    @ViewChild('createOrEditLogo', { static: true }) createOrEditLogo: | CreateOrEditLogoComponent | undefined;

    selectionSettings: { type: 'Single' };
    filterSettings: { type: 'FilterBar', mode: 'Immediate', immediateModeDelay: 150 };
    rowData: any[] = [];
    selectionRow: MstEsignLogoDto = new MstEsignLogoDto();
    pagination: PaginationParamCustom = { totalCount: 0, totalPage: 0, pageNum: 1, pageSize: 10 };
    columnDef: CustomColumn[] = [
        {
            header: this.l('TenancyName'),
            field: 'tenanceName',
            width: 40,
            textAlign: 'Left',
        },
        {
            header: this.l('Image'),
            field: 'logoMinUrl',
            width: 90,
            textAlign: 'Center',
            template: 'PreviewImage'
        },
        {
            header: this.l('Image'),
            field: 'logoMaxUrl',
            width: 90,
            textAlign: 'Center',
            template: 'PreviewImage'
        },
    ]


    constructor(
        injector: Injector,
        private _service: MstEsignLogoServiceProxy,
    ) {
        super(injector)
    }

    ngOnInit() {
        this.searchDatas();
    }

    searchDatas() {
        this._service.getAllLogos(
            this.pagination?.pageSize,
            this.pagination?.pageSize * (this.pagination?.pageNum - 1)
        ).subscribe(res => {
            this.pagination.totalCount = res.totalCount;
            this.rowData = res.items;
            this.rowData.forEach((item: any) => {
                item.logoMinUrl = AppConsts.remoteServiceBaseUrl + '/' + item.logoMinUrl;
                item.logoMaxUrl = AppConsts.remoteServiceBaseUrl + '/' + item.logoMaxUrl;
            });
        });
    }

    changeSelection(e: any) {
        this.selectionRow = e.data
    }

    changePager(event: any) {
        this.pagination = event;
        this.searchDatas();
    }

    deleteRow(system: MstEsignLogoDto) {
        this.message.confirm(this.l('Are You Sure To Delete', system.tenanceName), 'Delete Row', (isConfirmed) => {
            if (isConfirmed) {
                this._service.delete(system.id).subscribe(() => {
                    this.searchDatas();
                    this.notify.success(this.l('SuccessfullyDeleted'));
                });
            }
        });
    }
}
