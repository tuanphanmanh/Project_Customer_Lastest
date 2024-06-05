import { Component, Injector, OnInit, ViewChild } from '@angular/core';
import { CustomColumn } from '@app/shared/common/models/base.model';
import { AppComponentBase } from '@shared/common/app-component-base';
import { MstEsignAffiliateServiceProxy, MstEsignAffiliateDto, CreateOrEditMstEsignAffiliateDto } from '@shared/service-proxies/service-proxies';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { CreateOrEditAffiliateComponent } from './create-or-edit-affiliate-modal.component';
import { finalize } from 'rxjs';

@Component({
    selector: 'esign-affiliate',
    templateUrl: './affiliate.component.html',
    styleUrls: ['./affiliate.component.less']
})
export class AffiliateComponent extends AppComponentBase implements OnInit {
    @ViewChild('createOrEditAffiliate', { static: true }) createOrEditAffiliate: | CreateOrEditAffiliateComponent | undefined;

    code: string;
    name: string;
    selectionSettings: { type: 'Single' };
    filterSettings: { type: 'FilterBar', mode: 'Immediate', immediateModeDelay: 150 };
    rowData: any[] = [];
    selectionRow: MstEsignAffiliateDto = new MstEsignAffiliateDto();
    loading: boolean = false;
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
                header: this.l('Name'),
                field: 'name',
                width: 50,
                textAlign: 'Left',
            },
            {
                header: this.l('Description'),
                field: 'description',
                width: 150,
                textAlign: 'Left',
            },
            {
                header: this.l('ApiUsername'),
                field: 'apiUsername',
                width: 60,
                textAlign: 'Left',
                disabled: true
            },
            {
                header: this.l('ApiUrl'),
                field: 'apiUrl',
                width: 60,
                textAlign: 'Left',
                disabled: true
            },
        ]

    constructor(
        injector: Injector,
        private _service: MstEsignAffiliateServiceProxy,
        private _fileDownloadService: FileDownloadService
    ) {
        super(injector)
    }

    ngOnInit() {
        this.searchDatas();
    }

    searchDatas() {
        this._service.getAllSystems().subscribe(res => {
            this.rowData = res;
        });
    }

    clearTextSearch() {
        this.code = '';
        this.name = '';
        this.searchDatas();
    }

    changeSelection(e: any) {
        this.selectionRow = new CreateOrEditMstEsignAffiliateDto({
            id: e.data.id,
            code: e.data.code,
            name: e.data.name,
            description: e.data.description,
            apiUrl: e.data.apiUrl,
            apiUsername: e.data.apiUsername,
            apiPassword: e.data.apiPassword,
        });
    }

    deleteRow(system: MstEsignAffiliateDto) {
        this.message.confirm(this.l('AreYouSure'), 'Delete Row', (isConfirmed) => {
            if (isConfirmed) {
                this.loading = true;
                this._service.delete(system.id)
                .pipe(finalize(() => this.loading = false))
                .subscribe(() => {
                    this.searchDatas();
                    this.notify.success(this.l('SuccessfullyDeleted'));
                });
            }
        });
    }

    syncUserData(system: MstEsignAffiliateDto) {
        this.message.confirm(this.l('AreYouSure'), 'Sync User Data', (isConfirmed) => {
            if (isConfirmed) {
                this.loading = true;
                this._service.receiveMultiAffiliateUsersInfo(system.code)
                .pipe(finalize(() => this.loading = false))
                .subscribe(() => {
                    this.searchDatas();
                    this.notify.success(this.l('Success'));
                });
            }
        });
    }
}
