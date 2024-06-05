import { Component, EventEmitter, Injector, Input, OnInit, Output, ViewChild } from '@angular/core';
import { PaginationParamCustom } from '@app/shared/common/models/base.model';
import { AppComponentBase } from '@shared/common/app-component-base';
import { EsignRequestWebServiceProxy } from '@shared/service-proxies/service-proxies';
import { ListViewComponent } from '@syncfusion/ej2-angular-lists';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { TabView } from 'primeng/tabview';
import { finalize } from 'rxjs';

@Component({
    selector: 'search-request',
    templateUrl: './search-request.component.html',
    styleUrls: ['./search-request.component.less']
})
export class SearchRequestComponent extends AppComponentBase {
    @ViewChild('modal', { static: false }) modal!: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();
    @ViewChild('listViewSigner') listViewSigner: TabView;
    @ViewChild('listview') listview: ListViewComponent;
    @Input() requestId;
    @Input() references;
    searchFilter;
    dialogWidth: string = '35%';
    dialogHeight: string = '81%';
    visible: Boolean = false;
    visibleInfo: Boolean = false;
    animationSettings: Object = { effect: 'FadeZoom' };
    selectedRequest: any[] = [];
    searchResult: any[] = [];
    searchResultNoFilter: any[] = [];
    activeType: number = 0;
    loading = false;
    pagination: PaginationParamCustom = {pageNum: 1, pageSize: 40, totalCount: 0};
    constructor(
        injector: Injector,
        private _requestService: EsignRequestWebServiceProxy
    ) {
        super(injector);

    }

    ngOnInit() {
    }

    submit() {
        this.loading = true;
        this.modalSave.emit(this.selectedRequest);
        this.modal.hide();
        this.loading = false;
    }

    show() {
        this.searchAll();
        this.modal.show();
        this.selectedRequest = [];
        this.activeType = 0;
    }

    hide() {
        this.modal.hide();
        this.selectedRequest = [];
        this.searchResult = [];
        this.searchFilter = '';
        this.activeType = 0;
    }

    changeKey(event) {
        this.searchFilter = event.target.value;
        if (event.keyCode == 13) {
            this.searchAll();
        }
    }

    searchAll() {
        this.searchResult = [];
        this.searchResultNoFilter = [];
        this._requestService.getListRequestsBySearchValue(
            1,
            this.activeType,
            this.searchFilter,
            0,
            0,
            0,
            (this.pagination.pageNum - 1) * this.pagination.pageSize,
            this.pagination.pageSize
        ).pipe(finalize(() => {
            this.loading = false;
        }))
        .subscribe(result => {
            result.items.forEach(element => {
                this.searchResultNoFilter = this.searchResultNoFilter.concat(element.listRequest);
                this.searchResult = this.searchResult.concat(element.listRequest).filter(x => !this.references?.some(e => e.requestRefId == x.requestId));
            });
            this.pagination.totalCount = result.totalCount;
        });
    }

    getNewData(isAll = false){
        this.loading = true;
        this._requestService.getListRequestsBySearchValue(
            1,
            this.activeType,
            this.searchFilter,
            0,
            0,
            0,
            this.searchResultNoFilter.length,
            isAll ? (this.pagination.totalCount - this.searchResult.length) :this.pagination.pageSize
        ).pipe(finalize(() => {
            this.loading = false;
        }))
        .subscribe(result => {
            this.pagination.totalCount = result.totalCount;
            result.items.forEach(element => {
                this.searchResultNoFilter = this.searchResultNoFilter.concat(element.listRequest);
                this.searchResult = this.searchResult.concat(element.listRequest).filter(x => !this.references?.some(e => e.requestRefId == x.requestId));
            });
        });
    }

    onChangeType(event) {
        this.activeType = event.selectedIndex;
        this.searchAll();
    }

    onScroll() {
        if(this.modal.isShown == false) return;
        let pos = this.round((document.getElementById('listViewRequest').scrollTop || document.body.scrollTop) + document.getElementById('listViewRequest').offsetHeight);
        let match = this.round(document.getElementById('listViewRequest').scrollHeight * 0.68);
        let max = this.round(document.getElementById('listViewRequest').scrollHeight);
        if (pos == match && this.searchResultNoFilter.length < this.pagination.totalCount && this.activeType == 0 && this.loading == false) {
            this.getNewData();
        }
        else if (pos == max && this.searchResultNoFilter.length < this.pagination.totalCount && this.activeType == 0 && this.loading == false) {
            this.getNewData(true);
        }
    }

    round(num) {
        let num1 = Math.round(num)?.toString().slice(0, -3);
        return Number(num1);
    }
}
