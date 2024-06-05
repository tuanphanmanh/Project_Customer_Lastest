import { Component, Injector, OnInit, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { AppComponentBase } from '@shared/common/app-component-base';
import { CreateOrEditEsignActivityHistoryInputDto, EsignActivityHistoryServiceProxy, EsignCommentsServiceProxy, EsignDocumentListWebServiceProxy, EsignReferenceRequestServiceProxy, EsignRequestWebServiceProxy, EsignSignerListServiceProxy } from '@shared/service-proxies/service-proxies';
import { finalize, forkJoin } from 'rxjs';
import { RequestDetailComponent } from '../document-managerment/request-detail/request-detail.component';
import { LocalStorageService } from '@shared/utils/local-storage.service';

@Component({
    selector: 'app-view-reference',
    templateUrl: './view-reference.component.html',
    styleUrls: ['./view-reference.component.less']
})
export class ViewReferenceComponent extends AppComponentBase implements OnInit {
    @ViewChild('requestReference', { static: true }) requestReference: RequestDetailComponent;
    isLoading = false;
    referenceRequest: {
        id: number,
        summary: any,
        signers: any,
        documents: any,
        history: any,
        comments: any,
        refDocument: any,
        totalSignerCount: number,
        isFollowUp: boolean,
    } = {
            id: null,
            summary: {},
            signers: [],
            documents: [],
            history: [],
            comments: [],
            refDocument: [],
            totalSignerCount: 0,
            isFollowUp: false,
        };
    constructor(
        injector: Injector,
        private router: Router,
        private _requestServiceWeb: EsignRequestWebServiceProxy,
        private _documentService: EsignDocumentListWebServiceProxy,
        private _commentService: EsignCommentsServiceProxy,
        private _historyService: EsignActivityHistoryServiceProxy,
        private _signerService: EsignSignerListServiceProxy,
        private _refDocument: EsignReferenceRequestServiceProxy,
        private local : LocalStorageService

    ) {
        super(injector);
    }
    ngOnInit() {
        // get id from url have format: /esign/view-reference/1
        const id = this.router.url.split('/').pop();
        if(id && !isNaN(Number(id))){
            this.referenceRequest.id = Number(id);
            this.searchReaquestDetail();
        }
        else {
            this.router.navigate(['/app/main/document-managerment']);
        }
    }

    searchReaquestDetail() {
        this.isLoading = true;
        let summary = this._requestServiceWeb.getRequestSummaryById(this.referenceRequest.id);
        let signers = this._signerService.getListSignerByRequestId(this.referenceRequest.id);
        let documents = this._documentService.getEsignDocumentByRequestId(this.referenceRequest.id);
        let history = this._historyService.getListActivityHistory(this.referenceRequest.id);
        let comments = this._commentService.getAllCommentsForRequestId(this.referenceRequest.id);
        let refDocument = this._refDocument.getReferenceRequestByRequestId(this.referenceRequest.id);
        if (this.referenceRequest.id > 0) {
            forkJoin([summary, signers, documents, history, comments, refDocument])
                .pipe(finalize(() => {
                    if(this.referenceRequest.summary?.statusCode !== 'Draft'){
                        this.createHistoryViewed();
                    }
                    this.requestReference.changeRequest();
                    this.requestReference.tabSigner.linear.refresh();
                    this.isLoading = false;
                }))
                .subscribe(([summary, signers, documents, history, comments, refDocument]) => {
                    this.referenceRequest.summary = summary;
                    this.referenceRequest.signers = this.groupSigners(signers.items);
                    this.referenceRequest.totalSignerCount = signers.items.length;
                    this.referenceRequest.documents = documents.items.sort((a, b) => a.documentOrder - b.documentOrder);
                    this.referenceRequest.history = history.items;
                    this.referenceRequest.comments = comments.items;
                    this.referenceRequest.refDocument = refDocument.items;
                    this.isLoading = false;
                });
        }
        else {
            this.isLoading = false;
        }
    }

    groupSigners(signers) {
        const groupSigners: { rank: number, signers: any[] }[] = [];
        signers.map((signer) => {
            const rank = signer.signingOrder;
            if (!groupSigners[rank - 1]) {
                groupSigners[rank - 1] = {
                    rank,
                    signers: []
                };
            }
            groupSigners[rank - 1].signers.push(signer);
        });
        return groupSigners;
    }

    createHistoryViewed() {
        try{
            if(!this.referenceRequest.id) return;
            let body = new CreateOrEditEsignActivityHistoryInputDto({
                id: undefined,
                requestId: (this.referenceRequest.id == null) ? 0:this.referenceRequest.id,
                activityCode: 'Viewed',
                ipAddress: '68.68.68.68',
            });
            this._historyService.createSignerActivity(body).subscribe();
        }
        catch(ex) {
            console.error(ex);
        }
    }

    backPage() {
        this.local.removeItem("selectedDocumentId");
        this.local.getItem('backPageUrl',(err: any, data: string)=>{

            if(data) {
                if(data.indexOf('view-detail') != -1){
                    let _id = data.split('=')[1];
                    this.router.navigate(['/app/main/view-detail/' + _id]);
                }
                else if (data.indexOf('document-management') != -1) {
                    let _id = data.split('=')[1];
                    this.local.setItem("selectRequestFromDashboard",_id )
                    this.router.navigate(['app/main/document-management']);
                    // this.router.navigate(['app/main/document-management'], { queryParams: { requestid: _id } });
                }
                else {
                    this.local.setItem("selectRequestFromDashboard",this.referenceRequest.id )
                    this.router.navigate(['app/main/document-management']);
                    // this.router.navigate(['app/main/document-management'], { queryParams: { requestid: this.referenceRequest.id } });
                }
            }
            else{
                this.local.setItem("selectRequestFromDashboard",this.referenceRequest.id )
                this.router.navigate(['app/main/document-management']);

                // this.router.navigate(['app/main/document-management'], { queryParams: { requestid: this.referenceRequest.id } });
            }
        })
    }
}
