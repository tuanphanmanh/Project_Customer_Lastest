import { FileParameter, MstEsignLogoServiceProxy } from './../../../../../shared/service-proxies/service-proxies';
import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef } from '@angular/core';
import { AppComponentBase } from '@shared/common/app-component-base';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs';
import { FormBuilder, FormGroup } from '@angular/forms';
import { GlobalValidator } from '@shared/utils/validators';
import { FileInfo, RemovingEventArgs, SelectedEventArgs, UploaderComponent } from '@syncfusion/ej2-angular-inputs';
import { EventHandler, createElement, detach } from '@syncfusion/ej2-base';
import { MstEsignLogoDto } from './../../../../../shared/service-proxies/service-proxies';
import { AppConsts } from '@shared/AppConsts';
import { HttpClient, HttpHeaders } from '@angular/common/http';
@Component({
    selector: 'create-or-edit-logo-modal',
    templateUrl: './create-or-edit-logo-modal.component.html'
})
export class CreateOrEditLogoComponent extends AppComponentBase implements OnInit {
    @ViewChild('createOrEditLogo', { static: true }) modal: ModalDirective | undefined;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();
    @Output() modalClose: EventEmitter<any> = new EventEmitter<any>();
    @ViewChild("submitBtn", { static: false }) submitBtn: ElementRef;
    @ViewChild('previewuploadMin') public uploadObjMin: UploaderComponent;
    @ViewChild('previewuploadMax') public uploadObjMax: UploaderComponent;
    public fileDetailMin: FileInfo;
    public fileDetailMax: FileInfo;
    public parentElement: HTMLElement;
    public dropElementMin: HTMLElement;
    public dropElementMax: HTMLElement;
    public fileName: string;
    public allowExtensions: string = '.png, .jpg, .jpeg, .svg';
    createOrEditForm: FormGroup;
    isSubmit = false;
    listTenant;
    dialogCloseIcon = true;
    dialogWidth: string = '50%';
    isModal: Boolean = true;
    showCloseIcon: Boolean = false;
    visible: Boolean = false;
    animationSettings: Object = { effect: 'FadeZoom' };
    dialogHeader = this.l('CreateLogo');
    rowdata: MstEsignLogoDto = new MstEsignLogoDto();
    saving: boolean = false;
    config = {
        headers: {
            'Content-Type': 'multipart/form-data'
        }
    }
    imgMin: FileParameter = { data: undefined, fileName: undefined };
    imgMax: FileParameter = { data: undefined, fileName: undefined };
    constructor(
        injector: Injector,
        private _service: MstEsignLogoServiceProxy,
        private formBuilder: FormBuilder,

    ) {
        super(injector);
    }

    ngOnInit(): void {
        this._service.getAllTenants().subscribe(result => {
            this.listTenant = result;
        });
    }

    buildForm() {
        this.createOrEditForm = this.formBuilder.group({
            id: [0],
            tenantId: [this.rowdata.tenantId ?? 0, GlobalValidator.required],
        });
    }

    showModalCreateEdit(rowdata?: any): void {
        if (!rowdata) {
            this.dialogHeader = this.l('CreateLogo');
            this.rowdata = new MstEsignLogoDto();
        } else {
            this.rowdata = rowdata;
            this.dialogHeader = this.l('UpdateLogo')
        }
        this.buildForm();
        this.modal.show();
    }

    hideModal() {
        this.createOrEditForm = undefined;
        this.modal.hide();
    }

    save() {
        this.isSubmit = true;
        if (this.submitBtn) {
            this.submitBtn.nativeElement.click();
        }
        if (this.createOrEditForm.invalid) {
            return;
        }
        this.saving = true;
        if (this.fileDetailMin) {
            this.imgMin.data = this.fileDetailMin.rawFile;
            this.imgMin.fileName = this.fileDetailMin.name;
        }
        if(this.fileDetailMax){
            this.imgMax.data = this.fileDetailMax.rawFile;
            this.imgMax.fileName = this.fileDetailMax.name;
        }
        this.createOrEdit(
            this.rowdata.tenantId,
            this.imgMin,
            this.imgMax,
            this.rowdata.id ?? 0,
        )
            .pipe(finalize(() => this.saving = false))
            .subscribe(() => {
                this.notify.info(this.l('SavedSuccessfully'));
                this.hideModal();
                this.modalSave.emit(this.rowdata);
            });
        this.saving = false;
    }

    onCloseDialog() {
        this.createOrEditForm = undefined;
    }

    public onFileUploadMin(args: any): void {
        let li: Element = document.getElementById('dropAreaMin').querySelector('[data-file-name="' + args.file.name + '"]');
        let iconEle: HTMLElement = li.querySelector('#iconUploadMin') as HTMLElement;
        iconEle.style.cursor = 'not-allowed';
        iconEle.classList.add('e-uploaded');
        EventHandler.remove(li.querySelector('#iconUploadMin'), 'click', this.uploadFileMin);
    }


    public onFileUploadMax(args: any): void {
        let li: Element = document.getElementById('dropAreaMax').querySelector('[data-file-name="' + args.file.name + '"]');
        let iconEle: HTMLElement = li.querySelector('#iconUploadMax') as HTMLElement;
        iconEle.style.cursor = 'not-allowed';
        iconEle.classList.add('e-uploaded');
        EventHandler.remove(li.querySelector('#iconUploadMax'), 'click', this.uploadFileMax);
    }

    public uploadFileMin(args: any): void {
        this.uploadObjMin.upload(this.fileDetailMin, true);
    }

    public uploadFileMax(args: any): void {
        this.uploadObjMax.upload(this.fileDetailMax, true);
    }

    public onUploadFailedMin(args: any): void {
        let li: Element = document.getElementById('dropAreaMin').querySelector('[data-file-name="' + args.file.name + '"]');
        (li.querySelector('.file-name') as HTMLElement).style.color = 'red';
        li.setAttribute('title', args.e.currentTarget.statusText)
        if (args.operation === 'upload') {
            EventHandler.remove(li.querySelector('#iconUploadMin'), 'click', this.uploadFileMin);
        }
    }

    public onUploadFailedMax(args: any): void {
        let li: Element = document.getElementById('dropAreaMax').querySelector('[data-file-name="' + args.file.name + '"]');
        (li.querySelector('.file-name') as HTMLElement).style.color = 'red';
        li.setAttribute('title', args.e.currentTarget.statusText)
        if (args.operation === 'upload') {
            EventHandler.remove(li.querySelector('#iconUploadMax'), 'click', this.uploadFileMax);
        }
    }

    public onFileMinRemove(args: RemovingEventArgs): void {
        args.postRawFile = false;
    }

    public onFileMaxRemove(args: RemovingEventArgs): void {
        args.postRawFile = false;
    }

    public onSelectMin(args: SelectedEventArgs): void {
        if (!this.dropElementMin?.querySelector('li')) { this.fileDetailMin = null }
        if (document.getElementById('dropAreaMin')?.querySelector('.e-upload-min') == null) {
            this.parentElement = createElement('ul', { className: 'e-upload-min' });
            document.getElementsByClassName('e-upload')[0].appendChild(this.parentElement);
        }
        let validFiles: FileInfo = this.validateFiles(args);
        this.formSelectedMinData(validFiles, this);
        this.fileDetailMin = validFiles;
        args.cancel = true;
    }

    public onSelectMax(args: SelectedEventArgs): void {
        if (!this.dropElementMin?.querySelector('li')) { this.fileDetailMax = null }
        if (document.getElementById('dropAreaMax')?.querySelector('.e-upload-max') == null) {
            this.parentElement = createElement('ul', { className: 'e-upload-max' });
            document.getElementsByClassName('e-upload')[1].appendChild(this.parentElement);
        }
        let validFiles: FileInfo = this.validateFiles(args);
        this.formSelectedMaxData(validFiles, this);
        this.fileDetailMin = validFiles;
        args.cancel = true;
    }

    public validateFiles(args: any): FileInfo {
        let modifiedFile: FileInfo;
        let isModified: boolean = false;
        if (args.event.type === 'drop') {
            isModified = true;
            let allImages: string[] = ['png', 'jpg', 'jpeg', 'svg'];
            let file: FileInfo = args.filesData[0];
            if (allImages.indexOf(file.type) !== -1) {
                modifiedFile = file;
            }
        }
        let file: FileInfo = modifiedFile || isModified ? modifiedFile : args.filesData[0];
        this.fileName = file.name;
        return file;
    }

    public formSelectedMinData(file: FileInfo, proxy: any): void {
        let liEle: HTMLElement = createElement('li', { className: 'e-upload-file-min', attrs: { 'data-file-name': file.name } });
        let imageTag: HTMLImageElement = <HTMLImageElement>createElement('IMG', { className: 'upload-image-min', styles: 'height: 100px;margin: 0 10px;border-radius: 10px;box-shadow: rgba(0, 0, 0, 0.24) 0px 3px 8px;', attrs: { 'alt': 'Image' } });
        liEle.appendChild(imageTag);
        liEle.appendChild(createElement('div', { className: 'name file-name', styles: 'padding: 10px 10px 0px 10px;', innerHTML: 'File name: ' + file.name, attrs: { 'title': file.name } }));
        liEle.appendChild(createElement('div', { className: 'file-size', styles: 'padding: 10px 10px 0px 10px;', innerHTML: 'File size: ' + proxy.uploadObjMin.bytesToSize(file.size) }));
        let clearbtn: HTMLElement;
        clearbtn = createElement('span', { id: 'removeIcon', className: 'e-icons e-file-remove-btn', attrs: { 'title': 'Remove' } });
        EventHandler.add(clearbtn, 'click', this.removeMinFiles, proxy);
        liEle.appendChild(clearbtn);
        liEle.setAttribute('title', 'Ready to Upload');
        this.readMinURL(liEle, file); document.querySelector('.e-upload-min').appendChild(liEle);
    }

    public formSelectedMaxData(file: FileInfo, proxy: any): void {
        let liEle: HTMLElement = createElement('li', { className: 'e-upload-file-max', attrs: { 'data-file-name': file.name } });
        let imageTag: HTMLImageElement = <HTMLImageElement>createElement('IMG', { className: 'upload-image-max', styles: 'height: 100px;margin: 0 10px;border-radius: 10px;box-shadow: rgba(0, 0, 0, 0.24) 0px 3px 8px;', attrs: { 'alt': 'Image' } });
        liEle.appendChild(imageTag);
        liEle.appendChild(createElement('div', { className: 'name file-name', styles: 'padding: 10px 10px 0px 10px;', innerHTML: 'File name: ' + file.name, attrs: { 'title': file.name } }));
        liEle.appendChild(createElement('div', { className: 'file-size', styles: 'padding: 10px 10px 0px 10px;', innerHTML: 'File size: ' + proxy.uploadObjMax.bytesToSize(file.size) }));
        let clearbtn: HTMLElement;
        clearbtn = createElement('span', { id: 'removeIcon', className: 'e-icons e-file-remove-btn', attrs: { 'title': 'Remove' } });
        EventHandler.add(clearbtn, 'click', this.removeMaxFiles, proxy);
        liEle.appendChild(clearbtn);
        liEle.setAttribute('title', 'Ready to Upload');
        this.readMaxURL(liEle, file); document.querySelector('.e-upload-max').appendChild(liEle);
    }

    public removeMinFiles(args: any): void {
        let removeFile: FileInfo = this.fileDetailMin;
        let statusCode: string = removeFile.statusCode;
        if (statusCode === '2' || statusCode === '1') {
            this.uploadObjMin.remove(removeFile, true);
            this.uploadObjMin.element.value = '';
        }
        this.fileDetailMin = null;
        this.fileName = null;
        if (statusCode !== '2') { detach(args.currentTarget.parentElement); }
    }

    public removeMaxFiles(args: any): void {
        let removeFile: FileInfo = this.fileDetailMax;
        let statusCode: string = removeFile.statusCode;
        if (statusCode === '2' || statusCode === '1') {
            this.uploadObjMax.remove(removeFile, true);
            this.uploadObjMax.element.value = '';
        }
        this.fileDetailMax = null;
        this.fileName = null;
        if (statusCode !== '2') { detach(args.currentTarget.parentElement); }
    }

    public readMinURL(li: HTMLElement, args: any): void {
        let preview: HTMLImageElement = li.querySelector('.upload-image-min');
        let file: File = args.rawFile; let reader: FileReader = new FileReader();
        reader.addEventListener('load', () => { preview.src = reader.result as string; }, false);
        if (file) { reader.readAsDataURL(file); }
    }

    public readMaxURL(li: HTMLElement, args: any): void {
        let preview: HTMLImageElement = li.querySelector('.upload-image-max');
        let file: File = args.rawFile;
        this.fileDetailMax = args;
        let reader: FileReader = new FileReader();
        reader.addEventListener('load', () => { preview.src = reader.result as string; }, false);
        if (file) { reader.readAsDataURL(file); }
    }


    createOrEdit(tenantId: number | undefined, imageMin: FileParameter | undefined, imageMax: FileParameter | undefined, id: number | undefined) {
        let url_ = AppConsts.remoteServiceBaseUrl + "/api/services/app/v1/MstEsignLogo/CreateOrEdit";
        url_ = url_.replace(/[?&]$/, "");

        const content_ = new FormData();
        if (tenantId === null || tenantId === undefined)
            throw new Error("The parameter 'tenantId' cannot be null.");
        else
            content_.append("TenantId", tenantId.toString());
        if (imageMin.data === null || imageMin.data === undefined){}
        else
            content_.append("ImageMin", imageMin.data, imageMin.fileName ? imageMin.fileName : "ImageMin");
        if (imageMax.data === null || imageMax.data === undefined){}
        else
            content_.append("ImageMax", imageMax.data, imageMax.fileName ? imageMax.fileName : "ImageMax");
        if (id === null || id === undefined)
            throw new Error("The parameter 'id' cannot be null.");
        else
            content_.append("Id", id.toString());

        let options_ : any = {
            body: content_,
            observe: "response",
            responseType: "blob",
            headers: new HttpHeaders({
            })
        };
         return this.http.request("post", url_, options_);
    }
}
