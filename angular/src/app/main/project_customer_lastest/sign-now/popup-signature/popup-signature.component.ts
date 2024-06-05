import { DropDownListComponent } from '@syncfusion/ej2-angular-dropdowns';
import { Component, ElementRef, Injector, Input, OnInit, Output, ViewChild, EventEmitter } from '@angular/core';

import { ModalDirective } from 'ngx-bootstrap/modal';
import { FileInfo, RemovingEventArgs, SelectedEventArgs, SignatureComponent, UploaderComponent } from '@syncfusion/ej2-angular-inputs';
import { isNullOrUndefined,createElement,detach, EventHandler  } from '@syncfusion/ej2-base';
import { createSpinner, showSpinner, hideSpinner } from '@syncfusion/ej2-popups';
import { MstEsignUserImageDefaultWebInput, MstEsignUserImageServiceProxy, MstEsignUserImageSignatureInput } from '@shared/service-proxies/service-proxies';
import { DataFormatService } from '@app/shared/common/services/data-format.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { CommonFunction } from '@app/main/commonfuncton.component';


@Component({
    selector: 'popup-signature',
    templateUrl: './popup-signature.component.html',
    styleUrls: ['./popup-signature.component.less']
})


export class PopupSignatureComponent extends AppComponentBase implements OnInit {
    @Input() documents: any;
    @Input() SignerId: any;
    @Input() popupDrawFullName: any;
    @Input() popupFreeText: string = '';
    @Input() hasSubmitBtn = false;
    @Input() isSignatureDigital = true;
    @Input() hasSignBtn = true;
    @Output() _data: any;
    @Output() onApplyTemplateSignature: EventEmitter<any> = new EventEmitter<any>();
    @Output() onApplyUploadSignature: EventEmitter<any> = new EventEmitter<any>();
    @Output() onApplyDrawText_Signature: EventEmitter<any> = new EventEmitter<any>();
    @Output() onApplyDrawSignatureDefault: EventEmitter<any> = new EventEmitter<any>();
    @Output() submitDocument: EventEmitter<any> = new EventEmitter<any>();
    paramsEmit;
    // @Input() signAndSubmitBtn = false;



    @ViewChild('popupSignature', { static: true }) modal: ModalDirective | undefined;

    @ViewChild('mixCanvasHtml') public mixCanvasHtml: ElementRef;
    @ViewChild('canvasSignature') public canvasSignature: ElementRef;
    @ViewChild('canvasSignatureFinal') public canvasSignatureFinal: ElementRef;
    @ViewChild('canvasFullname') public canvasFullname: ElementRef;
    @ViewChild('canvasDate') public canvasDate: ElementRef;
    @ViewChild('mixCanvasFullname_date') public mixCanvasFullname_date: ElementRef;
    ctx: CanvasRenderingContext2D;
    ctxSignatureFinal: CanvasRenderingContext2D;
    trimCtxSignature: CanvasRenderingContext2D;
    trimCtxFullname: CanvasRenderingContext2D;
    trimCtxDate: CanvasRenderingContext2D;
    ctxMixFullnameDate: CanvasRenderingContext2D;
    colorDraw:string = "#1E1E1E";

    @ViewChild('signature') public signature?: SignatureComponent;
    @ViewChild('signature_tmp') public signature_tmp?: SignatureComponent;
    @ViewChild('drawfont') public drawfont?: DropDownListComponent;
    @ViewChild('drawsize') public drawsize?: DropDownListComponent;



    ///upload image
    @ViewChild('canvasUploadImage') public canvasUploadImage: ElementRef;
    trimCtxUploadImage: CanvasRenderingContext2D;
    @ViewChild('previewupload') public uploadObj: UploaderComponent;
    public path: Object = {
        saveUrl: 'https://services.syncfusion.com/angular/production/api/FileUploader/Save',
        removeUrl: 'https://services.syncfusion.com/angular/production/api/FileUploader/Remove'
    };
    public allowExtensions: string = '.png, .jpg, .jpeg';
    public dropElement: HTMLElement ;
    public filesName: string[] = [];
    public filesDetails : FileInfo[] = [];
    public filesList: HTMLElement[] = [];
    public uploadWrapper: HTMLElement;
    public parentElement: HTMLElement;
    public UploadImageBase64:string;

    //save signature
    isSaveUploadSignature:boolean = false;
    SaveImageSignatureTrimBase64: string = '';

    // objSaveSignature: MstEsignUserImageSignatureInput = new MstEsignUserImageSignatureInput();
        /**
             *
             *
             * @memberof SignNowComponent
             * @memberof Popup, param, value
             */
        // public popupDrawFullName:string = "Hoan Bui Ta";
        public popupDrawDate: Date = new Date();
        public fontValue: string = 'Arial';
        public sizeValue: number = 20;
        public height: string = '200px';
        public formatdateValue: string = 'mm/dd/yyyy';

        public fontItems: Object[] = [
            { value: 'Arial' },
            { value: 'Serif' },
            { value: 'Sans-serif' },
            { value: 'Cursive' },
            { value: 'Fantasy' }
        ];
        public fontfields: Object = { text: 'value', value: 'value' };
        public sizeData: Object[] = [
            { text: '20', value: 20 },
            { text: '30', value: 30 },
            { text: '40', value: 40 },
            { text: '50', value: 50 }
        ];
        public sizefields: Object = { text: 'value', value: 'value' };

        public formatdateItems: Object[] = [
        { value: 'mm/dd/yyyy' },
        { value: 'mmm-dd-yy' },
        { value: 'mm/dd/yy' },

        ];
        public formatdatefields: Object = { text: 'value', value: 'value' };

        public drawW:number = 572;
        public drawH:number = 224;


        isSignature:boolean = false; // có chọn chữ kí không
        SignatureType:number = 1; //1: Template, 2: Draw, 3: Upload
        SignatureTemplateID:number;
        SignatureImageBase64:string = "";

        isDrawExists: boolean = false; // xử lý trim ghép name... etc, không tự động clear
        isUploadExists: boolean = false;// xử lý trim ghép name... etc, không tự động clear


        // listTemplateSignature = [
        //     {id : 1,imgUrl: "/assets/common/images/signature.png", isUse: true, isEmpty: false, },
        //     {id : 2,imgUrl: "/assets/common/images/signature (1).png", isUse: false, isEmpty: false,},
        //     {id : 3,imgUrl: "/assets/common/images/signature (2).png", isUse: false, isEmpty: false,},
        //     {id : 4,imgUrl: "/assets/common/images/signature (3).png", isUse: false, isEmpty: false,},
        // ];

        listTemplateSignature: any[]= []
        signature_empty:number = 0;
        fn:CommonFunction = new CommonFunction();
        isLoading:boolean = false;

    constructor(
        injector: Injector,
        private _mstEsignUserImage: MstEsignUserImageServiceProxy,
        private dataFormatService: DataFormatService,
    ) {
        // this.popupDrawFullName = this.appSession.user.name;
        super(injector)
        this._mstEsignUserImage.getListSignatureByUserIdForWeb().subscribe(res =>{
            if(res) {
                this.listTemplateSignature = res.items;
                this.signature_empty = (4 - this.listTemplateSignature.length);

                let _default = this.listTemplateSignature.find(e => e.isUse == true);
                if(_default) this._selectTemPlateSigature = _default.id;

            }
        });
    }

    // getSignatureEmpty() {

    //     return this.fn.fornumbers(this.signature_empty);
    // }

    ngOnInit() {
        //upload image preview
        this.dropElement = document.getElementById('signatureUploadImage') as HTMLElement;
        this.SignerId = this.appSession.userId;
        this.popupDrawFullName = this.appSession.user.name;

        this.isDrawExists = false;
        this.isUploadExists = false;
    }


    show(data?: any){
        this.modal.show();
    }

    hide(){
        this.modal.hide();
    }

    submitDoc(){

        if(this.tabMode == "TEMPLATE") {
            this.isSignature = true;
            this.SignatureType = 1;

            this.applyTemplateSignature();
        }
        else if(this.tabMode == "DRAW") {
            this.isSignature = true;
            this.SignatureType = 2;
            // if(this.isDrawExists) {
            //     // lần mở popup thứ 2 chưa clear nên chữ kí vẫn còn, không cần thực hiện trim.ghép name....
            //     this.applyDrawSignature_again(); // không vẽ, không draw, không trim lại
            // }
            // else {
                // lần mở đầu tiên hoặc mở tiếp theo(nếu clear) thực hiện lại trim, ghép name....
                this.applyDrawSignature();
                this.isDrawExists = true;
            // }
         }
        else if(this.tabMode == "UPLOAD") {
            this.isSignature = true;
            this.SignatureType = 3;

            // if(this.isUploadExists) {
            //     // lần mở popup thứ 2 chưa clear nên chữ kí vẫn còn, không cần thực hiện trim.ghép name....
            // }
            // else {
                // lần mở đầu tiên hoặc mở tiếp theo(nếu clear) thực hiện lại trim, ghép name....
                this.applyUploadSignature();
                this.isUploadExists = true;
            // }
        }

        // let _applyTemplateSignature = this.listTemplateSignature.find(e => e.id == this._selectTemPlateSigature);

        // let params = {
        //     tabMode: this.tabMode,
        //     isSignature: this.isSignature,
        //     SignatureType: this.SignatureType,
        //     SignatureImageBase64 : _applyTemplateSignature.imgUrl,
        //     SignatureTemplateID : _applyTemplateSignature.id,
        //     isSaveUploadSignature: this.isSaveUploadSignature,
        // }
        this.ClosePopupSignature(false); // không clear chữ kí
        // setTimeout(()=>{
        //     this.submitDocument.emit(this.paramsEmit)
        // },1001)
    }

    tabMode:string = "TEMPLATE";
    popup_change_tab(_tab, _tabmode) {

        let _title_tabs = document.querySelectorAll<HTMLElement>(".popup-signature .sign2.active, .popup-signature .tabcontent.active");
        if(_title_tabs) for(let i=0; _title_tabs[i]; i++) _title_tabs[i].classList.remove('active');

        let _select_tabs = document.querySelectorAll<HTMLElement>(".popup-signature .sign2." + _tab + ", .popup-signature .tabcontent." + _tab);
        if(_select_tabs) for(let i=0; _select_tabs[i]; i++) _select_tabs[i].classList.add('active');

        this.tabMode = _tabmode;

        if(this.tabMode == 'DRAW') {
            this.onDrawPreview();
        }
    }

    ClosePopupSignature(isClear:boolean) {
        let _pop = document.querySelector<HTMLElement>(".popup-signature");
        _pop.style.display = 'none';
            if(isClear) {
                setTimeout(() => {
                    if(this.tabMode == 'DRAW') {
                        this.isDrawExists = false;
                        this.onClearDraw();
                    }
                    else if (this.tabMode == 'UPLOAD') {
                        this.isUploadExists = false;
                        this.clearUploadImageSignature();
                    }
                }, 1300);
            }
    }

    // objselectFieldSignature: any;
    // OpenPopupSignature(_signatureId,x,y) {

    //     this.objselectFieldSignature = this.listFieldSignatureForSigner.find(e => e.positionX == x && e.positionY == y);
    //     let _pop = document.querySelector<HTMLElement>(".popup-signature");
    //     // _pop.style.display = 'flex';

    //     this.popupSignature.show()
    // }

    _selectTemPlateSigature: any;
    selectTemplateSignature(_id) {
        this.isLoading = true;
        this._selectTemPlateSigature = _id;
        let _tmpSignature = document.querySelectorAll<HTMLElement>(".popup-signature .item-signature");
        if(_tmpSignature) for(let i=0; _tmpSignature[i]; i++) _tmpSignature[i].classList.remove('active');
        let _selectSignature = document.querySelector<HTMLElement>(".popup-signature .item-signature.itemSignature" + _id);
        if(_selectSignature) _selectSignature.classList.add('active');

        //update db
        let a = new MstEsignUserImageDefaultWebInput();
        a.id = _id;
        a.signerId = this.SignerId;
        this._mstEsignUserImage.updateSignatureDefautlForWeb(a).subscribe(e => {
            this.isLoading = false;
        });
    }

    applySignature(){
        if(this.tabMode == "TEMPLATE") {
            this.isSignature = true;
            this.SignatureType = 1;

            this.applyTemplateSignature();
            this.ClosePopupSignature(false);
        }
        else if(this.tabMode == "DRAW") {
            this.isSignature = true;
            this.SignatureType = 2;
            // if(this.isDrawExists) {
            //     // lần mở popup thứ 2 chưa clear nên chữ kí vẫn còn, không cần thực hiện trim.ghép name....
            //     this.applyDrawSignature_again(); // không vẽ, không draw, không trim lại
            // }
            // else {
                // lần mở đầu tiên hoặc mở tiếp theo(nếu clear) thực hiện lại trim, ghép name....
                this.applyDrawSignature();
                this.isDrawExists = true;
            // }
         }
        else if(this.tabMode == "UPLOAD") {
            this.isSignature = true;
            this.SignatureType = 3;

            // if(this.isUploadExists) {
            //     // lần mở popup thứ 2 chưa clear nên chữ kí vẫn còn, không cần thực hiện trim.ghép name....
            // }
            // else {
                // lần mở đầu tiên hoặc mở tiếp theo(nếu clear) thực hiện lại trim, ghép name....
                this.applyUploadSignature();
                this.isUploadExists = true;
            // }
        }
    }

    applyDrawSignature_again () {
        if(this.applyFullnameToDrawSignature == true) { // có add name vào chữ kí không?

            let params = {
                tabMode: this.tabMode,
                isSignature: this.isSignature,
                SignatureType: this.SignatureType,
                SignatureImageBase64 : this.ctxSignatureFinal.canvas.toDataURL(),
                SaveImageSignatureTrimBase64 : this.SaveImageSignatureTrimBase64,
                isSaveUploadSignature: this.isSaveUploadSignature,
            }
            this.onApplyDrawText_Signature.emit(params);
        }
        else{
            let params = {
                tabMode: this.tabMode,
                isSignature: this.isSignature,
                SignatureType: this.SignatureType,
                SignatureImageBase64 : this.SignatureImageBase64,
                SaveImageSignatureTrimBase64 : this.SaveImageSignatureTrimBase64,
                isSaveUploadSignature: this.isSaveUploadSignature,
            }
            this.onApplyDrawSignatureDefault.emit(params);
        }
    }

    applyDrawSignature() {

        if(this.applyFullnameToDrawSignature == true) { // có add name vào chữ kí không?
            this.onMixText_Signature();
        }
        else{
            this.onDrawSignatureDefault();
        }
    }

    applyTemplateSignature(){


        //lấy được data template
        if(!this._selectTemPlateSigature) {
            this.notify.warn(this.l("NoPositionInPdf"));
            return;

        }

        let _applyTemplateSignature = this.listTemplateSignature.find(e => e.id == this._selectTemPlateSigature);

        // this.listFieldSignatureForSigner.find(e => e.id == this._selectTemPlateSigature)
        // this.objselectFieldSignature.signatureImage = _applyTemplateSignature.imgUrl;
        //set all signature
        this.SignatureImageBase64 = _applyTemplateSignature.imgUrl;
        this.SignatureTemplateID = _applyTemplateSignature.id;

        let params = {
            tabMode: this.tabMode,
            isSignature: this.isSignature,
            SignatureType: this.SignatureType,
            SignatureImageBase64 : _applyTemplateSignature.imgUrl,
            SignatureTemplateID : _applyTemplateSignature.id,
            isSaveUploadSignature: this.isSaveUploadSignature,
        }
        // for(let i=0; this.listFieldSignatureForSigner[i]; i++){
        //     this.listFieldSignatureForSigner[i].signatureImage = _applyTemplateSignature.imgUrl;
        //     this.listFieldSignatureForSigner[i].isSignature = true;
        // }
        this.paramsEmit = params;

        if(this.hasSubmitBtn){
            this.submitDocument.emit(params)
        }
        // set ảnh cho field kí trong file pdf
        this.onApplyTemplateSignature.emit(params);
        // if(_applyTemplateSignature){

        //     let _field = document.querySelector('.fieldSignature-item.field_signature_id_' + this.objselectFieldSignature.id);
        //     if(_field) {
        //         let _img = _field.querySelectorAll<HTMLElement>(".img_signature");
        //         for(let i=0; _img[i]; i++) { _img[i].remove(); }
        //         _field.innerHTML  += '<img class="img_signature img_signature_'+this.objselectFieldSignature.id+' img_signature" style="height: '+ (this.objselectFieldSignature.positionH - 2) +'px; top: 0px; position: absolute; z-index: 2; " src="'+_applyTemplateSignature.imgUrl+'"/>';
        //     }

        //     this.pendding_rendering_field_html();
        // }

    }
    applyUploadSignature() {

        if(this.UploadImageBase64 && this.UploadImageBase64 != '') {

            // this.objselectFieldSignature.signatureImage = this.UploadImageBase64;
            this.SignatureImageBase64 = this.UploadImageBase64;
            this.SaveImageSignatureTrimBase64 = this.UploadImageBase64;

            let params = {
                tabMode: this.tabMode,
                isSignature: this.isSignature,
                SignatureType: this.SignatureType,
                SignatureImageBase64 : this.UploadImageBase64,
                SaveImageSignatureTrimBase64 : this.UploadImageBase64,
                isSaveUploadSignature: this.isSaveUploadSignature,
            }

            this.paramsEmit = params;
            this.SaveImageTemplateSignature();
            if(this.hasSubmitBtn){
                this.submitDocument.emit(params)
            }
            this.onApplyUploadSignature.emit(params);
            this.ClosePopupSignature(false);

            // for(let i=0; this.listFieldSignatureForSigner[i]; i++){
            //     this.listFieldSignatureForSigner[i].signatureImage = this.objselectFieldSignature.signatureImage;
            //     this.listFieldSignatureForSigner[i].isSignature = true;
            // }

            // let _field = document.querySelector('.fieldSignature-item.field_signature_id_' + this.objselectFieldSignature.id);
            // if(_field) {
            //     let _img = _field.querySelectorAll<HTMLElement>(".img_signature");
            //     for(let i=0; _img[i]; i++) { _img[i].remove(); }
            //     _field.innerHTML  += '<img class="img_signature img_signature_'+this.objselectFieldSignature.id+' img_signature" style="height: '+ (this.objselectFieldSignature.positionH - 2) +'px; top: 0px; position: absolute; z-index: 2; " src="'+this.objselectFieldSignature.signatureImage+'"/>';

            // }
            // this.pendding_rendering_field_html();
        }

    }

/*
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

*/

    annotationDoubleClick(e) {
        // this.pdfviewerControl.enableFormDesigner = false;
        // alert(e);
    }

    applyFullnameToDrawSignature:boolean = false;
    onDrawPreview(){

        this.signature_tmp.clear();
        this.signature_tmp.refresh();

        if(this.ctxMixFullnameDate) {
            this.ctxMixFullnameDate.canvas.width = this.drawW;
            this.ctxMixFullnameDate.canvas.height = this.drawH;
            this.ctxMixFullnameDate.clearRect(0,0, this.drawW, this.drawH);
            // this.ctxMixFullnameDate.beginPath();
        }
        if(this.trimCtxFullname) {
            this.trimCtxFullname.canvas.width = this.drawW;
            this.trimCtxFullname.canvas.height = this.drawH;
            this.trimCtxFullname.clearRect(0,0, this.drawW, this.drawH);
            // this.trimCtxFullname.beginPath();
        }
        if(this.trimCtxDate) {
            this.trimCtxDate.canvas.width = this.drawW;
            this.trimCtxDate.canvas.height = this.drawH;
            this.trimCtxDate.clearRect(0,0, this.drawW, this.drawH);
            // this.trimCtxDate.beginPath();
        }

        let imgFullname = new Image(this.drawW,this.drawH); //224
        let imgDate = new Image(this.drawW,this.drawH); //this.drawH

        this.signature_tmp.draw(this.popupDrawFullName, (this.drawfont as any).value, (this.drawsize as any).value, 20,50); //150
        let _drawfullname = this.signature_tmp.getSignature(); // full name IMG Base64
        this.signature_tmp.clear();
        this.signature_tmp.refresh();

/*
        // this.signature_tmp.draw(this.dataFormatService.dateFormat(this.popupDrawDate) , (this.drawfont as any).value, (this.drawsize as any).value, 20,50); //180
        let _dateformat = '';
        if(_date)  _dateformat = this.join(_date, '/');  // _date.getDate() + '/' + (_date.getMonth()+1) + '/' + _date.getFullYear();
        else _dateformat = this.join(this.popupDrawDate, '/'); // this.popupDrawDate.getDate() + '/' + (this.popupDrawDate.getMonth()+1) + '/' + this.popupDrawDate.getFullYear();
*/


        let _drawdate = '';
        if(this.popupFreeText!= ''){
            this.signature_tmp.draw( this.popupFreeText , (this.drawfont as any).value, (this.drawsize as any).value, 20,50); //180
            _drawdate = this.signature_tmp.getSignature(); // full name IMG Base64
            this.signature_tmp.clear();
            this.signature_tmp.refresh();

            imgDate.src = _drawdate;
        }

        imgFullname.src = _drawfullname;

        setTimeout(() => {

            this.trimCtxFullname = this.canvasFullname.nativeElement.getContext("2d", { willReadFrequently: true });
            this.trimCtxFullname.canvas.width = imgFullname.naturalWidth;
            this.trimCtxFullname.canvas.height = imgFullname.naturalHeight;
            this.trimCtxFullname.drawImage(imgFullname, 0, 0);
            this.trimFullname();

            if(this.popupFreeText!= ''){
                this.trimCtxDate = this.canvasDate.nativeElement.getContext("2d", { willReadFrequently: true });
                this.trimCtxDate.canvas.width = imgDate.naturalWidth;
                this.trimCtxDate.canvas.height = imgDate.naturalHeight;
                this.trimCtxDate.drawImage(imgDate, 0, 0);
                this.trimDate();
            }


            setTimeout(() => {
                let _max_w = this.trimCtxFullname.canvas.width;
                if(this.popupFreeText!= '') _max_w = (_max_w > this.trimCtxDate.canvas.width) ? _max_w:this.trimCtxDate.canvas.width;

                let _max_h = (this.trimCtxFullname.canvas.height + 4);
                if(this.popupFreeText!= '') _max_h = _max_h + (this.trimCtxDate.canvas.height) + 2;

                let _w_name_left = (_max_w - this.trimCtxFullname.canvas.width) / 2;
                let _w_date_left = 0;

                let imgFullnameTrim = new Image(this.trimCtxFullname.canvas.width,this.trimCtxFullname.canvas.height); //224
                imgFullnameTrim.src = this.trimCtxFullname.canvas.toDataURL();

                let imgDateTrim;
                if(this.popupFreeText!= '') {
                    _w_date_left = (_max_w - this.trimCtxDate.canvas.width) / 2;
                    imgDateTrim = new Image(this.trimCtxDate.canvas.width,this.trimCtxDate.canvas.height); //this.drawH
                    imgDateTrim.src = this.trimCtxDate.canvas.toDataURL();
                }

                this.ctxMixFullnameDate = this.mixCanvasFullname_date.nativeElement.getContext("2d", { willReadFrequently: true });
                this.ctxMixFullnameDate.canvas.width = _max_w;
                this.ctxMixFullnameDate.canvas.height = _max_h;

                setTimeout(() => {

                    this.ctxMixFullnameDate.drawImage(imgFullnameTrim, _w_name_left,0);
                    this.ctxMixFullnameDate.drawImage(imgFullnameTrim, _w_name_left,0);
                    if(this.popupFreeText!= '') {
                        this.ctxMixFullnameDate.drawImage(imgDateTrim, _w_date_left, this.trimCtxFullname.canvas.height + 4);
                        this.ctxMixFullnameDate.drawImage(imgDateTrim, _w_date_left, this.trimCtxFullname.canvas.height + 4);
                    }

                    let a = this.ctxMixFullnameDate.canvas.toDataURL();
                    let imgPreview = <HTMLImageElement>document.getElementById('_PreviewFullName_Date');
                    if(imgPreview) imgPreview.src = a;
                }, 200);
            }, 200);
        }, 200);
    }

    addDateToFreeText(_date) {
        let _date_old = this.join(this.popupDrawDate, '/');
        let _dateformat = '';

        if(_date) { _dateformat = this.join(_date, '/'); }

        if(_dateformat != '' && this.popupFreeText.indexOf(_date_old) != -1) { this.popupFreeText = this.popupFreeText.replace(_date_old,_dateformat); }
        else if(_dateformat != '' && this.popupFreeText.indexOf(_dateformat) != -1) { this.popupFreeText.replace(_dateformat,_dateformat); }
        else { this.popupFreeText = this.popupFreeText + _dateformat; }

        this.onDrawPreview();
    }

    join(date, separator) {
        let options = [{day: 'numeric'}, {month: 'short'}, {year: 'numeric'}];
        function format(option) {
           let formatter = new Intl.DateTimeFormat('en', option);
           return formatter.format(date);
        }
        return options.map(format).join(separator);
     }

    onClearDraw(){

        this.isDrawExists = false; // làm mới,
        this.isUploadExists = false;// làm mới,
        // this.applyFullnameToDrawSignature = false;
        this.signature.clear();
        this.signature.refresh();
        this.signature_tmp.clear();
        this.signature_tmp.refresh();

        // this.signature.backgroundColor = '#00000000'
        if(this.ctx)  {
            this.ctx.canvas.width = this.drawW;
            this.ctx.canvas.height = this.drawH;
            this.ctx.clearRect(0,0, this.ctx.canvas.width, this.ctx.canvas.height);
            this.ctx.beginPath();
            // this.ctx.reset();
        }

        if(this.ctxSignatureFinal)  {
            this.ctxSignatureFinal.canvas.width = this.drawW;
            this.ctxSignatureFinal.canvas.height = this.drawH;
            this.ctxSignatureFinal.clearRect(0,0, this.ctx.canvas.width, this.ctx.canvas.height);
            this.ctxSignatureFinal.beginPath();
            // this.ctx.reset();
        }
        if(this.trimCtxSignature) {
            this.trimCtxSignature.canvas.width = this.drawW;
            this.trimCtxSignature.canvas.height = this.drawH;
            this.trimCtxSignature.clearRect(0,0, this.drawW, this.drawH);
            // this.trimCtxSignature.beginPath();
        }
        // alert(this.signature.getSignature())
    }
    onMixText_Signature() {

        this.ctx = this.mixCanvasHtml.nativeElement.getContext("2d", { willReadFrequently: true });
        // this.ctx.clearRect(0,0, this.ctx.canvas.width, this.ctx.canvas.height);
        let imgSignature = new Image(this.drawW,this.drawH); //224

        // take only item draw signature, full name, date
        let _drawsignature = this.signature.getSignature();    // draw IMG Base64
        // this.signature.clear();
        // this.signature.refresh();

        imgSignature.src = _drawsignature;

        setTimeout(() => {

            this.trimCtxSignature = this.canvasSignature.nativeElement.getContext("2d", { willReadFrequently: true });
            this.trimCtxSignature.canvas.width = imgSignature.naturalWidth;
            this.trimCtxSignature.canvas.height = imgSignature.naturalHeight;
            this.trimCtxSignature.drawImage(imgSignature, 0, 0);

            this.trimSignature();

            setTimeout(() => {

                if(!this.isTrimSignature)  { //trim fail
                    this.notify.warn('please draw signature validate!');
                    return;
                }
               let _max_w = this.trimCtxSignature.canvas.width;
                   _max_w = (_max_w > this.ctxMixFullnameDate.canvas.width) ? _max_w:this.ctxMixFullnameDate.canvas.width;

                let _max_h = (this.trimCtxSignature.canvas.height + 4) +
                             (this.ctxMixFullnameDate.canvas.height);


                let _w_signature_left = (_max_w - this.trimCtxSignature.canvas.width) / 2;
                let _w_text_left = (_max_w - this.ctxMixFullnameDate.canvas.width) / 2;

                //mix fullname + date
                let imgSignatureTrim = new Image(this.trimCtxSignature.canvas.width,this.trimCtxSignature.canvas.height); //224
                let imgMixFullnameTrim = new Image(this.ctxMixFullnameDate.canvas.width,this.ctxMixFullnameDate.canvas.height); //224

                imgSignatureTrim.src = this.trimCtxSignature.canvas.toDataURL();
                imgMixFullnameTrim.src = this.ctxMixFullnameDate.canvas.toDataURL();
                this.SaveImageSignatureTrimBase64 = this.trimCtxSignature.canvas.toDataURL();


                this.ctxSignatureFinal = this.canvasSignatureFinal.nativeElement.getContext("2d", { willReadFrequently: true });
                this.ctxSignatureFinal.canvas.width = _max_w;
                this.ctxSignatureFinal.canvas.height = _max_h;

                // this.ctxMixFullnameDate = this.mixCanvasFullname_date.nativeElement.getContext("2d", { willReadFrequently: true });
                // this.ctxMixFullnameDate.canvas.width = _max_w;
                // this.ctxMixFullnameDate.canvas.height = _max_h;

                setTimeout(() => {

                    // tính toán chữ kí vượt quá khung
                    let h = this.trimCtxSignature.canvas.height + 4;
                    if(_max_h > this.drawH) {
                        let h_tmp = _max_h - this.drawH;
                        h = h - h_tmp;
                        this.ctxSignatureFinal.canvas.height = this.drawH;
                    }

                    this.ctxSignatureFinal.drawImage(imgSignatureTrim, _w_signature_left, 0);
                    this.ctxSignatureFinal.drawImage(imgSignatureTrim, _w_signature_left, 0);
                    this.ctxSignatureFinal.drawImage(imgMixFullnameTrim, _w_text_left, h);


                    let a = this.ctxSignatureFinal.canvas.toDataURL();
                    this.signature_tmp.load(a);



                    // add signature text to center frame

                    // let imgMixSignatureTrim = new Image(this.ctxMixFullnameDate.canvas.width,this.ctxMixFullnameDate.canvas.height);
                    // imgMixSignatureTrim.src = a;

                    setTimeout(() => {

                        let params = {
                            tabMode: this.tabMode,
                            isSignature: this.isSignature,
                            SignatureType: this.SignatureType,
                            SignatureImageBase64 : this.ctxSignatureFinal.canvas.toDataURL(),
                            SaveImageSignatureTrimBase64 : this.SaveImageSignatureTrimBase64,
                            isSaveUploadSignature: this.isSaveUploadSignature,
                        }
                        this.paramsEmit = params;
                        // if(this.hasSubmitBtn){
                        //     this.submitDocument.emit(params)
                        // }

                        this.SaveImageTemplateSignature();
                        if(this.hasSubmitBtn){
                            this.submitDocument.emit(params)
                        }
                        this.onApplyDrawText_Signature.emit(params);
                        this.ClosePopupSignature(false);    // không clear chữ kí
                        /*
                            this.objselectFieldSignature.signatureImage = this.ctxSignatureFinal.canvas.toDataURL();
                            this.SignatureImageBase64 = this.ctxSignatureFinal.canvas.toDataURL();


                            for(let i=0; this.listFieldSignatureForSigner[i]; i++){
                                this.listFieldSignatureForSigner[i].signatureImage = this.objselectFieldSignature.signatureImage;
                                this.listFieldSignatureForSigner[i].isSignature = true;
                            }

                            // let _field = document.querySelector('.fieldSignature-item.field_signature_id_' + this.objselectFieldSignature.id);
                            // if(_field) {
                            //     let _img = _field.querySelectorAll<HTMLElement>(".img_signature");
                            //     for(let i=0; _img[i]; i++) { _img[i].remove(); }
                            //     _field.innerHTML  += '<img class="img_signature img_signature_'+this.objselectFieldSignature.id+' img_signature" style="height: '+ (this.objselectFieldSignature.positionH - 2) +'px; top: 0px; position: absolute; z-index: 2; " src="'+this.objselectFieldSignature.signatureImage+'"/>';
                            // }
                            this.pendding_rendering_field_html(200);
                        */

                    }, 200);
                }, 300);
            }, 200);
        }, 300);
        // let jpegUrl = this.ctx.toDataURL("image/jpeg");
        // let pngUrl = this.ctx.toDataURL(); // base64 is the default
    }
    onDrawSignatureDefault() {
        this.ctx = this.mixCanvasHtml.nativeElement.getContext("2d", { willReadFrequently: true });
        // this.ctx.clearRect(0,0, this.ctx.canvas.width, this.ctx.canvas.height);
        let imgSignature = new Image(this.drawW,this.drawH); //224

        let _drawsignature = this.signature.getSignature();    // draw IMG Base64
        imgSignature.src = _drawsignature;

        setTimeout(() => {
            this.trimCtxSignature = this.canvasSignature.nativeElement.getContext("2d", { willReadFrequently: true });
            // this.trimCtxSignature.clearRect(0,0, this.trimCtxSignature.canvas.width, this.trimCtxSignature.canvas.height);

            this.trimCtxSignature.drawImage(imgSignature, 0, 0);
            this.trimSignature();

            let imgSignatureTrim = new Image(this.trimCtxSignature.canvas.width,this.trimCtxSignature.canvas.height); //224
            imgSignatureTrim.src = this.trimCtxSignature.canvas.toDataURL();

            setTimeout(() => {


                if(!this.isTrimSignature)  { //trim fail
                    this.notify.warn('please draw signature validate!');
                    return;
                }

                this.SignatureImageBase64 = this.trimCtxSignature.canvas.toDataURL();
                this.SaveImageSignatureTrimBase64 = this.trimCtxSignature.canvas.toDataURL();

                let params = {
                    tabMode: this.tabMode,
                    isSignature: this.isSignature,
                    SignatureType: this.SignatureType,
                    SignatureImageBase64 : this.SignatureImageBase64,
                    SaveImageSignatureTrimBase64 : this.SaveImageSignatureTrimBase64,
                    isSaveUploadSignature: this.isSaveUploadSignature,
                }
                this.paramsEmit = params;

                this.SaveImageTemplateSignature();
                this.onApplyDrawSignatureDefault.emit(params);
                if(this.hasSubmitBtn){
                    this.submitDocument.emit(params)
                }
                this.ClosePopupSignature(false);    // không clear chữ kí
/*

                ///done
                this.objselectFieldSignature.signatureImage = this.trimCtxSignature.canvas.toDataURL();
                this.SignatureImageBase64 = this.trimCtxSignature.canvas.toDataURL();
                this.SaveImageSignatureTrimBase64 = this.trimCtxSignature.canvas.toDataURL();
                ////
                for(let i=0; this.listFieldSignatureForSigner[i]; i++){
                    this.listFieldSignatureForSigner[i].signatureImage = this.objselectFieldSignature.signatureImage;
                    this.listFieldSignatureForSigner[i].isSignature = true;
                }

                let _field = document.querySelector('.fieldSignature-item.field_signature_id_' + this.objselectFieldSignature.id);
                if(_field) {
                    let _img = _field.querySelectorAll<HTMLElement>(".img_signature");
                    for(let i=0; _img[i]; i++) { _img[i].remove(); }
                    _field.innerHTML  += '<img class="img_signature img_signature_'+this.objselectFieldSignature.id+' img_signature" style="height: '+ (this.objselectFieldSignature.positionH - 2) +'px; top: 0px; position: absolute; z-index: 2; " src="'+this.objselectFieldSignature.signatureImage+'"/>';
                }
                this.pendding_rendering_field_html();
*/

            }, 200);
        }, 200);
    }

    isTrimSignature:boolean;
    trimSignature() {
        this.isTrimSignature = true;
        let a = <HTMLCanvasElement>document.getElementById('canvasSignature');
        let _context = a.getContext("2d", { willReadFrequently: true });
        const _imageData = _context.getImageData(0, 0, _context.canvas.width, _context.canvas.height);
        // const _imageData = _context.getImageData(0, 0, this.drawW, this.drawH);
        const { data, width, height } = _imageData;

        let top = 0;
        let bottom = height;
        let left = 0;
        let right = width;


        // Find the top edge
        for (let y = 0; y < height; y++) {
          for (let x = 0; x < width; x++) {
            const alpha = data[(y * width + x) * 4 + 3];
            if (alpha > 0) {
              top = y;
              break;
            }
          }
          if (top !== 0) {
            break;
          }
        }

        // Find the bottom edge
        for (let y = height - 1; y >= 0; y--) {
          for (let x = 0; x < width; x++) {
            const alpha = data[(y * width + x) * 4 + 3];
            if (alpha > 0) {
              bottom = y + 1;
              break;
            }
          }
          if (bottom < height) {
            break;
          }
        }


        // Find the left edge
        for (let x = 0; x < width; x++) {
          for (let y = 0; y < height; y++) {
            const alpha = data[(y * width + x) * 4 + 3];
            if (alpha > 0) {
              left = x;
              break;
            }
          }
          if (left !== 0) {
            break;
          }
        }

        // Find the right edge
        for (let x = width - 1; x >= 0; x--) {
          for (let y = 0; y < height; y++) {
            const alpha = data[(y * width + x) * 4 + 3];
            if (alpha > 0) {
              right = x + 1;
              break;
            }
          }
          if (right < width) {
            break;
          }
        }


        if(top ==0 && left == 0 && bottom == _context.canvas.height && right == _context.canvas.width) { this.isTrimSignature  = false; }

        const trimmedWidth = right - left;
        const trimmedHeight = bottom - top;
        const trimmedData = new Uint8ClampedArray(trimmedWidth * trimmedHeight * 4);

        for (let y = top; y < bottom; y++) {
          for (let x = left; x < right; x++) {
            const srcIndex = (y * width + x) * 4;
            const destIndex = ((y - top) * trimmedWidth + (x - left)) * 4;

            for (let i = 0; i < 4; i++) {
              trimmedData[destIndex + i] = data[srcIndex + i];
            }
          }
        }

        this.trimCtxSignature.canvas.width = trimmedWidth;
        this.trimCtxSignature.canvas.height = trimmedHeight;
        _context.putImageData(new ImageData(trimmedData, trimmedWidth, trimmedHeight), 0, 0);
    }

    trimFullname() {
        let a = <HTMLCanvasElement>document.getElementById('canvasFullname');
        let _context = a.getContext("2d", { willReadFrequently: true });

        // const _imageData = _context.getImageData(0, 0, a.width, a.height);
        const _imageData = _context.getImageData(0, 0, this.drawW, this.drawH);
        const { data, width, height } = _imageData;


        let top = 0;
        let bottom = height;
        let left = 0;
        let right = width;

        // Find the top edge
        for (let y = 0; y < height; y++) {
          for (let x = 0; x < width; x++) {
            const alpha = data[(y * width + x) * 4 + 3];
            if (alpha > 0) {
              top = y;
              break;
            }
          }
          if (top !== 0) {
            break;
          }
        }

        // Find the bottom edge
        for (let y = height - 1; y >= 0; y--) {
          for (let x = 0; x < width; x++) {
            const alpha = data[(y * width + x) * 4 + 3];
            if (alpha > 0) {
              bottom = y + 1;
              break;
            }
          }
          if (bottom < height) {
            break;
          }
        }

        // Find the left edge
        for (let x = 0; x < width; x++) {
          for (let y = 0; y < height; y++) {
            const alpha = data[(y * width + x) * 4 + 3];
            if (alpha > 0) {
              left = x;
              break;
            }
          }
          if (left !== 0) {
            break;
          }
        }

        // Find the right edge
        for (let x = width - 1; x >= 0; x--) {
          for (let y = 0; y < height; y++) {
            const alpha = data[(y * width + x) * 4 + 3];
            if (alpha > 0) {
              right = x + 1;
              break;
            }
          }
          if (right < width) {
            break;
          }
        }

        const trimmedWidth = right - left;
        const trimmedHeight = bottom - top;
        const trimmedData = new Uint8ClampedArray(trimmedWidth * trimmedHeight * 4);

        for (let y = top; y < bottom; y++) {
          for (let x = left; x < right; x++) {
            const srcIndex = (y * width + x) * 4;
            const destIndex = ((y - top) * trimmedWidth + (x - left)) * 4;

            for (let i = 0; i < 4; i++) {
              trimmedData[destIndex + i] = data[srcIndex + i];
            }
          }
        }

        this.trimCtxFullname.canvas.width = trimmedWidth;
        this.trimCtxFullname.canvas.height = trimmedHeight;
        _context.putImageData(new ImageData(trimmedData, trimmedWidth, trimmedHeight), 0, 0);
    }

    trimDate() {
        let a = <HTMLCanvasElement>document.getElementById('canvasDate');
        let _context = a.getContext("2d", { willReadFrequently: true });
        // const _imageData = _context.getImageData(0, 0, _context.canvas.width, _context.canvas.height);
        // const _imageData = _context.getImageData(0, 0, a.width, a.height);
        const _imageData = _context.getImageData(0, 0, this.drawW, this.drawH);
        const { data, width, height } = _imageData;

        let top = 0;
        let bottom = height;
        let left = 0;
        let right = width;

        // Find the top edge
        for (let y = 0; y < height; y++) {
          for (let x = 0; x < width; x++) {
            const alpha = data[(y * width + x) * 4 + 3];
            if (alpha > 0) {
              top = y;
              break;
            }
          }
          if (top !== 0) {
            break;
          }
        }

        // Find the bottom edge
        for (let y = height - 1; y >= 0; y--) {
          for (let x = 0; x < width; x++) {
            const alpha = data[(y * width + x) * 4 + 3];
            if (alpha > 0) {
              bottom = y + 1;
              break;
            }
          }
          if (bottom < height) {
            break;
          }
        }

        // Find the left edge
        for (let x = 0; x < width; x++) {
          for (let y = 0; y < height; y++) {
            const alpha = data[(y * width + x) * 4 + 3];
            if (alpha > 0) {
              left = x;
              break;
            }
          }
          if (left !== 0) {
            break;
          }
        }

        // Find the right edge
        for (let x = width - 1; x >= 0; x--) {
          for (let y = 0; y < height; y++) {
            const alpha = data[(y * width + x) * 4 + 3];
            if (alpha > 0) {
              right = x + 1;
              break;
            }
          }
          if (right < width) {
            break;
          }
        }

        const trimmedWidth = right - left;
        const trimmedHeight = bottom - top;
        const trimmedData = new Uint8ClampedArray(trimmedWidth * trimmedHeight * 4);

        for (let y = top; y < bottom; y++) {
          for (let x = left; x < right; x++) {
            const srcIndex = (y * width + x) * 4;
            const destIndex = ((y - top) * trimmedWidth + (x - left)) * 4;

            for (let i = 0; i < 4; i++) {
              trimmedData[destIndex + i] = data[srcIndex + i];
            }
          }
        }

        this.trimCtxDate.canvas.width = trimmedWidth;
        this.trimCtxDate.canvas.height = trimmedHeight;
        _context.putImageData(new ImageData(trimmedData, trimmedWidth, trimmedHeight), 0, 0);
    }

    isTrimUploadImage:boolean;
    trimUploadImage() {
        this.isTrimUploadImage = true;
        let a = <HTMLCanvasElement>document.getElementById('canvasUploadImage');
        let _context = a.getContext("2d", { willReadFrequently: true });

        const _imageData = _context.getImageData(0, 0, _context.canvas.width, _context.canvas.height);
        const { data, width, height } = _imageData;

        let top = 0;
        let bottom = height;
        let left = 0;
        let right = width;

        // Find the top edge
        for (let y = 0; y < height; y++) {
          for (let x = 0; x < width; x++) {
            const alpha = data[(y * width + x) * 4 + 3];
            if (alpha > 0) {
              top = y;
              break;
            }
          }
          if (top !== 0) {
            break;
          }
        }

        // Find the bottom edge
        for (let y = height - 1; y >= 0; y--) {
          for (let x = 0; x < width; x++) {
            const alpha = data[(y * width + x) * 4 + 3];
            if (alpha > 0) {
              bottom = y + 1;
              break;
            }
          }
          if (bottom < height) {
            break;
          }
        }

        // Find the left edge
        for (let x = 0; x < width; x++) {
          for (let y = 0; y < height; y++) {
            const alpha = data[(y * width + x) * 4 + 3];
            if (alpha > 0) {
              left = x;
              break;
            }
          }
          if (left !== 0) {
            break;
          }
        }

        // Find the right edge
        for (let x = width - 1; x >= 0; x--) {
          for (let y = 0; y < height; y++) {
            const alpha = data[(y * width + x) * 4 + 3];
            if (alpha > 0) {
              right = x + 1;
              break;
            }
          }
          if (right < width) {
            break;
          }
        }

        if(top ==0 && left == 0 && bottom == _context.canvas.height && right == _context.canvas.width) { this.isTrimUploadImage  = false; }

        const trimmedWidth = right - left;
        const trimmedHeight = bottom - top;
        const trimmedData = new Uint8ClampedArray(trimmedWidth * trimmedHeight * 4);

        for (let y = top; y < bottom; y++) {
          for (let x = left; x < right; x++) {
            const srcIndex = (y * width + x) * 4;
            const destIndex = ((y - top) * trimmedWidth + (x - left)) * 4;

            for (let i = 0; i < 4; i++) {
              trimmedData[destIndex + i] = data[srcIndex + i];
            }
          }
        }

        this.trimCtxUploadImage.canvas.width = trimmedWidth;
        this.trimCtxUploadImage.canvas.height = trimmedHeight;
        _context.putImageData(new ImageData(trimmedData, trimmedWidth, trimmedHeight), 0, 0);
    }

    trimImage() {
        let a = <HTMLCanvasElement>document.getElementById('mixCanvasHtml');
        let _context = a.getContext("2d", { willReadFrequently: true });
        const _imageData = _context.getImageData(0, 0, this.drawW, this.drawH);
        const { data, width, height } = _imageData;

        let top = 0;
        let bottom = height;
        let left = 0;
        let right = width;

        // Find the top edge
        for (let y = 0; y < height; y++) {
          for (let x = 0; x < width; x++) {
            const alpha = data[(y * width + x) * 4 + 3];
            if (alpha > 0) {
              top = y;
              break;
            }
          }
          if (top !== 0) {
            break;
          }
        }

        // Find the bottom edge
        for (let y = height - 1; y >= 0; y--) {
          for (let x = 0; x < width; x++) {
            const alpha = data[(y * width + x) * 4 + 3];
            if (alpha > 0) {
              bottom = y + 1;
              break;
            }
          }
          if (bottom < height) {
            break;
          }
        }

        // Find the left edge
        for (let x = 0; x < width; x++) {
          for (let y = 0; y < height; y++) {
            const alpha = data[(y * width + x) * 4 + 3];
            if (alpha > 0) {
              left = x;
              break;
            }
          }
          if (left !== 0) {
            break;
          }
        }

        // Find the right edge
        for (let x = width - 1; x >= 0; x--) {
          for (let y = 0; y < height; y++) {
            const alpha = data[(y * width + x) * 4 + 3];
            if (alpha > 0) {
              right = x + 1;
              break;
            }
          }
          if (right < width) {
            break;
          }
        }

        const trimmedWidth = right - left;
        const trimmedHeight = bottom - top;
        const trimmedData = new Uint8ClampedArray(trimmedWidth * trimmedHeight * 4);

        for (let y = top; y < bottom; y++) {
          for (let x = left; x < right; x++) {
            const srcIndex = (y * width + x) * 4;
            const destIndex = ((y - top) * trimmedWidth + (x - left)) * 4;

            for (let i = 0; i < 4; i++) {
              trimmedData[destIndex + i] = data[srcIndex + i];
            }
          }
        }

        this.ctx.canvas.width = trimmedWidth;
        this.ctx.canvas.height = trimmedHeight;
        _context.putImageData(new ImageData(trimmedData, trimmedWidth, trimmedHeight), 0, 0);
    }

    cropImage(x, y, _width, _height){

        // X-coordinate of the top-left corner of the crop area
        // Y-coordinate of the top-left corner of the crop area
        // Width of the crop area
        // Height of the crop area

        const croppedData = this.ctx.getImageData(x, y, _width, _height);

        // Create a new canvas for the cropped image
        const croppedCanvas = document.createElement('canvas');
        croppedCanvas.width = _width;
        croppedCanvas.height = _height;
        const croppedCtx = croppedCanvas.getContext("2d", { willReadFrequently: true });
        croppedCtx.putImageData(croppedData, 0, 0);
        // Convert the cropped canvas to a base64 data URL
        // let croppedCanvasBase64 = croppedCanvas.toDataURL(); //'image/png'

        this.ctx.beginPath();
        this.ctx.canvas.width = _width;
        this.ctx.canvas.height = _height;
        this.ctx.clearRect(0, 0, _width,_height);
        this.ctx.putImageData(croppedData, 0, 0);
        // let imgCrop = new Image(_width,_height);
        // imgCrop.src = croppedCanvasBase64;
        // this.ctx.drawImage(imgCrop, 0, 0);
    }

    DeleteTemplateSignature(){
        this.isLoading = true;
        this._mstEsignUserImage.deleteTemplateImageSignature(this.SignerId, this._selectTemPlateSigature).subscribe(e=> {
            //lấy list signature default
            this._mstEsignUserImage.getListSignatureByUserIdForWeb().subscribe(res =>{
                if(res) {
                    this.listTemplateSignature = res.items;
                    this.signature_empty = (4 - this.listTemplateSignature.length);

                    let _default = this.listTemplateSignature.find(e => e.isUse == true);
                    if(_default) this._selectTemPlateSigature = _default.id;

                    this.isLoading = false;
                }
            });
        });
    }




    objSaveSignature: MstEsignUserImageSignatureInput;
    SaveImageTemplateSignature() {
        // //lưu chữ kí
        if(this.isSaveUploadSignature) {

            if(this.tabMode == "TEMPLATE") {

            }
            else if(this.tabMode == "DRAW") {

                // this.SignatureImageBase64
                let img = <HTMLImageElement>document.getElementById('_SaveImageSignature');

                if(img && this.SaveImageSignatureTrimBase64 != ''){
                    this.isLoading = true;
                    img.src = this.SaveImageSignatureTrimBase64;
                    setTimeout(() => {
                        this.objSaveSignature = new MstEsignUserImageSignatureInput();
                        this.objSaveSignature.signerId = this.SignerId;
                        this.objSaveSignature.imgWidth = img.naturalWidth;
                        this.objSaveSignature.imgHeight = img.naturalHeight;
                        this.objSaveSignature.imgSize = 0;
                        this.objSaveSignature.imageSignature = this.SaveImageSignatureTrimBase64.replace('data:image/png;base64,','');

                        this._mstEsignUserImage.saveImageSignature(this.objSaveSignature).subscribe(e => {

                            //lấy list signature default
                            this._mstEsignUserImage.getListSignatureByUserIdForWeb().subscribe(res =>{
                                if(res) {
                                    this.listTemplateSignature = res.items;

                                    this.signature_empty = (4 - this.listTemplateSignature.length);

                                    let _default = this.listTemplateSignature.find(e => e.isUse == true);
                                    if(_default) this._selectTemPlateSigature = _default.id;

                                    this.isLoading = false;
                                } else {
                                    this.isLoading = false;
                                }
                            });
                        });
                    }, 200);

                }
            }
            else if(this.tabMode == "UPLOAD") {
                let img = <HTMLImageElement>document.getElementById('_SaveImageSignature');
                if(img && this.SaveImageSignatureTrimBase64 != ''){
                    this.isLoading = true;
                    img.src = this.SaveImageSignatureTrimBase64;
                    setTimeout(() => {
                        this.objSaveSignature = new MstEsignUserImageSignatureInput();
                        this.objSaveSignature.signerId = this.SignerId;
                        this.objSaveSignature.imgWidth = img.naturalWidth;
                        this.objSaveSignature.imgHeight = img.naturalHeight;
                        this.objSaveSignature.imgSize = 0;
                        this.objSaveSignature.imageSignature = this.SaveImageSignatureTrimBase64.replace('data:image/png;base64,','');

                        this._mstEsignUserImage.saveImageSignature(this.objSaveSignature).subscribe(e => {

                            //lấy list signature default
                            this._mstEsignUserImage.getListSignatureByUserIdForWeb().subscribe(res =>{
                                if(res) {
                                    this.listTemplateSignature = res.items;

                                    this.signature_empty = (4 - this.listTemplateSignature.length);

                                    let _default = this.listTemplateSignature.find(e => e.isUse == true);
                                    if(_default) this._selectTemPlateSigature = _default.id;

                                    this.isLoading = false;
                                }else {
                                    this.isLoading = false;
                                }
                            });
                        });
                    }, 200);
                }
            }

        }
    }





    public onSelect(args: SelectedEventArgs): void {
        if (!this.dropElement && !this.dropElement.querySelector('li')) { this.filesDetails = []; }
        if (isNullOrUndefined(document.getElementById('dropArea').querySelector('.e-upload-files'))) {
            this.parentElement = createElement('ul', { className: 'e-upload-files' });
            document.getElementsByClassName('e-upload')[0].appendChild(this.parentElement);
        }
        let validFiles: FileInfo[] = this.validateFiles(args, this.filesDetails);
        if (validFiles.length === 0) {
            args.cancel = true;
            return;
        }
        for (let i : number = 0; i < validFiles.length; i++) {
            this.formSelectedData(validFiles[i], this);
        }
        this.filesDetails = this.filesDetails.concat(validFiles);
        args.cancel = true;
    }

    public validateFiles(args: any, viewedFiles: FileInfo[]): FileInfo[] {
        let modifiedFiles: FileInfo[] = [];
        let validFiles: FileInfo[] = [];
        let isModified: boolean = false;
        if (args.event.type === 'drop') {
            isModified = true;
            let allImages: string[] = ['png', 'jpg', 'jpeg'];
            let files: FileInfo[] = args.filesData;
            for (let file of files) {
                if (allImages.indexOf(file.type) !== -1) {
                    modifiedFiles.push(file);
                }
            }
        }
        let files: FileInfo[] = modifiedFiles.length > 0 || isModified ? modifiedFiles : args.filesData;
        if (this.filesName.length > 0) {
            for (let file of files) {
                if (this.filesName.indexOf(file.name) === -1) {
                    this.filesName.push(file.name);
                    validFiles.push(file);
                }
            }
        } else {
            for (let file of files) {
                this.filesName.push(file.name);
                validFiles.push(file);
            }
        }
        return validFiles;
    }

    public formSelectedData (file : FileInfo, proxy: any): void {
        let liEle : HTMLElement = createElement('li',  {className: 'e-upload-file-list', attrs: {'data-file-name': file.name}});
        let imageTag: HTMLImageElement = <HTMLImageElement>createElement('IMG',  {className: 'upload-image', attrs: {'alt': 'Image'}});
        let wrapper: HTMLElement = createElement('span', {className: 'wrapper'});
        wrapper.appendChild(imageTag); liEle.appendChild(wrapper);
        // liEle.appendChild(createElement('div', {className: 'name file-name', innerHTML: file.name, attrs: {'title': file.name}}));
        // liEle.appendChild(createElement('div', {className: 'file-size', innerHTML: proxy.uploadObj.bytesToSize(file.size) }));
        // let clearbtn: HTMLElement;
        // let uploadbtn: HTMLElement;
        // clearbtn = createElement('span', {id: 'removeIcon', className: 'e-icons e-file-remove-btn', attrs: {'title': 'Remove'}});
        // EventHandler.add(clearbtn, 'click', this.removeFiles, proxy);
        liEle.setAttribute('title', 'Ready to Upload');
        // uploadbtn = createElement('span', {className: 'e-upload-icon e-icons e-file-remove-btn', attrs: {'title': 'Upload'}});
        // uploadbtn.setAttribute('id', 'iconUpload'); EventHandler.add(uploadbtn, 'click', this.uploadFile, proxy);
        // let progressbarContainer: HTMLElement;
        // progressbarContainer = createElement('progress', {className: 'progressbar', id: 'progressBar', attrs: {value: '0', max: '100'}});
        // liEle.appendChild(clearbtn); liEle.appendChild(uploadbtn);
        // liEle.appendChild(progressbarContainer);
        this.readURL(liEle, file); document.querySelector('.e-upload-files').appendChild(liEle);
        proxy.filesList.push(liEle);
    }
    public uploadFile(args: any): void {
        this.uploadObj.upload([this.filesDetails[this.filesList.indexOf(args.currentTarget.parentElement)]], true);
    }
    public removeFiles(args: any): void {
        let removeFile: FileInfo = this.filesDetails[this.filesList.indexOf(args.currentTarget.parentElement)];
        let statusCode: string = removeFile.statusCode;
        if (statusCode === '2' || statusCode === '1') {
            this.uploadObj.remove(removeFile, true);
            this.uploadObj.element.value = '';
        }
        let index: number = this.filesList.indexOf(args.currentTarget.parentElement);
        this.filesList.splice(index, 1);
        this.filesDetails.splice(index, 1);
        this.filesName.splice(this.filesName.indexOf(removeFile.name), 1);
        if (statusCode !== '2') { detach(args.currentTarget.parentElement); }
    }
    public onFileUpload(args : any) : void {
        let li : Element = document.getElementById('dropArea').querySelector('[data-file-name="' + args.file.name + '"]');
        let iconEle: HTMLElement = li.querySelector('#iconUpload') as HTMLElement;
        iconEle.style.cursor = 'not-allowed';
        iconEle.classList.add('e-uploaded');
        EventHandler.remove(li.querySelector('#iconUpload'), 'click', this.uploadFile);
        // let progressValue : number = Math.round((args.e.loaded / args.e.total) * 100);
        // if (!isNaN(progressValue) && li.querySelector('.progressbar')) {
        //     li.getElementsByTagName('progress')[0].value = progressValue;
        // }
    }
    public onUploadSuccess(args : any) : void {
        let spinnerElement: HTMLElement = document.getElementById('dropArea');
        let li : HTMLElement = document.getElementById('dropArea').querySelector('[data-file-name="' + args.file.name + '"]');
        // if (li && !isNullOrUndefined(li.querySelector('.progressbar'))) {
        //     (li.querySelector('.progressbar') as HTMLElement).style.visibility = 'hidden';
        // }
        if (args.operation === 'upload') {
            EventHandler.remove(li.querySelector('#iconUpload'), 'click', this.uploadFile);
            li.setAttribute('title', args.e.currentTarget.statusText);
            (li.querySelector('.file-name') as HTMLElement).style.color = 'green';
            (li.querySelector('.e-icons') as HTMLElement).onclick = () => {
                this.generateSpinner(this.dropElement.querySelector('#dropArea'));
            };
        } else {
            if (!isNullOrUndefined(li)) {
                detach(li);
            }
            if (!isNullOrUndefined(spinnerElement)) {
                hideSpinner(spinnerElement);
                detach(spinnerElement.querySelector('.e-spinner-pane'));
            }
        }
        li.querySelector('#removeIcon').removeAttribute('.e-file-remove-btn');
        li.querySelector('#removeIcon').setAttribute('class', 'e-icons e-file-delete-btn');
    }
    public generateSpinner(targetElement: HTMLElement): void {
        createSpinner({ target: targetElement, width: '25px' });
        showSpinner(targetElement);
    }
    public onUploadFailed(args : any) : void {
        let li : Element = document.getElementById('dropArea').querySelector('[data-file-name="' + args.file.name + '"]');
        (li.querySelector('.file-name') as HTMLElement).style.color = 'red';
        li.setAttribute('title', args.e.currentTarget.statusText);
        if (args.operation === 'upload') {
            EventHandler.remove(li.querySelector('#iconUpload'), 'click', this.uploadFile);
            // (li.querySelector('.progressbar') as HTMLElement).style.visibility = 'hidden';
        }
    }
    public readURL(li: HTMLElement, args: any): void {
        let preview: HTMLImageElement = li.querySelector('.upload-image');
        let file: File = args.rawFile; let reader: FileReader = new FileReader();
        reader.addEventListener('load', () => {
            preview.src = reader.result as string;
            // this.UploadImageBase64 =  reader.result as string;

            this.trimCtxUploadImage = this.canvasUploadImage.nativeElement.getContext("2d", { willReadFrequently: true });


            setTimeout(() => {
                this.trimCtxUploadImage.canvas.width = preview.naturalWidth;
                this.trimCtxUploadImage.canvas.height = preview.naturalHeight;
                this.trimCtxUploadImage.drawImage(preview, 0, 0);

                this.trimUploadImage();

                if(!this.isTrimUploadImage) { } //trim fail

                this.UploadImageBase64 = this.trimCtxUploadImage.canvas.toDataURL();
                preview.src = this.UploadImageBase64;


            }, 400);

        }, false);
        if (file) { reader.readAsDataURL(file); }
    }

    public onFileRemove(args: RemovingEventArgs): void {
        // args.postRawFile = false;
    }

    clearUploadImageSignature() {
        if (!this.dropElement.querySelector('ul')) { return; }
        detach(this.dropElement.querySelector('ul'));
        this.filesList = [];
        this.filesDetails = [];
        this.filesName = [];
        this.UploadImageBase64 = '';
        this.uploadObj.element.value = '';

        if (this.dropElement.querySelector('#dropArea').classList.contains('e-spinner-pane')) {
            hideSpinner(this.dropElement.querySelector('#dropArea'));
            detach(this.dropElement.querySelector('.e-spinner-pane'));
        }
    }


    colorState(_state){
        let obj = document.querySelector('.frame-1171275436 .color-item.active');
        if(obj) obj.classList.remove('active');

        let colorSelect = document.querySelector('.frame-1171275436 .color-item.colorState' + _state);
        if(colorSelect) colorSelect.classList.add('active');

        if(_state == 1) {this.colorDraw = "#1E1E1E"; }
        else if(_state == 2) {this.colorDraw = "#0057E3"; }
        else if(_state == 3) {this.colorDraw = "#00AE23"; }
        else if(_state == 4) {this.colorDraw = "#F83B3B"; }

        this.signature.strokeColor = this.colorDraw;
    }
    checkedSaveSignature(params){
        this.isSaveUploadSignature = params;
    }


}
