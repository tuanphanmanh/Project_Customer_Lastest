import { Component, Injector, OnInit, ViewChild } from '@angular/core';
import { AppComponentBase } from '@shared/common/app-component-base';
import { MstEsignEmailTemplateOutputDto, MstEsignEmailTemplateServiceProxy } from '@shared/service-proxies/service-proxies';
import { SelectionService } from '@syncfusion/ej2-angular-grids';
import { CustomColumn, PaginationParamCustom } from '@app/shared/common/models/base.model';
import { CreateOrEditEmailTemplateComponent } from './create-or-edit-email-template-modal.component';
@Component({
    selector: 'esign-email-template',
    templateUrl: './config-email-template.component.html',
    styleUrls: ['./config-email-template.component.less'],
    providers: [SelectionService],
})


export class ConfigEmailTemplateComponent extends AppComponentBase implements OnInit {
    @ViewChild('createOrEditDivision', { static: true }) createOrEditDivision: | CreateOrEditEmailTemplateComponent  | undefined;
    selectionSettings: { type: 'Single'};
    filterSettings: { type: 'FilterBar', mode: 'Immediate', immediateModeDelay: 150 };
    rowDataDepartment: any[] = [];
    selectionRow: MstEsignEmailTemplateOutputDto = new MstEsignEmailTemplateOutputDto();
    pagination : PaginationParamCustom = { totalCount: 0, totalPage: 0, pageNum: 1, pageSize: 10 };
    columnDeftDepartment : CustomColumn[] = [
        {
            header: this.l('Code'),
            field: 'templateCode',
            width: 70,
            textAlign: 'Left',
            isPrimaryKey: true,
        },
        {
            header: this.l('EmailTitle'),
            field: 'title',
            width: 100,
            textAlign: 'Left',
        },
        {
            header: this.l('EmailMessaage'),
            field: 'message',
            width: 100,
            textAlign: 'Left',
        },
        {
            header: this.l('BCC'),
            field: 'bcc',
            width: 100,
            textAlign: 'Left',
        },
    ]
    code: string;
    constructor(
        injector: Injector,
        private _service: MstEsignEmailTemplateServiceProxy,
    ) {
        super(injector);
    }

    ngOnInit(): void {
        this.searchDatas();
    }

    searchDatas() {
        this._service.getAllEmailTemplate(
            this.code,
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
        this.searchDatas();
    }

    deleteRow(template: MstEsignEmailTemplateOutputDto): void {
        this.message.confirm(this.l('Are You Sure To Delete', template.templateCode), 'Delete Row', (isConfirmed) => {
            if (isConfirmed) {
                this._service.delete(template.id).subscribe(() => {
                    this.searchDatas();
                    this.notify.success(this.l('SuccessfullyDeleted'));
                });
            }
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
