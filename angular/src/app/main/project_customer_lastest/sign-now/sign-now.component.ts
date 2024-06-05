
import { Component, ElementRef, Injector, OnInit, ViewChild } from '@angular/core';
import { AppComponentBase } from '@shared/common/app-component-base';
import { AnnotationService, BookmarkViewService, FormDesignerService, FormFieldsService, LinkAnnotationService, LoadEventArgs, MagnificationService, NavigationService, PdfViewerComponent, PrintService, SignatureFieldSettings, TextFieldSettings, TextSearchService, TextSelectionService, ThumbnailViewService, ToolbarService } from '@syncfusion/ej2-angular-pdfviewer';
import { SignSuccessComponent } from '../esign-modal/sign-success/sign-success.component';
import { RejectModalComponent } from '../esign-modal/reject-modal/reject-modal.component';
import { ReassignModalComponent } from '../esign-modal/reassign-modal/reassign-modal.component';
import * as FileSaver from "file-saver";

import { EsignRequestWebServiceProxy,MstEsignUserImageServiceProxy,
        SignDocumentInputDto, EsignRequestInfomationDto,
        MstEsignUserImageSignatureInput, RejectInputDto, EsignSignerListServiceProxy, } from '@shared/service-proxies/service-proxies';

import { LocalStorageService } from '@shared/utils/local-storage.service';
import { AppConsts } from '@shared/AppConsts';
import { PopupSignatureComponent } from './popup-signature/popup-signature.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Router } from '@angular/router';
import { HttpClient } from "@angular/common/http";



@Component({
    selector: 'app-sign-now',
    templateUrl: './sign-now.component.html',
    styleUrls: ['./sign-now.component.less'],
    animations: [appModuleAnimation()],
    providers: [LinkAnnotationService, BookmarkViewService, MagnificationService, 
        ThumbnailViewService, ToolbarService, NavigationService, 
        TextSearchService, TextSelectionService, PrintService, AnnotationService, FormFieldsService, FormDesignerService, ],
})
export class SignNowComponent extends AppComponentBase implements OnInit {

    public documentUrl1: string = 'https://cdn.syncfusion.com/content/pdf/form-designer.pdf';
    public documentUrl2: string = "https://cdn.syncfusion.com/ej2/23.1.40/dist/ej2-pdfviewer-lib";
    public documentUrl: string;


    serviceUrl = `${AppConsts.remoteServiceBaseUrl}/api/PdfViewer`;
    @ViewChild('popupSignature', { static: true }) popupSignature!: PopupSignatureComponent| undefined;

    @ViewChild('pdfviewer')
    public pdfviewerControl: PdfViewerComponent;
    @ViewChild('successModal') successModal!: SignSuccessComponent;
    @ViewChild('rejectModal') rejectModal!: RejectModalComponent;
    @ViewChild('reassignModal') reassignModal!: ReassignModalComponent;


    //save signature
    isSaveUploadSignature:boolean = false;
    SaveImageSignatureTrimBase64: string = '';
    objSaveSignature: MstEsignUserImageSignatureInput = new MstEsignUserImageSignatureInput();


    selectedRequest : EsignRequestInfomationDto = new EsignRequestInfomationDto();

    listTemplateSignature = [
        {id : 1,imgUrl: "/assets/common/images/signature.png", isUse: true},
        {id : 2,imgUrl: "/assets/common/images/signature (1).png", isUse: false},
        {id : 3,imgUrl: "/assets/common/images/signature (2).png", isUse: false},
        {id : 4,imgUrl: "/assets/common/images/signature (3).png", isUse: false},
    ];

    RequestID: number;
    SignerId: number;
    selectDocumentId: number;

    defaultSelectedDocumentId: number;
    tabMenuFilter: number;
    dataSummary:any;
    listFieldSignatureForSigner: any[]= [];
    isSignature:boolean = false;
    SignatureTemplateID:number;
    SignatureType:number = 1; //1: Template, 2: Draw, 3: Upload
    SignatureImageBase64:string = "";
    tempSignatureDefaultId: number;

    tabMode:string = "TEMPLATE";
    RequestAction:string = '';
    paramSigner;
    requesterUserId;
    public listDocumentsfields: Object = { text: 'documentName', value: 'id' };



    listDocuments = [
        {
            id: 123,
            documentName: "PaymentRequest (1).pdf",
            documentPath: "https://esign-standby.toyotavn.com.vn:5001/Upload/DownloadFile?hash=C15DA1F2B5E5ED6E6837A3802F0D1593",
            documentOrder: 1,
            signatureCount:0,
            totalPage: 2,
            totalSize: 136
        }
    ];

    //dropdownlist signature
    selectedPage: number = 0;
    listSignaturePage = [{
        qty: 0,
        documentId: 0,
        pageNum: 0,
        signerId: 0,
        isDigitalSignature:false,
        typeId:0,
        code: ""
    }];



    constructor(injector: Injector,
        private _esignRequest: EsignRequestWebServiceProxy,
        private _mstEsignUserImage: MstEsignUserImageServiceProxy,
        private router: Router,
        private _http: HttpClient,
        private esignSigner: EsignSignerListServiceProxy,
        private local : LocalStorageService,
        // private _color: MstEsignColorServiceProxy,
        ) {
        super(injector);

    }

    ngOnInit() {
        this.SignerId = this.appSession.userId;
        // this.popupSignature.SignerId = this.appSession.userId;
        // this.popupSignature.popupDrawFullName = this.appSession.user.name;

        // this.local.getItem("selectedRequestAction" ,(err, data)=>{
        //     this.RequestAction = data;
        // });
        // this.getColor();

        this.local.getItem("selectedDocumentId" ,(err, data)=>{
            if(data) {
                this.defaultSelectedDocumentId = data.docId;

            }
        });


        this.local.getItem("selectedRequest",(err, data)=>{
            this.RequestID = data.id;
            this.selectedRequest = data;
            this.paramSigner = [];
            data?.signers?.map(e => e.signers?.map(eS => {
                this.paramSigner.push(eS)
            }));
            this.requesterUserId = data?.summary?.requesterUserId;
            this.listDocuments = [];
            this.selectedRequest.documents.forEach(e => {
                this.listDocuments.push(Object.assign(e, {
                    signatureCount: 0
                }));
            });
            if(this.defaultSelectedDocumentId > 0) this.selectDocumentId = this.defaultSelectedDocumentId;
            else this.selectDocumentId =  this.listDocuments[0].id;


            this.dataSummary = data.summary;
            this.documentUrl = this.selectedRequest.documents.find(e=> e.id == this.selectDocumentId).documentPath;


            this._esignRequest.getEsignPositionsByRequestId(this.RequestID)
            .subscribe(result =>{
                if(result) {

                    result.items.map(e => {
                        this.listFieldSignatureForSigner.push(Object.assign(e, {
                            htmlId: null,
                            // isSignature: false
                        }));
                    });

                    //1: Signature, 2: Name, 3: Title, 4: Date Signed, 5: Text, 6: Company
                    this._mstEsignUserImage.getListSignatureByUserIdForWeb().subscribe(res =>{
                        if(res) {
                            this.listTemplateSignature = res.items;
                            let _default = res.items.find(img => img.isUse == true);
                            if(_default)
                            {
                                this.tempSignatureDefaultId = _default.id;
                                //set default vào API sai
                                let _listSignature  = this.listFieldSignatureForSigner.filter(eS =>
                                                                eS.documentId == this.selectDocumentId &&
                                                                eS.typeId == 1 && //1: Signature, 2: Name, 3: Title, 4: Date Signed, 5: Text, 6: Company
                                                                eS.signerId == this.SignerId);
                                for(let eSi = 0;_listSignature[eSi]; eSi++)
                                {
                                    if(_listSignature[eSi].tempSignatureDefaultId == null) {
                                        _listSignature[eSi].tempSignatureDefaultId = _default.id;
                                        _listSignature[eSi].signatureImage = _default.imgUrl;
                                    }
                                }
                            }

                        }
                    });

                    this.mergeCountSignatureDocument(this.listDocuments, this.listFieldSignatureForSigner);
                }
            });

            this.loadSignaturePage();

        });


        this._mstEsignUserImage.getListSignatureByUserIdForWeb().subscribe(res =>{
            if(res) {
                this.listTemplateSignature = res.items;
            }
        });

        //upload image preview
        // this.dropElement = document.getElementById('signatureUploadImage') as HTMLElement;
    }

    ChangeFileDocument(e, _page?){
        this.selectDocumentId = e;
        this.documentUrl = this.selectedRequest.documents.find(_file=>_file.id == e).documentPath;

        this.loadSignaturePage(_page);
    }

    mergeCountSignatureDocument(_listdoc:any , _signature:any){
        for(let i=0; _listdoc[i]; i++){
            let _doc = this.listFieldSignatureForSigner.filter(e => e.documentId == _listdoc[i].id &&
                                                                    e.typeId == 1 &&
                                                                    e.signerId == this.SignerId);

            if(_doc) _listdoc[i].signatureCount = _doc.length;
            else _listdoc[i].signatureCount = 0;
        }

    }
    addFieldFromAPI() {
        for(let i =0; this.listFieldSignatureForSigner[i]; i++){
            let _item = this.listFieldSignatureForSigner[i];

            this.pdfviewerControl.formDesignerModule.addFormField("Textbox",
            { bounds: { X: _item.positionX, Y: _item.positionY, Width: _item.positionW, Height: _item.positionH } } as TextFieldSettings); //false, 'signature_' + _item.id

        }
    }

    public documentLoaded(e: LoadEventArgs): void {
        //this.pdfviewerControl.formDesignerModule.addFormField("SignatureField", { bounds: { X: 157, Y: 303, Width: 200, Height: 43 } } as SignatureFieldSettings);
        //this.pdfviewerControl.formDesignerModule.addFormField("Textbox", {  bounds: { X: 157, Y: 523, Width: 200, Height: 43 } } as TextFieldSettings);
        //kiểm tra Positions Signature loaded?
        this.async_await_loaded_positions_signature();
        // this.loaded_PDF_field();
        // this.loaded_HTLM_field();
        // let formField = this.pdfviewerControl.retrieveFormFields();
    }

    pendding_rendering_field_html(_second?){

        setTimeout(() => {
            this.loaded_PDF_field();
            this.loaded_HTLM_field();
        }, (!_second)? 2000: _second);
    }

    /**
     *
     *
     * @memberof SignNowComponent
     * chạy kiểm tra Positions Signature đã được load API done
     */
    loaded_PDF_field_number: number;
    async_await_loaded_positions_signature(){
        setTimeout(() => {
            if(this.listFieldSignatureForSigner.length > 0) {
                this.loaded_PDF_field();
                this.pendding_rendering_field_html(1000);

            } else  {
                this.async_await_loaded_positions_signature();
            }
        }, 100);
    }

    /**
     *
     *
     * @memberof SignNowComponent
     * lấy id của field trên pdfviewer(do id tự zen mỗi lần load) -> map vào với data get từ db
     */
    loaded_PDF_field() {
        // let formFieldPDF = this.pdfviewerControl.retrieveFormFields();
        let formFieldPDF = this.pdfviewerControl.formFields;
        // this.loaded_PDF_field_number = formFieldPDF.length;

        for(let i=0; formFieldPDF[i]; i++) {
            // if(formFieldPDF[i].name.includes("signature")) {
                let _x = Math.round(formFieldPDF[i].bounds.x); // (formFieldPDF[i].bounds as any).x
                let _y = Math.round(formFieldPDF[i].bounds.y);  //(formFieldPDF[i].bounds as any).y
                let _field = this.listFieldSignatureForSigner.find(e => e.positionX ==  _x &&
                                                                        e.positionY == _y &&
                                                                        e.signerId == this.SignerId);
                let normalField = this.listFieldSignatureForSigner.find(e => e.positionX ==  _x &&
                    e.positionY == _y && e.pageNum == formFieldPDF[i].pageNumber
                    );
                    normalField.htmlId = formFieldPDF[i].id + "_content_html_element";
                if(!_field) continue;

                _field.htmlId = formFieldPDF[i].id + "_content_html_element";
                if(!_field.signatureImage || _field.signatureImage == null || _field.signatureImage == "") {
                    _field.signatureImage = "/assets/common/images/Sign_here.png";
                    _field.isSignature = false;
                    this.isSignature = false;

                    //tạm thời API đang sai , set ngược lại API
                    this.tempSignatureDefaultId = (_field.tempSignatureDefaultId == null)? 0:_field.tempSignatureDefaultId;
                    // _field.tempSignatureDefaultId = this.tempSignatureDefaultId
                }
            // }
        }
    }

    /**
     *
     *
     * @memberof SignNowComponent
     * dom html các field trên pdfviewer, push image vào field
     */
    loaded_HTLM_field () {
        let d = document.querySelectorAll('#pdfViewer .foreign-object > .foreign-object > .foreign-object');


        for(let i=0; d[i]; i++) {

            let _field_node_html = d[i].parentElement;
            let input =_field_node_html.querySelector('input');

            let elemId = _field_node_html.getAttribute('id');
            let fielData = this.listFieldSignatureForSigner.find(e => e.htmlId == elemId);
            // console.log(this.listFieldSignatureForSigner)
            // console.log(fielData)
            console.log(fielData?.rotate, fielData);

            if (fielData?.rotate && Number(fielData?.rotate) != 0 ) this.setupRotationForField(elemId.split('_')[0],fielData.rotate,fielData?.backgroundColor)

            //1: Signature, 2: Name, 3: Title, 4: Date Signed, 5: Text, 6: Company
            if (input.getAttribute("name").includes("signature") && fielData.signerId == this.SignerId) {
                let _field_node_html_id = _field_node_html.getAttribute('id');

                let _field = this.listFieldSignatureForSigner.find(e => e.htmlId == _field_node_html_id);
                // if(!_field) this.loaded_PDF_field();

                if(!_field) continue;

                //kiểm tra đã load field chưa, có rồi thì bỏ qua
                // if(_field_node_html.classList.contains('loaded') == true) continue;

                let _img = _field_node_html.querySelectorAll<HTMLElement>(".img_signature");
                for(let i=0; _img[i]; i++) { _img[i].remove(); }

                let cssRotate = "";
                if (_field?.rotate && Number(_field?.rotate) != 0 ) cssRotate = "transform: rotate("+_field?.rotate+"deg); ";

                _field_node_html.innerHTML  += '<img class="img_signature img_signature_'+ _field.id +' " style="position: absolute; z-index: 2; '+cssRotate+'" src="'+_field.signatureImage+'"/>';
                _field_node_html.classList.add('fieldSignature-item','loaded', 'field_signature_id_' + _field.id);


            }
        }
    }

    setupRotationForField(selectedFieldId, rotationValue ,originalColor?){
        // let group = this.signerData.find(e => e.signers.some(p => p.formFields?.some(k => k.fieldId == selectedFieldId)));
        // let me = group?.signers?.find(e => e.formFields?.some(k => k.fieldId == selectedFieldId))
        // let field = me?.formFields?.find(e => e.fieldId == selectedFieldId);

        let img = document.getElementById(selectedFieldId );
        if (img){
            // // console.log(img)
            img.style.transform = 'rotate(' + rotationValue + 'deg)';
            // img.style.height = 'fit-content'
            img.style.border = 'none';
            img.style.background = '#0000'
            img.style.width = 'auto'
            img.style.height = 'auto'


            img.parentElement.style.backgroundColor = originalColor ;
            img.parentElement.style.border = '1px solid black';

            img.parentElement.style.display = 'flex';
            img.parentElement.style.justifyContent = 'space-around';
            img.parentElement.style.alignItems = 'center';

        }
    }


    onPdfViewerLoad(event: any): void {
        const pdfViewer = event.detail;
        pdfViewer.on('zoomChange', (args: any) => {
          // This event handler will be called when the zoom level changes.
          // You can perform actions when the zoom changes, including when form fields reload.
        });
    }



    onApplyTemplateSignature(param){
        this.tabMode = param.tabMode;
        this.isSignature = param.isSignature;
        this.SignatureType = param.SignatureType;
        this.SignatureImageBase64 = param.SignatureImageBase64;
        this.SignatureTemplateID = param.SignatureTemplateID;
        this.isSaveUploadSignature = param.isSaveUploadSignature;

        for(let i=0; this.listFieldSignatureForSigner[i]; i++){
            this.listFieldSignatureForSigner[i].signatureImage = this.SignatureImageBase64;
            this.listFieldSignatureForSigner[i].isSignature = true;
        }

        this.pendding_rendering_field_html(200);

        // phuongdv - kí luôn 2024-04-10
        this.CompleteSigning();
    }


    onApplyDrawText_Signature(param){

        this.tabMode = param.tabMode;
        this.isSignature = param.isSignature;
        this.SignatureType = param.SignatureType;
        this.SignatureImageBase64 = param.SignatureImageBase64;
        this.SaveImageSignatureTrimBase64 = param.SaveImageSignatureTrimBase64;
        this.isSaveUploadSignature = param.isSaveUploadSignature;

        for(let i=0; this.listFieldSignatureForSigner[i]; i++){
            this.listFieldSignatureForSigner[i].signatureImage = this.SignatureImageBase64;
            this.listFieldSignatureForSigner[i].isSignature = true;
        }


        this.pendding_rendering_field_html(200);

        // phuongdv - kí luôn 2024-04-10
        this.CompleteSigning();

    }

    onApplyDrawSignatureDefault(param){

        this.tabMode = param.tabMode;
        this.isSignature = param.isSignature;
        this.SignatureType = param.SignatureType;
        this.SignatureImageBase64 = param.SignatureImageBase64;
        this.SaveImageSignatureTrimBase64 = param.SaveImageSignatureTrimBase64;
        this.isSaveUploadSignature = param.isSaveUploadSignature;

        for(let i=0; this.listFieldSignatureForSigner[i]; i++){
            this.listFieldSignatureForSigner[i].signatureImage = this.SignatureImageBase64;
            this.listFieldSignatureForSigner[i].isSignature = true;
        }


        this.pendding_rendering_field_html(200);

        // phuongdv - kí luôn 2024-04-10
        this.CompleteSigning();
    }

    onApplyUploadSignature(param){

        this.tabMode = param.tabMode;
        this.isSignature = param.isSignature;
        this.SignatureType = param.SignatureType;
        this.SignatureImageBase64 = param.SignatureImageBase64;
        this.SaveImageSignatureTrimBase64 = param.SaveImageSignatureTrimBase64;
        this.isSaveUploadSignature = param.isSaveUploadSignature;

        for(let i=0; this.listFieldSignatureForSigner[i]; i++){
            this.listFieldSignatureForSigner[i].signatureImage = this.SignatureImageBase64;
            this.listFieldSignatureForSigner[i].isSignature = true;
        }

        this.pendding_rendering_field_html(200);

        // phuongdv - kí luôn 2024-04-10
        this.CompleteSigning();
    }

    objselectFieldSignature: any;
    OpenPopupSignature(_signatureId,x,y) {

        this.objselectFieldSignature = this.listFieldSignatureForSigner.find(e => e.positionX == x && e.positionY == y);
        let _pop = document.querySelector<HTMLElement>(".popup-signature");

        _pop.style.display = 'flex';

        // this.popupSignature.show()
    }

    formFieldClick(e){
        // this.pdfviewerControl.enableFormDesigner = false;
        let field = this.pdfviewerControl.formDesigner.getFormField(e.field.id);
        if (field.name.includes("signature")){
            let _x = Math.round(field.bounds.x);// Number(this.pdfviewerControl.zoomValue.toString().replace('%','')) * 100;
            let _y = Math.round(field.bounds.y);  // Number(this.pdfviewerControl.zoomValue.toString().replace('%','')) * 100;
            let _field = this.listFieldSignatureForSigner.find(e => e.positionX == _x &&
                                                                        e.positionY == _y &&
                                                                        e.signerId == this.SignerId);
            if(_field) this.OpenPopupSignature(e.field.id, _x,_y);
            // alert(e);
        }
    }

    annotationDoubleClick(e) {
        // this.pdfviewerControl.enableFormDesigner = false;
        // alert(e);
    }


    loading: boolean = false;
    CompleteSigning() {

        if(this.loading == true) return;

        if(this.isSignature) { //đã chọn chữ kí
            // signerId!: number;
            // requestId!: number;
            // userId!: number;
            // imageSign!: string | undefined;
            let bodySign = new SignDocumentInputDto();
            bodySign.typeSignId = this.SignatureType;   //1: Template(default), 2: Draw, 3: Upload
            bodySign.templateSignatureId = this.tempSignatureDefaultId;
            bodySign.requestId = this.RequestID;
            bodySign.imageSign = (this.SignatureType != 1)? this.SignatureImageBase64.replace('data:image/png;base64,','') : undefined;
            this.loading = true;
            this.spinnerService.show();
            this._esignRequest.signerSign(bodySign).subscribe(e => {
                this.spinnerService.hide();
                if(e) this.successModal.show(e);
                else this.successModal.show(e);
                this.loading = false;
            },
            (err) => {
                this.spinnerService.hide();
                this.loading = false;
            });
        }
        else {  //chưa chọn chữ kí, get default
            // defautl = template

            let bodySign = new SignDocumentInputDto();
            bodySign.typeSignId = this.SignatureType;   //1: Template(default), 2: Draw, 3: Upload
            bodySign.templateSignatureId = this.tempSignatureDefaultId;
            bodySign.requestId = this.RequestID;
            // bodySign.imageSign = this.SignatureImageBase64;
            if(!this.tempSignatureDefaultId || this.tempSignatureDefaultId == null || this.tempSignatureDefaultId < 0){
                this.notify.error(this.l('PleaseSelectSigner'));
            }else {

                this.loading = true;
                this._esignRequest.signerSign(bodySign).subscribe(e => {
                    if(e) this.successModal.show(e);
                    else this.successModal.show(e);

                    this.loading = false;
                },
                (err) => {
                    this.spinnerService.hide();
                    this.loading = false;
                });
            }
        }

        // // //lưu chữ kí
        // if(this.isSaveUploadSignature) {

        //     if(this.tabMode == "TEMPLATE") {

        //     }
        //     else if(this.tabMode == "DRAW") {
        //         // this.SignatureImageBase64
        //         let img = <HTMLImageElement>document.getElementById('_SaveImageSignature');

        //         if(img && this.SaveImageSignatureTrimBase64 != ''){
        //             img.src = this.SaveImageSignatureTrimBase64;
        //             setTimeout(() => {
        //                 this.objSaveSignature.signerId = this.SignerId;
        //                 this.objSaveSignature.imgWidth = img.naturalWidth;
        //                 this.objSaveSignature.imgHeight = img.naturalHeight;
        //                 this.objSaveSignature.imgSize = 0;
        //                 this.objSaveSignature.imageSignature = this.SaveImageSignatureTrimBase64.replace('data:image/png;base64,','');

        //                 this._mstEsignUserImage.saveImageSignature(this.objSaveSignature).subscribe(e => {

        //                     //lấy list signature default
        //                     this._mstEsignUserImage.getListSignatureByUserIdForWeb().subscribe(res =>{
        //                         if(res) {
        //                             this.listTemplateSignature = res.items;
        //                         }
        //                     });
        //                 });
        //             }, 200);

        //         }
        //     }
        //     else if(this.tabMode == "UPLOAD") {
        //         let img = <HTMLImageElement>document.getElementById('_SaveImageSignature');
        //         if(img && this.SaveImageSignatureTrimBase64 != ''){
        //             img.src = this.SaveImageSignatureTrimBase64;
        //             setTimeout(() => {
        //                 this.objSaveSignature.signerId = this.SignerId;
        //                 this.objSaveSignature.imgWidth = img.naturalWidth;
        //                 this.objSaveSignature.imgHeight = img.naturalHeight;
        //                 this.objSaveSignature.imgSize = 0;
        //                 this.objSaveSignature.imageSignature = this.SaveImageSignatureTrimBase64.replace('data:image/png;base64,','');

        //                 this._mstEsignUserImage.saveImageSignature(this.objSaveSignature).subscribe(e => {

        //                     //lấy list signature default
        //                     this._mstEsignUserImage.getListSignatureByUserIdForWeb().subscribe(res =>{
        //                         if(res) {
        //                             this.listTemplateSignature = res.items;
        //                         }
        //                     });
        //                 });
        //             }, 200);
        //         }
        //     }

        // }
    }




    downloadDocument(){
        let _download_doc = this.listDocuments.find(e => e.id == this.selectDocumentId);
        if(_download_doc){
            this.spinnerService.show();
            this._http.get(this.downloadUrl,{
                params: {
                    'hash': _download_doc.documentPath.split('hash=')[1],
                    'isDownload': "true",
                }
                , responseType: 'blob'
            })
            .subscribe(blob => {
                FileSaver.saveAs(blob, _download_doc.documentName);
                this.spinnerService.hide();
            })
        }
    }

    rejectRequest(event){
        let body = new RejectInputDto({
            note: event,
            requestId: this.RequestID
        });
        this.esignSigner.rejectRequest(body).subscribe(() => {
            this.notify.success(this.l('RejectActionSuccess'));
            this.router.navigate(['app/main/document-management']);
        })
    }
    onReassignRequest(event){
            this.notify.success(this.l('ReassignSuccess'));
            this.router.navigate(['app/main/document-management']);
    }

    BackPage() {

        if (this.defaultSelectedDocumentId > 0 || this.RequestID > 0) {
            this.local.removeItem("selectedDocumentId");
            history.back();
        }
        else{
            history.back();
        }
    }




    loadSignaturePage(_page?) {
        this._esignRequest.getEsignSignaturePageByRequestId(this.RequestID).subscribe(result =>{
            if(result) {
                this.listSignaturePage = result;
                // có tham số _page tức là user bấm button page sang file khác
                if(_page) this.checkCurrentPage(_page); // kiểm tra và nhảy đến page cần current
                else {
                    // khi không có tham số _page kiểm tra page đầu tiên có signature không.
                    // nếu có thì tự động selected ở page 1 luôn
                    this.listSignaturePage.find(e => {
                        if(e.pageNum ==1) this.selectedPage = 1;
                    });
                }
            }
        });
    }

    checkCurrentPage(_page){
        if(!this.pdfviewerControl?.currentPageNumber) {
            // kiểm tra page load xong chưa (có thể sử dụng để check document load done. ex: zoom, change)
            setTimeout(() => {
                this.checkCurrentPage(_page);
            }, 500);
        }else {
            // load xong document -> nhảy đến page cần seleted
            this.ChangePageSignature(this.selectDocumentId, _page, true);
        }
    }

    /**
     * event click button page có chữ ký
     *
     * @input documentId cần selected
     * @input page cần nhảy đến
     * @input _first? truyền khi user chọn page ở 1 file khác. default = undefined
     */
    ChangePageSignature(_documentId, _page, _first?) {

        if(_documentId == this.selectDocumentId) {

            if(this.selectedPage == _page) {
                if(_first == undefined) this.selectedPage = 0;
            }
            else {
                this.selectedPage = _page;
                this.isGotopage = true;
                this.pdfviewerControl?.navigation.goToPage(_page);
                this.pendding_rendering_field_html();
            }
        }
        else {

            this.ChangeFileDocument(_documentId, _page);
        }
    }

    /**
     * event call liên tục để check trạng thái cần selected cho button pageSignature
     *
     * @input documentId cần selected
     * @input page cần selected
     */
    isShowSelectedPage(_documentId, _page) {

        // this.selectedPage = 0: lần đầu load trang, hoặc không chọn page nào, selected toàn bộ button page theo document đang selected
        if (this.selectedPage == 0) {
            if (_documentId != this.selectDocumentId) return false;
            else return false;
        } else {

            if (_documentId != this.selectDocumentId) return false;
            else {
                if(this.selectedPage == _page) return true;
                else return false;
            }
        }
    }

    // khi user cuộn pdf
    isGotopage: boolean = false;
    pageChange(e){
        if(this.isGotopage) {   //event từ người dùng bấm tới chứ không phải tự động
            this.isGotopage = false;
            this.pendding_rendering_field_html();
            return;
        }


        // event từ scroll cuộn lên xuống
        if(this.selectedPage != e.currentPageNumber && this.selectedPage != 0) {
            this.selectedPage = 0;
        }
        else if(this.selectedPage == 0) {
            let _selected = this.listSignaturePage.find(f => f.pageNum == e.currentPageNumber);
            if(_selected) {
                this.selectedPage = e.currentPageNumber;
            }
        }

        this.pendding_rendering_field_html();
    }


    public previousClicked(): void {
        this.pdfviewerControl.navigation.goToPreviousPage();
    }

    public nextClicked(): void {
        this.pdfviewerControl.navigation.goToNextPage();
    }

    public printClicked(): void {
        this.pdfviewerControl.print.print();
    }

    public downloadClicked(): void {
        let fileName: string = (document.getElementById('fileUpload') as HTMLInputElement).value.split('\\').pop();
        if (fileName !== '') {
            this.pdfviewerControl.fileName = fileName;
        }
        this.pdfviewerControl.download();
    }

    public pageFitClicked(): void {
        this.pdfviewerControl.magnification.fitToWidth();
    }

    public zoomInClicked(): void {
        this.pdfviewerControl.magnification.zoomIn();
    }

    public zoomOutClicked(): void {
        this.pdfviewerControl.magnification.zoomOut();
    }

    checkNeedSign(documentId){
        if(this.listFieldSignatureForSigner.some(e => e.documentId == documentId && e.signerId == abp.session.userId)) return true;
        else return false;
    }

}
