
// import { PdfMergeService } from './../../../shared/common/services/pdf-merge.service';
// import { GridParams } from './../../../shared/common/models/base.model';
import { ChangeDetectionStrategy, ChangeDetectorRef, Component, Injector, NgZone, OnInit, ViewChild, ViewEncapsulation } from '@angular/core';
import { AppComponentBase } from '@shared/common/app-component-base';
import { BeforeOpenCloseMenuEventArgs, ContextMenuComponent, ContextMenuModule, DragEventArgs, MenuEventArgs, MenuItemModel, ToolbarComponent } from '@syncfusion/ej2-angular-navigations';
import { Draggable, Droppable, DropEventArgs } from '@syncfusion/ej2-base';
import { AllowedInteraction, AnnotationResizerLocation, AnnotationSelectorSettingsModel, AnnotationService, BookmarkViewService, ContextMenuItem, DynamicStampItem, FontStyle, FormDesignerService, FormFieldType, FormFieldsService, LinkAnnotationService, LoadEventArgs, MagnificationService, NavigationService, PageChangeEventArgs, PdfViewer, PdfViewerBase, PdfViewerComponent, PrintService, RectangleSettings, SignStampItem, SignatureFieldSettings, TextFieldSettings, TextSearchService, TextSelectionService, ThumbnailViewService, ToolbarService } from '@syncfusion/ej2-angular-pdfviewer';
import { EventBusService } from '@app/shared/common/services/event-bus.service';
import { NavigationStart, Router } from '@angular/router';
import { LocalStorageService } from '@shared/utils/local-storage.service';
import { SignToPageModalComponent } from '../esign-modal/sign-to-page-modal/sign-to-page-modal.component';
import { CreateOrEditDocumentDto, CreateOrEditEsignRequestDto, CreateOrEditPositionsDto, CreateOrEditSignersDto, EsignRequestInfomationDto, EsignRequestServiceProxy, EsignRequestWebServiceProxy, EsignSignerTemplateLinkCreateNewRequestForWebDto, EsignSignerTemplateLinkCreateNewRequestv1Dto, EsignSignerTemplateLinkListSignerForWebDto, EsignSignerTemplateLinkListSignerv1Dto, EsignSignerTemplateLinkServiceProxy, MstEsignCategoryServiceProxy, MstEsignUserImageServiceProxy, MstEsignUserImageSignatureInput } from '@shared/service-proxies/service-proxies';
import { MissingSignatureComponent } from '../esign-modal/missing-signature/missing-signature.component';
import { CommonFunction } from '@app/main/commonfuncton.component';
import { catchError, finalize, tap, } from 'rxjs';
import { ConfirmSendRequestComponent } from '../esign-modal/confirm-send-request/confirm-send-request.component';
import { AppConsts } from '@shared/AppConsts';
import * as moment from 'moment';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { ContextMenuService } from '@syncfusion/ej2-angular-grids';
import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { ConfirmFileExistsComponent } from '../esign-modal/confirm-file-exists/confirm-file-exists.component';
import { HttpClient, } from '@angular/common/http';





@Component({
    selector: 'app-add-signature',
    templateUrl: './add-signature.component.html',
    styleUrls: ['./add-signature.component.less'],
    animations: [appModuleAnimation()],
    // encapsulation: ViewEncapsulation.None,
    // changeDetection: ChangeDetectionStrategy.OnPush,
    providers: [ LinkAnnotationService, BookmarkViewService, MagnificationService,
        ThumbnailViewService, ToolbarService, NavigationService,
        TextSearchService, TextSelectionService, PrintService,ContextMenuService,
        AnnotationService]
})
export class AddSignatureComponent extends AppComponentBase implements OnInit {
    @ViewChild('pdfviewer') public pdfviewerControl: PdfViewerComponent;
    @ViewChild('pdfviewerPreview') public pdfviewerPreviewControl: PdfViewerComponent;
    @ViewChild('choosePage') choosePage: SignToPageModalComponent;
    @ViewChild('missingSignature') missingSignature: MissingSignatureComponent;
    @ViewChild('confirm') confirm!: ConfirmSendRequestComponent;
    @ViewChild('confirmFileExists') confirmFileExists!: ConfirmFileExistsComponent;

    @ViewChild('contextmenu') public cmenu?: ContextMenuComponent;

    urlCreateOrEditRequest = `${AppConsts.remoteServiceBaseUrl}/api/services/app/v1/EsignRequestWeb/CreateOrEditEsignRequest`;

    dataForSave :any
    copyData: any;
    copyDatas: any;

    currentPageNumber = 1;
    zoomValue = 100;

    isPreview= false;

    firstMultiAnnotationX:any;
    firstMultiAnnotationY:any;
    newAnnotationPos: any;

    selectedFile = 0;
    public fields: Object = { text: 'fileName', value: 'id' };

    fontFamilyDataSource = [
        'Arial',
        'Times New Roman',
        'Helvetica',
        'Verdana',
        'Georgia',
        'Roboto',
        'Courier New',
        'Tahoma',
        'Trebuchet MS',
        'Palatino',
        'Comic Sans MS',
    ]

    fontSizeDataSource = [4,6,8,10,12, 14, 16, 18, 20, 24, 28, 32, 50, 40];;

    fontFamily = 'Roboto';

    fontSize = 8;
    fontStyle = FontStyle.None;

    isBold = false;
    isItalic = false;
    isUnderline = false;

    fontColor = "#035a"


    menuItems: MenuItemModel[] = [
        {
            text: 'Select All',
            id : 'selectAll-context'
        },

        {
            text: 'Cut',
            id : 'cut-context'
        },
        {
            text: 'Copy',
            id : 'copy-context'
        },
        {
            text: 'Paste',
            id : 'paste-context'
        },
        {
            text: 'Delete',
            id : 'delete-context'
        },
        {
            separator: true
        },
        {id :'contextItem-rotate90',text : "Rotate 90"},
        {id :'contextItem-rotate180',text : "Rotate 180"},
        {id :'contextItem-cloneToPage',text : "Replicate To Page"},
        {id :'contextItem-cloneToAllPages',text : "Replicate To All Pages"},
    ];

    altCloning = false;

    public documentUrl: string = 'http://esign-standby.toyotavn.com.vn:5001/Upload/DownloadFile?hash=C15DA1F2B5E5ED6E6837A3802F0D1593';
    public documentUrl2: string = 'http://esign-standby.toyotavn.com.vn:5001/Attachments/DATN.pdf';

    serviceUrl = `${AppConsts.remoteServiceBaseUrl}/api/PdfViewer`;

    pageMouseX: any;
    pageMouseY: any;
    pageNum:any;

    documentData : any ={};

    params:any;
    formFieldPositionList = [];

    pasteX;
    pasteY ;
    pastePageNum ;

    fileData = [];


    // isSaveTemplateSingerCC:boolean;
    // _dataSaveSingerCC: EsignSignerTemplateLinkCreateNewRequestForWebDto;
    // _dataSaveSingerCC_Signer: EsignSignerTemplateLinkListSignerForWebDto[];

    textFieldSetting = {
        annotationSelectorSettings: {
            selectionBorderColor: '#0374f6',
            resizerBorderColor: '#0374f6',
            resizerFillColor: '#0374f6',
            resizerSize: 8,
            selectionBorderThickness: 2,
            resizerShape: 'Square',
            selectorLineDashArray: [0],
            resizerLocation: AnnotationResizerLocation.Corners,
            resizerCursorType: null
        }
    }

    lineSettings = {};
    arrowSettings = {};
    rectangleSettings = {
        annotationSelectorSettings: {
            selectionBorderColor: '#0374f6',
            resizerBorderColor: '#0374f6',
            resizerFillColor: '#0374f6',
            resizerSize: 8,
            selectionBorderThickness: 2,
            resizerShape: 'Square',
            selectorLineDashArray: [0],
            resizerLocation: AnnotationResizerLocation.Corners,
            resizerCursorType: null
        }
    };
    circleSettings = {};
    polygonSettings = {};

    signatureFieldSettings ={};

    annotationSelectorSettings ={
        selectionBorderColor: '#0374f6',
        resizerBorderColor: '#0374f6',
        resizerFillColor: '#0374f6',
        resizerSize: 8,
        selectionBorderThickness: 2,
        resizerShape: 'Square',
        selectorLineDashArray: [0],
        resizerLocation: AnnotationResizerLocation.Corners,
        resizerCursorType: null,
    }
    ctrlKey;


    dragFromTool = true;
    selectedSigners = [];

    customContextMenuItem = [
        {id :'contextItem-rotate90',text : "Rotate 90"},
        {id :'contextItem-rotate180',text : "Rotate 180"},
        {id :'contextItem-cloneToAllPages',text : "Replicate To All Pages"},
    ]

    dragSigners= [];

    currentDragName;

    colorList = []

    backgroundColorList = [ ]


    dragElemX: any;
    dragElemY: any;

    signerData = [];
    toolbarSettings;

    requestData :EsignRequestInfomationDto = new EsignRequestInfomationDto();
    listCategory: any[] = [];
    isMandatory = false;
    _fn: CommonFunction = new CommonFunction();
    constructor(injector: Injector, private _eventBus: EventBusService,private zone : NgZone, private router : Router,private requestWeb : EsignRequestWebServiceProxy,
        private local : LocalStorageService, private cdr: ChangeDetectorRef, private _requestService: EsignRequestServiceProxy,private _requestWebService: EsignRequestWebServiceProxy,
        private _category : MstEsignCategoryServiceProxy,
        private _mstEsignUserImage: MstEsignUserImageServiceProxy,
        private _esignSignerTemplateLink: EsignSignerTemplateLinkServiceProxy,
        private dateFormat: DateTimeService,
        private _http: HttpClient,
    ) {
        super(injector);

        this._fn.isShowUserProfile();

        this.router.events.subscribe((event) => {
            if ( window.location.href.includes("add-signature") && event instanceof NavigationStart) {
                // alert(1)
                //this.updateCacheData();
            }
        })

        // window.addEventListener("beforeunload", function (e) {
        //     // Your code here
        //     // You can use this event to show a confirmation dialog to the user, e.g., "Are you sure you want to leave this page?"
        //     // Returning a string from this function will prompt the user with a confirmation dialog.
        //     e.returnValue = "Are you sure you want to leave this page?";
        //   });
    }

    // name: 'Form Designer',
    // fileNum: 1,
    // page: 6,
    // signature: 'https://ej2.syncfusion.com/demos/src/pdfviewer/images/signature/signature1.png',
    // signers: [
    //     {
    //         name: 'Nancy Davolio',
    //         imgUrl: 'https://i.pinimg.com/564x/89/8c/e8/898ce8537c9d8192175319d9c48cb029.jpg',
    //     },

    backToCreate(){

        this.local.getItem("documentData",(err, data)=>{
            this.documentData.id = data.id
        })

        this.saveDraftData();

        this.documentData.stepList?.forEach(e => {
            e.signers?.forEach(p => {
                p.check = false;
                p.numOfSign = 0;
                // p.formFields?.forEach(k => {
                //     k.field = {};
                // })
                p.formFields = [];
                //p.formFields = this.signerData.find(k => k.userId == e.userId );
            });
        })

        // alert(this.documentData.id)

        // Object.assign(this.documentData,{
        //     //formFields : formFieldSettings
        // })


        this.local.setItem("documentData",this.documentData)
        this.router.navigate(['/app/main/create-new-request']);

    }

    updateCacheData(){
        // let formFieldSettings = []
        // this.pdfviewerControl.formFields.forEach(e => {
        //     formFieldSettings.push(
        //         Object.assign({
        //             type: e.formFieldAnnotationType,
        //             name: e.name,
        //             value: e?.value ?? "",
        //             bounds: { X: e.bounds.x , Y: e.bounds.y , Width: e.bounds.width, Height: e.bounds.height },
        //             pageNumber : e.pageNumber,
        //             backgroundColor :e.backgroundColor,
        //             fontFamily: e?.fontFamily,
        //             color: e?.color,
        //             fontSize: e?.fontSize,
        //             fontStyle: e?.fontStyle,
        //             zIndex: 2
        //         }) as any
        //     )
        // })

        this.local.getItem("documentData",(err, data)=>{
            this.documentData.id = data.id;
            this.documentData.addCC = data.addCC;
        })

        this.documentData.stepList.forEach(e => {
            e.signers.forEach(p => {
                p.check = false;
                p.numOfSign = 0;
                p.formFields.forEach(k => {
                    k.field = {};
                })
                //p.formFields = this.signerData.find(k => k.userId == e.userId );
            });
        })

        // alert(this.documentData.id)

        // Object.assign(this.documentData,{
        //     //formFields : formFieldSettings
        // })

        this.local.setItem("documentData",this.documentData)

    }

    // initializePdfViewer() {
    //     // Access the PdfViewerComponent properties and methods here
    //     // For example, set up services, register events, etc.

    //     // Example: Setting up services
    //     this.pdfviewerControl.serviceUrl = this.serviceUrl;
    //     this.pdfviewerControl.formFieldMove = (e) => this.moveFormField(e);
    //     this.pdfviewerControl.created = (e) => this.onCreated();
    //     this.pdfviewerControl.annotationSelect = (e) => this.annotationSelect(e);
    //     this.pdfviewerControl.annotationMoving = (e) => this.annotationMove(e);
    //     this.pdfviewerControl.annotationUnSelect = (e) => this.annotationUnSelect(e);
    //     this.pdfviewerControl.pageMouseover = (e) => this.annotationUnSelect(e);
    //     this.pdfviewerControl.pageChange = (e) => this.changePagePdfViewer();
    //     this.pdfviewerControl.zoomChange = (e) => this.changeScalePdfViewer();
    //     this.pdfviewerControl.formFieldDoubleClick = (e) => this.chooseSignature(e);
    //     this.pdfviewerControl.pageClick = (e) => this.pageClick(e);
    //     this.pdfviewerControl.formFieldSelect = (e) => this.selectFormField(e);
    //     this.pdfviewerControl.formFieldUnselect = (e) => this.unselectFormField(e);
    //     this.pdfviewerControl.documentLoad = (e) => this.pdfDoneLoading(e);
    //     // this.pdfviewerControl.toolbarSettings = { showToolbar: true, toolbarItems: [] }; // Customize toolbar as needed
    //     // this.pdfviewerControl.serviceUrl = this.serviceUrl;
    //     // this.pdfviewerControl.magnificationService = new MagnificationService(this.pdfViewer);
    //     // this.pdfviewerControl.navigationService = new NavigationService(this.pdfViewer);
    //   }

    ngOnInit() {
        this._mstEsignUserImage.getListSignatureByUserIdForWeb().subscribe(res =>{
            if(res) {


                let _default = res.items.find(e => e.isUse == true);
                if(_default) this.SignatureTemplateID = _default.id;

                this.SignatureType = 1

            }
        });

        this._mstEsignUserImage.getListSignatureByUserIdForWeb().subscribe(res =>{
            if(res) {
                this.SignatureImageBase64  = res.items.find(e => e.isUse == true)?.imgUrl ?? "";
                this.hasSignature = (res.items.find(e => e.isUse == true)  && res.items.find(e => e.isUse == true)?.imgUrl) ? true : false ;

                // this.listTemplateSignature = [];
                // this.listTemplateSignature.push(Object.assign(res.items, {
                //     isEmpty: false
                // }));

                // for(let i=res.items.length; i<4; i++){
                //     this.listTemplateSignature.push({id : (0-i),imgUrl: "/assets/common/images/signature_empty.png", isUse: false, isEmpty: true,});
                // }
            }
        });


        this.local.getItem("documentData",(err, data)=>{
            this.documentUrl = data.itemList[0].documentPath;
            // console.log('-------------------------------------');
            // console.log(this.documentUrl);
        });


        // this.pdfviewerControl.enableThumbnail = true;
        // this.pdfviewerControl.isThumbnailViewOpen = true;

        // let page = document.getElementById("pdfViewer_sideBarTitle");
        // page.innerText = "Total pages :" + this.fileData.find(e => e.id == this.selectedFile )?.pageNum;

        let toggle = document.getElementById("kt_app_sidebar_toggle");
        toggle.addEventListener('click',()=> {
            setTimeout(()=>{

                this.pageFitClicked();
                //this.pdfviewerControl?.load(this.documentUrl?.replace("https","http")  ,null);
                // alert(1)
            },1000)
        });
        this._category.getAllCategories("","").subscribe(res => {
            this.listCategory = res.items;
        });


    }

    getRandomColor() {
        var color = Math.floor(0x1000000 * Math.random()).toString(16);
            return '#' + ('000000' + color).slice(-6);
    }

    onCreatePreview(){
        this.pdfviewerPreviewControl?.load(this.documentUrl?.replace("https","http")  ,null);
    }

    onCreated(){

        this.zone.runOutsideAngular(()=>{

            // let viewer = new PdfViewer;

            // viewer.annotationSelectorSettings  ={
            //     selectionBorderColor: '#4070FF',
            //     resizerBorderColor: '#4070FF',
            //     resizerFillColor: '#4070FF',
            //     resizerSize: 8,
            //     selectionBorderThickness: 2,
            //     resizerShape: 'Square',
            //     // selectorLineDashArray: ["Solid"],
            //     resizerLocation: AnnotationResizerLocation.Corners,
            //     resizerCursorType: null
            // }

            // viewer.appendTo("#pdfViewer");

        })

        document.getElementById('fileUpload').addEventListener('change', this.readFile.bind(this));
        //hide items in toolbar
        this.toolbarSettings = { toolbarItems : [
            'SearchOption', 'PrintOption', 'DownloadOption', 'OpenOption','FormDesignerEditTool', 'PageNavigationTool'
        ]};

        this.pdfviewerControl.isFormDesignerToolbarVisible = true;

        this.pdfviewerControl.enableFormDesignerToolbar = false;

        this.pdfviewerControl.enableShapeAnnotation = true;
        // this.pdfviewerControl.annotationMove= (e) => this.annotationMove(e)
        this.pdfviewerControl.contextMenuSettings = {
            contextMenuItems : []
        };

        setTimeout(()=>{
            // document.getElementById("pdfViewer_toolbarContainer").remove();
            // document.getElementById("pdfViewer_formdesigner_toolbar").remove();
            // //side tool
            // // document.getElementById("pdfViewer_sideBarToolbar").remove();
            // document.getElementById("pdfViewer_sideBarResizer").remove();

        })

        // alert(1)
        this.selectedFile = this.fileData[0].id;
        // this.documentUrl = this.fileData[0].documentPath; //Đoàn Hiệp comment dòng này
        this.pdfviewerControl?.load(this.documentUrl  ,null);
    }

    changeFile(params,previousSelectedFile){
        // this.applyPosToPdf();
        if(this.selectedFieldId) this.selectedFieldId = undefined;
        this.selectedFile = params;

        this.pdfviewerControl.formFields = [];

        this.pdfviewerControl.formFields.forEach(e => {
            this.pdfviewerControl.formDesignerModule.deleteFormField(e.id)
        });

        this.documentUrl = this.fileData.find(e => e.id == params )?.documentPath;

        let draf = this.setSaveRequestData();

        draf.id = this.documentData.id;

        draf.category = this.documentData.listCategoryId.join(",");
        draf.systemId = 1; //esign
        draf.statusType = 0; //on progress
        // draf.listCategoryId = [];
        // this.documentData?.categoryList?.forEach(e => {
        //     if(!draf?.listCategoryId?.some(p => p == parseFloat(e))) draf?.listCategoryId?.push(parseFloat(e))
        // })
        draf.expectedDate = this.documentData.expectedDate;

        if (draf.id && draf.id > 0){
            this.spinnerService.show();
            this._requestWebService.saveDraftRequest(draf)
            .pipe(finalize(()=>{
                this.spinnerService.hide();
                this.setUpFieldForSigner()
            }))
            .subscribe(res => {

            })
        }


        // let formFieldSettings = []
        // this.pdfviewerControl.formFields.forEach(e => {
        //     formFieldSettings.push(
        //         Object.assign({
        //             type: e.formFieldAnnotationType,
        //             name: e.name,
        //             value: e?.value ?? "",
        //             bounds: { X: e.bounds.x , Y: e.bounds.y , Width: e.bounds.width, Height: e.bounds.height },
        //             pageNumber : e.pageNumber,
        //             backgroundColor :e.backgroundColor,
        //             fontFamily: e?.fontFamily,
        //             color: e?.color,
        //             fontSize: e?.fontSize,
        //             fontStyle: e?.fontStyle,
        //             zIndex: 2
        //         }) as any
        //     )
        // })

        setTimeout(()=>{
            // document.getElementById("pdfViewer_toolbarContainer").remove();
            // document.getElementById("pdfViewer_formdesigner_toolbar").remove();
            // //side tool
            // // document.getElementById("pdfViewer_sideBarToolbar").remove();
            // document.getElementById("pdfViewer_sideBarResizer").remove();

        })

        // this.pdfviewerControl?.formFields.
        this.pdfviewerControl?.load(this.documentUrl  ,null);
        // this.pageFitClicked();

        (this.cmenu as ContextMenuComponent).hideItems(['Cut','Copy','Delete','Rotate 90','Rotate 180','Replicate To Page','Replicate To All Pages']);
            (this.cmenu as ContextMenuComponent).insertBefore([{
                text: 'Add Signature',
                id : 'addSignature-context',
                items: this.setSigner()
            }],"Select All");


    }

    getRequestData(){
        this._requestService.getRequestInfomationById(this.documentData?.id).subscribe(res => {
            this.requestData = res ?? new EsignRequestInfomationDto()
        })
    }

    ngAfterViewInit() {
            this.documentData = this.local.getItem("documentData",(err, data)=>{
                this.documentData = data;
                if (this.documentData?.id) this.getRequestData();
                if (this.documentData?.stepList?.length > 0){
                    this.signerData = [];
                    this.fileData = [];
                    let index = 0;
                    this.documentData?.stepList?.forEach((e,i)=>{
                        e.signers.forEach(p =>{
                                // p.color = this.colorList[index%8];
                                p.backgroundColor = this.lightenColor(p.color ?? "",0.5)
                                index++;
                            } )

                        this.signerData.push({
                            rank : i+ 1,
                            signers : e.signers
                        })

                    })


                }
                if (this.documentData?.itemList?.length > 0){
                    // let listData = []
                    this.documentData?.itemList?.forEach((e,i)=>{
                        this.fileData.push({
                            documentOrder: i + 1,
                            fileName: e.name,
                            documentPath: e.documentPath,
                            totalPageNumber : e.page,
                            id: e.id,
                        })
                        // listData.push(e.data)
                    })

                    // this.fileData.unshift({fileName : "All Files" , documentPath : "", id : 0})

                }


    // _dataSaveSingerCC: EsignSignerTemplateLinkCreateNewRequestv1Dto;
    // _dataSaveSingerCC_Signer: EsignSignerTemplateLinkListSignerv1Dto;
            });

        // this.pdfviewerControl.cdr.detach();
        this.zone.runOutsideAngular(()=>{
            // this.initializePdfViewer()
            // // this.pdfviewerControl.contextMenuSettings.contextMenuItems.unshift(16 as ContextMenuItem)
            // let hehe = document.createElement("ejs-pdfviewer");
            // hehe.setAttribute("id","pdfViewer");
            // hehe.setAttribute("documentPath",this.documentUrl);
            // hehe.setAttribute("serviceUrl",this.pdfServiceUrl);
            // hehe.style.height = '640px';
            // hehe.style.display = 'block';
            // hehe.style.width = '100%';

            // document.getElementsByClassName("frame-10000039792")[0].appendChild(hehe)

            this.recreateDragElement()

            new Droppable(document.documentElement, {
                drop: (e: DropEventArgs) => {
                    this.zone.runOutsideAngular(()=>{
                        e.droppedElement.remove();
                        // document.getElementById("lmao").remove();
                        this.recreateDragElement()
                    })

                }
            });

                // this.pdfDoneLoading();


            //this.pdfviewerControl.formDesignerModule.cole

            // document.addEventListener("keydown",(e)=>{
            //     e.preventDefault();
            //     if (e.ctrlKey ){
            //         this.ctrlKey = true;
            //     }
            //     else if (e.altKey){
            //         this.altCloning = true;
            //     }
            // })

            // document.addEventListener("keyup",(e)=>{
            //     e.preventDefault()
            //         this.ctrlKey = false;
            //         this.altCloning = false;

            // })

            document.getElementById("pdfViewer").addEventListener("mousemove",(e)=>{

            })

            document.getElementById("pdfViewer").addEventListener("keydown",(e)=>{
                e.preventDefault();
                if (e.ctrlKey ){
                    this.ctrlKey = true;
                }
                else if (e.altKey){
                    this.altCloning = true;
                }
                var keystroke = String.fromCharCode(e.keyCode).toLowerCase();
                if(this.ctrlKey && keystroke == 'c') {// ctrl c
                    this.copyDatas = [];
                    this.selectedFieldIds.forEach(e => {
                        this.copyDatas.push(this.pdfviewerControl.formDesignerModule.getFormField(e));
                    })
                }
                else if (this.ctrlKey && keystroke == 'x'){
                    this.copyDatas = []
                    this.selectedFieldIds.forEach(selectedFieldId =>{
                        let copyDataInfo = this.pdfviewerControl?.formDesignerModule.getFormField(selectedFieldId);
                        this.copyDatas.push(copyDataInfo)
                        this.pdfviewerControl?.formDesignerModule.deleteFormField(copyDataInfo);
                        // delete from array
                        this.signerData.forEach(e => {
                            e.signers.forEach(p => {
                                if (p.formFields?.some(k => k.fieldId == selectedFieldId)){
                                    let formFieldSelect = p.formFields?.find(e => e.fieldId == selectedFieldId);
                                    p.formFields?.splice(p.formFields?.findIndex(s => s.fieldId == selectedFieldId ),1)
                                    if (formFieldSelect.field.name.includes('signature')) p.numOfSign = (p.numOfSign >0) ? (p.numOfSign - 1) : 0

                                }
                            })

                        })
                    })
                    if (this.pdfviewerControl?.annotationCollection?.length > 0)this.pdfviewerControl.annotationModule.deleteAnnotationById(this.pdfviewerControl.annotationCollection[0].annotationId);

                    (this.cmenu as ContextMenuComponent).hideItems(['Cut','Copy','Delete','Rotate 90','Rotate 180','Replicate To Page','Replicate To All Pages']);
                    (this.cmenu as ContextMenuComponent).insertBefore([{
                        text: 'Add Signature',
                        id : 'addSignature-context',
                        items: this.setSigner()
                    }],"Select All");

                }
                else if(this.ctrlKey && keystroke == 'v') { //ctrl v
                    if (this.pdfviewerControl?.annotationCollection?.length > 0)this.pdfviewerControl.annotationModule.deleteAnnotationById(this.pdfviewerControl.annotationCollection[0].annotationId)
                    let zoomValue = parseFloat(this.pdfviewerControl.zoomValue.toString().replace("%",""));

                    let x = (this.pageMouseX )
                    let y = (this.pageMouseY )
                    if (this.copyDatas.length > 1){

                        let x2 = x +  (x/zoomValue)*(100-zoomValue) ;
                        let y2 = y +  (y/zoomValue)*(100-zoomValue) ;

                        let x1 = (this.copyDatas.length > 1) ? this.firstMultiAnnotationX : this.copyDatas[0].bounds.x;
                        let y1 = (this.copyDatas.length > 1) ? this.firstMultiAnnotationY : this.copyDatas[0].bounds.y ;

                        // let x2 = (this.pasteX )
                        // let y2 = (this.pasteX )

                        this.copyDatas.forEach(copyDataInfo => {
                            // let data = this.pdfviewerControl.formDesignerModule.getFormField(e);
                            let newPos = this.calculateNewPoint(x1,y1,x2,y2,copyDataInfo.bounds.x,copyDataInfo.bounds.y);

                            // if(this.altCloning){
                            this.pdfviewerControl?.formDesignerModule.addFormField(copyDataInfo.formFieldAnnotationType as FormFieldType,
                                Object.assign({
                                    // type: copyDataInfo.formFieldAnnotationType,
                                    name: copyDataInfo.name,
                                    value: copyDataInfo.value,
                                    bounds: { X: newPos.x , Y: newPos.y , Width: copyDataInfo.bounds.width, Height: copyDataInfo.bounds.height },
                                    pageNumber : this.pageNum,
                                    backgroundColor :copyDataInfo.backgroundColor,
                                    fontFamily: copyDataInfo.fontFamily,
                                    fontSize: copyDataInfo.fontSize,
                                    color : copyDataInfo.color,
                                    alignment:  copyDataInfo.alignment,
                                    zIndex: 2
                                }) as any);
                            this.signerData.forEach(e => {
                                e.signers.forEach(p => {
                                    if (p.formFields?.some(k => k.fieldId == copyDataInfo.id)){
                                        p.numOfSign = (p.numOfSign ?? 0) + 1
                                        p.formFields?.push({
                                            pageNumber: this.pdfviewerControl.formFields[this.pdfviewerControl.formFields.length-1].pageNumber,
                                            position: {
                                                x:Math.round(this.pdfviewerControl.formFields[this.pdfviewerControl.formFields.length-1].bounds.x),
                                                y:Math.round(this.pdfviewerControl.formFields[this.pdfviewerControl.formFields.length-1].bounds.y),
                                                w:Math.round(this.pdfviewerControl.formFields[this.pdfviewerControl.formFields.length-1].bounds.width),
                                                h:Math.round(this.pdfviewerControl.formFields[this.pdfviewerControl.formFields.length-1].bounds.height),
                                                rotate: 0,
                                            },
                                            style: {
                                                isBold: false,
                                                isItalic: false,
                                                isUnderline: false,
                                                alignment: this.pdfviewerControl.formFields[this.pdfviewerControl.formFields.length-1].alignment,
                                                color: this.pdfviewerControl.formFields[this.pdfviewerControl.formFields.length-1].color,
                                                name : 'signature',
                                                value: this.pdfviewerControl.formFields[this.pdfviewerControl.formFields.length-1].value,
                                                typeId : 1,
                                                backgroundColor: this.pdfviewerControl.formFields[this.pdfviewerControl.formFields.length-1].backgroundColor,
                                                fontSize : this.pdfviewerControl.formFields[this.pdfviewerControl.formFields.length-1].fontSize ?? 8
                                            },
                                            documentId :this.selectedFile,
                                            fieldId : this.pdfviewerControl.formFields[this.pdfviewerControl.formFields.length-1].id
                                        })
                                    }
                                })

                            })
                            // }
                            // this.pdfviewerControl.formDesignerModule.updateFormField(this.pdfviewerControl.formDesigner.getFormField(e),{  bounds: {X : newPos.x + data.bounds.width/2   , Y: newPos.y + data.bounds.height/2 , Width: data.bounds.width, Height: data.bounds.height }} as any)

                        })

                        this.firstMultiAnnotationX = this.newAnnotationPos.x ;
                        this.firstMultiAnnotationY = this.newAnnotationPos.y ;
                        this.newAnnotationPos = undefined;

                        this.pendding_rendering_field_html(200)
                    }
                    else if (this.copyDatas.length == 1){
                        let x2 = x +  (x/zoomValue)*(100-zoomValue) ;
                        let y2 = y +  (y/zoomValue)*(100-zoomValue) ;
                        this.copyDatas.forEach(copyDataInfo => {
                            this.pdfviewerControl?.formDesignerModule.addFormField(copyDataInfo.formFieldAnnotationType as FormFieldType,
                                Object.assign({
                                    // type: copyDataInfo.formFieldAnnotationType,
                                    name: copyDataInfo.name,
                                    bounds: { X: x2 , Y: y2 , Width: copyDataInfo.bounds.width, Height: copyDataInfo.bounds.height },
                                    pageNumber : this.pageNum,
                                    backgroundColor :copyDataInfo.backgroundColor,
                                    zIndex: 2
                                }) as any);
                        })
                    }


                    this.pasteX = undefined;
                    this.pasteY = undefined;
                    this.pastePageNum = undefined;
                }
                else if(this.ctrlKey && keystroke == 'a') {// ctrl c
                   this.selectAllCurrentPageFormField()
                }
                else if (e.keyCode === 8){
                    this.selectedFieldIds.forEach(selectedFieldId =>{
                        this.pdfviewerControl?.formDesignerModule.deleteFormField(selectedFieldId);
                         // delete from array
                         this.signerData.forEach(e => {
                            e.signers.forEach(p => {
                                if (p.formFields?.some(k => k.fieldId == selectedFieldId)){
                                    let formFieldSelect = p.formFields?.find(e => e.fieldId == selectedFieldId);
                                    p.formFields?.splice(p.formFields?.findIndex(s => s.fieldId == selectedFieldId ),1)
                                    if (formFieldSelect.field.name.includes('signature')) p.numOfSign = (p.numOfSign >0) ? (p.numOfSign - 1) : 0

                                }
                            })

                        })
                        if (this.pdfviewerControl?.annotationCollection?.length > 0)this.pdfviewerControl.annotationModule.deleteAnnotationById(this.pdfviewerControl.annotationCollection[0].annotationId)
                    });
                    (this.cmenu as ContextMenuComponent).hideItems(['Cut','Copy','Delete','Rotate 90','Rotate 180','Replicate To Page','Replicate To All Pages']);
                    (this.cmenu as ContextMenuComponent).insertBefore([{
                        text: 'Add Signature',
                        id : 'addSignature-context',
                        items: this.setSigner()
                    }],"Select All");
                }

                // this.pdfviewerControl.magnificationModule.pageRerenderOnMouseWheel()
            })

            document.getElementById("pdfViewer").addEventListener("keypress",(p)=>{
                p.preventDefault()

            })

            document.getElementById("pdfViewer").addEventListener("keyup",(e)=>{
                e.preventDefault()
                    this.ctrlKey = false;
                    this.altCloning = false;

            })

        })

        this.pdfviewerControl.annotationSelectorSettings = this.annotationSelectorSettings as AnnotationSelectorSettingsModel;

    }

    firstLoad = true;

    pdfDoneLoading(params){
        this.zone.runOutsideAngular(()=>{


            // if (this.documentData?.formFieldPositionList?.length > 0){
            //     this.documentData?.formFieldPositionList?.forEach(k=>{
            //         this.pdfviewerControl?.formDesignerModule?.addFormField( k.type.toString() as FormFieldType, k.detail as any);
            //     })

            // }

            //this.pdfviewerControl.enableThumbnail = true;

            this.pageFitClicked();


            // this.pdfviewerControl.annotationModule.formFieldDataChangee = function (args) {
            //         alert(1)
            //         if (args.value && args.name) {
            //         // Store the form field data in localStorage
            //         localStorage.setItem(args.name, args.value);
            //     }
            // };
            if(this.firstLoad) {
                this.setUpFieldForSigner();
                (this.cmenu as ContextMenuComponent).hideItems(['Cut','Copy','Delete','Rotate 90','Rotate 180','Replicate To Page','Replicate To All Pages']);
                (this.cmenu as ContextMenuComponent).insertBefore([{
                    text: 'Add Signature',
                    id : 'addSignature-context',
                    items: this.setSigner()
                }],"Select All");

                this.firstLoad = false;
            }

            // let newFieldSetting = Object.assign({
            //     value: "temp",
            //     type: "Textbox",
            //     name: "Textbox", bounds: { X: -100 , Y: -100, Width: 0, Height: 0 },
            //     pageNumber :1,
            //     backgroundColor : "white",
            //     zIndex: 2,
            //     fontFamily: this.fontFamily,
            //     fontSize: this.fontSize,
            //     color : this.fontColor,
            //     alignment: "center",
            // })
            // this.pdfviewerControl?.formDesignerModule.addFormField( "Textbox" as FormFieldType,
            //     newFieldSetting as TextFieldSettings);

        })

        let pdfViewer = document.getElementById("pdfViewer_viewerContainer");

        pdfViewer.addEventListener("scroll",(e) => {
            if (this.isScroll) return;
            e.preventDefault();
            e.stopPropagation();
            // // console.log(pdfViewer.scrollLeft, pdfViewer.scrollTop)

            pdfViewer.scrollTo(this.scrollX,this.scrollY);

            // // console.log('last jit',this.scrollX,this.scrollY,pdfViewer.scrollLeft, pdfViewer.scrollTop)

            this.isScroll = true;
            return false;
            //  pdfViewer.style.overflowY = "hidden";
            // pdfViewer.style.overflow = "hidden";
            // setTimeout(()=>{
            //     pdfViewer.style.overflow = "auto";
            //     pdfViewer.style.overflowY = "auto";
            // })
            // return false;
            // pdfViewer.scrollTo(0, 0)
        })

    }

    fieldMouseOver(params){
        // if (!this.ctrlKey){


            // let pdfViewer = document.getElementById("pdfViewer_viewerContainer");
            // if(pdfViewer){

            //     // pdfViewer.onscroll = ""
            //     pdfViewer.addEventListener("scroll",(e) => {
            //         e.preventDefault();
            //         e.stopPropagation();
            //         //  pdfViewer.style.overflowY = "hidden";
            //         // pdfViewer.style.overflow = "hidden";
            //         // setTimeout(()=>{
            //         //     pdfViewer.style.overflow = "auto";
            //         //     pdfViewer.style.overflowY = "auto";
            //         // })
            //         // return false;
            //         // pdfViewer.scrollTo(0, 0)
            //     },{passive: false})
            // }

            if(this.selectedFieldIds.length <= 1 ){
                let x = params?.X
                let y = params?.Y
                let x2 = x +  (x/this.zoomValue)*(100-this.zoomValue) ;
                let y2 = y +  (y/this.zoomValue)*(100-this.zoomValue) ;
                let field = this.pdfviewerControl.formFields.find(e => (e.bounds.x + e.bounds.width) >= x2 &&  (e.bounds.y + e.bounds.height)>= y2
                    && x2 >= e.bounds.x && y2 >= e.bounds.y
                    && e.pageNumber == (params.pageIndex + 1))
                if (field){

                    this.pdfviewerControl.formDesignerModule.selectFormField(field?.id)

                    // this.pdfviewerControl?.annotation.addAnnotation(
                    //     "Rectangle",
                    //     {
                    //         offset : {
                    //             x : field?.bounds?.x  ,
                    //             y : field?.bounds?.y ,
                    //         },
                    //         opacity: 1,
                    //         fillColor: '',
                    //         strokeColor: '#0374f6',
                    //         author: 'Guest',
                    //         thickness: 3,
                    //         height: field?.bounds?.height,
                    //         width: field?.bounds?.width,
                    //         annotationSelectorSettings: {
                    //             selectionBorderColor: '#0374f6',
                    //             resizerBorderColor: '#0374f6',
                    //             resizerFillColor: '#0374f6',
                    //             resizerSize: 3,
                    //             // selectionBorderThickness: 3,
                    //             resizerShape: 'Square',
                    //             selectorLineDashArray: [0],
                    //             resizerLocation: AnnotationResizerLocation.Corners ,
                    //             resizerCursorType: null,
                    //         },
                    //         customData:this.selectedFieldIds,
                    //         allowedInteractions: [AllowedInteraction.Move],
                    //         pageNumber:this.pageNum
                    //     } as RectangleSettings ,
                    //     // standardBusinessStampItem?: StandardBusinessStampItem
                    // )
                    // // console.log(this.pdfviewerControl.annotationCollection)
                    // // console.log(this.pdfviewerControl.annotationCollection[0].annotationId)
                    // this.pdfviewerControl.annotationModule.selectAnnotation(this.pdfviewerControl.annotationCollection[0].annotationId);
                }


            }
        // }
    }

    annotationResize(params){
        // alert(1)
        this.zone.runOutsideAngular(()=>{
            let x =  params.annotationBound.x
            let y = params.annotationBound.y

            if (this.selectedFieldIds.length == 1){
                let field = this.pdfviewerControl.formDesignerModule.getFormField(this.selectedFieldIds[0]);
                // // console.log(params.annotationBound)
                // // console.log(field.bounds)
                if (field){
                    this.pdfviewerControl.formDesignerModule.updateFormField(field,{ bounds: {X:x + params.annotationBound.width/2 ,Y:y+params.annotationBound.height/2, Width: params.annotationBound.width,Height: params.annotationBound.height}} as any)
                }

            }
        })

    }

    annotationX = 0;
    annotationY = 0;

    drag = false;

    annotationMove(params){
        this.drag = true;
        this.zone.runOutsideAngular(()=>{
            // // console.log(params)
            // if (this.pdfviewerControl.annotationCollection.length > 0){
            //     this.pdfviewerControl.annotationModule.deleteAnnotationById(this.pdfviewerControl.annotationCollection[0].annotationId);
            // }
            this.annotationX =  params.currentPosition.x
            this.annotationY = params.currentPosition.y
            this.newAnnotationPos = params.currentPosition


            if (this.selectedFieldIds.length == 1){
                let field = this.pdfviewerControl.formDesignerModule.getFormField(this.selectedFieldIds[0]);
                // // console.log(params.annotationBound)
                // // console.log(field.bounds)
                if (field){
                    this.pdfviewerControl.formDesignerModule.updateFormField(field,{ bounds: {X:this.annotationX + field.bounds.width/2 ,Y:this.annotationY+field.bounds.height/2, Width: field.bounds.width,Height: field.bounds.height}} as any)
                }

            }

        })

    }

    annotationUnSelect(params){
        this.zone.runOutsideAngular(()=>{
            if (this.pdfviewerControl.annotationCollection.length > 0){
                this.pdfviewerControl.annotationModule.deleteAnnotationById(this.pdfviewerControl.annotationCollection[0].annotationId);
            }

            if (!this.ctrlKey){
                this.selectedFieldIds = [];
                if(this.selectedFieldId) this.selectedFieldIds.push(this.selectedFieldId)
            }

            (this.cmenu as ContextMenuComponent).hideItems(['Cut','Copy','Delete','Rotate 90','Rotate 180','Replicate To Page','Replicate To All Pages']);
            (this.cmenu as ContextMenuComponent).insertBefore([{
                text: 'Add Signature',
                id : 'addSignature-context',
                items: this.setSigner()
            }],"Select All");
        })
    }

    fieldMouseLeave(params){
    }

    updateFormFieldIds(){
        const formFields2 = this.pdfviewerControl.formFields;
        // formFields.forEach(field => {
        //     if (field.FormField)
        //   // Update field IDs as needed
        // //   field.id = 'newId';
        // });

        this.signerData?.forEach(k =>{
            k.signers?.forEach(s => {
                // formFields.forEach(e =>{
                //     let newFieldBase = this.pdfviewerControl.formDesignerModule.getFormField(e.Key.split("_")[0])
                //     let newField = s.formFields?.find(field => Math.round(e.FormField.lineBound.X) == Math.round(field.field.bounds.x) && Math.round(e.FormField.lineBound.Y) == Math.round(field.field.bounds.y) )
                //     if(newField){
                //         newField.fieldId = e.Key.split("_")[0];
                //     }
                //     else {
                //         // if ()
                //         s.numOfSign = (s.numOfSign ?? 0) + 1
                //         s.formFields?.push({documentId :this.selectedFile,fieldId : newFieldBase.id ,field: newFieldBase})
                //     }

                // })
                s.formFields?.forEach(field => {
                   if (field.documentId == this.selectedFile){
                    //let newField = formFields.find(e => Math.round(e.FormField.lineBound.X) == Math.round(field.field.bounds.x) && Math.round(e.FormField.lineBound.Y) == Math.round(field.field.bounds.y) )
                    let newField2 = formFields2.find(e => Math.round(e.bounds.x) == Math.round(field.position.x) && Math.round(e.bounds.y) == Math.round(field.position.y) && e.pageNumber == field.pageNumber)

                        if (newField2){
                            // field.fieldId = newField.Key.split("_")[0];
                            field.fieldId = newField2.id;
                            field.field = newField2;
                        }

                        if (field?.position?.rotate && Number(field?.position?.rotate) != 0 ) this.setupRotationForField(field.fieldId,field.position.rotate)


                   }

                })

            })
        })
    }

    changeFontFamily(params){
        this.selectedFieldIds.forEach(selectedFieldId => {
            let selectField = this.pdfviewerControl.formDesigner.getFormField(selectedFieldId);
            this.pdfviewerControl.formDesignerModule.updateFormField(selectField,{ fontFamily: params,fontSize: this.fontSize,fontStyle: this.fontStyle} as any)

        })
    }

    changeFontSize(params){
        this.selectedFieldIds.forEach(selectedFieldId => {
            let selectField = this.pdfviewerControl.formDesigner.getFormField(selectedFieldId);
            this.pdfviewerControl.formDesignerModule.updateFormField(selectField,{ fontFamily: this.fontFamily,fontSize: params,fontStyle: this.fontStyle} as any)

            this.signerData.forEach(k => {
                k.signers.forEach(p => {
                    // p.numOfSign = (p.numOfSign ?? 0) + 1
                    let field = p.formFields?.find(l => l.fieldId == selectedFieldId)?.field
                    if (field){
                        p.formFields.splice(p.formFields.findIndex(l => l.fieldId == this.selectedFieldId),1);
                        p.formFields?.push({
                            pageNumber: field.pageNumber,
                            position: {
                                x:Math.round(field?.position?.x ?? 0),
                                y:Math.round(field?.position?.y ?? 0),
                                w:Math.round(field?.position?.w ?? 0),
                                h:Math.round(field?.position?.h ?? 0),
                                rotate: field?.position?.rotate ?? 0 ,
                            },
                            style: {
                                isBold: false,
                                isItalic: false,
                                isUnderline: false,
                                alignment: field.alignment,
                                color: field.color,
                                name : field?.name ?? 'signature',
                                value: field.value,
                                typeId : this.getTypeId(field?.name ?? 'signature'),
                                backgroundColor: field.backgroundColor,
                                fontSize: this.fontSize ?? 8
                            },
                            documentId :this.selectedFile,
                            fieldId : field.id ,
                            field: field
                        })
                    }
                })

            })

        })
    }

    changeFontStyle(params){
        this.selectedFieldIds.forEach(selectedFieldId => {
            let selectField = this.pdfviewerControl.formDesigner.getFormField(selectedFieldId);
            this.pdfviewerControl.formDesignerModule.updateFormField(selectField,{ fontFamily: this.fontFamily,fontSize: this.fontSize,fontStyle: FontStyle.None} as any)
            if (params == 1)  this.isBold = !this.isBold;
            else if (params == 2) this.isItalic = !this.isItalic;
            else if (params == 4)this.isUnderline = !this.isUnderline;

            if (this.isBold) this.pdfviewerControl.formDesignerModule.updateFormField(selectField,{ fontFamily: this.fontFamily,fontSize: this.fontSize,fontStyle: FontStyle.Bold} as any)
            if (this.isItalic) this.pdfviewerControl.formDesignerModule.updateFormField(selectField,{ fontFamily: this.fontFamily,fontSize: this.fontSize,fontStyle: FontStyle.Italic} as any)
            if (this.isUnderline) this.pdfviewerControl.formDesignerModule.updateFormField(selectField,{ fontFamily: this.fontFamily,fontSize: this.fontSize,fontStyle: FontStyle.Underline} as any)

        })
    }


    changeFontColor(params){
        this.selectedFieldIds.forEach(selectedFieldId => {
            let selectField = this.pdfviewerControl.formDesigner.getFormField(selectedFieldId);
            this.pdfviewerControl.formDesignerModule.updateFormField(selectField,{ color: params} as any)

        })
    }

    changeTextAlignment(params){
        this.selectedFieldIds.forEach(selectedFieldId => {
            let selectField = this.pdfviewerControl.formDesigner.getFormField(selectedFieldId);
            this.pdfviewerControl.formDesignerModule.updateFormField(selectField,{ alignment: params} as any)

        })
    }

    onCreatedContext() {
        // if(this.selectedFieldIds.length > 0){
        //     (this.cmenu as ContextMenuComponent).removeItems(['paste-context']);
        // }
        // (this.cmenu as ContextMenuComponent).insertAfter([{text: 'Sort By'}] , 'Refresh');
        // (this.cmenu as ContextMenuComponent).insertBefore([{text: 'Display Settings'}] , 'Personalize');
      }


    beforeOpenContextMenuAndGetPastePos(args:BeforeOpenCloseMenuEventArgs){
            this.pasteX = this.pageMouseX;
            this.pasteY = this.pageMouseY;
            this.pastePageNum = this.pageNum;
        // (this.cmenu as ContextMenuComponent).showItems(['Cut','Copy','Delete','Rotate 90','Rotate 180','Replicate To Page','Replicate To All Pages']);

            // this.setContextMenu();

            // document.getElementById("pdfViewer_context_menu")?.remove()
            // (this.cmenu as ContextMenuComponent).hideItems(['Cut','Copy','Delete','Rotate 90','Rotate 180','Replicate To Page','Replicate To All Pages']);
    }

    pageMouseOver(params){

        this.zone.runOutsideAngular(()=>{
            this.pageMouseX = params.pageX;
            this.pageMouseY = params.pageY;
            this.pageNum = this.pdfviewerControl?.currentPageNumber ;
        })
    }

    setupRotationForField(selectedFieldId, rotationValue ,originalColor?){
        let group = this.signerData.find(e => e.signers.some(p => p.formFields?.some(k => k.fieldId == selectedFieldId)));
        let me = group?.signers?.find(e => e.formFields?.some(k => k.fieldId == selectedFieldId))
        let field = me?.formFields?.find(e => e.fieldId == selectedFieldId);

        let img = document.getElementById(selectedFieldId );
        if (img){
            // // console.log(img)
            img.style.transform = 'rotate(' + rotationValue + 'deg)';
            // img.style.height = 'fit-content'
            img.style.border = 'none';
            img.style.background = '#0000'
            img.style.width = 'auto'
            img.style.height = 'auto'


            img.parentElement.style.backgroundColor = originalColor ?? field?.style?.backgroundColor;
            img.parentElement.style.border = '1px solid black';

            img.parentElement.style.display = 'flex';
            img.parentElement.style.justifyContent = 'space-around';
            img.parentElement.style.alignItems = 'center';

        }
    }

    selectOption(params){
        let name = params.item.properties;
        let id = params.item.properties.id;

        if (id == 'contextItem-rotate90'){
            this.selectedFieldIds.forEach(selectedFieldId =>{

                let group = this.signerData.find(e => e.signers.some(p => p.formFields?.some(k => k.fieldId == selectedFieldId)));
                let me = group?.signers?.find(e => e.formFields?.some(k => k.fieldId == selectedFieldId))
                let field = me?.formFields?.find(e => e.fieldId == selectedFieldId);

                let selectField = this.pdfviewerControl.formDesigner.getFormField(selectedFieldId);
                //this.pdfviewerControl.formDesignerModule.updateFormField(this.pdfviewerControl.formDesigner.getFormField(this.selectedFieldId),{  bounds: {X : selectField.bounds.x + selectField.bounds.width/2   , Y: selectField.bounds.y + selectField.bounds.height/2 , Width: selectField.bounds.height, Height: selectField.bounds.width }} as any)
                //this.pdfviewerControl.formDesignerModule.selectFormField(this.pdfviewerControl.formDesigner.getFormField(this.selectedFieldId))
                this.pdfviewerControl.formDesignerModule.updateFormField(selectField,{  bounds: {X : selectField.bounds.x + selectField.bounds.width/2   , Y: selectField.bounds.y + selectField.bounds.height/2 , Width: selectField.bounds.height, Height: selectField.bounds.width }} as any)
                this.pdfviewerControl.formDesignerModule.selectFormField(selectField);

                if (field){
                    field.position.rotate = field.position.rotate + 90;
                    let temp = field.position.h;
                    field.position.h = field.position.w;
                    field.position.w = temp;

                    this.setupRotationForField(selectedFieldId,field.position.rotate);
                    // this.signerData = [...this.signerData]
                    // let img = document.getElementById(selectedFieldId + "_content_html_element" );

                }
            })
            // this.pendding_rendering_field_html(200);

        }
        else if (id == 'contextItem-rotate180'){
            this.selectedFieldIds.forEach(selectedFieldId =>{

                let group = this.signerData.find(e => e.signers.some(p => p.formFields?.some(k => k.fieldId == selectedFieldId)));
                let me = group?.signers?.find(e => e.formFields?.some(k => k.fieldId == selectedFieldId))
                let field = me?.formFields?.find(e => e.fieldId == selectedFieldId);

                if (field){
                    field.position.rotate = field.position.rotate + 180;
                    // this.signerData = [...this.signerData]
                    this.setupRotationForField(selectedFieldId,field.position.rotate);

                }

                // let selectField = this.pdfviewerControl.formDesigner.getFormField(selectedFieldId);
                // this.pdfviewerControl.formDesignerModule.updateFormField(this.pdfviewerControl.formDesigner.getFormField(this.selectedFieldId),{  bounds: {X : selectField.bounds.x + selectField.bounds.width/2   , Y: selectField.bounds.y + selectField.bounds.height/2 , Width: selectField.bounds.height, Height: selectField.bounds.width }} as any)
                //  this.pdfviewerControl.formDesignerModule.selectFormField(this.pdfviewerControl.formDesigner.getFormField(this.selectedFieldId))
            })
        }
        else if (id == 'selectAll-context'){
            this.selectAllCurrentPageFormField()
        }
        else if (id == 'cut-context'){
            this.copyDatas = []
            this.selectedFieldIds.forEach(selectedFieldId =>{
                let copyDataInfo = this.pdfviewerControl?.formDesignerModule.getFormField(selectedFieldId);
                this.copyDatas.push(copyDataInfo)
                this.pdfviewerControl?.formDesignerModule.deleteFormField(copyDataInfo);
                 // delete from array
                 this.signerData.forEach(e => {
                    e.signers.forEach(p => {
                        if (p.formFields?.some(k => k.fieldId == selectedFieldId)){
                            let formFieldSelect = p.formFields?.find(e => e.fieldId == selectedFieldId);
                            p.formFields?.splice(p.formFields?.findIndex(s => s.fieldId == selectedFieldId ),1)
                            if (formFieldSelect.field.name.includes('signature')) p.numOfSign = (p.numOfSign >0) ? (p.numOfSign - 1) : 0

                        }
                    })

                })
            })
            if (this.pdfviewerControl?.annotationCollection?.length > 0)this.pdfviewerControl.annotationModule.deleteAnnotationById(this.pdfviewerControl.annotationCollection[0].annotationId);

            (this.cmenu as ContextMenuComponent).hideItems(['Cut','Copy','Delete','Rotate 90','Rotate 180','Replicate To Page','Replicate To All Pages']);
            (this.cmenu as ContextMenuComponent).insertBefore([{
                text: 'Add Signature',
                id : 'addSignature-context',
                items: this.setSigner()
            }],"Select All");
        }
        else if (id == 'paste-context'){

            let zoomValue = parseFloat(this.pdfviewerControl.zoomValue.toString().replace("%",""));

            let x = (this.pasteX )
            let y = (this.pasteY )

            let x2 = x +  (x/zoomValue)*(100-zoomValue) ;
            let y2 = y +  (y/zoomValue)*(100-zoomValue) ;

            let x1 = (this.copyDatas.length > 1) ? this.firstMultiAnnotationX : this.copyDatas[0].bounds.x;
            let y1 = (this.copyDatas.length > 1) ? this.firstMultiAnnotationY : this.copyDatas[0].bounds.y ;

            // let x2 = (this.pasteX )
            // let y2 = (this.pasteX )

            this.copyDatas.forEach(copyDataInfo => {
                // let data = this.pdfviewerControl.formDesignerModule.getFormField(e);
                let newPos = this.calculateNewPoint(x1,y1,x2,y2,copyDataInfo.bounds.x,copyDataInfo.bounds.y);

                // if(this.altCloning){
                this.pdfviewerControl?.formDesignerModule.addFormField(copyDataInfo.formFieldAnnotationType as FormFieldType,
                    Object.assign({
                        // type: copyDataInfo.formFieldAnnotationType,
                        name: copyDataInfo.name,
                        value: copyDataInfo.value,
                        bounds: { X: newPos.x , Y: newPos.y , Width: copyDataInfo.bounds.width, Height: copyDataInfo.bounds.height },
                        pageNumber : this.pastePageNum,
                        backgroundColor :copyDataInfo.backgroundColor,
                        fontFamily: copyDataInfo.fontFamily,
                        fontSize: copyDataInfo.fontSize,
                        color : copyDataInfo.color,
                        alignment:  copyDataInfo.alignment,
                        zIndex: 2
                    }) as any);
                this.signerData.forEach(e => {
                        e.signers.forEach(p => {
                            if (p.formFields?.some(k => k.fieldId == copyDataInfo.id)){
                                p.numOfSign = (p.numOfSign ?? 0) + 1
                                p.formFields?.push({
                                    pageNumber: this.pdfviewerControl.formFields[this.pdfviewerControl.formFields.length-1].pageNumber,
                                    position: {
                                        x:Math.round(this.pdfviewerControl.formFields[this.pdfviewerControl.formFields.length-1].bounds.x),
                                        y:Math.round(this.pdfviewerControl.formFields[this.pdfviewerControl.formFields.length-1].bounds.y),
                                        w:Math.round(this.pdfviewerControl.formFields[this.pdfviewerControl.formFields.length-1].bounds.width),
                                        h:Math.round(this.pdfviewerControl.formFields[this.pdfviewerControl.formFields.length-1].bounds.height),
                                        rotate: 0,
                                    },
                                    style: {
                                        isBold: false,
                                        isItalic: false,
                                        isUnderline: false,
                                        alignment: this.pdfviewerControl.formFields[this.pdfviewerControl.formFields.length-1].alignment,
                                        color: this.pdfviewerControl.formFields[this.pdfviewerControl.formFields.length-1].color,
                                        name : 'signature',
                                        value: this.pdfviewerControl.formFields[this.pdfviewerControl.formFields.length-1].value,
                                        typeId : 1,
                                        backgroundColor: this.pdfviewerControl.formFields[this.pdfviewerControl.formFields.length-1].backgroundColor,
                                        fontSize: this.pdfviewerControl.formFields[this.pdfviewerControl.formFields.length-1].fontSize ?? 8
                                    },
                                    documentId :this.selectedFile,
                                    fieldId : this.pdfviewerControl.formFields[this.pdfviewerControl.formFields.length-1].id ,
                                    field: this.pdfviewerControl.formFields[this.pdfviewerControl.formFields.length-1]
                                })
                            }
                        })

                    })
                // }
                // this.pdfviewerControl.formDesignerModule.updateFormField(this.pdfviewerControl.formDesigner.getFormField(e),{  bounds: {X : newPos.x + data.bounds.width/2   , Y: newPos.y + data.bounds.height/2 , Width: data.bounds.width, Height: data.bounds.height }} as any)

            })

            this.firstMultiAnnotationX = this.newAnnotationPos.x ;
            this.firstMultiAnnotationY = this.newAnnotationPos.y ;

            this.newAnnotationPos = undefined;

            this.pasteX = undefined;
            this.pasteY = undefined;
            this.pastePageNum = undefined;

            this.pendding_rendering_field_html(200);
            // this.copyDatas = [];
        }
        else if (id == 'delete-context'){
            this.selectedFieldIds.forEach(selectedFieldId =>{
                this.pdfviewerControl?.formDesignerModule.deleteFormField(selectedFieldId);
                 // delete from array

                 this.signerData.forEach(e => {
                    e.signers.forEach(p => {
                        if (p.formFields?.some(k => k.fieldId == selectedFieldId)){
                            let formFieldSelect = p.formFields?.find(e => e.fieldId == selectedFieldId);
                            p.formFields?.splice(p.formFields?.findIndex(s => s.fieldId == selectedFieldId ),1)
                            if (formFieldSelect.field.name.includes('signature')) p.numOfSign = (p.numOfSign >0) ? (p.numOfSign - 1) : 0

                        }
                    })

                })
                if (this.pdfviewerControl?.annotationCollection?.length > 0)this.pdfviewerControl.annotationModule.deleteAnnotationById(this.pdfviewerControl.annotationCollection[0].annotationId)
            });
            // this.pdfviewerControl?.formDesignerModule.deleteFormField(this.selectedFieldId);
            // if (this.pdfviewerControl?.annotationCollection?.length > 0)this.pdfviewerControl.annotationModule.deleteAnnotationById(this.pdfviewerControl.annotationCollection[0].annotationId)
            // this.selectedFieldId = undefined;
            (this.cmenu as ContextMenuComponent).hideItems(['Cut','Copy','Delete','Rotate 90','Rotate 180','Replicate To Page','Replicate To All Pages']);
            (this.cmenu as ContextMenuComponent).insertBefore([{
                text: 'Add Signature',
                id : 'addSignature-context',
                items: this.setSigner()
            }],"Select All");
        }
        else if (id == 'copy-context'){
            this.copyDatas = []
            this.selectedFieldIds.forEach(selectedFieldId =>{
                let copyDataInfo = this.pdfviewerControl?.formDesignerModule.getFormField(selectedFieldId);
                this.copyDatas.push(copyDataInfo)
            })
            // this.copyData = this.pdfviewerControl?.formDesignerModule.getFormField(this.selectedFieldId);
        }

        else if (id == 'contextItem-cloneToAllPages'){
            this.replicateToPage();
        }
        else if (id == 'contextItem-cloneToPage'){
            this.choosePage.show();
        }
        else if (id.includes("signature-")){
            let group = this.signerData.find(e => e.signers.some(p => p.id == id.split("-")[1]));
            let p = group?.signers?.find(e => e.id == id.split("-")[1] )

            let zoomValue = parseFloat(this.pdfviewerControl.zoomValue.toString().replace("%",""));

            let x = (this.pasteX )
            let y = (this.pasteY )

            let x2 = x +  (x/zoomValue)*(100-zoomValue) ;
            let y2 = y +  (y/zoomValue)*(100-zoomValue) ;

            let width = 160;
            let height = 55;


            let newFieldSetting = Object.assign({
                value: p.fullName,
                type: "Textbox",
                name: p.name, bounds: { X: x2 , Y: y2 , Width: width, Height: height },
                pageNumber :this.pastePageNum,
                backgroundColor : p.backgroundColor,
                zIndex:2,
                fontFamily: this.fontFamily,
                fontSize: this.fontSize,
                color : this.fontColor,
                alignment: "center",
                fontStyle: this.fontStyle
            },this.applyDataForFormField("signature", p))
            this.pdfviewerControl?.formDesignerModule.addFormField("Textbox" as FormFieldType,
                newFieldSetting as any);
            if(this.getType("signature") == "SignatureField"){
                p.numOfSign = (p.numOfSign ?? 0) + 1;
            }
            p.formFields?.push({
                pageNumber: this.pdfviewerControl.formFields[this.pdfviewerControl.formFields.length-1].pageNumber,
                position: {
                    x:Math.round(this.pdfviewerControl.formFields[this.pdfviewerControl.formFields.length-1].bounds.x),
                    y:Math.round(this.pdfviewerControl.formFields[this.pdfviewerControl.formFields.length-1].bounds.y),
                    w:Math.round(this.pdfviewerControl.formFields[this.pdfviewerControl.formFields.length-1].bounds.width),
                    h:Math.round(this.pdfviewerControl.formFields[this.pdfviewerControl.formFields.length-1].bounds.height),
                    rotate: 0,
                },
                style: {
                    isBold: false,
                    isItalic: false,
                    isUnderline: false,
                    alignment: this.pdfviewerControl.formFields[this.pdfviewerControl.formFields.length-1].alignment,
                    color: this.pdfviewerControl.formFields[this.pdfviewerControl.formFields.length-1].color,
                    name : 'signature',
                    value: this.pdfviewerControl.formFields[this.pdfviewerControl.formFields.length-1].value,
                    typeId : 1,
                    backgroundColor: this.pdfviewerControl.formFields[this.pdfviewerControl.formFields.length-1].backgroundColor,
                    fontSize:  this.pdfviewerControl.formFields[this.pdfviewerControl.formFields.length-1]?.fontSize ?? 8
                },
                documentId :this.selectedFile,
                fieldId : this.pdfviewerControl.formFields[this.pdfviewerControl.formFields.length-1].id ,
                field: this.pdfviewerControl.formFields[this.pdfviewerControl.formFields.length-1]
            })

            this.pendding_rendering_field_html(200);

        }
    }

    replicateToPage(pageNumber?){
        if (!pageNumber){
            pageNumber = [];
            for(var i = 0;i <= this.pdfviewerControl.pageCount;i++){
                if (!pageNumber.some(p => p == i)) pageNumber.push(i)
            }
        }
        // this.spinnerService.show();
        const promises: Promise<void>[] = [];
        this.selectedFieldIds.forEach(selectedFieldId => {
            const promise = new Promise<void>((resolve) => {
                let selectedField = this.pdfviewerControl?.formDesignerModule.getFormField(selectedFieldId);
                pageNumber.forEach(pageNum => {
                    if ( selectedField.pageNumber != pageNum ){
                        this.pdfviewerControl?.formDesignerModule.addFormField( selectedField.formFieldAnnotationType as FormFieldType,
                            Object.assign({
                                value: selectedField.value,
                                name: selectedField.name,
                                bounds: { X: selectedField.bounds.x , Y: selectedField.bounds.y , Width: selectedField.bounds.width, Height: selectedField.bounds.height },
                                pageNumber :pageNum,
                                backgroundColor : selectedField.backgroundColor,
                                zIndex:2,
                                fontFamily: selectedField.fontFamily,
                                fontSize: selectedField.fontSize,
                                color : selectedField.color,
                                alignment:  selectedField.alignment,
                            }) as any);

                        let signerDataList = this.signerData?.find(k => k.signers?.some(l => l.formFields?.some(s => s.fieldId == selectedFieldId)))?.signers
                        let signerField = signerDataList?.find(k => k.formFields?.some(s => s.fieldId == selectedFieldId));

                        signerField?.formFields?.push({
                            pageNumber: this.pdfviewerControl.formFields[this.pdfviewerControl.formFields.length-1].pageNumber,
                            position: {
                                x:Math.round(this.pdfviewerControl.formFields[this.pdfviewerControl.formFields.length-1].bounds.x),
                                y:Math.round(this.pdfviewerControl.formFields[this.pdfviewerControl.formFields.length-1].bounds.y),
                                w:Math.round(this.pdfviewerControl.formFields[this.pdfviewerControl.formFields.length-1].bounds.width),
                                h:Math.round(this.pdfviewerControl.formFields[this.pdfviewerControl.formFields.length-1].bounds.height),
                                rotate: signerField?.position?.rotate ?? 0,
                            },
                            style: {
                                isBold: false,
                                isItalic: false,
                                isUnderline: false,
                                alignment: this.pdfviewerControl.formFields[this.pdfviewerControl.formFields.length-1].alignment,
                                color: this.pdfviewerControl.formFields[this.pdfviewerControl.formFields.length-1].color,
                                name : this.pdfviewerControl.formFields[this.pdfviewerControl.formFields.length-1].name,
                                value: this.pdfviewerControl.formFields[this.pdfviewerControl.formFields.length-1].value,
                                typeId : 1,
                                backgroundColor: this.pdfviewerControl.formFields[this.pdfviewerControl.formFields.length-1].backgroundColor,
                                fontSize : this.pdfviewerControl.formFields[this.pdfviewerControl.formFields.length-1].fontSize ?? 8
                            },
                            documentId :this.selectedFile,
                            fieldId : this.pdfviewerControl.formFields[this.pdfviewerControl.formFields.length-1].id ,
                            field: this.pdfviewerControl.formFields[this.pdfviewerControl.formFields.length-1]
                        })

                        signerField.numOfSign = (signerField?.numOfSign ?? 0) + 1
                    }
                })
                // for (var i = (pageNumber ? this.pageNum : 0); i < (pageNumber ? pageNumber : this.pdfviewerControl?.pageCount); i++){

                // }
                resolve();
            });
            promises.push(promise);
        })
        // Promise.all(promises)
        // .then(() => {
        //     this.spinnerService.hide();
        // })
    }

    moveFormField(params){
        this.zone.runOutsideAngular(()=>{
            let copyData = this.pdfviewerControl.formDesignerModule.getFormField(this.selectedFieldId)
            if (this.altCloning){
                this.pdfviewerControl?.formDesignerModule?.addFormField( copyData.formFieldAnnotationType as FormFieldType,
                    Object.assign({
                        // type: copyData.formFieldAnnotationType,
                        name: copyData.name,
                        value: copyData.value,
                        bounds: { X: params.previousPosition.X - params.previousPosition.Width/2, Y: params.previousPosition.Y - params.previousPosition.Height/2, Width: params.previousPosition.Width, Height: params.previousPosition.Height },
                        pageNumber :copyData.pageNumber,
                        backgroundColor : copyData.backgroundColor,
                        zIndex:2,
                        fontFamily: copyData.fontFamily,
                        fontSize: copyData.fontSize,
                        color : copyData.color,
                        alignment:  copyData.alignment,
                    }) as any)
                this.signerData.forEach(e => {
                    e.signers.forEach(p => {
                        if (!p.formFields?.some(k => k.fieldId == copyData.id)){
                            p.numOfSign = (p.numOfSign ?? 0) + 1
                            p.formFields?.push({
                                pageNumber: this.pdfviewerControl.formFields[this.pdfviewerControl.formFields.length-1].pageNumber,
                                position: {
                                    x:Math.round(this.pdfviewerControl.formFields[this.pdfviewerControl.formFields.length-1].bounds.x),
                                    y:Math.round(this.pdfviewerControl.formFields[this.pdfviewerControl.formFields.length-1].bounds.y),
                                    w:Math.round(this.pdfviewerControl.formFields[this.pdfviewerControl.formFields.length-1].bounds.width),
                                    h:Math.round(this.pdfviewerControl.formFields[this.pdfviewerControl.formFields.length-1].bounds.height),
                                    rotate: p.formFields?.find(s => s.fieldId == copyData.id)?.position?.rotate ?? 0,
                                },
                                style: {
                                    isBold: false,
                                    isItalic: false,
                                    isUnderline: false,
                                    alignment: this.pdfviewerControl.formFields[this.pdfviewerControl.formFields.length-1].alignment,
                                    color: this.pdfviewerControl.formFields[this.pdfviewerControl.formFields.length-1].color,
                                    name : 'signature',
                                    value: this.pdfviewerControl.formFields[this.pdfviewerControl.formFields.length-1].value,
                                    typeId : 1,
                                    backgroundColor: this.pdfviewerControl.formFields[this.pdfviewerControl.formFields.length-1].backgroundColor,
                                    fontSize: this.pdfviewerControl.formFields[this.pdfviewerControl.formFields.length-1].fontSize ?? 8
                                },
                                documentId :this.selectedFile,
                                fieldId : this.pdfviewerControl.formFields[this.pdfviewerControl.formFields.length-1].id ,
                                field: this.pdfviewerControl.formFields[this.pdfviewerControl.formFields.length-1]
                            })
                        }
                    })

                })
            }
            else {
                this.signerData.forEach(e => {
                    e.signers.forEach(p => {
                        // p.numOfSign = (p.numOfSign ?? 0) + 1
                        let field = p.formFields.find(l => l.fieldId == this.selectedFieldId)?.field
                        if (field){
                            let data = p.formFields.find(l => l.fieldId == this.selectedFieldId);
                            p.formFields.splice(p.formFields.findIndex(l => l.fieldId == this.selectedFieldId),1);
                            p.formFields?.push({
                                pageNumber: field.pageNumber,
                                position: {
                                    x:Math.round(field.bounds.x),
                                    y:Math.round(field.bounds.y),
                                    w:Math.round(field.bounds.width),
                                    h:Math.round(field.bounds.height),
                                    rotate: data?.position?.rotate ?? 0,
                                },
                                style: {
                                    isBold: false,
                                    isItalic: false,
                                    isUnderline: false,
                                    alignment: field?.alignment,
                                    color: field?.color,
                                    name : 'signature',
                                    value: field?.value,
                                    typeId : 1,
                                    backgroundColor: field?.backgroundColor,
                                    fontSize: field?.fontSize ?? 8
                                },
                                documentId :this.selectedFile,
                                fieldId : field.id ,
                                field: field
                            })
                        }
                    })
                })
            }
        })
    }

    addToCenterOfCurrentPage(params){

        // if (params != "signature") return;
        let zoomValue = parseFloat(this.pdfviewerControl.zoomValue.toString().replace("%",""));
                    let x = (250)
                    let y = (250 )

                    let width = 160;
                    let height = 55;

                    let newx = (x/100)*zoomValue + (x/zoomValue)*(100-zoomValue) ;
                    let newy = (y/100)*zoomValue + (y/zoomValue)*(100-zoomValue) ;

                    let multiX = 0;
                    let multiY = 0

                    let distance = 25;
        this.selectedSigners.forEach((p,i) => {
            this.createToCenter(p,params,newx,newy,width,height,distance)

        })
    }

    createToCenter(p,params,newx,newy,width,height,distance){
        if (this.pdfviewerControl.formFields.some(e => e.bounds.x == newx && e.bounds.y == newy)){
            let data = this.pdfviewerControl.formFields.find(e => e.bounds.x == newx && e.bounds.y == newy);
            if (data) return this.createToCenter(p,params,newx,newy + data.bounds.height + distance,data.bounds.width,data.bounds.height,distance);
        }

        this.pdfviewerControl?.formDesignerModule.addFormField("Textbox" as FormFieldType,
            Object.assign({
                // value: p.name,
                type: "Textbox",
                name: p.name, bounds: { X: newx , Y: newy, Width: width, Height: height },
                pageNumber :this.pdfviewerControl.currentPageNumber,
                backgroundColor : p.backgroundColor,
                zIndex:2,
                fontFamily: this.fontFamily,
                fontSize: this.fontSize,
                color : this.fontColor,
                alignment: "center",
            },this.applyDataForFormField(params, p)) as any);
        if (this.getType(params.toString()) == "SignatureField"){
            p.numOfSign = (p.numOfSign ?? 0) + 1

        }
        p.formFields?.push({
            pageNumber: this.pdfviewerControl.formFields[this.pdfviewerControl.formFields.length-1].pageNumber,
            position: {
                x:Math.round(this.pdfviewerControl.formFields[this.pdfviewerControl.formFields.length-1].bounds.x),
                y:Math.round(this.pdfviewerControl.formFields[this.pdfviewerControl.formFields.length-1].bounds.y),
                w:Math.round(this.pdfviewerControl.formFields[this.pdfviewerControl.formFields.length-1].bounds.width),
                h:Math.round(this.pdfviewerControl.formFields[this.pdfviewerControl.formFields.length-1].bounds.height),
                rotate: 0,
            },
            style: {
                isBold: false,
                isItalic: false,
                isUnderline: false,
                alignment: this.pdfviewerControl.formFields[this.pdfviewerControl.formFields.length-1].alignment,
                color: this.pdfviewerControl.formFields[this.pdfviewerControl.formFields.length-1].color,
                name : params,
                value: this.pdfviewerControl.formFields[this.pdfviewerControl.formFields.length-1].value,
                typeId : 1,
                backgroundColor: this.pdfviewerControl.formFields[this.pdfviewerControl.formFields.length-1].backgroundColor,
                fontSize : this.pdfviewerControl.formFields[this.pdfviewerControl.formFields.length-1].fontSize ?? 8
            },
            documentId :this.selectedFile,
            fieldId : this.pdfviewerControl.formFields[this.pdfviewerControl.formFields.length-1].id ,
            field: this.pdfviewerControl.formFields[this.pdfviewerControl.formFields.length-1]
        })
    }

    drop = false;

    dropEvent(e: DropEventArgs, pageNum,pageElem){
                e.droppedElement.remove();
                // this.pdfviewerControl.enableFormDesigner = true
                //e.droppedElement.querySelector('.drag-text').textContent = 'Dropped';
                this.drop = true;
                if (this.pageMouseX && this.pageMouseY && pageNum){
                    let zoomValue = this.zoomValue;
                    let x = (this.pageMouseX )
                    let y = (this.pageMouseY )

                    let width = 160;
                    let height = 55;

                    let newx = x +  (x/zoomValue)*(100-zoomValue) ;
                    let newy = y +  (y/zoomValue)*(100-zoomValue) ;

                    let multiX = 0;
                    let multiY = 0

                    let distance = 25;

                    if (["title","datesigned"].includes(e.dragData.draggedElement.id)){

                            this.selectedSigners.forEach((p,i) => {
                                let newFieldSetting = Object.assign({
                                    type: "Textbox",
                                    name :e.dragData.draggedElement.id, bounds: { X: newx , Y: newy + i*height + i* distance, Width: width, Height: height },
                                    pageNumber :pageNum,
                                    backgroundColor : p.backgroundColor,
                                    zIndex:2,
                                    fontFamily: this.fontFamily,
                                    fontSize: this.fontSize,
                                    color : this.fontColor,
                                    alignment: "center",
                                    fontStyle: this.fontStyle
                                },this.applyDataForFormField(e.dragData.draggedElement.id, p))
                                this.pdfviewerControl?.formDesignerModule.addFormField("Textbox" as FormFieldType,
                                    newFieldSetting as any);
                                if (i == 0){
                                    multiX = newx;
                                    multiY = newy;
                                }
                                // if(this.getType(e.dragData.draggedElement.id.toString()) == "SignatureField"){
                                //     p.numOfSign = (p.numOfSign ?? 0) + 1;
                                // }

                                p.formFields?.push({
                                    pageNumber: this.pdfviewerControl.formFields[this.pdfviewerControl.formFields.length-1].pageNumber,
                                    position: {
                                        x:Math.round(this.pdfviewerControl.formFields[this.pdfviewerControl.formFields.length-1].bounds.x),
                                        y:Math.round(this.pdfviewerControl.formFields[this.pdfviewerControl.formFields.length-1].bounds.y),
                                        w:Math.round(this.pdfviewerControl.formFields[this.pdfviewerControl.formFields.length-1].bounds.width),
                                        h:Math.round(this.pdfviewerControl.formFields[this.pdfviewerControl.formFields.length-1].bounds.height),
                                        rotate: 0,
                                    },
                                    style: {
                                        isBold: false,
                                        isItalic: false,
                                        isUnderline: false,
                                        alignment: this.pdfviewerControl.formFields[this.pdfviewerControl.formFields.length-1].alignment,
                                        color: this.pdfviewerControl.formFields[this.pdfviewerControl.formFields.length-1].color,
                                        name : e.dragData.draggedElement.id,
                                        value: this.pdfviewerControl.formFields[this.pdfviewerControl.formFields.length-1].value,
                                        typeId :e.dragData.draggedElement.id == "title" ? 3 :  e.dragData.draggedElement.id == "datesigned" ? 4 : 1,
                                        backgroundColor: this.pdfviewerControl.formFields[this.pdfviewerControl.formFields.length-1].backgroundColor,
                                        fontSize : this.pdfviewerControl.formFields[this.pdfviewerControl.formFields.length-1].fontSize ?? 8
                                    },
                                    documentId :this.selectedFile,
                                    fieldId : this.pdfviewerControl.formFields[this.pdfviewerControl.formFields.length-1].id ,
                                    field: this.pdfviewerControl.formFields[this.pdfviewerControl.formFields.length-1]
                                })
                            })
                    }
                    else {

                        if ( this.dragFromTool){
                            if (this.selectedSigners.length <=0) return this.notify.warn(this.l("PleaseSelectSigner"))
                            this.selectedSigners.forEach((p,i) => {
                                let newFieldSetting = Object.assign({
                                    value: p.fullName,
                                    type: "Textbox",
                                    name: p.name, bounds: { X: newx , Y: newy + i*height + i* distance, Width: width, Height: height },
                                    pageNumber :pageNum,
                                    backgroundColor : p.backgroundColor,
                                    zIndex:2,
                                    fontFamily: this.fontFamily,
                                    fontSize: this.fontSize,
                                    color : this.fontColor,
                                    alignment: "center",
                                    fontStyle: this.fontStyle
                                },this.applyDataForFormField(e.dragData.draggedElement.id, p))
                                this.pdfviewerControl?.formDesignerModule.addFormField("Textbox" as FormFieldType,
                                    newFieldSetting as any);
                                if (i == 0){
                                    multiX = newx;
                                    multiY = newy;
                                }
                                if(this.getType(e.dragData.draggedElement.id.toString()) == "SignatureField"){
                                    p.numOfSign = (p.numOfSign ?? 0) + 1;
                                }
                                p.formFields?.push({
                                    pageNumber: this.pdfviewerControl.formFields[this.pdfviewerControl.formFields.length-1].pageNumber,
                                    position: {
                                        x:Math.round(this.pdfviewerControl.formFields[this.pdfviewerControl.formFields.length-1].bounds.x),
                                        y:Math.round(this.pdfviewerControl.formFields[this.pdfviewerControl.formFields.length-1].bounds.y),
                                        w:Math.round(this.pdfviewerControl.formFields[this.pdfviewerControl.formFields.length-1].bounds.width),
                                        h:Math.round(this.pdfviewerControl.formFields[this.pdfviewerControl.formFields.length-1].bounds.height),
                                        rotate: 0,
                                    },
                                    style: {
                                        isBold: false,
                                        isItalic: false,
                                        isUnderline: false,
                                        alignment: this.pdfviewerControl.formFields[this.pdfviewerControl.formFields.length-1].alignment,
                                        color: this.pdfviewerControl.formFields[this.pdfviewerControl.formFields.length-1].color,
                                        name : 'signature',
                                        value: this.pdfviewerControl.formFields[this.pdfviewerControl.formFields.length-1].value,
                                        typeId : 1,
                                        backgroundColor: this.pdfviewerControl.formFields[this.pdfviewerControl.formFields.length-1].backgroundColor,
                                        fontSize : this.pdfviewerControl.formFields[this.pdfviewerControl.formFields.length-1].fontSize ?? 8
                                    },
                                    documentId :this.selectedFile,
                                    fieldId : this.pdfviewerControl.formFields[this.pdfviewerControl.formFields.length-1].id ,
                                    field: this.pdfviewerControl.formFields[this.pdfviewerControl.formFields.length-1]
                                })
                            })

                        }
                        else {
                            this.dragSigners.forEach((p,i)=>{
                                let newFieldSetting = Object.assign({
                                    value: p.fullName,
                                    type: "Textbox",
                                    name: p.name, bounds: { X: newx , Y: newy + i*height + i* distance, Width: width, Height: height },
                                    pageNumber :pageNum,
                                    backgroundColor : p.backgroundColor,
                                    zIndex: 2,
                                    fontFamily: this.fontFamily,
                                    fontSize: this.fontSize,
                                    color : this.fontColor,
                                    alignment: "center",
                                },this.applyDataForFormField(e.dragData.draggedElement.id, p))
                                this.pdfviewerControl?.formDesignerModule.addFormField( "Textbox" as FormFieldType,
                                    newFieldSetting as any);
                                if (i == 0){
                                    multiX = newx;
                                    multiY = newy;
                                }
                                if(this.getType(e.dragData.draggedElement.id.toString()) == "SignatureField"){
                                    p.numOfSign = (p.numOfSign ?? 0) + 1;
                                }
                                p.formFields?.push({
                                    pageNumber: this.pdfviewerControl.formFields[this.pdfviewerControl.formFields.length-1].pageNumber,
                                    position: {
                                        x:Math.round(this.pdfviewerControl.formFields[this.pdfviewerControl.formFields.length-1].bounds.x),
                                        y:Math.round(this.pdfviewerControl.formFields[this.pdfviewerControl.formFields.length-1].bounds.y),
                                        w:Math.round(this.pdfviewerControl.formFields[this.pdfviewerControl.formFields.length-1].bounds.width),
                                        h:Math.round(this.pdfviewerControl.formFields[this.pdfviewerControl.formFields.length-1].bounds.height),
                                        rotate: 0,
                                    },
                                    style: {
                                        isBold: false,
                                        isItalic: false,
                                        isUnderline: false,
                                        alignment: this.pdfviewerControl.formFields[this.pdfviewerControl.formFields.length-1].alignment,
                                        color: this.pdfviewerControl.formFields[this.pdfviewerControl.formFields.length-1].color,
                                        name : 'signature',
                                        value: this.pdfviewerControl.formFields[this.pdfviewerControl.formFields.length-1].value,
                                        typeId : 1,
                                        backgroundColor: this.pdfviewerControl.formFields[this.pdfviewerControl.formFields.length-1].backgroundColor,
                                        fontSize : this.pdfviewerControl.formFields[this.pdfviewerControl.formFields.length-1].fontSize ?? 8
                                    },
                                    documentId :this.selectedFile,
                                    fieldId : this.pdfviewerControl.formFields[this.pdfviewerControl.formFields.length-1].id ,
                                    field: this.pdfviewerControl.formFields[this.pdfviewerControl.formFields.length-1]
                                })
                            })

                        }

                    }
                    // let dragElement: HTMLElement = document.getElementById('form_field_'+ this.pdfviewerControl.formFields[this.pdfviewerControl.formFields.length -1]?.id +'_content_html_element');
                    // let draggable: Draggable = new Draggable(dragElement,{ clone: true,tapHoldThreshold:0,dragStart : (e) => this.dragEvent(e),helper: (e)=> this.customCloneElement(e)} );
                    // // form_field_UGPfX_content_html_element
                    this.pdfviewerControl.formDesignerModule.selectFormField(this.pdfviewerControl.formFields[this.pdfviewerControl.formFields.length -1]?.id);
                    this.selecting = false;
                    this.selectedFieldId = this.pdfviewerControl.formFields[this.pdfviewerControl.formFields.length -1]?.id;
                    if (!this.selectedFieldIds.some(e => e == this.pdfviewerControl.formFields[this.pdfviewerControl.formFields.length -1]?.id)) this.selectedFieldIds.push(this.pdfviewerControl.formFields[this.pdfviewerControl.formFields.length -1]?.id)

                    this.dragSigners = [];

                }
                this.pageMouseX = undefined;
                this.pageMouseY = undefined;
                this.pageNum = undefined;

                this.pendding_rendering_field_html(200);
    }

    applyDataForFormField(id,p){
        switch(id){
            case "signature":
                return {name: "signature" , value : p.fullName };
            case "name":
                return {name: id , value : p.fullName};
            case "title":
                return {name: id , value : p.title};
            case "datesigned":
                return {name: id, format:  'dd/MMM/yyyy',placeholder : 'dd/MMM/yyyy' ,value: 'dd/MMM/yyyy'};
            case "company":
                 return {name: id, value : p.company };
            case "text":
                return {name: id };
            default :
                return {name: "signature", value : p.fullName };
        }
    }

        menuItems2: MenuItemModel[] = [
        // {
        //     text: 'Select All',
        //     id : 'selectAll-context'
        // },
        {
            text: 'Cut',
            id : 'cut-context'
        },
        {
            text: 'Copy',
            id : 'copy-context'
        },
        // {
        //     text: 'Paste',
        //     id : 'paste-context'
        // },
        {
            text: 'Delete',
            id : 'delete-context'
        },
        {
            separator: true
        },
        {id :'contextItem-rotate90',text : "Rotate 90"},
        {id :'contextItem-rotate180',text : "Rotate 180"},
        {id :'contextItem-cloneToPage',text : "Replicate To Page"},
        {id :'contextItem-cloneToAllPages',text : "Replicate To All Pages"},
    ];

    selectedFieldId = "";
    selectedFieldIds = [];

    openContext(){
        // setTimeout(()=>{
        //     if (this.selectedFieldIds.length > 0) (this.cmenu as ContextMenuComponent).showItems(['Cut','Copy','Delete','Rotate 90','Rotate 180','Replicate To Page','Replicate To All Pages']);
        //     else (this.cmenu as ContextMenuComponent).hideItems(['Cut','Copy','Delete','Rotate 90','Rotate 180','Replicate To Page','Replicate To All Pages']);
        // },500);
        // (this.cmenu as ContextMenuComponent).showItems(['Cut','Copy','Delete','Rotate 90','Rotate 180','Replicate To Page','Replicate To All Pages']);
    }

    // fieldEnter(params){
    //     (this.cmenu as ContextMenuComponent).showItems(['Cut','Copy','Delete','Rotate 90','Rotate 180','Replicate To Page','Replicate To All Pages']);
    // }

    itemRender(args:MenuEventArgs ) {
        if (args.item.id.includes("signature")) {
            //   args.element.classList.add('bg-transparent');
            args.element.innerHTML = '';
            args.element.appendChild(this.createElem(args.item));
        }
     }

     createElem(signer){
        let header: HTMLElement = document.createElement('div');
        header.classList.add("signer-context");
        let img = document.createElement('img')
        img.setAttribute('src',signer?.imgUrl)
        img.classList.add('ellipse-2068')
        img.style.height = '100%'
        img.style.borderRadius = '50%';
        img.style.borderStyle = 'solid';
        img.style.borderColor = '#28c0f9';
        img.style.borderWidth = '1px';
        img.style.flexShrink = '0';
        img.style.width = '32px';
        img.style.height = '32px';

        header.appendChild(img)
        let name = document.createElement('p')
        name.innerHTML = signer?.fullName,
        name.style.marginLeft = "4px"

        header.style.height = '36px'
        header.style.display = 'flex'
        header.appendChild(img)
        header.appendChild(name)
        return header;
     }

    setSigner(){
        let data = [];
        this.signerData.forEach(e => {
            e.signers.forEach(k => {
                data.push(Object.assign({
                        text: k.fullName,
                        id : k.id,
                    },k,{id : "signature-"+k.id}))
            })
        })
        return data;
    }

    selecting = false; // fix: khi chọn field thì không có unselect
    resizeField(params){
        this.selecting = false;
    }

    isUnSelectedField:boolean = false; // fix: chạy 2 lần
    unselectFormField(params){

        //fix unselect 2 lần
        // if(this.isUnSelectedField == false) {
        //     this.isUnSelectedField = true; return;
        // }
        // this.isUnSelectedField = false;

        // if (this.selecting){ // fix khi selected
        //     this.selecting = false; return;
        // }


        // this.selectedFieldId = undefined;
        // this.selectedFieldIds = [];
        (this.cmenu as ContextMenuComponent).hideItems(['Cut','Copy','Delete','Rotate 90','Rotate 180','Replicate To Page','Replicate To All Pages']);
        (this.cmenu as ContextMenuComponent).insertBefore([{
            text: 'Add Signature',
            id : 'addSignature-context',
            items: this.setSigner()
        }],"Select All");


        // this.zone.runOutsideAngular(()=>{
        //     this.setContextMenu();
        // })
    }

    isScroll: boolean = true;
    scrollX: number = 0;
    scrollY: number = 0;
    selectFormField(params){
        // this.selecting = true;
        (this.cmenu as ContextMenuComponent).showItems(['Cut','Copy','Delete','Rotate 90','Rotate 180','Replicate To Page','Replicate To All Pages']);
        (this.cmenu as ContextMenuComponent).removeItems(['Add Signature']);
        this.zone.runOutsideAngular(()=>{
            this.selectedFieldId = params?.field?.id ?? "";
            if (this.pdfviewerControl.annotationCollection.length > 0){
                this.pdfviewerControl.annotationModule.deleteAnnotationById(this.pdfviewerControl.annotationCollection[0].annotationId);
            }

            // this.selectedFieldIds.push(params?.field?.id)

            // this.pdfviewerControl.formDesignerModule.selectFormField(this.pdfviewerControl.formDesignerModule.getFormField(this.selectedFieldId));

            // this.setContextMenu();
            // // console.log(this.ctrlKey)

            // // console.log(this.selectedFieldIds)
            if (this.ctrlKey){
                if ( !this.selectedFieldIds.some(e => e == params?.field?.id) && params?.field?.id)this.selectedFieldIds.push(params?.field?.id)
                let firstField = this.pdfviewerControl.formDesignerModule.getFormField(this.selectedFieldIds[0])
                if (firstField){
                    let minx = firstField.bounds.x;
                    let miny = firstField.bounds.y;

                    let maxx = firstField.bounds.x + firstField.bounds.width ;
                    let maxy = firstField.bounds.y + firstField.bounds.height;

                    let firstWidth = firstField.bounds.width;
                    let firstHieght = firstField.bounds.height;

                    if (this.selectedFieldIds.length > 1)
                    {
                        this.selectedFieldIds.forEach(p => {
                            if (this.pdfviewerControl.annotationCollection.length > 0){
                                this.pdfviewerControl.annotationModule.deleteAnnotationById(this.pdfviewerControl.annotationCollection[0].annotationId);
                            }

                            let field = this.pdfviewerControl.formDesignerModule.getFormField(p)
                            if (field.bounds.x <= minx){
                                minx = field.bounds.x;
                            }
                            if (field.bounds.y <= miny) miny = field.bounds.y;

                            if ((field.bounds.x + field.bounds.width) >= maxx) maxx = field.bounds.x + field.bounds.width ;
                            if ((field.bounds.y + field.bounds.height) >= maxy) maxy = field.bounds.y + field.bounds.height ;

                            this.firstMultiAnnotationX = minx;
                            this.firstMultiAnnotationY = miny;
                            // if (this.pdfviewerControl?.annotationCollection?.length > 0)this.pdfviewerControl.annotationModule.deleteAnnotationById(this.pdfviewerControl.annotationCollection[0].annotationId)
                            this.pdfviewerControl?.annotation.addAnnotation(
                                "Rectangle",
                                {
                                    offset : {
                                        x : minx  ,
                                        y :miny ,
                                    },
                                    opacity: 1,
                                    fillColor: '',
                                    strokeColor: '#0374f6',
                                    author: 'Guest',
                                    thickness: 3,
                                    height: maxy - miny,
                                    width: maxx - minx,
                                    minHeight: maxy - miny,
                                    minWidth: maxx - minx,
                                    maxHeight: maxy - miny,
                                    maxWidth: maxx - minx,
                                    annotationSelectorSettings: {
                                        selectionBorderColor: '#0374f6',
                                        resizerBorderColor: '#0374f6',
                                        resizerFillColor: '#0374f6',
                                        resizerSize: 1,
                                        // selectionBorderThickness: 3,
                                        resizerShape: 'Square',
                                        selectorLineDashArray: [0],
                                        resizerLocation: AnnotationResizerLocation.Corners ,
                                        resizerCursorType: null,
                                    },
                                    customData:this.selectedFieldIds,
                                    allowedInteractions: [AllowedInteraction.Move,AllowedInteraction.Select,AllowedInteraction.Resize],
                                    pageNumber:firstField.pageNumber
                                } as RectangleSettings ,
                                // standardBusinessStampItem?: StandardBusinessStampItem
                            )



                            // this.pdfviewerControl.annotationModule.selectAnnotation(this.pdfviewerControl.annotationCollection[0].annotationId);
                            this.selectedFieldId = undefined;
                        })
                    }
                    else {
                        this.selectedFieldIds = [];
                        this.selectedFieldIds.push(params?.field?.id);

                        // // console.log(params?.field?.id)

                        let field = this.pdfviewerControl.formDesignerModule.getFormField(this.selectedFieldIds[0])
                        this.pdfviewerControl?.annotation.addAnnotation(
                            "Rectangle",
                            {
                                offset : {
                                    x : field?.bounds?.x  ,
                                    y : field?.bounds?.y ,
                                },
                                opacity: 1,
                                fillColor: '',
                                strokeColor: '#0374f6',
                                author: 'Guest',
                                thickness: 3,
                                height: field?.bounds?.height,
                                width: field?.bounds?.width,
                                annotationSelectorSettings: {
                                    selectionBorderColor: '#0374f6',
                                    resizerBorderColor: '#0374f6',
                                    resizerFillColor: '#0374f6',
                                    resizerSize: 1,
                                    // selectionBorderThickness: 3,
                                    resizerShape: 'Square',
                                    selectorLineDashArray: [0],
                                    resizerLocation:  AnnotationResizerLocation.Corners as any ,
                                    resizerCursorType: null,
                                },
                                customData:this.selectedFieldIds,
                                allowedInteractions: [AllowedInteraction.Move,AllowedInteraction.Select,AllowedInteraction.Resize],
                                pageNumber:field.pageNumber
                            } as RectangleSettings ,
                            // standardBusinessStampItem?: StandardBusinessStampItem
                        )
                    }
                }

            }
            else {
                // if (this.pdfviewerControl?.annotationCollection?.length > 0)this.pdfviewerControl.annotationModule.deleteAnnotationById(this.pdfviewerControl.annotationCollection[0].annotationId)
                this.selectedFieldIds = [];
                // // console.log(params?.field?.id)
                if(params?.field?.id){
                    this.selectedFieldIds.push(params?.field?.id);
                    let field = this.pdfviewerControl.formDesignerModule.getFormField(this.selectedFieldIds[0])
                    // console.log(field)
                    this.pdfviewerControl?.annotation.addAnnotation(
                        "Rectangle",
                        {
                            offset : {
                                x : field?.bounds?.x  ,
                                y : field?.bounds?.y ,
                            },
                            opacity: 1,
                            fillColor: '',
                            strokeColor: '#0374f6',
                            author: 'Guest',
                            thickness: 3,
                            height: field?.bounds?.height,
                            width: field?.bounds?.width,
                            annotationSelectorSettings: {
                                selectionBorderColor: '#0374f6',
                                resizerBorderColor: '#0374f6',
                                resizerFillColor: '#0374f6',
                                resizerSize: 1,
                                // selectionBorderThickness: 3,
                                resizerShape: 'Square',
                                selectorLineDashArray: [0],
                                resizerLocation: AnnotationResizerLocation.Corners as any ,
                                resizerCursorType: null,
                            },
                            customData:this.selectedFieldIds,
                            allowedInteractions: [AllowedInteraction.Move,AllowedInteraction.Select,AllowedInteraction.Resize],
                            pageNumber:field.pageNumber
                        } as RectangleSettings ,
                        // standardBusinessStampItem?: StandardBusinessStampItem
                    )
                }
            }

            let pdfViewer = document.getElementById("pdfViewer_viewerContainer");

            if(pdfViewer){
                this.isScroll = false;
                this.scrollY = pdfViewer.scrollTop;
                this.scrollX = pdfViewer.scrollLeft;

                if (this.pdfviewerControl.annotationCollection.length > 0) this.pdfviewerControl.annotationModule.selectAnnotation(this.pdfviewerControl.annotationCollection[0].annotationId);

            }
                // this.pdfviewerControl.magnificationModule.pageRerenderOnMouseWheel()
        });


        // setTimeout(()=>{
        //     (this.cmenu as ContextMenuComponent).showItems(['Cut','Copy','Delete','Rotate 90','Rotate 180','Replicate To Page','Replicate To All Pages']);
        // },100)
    }

    clickFormField(params){
        this.zone.runOutsideAngular(()=>{

        })
    }

    getCenter(element) {
        const {left, top, width, height} = element.getBoundingClientRect();
        return {x: left + width / 2, y: top + height / 2}
    }

    dragEvent(e : any,step?, group? , signer? ){
        this.zone.runOutsideAngular(()=>{
            // e.preventDefault();
            Array.from(document.getElementsByClassName('e-pv-page-div') as HTMLCollectionOf<HTMLElement>).forEach((e,i) => {
                // e.setAttribute("pDroppable","true");
                // e.addEventListener("drop",(p) => this.drop(p))

                let pageNum = Number(e.id.split("_")[e.id.split("_").length -1]) + 1;

                let droppable: Droppable = new Droppable(e, {
                    drop: (p: DropEventArgs) => this.zone.runOutsideAngular(()=>{
                        this.dropEvent(p,pageNum,e)
                    })
                });
            })

            // e.dragElement.remove();
            // this.set

            // alert(this.dragFromTool)
        })
    }

    customCloneElement(e : any,step? , signer?){
        this.dragFromTool = true;
        if (step && !signer){
            this.dragSigners = [];
            step.signers.forEach(e => this.dragSigners.push(e))
            this.dragFromTool = false;
        }
        else if (step && signer && !this.selectedSigners.some(e => e.id == signer.id)){
            this.dragSigners = [];
            this.dragSigners.push(signer)
            this.dragFromTool = false;
        }
        else if (step && signer && this.selectedSigners.some(e => e.id == signer.id)){{
            this.dragSigners = [];
            this.selectedSigners.forEach(e => this.dragSigners.push(e))
            this.dragFromTool = false;
        }}

        const helperElement: HTMLElement = document.createElement('div');
        helperElement.innerText = '';
        helperElement.style.width  = 160*(this.zoomValue/100) + "px";
        helperElement.style.height  = 55*(this.zoomValue/100) + "px";
        helperElement.style.marginLeft = `${e.sender.offsetX + 10 }px`;
        helperElement.style.marginTop  = `${e.sender.offsetY + 10 }px`;
        helperElement.style.zIndex  = '10';
        // helperElement.style.backgroundColor  = 'red';
        // helperElement.style.transform  = "scale("+this.zoomValue/100+")";
        // helperElement.style.opacity = '0.5';
        // helperElement.style.border = '2px solid black';



            if ((this.dragFromTool &&this.selectedSigners.length > 1) || (!this.dragFromTool && this.dragSigners.length > 1)){
                // e.dragElement.innerHTML = '';

                let p = document.createElement('img');
                p.setAttribute("src","/assets/common/images/2.png");
                // p.style.marginLeft = `${e.event.offsetX }px`;
                // p.style.marginTop  = `${e.event.offsetY}px`;

                p.style.width  = 160*(this.zoomValue/100) + "px";
                p.style.height  = 55*(this.zoomValue/100) + "px";
                p.style.opacity  = ".5";
                p.style.display  = "flex";
                p.style.justifyContent  = "space-around";
                // p.style.transform  = "scale("+this.zoomValue/100+")";
                helperElement.style.cursor  = 'grabbing';
                helperElement.appendChild(p);
            }

            else {
                // e.dragElement.innerHTML = '';
                let p = document.createElement('img');
                p.setAttribute("src","/assets/common/images/1.png");
                // p.style.marginLeft = `${e.event.offsetX}px`;
                // p.style.marginTop  = `${e.event.offsetY}px`;

                p.style.width  = 160*(this.zoomValue/100) + "px";
                p.style.height  = 55*(this.zoomValue/100) + "px";
                p.style.opacity  = ".5";
                p.style.display  = "flex";
                p.style.justifyContent  = "space-around";
                // p.style.transform  = "scale("+this.zoomValue/100+")";
                helperElement.style.cursor  = 'grabbing';
                helperElement.appendChild(p);

            }


        // Append the helper element to the document body
        document.body.appendChild(helperElement);
        return helperElement;
    }

    recreateDragElement(){
        this.zone.runOutsideAngular(()=>{


            setTimeout(() => {
                let dragElement: HTMLElement = document.getElementById('signature');
                let draggable: Draggable = new Draggable(dragElement,{ clone: true,tapHoldThreshold:0,dragStart : (e) => this.dragEvent(e),helper: (e)=> this.customCloneElement(e)} );
                // let dragElement2: HTMLElement = document.getElementById('name');
                // let draggable2: Draggable = new Draggable(dragElement2,{ clone: true,tapHoldThreshold:0,dragStart : (e) => this.dragEvent(e),helper: (e)=> this.customCloneElement(e) });
                if (!this.documentData?.isDigitalSignature){
                    let dragElement3: HTMLElement = document.getElementById('title');
                    let draggable3: Draggable = new Draggable(dragElement3,{ clone: true,tapHoldThreshold:0,dragStart : (e) => this.dragEvent(e) ,helper: (e)=> this.customCloneElement(e)});
                    let dragElement4: HTMLElement = document.getElementById('datesigned');
                    let draggable4: Draggable = new Draggable(dragElement4,{ clone: true,tapHoldThreshold:0,dragStart : (e) => this.dragEvent(e),helper: (e)=> this.customCloneElement(e) });
                }
                // let dragElement5: HTMLElement = document.getElementById('company');
                // let draggable5: Draggable = new Draggable(dragElement5,{ clone: true,tapHoldThreshold:0,dragStart : (e) => this.dragEvent(e),helper: (e)=> this.customCloneElement(e) });
                // let dragElement6: HTMLElement = document.getElementById('text');
                // let draggable6: Draggable = new Draggable(dragElement6,{ clone: true,tapHoldThreshold:0,dragStart : (e) => this.dragEvent(e),helper: (e)=> this.customCloneElement(e) });

                this.signerData.forEach((k,i) => {
                    let groupElement = document.getElementById('step_'+i);
                    // Array.from(groupElement.children[0].children[1].children).forEach(l => {
                    //     l.addEventListener("dragstart",(s)=>{
                    //         s.preventDefault()
                    //     })
                    // })
                    let groupElementDraggable = new Draggable(groupElement,{ clone: true,tapHoldThreshold:0,dragStart : (e) => this.dragEvent(e,k),helper: (e)=> this.customCloneElement(e,k) });
                    k.signers.forEach((p,index) => {
                            let signerElement = document.getElementById('step_'+i+'_signer_'+index)
                            let groupElementDraggable = new Draggable(signerElement,{ clone: true,tapHoldThreshold:0,dragStart : (e) => this.dragEvent(e,k,p),helper: (e)=> this.customCloneElement(e,k,p) });
                        })

                })
            });
        })


    }

    getType(id){
        switch(id){
            case "signature":
                this.fontSize = 8;
                return "SignatureField";
            case "name": return "Textbox";
            case "title": return "Textbox";
            case "datesigned": return "Textbox";
            case "company": return "Textbox";
            case "text": return "Textbox";
            default :
                this.fontSize = 8;
                return "SignatureField"
        }
    }

    annotationSelect(params){
        this.zone.runOutsideAngular(()=>{
            //select annotation
            // params.preventDefault();
            (this.cmenu as ContextMenuComponent).showItems(['Cut','Copy','Delete','Rotate 90','Rotate 180','Replicate To Page','Replicate To All Pages']);
            (this.cmenu as ContextMenuComponent).removeItems(['Add Signature']);
        })
    }


    pageClick(params){
        this.zone.runOutsideAngular((e)=>{
           setTimeout(()=>{
            // let pdfViewer = document.getElementById("pdfViewer_viewerContainer");
            // if(pdfViewer){
            //     pdfViewer.style.overflow = "auto";
            //     // setTimeout(()=>{
            //     //     pdfViewer.style.overflow = "auto";
            //     // },1)

            //     pdfViewer.addEventListener("scroll",(e => {
            //         e.preventDefault();
            //     }))
            // }
            this.pageNum = params?.pageNumber ;
            // if (this.selectedFieldIds.length > 1) this.annotationMove(e)

            if (this.firstMultiAnnotationX && this.firstMultiAnnotationY && this.newAnnotationPos && this.selectedFieldIds.length > 1){
                let x1 = this.firstMultiAnnotationX ;
                let y1 = this.firstMultiAnnotationY ;

                let x2 = this.newAnnotationPos.x ;
                let y2 = this.newAnnotationPos.y ;
                let removeField = [];
                this.selectedFieldIds.forEach(e => {
                    let data = this.pdfviewerControl.formDesignerModule.getFormField(e);
                    let newPos = this.calculateNewPoint(x1,y1,x2,y2,data.bounds.x,data.bounds.y);
                    if(this.altCloning){
                        this.pdfviewerControl?.formDesignerModule?.addFormField( data.formFieldAnnotationType as FormFieldType,
                            Object.assign({

                                // type: data.formFieldAnnotationType,
                                 name: data.name,
                                 value: data.value,
                                 bounds: { X: data.bounds.x , Y: data.bounds.y , Width: data.bounds.width, Height: data.bounds.height },
                                 pageNumber :data.pageNumber,
                                 backgroundColor : data.backgroundColor,
                                 zIndex:2,
                                 fontFamily: data.fontFamily,
                                 fontSize: data.fontSize,
                                 color : data.color,
                                 alignment:  data.alignment,
                            }) as any);
                        // if (this.getType(e.dragData.draggedElement.id.toString()) == "SignatureField"){
                        //     p.numOfSign = (p.numOfSign ?? 0) + 1
                        // }
                    }


                    this.pdfviewerControl?.formDesignerModule?.updateFormField(e,{ bounds: {X : newPos.x + data.bounds.width/2   , Y: newPos.y + data.bounds.height/2 , Width: data.bounds.width, Height: data.bounds.height }} as any);

                    // let signerDataList = this.signerData?.find(k => k.signers?.some(l => l.formFields?.some(s => s.fieldId == e)))?.signers
                    // let signerField = signerDataList?.find(k => k.formFields?.some(s => s.fieldId == e));

                    // signerField?.formFields?.splice(signerField?.formFields?.findIndex(e => e.fieldId == e),1)

                    // signerField?.formFields?.push({
                    //     pageNumber: data.pageNumber,
                    //     position: {
                    //         x:Math.round(newPos.x + data.bounds.width/2),
                    //         y:Math.round(newPos.y + data.bounds.height/2),
                    //         w:Math.round(data.bounds.width),
                    //         h:Math.round(data.bounds.height),
                    //     },
                    //     style: {
                    //         isBold: false,
                    //         isItalic: false,
                    //         isUnderline: false,
                    //         alignment: data.alignment,
                    //         color: data.color,
                    //         name : 'signature',
                    //         value: data.value,
                    //         typeId : 1,
                    //         backgroundColor: data.backgroundColor
                    //     },
                    //     documentId :this.selectedFile,
                    //     fieldId : data.id ,
                    //     field: this.pdfviewerControl.formDesignerModule.getFormField(e)
                    // })

                    this.signerData.forEach(k => {
                        k.signers.forEach(p => {
                            // p.numOfSign = (p.numOfSign ?? 0) + 1
                            let field = p.formFields?.find(l => l.fieldId == e)?.field
                            if (field){
                                p.formFields.splice(p.formFields.findIndex(l => l.fieldId == this.selectedFieldId),1);
                                p.formFields?.push({
                                    pageNumber: field.pageNumber,
                                    position: {
                                        x:Math.round(field?.position?.x ?? 0),
                                        y:Math.round(field?.position?.y ?? 0),
                                        w:Math.round(field?.position?.w ?? 0),
                                        h:Math.round(field?.position?.h ?? 0),
                                        rotate: field?.position?.rotate ?? 0 ,
                                    },
                                    style: {
                                        isBold: false,
                                        isItalic: false,
                                        isUnderline: false,
                                        alignment: field.alignment,
                                        color: field.color,
                                        name : field?.name ?? 'signature',
                                        value: field.value,
                                        typeId : this.getTypeId(field?.name ?? 'signature'),
                                        backgroundColor: field.backgroundColor,
                                        fontSize : field.fontSize ?? 8
                                    },
                                    documentId :this.selectedFile,
                                    fieldId : field.id ,
                                    field: field
                                })
                            }
                        })

                    })

                })

                this.firstMultiAnnotationX = this.newAnnotationPos.x ;
                this.firstMultiAnnotationY = this.newAnnotationPos.y ;

                this.newAnnotationPos = undefined;
            }
            else if (this.selectedFieldIds.length == 1){

                if (!this.drop && !this.drag){
                    let annotation = this.pdfviewerControl.annotationCollection[0];
                    // console.log(annotation)
                    if (annotation){
                        this.zone.runOutsideAngular(()=>{
                            let x =  annotation.bounds.left
                            let y = annotation.bounds.top

                            let field = this.pdfviewerControl.formDesignerModule.getFormField(this.selectedFieldIds[0]);
                            // // console.log(this.selectedFieldIds[0])
                            // // console.log(field)
                            // // console.log(field.bounds)
                            // // console.log(annotation.bounds)

                            // let newX = x + annotation.bounds.width/2;
                            // let newY = y + annotation.bounds.height/2;
                            // // console.log(newX,newY)
                            // // console.log(params.annotationBound)
                            // // console.log(field.bounds)
                            if (field){
                                this.pdfviewerControl.formDesignerModule.updateFormField(field,{ bounds: {X:x + annotation.bounds.width/2 ,Y:y + annotation.bounds.height/2, Width: annotation.bounds.width,Height: annotation.bounds.height}} as any)
                            }

                        })
                    }
                }

                let data = this.pdfviewerControl.formDesignerModule.getFormField(this.selectedFieldIds[0]);
                let signerDataList = this.signerData?.find(k => k.signers?.some(l => l.formFields?.some(s => s.fieldId == this.selectedFieldIds[0])))?.signers
                let signerField = signerDataList?.find(k => k.formFields?.some(s => s.fieldId == this.selectedFieldIds[0]));



                let fieldData = signerField?.formFields?.find(e => e.fieldId == this.selectedFieldIds[0])

                if(data){
                    signerField?.formFields?.splice(signerField?.formFields?.findIndex(e => e.fieldId == this.selectedFieldIds[0]),1)

                    signerField?.formFields?.push({
                        pageNumber: data.pageNumber,
                        position: {
                            x:Math.round(data.bounds.x),
                            y:Math.round(data.bounds.y),
                            w:Math.round(data.bounds.width),
                            h:Math.round(data.bounds.height),
                            rotate: fieldData?.position?.rotate ?? 0,
                        },
                        style: {
                            isBold: false,
                            isItalic: false,
                            isUnderline: false,
                            alignment: data.alignment,
                            color: data.color,
                            name : data?.name ?? 'signature',
                            value: data.value,
                            typeId : this.getTypeId(data?.name ?? 'signature'),
                            backgroundColor: data.backgroundColor,
                            fontSize : data.fontSize ?? 8
                        },
                        documentId :this.selectedFile,
                        fieldId : data.id ,
                        field: data
                    })
                }

            }

           },100)
        })

        setTimeout(()=>{
            this.drop = false;
            this.drag = false;
        },200)
    }

    calculateNewPoint(x1,y1,x2,y2,currentX, currentY) {
        // Calculate the direction vector from (x1, y1) to (x2, y2)
        const dx = x2 - x1;
        const dy = y2 - y1;


        // Normalize the vector (convert it to a unit vector)
        const length = Math.sqrt(dx * dx + dy * dy);
        const unitVectorX = dx / length;
        const unitVectorY = dy / length;

        // Calculate the new point by offsetting (x3, y3) along the unit vector
        let newX = currentX + unitVectorX*length;
        let newY = currentY + unitVectorY*length;
        return {x: newX,y:newY};
      }

      checkAll = false;
    checkAllHandler(event) {
        //select all signers
        this.checkAll = event.checked;
        this.selectedSigners =[];
        this.signerData.forEach((item) => {
            item.signers.forEach((signer) => {
                    signer.check = event.checked;
                    if (event.checked) this.selectedSigners.push(signer)
                });
            });

    }


    checkSigner(stepI, signerI, params){
        this.signerData[stepI].signers[signerI].check = params;
        if (params) this.selectedSigners.push(this.signerData[stepI].signers[signerI])
        else this.selectedSigners.splice(this.selectedSigners.findIndex(e => e == this.signerData[stepI].signers[signerI]),1)

        let signerNum = this.signerData.reduce((a,b) =>  { return a + b.signers.length ; }, 0);
        if (this.selectedSigners.length == signerNum){
            this.checkAll = true;
        }
        else this.checkAll = false;
    }


    changePage(params){
        this.pdfviewerControl.navigation.goToPage(params);
        this.pendding_rendering_field_html();
    }

    changePagePdfViewer(){
        this.currentPageNumber = this.pdfviewerControl?.currentPageNumber ?? 1
        this.pendding_rendering_field_html();

    }

    changeScalePdfViewer(){
        this.zoomValue = this.pdfviewerControl?.zoomValue ?? 100
        this.pendding_rendering_field_html();

    }

    changeScalePdfViewerPreview(params){
        this.pendding_rendering_preview_field_html();

    }

    changePagePdfViewerPreview(params){
        this.pendding_rendering_preview_field_html();

    }

    changeScale(params){
        if(params.keyCode == 13)
        {
            this.pdfviewerControl.magnification.zoomTo(this.zoomValue);
            // this.zoomValue = this.pdfviewerControl?.zoomValue ?? 100
            this.pendding_rendering_field_html();
        }

    }




    public openDocument(): void {
        document.getElementById('fileUpload').click();
    }

    public previousClicked(): void {
        this.zone.runOutsideAngular(()=>{
            this.pdfviewerControl.navigation.goToPreviousPage();
            this.pendding_rendering_field_html();

        })
    }

    public nextClicked(): void {
        this.zone.runOutsideAngular(()=>{
            this.pdfviewerControl.navigation.goToNextPage();
             this.pendding_rendering_field_html();

        })
    }

    public printClicked(): void {
        this.zone.runOutsideAngular(()=>{
            this.pdfviewerControl.print.print();
        })
    }

    public downloadClicked(): void {
        let fileName: string = (document.getElementById('fileUpload') as HTMLInputElement).value.split('\\').pop();
        if (fileName !== '') {
            this.pdfviewerControl.fileName = fileName;
        }
        this.pdfviewerControl.download();
    }

    public pageFitClicked(): void {
        this.zone.runOutsideAngular(()=>{
            this.pdfviewerControl.magnification.fitToPage();
            this.pendding_rendering_field_html();

        })

    }

    public zoomInClicked(): void {
        this.zone.runOutsideAngular(()=>{
            this.pdfviewerControl.magnification.zoomIn();
            this.pendding_rendering_field_html();

        })
    }

    public zoomOutClicked(): void {
        this.zone.runOutsideAngular(()=>{
            this.pdfviewerControl.magnification.zoomOut();
                this.pendding_rendering_field_html();

        })
    }

    // public pageChanged(): void {
    //     (document.getElementById('currentPage') as HTMLInputElement).value = this.pdfviewerControl.currentPageNumber.toString();
    // }

    // public onCurrentPageBoxClicked(): void {
    //     (document.getElementById('currentPage') as HTMLInputElement).select();
    // }

    // public onCurrentPageBoxKeypress(e: KeyboardEvent): boolean {
    //     return this.zone.runOutsideAngular(()=>{
    //         if ((e.which < 48 || e.which > 57) && e.which !== 8 && e.which !== 13) {
    //             e.preventDefault();
    //             return false;
    //         } else {
    //             // tslint:disable-next-line:radix
    //             const currentPageNumber: number = parseInt((document.getElementById('currentPage') as HTMLInputElement).value);
    //             if (e.which === 13) {
    //                 if (currentPageNumber > 0 && currentPageNumber <= this.pdfviewerControl.pageCount) {
    //                     this.pdfviewerControl.navigation.goToPage(currentPageNumber);
    //                 } else {
    //                     // tslint:disable-next-line:max-line-length
    //                     (document.getElementById('currentPage') as HTMLInputElement).value = this.pdfviewerControl.currentPageNumber.toString();
    //                 }
    //             }
    //             return true;
    //         }
    //     })

    // }


    // tslint:disable-next-line
    private readFile(args: any): void {
        // tslint:disable-next-line
        let upoadedFiles: any = args.target.files;
        if (args.target.files[0] !== null) {
            let uploadedFile: File = upoadedFiles[0];
            if (uploadedFile) {
                let reader: FileReader = new FileReader();
                reader.readAsDataURL(uploadedFile);
                // tslint:disable-next-line
                let proxy: any = this;
                // tslint:disable-next-line
                reader.onload = (e: any): void => {
                    let uploadedFileUrl: string = e.currentTarget.result;
                    proxy.pdfviewerControl.load(uploadedFileUrl, null);
                };
            }
        }
    }

    change(e: any): void {
        this.pdfviewerControl.serviceUrl = 'https://ej2services.syncfusion.com/angular/development/api/pdfviewer';
        this.pdfviewerControl.dataBind();
        this.pdfviewerControl.load(this.pdfviewerControl.documentPath, null);
    }


    selectAllCurrentPageFormField(){

        if (this.pdfviewerControl.formFields.filter(e => e.pageNumber == this.pageNum && e.bounds.x != -100 && e.bounds.y != -100).length > 1){
            let firstField = this.pdfviewerControl.formFields.filter(e => e.pageNumber == this.pageNum && e.bounds.x != -100 && e.bounds.y != -100)[0];
            if (firstField){
                // this.selectedFieldId = undefined;
                this.selectedFieldIds = []
                let minx = firstField.bounds.x;
                let miny = firstField.bounds.y;

                let maxx = firstField.bounds.x + firstField.bounds.width ;
                let maxy = firstField.bounds.y + firstField.bounds.height;

                let firstWidth = firstField.bounds.width;
                let firstHieght = firstField.bounds.height;

                this.pdfviewerControl.formFields.filter(e => e.pageNumber == this.pageNum ).forEach(field => {
                    if (field.bounds.x != -100 && field.bounds.y != -100){
                        this.selectedFieldIds.push(field.id)
                        if (field.bounds.x <= minx){
                            minx = field.bounds.x;
                        }
                        if (field.bounds.y <= miny) miny = field.bounds.y;

                        if ((field.bounds.x + field.bounds.width) >= maxx) maxx = field.bounds.x + field.bounds.width ;
                        if ((field.bounds.y + field.bounds.height) >= maxy) maxy = field.bounds.y + field.bounds.height ;

                        this.firstMultiAnnotationX = minx;
                        this.firstMultiAnnotationY = miny;
                    }

                })
                this.pdfviewerControl?.annotation.addAnnotation(
                    "Rectangle",
                    {
                        offset : {
                            x : minx  ,
                            y :miny ,
                        },
                        opacity: 1,
                        fillColor: '',
                        strokeColor: '#0374f6',
                        author: 'Guest',
                        thickness: 3,
                        height: maxy - miny,
                        width: maxx - minx,
                        minHeight: maxy - miny,
                        minWidth: maxx - minx,
                        maxHeight: maxy - miny,
                        maxWidth: maxx - minx,
                        annotationSelectorSettings: {
                            selectionBorderColor: '#0374f6',
                            resizerBorderColor: '#0374f6',
                            resizerFillColor: '#0374f6',
                            resizerSize: 3,
                            // selectionBorderThickness: 3,
                            resizerShape: 'Square',
                            selectorLineDashArray: [0],
                            resizerLocation: AnnotationResizerLocation.Corners ,
                            resizerCursorType: null,
                        },
                        // customData:this.selectedFieldIds,
                        allowedInteractions: [AllowedInteraction.Move],
                        pageNumber :  this.pageNum
                    } as RectangleSettings ,
                    // standardBusinessStampItem?: StandardBusinessStampItem
                )

                let pdfViewer = document.getElementById("pdfViewer_viewerContainer");

                if(pdfViewer){
                    this.isScroll = false;
                    this.scrollY = pdfViewer.scrollTop;
                    this.scrollX = pdfViewer.scrollLeft;

                    this.pdfviewerControl.annotationModule.selectAnnotation(this.pdfviewerControl.annotationCollection[0].annotationId);

                }
                // setTimeout(()=>{
                    // this.pdfviewerControl?.annotationModule?.selectAnnotation(this.pdfviewerControl?.annotationCollection[0]?.annotationId);
                    this.firstMultiAnnotationX = minx
                    this.firstMultiAnnotationY = miny
                    this.selectedFieldId = undefined;
                // },100)
            }
        }
        else {
            this.selectedFieldId = this.pdfviewerControl.formFields[this.pdfviewerControl.formFields.length -1]?.id;
            if (!this.selectedFieldIds.some(e => e == this.pdfviewerControl.formFields[this.pdfviewerControl.formFields.length -1]?.id)) this.selectedFieldIds.push(this.pdfviewerControl.formFields[this.pdfviewerControl.formFields.length -1]?.id)
        }

    }


    showPreview(params){

        let draf = this.setSaveRequestData();

        draf.id = this.documentData.id;

        draf.category = this.documentData.listCategoryId.join(",");
        draf.systemId = 1; //esign
        draf.statusType = 0; //on progress
        // draf.listCategoryId = [];
        // this.documentData?.categoryList?.forEach(e => {
        //     if(!draf?.listCategoryId?.some(p => p == parseFloat(e))) draf?.listCategoryId?.push(parseFloat(e))
        // })
        draf.expectedDate = this.documentData.expectedDate; // --> Datetime to Datetime

        if (draf.id && draf.id > 0){
            this.spinnerService.show();
            this._requestWebService.saveDraftRequest(draf)
            .pipe(finalize(()=>{
                this.spinnerService.hide();
                // if (this.checkHadMySignField() && !this.hasSignature) {

                //     // this.pdfviewerControl.deleteAnnotations();
                //     if(this.selectedFieldId) this.pdfviewerControl.formDesignerModule.clearSelection(this.selectedFieldId);
                //         this.OpenPopupSignature();
                //     // this._mstEsignUserImage.getListSignatureByUserIdForWeb().subscribe(res =>{
                //     //     if(res && res.items.length == 0) {
                //     //         this.notify.warn("Please provide your signature");
                //     //         if(this.selectedFieldId) this.pdfviewerControl.formDesignerModule.clearSelection(this.selectedFieldId);
                //     //         this.OpenPopupSignature();
                //     //     }
                //     //     else {
                //     //         this.showPreviewDefault(params);
                //     //     }
                //     // });
                // }
                // else {
                    this.showPreviewDefault(params);
                // }

            }))
            .subscribe(res => {
                // this.notify.success("Draft Saved")
            })
        }


    }

    showPreviewDefault(params){
        this.isPreview = params;
            // this.documentUrl = this.fileData.find(e => e.id == params )?.documentPath;
            // this.pdfviewerPreviewControl?.load(this.documentUrl?.replace("https","http")  ,null);

            let formFieldSettings = []
            this.pdfviewerControl.formFields.forEach(e => {
                formFieldSettings.push(
                    Object.assign({
                        // type: e.formFieldAnnotationType,
                        name: e.name,
                        value: e?.value ?? "",
                        bounds: { X: e.bounds.x , Y: e.bounds.y , Width: e.bounds.width, Height: e.bounds.height },
                        pageNumber : e.pageNumber,
                        backgroundColor :e.backgroundColor,
                        fontFamily: e?.fontFamily,
                        color: e?.color,
                        fontSize: e?.fontSize,
                        fontStyle: e?.fontStyle,
                        zIndex: 2
                    }) as any
                )
            });
            this.documentData.category = this.listCategory.filter(e => this.documentData.listCategoryId?.some(p => p == e.id)).map(e => e.localName)?.join(", ");
            this.isMandatory = this.documentData?.category?.includes("Ringi") ?? false;
            this.documentData.itemList = this.documentData?.itemList?.map(e => {
                return {
                    ...e,
                    signatureCount: this.signerData?.reduce((a,b) =>  { return a.concat(b?.signers) ; }, [])
                                .filter(p => p?.formFields?.some(k => k.documentId == e.id))?.reduce((a,b) =>  { return a.concat(b?.formFields) }, []).filter(k => k?.documentId == e.id)?.length,
                    signers: this.signerData?.reduce((a,b) =>  { return a.concat(b?.signers) ; }, [])
                                .filter(p => p.formFields?.some(k => k?.documentId == e.id))
                }
            });
    }

    listPreviewField = [];
    selectedDocId = 0;

    setupFieldForPreview(){
        this.listPreviewField = [];
        if(this.selectedDocId == 0) this.selectedDocId = this.selectedFile;
        this.signerData.forEach(e => {
            e.signers.forEach(p => {
                p.formFields.forEach(k =>{
                    if(k.documentId == this.selectedDocId){
                        let data =  Object.assign({
                            // type: field.formFieldAnnotationType,
                             name: k.field.name,
                             value: k.field?.value,
                             bounds: { X: k.position.x , Y: k.position.y , Width: k.field.bounds.width, Height: k.field.bounds.height },
                             pageNumber : k.field.pageNumber,
                             backgroundColor : k.field.backgroundColor,
                             zIndex:2,
                             fontFamily: k.field.fontFamily,
                             fontSize: k.field.fontSize,
                             color: k.field.color,
                             fontStyle: k.field.fontStyle,
                             alignment:  k.field.alignment,
                        }) as any;
                        this.pdfviewerPreviewControl.formDesignerModule.addFormField("Textbox" as FormFieldType,data as any);

                        this.listPreviewField.push({
                            hasSign : p.id == abp.session.userId,
                            field : this.pdfviewerPreviewControl.formFields[this.pdfviewerPreviewControl.formFields.length - 1]
                        })
                    }
                })
            })
        })
    }

    previewPdfDoneLoading(params){
        // this.pdfviewerControl.annotationResize = this.annotationResize.bind(this)

        this.zone.runOutsideAngular(()=>{

            // setTimeout(() => this.setupFieldForPreview());

            // let group = this.signerData.find(e => e.signers.some(p => p.id == this.appSession.userId));
            // let me = group.signers.find(e => e.id == this.appSession.userId);
            // this.selectedDocId = this.selectedFile;

            // let listFieldData = me.formFields;
            // let listField = me.formFields.map(e => e.fieldId + "_content_html_element")

            // setTimeout(()=>{
            //     for (const field of this.pdfviewerControl?.formFields) {
            //         let data =  Object.assign({
            //             // type: field.formFieldAnnotationType,
            //             id : field.id,
            //              name: field.name,
            //              value: field?.value,
            //              bounds: { X: field.bounds.x , Y: field.bounds.y , Width: field.bounds.width, Height: field.bounds.height },
            //              pageNumber :field.pageNumber,
            //              backgroundColor : field.backgroundColor,
            //              zIndex:2,
            //              fontFamily: field.fontFamily,
            //              fontSize: field.fontSize,
            //              color: field.color,
            //              fontStyle: field.fontStyle,
            //              alignment:  field.alignment,
            //         }) as any;
            //         this.pdfviewerPreviewControl.formDesignerModule.addFormField(field.formFieldAnnotationType as FormFieldType,data as any);

            //       }
            // })
            if (this.checkHadMe()){
                this.setUpListSignatureForPreview()
                this.pendding_rendering_preview_field_html();
            }
        })


    }

    // OpenPopupSignature() {

    //     // this.objselectFieldSignature = this.listFieldSignatureForSigner.find(e => e.positionX == x && e.positionY == y);
    //     let _pop = document.querySelector<HTMLElement>(".popup-signature");
    //     _pop.style.display = 'flex';
    // }




    chooseSignature(params){

        if (this.signerData.some(e => e.signers.some(p => p.id == abp.session.userId && !p.formFields.some(k => k.fieldId == params.field.id)))) return ;
        if (this.checkHadMe() && this.checkHadMySignField()){
            // this.notify.warn("Please provide your signature");
            if(this.selectedFieldId) this.pdfviewerControl.formDesignerModule.clearSelection(this.selectedFieldId);
            // this.pdfviewerControl.deleteAnnotations();
            this.OpenPopupSignature();
        }
    }

    chooseSignatureFromDbClickAnnotation(params?){
        setTimeout(()=>{
            this.pdfviewerControl.enableShapeAnnotation = true;
            this.pdfviewerControl.enableAnnotation = true;
            this.pdfviewerControl.isFormDesignerToolbarVisible = true;
            if(this.pdfviewerControl.annotationCollection.length > 0){
                this.pdfviewerControl.annotationModule.selectAnnotation(this.pdfviewerControl.annotationCollection[0].annotationId);
            }
            if(this.selectedFieldId) this.pdfviewerControl.formDesignerModule.clearSelection(this.selectedFieldId);

        },1)
        // if (this.signerData.some(e => e.signers.some(p => p.id == abp.session.userId && !p.formFields.some(k => k.fieldId == this.selectedFieldId)))) return ;
        // if (this.checkHadMe() && this.checkHadMySignField()){
        //     this.OpenPopupSignature();
        //     if(this.pdfviewerControl.annotationCollection.length > 0){
        //         this.pdfviewerControl.annotationModule.deleteAnnotationById(this.pdfviewerControl.annotationCollection[0].annotationId);
        //     }
        // }



        // // console.log(params)
    }

    checkFieldSignature() {

        let missingSigners = [];
        this.signerData.forEach(step => {
            step.signers.forEach(signer => {
                if (!signer.numOfSign || signer.numOfSign == 0) missingSigners.push(signer)
            })
        })
        if(missingSigners.length === this.signerData.reduce((a,b) =>  { return a + b.signers.length ; }, 0)){
            this.notify.warn(this.l("NoPositionInPdf"));
        }
        else if(missingSigners.length > 0){
            this.missingSignature.show(missingSigners);
        }
        else{
            if (this.checkHadMe() && this.checkHadMySignField()){
                if(this.selectedFieldId){
                    //this.pdfviewerControl.formDesignerModule.clearSelection(this.selectedFieldId);
                    if(this.pdfviewerControl.annotationCollection.length) this.pdfviewerControl.annotationModule.deleteAnnotationById(this.pdfviewerControl.annotationCollection[0].annotationId);
                }


                // có chữ kí của người tạo
                // kiểm tra chữ kí số và chữ kí thường
                if (this.documentData?.isDigitalSignature){ // là chữ kí số.


                    let _datacheck = this.setSaveRequestData();
                    _datacheck.category = this.dataForSave?.listCategoryId?.join(",");
                    _datacheck.systemId = 1; //esign
                    _datacheck.statusType = 1; //on progress
                    _datacheck.expectedDate = this.documentData.expectedDate; // --> Datetime to Datetime

                    this.spinnerService.show();
                    this.requestWeb.validateDigitalSignature(_datacheck)
                    .pipe(finalize(()=>{
                        this.spinnerService.hide();
                    }))
                    .subscribe(result => {
                        //this.notify.success(this.l("CreateDocumentSuccess"));
                        if(result){
                            if( result.code == "Error") {
                                let _message = "";
                                if(result.errMsgDigitalSignature != "") _message = result.errMsgDigitalSignature;
                                if(result.errMsgDigitalSignatureExpired != "") _message = result.errMsgDigitalSignatureExpired;

                                this.confirmFileExists.show(this.l('DigitalSignature'), _message, 'DigitalSignature');
                            }
                            else {

                                // check chữ kí số ok kí luôn
                                this.spinnerService.show();
                                this.requestWeb.createOrEditEsignRequest(_datacheck)
                                .pipe(finalize(()=>{
                                    this.spinnerService.hide();
                                }))
                                .subscribe(res => {
                                    this.notify.success(this.l("CreateDocumentSuccess"));
                                    this.local.removeItem("documentData");
                                    setTimeout(() => {
                                        this.router.navigate(['/app/main/document-management']);
                                    }, 200);
                                });
                            }
                        }
                    });
                    /*
                    (err) => {

                        let _err_digital = localStorage.getItem("CODE_268"); // // console.log(_err_digital)
                        if(_err_digital) {
                            localStorage.removeItem("CODE_268");
                            let _d = JSON.parse(_err_digital)
                            document.getElementById("messageCustom").style.display = "none";
                            this.confirmFileExists.show(this.l('DigitalSignature'), _d.message, 'DigitalSignature');
                        }

                        //269 chữ kí số hết hạn
                        let _err_digital_expired = localStorage.getItem("CODE_269"); // // console.log(_err_digital_expired)
                        if(_err_digital_expired) {
                            localStorage.removeItem("CODE_269");
                            let _d = JSON.parse(_err_digital_expired)
                            document.getElementById("messageCustom").style.display = "none";
                            this.confirmFileExists.show(this.l('DigitalSignature'), _d.message, 'DigitalSignature');
                        }
                    });*/
                }
                else { // là chữ kí thường
                    this.OpenPopupSignature();
                }


                // this._mstEsignUserImage.getListSignatureByUserIdForWeb().subscribe(res =>{
                //     if(res && res.items.length == 0) {
                //         this.notify.warn("Please provide your signature");
                //         if(this.selectedFieldId) this.pdfviewerControl.formDesignerModule.clearSelection(this.selectedFieldId);
                //         this.OpenPopupSignature();
                //     }
                //     else {
                //         let data = this.setSaveRequestData()
                //         this.confirm.show(data);
                //     }
                // });
            }
            else{
                // let data = this.setSaveRequestData()
                this.saveRequest()
                // this.confirm.show(data);
            }

        }
    }

    //1: Signature, 2: Name, 3: Title, 4: Date Signed, 5: Text, 6: Company
    getTypeId(id){
        switch(id){
            case "signature":
                return 1;
            case "name": return 2;
            case "title":return 3;
            case "datesigned": return 4;
            case "company": return 6;
            case "text":return 5;
            default :return 1
        }
    }

    setSaveRequestData(){
        //save request data
        this.dataForSave = Object.assign(new CreateOrEditEsignRequestDto(),this.documentData);

        if (this.checkHadMySignField()) {
            if(this.SignatureType == 1){
                this.dataForSave.templateSignatureId = this.SignatureTemplateID;
                this.dataForSave.typeSignId = this.SignatureType;
            }
            else {
                this.dataForSave.imageSign = this.SignatureImageBase64.replace('data:image/png;base64,','');;
                this.dataForSave.typeSignId = this.SignatureType;
            }
        }

        this.dataForSave.projectScheduleFrom = this.documentData.projectScheduleFrom ? this.documentData.projectScheduleFrom : undefined;//? moment(this.documentData.projectScheduleFrom).add(1,'month').toISOString() as any : undefined,
        this.dataForSave.projectScheduleTo = this.documentData.projectScheduleTo ? this.documentData.projectScheduleTo : undefined; //? moment(this.documentData.projectScheduleTo).add(1,'month').toISOString() as any : undefined,


        this.dataForSave.documents = [] //CreateOrEditDocumentDto;
        this.dataForSave.signers = [] //CreateOrEditSignersDto;
        this.dataForSave.listCategoryId = [];
        this.documentData?.listCategoryId?.forEach(e => {
            this.dataForSave?.listCategoryId?.push(parseFloat(e))
        })

        this.signerData?.filter(step => step.signers.some(p => p?.numOfSign && p?.numOfSign > 0))
        .forEach((e, index) => {
            e.signers.forEach(p => {
                let signerData = new CreateOrEditSignersDto();
                signerData.userId = p?.id;
                signerData.signingOrder = index + 1;
                signerData.colorId = p?.colorId ?? 1;
                signerData.privateMessage = p?.note;
                if(p?.numOfSign && p?.numOfSign > 0)
                {
                    this.dataForSave.signers.push(signerData)
                }
            })
        });


        this.dataForSave.stepList = this.dataForSave?.stepList?.filter(e => e.signers.some(p => p.numOfSign > 0)).map(e => {
            return {
                ...e,
                signers: e.signers.filter(p => p?.numOfSign > 0)
            }
        })
        this.dataForSave.itemList = this.documentData?.itemList?.map(e => {
            return {
                ...e,
                signatureCount: this.signerData?.reduce((a,b) =>  { return a.concat(b?.signers) ; }, [])
                            .filter(p => p?.formFields?.some(k => k.documentId == e.id))?.reduce((a,b) =>  { return a.concat(b?.formFields) }, []).filter(k => k?.documentId == e.id)?.length,
                signers: this.signerData?.reduce((a,b) =>  { return a.concat(b?.signers) ; }, [])
                            .filter(p => p.formFields?.some(k => k?.documentId == e.id))
            }
        });
        this.fileData?.forEach((e,i) =>{
            let documentData = new CreateOrEditDocumentDto();
            documentData.documentOrder = i + 1;
            documentData.documentTempId = e.id;
            documentData.id = e.id;
            documentData.positions = []; //CreateOrEditPositionsDto

            this.signerData?.forEach(k =>{
                k.signers?.forEach(s => {
                    s.formFields?.forEach(field => {
                        let zoomValue = parseFloat(this.pdfviewerControl.zoomValue.toString().replace("%",""));
                        if (e.id == field.documentId && field.pageNumber >= 1 && field.pageNumber <=  e.totalPageNumber)
                        {
                            let newPos = new CreateOrEditPositionsDto();
                            newPos.documentId = e.id;
                            newPos.pageNum = field.pageNumber;
                            newPos.signerId = s.id;
                            newPos.id = 0;
                            // newPos.signatureImage = e.id;
                            // newPos.positionX = k.bounds.x;
                            // newPos.positionY = k.bounds.y;
                            newPos.positionX = Math.round(field.position.x);
                            newPos.positionY = Math.round(field.position.y);
                            newPos.positionW =  Math.round(field.position.w)
                            newPos.positionH =  Math.round(field.position.h)
                            newPos.backGroundColor = field.style.backgroundColor ?? "";
                            newPos.textName = field.style?.name ?? "";
                            newPos.textValue = field.style?.value ?? "";
                            newPos.rotate = field.position?.rotate ?? 0;
                            newPos.isBold = field.style?.isBold;
                            newPos.isItalic = field.style?.isItalic;
                            newPos.isUnderline = field.style?.isUnderline;
                            newPos.textAlignment = field.style?.alignment ?? "center";
                            newPos.typeId = this.getTypeId(field.style?.name);
                            newPos.fontSize = field?.style?.fontSize ? (Math.ceil(Number(field?.style?.fontSize)) == 11 ? 10 : Math.ceil(Number(field?.style?.fontSize))) : 8
                            documentData.positions.push(newPos);
                        }

                    })

                })
            })
            this.dataForSave.documents.push(documentData);

            // let pageSignature = this.pdfviewerControl.formFields.filter(p => p.pageNumber >= (lastPageNum + 1) && p.pageNumber <= (lastPageNum+ e.page))
            // lastPageNum = e.page ;

        })

        return this.dataForSave;


    }

    lightenColor(originalColor, percentage) {
        // Convert the original color to RGB format
        originalColor = originalColor.replace(/^#/, '');
        const r = parseInt(originalColor.slice(0, 2), 16);
        const g = parseInt(originalColor.slice(2, 4), 16);
        const b = parseInt(originalColor.slice(4, 6), 16);

        // Calculate the new RGB values to make it lighter
        const newR = Math.min(255, r + (255 - r) * percentage);
        const newG = Math.min(255, g + (255 - g) * percentage);
        const newB = Math.min(255, b + (255 - b) * percentage);

        // Convert the new RGB values to a hexadecimal color code
        const lighterColor = `#${Math.round(newR).toString(16)}${Math.round(newG).toString(16)}${Math.round(newB).toString(16)}`;
        return lighterColor;
      }


      checkAndSave(){
        // if (this.signerData.some(e => e.signers.some(p => p.id == abp.session.userId && !p.formFields.some(k => k.fieldId == this.selectedFieldId)))) {
        //     this.saveRequest();
        // } ;
        if (this.checkHadMe() && this.checkHadMySignField()){
            this.OpenPopupSignature();
            if(this.pdfviewerControl.annotationCollection.length > 0){
                this.pdfviewerControl.annotationModule.deleteAnnotationById(this.pdfviewerControl.annotationCollection[0].annotationId);
            }
        }
        else {
            this.saveRequest();
        }
      }

    saveRequest(param?, _isDigitalSignatureSubmitAnyway?, _isDigitalSignatureSubmitChange?){


        if(param){
            this.hasSignature = true;
            this.tabMode = param.tabMode; // template. upload, draw
            // // this.isSignature = param.isSignature;
            this.SignatureType = param.SignatureType;
            this.SignatureImageBase64 = param.SignatureImageBase64;
            this.SignatureTemplateID = param.SignatureTemplateID;
            this.isSaveUploadSignature = param.isSaveUploadSignature;
        }


        let saveData = this.setSaveRequestData();
        saveData.category = this.dataForSave?.listCategoryId?.join(",");
        saveData.systemId = 1; //esign
        saveData.statusType = 1; //on progress
        saveData.expectedDate = this.documentData.expectedDate; // --> Datetime to Datetime

        // trường hợp submitAnyway với chữ kí số
        if(_isDigitalSignatureSubmitAnyway) saveData.isDigitalSignatureSubmitAnyway = true;
        else saveData.isDigitalSignatureSubmitAnyway = false;
        // trường hợp submitChange với chữ kí số tự chuyển về chữ kí thường
        if(_isDigitalSignatureSubmitChange) saveData.isDigitalSignature = false;

        // saveData.projectScheduleFrom = this.documentData.projectScheduleFrom ? moment(this.documentData.projectScheduleFrom).add(1,'month').toISOString() as any : undefined,
        // saveData.projectScheduleTo = this.documentData.projectScheduleTo ? moment(this.documentData.projectScheduleTo).add(1,'month').toISOString() as any : undefined,
        // // console.log(saveData)
        // return;


        this.spinnerService.show();
        this.requestWeb.createOrEditEsignRequest(saveData)
        .pipe(finalize(()=>{
            this.spinnerService.hide();
        }))
        .subscribe(res => {
            this.notify.success(this.l("CreateDocumentSuccess"));
            this.local.removeItem("documentData");
            setTimeout(() => {
                this.router.navigate(['/app/main/document-management']);
            }, 200);
        },
        (err) => {
            //check digital signature
            //268 không có chữ kí số
            let _err_digital = localStorage.getItem("CODE_268"); // // console.log(_err_digital)
            if(_err_digital) {
                localStorage.removeItem("CODE_268");
                let _d = JSON.parse(_err_digital)
                document.getElementById("messageCustom").style.display = "none";
                this.confirmFileExists.show(this.l('DigitalSignature'), _d.message, 'DigitalSignature');
            }

            //269 chữ kí số hết hạn
            let _err_digital_expired = localStorage.getItem("CODE_269"); // // console.log(_err_digital_expired)
            if(_err_digital_expired) {
                localStorage.removeItem("CODE_269");
                let _d = JSON.parse(_err_digital_expired)
                document.getElementById("messageCustom").style.display = "none";
                this.confirmFileExists.show(this.l('DigitalSignature'), _d.message, 'DigitalSignature');
            }
        });

    }


    ngDoCheck(): void {
        //Called every time that the input properties of a component or a directive are checked. Use it to extend change detection by performing a custom check.
        //Add 'implements DoCheck' to the class.
        //this.pdfviewerControl?.magnification?.fitToPage();

    }

    checkHadMe(){
        if (this.signerData.some(e => e.signers.some(p => p.id == abp.session.userId))) return true;
        else return false;
    }

    checkHadMySignField(){
        if (this.signerData.some(e => e.signers.some(p => p.id == abp.session.userId && p.formFields.length > 0))) return true;
        else return false;
    }

    objselectFieldSignature: any;
    OpenPopupSignature(_signatureId?,x?,y?) {

        // this.objselectFieldSignature = this.listFieldSignatureForSigner.find(e => e.positionX == x && e.positionY == y);
        let _pop = document.querySelector<HTMLElement>(".popup-signature");
        _pop.style.display = 'flex';

        // this.popupSignature.show()
    }

    objSaveSignature: MstEsignUserImageSignatureInput = new MstEsignUserImageSignatureInput();
    isSaveUploadSignature = false;
    SignatureTemplateID = 0;
    SignatureImageBase64 ;
    SaveImageSignatureTrimBase64;
    SignatureType = 1;
    hasSignature = false;
    tabMode;

    pendding_rendering_field_html(_second?){

        setTimeout(() => {
            // this.loaded_PDF_field();
            this.updateFormFieldIds();
            this.loaded_HTLM_field();
            setTimeout(() => {
            }, 500);
        }, (!_second)? 2000: _second);
    }

    loaded_HTLM_field () {
        if (this.hasSignature){
            let d = document.querySelectorAll('#pdfViewer .foreign-object > .foreign-object > .foreign-object');
            for(let i=0; d[i]; i++) {

                let _field_node_html = d[i].parentElement;
                let input =_field_node_html.querySelector('input');

                //1: Signature, 2: Name, 3: Title, 4: Date Signed, 5: Text, 6: Company
                // if (input.getAttribute("name").includes("signature")) {
                    let _field_node_html_id = _field_node_html.getAttribute('id');

                    // var x = parseFloat(_field_node_html.style.getPropertyValue("left")?.replace("px",""))
                    // var y = parseFloat(_field_node_html.style.getPropertyValue("top")?.replace("px",""))

                    let group = this.signerData.find(e => e.signers.some(p => p.id == this.appSession.userId));
                    let me = group?.signers?.find(e => e.id == this.appSession.userId)

                    // let listFieldData = me.formFields;
                    let listField = me?.formFields.filter(e => e.name?.includes("signature")).map(e => e.fieldId + "_content_html_element")

                    let _field = listField?.find(e => e == _field_node_html_id);
                    let _fieldData = me?.formFields.find(e => e == _field_node_html_id.split("_")[0]);

                    let currentFieldId = _field?.split("_")[0];

                    // if(!_field) this.loaded_PDF_field();

                    if(!_field) continue;

                    //kiểm tra đã load field chưa, có rồi thì bỏ qua
                    // if(_field_node_html.classList.contains('loaded') == true) continue;

                    let _img = _field_node_html.querySelectorAll<HTMLElement>(".img_signature");
                    for(let i=0; _img[i]; i++) { _img[i].remove(); }

                    _field_node_html.innerHTML  += '<img class="img_signature img_signature_'+ currentFieldId +' " style="position: absolute; z-index: 2;transform: rotate(' + 0 + 'deg); " src="'+this.SignatureImageBase64+'"/>';
                    _field_node_html.classList.add('fieldSignature-item','loaded', 'field_signature_id_' + currentFieldId);

                    let signatureTopHeader = document.getElementById( currentFieldId + "_designer_name");
                    if (signatureTopHeader) signatureTopHeader.style.opacity = '0';


                // }
            }
        }

    }


    pendding_rendering_preview_field_html(_second?){
        // this.spinnerService.show();
        setTimeout(() => {
            // this.loaded_PDF_field();
            this.loaded_preview_PDF_field_data();
            this.loaded_preview_HTLM_field_data();
        }, (!_second)? 2000: _second);
    }

    loaded_preview_PDF_field_data() {
        // let formFieldPDF = this.pdfviewerControl.retrieveFormFields();
        let formFieldPDF = this.pdfviewerPreviewControl.formFields;
        // this.loaded_PDF_field_number = formFieldPDF.length;

        for(let i=0; formFieldPDF[i]; i++) {
            // if(formFieldPDF[i].name.includes("signature")) {
                let _x = Math.round(formFieldPDF[i].bounds.x); // (formFieldPDF[i].bounds as any).x
                let _y = Math.round(formFieldPDF[i].bounds.y);  //(formFieldPDF[i].bounds as any).y
                let _field = this.listFieldSignatureForSigner.find(e => e.positionX ==  _x &&
                                                                        e.positionY == _y &&
                                                                        e.pageNum == formFieldPDF[i].pageNumber
                                                                        );
                if(!_field) continue;

                // // console.log(_field)


                _field.htmlId = formFieldPDF[i].id + "_content_html_element";

            // }
        }
    }


    loaded_preview_HTLM_field_data () {
        let d = document.querySelectorAll('#pdfviewerPreview .foreign-object > .foreign-object > .foreign-object');
        for(let i=0; d[i]; i++) {


            let _field_node_html = d[i].parentElement;
            let input =_field_node_html.querySelector('input');

            let elemId = _field_node_html.getAttribute('id');
            let fielData = this.listFieldSignatureForSigner.find(e => e.htmlId == elemId);
            // // console.log(this.listFieldSignatureForSigner)
            // // console.log(fielData)
            if (fielData?.rotate && Number(fielData?.rotate) != 0 ) this.setupRotationForField(elemId.split('_')[0],fielData.rotate,fielData?.backgroundColor)

            //1: Signature, 2: Name, 3: Title, 4: Date Signed, 5: Text, 6: Company
            if (input.getAttribute("name").includes("signature") && fielData.signerId == abp.session.userId) {
                let _field_node_html_id = _field_node_html.getAttribute('id');

                let _field = this.listFieldSignatureForSigner.find(e => e.htmlId == _field_node_html_id);
                // if(!_field) this.loaded_PDF_field();

                if(!_field) continue;

                //kiểm tra đã load field chưa, có rồi thì bỏ qua
                // if(_field_node_html.classList.contains('loaded') == true) continue;

                let _img = _field_node_html.querySelectorAll<HTMLElement>(".img_signature");
                for(let i=0; _img[i]; i++) { _img[i].remove(); }

                // let image = document.createElement('img')
                // image.setAttribute("src",this.SignatureImageBase64)
                const img = new Image();

                img.src = this.SignatureImageBase64;
                let data = this.SignatureImageBase64;

                // img.onload = function() {
                //     const imgWidth = img.naturalWidth;
                //     const imgHeight = img.naturalHeight;


                //     let height = imgHeight < _field.positionH ? 'auto' : ((((_field.rotate+90)%180 == 0) ? (_field.positionW) : _field.positionH  ) + 'px') ;
                //     let width = imgWidth < _field.positionW ? 'auto' : ((((_field.rotate+90)%180 == 0) ? ( _field.positionH ) : _field.positionW) + 'px')  ;

                //     _field_node_html.children[0].innerHTML  += '<img class="img_signature img_signature_'+ _field.id +' " style="position: absolute; z-index: 2;transform: rotate(' + _field.rotate + 'deg); height:'+height+';width:'+width+';" src="'+data+'"/>';
                //     //_field_node_html.lastElementChild
                //     _field_node_html.children[0].classList.add('fieldSignature-item-add-sign','loaded', 'field_signature_id_' + _field.id);
                // };

                let zoomValue = parseFloat(this.pdfviewerPreviewControl.zoomValue.toString().replace("%",""));

                let width1 = (((_field.rotate+90)%180 == 0) ? ( _field.positionH ) : _field.positionW)
                let height1 = (((_field.rotate+90)%180 == 0) ? (_field.positionW) : _field.positionH)

                // let newWidth =  (width1/zoomValue)*(100-zoomValue) - width1;
                // let newHeight = (height1/zoomValue)*(100-zoomValue) - height1;
                // // console.log(_field.rotate)
                // // console.log(_field.positionW,_field.positionH)

                let newWidth =  (width1*(zoomValue/100));
                let newHeight = (height1*(zoomValue/100));

                // let newWidth =  (_field.positionW*(zoomValue/100));
                // let newHeight = (_field.positionH*(zoomValue/100));

                let height =  (newHeight + 'px') ;
                let width =  (newWidth + 'px')  ;

                _field_node_html.children[0].innerHTML  += '<img class="img_signature img_signature_'+ _field.id +' " style="position: absolute; z-index: 2;transform: rotate(' + _field.rotate + 'deg); height:'+height+';width:'+width+';" src="'+this.SignatureImageBase64+'"/>';
                //_field_node_html.lastElementChild
                _field_node_html.children[0].classList.add('fieldSignature-item-add-sign','loaded', 'field_signature_id_' + _field.id);


            }
        }

    }

    getImageDimensions(file) {
        return new Promise (function (resolved, rejected) {
          var i = new Image()
          i.onload = function(){
            resolved({w: i.width, h: i.height})
          };
          i.src = file
        })
      }


    // rotateBase64Image(base64data,rotateAngle, callback) {
    //     const canvas = document.createElement("canvas");
    //     const ctx = canvas.getContext("2d");
    //     const image = new Image();
    //     image.src = base64data;

    //     canvas.height = image.width;
    //     canvas.width = image.height;

    //     // image.onload = () => {
    //     //         ctx.translate(image.height, image.width);
    //     //         ctx.rotate(rotateAngle * Math.PI / 180);
    //     //         ctx.drawImage(image, 0, 0);
    //     //         return canvas.toDataURL()
    //     // };

    //     // image.onload = () => {
    //     //     ctx.translate(image.height, image.width);
    //     //     ctx.rotate(rotateAngle * Math.PI / 180);
    //     //     ctx.drawImage(image, 0, 0);
    //     //     resolve(canvas.toDataURL())
    //     //   };
    //     return new Promise(resolve => {
    //         image.onload = () => {
    //             ctx.translate(image.height, image.width);
    //             ctx.rotate(rotateAngle * Math.PI / 180);
    //             ctx.drawImage(image, 0, 0);
    //             resolve(canvas.toDataURL())
    //           };
    //     })
    // }


    loaded_preview_HTLM_field () {
        if (this.hasSignature){
            let d = document.querySelectorAll('#pdfviewerPreview .foreign-object > .foreign-object > .foreign-object');
            for(let i=0; d[i]; i++) {

                let _field_node_html = d[i].parentElement;
                let input =_field_node_html.querySelector('input');

                //1: Signature, 2: Name, 3: Title, 4: Date Signed, 5: Text, 6: Company
                // if (input.getAttribute("name").includes("signature")) {
                    let _field_node_html_id = _field_node_html.getAttribute('id');

                    // let listField = me.formFields.map(e => e.fieldId + "_content_html_element")

                    let _field = this.listPreviewField.find(e => e.hasSign == true && e.field.id + "_content_html_element" == _field_node_html_id);

                    let currentFieldId = _field?.field?.id
                    // if(!_field) this.loaded_PDF_field();


                    if(!_field) continue;

                    //kiểm tra đã load field chưa, có rồi thì bỏ qua
                    // if(_field_node_html.classList.contains('loaded') == true) continue;

                    let _img = _field_node_html.querySelectorAll<HTMLElement>(".img_signature");
                    for(let i=0; _img[i]; i++) { _img[i].remove(); }

                    _field_node_html.innerHTML  += '<img class="img_signature img_signature_'+ currentFieldId +' " style="position: absolute; z-index: 2; transform: rotate(' + 0 + 'deg);" src="'+this.SignatureImageBase64+'"/>';
                    _field_node_html.classList.add('fieldSignature-item','loaded', 'field_signature_id_' + currentFieldId);

                    let signatureTopHeader = document.getElementById( currentFieldId + "_designer_name");
                    if (signatureTopHeader) signatureTopHeader.style.opacity = '0';
                // }
            }
        }

    }

    changeFilePreview(document){
        this.selectedDocId = document.id;

        this.pdfviewerPreviewControl?.load(document.documentPath,null);

        //this.setupFieldForPreview();
    }

    onApplyTemplateSignature(param){
        this.hasSignature = true;
        this.tabMode = param.tabMode; // template. upload, draw
        // // this.isSignature = param.isSignature;
        this.SignatureType = param.SignatureType;
        this.SignatureImageBase64 = param.SignatureImageBase64;
        this.SignatureTemplateID = param.SignatureTemplateID;
        this.isSaveUploadSignature = param.isSaveUploadSignature;

        // for(let i=0; this.listFieldSignatureForSigner[i]; i++){
        //     this.listFieldSignatureForSigner[i].signatureImage = this.SignatureImageBase64;
        //     this.listFieldSignatureForSigner[i].isSignature = true;
        // }

        this.pendding_rendering_field_html(200);

    }


    onApplyDrawText_Signature(param){
        this.hasSignature = true;
        this.tabMode = param.tabMode;
        // this.isSignature = param.isSignature;
        this.SignatureType = param.SignatureType;
        this.SignatureImageBase64 = param.SignatureImageBase64;
        this.SaveImageSignatureTrimBase64 = param.SaveImageSignatureTrimBase64;
        this.isSaveUploadSignature = param.isSaveUploadSignature;


        this.pendding_rendering_field_html(200);

    }

    onApplyDrawSignatureDefault(param){
        this.hasSignature = true;
        this.tabMode = param.tabMode;
        // this.isSignature = param.isSignature;
        this.SignatureType = param.SignatureType;
        this.SignatureImageBase64 = param.SignatureImageBase64;
        this.SaveImageSignatureTrimBase64 = param.SaveImageSignatureTrimBase64;
        this.isSaveUploadSignature = param.isSaveUploadSignature;



        this.pendding_rendering_field_html(200);
    }



    onApplyUploadSignature(param){
        this.hasSignature = true;
        this.tabMode = param.tabMode;
        // this.isSignature = param.isSignature;
        this.SignatureType = param.SignatureType;
        this.SignatureImageBase64 = param.SignatureImageBase64;
        this.SaveImageSignatureTrimBase64 = param.SaveImageSignatureTrimBase64;
        this.isSaveUploadSignature = param.isSaveUploadSignature;



        this.pendding_rendering_field_html(200);
    }

    onSaveSignatureDefault(){

        //lưu chữ kí
        if(this.isSaveUploadSignature) {

            if(this.tabMode == "TEMPLATE") { } // chưa làm
            else if(this.tabMode == "DRAW") {
                // this.SignatureImageBase64
                let img = <HTMLImageElement>document.getElementById('_SaveImageSignature'); //xử lý lấy widht, height, size

                if(img && this.SaveImageSignatureTrimBase64 != ''){
                    img.src = this.SaveImageSignatureTrimBase64;
                    setTimeout(() => {
                        this.objSaveSignature.signerId = abp.session.userId;
                        this.objSaveSignature.imgWidth = img.naturalWidth;
                        this.objSaveSignature.imgHeight = img.naturalHeight;
                        this.objSaveSignature.imgSize = 0;
                        this.objSaveSignature.imageSignature = this.SaveImageSignatureTrimBase64.replace('data:image/png;base64,','');

                        this._mstEsignUserImage.saveImageSignature(this.objSaveSignature).subscribe(e => {

                            //lấy list signature default -> show lại list signature
                            // this._mstEsignUserImage.getListSignatureByUserIdForWeb().subscribe(res =>{
                            //     if(res) {
                            //         this.listTemplateSignature = res.items;
                            //     }
                            // });
                        });
                    }, 200);

                }
            }
            else if(this.tabMode == "UPLOAD") {
                let img = <HTMLImageElement>document.getElementById('_SaveImageSignature');
                if(img && this.SaveImageSignatureTrimBase64 != ''){
                    img.src = this.SaveImageSignatureTrimBase64;
                    setTimeout(() => {
                        this.objSaveSignature.signerId = abp.session.userId;
                        this.objSaveSignature.imgWidth = img.naturalWidth;
                        this.objSaveSignature.imgHeight = img.naturalHeight;
                        this.objSaveSignature.imgSize = 0;
                        this.objSaveSignature.imageSignature = this.SaveImageSignatureTrimBase64.replace('data:image/png;base64,','');

                        this._mstEsignUserImage.saveImageSignature(this.objSaveSignature).subscribe(e => {

                            //lấy list signature default -> show lại list signature
                            // this._mstEsignUserImage.getListSignatureByUserIdForWeb().subscribe(res =>{
                            //     if(res) {
                            //         this.listTemplateSignature = res.items;
                            //     }
                            // });
                        });
                    }, 200);
                }
            }
        }
    }

    saveDraftData(){
        let draf = this.setSaveRequestData();

        draf.id = this.documentData.id;

        draf.category = this.documentData.listCategoryId.join(",");
        draf.systemId = 1; //esign
        draf.statusType = 0; //on progress
        // draf.listCategoryId = [];
        // this.documentData?.categoryList?.forEach(e => {
        //     if(!draf?.listCategoryId?.some(p => p == parseFloat(e))) draf?.listCategoryId?.push(parseFloat(e))
        // })
        // alert(this.documentData.expectedDate)
        draf.expectedDate = this.documentData.expectedDate; // --> Datetime to Datetime


        if (draf.id && draf.id > 0){
            this.spinnerService.show();
            this._requestWebService.saveDraftRequest(draf)
            .pipe(finalize(()=>{
                this.spinnerService.hide();
            }))
            .subscribe(res => {
                this.notify.success(this.l("SavedSuccessfully"));
            })
        }
    }

    listFieldSignatureForSigner: any[] = [];

    setUpListSignatureForPreview(){
        this.spinnerService.show();

        this._requestWebService.getEsignPositionsByRequestId(this.documentData?.id)
        .pipe(finalize(()=>{
            this.spinnerService.hide();
        }))
        .subscribe(res => {

            res.items.map(e => {
                this.listFieldSignatureForSigner.push(Object.assign(e, {
                    htmlId: null,
                    // isSignature: false
                }));
            })
        })
    }

    setUpFieldForSigner(){
        this.spinnerService.show();
        this._requestWebService.getEsignPositionsByRequestId(this.documentData?.id)
        .pipe(finalize(()=>{
            this.spinnerService.hide();
            this.updateFormFieldIds();
        }))
        .subscribe(res => {

            res.items.map(e => {
                this.listFieldSignatureForSigner.push(Object.assign(e, {
                    htmlId: null,
                    // isSignature: false
                }));
            })

            this.signerData.forEach(e => {
                e.signers.forEach(p =>{
                    // p.formFields = [];
                    p.formFields.forEach((l,i) => {
                        if (!this.documentData?.itemList.some(f => f.id == l.documentId)) p.formFields.splice(i,1)
                        // p.numOfSign = p.formFields?.length;
                    })
                    res.items.filter(k => k.signerId == p.id ).forEach((c,index) => {
                        if (this.documentData?.itemList.some(f => f.id == c.documentId)){
                            if (!p.formFields.some(s =>  Math.round(s.position.x) ==  Math.round(c.positionX)
                                && Math.round(s.position.y) ==  Math.round(c.positionY)
                                && s.documentId == c.documentId
                                && s.pageNumber == c.pageNum
                            ))
                            {
                                p.formFields?.push({
                                    // position:{x: Math.round(c.positionX),y :  Math.round(c.positionY)},
                                    documentId :c.documentId,
                                    position: {
                                        x:Math.round(c.positionX),
                                        y:Math.round(c.positionY),
                                        w:Math.round(c.positionW),
                                        h:Math.round(c.positionH),
                                        rotate: c.rotate,
                                    },
                                    style: {
                                        isBold: false,
                                        isItalic: false,
                                        isUnderline: false,
                                        alignment: c.textAlignment,
                                        color: "black",
                                        name : 'signature',
                                        value: c.textValue,
                                        typeId : 1,
                                        backgroundColor: c.backgroundColor,
                                        fontSize :  8
                                    },
                                    pageNumber: c.pageNum,
                                    fieldId : this.pdfviewerControl.formFields.find(f => f.bounds.x == c.positionX &&  f.bounds.y == c.positionY && f.pageNumber == c.pageNum)?.id ,
                                    field: this.pdfviewerControl.formFields.find(f => f.bounds.x == c.positionX &&  f.bounds.y == c.positionY && f.pageNumber == c.pageNum)
                                })

                            }
                        }
                        p.numOfSign = p.formFields?.length;
                    })
                })
            })
        })
    }

    replaceCC(value){
        if(!value){
            return  '';
        }
        else {
            return value?.replace(/;/g, ', ');
        }
    }

    submitAnyway(){
        // if (this.checkHadMe() && this.checkHadMySignField() && !this.hasSignature){
        //     // this.pdfviewerControl.deleteAnnotations();
        //     this._mstEsignUserImage.getListSignatureByUserIdForWeb().subscribe(res =>{
        //         if(res && res.items.length == 0) {
        //             this.notify.warn("Please provide your signature");
        //             if(this.selectedFieldId) this.pdfviewerControl.formDesignerModule.clearSelection(this.selectedFieldId);
        //             this.OpenPopupSignature();
        //         }
        //         else {
        //             let data = this.setSaveRequestData()
        //             this.confirm.show(data);
        //         }
        //     });
        // }
        // else{
        //     let data = this.setSaveRequestData()
        //     this.confirm.show(data);
        // }
        if (this.checkHadMe() && this.checkHadMySignField()){
            if(this.selectedFieldId){
                this.pdfviewerControl.formDesignerModule.clearSelection(this.selectedFieldId);
                if(this.pdfviewerControl.annotationCollection.length) this.pdfviewerControl.annotationModule.deleteAnnotationById(this.pdfviewerControl.annotationCollection[0].annotationId);
            }
            this.OpenPopupSignature();

        }
        else{
            this.saveRequest()
        }
    }


    formatDate(input, onlyMonth){
        if(input){
            if(onlyMonth){
                return moment(input).format('MMM YYYY');
            }
            else{
                return moment(input).format('DD MMM YYYY');
            }
        }
        else return '';
    }


    confirmFileExistsNo(e) {

    }
    confirmFileExistsYes(e) {

    }

    signAnyway(e){
        //thay đổi chữ kí số thành chữ kí điện tử
        if(e == 'submitChange') {
            this.documentData.isDigitalSignature = false;
            this.checkFieldSignature();
        }
        else if (e == 'submitAnyway') this.saveRequest(undefined, true, false);
    }


    // handleMouseMove(event: MouseEvent) {
    //     this.zone.runOutsideAngular(() => {
    //       // Code that should not trigger change detection
    //     });
    //   }

}
