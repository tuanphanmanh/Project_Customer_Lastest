import { Component, Injector, OnInit } from '@angular/core';
import { CustomColumn, PaginationParamCustom } from '@app/shared/common/models/base.model';
import { AppComponentBase } from '@shared/common/app-component-base';
import { MstEsignSignerTemplateOutputDto, MstEsignSignerTemplateServiceProxy } from '@shared/service-proxies/service-proxies';

@Component({
    selector: 'app-signer-template',
    templateUrl: './signer-template.component.html',
    styleUrls: ['./signer-template.component.less']
})
export class SignerTemplateComponent extends AppComponentBase implements OnInit {
    selectionSettings: { type: 'Single' };
    filterSettings: { type: 'FilterBar', mode: 'Immediate', immediateModeDelay: 150 };
    rowData: any[] = [];
    rowDataDetail: any[] = [];
    selectionRow: MstEsignSignerTemplateOutputDto = new MstEsignSignerTemplateOutputDto();
    pagination: PaginationParamCustom = { totalCount: 0, totalPage: 0, pageNum: 1, pageSize: 10 };
    paginationDetail: PaginationParamCustom = { totalCount: 0, totalPage: 0, pageNum: 1, pageSize: 10 };
    columnDef: CustomColumn[] = [
        {
            header: this.l('Code'),
            field: 'code',
            width: 50,
            textAlign: 'Left',
            isPrimaryKey: true,
        },
        {
            header: this.l('CC'),
            field: 'addCC',
            width: 120,
            textAlign: 'Left',
        },
        {
            header: this.l('LocalName'),
            field: 'localName',
            width: 80,
            textAlign: 'Left',
        },
        {
            header: this.l('InternationalName'),
            field: 'internationalName',
            width: 80,
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
    ];
    detailColumnDef: CustomColumn[] = [
        {
            header: this.l('Image'),
            field: 'userProfilePicture',
            width: 100,
            textAlign: 'Left',
            isPrimaryKey: true,
            template: 'PreviewImage'
        },        {
            header: this.l('Name'),
            field: 'fullName',
            width: 100,
            textAlign: 'Left',
        },
        {
            header: this.l('SigningOrder'),
            field: 'signingOrder',
            width: 100,
            textAlign: 'Center',
        },
        {
            header: this.l('ColorCode'),
            field: 'colorCode',
            width: 100,
            textAlign: 'Left',
        },
        {
            header: this.l('Color'),
            field: 'colorCode',
            width: 50,
            textAlign: 'Left',
            template: 'PreviewColor'
        },
    ];
    code: string;
    localName: string;
    constructor(
        injector: Injector,
        private _service: MstEsignSignerTemplateServiceProxy,
    ) {
        super(injector);
    }

    ngOnInit(): void {
        this.searchDatas();
    }

    searchDatas() {
        this._service.getAllSignatureTemplate(
            this.code,
            this.localName,
            this.pagination?.pageSize,
            this.pagination?.pageSize * (this.pagination?.pageNum - 1)
        )
            .subscribe((result) => {
                this.pagination.totalCount = result.totalCount;
                this.rowData = result.items;
            });
    }

    searchDatasDetail() {
        this._service.getAllSignatureTemplateLinkById(this.selectionRow.id)
        .subscribe((result) => {
            this.rowDataDetail = result;
        });
    }

    clearTextSearch() {
        this.code = '';
        this.localName = '';
        this.searchDatas();
    }

    changeSelection(e: any) {
        this.selectionRow = e.data
        this.searchDatasDetail();
    }

    changePager(event: any) {
        this.pagination = event;
        this.searchDatas();
    }
}
