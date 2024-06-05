import { CreatOrEditEsignReferenceRequestDto, EsignDocumentListUpdateNameInputDto, EsignReferenceRequestDto, EsignReferenceRequestServiceProxy, EsignRequestSearchValueListRequestDto, EsignSignerSearchHistoryServiceProxy } from './../../../../shared/service-proxies/service-proxies';
import { DataFormatService } from './../../../shared/common/services/data-format.service';
import { AfterViewInit, Component, Injector, OnInit, QueryList, ViewChild, ViewChildren } from "@angular/core";
import { AppComponentBase } from "@shared/common/app-component-base";
import {CurrencyPipe} from '@angular/common';
import { CdkDragEnd, CdkDropList, moveItemInArray, transferArrayItem } from "@angular/cdk/drag-drop";
import { HttpClient, HttpEventType, HttpRequest } from "@angular/common/http";
import { EventBusService } from "@app/shared/common/services/event-bus.service";
import { RemoveDocumentComponent } from "../esign-modal/remove-document/remove-document.component";
import { asapScheduler, finalize, forkJoin } from "rxjs";
import { LocalStorageService } from "@shared/utils/local-storage.service";
import { Router } from "@angular/router";
import { CreateOrEditDocumentDto, CreateOrEditEsignRequestDto, CreateOrEditPositionsDto, CreateOrEditSignersDto, EsignDocumentListWebServiceProxy, EsignRequestServiceProxy, EsignRequestWebServiceProxy, EsignSignerListServiceProxy, EsignSignerTemplateLinkCreateNewRequestForWebDto, EsignSignerTemplateLinkCreateNewRequestv1Dto, EsignSignerTemplateLinkListSignerForWebDto, EsignSignerTemplateLinkServiceProxy, MstEsignActiveDirectoryServiceProxy, MstEsignCategoryServiceProxy, MstEsignColorServiceProxy } from "@shared/service-proxies/service-proxies";
import * as moment from "moment";
import { CommonFunction } from "@app/main/commonfuncton.component";
import { MenuComponent } from '@syncfusion/ej2-angular-navigations';
import { SearchSignerComponent } from './search-signer/search-signer.component';
import { DateTime } from 'luxon';
import { ReviewComponent } from '../document-managerment/review/review.component';
import { ConfirmFileExistsComponent } from '../esign-modal/confirm-file-exists/confirm-file-exists.component';
@Component({
  selector: "app-create-new-request",
  templateUrl: "./create-new-request.component.html",
  styleUrls: ["./create-new-request.component.scss"],
    // animations: [appModuleAnimation()],
    //   providers: [CheckBoxSelectionService]
//   encapsulation: ViewEncapsulation.Emulated
})

export class CreateNewRequestComponent extends AppComponentBase implements OnInit,AfterViewInit {
    @ViewChild('removeModal') removeModal!: RemoveDocumentComponent;
    @ViewChild('menuMore') menuMore!: MenuComponent;
    @ViewChild('searchSigner') searchSigner!: SearchSignerComponent;
    @ViewChild('reviewModal') reviewModal!: ReviewComponent;
    @ViewChild('confirmFileExists') confirmFileExists!: ConfirmFileExistsComponent;

    // drop file list
    @ViewChildren(CdkDropList) private dlfile: QueryList<CdkDropList>;
    // --> end

    rangeDates: Date[] = [];

    documentData : any = new CreateOrEditEsignRequestDto();

    searchSignerData = [];
    selectedCategory = [];
    data;

    today = new Date();

    @ViewChildren(CdkDropList) private dlq: QueryList<CdkDropList>;
    private readonly CHUNK_SIZE = 100 * 1024; // 100kb
    private readonly STORAGE_KEY = 'chunked_file';

    public dls: CdkDropList[] = [];

    searchResultName = [];
    searchResultEmail = [];

    itemList: any[] = [
        // { fileName: "Get_App.pdf",size : 5.21},
        // { fileName: "Get_App.pdf",size : 5.21},

    ];

    // data: string[] = ['Snooker', 'Tennis', 'Cricket', 'Football', 'Rugby'];

    public categoryList: any[] =  [
        // { id: '1', category: 'Ringi' },
        // { id: '7', category: 'Frame Contract' },
        // { id: '3', category: 'Normal Contract' },
        // { id: '4', category: 'Advance Request' },
        // { id: '5', category: 'Orther' }
    ];
    // maps the appropriate column to fields property
    public fields: Object = { text: 'category', value: 'id' };

    isCC = false;
    ccText = "";
    cc;
    showCalendar = true;
    coloList = [];
    posList = [];

    // tempListCategoryId = [];

    colorStepList = [
        '#FBEEB0',
        '#ABFFB9',
        "#AEE7FD",
        "#F1D3FF",
        "#ACBDFF",
        '#B0FBED',

        '#FEF9E4',
        '#E0FFE6',
        '#E3F7FE',
        '#F7E6FF',
        '#E6EBFF',
        '#E6EBFE',
    ];

    stepList = [
        {
            signers: [
                {
                    fullName: "",
                    email: "",
                    // title: 'CEO',
                    // imgUrl: '/assets/common/images/avata-none.png',
                    // color: '#dc60eb',
                    // backgroundColor: '#ffe6ff',
                    // department: "Risk Management/Planning" ,
                    // position:"Manager/Department Head",
                    // note : "Please strong support us to sign this document" ,
                    // private:false,
                    // numOfSign: 0,
                    // check: false,
                    // formFields: []
                } as any,
            ]
        },

        // { user: [
        //     {name : "Pham Thi Xuan" ,email: "xuanpt@toyotavn.com.vn",department: "Risk Management/Planning" , position:"Manager/Department Head", note : "Please strong support us to sign this document"  ,private:false},
        //     {name : "Pham Thi Xuan" ,email: "xuanpt@toyotavn.com.vn",department: "Risk Management/Planning" , position:"Manager/Department Head", note : "Please strong support us to sign this document" ,private:false },
        // ]},
    ];

    _fn: CommonFunction = new CommonFunction();
    isAddMe = false;
    selectedCC: any[] | undefined;

    totalCostTemp;
    constructor(injector: Injector,private _http: HttpClient,
        private _eventBus : EventBusService,
        private local: LocalStorageService,
        private activeDirectoryService : MstEsignActiveDirectoryServiceProxy,
        private colorService : MstEsignColorServiceProxy,
        private router: Router,
        private _requestService: EsignRequestServiceProxy,
        private _requestWebService: EsignRequestWebServiceProxy,
        private _category: MstEsignCategoryServiceProxy,
        private _signerList: EsignSignerListServiceProxy,
        private _documentList: EsignDocumentListWebServiceProxy,
        private currencyPipe : CurrencyPipe,
        private signerHis : EsignSignerSearchHistoryServiceProxy,
        private _esignSignerTemplateLink: EsignSignerTemplateLinkServiceProxy,
        private dataFormatService : DataFormatService,
        private refrence : EsignReferenceRequestServiceProxy,
    ) {
        super(injector);
        this._fn.isShowUserProfile();

    }

    scrollFunction() {
        const button = document.getElementById("nextAction");
        let body_scroll = document.getElementsByClassName("frame-body-scroll")[0];

        if (body_scroll.scrollTop > 40 ) {
            // button.classList.add("visible");
            button.style.display = "flex"; // Display the button
        } else {
            // button.classList.remove("visible");
            button.style.display = "none"; // Hide the button
        }
    }

    showSummary(){
        if(this.categoryList.filter(e => this.documentData?.listCategoryId?.some(p => p == e.id))?.some(e => e.isMadatory == true)) {
            return true;
        }
        else
        {
            this.documentData.totalCost = undefined;
            this.documentData.roi = undefined;
            this.documentData.content = undefined;
            this.documentData.projectScheduleFrom = undefined;
            this.documentData.projectScheduleTo = undefined;
            this.rangeDates = [];
            return false;
        }
    }
    // transformAmount(element){
    //     this.documentData.totalCost = ;
    // }

    // changeValue(event){
    //     const value = event ? (event.target as HTMLInputElement).value : null;
    //     const newvalue = this.documentData.totalCost?.replaceAll(',', '');
    //     // if (parseInt(value) < this.min) {
    //     //     this.documentData.totalCost = this.min;
    //     // }
    //     this.documentData.totalCost = this.dataFormatService?.moneyFormat(Number(newvalue) ?? 0);
    // }

    ngOnInit() {
        // this.local.removeItem("documentData")
        this.getCache();

        // this.local.getItem("documentData", (err, data) =>{
        //     this.documentData = Object.assign(data, new CreateOrEditEsignRequestDto() )
        //     this.documentData.expectedDate  = moment(this.documentData.expectedDate)
        //     this.documentData.projectScheduleFrom  = moment(this.documentData.projectScheduleFrom)
        //     this.documentData.projectScheduleTo  = moment(this.documentData.projectScheduleTo)
        //     this.rangeDates.push(this.documentData.projectScheduleFrom,this.documentData.projectScheduleTo)
        //     this.stepList = this.documentData?.stepList?.length > 0 ? this.documentData?.stepList : this.stepList;
        //     this.itemList = this.documentData?.itemList.length > 0 ? this.documentData?.itemList.filter(e => e.uploadStatus != 'failed') : this.itemList;
        // })
    }

    // lấy dữ liệu từ db
    getDraftRequest(selectedId){
        this.local.removeItem("documentData");
        this.spinnerService.show();
        this._requestWebService.getRequestSummaryById(selectedId)
        .pipe(finalize(()=>{

            //gán expectedDate từ db vào ô input    // mặc định expectedDate từ localStorge và db lấy lên là Datetime
            if(this.documentData.expectedDate) {
                let a = this.documentData.expectedDate as DateTime;
                let b = new Date(a.year, a.month -1,a.day, 10, 0, 0);
                this.expectedDateInput = this.documentData.expectedDate ? moment(b).toDate() as Date : null; // gán vào ô input -- > dung
            }

            this.rangeDates = (this.documentData.projectScheduleFrom && this.documentData.projectScheduleTo) ? [] : null;
            if (this.rangeDates){
                let f = this.documentData.projectScheduleFrom as DateTime;
                let f1 = new Date(f.year, f.month-1,1);
                let t = this.documentData.projectScheduleTo as DateTime;
                let t1 = new Date(t.year, t.month-1,1);
                // this.rangeDates.push(moment(f1).add(-1,'month').toDate() as Date); // deo hieu tai sao luc can tru 1, luc khong can tru 1
                this.rangeDates.push(f1); // deo hieu tai sao luc can tru 1, luc
                this.rangeDates.push(t1);
            }

            this.stepList = this.documentData?.stepList?.length > 0 ? this.documentData?.stepList : this.stepList;
            this.itemList = this.documentData?.itemList?.length > 0 ? this.documentData?.itemList.filter(e => e.uploadStatus != 'failed') : this.itemList;
           
            this.setListDrop();
        }))
        .subscribe(res => {

            this.documentData = res;
            this.documentData.listCategoryId = res.categoryList;
            this.selectedCC = res?.addCC?.includes('@') ? res.addCC?.split(";").map(e => {
                return {
                    email: e,
                    imgUrl: this.searchSignerData.find(k => k.email == e)?.imgUrl,
                    fullName: this.searchSignerData.find(k => k.email == e)?.fullName,
                    id: this.searchSignerData.find(k => k.email == e)?.id,
                }
            }) : [];
            // this.tempListCategoryId = res.categoryList;
            // this.documentData.title = res.title;
            // this.documentData.totalCost = res.totalCost;
            // this.documentData.roi = res.roi;
            // this.documentData.projectScheduleFrom = res.projectScheduleFrom;
            // this.documentData.projectScheduleTo = res.projectScheduleTo;
            // this.documentData.content = res.content;
            // this.documentData.message = res.message;
            // this.documentData.category = res.category;
            // // this.documentData.categoryList = res.categoryList;
            this.documentData.expectedDate = res.expectedDate; // ??
            // this.documentData.isDigitalSignature = res.isDigitalSignature;

        });

        forkJoin([
            this._signerList.getListSignerByRequestId(selectedId),
            this._documentList.getEsignDocumentByRequestId(selectedId),
            this._requestWebService.getEsignPositionsByRequestId(selectedId),
            this.refrence.getReferenceRequestByRequestId(selectedId)
        ])
        .pipe(finalize(()=>{
            this.spinnerService.hide();
        }))
        .subscribe(res2 => {

            this.stepList = [];
            let maxSigningOrder = res2[0].items.reduce((max, p) => p?.signingOrder > max ? p?.signingOrder : max, res2[0]?.items[0]?.signingOrder);
            for(let i = 0; i < maxSigningOrder; i++){
                this.stepList.push({
                    signers: [
                        ...res2[0].items.filter(e => e?.signingOrder == i + 1).map(e => {
                            return {
                                id: e.userId,
                                fullName: e.fullName,
                                email: e.email,
                                title: e.title,
                                formFields: [],
                                imgUrl: e.imgUrl,
                                backgroundColor: e.color,
                                note: e.privateMessage,
                            };
                        })
                    ]
                });
            }

            res2[1].items.map(e => {
                this.itemList.push({
                    id: e.id,
                    name: e.documentName,
                    documentPath: e.documentPath,
                    order: e.documentOrder,
                    page: e.totalPage,
                    fileSize: e.totalSize +" kb",
                    uploadStatus: 'success',
                });
            });

            res2[2].items.forEach(k => {
                let newPos = new CreateOrEditPositionsDto();
                newPos.documentId = k.documentId;
                newPos.pageNum = k.pageNum;
                newPos.signerId = k.signerId;
                newPos.id = 0;
                // newPos.signatureImage = e.id;
                // newPos.positionX = k.bounds.x;
                // newPos.positionY = k.bounds.y;
                newPos.positionX = Math.round(k.positionX);
                newPos.positionY = Math.round(k.positionY);
                newPos.positionW =  Math.round(k.positionW)
                newPos.positionH =  Math.round(k.positionH)
                newPos.backGroundColor = k.backgroundColor ?? "";
                newPos.textName = k.textName ?? "";
                newPos.textValue = k.textValue ?? "";
                newPos.rotate = k.rotate;
                newPos.isBold = false;
                newPos.isItalic = false;
                newPos.isUnderline = false;
                newPos.textAlignment = k.textAlignment ?? "center";
                newPos.typeId = 1;

                this.posList.push(newPos)
            })

            this.references = res2[3].items;
        });


    }

    expectedDateInput;
    projectScheduleFromInput;
    projectScheduleToInput;
    // get từ local ra thì lại dùng moment().toDate() , get từ db ra thì dùng moment bị lệch
    getFromLocal(){
        this.local.getItem("documentData", (err, data) =>{

            this.documentData = Object.assign(this.documentData, new CreateOrEditEsignRequestDto() ,data );

            //gán expectedDate từ db vào ô input    // mặc định expectedDate từ localStorge và db lấy lên là Datetime
            if(this.documentData.expectedDate) {
                this.expectedDateInput = this.documentData.expectedDate ? moment(this.documentData.expectedDate).toDate() as Date : null; // gán vào ô input
            }

            // this.documentData.projectScheduleFrom  = moment(this.documentData.projectScheduleFrom).toDate() as Date;
            // this.documentData.projectScheduleTo  = moment(this.documentData.projectScheduleTo).toDate() as Date;
            this.rangeDates = (this.documentData.projectScheduleFrom && this.documentData.projectScheduleTo)  ? [] : null;
            if (this.rangeDates){
                this.rangeDates.push(this.documentData.projectScheduleFrom ? moment(this.documentData.projectScheduleFrom).toDate() as Date: null );
                this.rangeDates.push(this.documentData.projectScheduleTo ? moment(this.documentData.projectScheduleTo).toDate() as Date : null);
            }

            this.stepList = this.documentData?.stepList?. length > 0 ? this.documentData?.stepList : this.stepList;
            this.references = this.documentData?.ref?. length > 0 ? this.documentData?.ref : this.references;
            this.itemList = this.documentData?.itemList?.length > 0 ? this.documentData?.itemList.filter(e => e.uploadStatus != 'failed') : this.itemList;
          
            this.isSaveTemplateSingerCC = this.documentData?.isSaveTemplateSingerCC;
            this.ccText = this.documentData?.ccText;
            this.selectedCC = this.documentData?.addCC?.includes('@') ? this.documentData?.addCC?.split(";").map(e => {
                return {
                    email: e,
                    imageUrl: this.searchSignerData.find(k => k.email == e)?.imgUrl,
                    fullName: this.searchSignerData.find(k => k.email == e)?.fullName,
                    id: this.searchSignerData.find(k => k.email == e)?.id,
                }
            }) : [];
            this._requestWebService.getEsignPositionsByRequestId(data?.id)
            .subscribe(res =>{
                res.items.forEach(k => {
                    let newPos = new CreateOrEditPositionsDto();
                    newPos.documentId = k.documentId;
                    newPos.pageNum = k.pageNum;
                    newPos.signerId = k.signerId;
                    newPos.id = 0;
                    // newPos.signatureImage = e.id;
                    // newPos.positionX = k.bounds.x;
                    // newPos.positionY = k.bounds.y;
                    newPos.positionX = Math.round(k.positionX);
                    newPos.positionY = Math.round(k.positionY);
                    newPos.positionW =  Math.round(k.positionW)
                    newPos.positionH =  Math.round(k.positionH)
                    newPos.backGroundColor = k.backgroundColor ?? "";
                    newPos.textName = k.textName ?? "";
                    newPos.textValue = k.textValue ?? "";
                    newPos.rotate = k.rotate;
                    newPos.isBold = false;
                    newPos.isItalic = false;
                    newPos.isUnderline = false;
                    newPos.textAlignment = k.textAlignment ?? "center";
                    newPos.typeId = 1;

                    this.posList.push(newPos)
                })
            })

            this.setListDrop();

        });
    }

    signerHistory = [];

    getCache(){


        this.spinnerService.show();
        forkJoin([
            this.colorService.getAllColor("",""),
            this.activeDirectoryService.getAllSignersForWeb( "", undefined, 0,1000000000 ) ,
            this._category.getAllCategories("",""),
            this.signerHis.getSignerSearchHistory(),
        ])
        .pipe(finalize(()=>{

            this.spinnerService.hide();
            let selectedId = 0;
            this.router.routerState.root.queryParams
            .subscribe(params => {
                selectedId = Number(params['requestId']);
                if(selectedId){
                    this.getDraftRequest(selectedId);
                }
                else {
                    this.getFromLocal()
                }
            });
        }))
        .subscribe(res => {

            this.coloList = res[0].items;

            this.searchSignerData = [];

            res[1].items.forEach(e => {
                this.searchSignerData.push({
                    id: e.id,
                    fullName: e.fullName,
                    imgUrl: e.imageUrl,
                    title: e.title,
                    department: e.department,
                    email: e.email,
                    colorId: 0,
                    // department: "" ,
                    position:e.position,
                    note : "" ,
                    private:false,
                    check: false,
                    numOfSign: 0,
                    formFields: [],
                    division: e.division,
                    company: e.company,
                    manager : e.manager,
                    unsignedFullName: e.unsignedFullName,
                    // email: 'hnthulm@toyotavn.com.vn'
                });
            });

            this.signerHistory = res[3].items;


             //= res.items.filter(item => item.fullName.toLowerCase().includes(event.query.toLowerCase()));
                this.categoryList = [];
            res[2].items.forEach(e => {
                this.categoryList.push(
                    { id: e.id, category: e.internationalName, isMadatory : e.isMadatory },
                );
            });
        });
    }




    ngAfterViewInit() {

        // let cc = document.getElementById("inputCC")
        // let dropChild =  cc.children[0].children[0];
        // dropChild.setAttribute("cdkDropList","true");
        // dropChild.setAttribute("cdkDropListConnectedTo","dls");
        // dropChild.addEventListener("onCdkDropListDropped",this.dropCC.bind(this));

        if (this.documentData?.itemList?.length <= 0 || this.documentData?.itemList?.some(e => e.uploadStatus != 'success')) return;

        //this.local.setItem('documentData',this.documentData);


       this.setListDrop();

       //this.setListDropFilePdf();
    }

    setListDrop(){
        setTimeout(()=>{
            let ldls: CdkDropList[] = [];

            this.dlq?.forEach((dl) => {
            ldls.push(dl);
            });

            ldls = ldls.reverse();

            asapScheduler.schedule(() => { this.dls = ldls; });
        });
    }

    setListDropFilePdf() {
        setTimeout(()=>{
            let listfiles: CdkDropList[] = [];

            this.dlfile?.forEach((item) => {
                listfiles.push(item);
            });

            listfiles = listfiles.reverse();

            //asapScheduler.schedule(() => { this.dlfile = listfiles; });
        });
    }

    getRandomColor() {
        var color = Math.floor(0x1000000 * Math.random()).toString(16);
            return '#' + ('000000' + color).slice(-6);
        }

    _filename_old:string;
    changeFileName(indexs) {

        this._filename_old = this.itemList[indexs].name;

        let fileDoc = document.querySelector('.pdf-filename_' + indexs);
        if(fileDoc) {
            fileDoc.classList.add('edit');
            let fileInput = document.querySelector<HTMLInputElement>('.pdf-filename_' + indexs + ' input');
            if(fileInput) fileInput.select();
        }
    }
    renameOutFocus(indexs) {
        let fileDoc = document.querySelector('.pdf-filename_' + indexs);
        if(fileDoc) {
            let fileInput = document.querySelector<HTMLInputElement>('.pdf-filename_' + indexs + ' input');
            this.itemList[indexs].name = fileInput.value + '.pdf';
            fileDoc.classList.remove('edit');

            if(this._filename_old.toLowerCase() != fileInput.value.toLowerCase() + '.pdf' &&
               this.itemList[indexs].id > 0) {

                let _filebody = new EsignDocumentListUpdateNameInputDto();
                _filebody.id = this.itemList[indexs].id;
                _filebody.documentName = this.itemList[indexs].name;

                this._documentList.updateDocumentNameById(_filebody) //3627
                .subscribe(res => {
                    this.local.setItem('documentData',Object.assign({},this.documentData));
                });
            }
        }
    }

    removeFileItem(indexs){
        if(indexs.length == 1){
            this.itemList.splice(indexs[0],1);
            this.setLocalData();
        }
        else if (indexs.length > 0)
        {
            this.itemList.splice(0,this.itemList.length);
        }



        // const chunks = [];
        // let i = 0;
        // var chunk ;
        // this.local.getItem(fileName + "-chunk-" + i,(er,data)=>{
        //     if (!data) chunk = undefined
        //     else chunk = data;
        // });

        // setTimeout(() => {
        //     this.removeFileChunk(fileName + "-chunk-",0)
        // })


    }

    removeRefItem(indexs){
        if(indexs.length == 1){
            this.references.splice(indexs[0],1);
            this.setLocalData();
        }
        else if (indexs.length > 0)
        {
            this.references.splice(0,this.itemList.length);
        }

    }

    removeFileChunk(name,i){
        this.local.getItem(name+i,(er,data)=>{
            if(data) {
                this.local.removeItem(data.key);
                this.removeFileChunk(name, i+1);
            }
        });
    }

    isGroup(item: any): boolean {
        // return item?.group?.length > 1 ? true : false
        return true;
    }

    isSaveTemplateSingerCC:boolean = false;
    _dataSaveSingerCC: EsignSignerTemplateLinkCreateNewRequestForWebDto;
    _dataSaveSingerCC_Signer: EsignSignerTemplateLinkListSignerForWebDto[];
    focusCC(params){
        this.isSaveTemplateSingerCC = params;
        if (this.isSaveTemplateSingerCC) {
            document.getElementById("ccInput").focus();
        }
        else {
            this.ccText = null;
        }
    }

    addUser(seq, data? ){
        this.stepList.find((e,i) => (i+1) == seq).signers.push(
            {
                fullName: '',
                email: "",
                // name: '',
                // email: "",
                // title: '',
                // imgUrl: 'https://i.pinimg.com/564x/89/8c/e8/898ce8537c9d8192175319d9c48cb029.jpg',
                // color: '',
                // backgroundColor: '',
                // department: "" ,
                //  position:"",
                //  note : "" ,
                // private:false,
                // check: false,
                // numOfSign: 0,
                // formFields: []
            } as any
        );
       this.setListDrop();

    }
    // uploadUrl: string = AppConsts.remoteServiceBaseUrl + '/AttachFile/UploadFileToFolder';

    onFocusOut(){
        console.log('focus out');
    }

    onKeyDown(event){
        if(event.keyCode == 13){
            console.log('enter');
        }
    }
    references = [];
    saveRefDocument(params){
        console.log(params)
        params?.forEach(e  => {
            this.references?.push(Object.assign(new EsignReferenceRequestDto(),{
                requestRefId : e.requestId,
                title : e.title,
            }))
        })
    }

    addStep(){

        this.stepList.push({
            signers: [
                {
                    fullName: '',
                    email: "",
                    // title: '',
                    // imgUrl: '/assets/common/images/avata-none.png',
                    // color: '',
                    // backgroundColor: '',
                    // department: "" ,
                    // position:"",
                    // note : "" ,
                    // private:false,
                    // check: false,
                    // numOfSign: 0,
                    // formFields: []
                } as any
            ]

        });
        this.setListDrop();
    }
    addMe(){
        let me = this.searchSignerData.find(e => e.id == abp.session.userId);
        this.stepList.unshift({
                    signers: [
                        {
                            id : abp.session.userId,
                            fullName: me?.fullName,
                            email: me?.email,
                            title: me?.title,
                            imgUrl: me?.imgUrl,
                            colorId: 0,
                            backgroundColor: '',
                            department: me?.department ,
                            position:me?.position,
                            division:me?.division,
                            manager:me?.manager,
                            company: me?.company,
                            note : "" ,
                            private:false,
                            numOfSign: 0,
                            check: false,
                            formFields: []
                        } as any
            ]
        });
        this.isAddMe = true;
        this.setListDrop();
    }

    nextAction(){
        if (!this.documentData?.title || this.documentData?.title?.trim() == "" ) return this.notify.warn(this.l("TitleCannotBeEmpty"));
        // validate
        if (this.checkHasMe() && this.stepList.findIndex(e => e.signers.some(p => p.id == abp.session.userId)) != 0) return this.notify.warn(this.l("YouMustBeTheFirstSignerInSignersList"));

        if(!this.formValidate()) return;

        let data = [];


        let i = 0;
        this.stepList = this.stepList.filter(e => !(e.signers.length == 1 && !e.signers[0].id))
        this.stepList.forEach(e => {
            e.signers = e.signers.filter(l => l.id)
            e.signers.forEach(p =>{
                // if(!p.id && e.signers?.length > 1) e.signers.splice(e.signers.findIndex(k => k.id == p.id ),1)
                // else if (!p.id && e.signers?.length == 1){
                //     this.stepList.splice(this.stepList.findIndex(s => !s.signers[0].id),1)
                // }
                // else{
                    Object.assign(p,{
                        colorId: this.coloList[i%this.coloList?.length]?.id,
                        color : this.coloList[i%this.coloList?.length]?.code
                    });
                // }

                i++;
            });
        });

        this.documentData = Object.assign(this.documentData,{
            categoryList: this.documentData?.categoryList,
            listCategoryId: this.documentData?.listCategoryId,
            stepList: this.stepList,
            ref : this.references,
            itemList: this.itemList,
            projectScheduleFrom : this.documentData.projectScheduleFrom , // da duoc gan luc change data
            projectScheduleTo : this.documentData.projectScheduleTo,//moment(this.documentData.projectScheduleTo).toISOString(),
            addCC: this.selectedCC?.map(e => e.email)?.join(";") ?? '',
            // isSaveTemplateSingerCC: this.isSaveTemplateSingerCC,
            // ccText: this.ccText,
        });

        if (! this.documentData?.listCategoryId ||  this.documentData?.listCategoryId?.length <= 0) return this.notify.warn(this.l("PleaseChooseCategory"));
        // this.documentData.category = this.selectedCategory;

        if (this.documentData.itemList.length <= 0) return this.notify.warn(this.l("PleaseUploadFile"));
        if ( this.documentData.itemList?.some(e => e.uploadStatus != 'success')) return this.notify.warn(this.l("FileNotFullyUpload"));
        if (this.stepList.length == 0 ) return this.notify.warn(this.l("PleaseAddAtleast1SignerToContinue"));
        if (this.stepList?.some(e => e.signers?.some(k => !k.fullName && !k.email))) return this.notify.warn(this.l("PleaseFillAllSignersData"));

        // if(!this.documentData?.id || this.documentData?.id == 0){

        let draftId = 0;
        let _draftData = this.setDraftData();

        this.SaveTemplateSignerCC();

            this.spinnerService.show();
            this._requestWebService.saveDraftRequest(_draftData)
            .pipe(finalize(()=>{
                this.spinnerService.hide();
                this.documentData = Object.assign({},this.documentData ?? {id : draftId }, new CreateOrEditEsignRequestDto(), {id : draftId ,ref :  this.references} );
                    // this.local.setItem('documentData',this.documentData);

                    this.local.setItem('documentData',Object.assign({},this.documentData));
                this.router.navigate(['/app/main/add-signature']);

            }))
            .subscribe(res => {
                draftId = res;
                this.refrence.getReferenceRequestByRequestId(draftId).pipe(finalize(()=>{

                }))
                .subscribe(res => {

                    this.references = res.items
                    console.log(this.references)
                    this.documentData = Object.assign(this.documentData,{
                        categoryList: this.documentData?.categoryList,
                        listCategoryId: this.documentData?.listCategoryId,
                        stepList: this.stepList,
                        ref : this.references,
                        itemList: this.itemList,
                        projectScheduleFrom : this.documentData.projectScheduleFrom , // da duoc gan luc change data
                        projectScheduleTo : this.documentData.projectScheduleTo,//moment(this.documentData.projectScheduleTo).toISOString(),
                        addCC: this.selectedCC?.map(e => e.email)?.join(";") ?? '',
                        // isSaveTemplateSingerCC: this.isSaveTemplateSingerCC,
                        // ccText: this.ccText,
                    });
                    this.local.setItem('documentData',Object.assign({},this.documentData));
                })

            });
        // }



    }

    formValidate(){
        if(this.isSaveTemplateSingerCC) {   // có lưu template list signer, cc: validate name template
            let _ccInput = document.getElementById('ccInput');
            if( this.ccText == undefined || this.ccText?.trim() == ''){

                this.notify.warn(this.l('PleaseInputNameTemplateOfTheListOfSignersAndCC'));
                if(_ccInput) {
                    _ccInput.focus();
                    _ccInput.style.border = '1px solid #cf103b'
                }
                let _scroll = document.querySelector('.frame-body-scroll');
                if(_scroll) _scroll.scrollTo({
                    top: 800,
                    left: 0,
                    behavior: "smooth",
                });

                return false;
            }else {
                if(_ccInput)  _ccInput.style.border = '1px solid #dbe0f0';
                return true;
            }
        }
        return true;
    }
    setDraftData(){

        let newData = new CreateOrEditEsignRequestDto();
        newData.id = (!this.documentData?.id || isNaN(this.documentData?.id)) ? 0 : this.documentData?.id;
        Object.assign(newData, this.documentData);
        newData.title = this.documentData.title;
        newData.signers = [];
        newData.documents = [];
        newData.requestRefs = [];

        this.references.forEach((e: EsignReferenceRequestDto) => {
            // console.log(e)
            newData.requestRefs.push(Object.assign(new CreatOrEditEsignReferenceRequestDto(),{
                requestRefId : e.requestRefId,
                note : e.note,
                id : e.id,
                requestId : e.id,
            }))
        })

        // newData.category = this.selectedCategory.join(",");
        newData.listCategoryId = this.documentData?.listCategoryId;



        // gán expectedDate từ ô input lại local và db
        if(this.expectedDateInput) {

            // console.log(this.expectedDateInput);
            // console.log(this.expectedDateInput.constructor);

            let _dayX = 0;
            let _d = moment(this.expectedDateInput).toDate() as any;// fix không phải + - day
            // if(_d.getHours() != 10) _dayX = 1;
            let year = _d.getFullYear();
            let month = _d.getMonth() + 1;
            let day = _d.getDate();
            newData.expectedDate = _d ? new Date(year, month - 1, day, 10, 0, 0).toDateString() as any : undefined;
            this.documentData.expectedDate = newData.expectedDate;

            // console.log('----------------------2')
            // console.log(newData.expectedDate);
        }


        //this.documentData.expectedDate = this.expectedDateInput.toISOString() as any; // da duoc gan luc change value

        // value da duoc gan luc change value sử dụng toISOString() + 1 day
        if (this.rangeDates){
            newData.projectScheduleFrom = (this.rangeDates[0]) ? moment(this.rangeDates[0]).add(1,'day').toISOString() as any : undefined;
            newData.projectScheduleTo = (this.rangeDates[1]) ? moment(this.rangeDates[1]).add(1,'day').toISOString() as any  : undefined;
            this.documentData.projectScheduleFrom = newData.projectScheduleFrom;
            this.documentData.projectScheduleTo = newData.projectScheduleTo;
        }
        // this.selectedCategory?.forEach(e => {
        //     if(!newData?.listCategoryId?.some(p => p == parseFloat(e))) newData?.listCategoryId?.push(parseFloat(e))
        // })



        newData.statusType = 0;
        newData.systemId = 1;

        this.stepList?.forEach((e,i) => {
            e.signers.forEach(p => {
                let signerData = new CreateOrEditSignersDto();
                signerData.userId = p.id;
                signerData.signingOrder = i+1;
                signerData.colorId = p?.colorId ?? 1;
                signerData.privateMessage = p.note;
                newData.signers.push(signerData);
            });

        })


        this.itemList?.forEach((e,i,array) =>{
            let documentData = new CreateOrEditDocumentDto();
            documentData.documentOrder = i + 1;
            documentData.documentTempId = e.id;
            documentData.id = e.id;
            documentData.positions = this.posList.filter(k => k.documentId == e.id);
            //update color signer theo list signer moi
            documentData.positions.forEach(s => {
                let stepHasPos = this.stepList.find(k => k.signers.some(l => l.id == s.signerId))
                if (stepHasPos){
                    let signer = stepHasPos?.signers.find(l => l.id == s.signerId);
                    if (signer ){
                        s.backGroundColor = this.coloList.find(l => l.id == signer.colorId)?.code
                    }
                }
            })

            newData.documents.push(documentData);




                //CreateOrEditPositionsDto
            newData.addCC =  this.selectedCC?.map(e => e.email)?.join(";") ?? '';
            // let pageSignature = this.pdfviewerControl.formFields.filter(p => p.pageNumber >= (lastPageNum + 1) && p.pageNumber <= (lastPageNum+ e.page))
            // lastPageNum = e.page ;

        });

        return newData;

    }

    dragData;
    onDragStarted(event){
        this.setListDrop();

        this.dragData = event.source.data;
    }

    removeUser(stepIndex, stepUserIndex){
        if (this.stepList[stepIndex].signers[stepUserIndex].id == abp.session.userId){
            this.isAddMe = false;
        }
        if (this.stepList[stepIndex].signers.length == 1){
            // if (this.stepList[stepIndex].group.length > 1){
            //     this.stepList[stepIndex].group.splice(groupIndex,1);
            // }
            // else {
            //     this.stepList.splice(stepIndex,1);
            // }
            // return;
            return this.stepList.splice(stepIndex,1);
        }
        this.stepList[stepIndex].signers.splice(stepUserIndex,1);

        this.setListDrop();

    }

    addPrivateMessage(stepIndex, stepUserIndex){
        Object.assign(this.stepList[stepIndex].signers[stepUserIndex],{private:!this.stepList[stepIndex].signers[stepUserIndex]?.private});

        if (!this.stepList[stepIndex].signers[stepUserIndex]?.private){
            this.stepList[stepIndex].signers[stepUserIndex].note = "";
        }
    }

    checkHasMe(){
        if (this.stepList.some(e => e.signers.some(p => p.id == abp.session.userId))) return true ;
        else return false;
    }

    onDragEnded(event: CdkDragEnd): void {
        //event.source._dragRef.reset();
    }

    currentUserId = abp.session.userId;

    hasMe(params){
        if (params.some(e => e.id == abp.session.userId))return true;
        else return false;
    }

    drop(event,dropZone){
        // if (!event.item.data?.signers?.some(e => e.id == abp.session.userId)  && this.checkHasMe() && event.currentIndex == 0) return this.notify.warn("You must be the first signer");

        let dragFrom = event.previousContainer.data?.some(e => e['signers']);
        let dragTo = event.container.data?.some(e => e['signers']);

        if (!dragFrom && !dragTo){
            //if ((event.item.data?.id == abp.session.userId || event.item.data?.signers?.some(e => e.id == abp.session.userId) ) && this.checkHasMe() && event.currentIndex != 0) return this.notify.warn("You must be the first signer");
            if (event.previousContainer == event.container) {
                    moveItemInArray(event.container.data, event.previousIndex, event.currentIndex);
                } else {
                    transferArrayItem(event.previousContainer.data,
                        event.container.data,
                        event.previousIndex,
                        event.currentIndex);
                }
            // moveItemInArray(event.container.data, event.previousIndex, event.currentIndex);
        }
        else if (dragFrom && dragTo){
            transferArrayItem(event.previousContainer.data,
                  event.container.data,
                  event.previousIndex,
                  event.currentIndex);
        }else if (dragFrom && !dragTo){// drag parrent in to group child
            // if (( event.item.data?.signers?.some(e => e.id == abp.session.userId) ) && this.checkHasMe() && event.container.currentIndex != 0 ) return this.notify.warn("You must be the first signer");
            // if (!event.item.data?.signers?.some(e => e.id == abp.session.userId)  && this.checkHasMe() && event.currentIndex == 0) return this.notify.warn("You must be the first signer");
            event.previousContainer.data.splice(event.previousIndex,1);
            this.dragData.signers.forEach(e => {
                event.container.data.push(e);
            });

        }
        else if (!dragFrom && dragTo){// drag child out
            // if (( event.item.data?.signers?.some(e => e.id == abp.session.userId) ) && this.checkHasMe() && event.currentIndex != 0 ) return this.notify.warn("You must be the first signer");
            // if ( event.item.data?.id != abp.session.userId && this.checkHasMe() && event.currentIndex == 0) return this.notify.warn("You must be the first signer");
            let list = [];
            list.push({
                signers :[ this.dragData]
            });
            event.previousContainer.data.splice(event.previousIndex,1);
            transferArrayItem(list,
                event.container.data,
                event.previousIndex,
                event.currentIndex);


            // this.stepList.push(data)
        }

        this.setListDrop();


    }

    dropFile(event){
        // move the item in array
        moveItemInArray(this.itemList, event.previousIndex, event.currentIndex);
    }

    inputFile:any;
    uploadFile(){
        let input = document.createElement('input');
        input.type = 'file';
        input.accept = '.docx,.doc,.pdf,.xls,.xlsx,.png,.jpg,.jpeg';
        input.className = 'd-none';
        input.id = 'imgInput';
        input.multiple = true;




        input.onchange = () => {
            let files = Array.from(input.files);
            this.inputFile = files;
            if (this.confirmFileExistsChecked(files)) {
                this.confirmFileExists.show(this.l('Confirm'), this.l('ConfirmFilenameExists'), "DuplicatedFile");
            }
            else {
                this.onUpload(files);
            }
            // this.onUpload(files);
        };
        input.click();
    }

    dropHandler(ev) {
        // Prevent default behavior (Prevent file from being opened)
        ev.preventDefault();
        this.inputFile = [...ev.dataTransfer.files];
        if (this.confirmFileExistsChecked(this.inputFile)) {
            this.confirmFileExists.show(this.l('Confirm'), this.l('ConfirmFilenameExists'));
        }
        else {
            this.onUpload(this.inputFile);
        }
        // this.onUpload([...ev.dataTransfer.files]);
    }

    dragOverHandler(ev) {
        // Prevent default behavior (Prevent file from being opened)
        ev.preventDefault();
    }


    confirmFileExistsNo(e) {
        this.inputFile = undefined;
    }
    confirmFileExistsYes(e) {
        this.onUpload(this.inputFile);
    }
    confirmFileExistsChecked(e) {
        let isFileNamExists = false;

        if (this.inputFile.length > 0) {
            this.inputFile.forEach(f => {
                let formData: FormData = new FormData();
                let file = {
                    file: f,
                    // formData: formData,
                    name: f.name?.replaceAll("xlsx","pdf")?.replaceAll("xls","pdf")?.replaceAll("docx","pdf").replaceAll("doc","pdf"),
                    // page : 0,
                    // documentPath: "",
                    // id: 0,
                    // fileSize : this.getFileSize(f),
                };

                this.itemList.forEach(e => {
                    if(e.name == file.name) {
                        isFileNamExists = true;
                        return;
                    }
                });

            });
        }

        return isFileNamExists;
    }


    onUpload(files: Array<any>): void {
        if (files.length > 0) {
            files.forEach(f => {
                let formData: FormData = new FormData();
                let serverName =  Date.now().toString() + '_' + f.name;
                formData.append('file', f);

                let file = {
                    file: f,
                    formData: formData,
                    name: f.name?.replaceAll("xlsx","pdf")?.replaceAll("xls","pdf")?.replaceAll("docx","pdf")?.replaceAll("doc","pdf"),
                    page : 0,
                    documentPath: "",
                    id: 0,
                    fileSize : this.getFileSize(f),
                };



                this.itemList.push(file);
                let index = this.itemList.length-1;
                // var elem = document.getElementById("bar"+index);
                // elem.style.width = "0%";
                // this.itemList.push(file);
                // this.readFileAndUpdateInfo(file,this.itemList.length-1);

                setTimeout(() =>{
                    this.readFileAndUpdateInfo(file,index,f.size);
                } , 100);

                // this._http
                // .post<any>(this.uploadUrl, formData, {
                //     params: {}
                // })
                // .subscribe(k => {

                // });


            });
        }
    }


    readFileAndUpdateInfo(file,index,fileSize){
        // var input = document.getElementById("files");

        let formData: FormData = new FormData();
        formData.append('file', file.file);

        var elem = document.getElementById("bar"+index);

        // var reader = new FileReader();
        // // reader.readAsBinaryString(file?.file);
        // reader.onload = function(e){
        //     // var count = (reader.result as any).match(/\/Type[\s]*\/Page[^s]/g).length;
        //     // file.page = count;
        //     // return count;

        //     const base64String = (e.target.result as any).split(',')[1]; // Extract the Base64 data
        //     file.data = base64String;
        // }

        // reader.readAsDataURL(file.file);

       if (elem){
        const req = new HttpRequest('POST', this.uploadUrl, formData, {
            reportProgress: true,
        });

        let isErr = false;
        let isOnline = window.navigator.onLine;
        let percentage = 0; // Reset progress

        // Listen for online/offline events
        window.addEventListener('online', () => {
            isOnline = true;
        });

        window.addEventListener('offline', () => {
            isOnline = false;
        });

        this._http.request(req)
        .pipe(finalize(()=>{
            file.data = "";
            this.setLocalData();
        }))
        .subscribe((event) => {

            if(!isErr && isOnline){
                if (event.type === HttpEventType.UploadProgress) {
                    // file.data = this.base64StringToBlob(file.data)
                    // this.splitAndStoreFile(file.data,file.name);
                    if (isErr || !isOnline){
                        elem.style.backgroundColor = "red";
                        this.itemList[index].uploadStatus = "failed";
                        return;
                    }
                    else {
                        elem.style.backgroundColor = "#12d176";
                        this.itemList[index].uploadStatus = "uploading";
                        percentage = Math.round((100 * event.loaded) / event.total);
                        elem.style.width = percentage + "%";
                    }

                    // elem.style.width = percentage + "%";
                } else if (event.type === HttpEventType.Response ) {
                    percentage = 100;
                    elem.style.width = percentage + "%";
                    file.page = (event.body as any).result.totalPage;
                    this.itemList[index].page = file.page;
                    this.itemList[index].uploadStatus = "success";
                    this.itemList[index].id = (event.body as any).result.id;
                    this.itemList[index].documentPath = (event.body as any).result.documentPath;
                }
            }

        },err => {
            isErr = true;
        });

        // let percentage = 0; // Reset progress
        // elem.style.backgroundColor = "#12d176";

        // const fileSizeBytes = fileSize; // File size in bytes
        // const averageUploadSpeedBytesPerSecond = 200 * 1024; // Adjust as needed (e.g., 200 KB/s)
        // // Estimate the expected execution time in milliseconds
        // const expectedExecutionTimeMs =   (fileSizeBytes / averageUploadSpeedBytesPerSecond) * 1000; // Adjust as needed

        // // Calculate the progress step based on the expected time
        // const progressStep = 100 / (expectedExecutionTimeMs / 500);

        // let progressSubscription = timer(0, 500).subscribe(() => {
        //     if (percentage < (100 - progressStep)) {

        //         if (isErr || !isOnline){
        //             elem.style.backgroundColor = "red";
        //             this.itemList[index].uploadStatus = "failed";
        //             return;
        //         }
        //         else {
        //             elem.style.backgroundColor = "#12d176";
        //             this.itemList[index].uploadStatus = "uploading";
        //             percentage += progressStep;
        //             elem.style.width = percentage + "%";
        //         }

        //     } else {
        //         percentage = 100; // Ensure the progress bar reaches 100%

        //         // Stop the progress simulation
        //         if (progressSubscription) {
        //             progressSubscription.unsubscribe();
        //         }

        //     }
        // });
       }
        //
    }

    setLocalData(){
        // this.local.getItem("documentData",(err,data)=>{
        //     this.documentData['id'] = (data && data?.id) ? data?.id : 0
        // })
        this.documentData = Object.assign({},this.documentData ,{
            stepList: this.stepList,
            itemList: this.itemList
        });
        // alert(this.documentData.itemList)
        this.local.setItem('documentData',this.documentData);
    }

    getFileSize(file){
        var _size = file.size;
            var fSExt = new Array('Bytes', 'KB', 'MB', 'GB'),
            i=0;while(_size>900){_size/=1024;i++;}
            var exactSize = (Math.round(_size*100)/100)+' '+fSExt[i];
            return exactSize;
    }

    dragUserStart(event){
        event.preventDefault();
        event.stopPropagation();
    }
    // i = 0;
    // move(i) {
    //     if (this.i == 0) {
    //       this.i = 1;
    //       var elem = document.getElementById("bar"+i);
    //       elem.classList.add("myBar")
    //       var width = 1;
    //       var id = setInterval(frame, 10);
    //       function frame() {
    //         if (width >= 100) {
    //           clearInterval(id);
    //           const i = 0;
    //         } else {
    //           width++;
    //           elem.style.width = width + "%";
    //         }
    //       }
    //     }
    //   }

    // email!: string | undefined;
    // title!: string | undefined;
    // department!: string | undefined;
    // fullName!: string | undefined;
    // imageUrl!: string | undefined;
    // id!: number;

    filterUserName(event){
        this.searchResultName = event.query
        ? this.searchSignerData?.filter(item =>( item.fullName?.toLowerCase()?.includes(event.query?.toLowerCase()) || item.unsignedFullName?.toLowerCase().includes(event.query?.toLowerCase())) && !this.stepList?.some(e => e.signers?.some(p => p.email == item.email) ) && item.id != abp.session.userId )
        .sort((a,b)=>{
            let div = this.stepList[this.stepList?.length - 2]?.signers[0]?.title?.split('/')[ this.stepList[this.stepList?.length - 2]?.signers[0]?.title?.split('/').length -1]
            let div2 = a?.title?.split('/')[a?.title?.split('/').length - 1]
            let div3 = b?.title?.split('/')[b?.title?.split('/').length - 1]
            // console.log(this.stepList[0].signers[0])
            // console.log(this.stepList[this.stepList?.length - 1]?.signers)
            // console.log(div)
            // console.log(div2)
            if(div == div2 && div2 != div3) return -1;
            else return 1
        })
        .slice(0,5)
        : this.searchSignerData?.filter(item => this.signerHistory.some(e => e.userId == item.id) && !this.stepList?.some(e => e.signers?.some(p => p.email == item.email) ) && item.id != abp.session.userId ) ;
    }

    filterUserEmail(event){
        this.searchResultEmail = event.query
        ? this.searchSignerData?.filter(item => item.email?.toLowerCase()?.includes(event.query?.toLowerCase()) && !this.stepList?.some(e => e.signers?.some(p => p.email == item.email) ) && item.id != abp.session.userId )
        .sort((a,b)=>{
            let div = this.stepList[this.stepList?.length - 2]?.signers[0]?.title?.split('/')[ this.stepList[this.stepList?.length - 2]?.signers[0]?.title?.split('/').length -1]
            let div2 = a?.title?.split('/')[a?.title?.split('/').length - 1]
            let div3 = b?.title?.split('/')[b?.title?.split('/').length - 1]
            // console.log(this.stepList[0].signers[0])
            // console.log(this.stepList[this.stepList?.length - 1]?.signers)
            // console.log(div)
            // console.log(div2)
            if(div == div2 && div2 != div3) return -1;
            else return 1
        })
        .slice(0, 5)
        : this.searchSignerData?.filter(item => this.signerHistory.some(e => e.userId == item.id) && !this.stepList?.some(e => e.signers?.some(p => p.email == item.email) ) && item.id != abp.session.userId ).slice(0, 4);
    }

    onSelectSigner(event){
        if(event?.value){
            this.selectedCC = this.selectedCC?.filter(e => e.id != event?.value?.id);
        }
        else {
            this.selectedCC = this.selectedCC?.filter(e => e.id != event?.id);
        }
        if(this.stepList.length > 0 && !this.stepList[this.stepList.length-1].signers.some(e => !e.id || e.id == 0)){
            this.addStep();
        }
    }



    filterUserCC(event){
        this.searchResultEmail = this.searchSignerData?.filter(item => item.email?.toLowerCase()?.includes(event.query?.toLowerCase()) && !this.selectedCC?.some(p => p.email == item.email) && !this.stepList?.some(e => e.signers?.some(p => p.email == item.email)) && item.id != abp.session.userId ).slice(0, 5)
        .map(e => {
            return {
                ...e,
                imageUrl: e.imgUrl
            }});
    }

    preventFocusLoss(event: Event) {
        event.preventDefault();
    }

    trackByFn(index, item) {
        return item.id; // Replace 'id' with your unique identifier
    }

    selectedFiles = [];
    isDisableConvertPdf = true; //
    
    selectFileToMerge(params,fileIndex){
        // console.log(params, params.checked, fileIndex)
        if (params.checked){
            this.selectedFiles.push(this.itemList[fileIndex]) ;
        }
        else {
            // this.selectedFiles.splice(fileIndex,1)  // fileIndex là index của ListFile --> sai, cần Index của selectedFile
            this.selectedFiles = this.selectedFiles.filter(item => item !== this.itemList[fileIndex]);
        }
   
        //convert file
        this.isDisableConvertPdf = true;
        this.selectedFiles.forEach(e => {
            //convert file not type pdf //phuongdv - 2024-05-04
            let _types = e.name.split('.');
            if(_types[_types.length -1] != 'pdf') {  
                this.isDisableConvertPdf = false;
            }
        });
 
    }

    mergeDocumentFile(){
        let idList = [];
        this.selectedFiles.forEach(e => {
            idList.push(e.id)
        })

        const req = new HttpRequest('POST', this.mergeFile, idList,
        );

        this.spinnerService.show();
        this._http.request(req)

        // .post(this.mergeFile,{
        //     params: {
        //         'listId': idList,
        //     }
        //     // , responseType: 'blob'
        // })
        .pipe(finalize(()=>{
            this.spinnerService.hide();
            this._requestWebService.getEsignPositionsByRequestId(this.documentData?.id ?? 0)
            .pipe(finalize(()=>{
                this.spinnerService.hide();
                this.setLocalData();

            }))
            .subscribe(res2 => {

                if(res2){
                    res2.items.forEach(k => {
                        let newPos = new CreateOrEditPositionsDto();
                        newPos.documentId = k.documentId;
                        newPos.pageNum = k.pageNum;
                        newPos.signerId = k.signerId;
                        newPos.id = 0;
                        // newPos.signatureImage = e.id;
                        // newPos.positionX = k.bounds.x;
                        // newPos.positionY = k.bounds.y;
                        newPos.positionX = Math.round(k.positionX);
                        newPos.positionY = Math.round(k.positionY);
                        newPos.positionW =  Math.round(k.positionW)
                        newPos.positionH =  Math.round(k.positionH)
                        newPos.backGroundColor = k.backgroundColor ?? "";
                        newPos.textName = k.textName ?? "";
                        newPos.textValue = k.textValue ?? "";
                        newPos.rotate = k.rotate;
                        newPos.isBold = false;
                        newPos.isItalic = false;
                        newPos.isUnderline = false;
                        newPos.textAlignment = k.textAlignment ?? "center";
                        newPos.typeId = 1;

                        this.posList.push(newPos)
                    })
                }

            });
        }))
        .subscribe((event: any) => {
            // this.selectedFiles.forEach(e => {
            //     console.log(e)
            //     console.log(this.itemList.findIndex(p => e.id == p.id))
            //     this.itemList.splice(this.itemList.findIndex(p => e.id == p.id),1)
            // })
            if (event.type === HttpEventType.Response ){
                this.itemList = this.itemList.filter(e => !this.selectedFiles.some(p=> p.id == e.id))

                let data = event?.body?.result
                // console.log(event)
                if(data){
                        this.itemList.push({
                            documentPath: data?.documentPath,
                            fileSize: data?.fileSize ?? "--",
                            id: data?.id,
                            name: data?.fileName ?? "MergeDocument.pdf",
                            // order : 1,
                            page: data?.totalPage,
                            uploadStatus: "success"
                        })
                }

                this.selectedFiles = [];
                this.notify.success(this.l("MergeSuccessfully"))
            }

        });
    }

    removeAllFile(){
        let input = [];
        this.itemList.forEach((e,i)=>{
            input.push(i);
        })
        this.removeModal.show(input);
    }

    // phuongdv - 2024-05-03 add 
    ConvertToPDF(){
        let idList = [];
        this.selectedFiles.forEach(e => {
            //convert file not type pdf //phuongdv - 2024-05-03
            let _types = e.name.split('.');
            if(_types[_types.length -1] != 'pdf') { 
                idList.push(e.id);
            }
        })
 
        const req = new HttpRequest('POST', this.convertToPDF, idList,
        );

        this.spinnerService.show();
        this._http.request(req)

        // .post(this.mergeFile,{
        //     params: {
        //         'listId': idList,
        //     }
        //     // , responseType: 'blob'
        // })
        .pipe(finalize(()=>{
            this.spinnerService.hide(); 
        }))
        .subscribe((event: any) => {
            // this.selectedFiles.forEach(e => {
            //     console.log(e)
            //     console.log(this.itemList.findIndex(p => e.id == p.id))
            //     this.itemList.splice(this.itemList.findIndex(p => e.id == p.id),1)
            // });

            if (event.type === HttpEventType.Response ){

                idList.forEach(eSF => {
                    const foundIndex = this.itemList.findIndex(item => item.id === eSF);
                    if(foundIndex >= 0) 
                    {  
                        let _types =this.itemList[foundIndex].name.split('.');
                        let _type = '.' + _types[_types.length -1]; 
                        this.itemList[foundIndex].name = this.itemList[foundIndex].name.replaceAll(_type,'.pdf');  
                    }
                    //console.log(this.itemList[foundIndex]);
                });

                this.setLocalData();

                /*
                this.itemList = this.itemList.filter(e => !this.selectedFiles.some(p=> p.id == e.id))

                let data = event?.body?.result
                // console.log(event)
                if(data){
                        this.itemList.push({
                            documentPath: data?.documentPath,
                            fileSize: data?.fileSize ?? "--",
                            id: data?.id,
                            name: data?.fileName ?? "MergeDocument.pdf",
                            // order : 1,
                            page: data?.totalPage,
                            uploadStatus: "success"
                        });
                }
                */

                this.selectedFiles = []; 
                this.isDisableConvertPdf = true;
                this.notify.success(this.l("ConvertFilesSuccessfully"))
            }
        });
    }

    isDetectIcon(item, _type) {

        let _types = item.name.split('.');
        let _istype = _types[_types.length -1];
        if(_type.includes(_istype)) { 
            return true;
        }
        return false;         
    }

    submitSearchSigner(event){
        this.stepList = [];
        let listSigner = event.signers.filter(e => !this.stepList?.some(p => p.signers?.some(k => k.id == e.id)));
        let maxSigningOrder = listSigner.reduce((max, p) => p?.signingOrder > max ? p?.signingOrder : max, listSigner[0]?.signingOrder);
        for(let i = 0; i < maxSigningOrder; i++){
            if(listSigner.filter(e => e?.signingOrder == i + 1 )?.length)
            this.stepList.push({
                signers: [
                    ...listSigner
                    .filter(e => e?.signingOrder == i + 1 )
                    .map((e, index )=> {
                        return {
                            id: e.id,
                            fullName: e.fullName,
                            email: e.email,
                            title: e.title,
                            formFields: [],
                            imgUrl: e.imgUrl,
                            backgroundColor: e.colorCode ?? this.coloList[i * index + 1]?.code,
                            colorId: e.colorId ?? this.coloList[ i * index + 1]?.id,
                            note: e.privateMessage,
                        };
                    })
                ]
            });
        }
        this.selectedCC = event.addCC;
        if(this.stepList.some(e => e.signers.some(p => p.id === abp.session.userId))){
            this.isAddMe = true;
        }
        else {
            this.isAddMe = false;
        }

        setTimeout(()=>{
            this.addStep();
            this.setListDrop();
        })
    }

    saveDraftData(){
        if (!this.documentData?.title || this.documentData?.title?.trim() == "" ) return this.notify.warn(this.l("TitleCannotBeEmpty"));
        if (! this.documentData?.listCategoryId ||  this.documentData?.listCategoryId?.length <= 0) return this.notify.warn(this.l("PleaseChooseCategory"));
        // this.documentData.category = this.selectedCategory;

        // if (this.documentData.itemList.length <= 0) return this.notify.warn("Please upload File");
        // if ( this.documentData.itemList?.some(e => e.uploadStatus != 'success')) return this.notify.warn("File not fully upload");
        if (this.stepList.length == 0 ) return this.notify.warn(this.l("PleaseAddAtleast1SignerToContinue"));
        // if (this.stepList?.some(e => e.signers?.some(k => !k.fullName && !k.email))) return this.notify.warn("Please fill all signers data");
        let draftId = 0;

        let i = 0;
        this.stepList = this.stepList.filter(e => !(e.signers.length == 1 && !e.signers[0].id))
        this.stepList.forEach(e => {
            e.signers = e.signers.filter(l => l.id)
            e.signers.forEach(p =>{
                // if(!p.id && e.signers?.length > 1) e.signers.splice(e.signers.findIndex(k => k.id == p.id ),1)
                // else if (!p.id && e.signers?.length == 1){
                //     this.stepList.splice(this.stepList.findIndex(s => !s.signers[0].id),1)
                // }
                // else{
                    Object.assign(p,{
                        colorId: this.coloList[i%this.coloList?.length]?.id,
                        color : this.coloList[i%this.coloList?.length]?.code
                    });
                // }
                i++;
            });
        });

        let _data = this.setDraftData();  // thừa


        this.spinnerService.show();
        this._requestWebService.saveDraftRequest(_data)
        .pipe(finalize(()=>{
            this.spinnerService.hide();
            this.documentData = Object.assign({},this.documentData ?? {id : draftId }, new CreateOrEditEsignRequestDto(), {id : draftId } );
        }))
        .subscribe(res => {
            draftId = res;
            this.refrence.getReferenceRequestByRequestId(draftId).pipe(finalize(()=>{

            }))
            .subscribe(res => {

                this.references = res.items
                console.log(this.references)
                this.documentData = Object.assign(this.documentData,{
                    categoryList: this.documentData?.categoryList,
                    listCategoryId: this.documentData?.listCategoryId,
                    stepList: this.stepList,
                    ref : this.references,
                    itemList: this.itemList,
                    projectScheduleFrom : this.documentData.projectScheduleFrom , // da duoc gan luc change data
                    projectScheduleTo : this.documentData.projectScheduleTo,//moment(this.documentData.projectScheduleTo).toISOString(),
                    addCC: this.selectedCC?.map(e => e.email)?.join(";") ?? '',
                    // isSaveTemplateSingerCC: this.isSaveTemplateSingerCC,
                    // ccText: this.ccText,
                });
                this.local.setItem('documentData',Object.assign({},this.documentData));
            })

            this.notify.success(this.l("SavedSuccessfully"));
        });
    }

    SaveTemplateSignerCC(){
        if(this.isSaveTemplateSingerCC){
            this._dataSaveSingerCC = new EsignSignerTemplateLinkCreateNewRequestForWebDto();
            this._dataSaveSingerCC.name = this.ccText;
            this._dataSaveSingerCC.listCC = [];
            this._dataSaveSingerCC.listSigners = [];
            if (this.documentData?.stepList?.length > 0){
                this.documentData?.stepList?.forEach((e,i)=>{
                    let aSigner = new EsignSignerTemplateLinkListSignerForWebDto();
                    let aSignerId = [];
                    let aColorId = [];
                    e.signers.forEach((itemSinger,indexSigner)=>{
                        aSignerId.push(itemSinger.id);
                        aColorId.push(itemSinger.colorId);
                    });
                    aSigner.signingOrder = i + 1;
                    aSigner.id = aSignerId;
                    aSigner.colorId = aColorId;
                    this._dataSaveSingerCC.listSigners.push(aSigner);
                });
            }

            if(this.selectedCC?.length > 0) {
                let _CCId = [];
                this.selectedCC?.forEach((e,i)=>{
                    _CCId.push(e.id);
                });
                this._dataSaveSingerCC.listCC = _CCId;
            }
            this._esignSignerTemplateLink.createNewTemplateForWebRequesterForWeb(this._dataSaveSingerCC).subscribe(e => {
            });
        }
    }

    showSearchSigner(){
        let listStepSigner = this.stepList.filter(e => e.signers.some(p => p.id)).map((e, index) => {
            return {
                signers: e.signers.map(p => {
                    return {
                        id: p.id,
                        fullName: p.fullName,
                        email: p.email,
                        title: p.title,
                        formFields: [],
                        imgUrl: p.imgUrl,
                        backgroundColor: p.backgroundColor,
                        colorId: p.colorId,
                        note: p.note,
                        signingOrder: index + 1,
                    };
                })
            }
        });
        let signer = listStepSigner.reduce((acc, val) => acc.concat(val.signers), []);
        this.searchSigner.show(signer, this.selectedCC);
    }

    dropCC(params){
        if (params?.item?.data['signers']){
            if (params?.item?.data['signers'].some(e => e.id == abp.session.userId))  return this.notify.warn(this.l("YouMustBeSigner"));
            this.stepList.splice(params?.previousIndex,1)
            params?.item?.data['signers'].forEach(e => {
                this.selectedCC.push({
                    email: e.email,
                    fullName: e.fullName,
                    id : e.id ,
                    imgUrl: e.imgUrl,
                });
            })
        }
        else {
            var data = params.previousContainer.data.find((e,i) => i == params?.previousIndex);
            if (data.id == abp.session.userId)  return this.notify.warn(this.l("YouMustBeSigner")) ;
            params.previousContainer.data.splice(params?.previousIndex,1);
            this.selectedCC.push({
                email: data.email,
                fullName: data.fullName,
                id : data.id ,
                imgUrl: data.imgUrl,
            });
        }
    }

    formatCurrency() {
        if(this.totalCostTemp){
            // check regex a-z A-Z and same â,ă,ê,ô,ơ,ư,đ or . in input string
            let regexAscii = /[a-zA-Z\u00C0-\u00FF]+|\.+/g;
            if (regexAscii.test(this.totalCostTemp)) {
                this.totalCostTemp = null;
                this.documentData.totalCost = null;
                return;
            }
            let number = this.totalCostTemp.toString().replace(/,/g, '');
            let regex = /^\d+$/;
            if (regex.test(number)) {
                this.totalCostTemp = number.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
                this.documentData.totalCost = this.totalCostTemp.replace(/,/g, '');
            }
        }
    }

    expectedDateChange(e) {
        //this.documentData.expectedDate = moment(e).add(1,'day').toDate() as any; // dung khi mo 1 request draft
        let _dateX = 0;
        let _d = moment(this.expectedDateInput).toDate() as any;
        // if(_d.getHours() != 17) _dateX = 1;
        this.documentData.expectedDate = moment(_d).add(_dateX, 'day').toDate() as any;
        // alert(this.documentData.expectedDate);
    }

    changeProjectDate(params){

        // dúng khi khởi tạo
        this.documentData.projectScheduleFrom =  (params && params[0]) ? moment(params[0]).toDate() : null;
        this.documentData.projectScheduleTo =  (params && params[1]) ? moment(params[1]).toDate() : null;

        if (params && params[0] && params[1]) this.showCalendar = false;
        else this.showCalendar = true;
    }


    clickPress(event,index){
        if (event.keyCode == 13) {
            // do something
            this.renameOutFocus(index)
        }
    }

    previewDoc(document) {
        this.reviewModal.show(document.documentPath, document.name, document.id)
        // this.local.setItem('selectedRequest', this.documentData);
        // this.local.setItem('selectedDocumentId', {docId: document.id });
        // this.router.navigate(['/app/main/sign-now']);
    }



}
