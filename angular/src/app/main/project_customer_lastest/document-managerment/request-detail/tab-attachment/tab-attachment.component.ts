import { HttpClient, HttpEventType, HttpRequest } from '@angular/common/http';
import { Component, EventEmitter, Injector, Input, OnInit, Output, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { AppComponentBase } from '@shared/common/app-component-base';
import { UploadServiceProxy } from '@shared/service-proxies/service-proxies';
import { LocalStorageService } from '@shared/utils/local-storage.service';
import * as FileSaver from 'file-saver';
import { finalize } from 'rxjs';

@Component({
    selector: 'tab-attachment',
    templateUrl: './tab-attachment.component.html',
    styleUrls: ['./tab-attachment.component.less']
})
export class TabAttachmentComponent extends AppComponentBase implements OnInit {
    @Input() documents: any;
    @Input() request: any;
    @Input() requestId;
    @Input() TabMenuFilter;
    @Input() isUploadFile: boolean = false;
    @Output() onSelectDocument: EventEmitter<any> = new EventEmitter<any>();
    @Input() allowAction: boolean = true;

    constructor(
        injector: Injector,
        private _http: HttpClient,
        private local: LocalStorageService,
        private router: Router,
    ) {
        super(injector);

    }

    ngOnInit() {
    }

    previewSign(document) {
        this.local.setItem('selectedRequest', this.request);
        this.local.setItem('selectedDocumentId', {docId: document.id });
        this.router.navigate(['/app/main/sign-now']);
    }

    downloadDocument(document?){
        if(document){
            this.spinnerService.show();
            this._http.get(this.downloadUrl,{
                params: {
                    'hash': document.documentPath.split('hash=')[1],
                    'isDownload': "true",
                }
                , responseType: 'blob'
            }).pipe(finalize(()=>{
                this.spinnerService.hide();
            })).subscribe(blob => {
                FileSaver.saveAs(blob, document.documentName)
            })

        }
        else {
            //download all file
            this.documents.forEach(e => {
                this.spinnerService.show();
                this._http.get(this.downloadUrl,{
                    params: {
                        'hash': e.documentPath.split('hash=')[1],
                        'isDownload': "true",
                    }
                    , responseType: 'blob'
                }).pipe(finalize(()=>{
                    this.spinnerService.hide();
                })).subscribe(blob => {
                    FileSaver.saveAs(blob, e.documentName)
                })
            })

        }
    }
    goToReview(documentId: number){
        this.onSelectDocument.emit(documentId);
    }

    uploadFile(){
        let input = document.createElement('input');
        input.type = 'file';
        input.accept = '.pdf,.xls,.xlsx';
        input.className = 'd-none';
        input.id = 'imgInput';
        input.multiple = true;
        input.onchange = () => {
            let files = Array.from(input.files);
            this.onUpload(files);
        };
        input.click();
    }

    onUpload(files: Array<any>): void {
        if (files.length > 0) {
            files.forEach(f => {
                let formData: FormData = new FormData();
                formData.append('file', f);
                let file = {
                    file: f,
                    formData: formData,
                    name: f.name?.replaceAll("xlsx","pdf")?.replaceAll("xls","pdf")?.replaceAll("doc","pdf")?.replaceAll("docx","pdf"),
                    page : 0,
                    documentPath: "",
                    id: 0,
                };
                this.documents.push(file);
                let index = this.documents.length - 1;
                setTimeout(() =>{
                    this.readFileAndUpdateInfo(file, index);
                } , 100);
            });
        }
    }


    readFileAndUpdateInfo(file, index){
        let formData: FormData = new FormData();
        formData.append('file', file.file);

        const req = new HttpRequest('POST', this.uploadAdditionalUrl + "&requestId=" + this.requestId + "&documentOrder=" + (index + 1), formData, {
            reportProgress: true,
        });

        let isErr = false;
        let isOnline = window.navigator.onLine;

        this._http.request(req)
        .pipe(finalize(()=>{
            file.data = "";
        }))
        .subscribe((event) => {
            if(!isErr && isOnline){
                if (event.type === HttpEventType.UploadProgress) {
                    if (isErr || !isOnline){
                        this.documents[index].uploadStatus = "failed";
                        return;
                    }
                    else {
                        this.documents[index].uploadStatus = "uploading";
                    }
                } else if (event.type === HttpEventType.Response ) {
                    file.page = (event.body as any).result.totalPage;
                    this.documents[index].id = (event.body as any).result.id;
                    this.documents[index].documentPath = (event.body as any).result.documentPath;
                    this.documents[index].documentName = file.name;
                    this.documents[index].isAdditionalFile = true;
                    this.documents[index].documentOrder = Number(index + 1);

                }
            }
        });
    }
}
