import { Component, EventEmitter, Injector, Input, OnInit, Output, ViewChild } from '@angular/core';
import { AppComponentBase } from '@shared/common/app-component-base';
import { EsignSignerListServiceProxy, ShareInputDto, EsignRequestServiceProxy, CreateShareRequest_WebDto } from '@shared/service-proxies/service-proxies';
import { EsignSignerTemplateLinkOutputWebDto, MstEsignActiveDirectoryServiceProxy, MstEsignColorServiceProxy } from '@shared/service-proxies/service-proxies';
import { ListViewComponent } from '@syncfusion/ej2-angular-lists';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { TabView } from 'primeng/tabview';
import { finalize } from 'rxjs';

@Component({
    selector: 'share-request',
    templateUrl: './share-request.component.html',
    styleUrls: ['./share-request.component.less']
})
export class ShareRequestComponent extends AppComponentBase {
    @ViewChild('modal', { static: false }) modal!: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();
    @ViewChild('listViewSigner') listViewSigner: TabView;
    @ViewChild('listview') listview: ListViewComponent;
    @Input() requestId;
    @Input() listSigner;
    @Input() requesterId;
    searchFilter;
    dialogWidth: string = '35%';
    dialogHeight: string = '81%';
    visible: Boolean = false;
    visibleInfo: Boolean = false;
    animationSettings: Object = { effect: 'FadeZoom' };
    selectedSigner: any[] = [];
    searchResult: any[] = [];
    searchResultTemplate: {
        id: number,
        localName: string,
        text: string,
        internationalName: string,
        child: EsignSignerTemplateLinkOutputWebDto[]
    }[] = [];
    activeType: number = 0;
    allColor;
    loading = false;
    selectSignerInfo;
    isSelectAll = false;
    allSigner = [];
    constructor(
        injector: Injector,
        private _colorService: MstEsignColorServiceProxy,
        private _signerInfo: MstEsignActiveDirectoryServiceProxy,
        private _requestService: EsignSignerListServiceProxy,
        private _EsignRequestServiceProxy: EsignRequestServiceProxy
    ) {
        super(injector);

    }

    ngOnInit() {
        this._colorService.getAllColor("", "").subscribe(res => {
            this.allColor = res.items;
        });
    }

    submit() {
        this.loading = true;
        let body = new CreateShareRequest_WebDto({
            listUserId: this.selectedSigner.map(x => x.id),
            requestId: this.requestId
        });
        this._EsignRequestServiceProxy.shareRequest_Web(body)
            .pipe(finalize(() => {
                this.modal.hide();
                this.loading = false;
                this.modalSave.emit(null);
            }))
            .subscribe(res => {
                this.notify.success(this.l('ShareRequestSuccess'));
            });
        this.modal.hide();
        this.loading = false;
    }

    show() {
        this.selectedSigner = [];
        this.activeType = 0;
        this._signerInfo.getAllSignerByGroup(
            this.searchFilter,
            this.activeType == 0 ? -1 : (this.activeType == 3 ? 4 : this.activeType),
            0,
            1000000
        )
            .pipe(finalize(() => this.modal.show()))
            .subscribe(res => {
                this.allSigner = res.items.filter(x => !this.listSigner?.some(e => e.userId == x.id)).filter(x => x.id !== abp.session.userId && x.id !== this.requesterId);
                this.searchResult = this.allSigner.slice(0, 50);
            });
    }

    hide() {
        this.modal.hide();
        this.selectedSigner = [];
        this.searchResult = [];
        this.searchResultTemplate = [];
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
        this._signerInfo.getAllSignerByGroup(
            this.searchFilter,
            this.activeType == 0 ? -1 : (this.activeType == 3 ? 4 : this.activeType),
            0,
            50
        )
            .pipe(finalize(() => this.loading = false))
            .subscribe(res => {
                this.searchResult = res.items.filter(x => !this.listSigner?.some(e => e.userId == x.id)).filter(x => x.id !== abp.session.userId && x.id !== this.requesterId);
            });
    }

    onCloseSigner(event) {
        this.listview.uncheckItem(this.selectedSigner.find(x => x.id == event.id));
    }

    onChangeType(event) {
        this.activeType = event.selectedIndex;
        this.searchAll();
    }

    showInfo(id) {
        this._signerInfo.getUserInformationById(id).subscribe(res => {
            this.selectSignerInfo = res;
        });
        this.visibleInfo = true;
    }

    onSelectAllChange(event) {
        if (event.checked) {
            this.selectedSigner = this.searchResult;
        } else {
            this.selectedSigner = [];
        }
    }

    onScroll() {
        let pos = this.round((document.getElementById('listView').scrollTop || document.body.scrollTop) + document.getElementById('listView').offsetHeight);
        let match = this.round(document.getElementById('listView').scrollHeight * 0.68);
        let max = this.round(document.getElementById('listView').scrollHeight);
        if (pos == match && this.searchResult.length < this.allSigner.length && this.activeType == 0) {
            this.searchResult = this.searchResult.concat(this.allSigner.filter(e => !this.searchResult?.some(x => x.id == e.id)).slice(0, 111));
        }
        else if (pos == max && this.searchResult.length < this.allSigner.length && this.activeType == 0) {
            this.searchResult = this.searchResult.concat(this.allSigner.filter(e => !this.searchResult?.some(x => x.id == e.id)));
        }
    }

    round(num) {
        let num1 = Math.round(num)?.toString().slice(0, -3);
        return Number(num1);
    }
}
