import { MstEsignUserImageOutputDto, MstEsignUserImageServiceProxy } from './../../../../../shared/service-proxies/service-proxies';
import { Component, Injector, OnInit } from '@angular/core';
import { CustomColumn, PaginationParamCustom } from '@app/shared/common/models/base.model';
import { AppComponentBase } from '@shared/common/app-component-base';

@Component({
    selector: 'esign-user-image',
    templateUrl: './user-image.component.html',
    styleUrls: ['./user-image.component.less']
})
export class UserImageComponent extends AppComponentBase implements OnInit {
    selectionSettings: { type: 'Single' };
    filterSettings: { type: 'FilterBar', mode: 'Immediate', immediateModeDelay: 150 };
    rowData: any[] = [];
    selectionRow: MstEsignUserImageOutputDto = new MstEsignUserImageOutputDto();
    pagination: PaginationParamCustom = { totalCount: 0, totalPage: 0, pageNum: 1, pageSize: 10 };
    columnDef: CustomColumn[] = [
        {
            header: this.l('ImgSize'),
            field: 'imgSize',
            width: 50,
            textAlign: 'Left',
            isPrimaryKey: true,
        },
        {
            header: this.l('ImgUrl'),
            field: 'imgUrl',
            width: 300,
            textAlign: 'Left',
        },
        {
            header: this.l('Image'),
            field: 'imgUrl',
            width: 70,
            textAlign: 'Center',
            template: 'PreviewImage'
        },
        {
            header: this.l('IsUse'),
            field: 'isUse',
            width: 40,
            textAlign: 'Center',
        },
    ]
    constructor(
        injector: Injector,
        private _service: MstEsignUserImageServiceProxy,
    ) {
        super(injector);
    }

    ngOnInit(): void {
        this.searchDatas();
    }

    searchDatas() {
        this._service.getAllSignature(
            this.pagination?.pageSize,
            this.pagination?.pageSize * (this.pagination?.pageNum - 1)
        )
            .subscribe((result) => {
                this.pagination.totalCount = result.totalCount;
                this.rowData = result.items;
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
