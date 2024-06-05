
import { Component, EventEmitter, Injector, Input, OnInit, Output, ViewChild } from '@angular/core';
import { RemindModalComponent } from '../../esign-modal/remind-modal/remind-modal.component';
import { RevokeModalComponent } from '../../esign-modal/revoke-modal/revoke-modal.component';
import { RejectModalComponent } from '../../esign-modal/reject-modal/reject-modal.component';
import { ReassignModalComponent } from '../../esign-modal/reassign-modal/reassign-modal.component';
import { TabSignerComponent } from './tab-signer/tab-signer.component';
import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { Router } from '@angular/router';
import { LocalStorageService } from '@shared/utils/local-storage.service';
import { EsignRequestWebServiceProxy, EsignSignerListServiceProxy, MstEsignUserImageServiceProxy, RejectInputDto,
            RevokeInputDto, SignDocumentInputDto,CloneToDraftDto, CloneToDraftRequest, 
            EsignDocumentListWebServiceProxy,
            EsignActivityHistoryServiceProxy,
            EsignCommentsServiceProxy,
            EsignReferenceRequestServiceProxy} from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { SignSuccessComponent } from '../../esign-modal/sign-success/sign-success.component';
import { RemindInputDto } from '@shared/service-proxies/service-proxies';
import { finalize, forkJoin } from 'rxjs';

@Component({
    selector: 'request-detail',
    templateUrl: './request-detail.component.html',
    styleUrls: ['./request-detail.component.less']
})

export class RequestDetailComponent extends AppComponentBase implements OnInit {
    @ViewChild('remindModal') remindModal: RemindModalComponent;
    @ViewChild('revokeModal') revokeModal: RevokeModalComponent;
    @ViewChild('rejectModal') rejectModal: RejectModalComponent;
    @ViewChild('reassignModal') reassignModal: ReassignModalComponent;
    @ViewChild('tabSigner') tabSigner: TabSignerComponent;
    @ViewChild('successModal') successModal!: SignSuccessComponent;

    @Output() onComments: EventEmitter<any> = new EventEmitter<any>();
    @Output() onRevoke: EventEmitter<any> = new EventEmitter<any>();
    @Output() onReject: EventEmitter<any> = new EventEmitter<any>();
    // @Output() gotoReview: EventEmitter<any> = new EventEmitter<any>();
    @Output() onReassign: EventEmitter<any> = new EventEmitter<any>();
    @Output() onFollowUp: EventEmitter<any> = new EventEmitter<any>();
    @Output() onSignSuccess: EventEmitter<any> = new EventEmitter<any>();
    @Output() onShareSuccess: EventEmitter<any> = new EventEmitter<any>();
    @Output() onDeleteRequest: EventEmitter<any> = new EventEmitter<any>();
    @Output() onChangeRefDocument: EventEmitter<any> = new EventEmitter<any>();

    @Input() request: any;
    @Input() backPageUrl: string;
    @Input() paramSigner: any;
    @Input() TabMenuFilter: any;
    @Input() allowAction: boolean = true;

    userIdLogin:number;
    isLoading = false;
    isRequester = false;
    constructor(
        injector: Injector,
        private dateFormat: DateTimeService,
        private router: Router,
        private local: LocalStorageService,
        private esignSigner: EsignSignerListServiceProxy,
        private _mstEsignUserImage: MstEsignUserImageServiceProxy, 
        private _documentService: EsignDocumentListWebServiceProxy,
        private _historyService: EsignActivityHistoryServiceProxy,
        private _esignRequest: EsignRequestWebServiceProxy,
        private _commentService: EsignCommentsServiceProxy,
        private _refDocument: EsignReferenceRequestServiceProxy,
    ) {
        super(injector);
    }

    ngOnInit() {
        this.userIdLogin = abp.session.userId;
    }

    downloadFile(path: string) {
        const link = document.createElement('a');
        link.setAttribute('target', '_blank');
        link.setAttribute('download', path.replace(/^.*[\\\/]/, ''));
        document.body.appendChild(link);
        setTimeout(() => {
            link.click();
            link.remove();
        }, 150);
    }

    changeRequest() {
        this.isRequester = this.request.summary?.requesterUserId === abp.session.userId;
    }

    addComment() {
        this.onComments.emit(null)
    }

    formatDate(input, isShowDate = false) {
        try{
            if (input) {
                if(isShowDate){
                //return type Oct 21, 2020
                    return this.dateFormat.formatDate(input as Date, 'dd/MMM/yyyy');
                }
                else{
                    return this.dateFormat.formatDate(input as Date, 'MMM/yyyy');
                }
            }
            else return '';
        }catch(e) {
            return ''
        }
    }

    totalSignerSigned() {
         return this.request.signers.filter((signer) => signer.signers.some((item) => item.statusCode === "Completed"))?.length ?? 0;
    }

    loading: boolean = false;
    listTemplateSignature;
    SignNow() {
        // [routerLink]="['/app/main/sign-now']";
        if(this.loading == true) return;
        // lấy chữ kí default
        this.loading = true;
        this._mstEsignUserImage.getListSignatureByUserIdForWeb().subscribe(res =>{
            if(res) {
                this.listTemplateSignature = res.items ?? [];
                if(this.listTemplateSignature.length > 0) {
                    let _default = this.listTemplateSignature.find(e => e.isUse == true);

                    let bodySign = new SignDocumentInputDto();
                        bodySign.typeSignId = 1;   //1: Template(default), 2: Draw, 3: Upload
                        bodySign.templateSignatureId = _default.id;
                        bodySign.requestId = this.request.id;

                    this._esignRequest.signerSign(bodySign)
                    .subscribe(e => {
                        // if(e) this.successModal.show(e);
                        // else this.successModal.show(e);
                        this.notify.success(this.l('SignnowSuccess'));
                        this.loadSigner();
                        this.loading = false;
                    },
                    (err) => {
                        this.loading = false;
                        //console.log(err);
                        // this.notify.error(this.l("AnErrorOccurred"));
                    });
                }
                else {
                    this.local.setItem('selectedRequest', this.request);
                    // this.local.setItem('selectedRequestAction', 'SignNow');
                    this.local.setItem('selectedDocumentId', 0);
                    this.router.navigate(['/app/main/sign-now']);
                }
            }
        });
    }
    StartSigning() {
        // [routerLink]="['/app/main/sign-now']";
        this.local.setItem('selectedRequest', this.request);
        // this.local.setItem('selectedRequestAction', 'StartSigning');
        this.local.setItem('selectedDocumentId', 0);
        this.router.navigate(['/app/main/sign-now']);
    }

    loadSigner(){
        let signers = this.esignSigner.getListSignerByRequestId(this.request.id);
        forkJoin([ signers])
        .pipe(finalize(() => { 
            this.changeRequest();
            this.tabSigner.linear.refresh();
            this.isLoading = false;
        }))
        .subscribe(([signers]) => { 
            this.request.signers = this.groupSigners(signers.items); 
            this.request.totalSignerCount = signers.items.length; 
            this.totalSignerSigned();
            this.isLoading = false;
        }); 
        
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
 

    remindRequest(event) {
        this.isLoading = true;
        let body = new RemindInputDto({
            note: '',
            requestId: this.request.id
        });``
        this.esignSigner.remindRequest(body)
        .pipe(finalize(() => {
            this.isLoading = false;
        }))
        .subscribe(() => {
            this.notify.success(this.l('RemindActionSuccess'));
        })
    }

    revokeRequest(event){
        let body = new RevokeInputDto({
            note: event,
            requestId: this.request.id,
            userId : this.userIdLogin
        });

        this.esignSigner.revokeRequest(body).subscribe(() => {
            this.notify.success(this.l('RevokeActionSuccess'));
            this.onRevoke.emit(null);
        });
    }

    rejectRequest(event){
        let body = new RejectInputDto({
            note: event,
            requestId: this.request.id
        });
        this.esignSigner.rejectRequest(body).subscribe(() => {
            this.notify.success(this.l('RejectActionSuccess'));
            this.onReject.emit(null);
        })
    }

    // onSelectDocument(event){
    //     this.gotoReview.emit(event);
    // }

    onAddRefSuccess(){
        this.onChangeRefDocument.emit(null);
    }

    onReassignRequest(){
        this.onReassign.emit(null);
    }

    followUp(){
        this.request.isFollowUp = !this.request.isFollowUp;
        this.onFollowUp.emit(this.request.isFollowUp);
    }

    CloneRequest(_type) {

        this.isLoading = true;


        let body = new CloneToDraftRequest({
            requestId: this.request.id
        });

        if(_type==0) {
            this.esignSigner.cloneRequest(body)
            .subscribe((result: CloneToDraftDto) => {

                if(result) {
                    this.local.removeItem("documentData");
                    this.router.navigate(['app/main/create-new-request'], { queryParams: { requestId: result?.requestId } });
                }

                this.isLoading = false;
            },
            (error) => {
                this.notify.error(this.l("AnErrorOccurred"));
                this.isLoading = false;
            });
        }
        else if (_type==1) {
            this.esignSigner.cloneRequestWithoutFields(body)
            .subscribe((result: CloneToDraftDto) => {

                if(result) {
                    this.local.removeItem("documentData");
                    this.router.navigate(['app/main/create-new-request'], { queryParams: { requestId: result?.requestId } });
                }

                this.isLoading = false;
            },
            (error) => {
                this.notify.error(this.l("AnErrorOccurred"));
                this.isLoading = false;
            });
        }

    }

    editRequest(){
        this.local.removeItem("documentData");
        this.router.navigate(['app/main/create-new-request'], { queryParams: { requestId: this.request.id } });
    }

    deleteDraft(){
        this.isLoading = true;
        this._esignRequest.deleteDraftRequest(this.request.id)
        .pipe(finalize(() => {
            this.onDeleteRequest.emit(null);
            this.isLoading = false;
        }))
        .subscribe(() => {
            this.notify.success(this.l('DeleteActionSuccess'));
        })
    }

    tab_default_selecting(ev) {
        if (ev.selectingIndex == 2 && this.isShowTabRefDocuments == true)
        {
            this.isShowTabRefDocuments = false;
            ev.cancel = true;
        }
    }



    isShowTabRefDocuments: boolean = true; // khi show open window, khong change tab / phuongdv - 2024-05-06
    refDocumentsWindow;
    showTabRefDocuments(refDocuments, event) {
        // this.isShowTabRefDocuments = true;

        // if(this.allowAction == true) {

        //     if(this.backPageUrl == 'view-detail'){
        //         this.local.setItem('backPageUrl', 'view-detail=' + this.request.id.toString());
        //     }
        //     else {
        //         this.local.setItem('backPageUrl', 'document-management=' + this.request.id.toString());
        //     }

        //     // this.router.navigate(['/app/main/view-reference/' + this.request.id]);

        //     let pX = event.clientX;
        //     let pY = event.clientY;
        //     for (let index = 0; index < this.request.refDocument.length; index++) {
        //         const element = this.request.refDocument[index];
        //         console.log(element);

        //         window.open('/app/main/view-reference/' + element.requestRefId, '_blank', 'width=1200,height=800,left='+pX+',top='+pY+'');
        //         pX+=22;
        //         pY+=22;
        //     }
        // }
        // let data = {
        //     refDocument: this.request.refDocument,
        //     isRefDocument: this.request?.summary?.statusCode == 'OnProgress' && this.request?.summary?.requesterUserId == this.appSession.userId,
        //     allowAction: this.allowAction
        // }
        // this.local.setItem("openwindowRefDocuments", data);

        //this.refDocumentsWindow = window.open('/app/main/document-management/view-refDocument', '_blank', 'width=600,height=400,left='+event.clientX+',top='+event.clientY+'');

    }
}
