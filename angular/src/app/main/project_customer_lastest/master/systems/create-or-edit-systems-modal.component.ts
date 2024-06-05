import { FileParameter } from './../../../../../shared/service-proxies/service-proxies';
import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef } from '@angular/core';
import { AppComponentBase } from '@shared/common/app-component-base';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { MstEsignSystemsServiceProxy, MstEsignSystemsDto } from '@shared/service-proxies/service-proxies';
import { finalize } from 'rxjs';
import { FormBuilder, FormGroup } from '@angular/forms';
import { GlobalValidator } from '@shared/utils/validators';
import { FileInfo, RemovingEventArgs, SelectedEventArgs, UploaderComponent } from '@syncfusion/ej2-angular-inputs';
import { EventHandler, createElement, detach } from '@syncfusion/ej2-base';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { AppConsts } from '@shared/AppConsts';
@Component({
    selector: 'create-or-edit-systems-modal',
    templateUrl: './create-or-edit-systems-modal.component.html'
})
export class CreateOrEditSystemsComponent extends AppComponentBase implements OnInit {
    @ViewChild('createOrEditSystems', { static: true }) modal: ModalDirective | undefined;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();
    @Output() modalClose: EventEmitter<any> = new EventEmitter<any>();
    @ViewChild("submitBtn", { static: false }) submitBtn: ElementRef;
    @ViewChild('previewupload') public uploadObj: UploaderComponent;
    public fileDetails: FileInfo;
    public parentElement: HTMLElement;
    public dropElement: HTMLElement;
    public fileName: string;
    public allowExtensions: string = '.png, .jpg, .jpeg, .svg';
    createOrEditForm: FormGroup;
    isSubmit = false;

    dialogCloseIcon = true;
    dialogWidth: string = '50%';
    isModal: Boolean = true;
    showCloseIcon: Boolean = false;
    visible: Boolean = false;
    animationSettings: Object = { effect: 'FadeZoom' };
    dialogHeader = this.l('CreateSystems');
    rowdata: MstEsignSystemsDto = new MstEsignSystemsDto();
    saving: boolean = false;
    config = {
        headers: {
            'Content-Type': 'multipart/form-data'
        }
    }
    img: FileParameter = { data: undefined, fileName: undefined };
    constructor(
        injector: Injector,
        private _service: MstEsignSystemsServiceProxy,
        private formBuilder: FormBuilder,
    ) {
        super(injector);
    }

    ngOnInit(): void {

    }

    buildForm() {
        this.createOrEditForm = this.formBuilder.group({
            id: [0],
            code: [undefined, GlobalValidator.required],
            localName: [undefined],
            internationalName: [undefined],
            localDescription: [undefined],
            internationalDescription: [undefined],
        });
    }

    showModalCreateEdit(rowdata?: any): void {
        if (!rowdata) {
            this.dialogHeader = this.l('CreateSystems');
            this.rowdata = new MstEsignSystemsDto();
        } else {
            this.rowdata = rowdata;
            this.dialogHeader = this.l('UpdateSystems')
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
        if (this.fileDetails) {
            this.img.data = this.fileDetails.rawFile;
            this.img.fileName = this.fileDetails.name;
        }
        this.createOrEdit(
            this.rowdata.code ?? '',
            this.rowdata.localName ?? '',
            this.rowdata.internationalName ?? '',
            this.rowdata.localDescription ?? '',
            this.rowdata.internationalDescription ?? '',
            this.img,
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

    changeColor(event: any) {
        this.rowdata.code = event.currentValue.hex;
    }

    onCloseDialog() {
        this.createOrEditForm = undefined;
    }

    public onFileUpload(args: any): void {
        let li: Element = document.getElementById('dropArea').querySelector('[data-file-name="' + args.file.name + '"]');
        let iconEle: HTMLElement = li.querySelector('#iconUpload') as HTMLElement;
        iconEle.style.cursor = 'not-allowed';
        iconEle.classList.add('e-uploaded');
        EventHandler.remove(li.querySelector('#iconUpload'), 'click', this.uploadFile);
    }

    public uploadFile(args: any): void {
        this.uploadObj.upload([this.fileDetails], true);
    }

    public onUploadFailed(args: any): void {
        let li: Element = document.getElementById('dropArea').querySelector('[data-file-name="' + args.file.name + '"]');
        (li.querySelector('.file-name') as HTMLElement).style.color = 'red';
        li.setAttribute('title', args.e.currentTarget.statusText)
        if (args.operation === 'upload') {
            EventHandler.remove(li.querySelector('#iconUpload'), 'click', this.uploadFile);
        }
    }

    public onFileRemove(args: RemovingEventArgs): void {
        args.postRawFile = false;
    }

    public onSelect(args: SelectedEventArgs): void {
        if (!this.dropElement?.querySelector('li')) { this.fileDetails = null }
        if (document.getElementById('dropArea')?.querySelector('.e-upload-files') == null) {
            this.parentElement = createElement('ul', { className: 'e-upload-files' });
            document.getElementsByClassName('e-upload')[0].appendChild(this.parentElement);
        }
        let validFiles: FileInfo = this.validateFiles(args);
        this.formSelectedData(validFiles, this);
        this.fileDetails = validFiles;
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

    public formSelectedData(file: FileInfo, proxy: any): void {
        let liEle: HTMLElement = createElement('li', { className: 'e-upload-file-list', attrs: { 'data-file-name': file.name } });
        let imageTag: HTMLImageElement = <HTMLImageElement>createElement('IMG', { className: 'upload-image', styles: 'height: 100px;margin: 0 10px;border-radius: 10px;box-shadow: rgba(0, 0, 0, 0.24) 0px 3px 8px;', attrs: { 'alt': 'Image' } });
        liEle.appendChild(imageTag);
        liEle.appendChild(createElement('div', { className: 'name file-name', styles: 'padding: 10px 10px 0px 10px;', innerHTML: 'File name: ' + file.name, attrs: { 'title': file.name } }));
        liEle.appendChild(createElement('div', { className: 'file-size', styles: 'padding: 10px 10px 0px 10px;', innerHTML: 'File size: ' + proxy.uploadObj.bytesToSize(file.size) }));
        let clearbtn: HTMLElement;
        clearbtn = createElement('span', { id: 'removeIcon', className: 'e-icons e-file-remove-btn', attrs: { 'title': 'Remove' } });
        EventHandler.add(clearbtn, 'click', this.removeFiles, proxy);
        liEle.appendChild(clearbtn);
        liEle.setAttribute('title', 'Ready to Upload');
        this.readURL(liEle, file); document.querySelector('.e-upload-files').appendChild(liEle);
    }

    public removeFiles(args: any): void {
        let removeFile: FileInfo = this.fileDetails;
        let statusCode: string = removeFile.statusCode;
        if (statusCode === '2' || statusCode === '1') {
            this.uploadObj.remove(removeFile, true);
            this.uploadObj.element.value = '';
        }
        this.fileDetails = null;
        this.fileName = null;
        if (statusCode !== '2') { detach(args.currentTarget.parentElement); }
    }
    public readURL(li: HTMLElement, args: any): void {
        let preview: HTMLImageElement = li.querySelector('.upload-image');
        let file: File = args.rawFile; let reader: FileReader = new FileReader();
        reader.addEventListener('load', () => { preview.src = reader.result as string; }, false);
        if (file) { reader.readAsDataURL(file); }
    }

    createOrEdit(code: string | undefined, localName: string | undefined, internationalName: string | undefined, localDescription: string | undefined, internationalDescription: string | undefined, image: FileParameter | undefined, id: number | undefined) {
        let url_ = AppConsts.remoteServiceBaseUrl + "/api/services/app/v1/MstEsignSystems/CreateOrEdit";
        url_ = url_.replace(/[?&]$/, "");

        const content_ = new FormData();
        if (code === null || code === undefined)
            throw new Error("The parameter 'code' cannot be null.");
        else
            content_.append("Code", code.toString());
        if (localName === null || localName === undefined)
            throw new Error("The parameter 'localName' cannot be null.");
        else
            content_.append("LocalName", localName.toString());
        if (internationalName === null || internationalName === undefined)
            throw new Error("The parameter 'internationalName' cannot be null.");
        else
            content_.append("InternationalName", internationalName.toString());
        if (localDescription === null || localDescription === undefined)
            throw new Error("The parameter 'localDescription' cannot be null.");
        else
            content_.append("LocalDescription", localDescription.toString());
        if (internationalDescription === null || internationalDescription === undefined)
            throw new Error("The parameter 'internationalDescription' cannot be null.");
        else
            content_.append("InternationalDescription", internationalDescription.toString());
        if (image.data === null || image.data === undefined){}
        else
            content_.append("Image", image.data, image.fileName ? image.fileName : "Image");
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
