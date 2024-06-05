import { map } from 'rxjs/operators';
import { Component, EventEmitter, Injector, Input, OnInit, Output, ViewChild } from '@angular/core';
import { SearchRequestComponent } from '@app/main/project_customer_lastest/esign-modal/search-request/search-request.component';
import { AppComponentBase } from '@shared/common/app-component-base';
import { CreatEsignReferenceRequestDto, CreateNewReferenceRequestDto, EsignReferenceRequestServiceProxy } from '@shared/service-proxies/service-proxies';
import { finalize, forkJoin } from 'rxjs';
import { CreatOrEditEsignReferenceRequestDto } from '@shared/service-proxies/service-proxies';
import { Router } from '@angular/router';
import { LocalStorageService } from '@shared/utils/local-storage.service';

@Component({
    selector: 'tab-reference',
    templateUrl: './tab-reference.component.html',
    styleUrls: ['./tab-reference.component.less']
})
export class TabReferenceComponent extends AppComponentBase implements OnInit {
    @ViewChild('searchRequest') searchRequest: SearchRequestComponent;
    @Input() references: any;
    @Input() request: any;
    @Input() backPageUrl: any;
    @Input() requestId;
    @Input() TabMenuFilter;
    @Input() isRefDocument: boolean = false;
    @Output() onAddRefSuccess: EventEmitter<any> = new EventEmitter<any>();
    @Input() allowAction: boolean = true;
    saving: boolean = false;
    constructor(
        injector: Injector,
        private _refDocService: EsignReferenceRequestServiceProxy,
        private local: LocalStorageService,
        private _router: Router
    ) {
        super(injector);

    }

    ngOnInit() {
    }

    saveRefDocument(event){
        this.saving = true;
            if(event.length > 0){
            let body = new CreateNewReferenceRequestDto({
                isAddHistory: true,
                referenceRequests: event.map(e => e.requestId).map(e => new CreatEsignReferenceRequestDto({
                    note: null,
                    requestRefId: e,
                    })),
                requestId: this.requestId
            });
            this._refDocService.createNewReferenceRequest(body)
            .pipe(finalize(() => {
                this.saving = false;
            }))
            .subscribe(() => {
                this.saving = false;
                this.notify.success(this.l('AdditionalRefDoc'));
                this.onAddRefSuccess.emit(null);
            },
            ()=>{
                this.saving = false;
                this.notify.error(this.l('AddReferenceDocumentFailed'));
            })
        }else{
            this.notify.error(this.l('PleaseSelectReferenceDocument'));
        }
    }

    onFocusOut(doc){
        if(doc.note){
            this.saving = true;
            this._refDocService.createOrEditReferenceRequest(doc)
            .pipe(finalize(() => {
                this.saving = false;
                this.onAddRefSuccess.emit(null);
            }))
            .subscribe(() => {
                this.notify.success(this.l('UpdateNoteSuccessfully'));
            },
            ()=>{
                this.saving = false;
                this.notify.error(this.l('UpdateNoteFailed'));
            })
        }
    }

    onKeyDown(event,index){
        if(event.keyCode == 13){
            let element = document.getElementById('note-'+index);
            element.setAttribute('disabled', 'true');
            element.blur();
        }
    }

    editNote(index){
        let element = document.getElementById('note-'+index);
        if(element?.hasAttribute('disabled')){
            element.removeAttribute('disabled');
            element.focus();
        }
    }

    viewRefRequest(requestId){
        if(this.allowAction == true) {

            if(this.backPageUrl == 'view-detail'){
                this.local.setItem('backPageUrl', 'view-detail=' + this.request.id.toString());
            }
            else {
                this.local.setItem('backPageUrl', 'document-management=' + this.request.id.toString());
            }

            //this._router.navigate(['/app/main/view-reference/' + requestId]);
            window.open('/app/main/view-reference/' + requestId, '_blank', 'width=1200,height=800');
        }

    }
}
