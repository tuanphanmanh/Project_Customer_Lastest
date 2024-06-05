import { Component, EventEmitter, Injector, Input, Output, ViewChild } from '@angular/core';
import { AppComponentBase } from '@shared/common/app-component-base';
import { EsignRequestServiceProxy, EsignSignerListServiceProxy, MstEsignActiveDirectoryDto, MstEsignActiveDirectoryServiceProxy, ReAssignInputDto } from '@shared/service-proxies/service-proxies';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs';

@Component({
    selector: 'reassign-modal',
    templateUrl: './reassign-modal.component.html',
    styleUrls: ['./reassign-modal.component.less']
})
export class ReassignModalComponent extends AppComponentBase {
    @ViewChild('modal', { static: false }) modal!: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();
    @Input() listSigner;
    @Input() requesterId;
    dialogWidth: string = '40%';
    visible: Boolean = false;
    animationSettings: Object = { effect: 'FadeZoom' };
    messageText;
    searchResult: any[] = [];
    reassignTo;
    requestId;
    loading = false;
    constructor(
        injector: Injector,
        private activeDirectoryService: MstEsignActiveDirectoryServiceProxy,
        private _esignService: EsignSignerListServiceProxy
        ) {
        super(injector);
    }

    ngOnInit() {
    }
    submit(){
        if(!this.reassignTo) {
            this.notify.warn(this.l('PleaseSelectUser'));
            return;
        }
        // if(!this.messageText || this.messageText == '') {
        //     this.notify.warn(this.l('PleaseEnterMessage'));
        //     return;
        // }
        this.loading = true;
        let body = new ReAssignInputDto({
            requestId: this.requestId,
            note: this.messageText,
            reAssignUserId: this.reassignTo.id
        });
        this._esignService.reassignRequest(body)
        .pipe(finalize(() => {
            this.loading = false;
            this.modal.hide();
            this.modalSave.emit(null);
        }))
        .subscribe(res => {
            this.notify.success(this.l('ReassignSuccess'));
            this.modal.hide();
        });
    }

    show(requestId?: number){
        this.requestId = requestId;
        this.modal.show();
    }

    hide(){
        this.modal.hide();
    }

    filterUser(event) {
        this.activeDirectoryService.getAllSignersForTransfer(event.query, undefined, 0, 50).subscribe(res => {
            this.searchResult = res.items.filter(x => !this.listSigner?.some(e => e.userId == x.id)).filter(x => x.id !== this.requesterId);
        });
    }
}
