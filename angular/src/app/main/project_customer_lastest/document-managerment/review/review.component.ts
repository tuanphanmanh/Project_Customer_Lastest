import { HttpClient } from "@angular/common/http";
import { Component, EventEmitter, Injector, Input, OnInit, Output, ViewChild } from "@angular/core";
import { AppConsts } from "@shared/AppConsts";
import { AppComponentBase } from "@shared/common/app-component-base";
import { EsignRequestInfomationDto, EsignRequestWebServiceProxy, MstEsignUserImageServiceProxy, MstEsignUserImageSignatureInput } from "@shared/service-proxies/service-proxies";
import { LocalStorageService } from "@shared/utils/local-storage.service";
import {
    AnnotationService, BookmarkViewService, FormDesignerService, FormFieldsService, LinkAnnotationService,
    MagnificationService, NavigationService, PdfViewerComponent, PrintService, TextSearchService, TextSelectionService,
    ThumbnailViewService, ToolbarService
} from "@syncfusion/ej2-angular-pdfviewer";
import * as FileSaver from "file-saver";
import { ModalDirective } from "ngx-bootstrap/modal";
import { finalize } from "rxjs";

@Component({
    selector: 'app-review',
    templateUrl: './review.component.html',
    styleUrls: ['./review.component.less'],
    providers: [LinkAnnotationService, BookmarkViewService, MagnificationService, 
        ThumbnailViewService, ToolbarService, NavigationService, 
        TextSearchService, TextSelectionService, PrintService, AnnotationService, FormFieldsService, FormDesignerService],
})
export class ReviewComponent extends AppComponentBase implements OnInit {

    serviceUrl = `${AppConsts.remoteServiceBaseUrl}/api/PdfViewer`;

    @ViewChild('modal', { static: false }) modal!: ModalDirective;
    @ViewChild('pdfviewer') public pdfviewerControl: PdfViewerComponent;
    @Input() listDocuments = [];
    @Input() selectDocumentId: number;
    documentUrl ;
    documentName;
    dialogWidth: string = '35%';
    dialogHeight: string = '71%';
    visible: Boolean = false;
    animationSettings: Object = { effect: 'FadeZoom' };
    selectedRequest : EsignRequestInfomationDto = new EsignRequestInfomationDto();
    public listDocumentsfields: Object = { text: 'documentName', value: 'id' };

    toolbarSettings = { toolbarItems : [
        'SearchOption', 'PrintOption', 'DownloadOption', 'OpenOption','FormDesignerEditTool', 'PageNavigationTool'
    ]};


    constructor(injector: Injector,
        private _http: HttpClient,
    ) {
        super(injector);

    }

    public ngOnInit() {
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

    public pageFitClicked(): void {
        this.pdfviewerControl.magnification.fitToWidth();
    }

    public zoomInClicked(): void {
        this.pdfviewerControl.magnification.zoomIn();
    }

    public zoomOutClicked(): void {
        this.pdfviewerControl.magnification.zoomOut();
    }

    show(path?, name?, id?){
        // console.log('--------------------------------------------------');
        // console.log(path);
        
        this.documentUrl = path;
        this.documentName = name;
        this.selectDocumentId = id;
        this.modal.show();
    }

    hide(){
        this.modal.hide();
    }

    changeFileDocument(e){
        this.selectDocumentId = e;
        this.documentUrl = this.listDocuments.find(_file=>_file.id == e).documentPath;
    }
    downloadDocument(){
        let document = this.listDocuments.find(_file=>_file.id == this.selectDocumentId);
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
    }
}
