import { Component, Injector, OnInit, ViewChild } from '@angular/core';
import { AppComponentBase } from '@shared/common/app-component-base';
import { AnimationModel, ILoadedEventArgs, ProgressTheme } from '@syncfusion/ej2-angular-progressbar';
import { GroupService, SortService } from '@syncfusion/ej2-angular-grids';
import { RequestDetailComponent } from './request-detail/request-detail.component';
import { BeforeOpenCloseMenuEventArgs, MenuComponent, MenuItemModel, TabComponent } from '@syncfusion/ej2-angular-navigations';
import { DataViewComponent } from '../esign-modal/data-view/data-view.component';
import { CommonFunction } from '@app/main/commonfuncton.component';
import { EsignActivityHistoryServiceProxy, EsignCommentsServiceProxy, EsignDocumentListWebServiceProxy, EsignReferenceRequestServiceProxy, EsignRequestServiceProxy, EsignRequestWebServiceProxy, EsignSignerListServiceProxy, MstEsignActiveDirectoryServiceProxy, MstEsignDivisionServiceProxy, MstEsignSystemsServiceProxy } from '@shared/service-proxies/service-proxies';
import { PaginationParamCustom } from '@app/shared/common/models/base.model';
import { finalize, forkJoin } from 'rxjs';
import { DialogComponent } from '@syncfusion/ej2-angular-popups';
import { ActivatedRoute } from '@angular/router';
import { CreateOrEditEsignActivityHistoryInputDto } from '@shared/service-proxies/service-proxies';
import { EsignFollowUpServiceProxy } from '@shared/service-proxies/service-proxies';
import { closest } from '@syncfusion/ej2-base';
import { LocalStorageService } from '@shared/utils/local-storage.service';
import { NotificationService } from '@app/shared/layout/notifications/notification.service';
import { AutoComplete } from 'primeng/autocomplete';
import { DataViewGroupComponent } from '../esign-modal/data-view-group/data-view-group.component';
@Component({
    selector: 'app-document-management',
    templateUrl: './document-managerment.component.html',
    styleUrls: ['./document-managerment.component.less'],
    providers: [SortService, GroupService],
    // animations: [appModuleAnimation()],
})
export class DocumentManagermentComponent extends AppComponentBase implements OnInit  {
    @ViewChild('requestDetail', { static: true }) requestDetail: RequestDetailComponent;
    @ViewChild('dataView', { static: false }) dataView: DataViewComponent;
    @ViewChild('dataViewGroup', { static: false }) dataViewGroup: DataViewGroupComponent;
    @ViewChild('dataViewFollowUp', { static: false }) dataViewFollowUp: DataViewComponent;
    @ViewChild('tabFilter', { static: true }) tabFilter: TabComponent;
    @ViewChild('requester') requester: DialogComponent | any;
    // @ViewChild('review') review: ReviewComponent | any;
    @ViewChild('menuS') menuS: MenuComponent;
    @ViewChild('searchUser') searchUser: AutoComplete;
    public animation: AnimationModel = { enable: true, duration: 1000, delay: 0 };
    currentPage = 1;
    systemFilter = 0;
    systemFilterContent = this.l('AllSystems');
    typeFilter = 1; // value: 1,2,3 -> index: 0,1,2
    statusFilter = '';
    statusFilter_tmp = '';
    statusFilterIndex: number = -1;
    isSelectTab:boolean = true;
    filterRequester: any;
    filterRequesterName = this.l('Requester');
    filterRequesterIcon = 'e-icons e-filter';
    iconOrderCreationTime = 'pi pi-angle-down'
    iconOrderModifyTime = 'pi pi-angle-down'
    orderCreationTimeFilter = 'desc';
    orderModifyTimeFilter = 'desc';
    listUser: any;
    searchFilter;
    preSearchFilterType = 1;
    searchFilterType = 1;
    pagination: PaginationParamCustom = { pageNum: 1, pageSize: 100, totalCount: 0, totalPage: 1 };
    isFollowUp: number = 0;
    dialogHeader = this.l('Requester');
    animationSettings: Object = { effect: 'FadeZoom' };
    isReview: boolean = false;
    selectDocumentId: number;
    selectedRequest: {
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
    listDataFilter;
    itemSelectedFirst;
    _fn: CommonFunction = new CommonFunction();
    public items: any[] = [];
    isLoading = false;
    searchDetailResult: any[] = [];
    activeIndexDetail = 0;
    groupSettings = {showDropArea: false, columns: ['isSigned']};
    groupSearchSettings = {showDropArea: false, columns: ['division']};
    public fields: Object = { groupBy: 'division' };
    paramSigner;
    public menuItems: MenuItemModel[] = [
        {
            text: this.getSearchType(),
            items: [
                { text: this.l('All'), id: 'All' },
            ]
        },
    ];
    pendding = false;
    constructor(
        injector: Injector,
        private local: LocalStorageService,
        private _requestServiceWeb: EsignRequestWebServiceProxy,
        private _documentService: EsignDocumentListWebServiceProxy,
        private _commentService: EsignCommentsServiceProxy,
        private _historyService: EsignActivityHistoryServiceProxy,
        private _signerService: EsignSignerListServiceProxy,
        private _systemService: MstEsignSystemsServiceProxy,
        private activeDirectoryService: MstEsignActiveDirectoryServiceProxy,
        private _activatedRoute: ActivatedRoute,
        private _followUpService: EsignFollowUpServiceProxy,
        private _mstEsignDivision: MstEsignDivisionServiceProxy,
        private _refDocument: EsignReferenceRequestServiceProxy,
        private changeNotification: NotificationService,

    ) {
        super(injector);

        this._fn.isShowUserProfile();
    }
    // shouldDetach(route: ActivatedRouteSnapshot): boolean {
    //     throw new Error('Method not implemented.');
    // }
    // store(route: ActivatedRouteSnapshot, detachedTree: DetachedRouteHandle): void {
    //     throw new Error('Method not implemented.');
    // }
    // shouldAttach(route: ActivatedRouteSnapshot): boolean {
    //     throw new Error('Method not implemented.');
    // }
    // retrieve(route: ActivatedRouteSnapshot): DetachedRouteHandle {
    //     throw new Error('Method not implemented.');
    // }
    // shouldReuseRoute(future: ActivatedRouteSnapshot, curr: ActivatedRouteSnapshot): boolean {
    //     throw new Error('Method not implemented.');
    // }

    ngOnInit() {
        this.isReview = false;
        this.statusFilter = '';
        this.searchFilter = ''
        this.filterRequester = undefined;
        this.getAllDivision();
        this.getAllSystem();

        // link từ mail tới đã có màn hình view-detail riêng, từ dashboard vào nên vẫn có
        // this.local.getItem("selectRequest",(data)=>{
        //     console.log(data)
        // })
        // this.local.getItem("documentData",(data)=>{
        //     console.log(data)
        // })
        this.local.getItem("selectRequestFromDashboard",(err, data)=>{
            let requestId = data;
            if (requestId) {
                // có requestId : select bản ghi với requestId này
                // bản ghi này đang ở tab "Action Required" hay "waiting for other"
                this.selectedRequest.id = Number(requestId);
                this.local.removeItem("selectRequestFromDashboard");
                this.isFirstLoadRequestId_exists = true; // tại sao bằng true chỗ này ? mặc định = true
                //console.log('-------------------1. ', this.selectedRequest.id);
                this._requestServiceWeb.getRequestsByIdForSelectedItemWeb(this.selectedRequest.id)
                .subscribe(res => {
                  //  console.log('-------------------2. ', res);
                    this.itemSelectedFirst = res;
                    this.getLocalSearchFilter(res.typeFilter);
                });
            }
            else {
                this.itemSelectedFirst = null;
                this.getLocalSearchFilter()
                setTimeout(() => {
                // không truyền request id, --> không select need sign
                    this.isSelectTab = false;
                    let _tabFilterSelect = document.querySelector('#tabFilter .e-toolbar-item.e-icon.e-template.e-active');
                    if(_tabFilterSelect) _tabFilterSelect.classList.add('tab_unselect');
                }, 200);
            }

            setTimeout(() => {
                this.searchRequest();
            }, 50);

            this.changeNotification.changeNotification.subscribe(res => {
                this.searchRequest();
            });
        })


    }

    ngAfterViewInit(): void {
        //Called after ngAfterContentInit when the component's view has been initialized. Applies to components only.
        //Add 'implements AfterViewInit' to the class.
        // this.local.getItem("selectRequest",(data)=>{
        //     console.log(data)
        // })
        // this.local.getItem("documentData",(data)=>{
        //     console.log(data)
        // })
        // this.local.getItem("selectRequestFromDashboard",(data)=>{
        //     console.log(data)
        //     let requestId = data;
        //     if (requestId) {
        //         // có requestId : select bản ghi với requestId này
        //         // bản ghi này đang ở tab "Action Required" hay "waiting for other"
        //         this.selectedRequest.id = Number(requestId);

        //         this.local.removeItem("selectRequestFromDashboard");
        //         this.isFirstLoadRequestId_exists = true;

        //         console.log('------------------------ this.selectedRequest.id')
        //         console.log( this.selectedRequest.id)
        //         this._requestServiceWeb.getRequestsByIdForSelectedItemWeb(this.selectedRequest.id)
        //         .subscribe(res => {
        //             // console.log(this.selectedRequest.id, res);
        //             this.itemSelectedFirst = res;
        //             this.getLocalSearchFilter(res.typeFilter);
        //         });
        //     }
        //     else {
        //         this.itemSelectedFirst = null;
        //         this.getLocalSearchFilter()
        //         setTimeout(() => {
        //         // không truyền request id, --> không select need sign
        //             this.isSelectTab = false;
        //             let _tabFilterSelect = document.querySelector('#tabFilter .e-toolbar-item.e-icon.e-template.e-active');
        //             if(_tabFilterSelect) _tabFilterSelect.classList.add('tab_unselect');
        //         }, 200);
        //     }

        //     setTimeout(() => {
        //         this.searchRequest();
        //     }, 50);

        //     this.changeNotification.changeNotification.subscribe(res => {
        //         this.searchRequest();
        //     });
        // })


    }

    getAllSystem() {
        this._systemService.getAllSystems('', '', 100, 0)
        .subscribe(res => {
            this.items = [{ id: '0', text: this.l('AllSystems') }];
            res.items.map((item) => {
                this.items.push({ id: item.id.toString(), text: '<img src="' + item.imgUrl + '" alt="logo" style="height: 35px;"> ' + item.localName });
            });
        });
    }

    getAllDivision() {
        this._mstEsignDivision.getAllDivisionBySearchValue("")
        .subscribe(res => {
            this.menuS.insertAfter([{ text: this.l('File'), id: 'File', items: [] },{ text: this.l('Division'), id: 'Division', items: res.items.map(e => { return { text: e.divisionName, id: e.divisionId.toString() } }) }],  'All', true);
        });
    }

    submitSearchDetail(type, query){
        if(type === 2){
            this.searchFilter = query;
        }
    }

    searchReaquestDetail(useParam: boolean = false) {
        this.isLoading = true;
        let summary = this._requestServiceWeb.getRequestSummaryById(this.selectedRequest.id);
        let signers = this._signerService.getListSignerByRequestId(this.selectedRequest.id);
        let documents = this._documentService.getEsignDocumentByRequestId(this.selectedRequest.id);
        let history = this._historyService.getListActivityHistory(this.selectedRequest.id);
        let comments = this._commentService.getAllCommentsForRequestId(this.selectedRequest.id);
        let refDocument = this._refDocument.getReferenceRequestByRequestId(this.selectedRequest.id);
        if (this.selectedRequest.id > 0) {
            forkJoin([summary, signers, documents, history, comments, refDocument])
                .pipe(finalize(() => {
                    if(useParam){
                        this.listDataFilter = [{
                            requestDate: this.selectedRequest.summary?.requestDate,
                            fromRequester: this.selectedRequest.summary?.fromRequester,
                            isRead: true,
                            message: this.selectedRequest.summary?.message,
                            requestId: Number(this.selectedRequest.id),
                            requesterImgUrl: this.selectedRequest.summary?.requesterImgUrl,
                            statusCode: this.selectedRequest.summary?.statusCode,
                            title: this.selectedRequest.summary?.title,
                            totalSignerCount: this.selectedRequest.totalSignerCount,
                            listSignerBySystemIdDto: this.paramSigner.map(e => {
                                return {
                                    ...e,
                                    imgUrl: e.imageUrl,
                                }}),
                        }];
                    }
                    if(this.selectedRequest.summary?.statusCode !== 'Draft'){
                        this.createHistoryViewed();
                    }
                    this.requestDetail.changeRequest();
                    this.requestDetail.tabSigner.linear.refresh();
                    this.isLoading = false;
                }))
                .subscribe(([summary, signers, documents, history, comments, refDocument]) => {
                    this.selectedRequest.summary = summary;
                    this.selectedRequest.signers = this.groupSigners(signers.items);
                    this.paramSigner = signers.items;
                    this.selectedRequest.totalSignerCount = signers.items.length;
                    this.selectedRequest.documents = documents.items.sort((a, b) => a.documentOrder - b.documentOrder);
                    this.selectedRequest.history = history.items;
                    this.selectedRequest.comments = comments.items;
                    this.selectedRequest.refDocument = refDocument.items;
                    this.isLoading = false;
                });
        }
        else {
            this.isLoading = false;
        }
    }

    saveLocalSearchFilter() {

        this.local.setItem('localSearchFilter' + abp.session.userId,Object.assign({}, {
            requestId: this.selectedRequest.id,
            typeFilter: this.typeFilter,
            systemFilter: this.systemFilter,
            statusFilter: (this.statusFilter)? this.statusFilter:"",
            statusFilterIndex: this.statusFilterIndex,
            searchFilter: this.searchFilter,
            searchFilterType: this.searchFilterType,
            isFollowUp: this.isFollowUp,
            filterRequester: this.filterRequester,
            orderCreationTimeFilter: this.orderCreationTimeFilter,
            orderModifyTimeFilter: this.orderModifyTimeFilter,
            pageSize: this.pagination.pageSize,

        }));
    }

    getLocalSearchFilter(p_typeFilter?) {
        this.local.getItem("localSearchFilter" + abp.session.userId, (err, data) =>{
            if(data) {
                if(p_typeFilter) this.typeFilter = Number(p_typeFilter);
                else this.typeFilter = data.typeFilter ? data.typeFilter: 1;
                // this.systemFilter = data.systemFilter;
                this.statusFilter = (data.statusFilter)? data.statusFilter:"";
                this.statusFilterIndex = data.statusFilterIndex ? data.statusFilterIndex: -1;
                // this.searchFilter = data.searchFilter;
                // this.searchFilterType = data.searchFilterType;
                // this.isFollowUp = data.isFollowUp;
                // this.filterRequester = data.filterRequester;
                // this.orderCreationTimeFilter = data.orderCreationTimeFilter;
                // this.orderModifyTimeFilter = data.orderModifyTimeFilter;
                // this.pagination.pageSize = data.pageSize;
            }
            setTimeout(() => {
                let _submenu = this.getStatusIndex(this.statusFilter);
                if(_submenu >= 0) {
                    this.statusFilterIndex = _submenu;
                    // console.log('-------------------3.1 ', _submenu);
                }
                else {
                    // console.log('-------------------3.2 ', _submenu);
                    this.isSelectTab = false;
                    let _tabFilterSelect = document.querySelector('#tabFilter .e-toolbar-item.e-icon.e-template.e-active');
                    if(_tabFilterSelect) _tabFilterSelect.classList.add('tab_unselect');
                }
            }, 100);
        });
    }

    searchRequest() {
        this.isLoading = true;
        this._requestServiceWeb.getListRequestsBySystemIdWeb(
            this.isFollowUp == 1 ? 0 : this.typeFilter,
            this.systemFilter,
            this.statusFilter,
            this.searchFilter,
            this.searchFilterType,
            this.isFollowUp,
            this.filterRequester?.id ?? 0,
            this.orderCreationTimeFilter,
            this.orderModifyTimeFilter,
            this.pagination.pageSize,
            0,
        )
            .pipe(finalize(() => {
                    if (this.listDataFilter?.length) {
                        setTimeout(() => {
                            this.Dataview_SelectItem();
                        }, 500);
                    }
                    else {
                        this.selectedRequest = {
                            id: 0,
                            summary: {},
                            signers: [],
                            documents: [],
                            history: [],
                            comments: [],
                            refDocument: [],
                            totalSignerCount: 0,
                            isFollowUp: false,
                        };
                    }

                this.isLoading = false;
            }))
            .subscribe(res => {
                this.listDataFilter = res.items;

                this.pagination.totalCount = res.totalCount;
                this.selectedRequest.totalSignerCount = this.listDataFilter[0]?.totalSignerCount;
                // this.saveLocalSearchFilter();
            });
    }

    getNewData(isfirstSelectRow?) {
        if(this.pendding || this.listDataFilter.length >= this.pagination.totalCount) return;
        this.pendding = true;
        this._requestServiceWeb.getListRequestsBySystemIdWeb(
            this.isFollowUp == 1 ? 0 : this.typeFilter,
            this.systemFilter,
            this.statusFilter,
            this.searchFilter,
            this.searchFilterType,
            this.isFollowUp,
            this.filterRequester?.id ?? 0,
            this.orderCreationTimeFilter,
            this.orderModifyTimeFilter,
            50,
            this.listDataFilter?.length,
        )
        .subscribe(res => {
            if (res.items.length) {
                this.listDataFilter = this.listDataFilter.concat(res.items);
                this.pagination.totalCount = res.totalCount;

            }
            this.pendding = false;
            if(this.isScroll) {
                setTimeout(() => {
                    this.Dataview_SelectItem();
                }, 500);
            }

        });
    }

    isFirstLoadRequestId_exists: boolean = false; // lần đầu tiên vào trang và có requestId cần selected:-> true
    onSelect(event: any) {

        let _time_cc = 0;
        if(this.isFirstLoadRequestId_exists) _time_cc = 200; // 2024-04-10 fix từ dashboard vào select luôn 1 item.

        //lần đầu tiên vào trang và có requestId cần select thì không thay đổi giá trị  selectedRequest.id
        setTimeout(() => {

            if(!this.isFirstLoadRequestId_exists) {

                this.selectedRequest.id = event.data?.requestId;
                // không tự động selected khi isFirstLoadRequestId_exists = true
                this.requestDetail.isLoading = true;
                this.selectedRequest.totalSignerCount = event.data?.totalSignerCount;
                this.selectedRequest.isFollowUp = event.data?.isFollowUp;
                this.listDataFilter.map(item => {
                    if (item.requestId == this.selectedRequest.id) {
                        item.isRead = true;
                    }
                });

                setTimeout(() => {
                    this.searchReaquestDetail();
                }, 50);

                this.requestDetail.isLoading = false;
            }

            // do change tab 2 lần nên không gán lại = false ở đây được
            // else this.isFirstLoadRequestId_exists = false;

        }, _time_cc);

    }

    createHistoryViewed() {
        try{

            if(!this.selectedRequest.id) return;
            this.getClientIpAddress(this.http)
            .then((ipAddress) => {
                if(ipAddress) {
                    let body = new CreateOrEditEsignActivityHistoryInputDto({
                        id: undefined,
                        requestId: (this.selectedRequest.id == null) ? 0:this.selectedRequest.id,
                        activityCode: 'Viewed',
                        ipAddress: ipAddress,
                    });
                    this._historyService.createSignerActivity(body).subscribe();
                }
            });
        }
        catch(ex) {
            console.error(ex);
        }
    }

    public load(args: ILoadedEventArgs): void {
        let div: HTMLCollection = document.getElementsByClassName('progressbar-label');
        let selectedTheme: string = location.hash.split('/')[1];
        selectedTheme = selectedTheme ? selectedTheme : 'Material';
        args.progressBar.theme = <ProgressTheme>(selectedTheme.charAt(0).toUpperCase() +
            selectedTheme.slice(1)).replace(/-dark/i, 'Dark').replace(/contrast/i, 'Contrast');
        if (args.progressBar.theme === 'HighContrast' || args.progressBar.theme === 'Bootstrap5Dark' || args.progressBar.theme === 'BootstrapDark' || args.progressBar.theme === 'FabricDark'
            || args.progressBar.theme === 'TailwindDark' || args.progressBar.theme === 'MaterialDark' || args.progressBar.theme === 'FluentDark' || args.progressBar.theme === 'Material3Dark') {
            for (let i = 0; i < div.length; i++) {
                div[i].setAttribute('style', 'color:white');
            }
        } else if (selectedTheme.indexOf('bootstrap') > -1) {
            for (let i = 0; i < div.length; i++) {
                div[i].setAttribute('style', 'top: 0px');
            }
        }
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

    saveMessage(event) {
        this.selectedRequest.signers.map((signer) => {
            signer.signers.map((item) => {
                if (item.id === event.id) {
                    item.message = event.message;
                }
            });
        });
    }

    isTabMain: boolean;
    onSelectTab(event) {



        this.isLoading = true;
        this.isTabMain = true; //-> event tab main -> không chạy event tab filterMenu
        this.typeFilter = event.selectedIndex + 1;

        //clear search value
        this.isSelectTab = false;
        this.statusFilter = '';
        this.statusFilterIndex = -1;

        //lần đầu tiên vào trang và có requestId cần select thì không thay đổi giá trị  selectedRequest.id
        if(!this.isFirstLoadRequestId_exists){
            // thay đổi giá trị khi isFirstLoadRequestId_exists được set lại bằng false
            this.selectedRequest = {
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
        }


        if(this.typeFilter == 3) { this.isFollowUp = 1; }
        else this.isFollowUp = 0;

        setTimeout(() => {
            if(this.statusFilterIndex<0) {
                let _tabFilterSelect = document.querySelector('#tabFilter .e-toolbar-item.e-icon.e-template.e-active');
                if(_tabFilterSelect) _tabFilterSelect.classList.add('tab_unselect');
            }
        }, 10);

        this.searchRequest();
    }

    onSelectTabStatus(event) {
        if(this.isTabMain != true) {
            if(event?.selectedItem){
                this.statusFilter = this.getStatusCode(event?.selectedItem?.innerText.toLowerCase());
                this.statusFilter_tmp = this.getStatusCode(event?.selectedItem?.innerText.toLowerCase());
            }
            else if(event?.target){
                this.statusFilter = this.getStatusCode(event?.target?.innerText.toLowerCase());
                this.statusFilter_tmp = this.getStatusCode(event?.target?.innerText.toLowerCase());
            }


        }

        this.isTabMain = false;
        this.isSelectTab = false;
    }

    getStatusIndex(statusCode) {
        if(this.typeFilter == 1) {
            switch (statusCode) {
                case 'NotSigned':
                    return 0;
                case 'OnProgress':
                    return 1;
                case 'Rejected':
                    return 2;
                case 'Completed':
                    return 3;
                case 'Revoked':
                    return 4;
                default:
                    return -1;
            }
        }
        else if(this.typeFilter == 2){
            switch (statusCode) {
                case 'Draft':
                    return 0;
                case 'OnProgress':
                    return 1;
                case 'Rejected':
                    return 2;
                case 'Completed':
                    return 3;
                case 'Shared':
                    return 4;
                case 'Transfer':
                    return 5;
                case 'Revoked':
                    return 6;
                default:
                    return -1;
            }
        }
        else {
            switch (statusCode) {
                case 'Draft':
                    return 0;
                case 'NotSigned':
                    return 1;
                case 'OnProgress':
                    return 2;
                case 'Rejected':
                    return 3;
                case 'Completed':
                    return 4;
                case 'Shared':
                    return 5;
                case 'Transfer':
                    return 6;
                case 'Revoked':
                    return 7;
                default:
                    return -1;
            }
        }
    }

    getStatusCode(statusCode) {
        switch (statusCode) {
            case this.l('Draft').toLowerCase():
                return 'Draft';
            case this.l('OnProgress').toLowerCase():
                return 'OnProgress';
            case this.l('NeedsToSign').toLowerCase():
                return 'NotSigned';
            case this.l('Rejected').toLowerCase():
                return 'Rejected';
            case this.l('Completed').toLowerCase():
                return 'Completed';
            case this.l('Shared').toLowerCase():
                return 'Shared';
            case this.l('Transfer').toLowerCase():
                return 'Transfer';
            case this.l('Revoked').toLowerCase():
                return 'Revoked';
            default:
                return '';
        }
    }

    TabFilterClick(event) {


        if(event.srcElement.className == "e-nav-right-arrow e-nav-arrow e-icons" ||
            event.srcElement.className == "e-nav-left-arrow e-nav-arrow e-icons" ||
            event.srcElement.className == "e-toolbar-items e-lib e-hscroll e-control e-touch") {
            return;
        }
        this.selectedRequest = {
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

        if(this.isSelectTab) {
            this.isSelectTab = false;
            this.statusFilter = '';

            let _tabFilterSelect = document.querySelector('#tabFilter .e-toolbar-item.e-icon.e-template.e-active');
            if(_tabFilterSelect) _tabFilterSelect.classList.add('tab_unselect');

            this.searchRequest();
        }else {
            this.isSelectTab = true;
            this.statusFilterIndex = this.getStatusIndex(this.getStatusCode(event?.target?.innerText.toLowerCase()));
            let _tabFilterSelect = document.querySelector('#tabFilter .e-toolbar-item.e-icon.e-template.e-active');
            if(_tabFilterSelect) _tabFilterSelect.classList.remove('tab_unselect');
            this.statusFilter = this.getStatusCode(event?.target?.innerText.toLowerCase());

            this.searchRequest();
        }
    }

    onScroll() {
        if(this.typeFilter == 2){
            let pos = this.round((document.getElementById('dataView').scrollTop || document.body.scrollTop) + document.getElementById('dataView').offsetHeight);
            let match = this.round(document.getElementById('dataView').scrollHeight * 0.68);
            if (pos == match && this.listDataFilter.length < this.pagination.totalCount && !this.pendding) {
                this.getNewData();
            }
        }
        else if(this.typeFilter == 1){
            let pos = this.round((document.getElementById('dataViewGroup').scrollTop || document.body.scrollTop) + document.getElementById('dataViewGroup').offsetHeight);
            let match = this.round(document.getElementById('dataViewGroup').scrollHeight * 0.68);
            if (pos == match && this.listDataFilter.length < this.pagination.totalCount && !this.pendding) {
                this.getNewData();
            }
        }
        else if(this.typeFilter == 3){
            let pos = this.round((document.getElementById('dataViewFollowUp').scrollTop || document.body.scrollTop) + document.getElementById('dataViewFollowUp').offsetHeight);
            let match = this.round(document.getElementById('dataViewFollowUp').scrollHeight * 0.68);
            if (pos == match && this.listDataFilter.length < this.pagination.totalCount && !this.pendding) {
                this.getNewData();
            }
        }
    }

    toScroll() {
        this.getNewData(true);
    }
    isScroll:boolean = false;
    scrollHeight:number = 0;
    // scrollIndex:number = 0;
    Dataview_SelectItem(){

        if(this.selectedRequest.id >0 ) {   //dataViewGroup
            if(this.typeFilter == 1) {
                let _doc = document.querySelector<HTMLElement>("#dataViewGroup");
                let _queryHtml = "li.e-list-item data-view-item[id=requestId_"+this.selectedRequest.id.toString()+"]";
                let item = document.querySelector<HTMLElement>(_queryHtml);
                //let item = _doc.querySelector<HTMLElement>(".e-gridcontent table.e-table:nth-child(1) > tbody tr.e-row td.e-rowcell data-view-item#requestId_"+this.selectedRequest.id);
                if(item) {
                    this.isScroll = false;
                    let p_index = this.listDataFilter.findIndex(e => e.requestId == this.selectedRequest.id);
                    this.dataViewGroup.selectIndex(p_index);
                    let _scroll = _doc.querySelector('#listviewGroup');
                    let _top = (p_index * 100) //+ _spec;
                    _scroll.scroll({ top: _top, behavior: "smooth"});
                    this.isFirstLoadRequestId_exists = false; // tìm được hay không cũng set false

                }
                else {
                    // setTimeout(() => {
                        //scroll to
                        if(this.isFirstLoadRequestId_exists) { // chỉ csroll 1 lần
                            this.isFirstLoadRequestId_exists = false; // tìm được hay không cũng set false

                            this.isScroll = true;
                            let p_item = _doc.querySelector<HTMLElement>("li.e-list-item data-view-item[id=requestId_"+this.selectedRequest.id.toString()+"]");
                            let _scroll = _doc.querySelector('#listviewGroup');
                            this.scrollHeight = this.scrollHeight + Math.round(p_item.getBoundingClientRect().top);
                            _scroll.scroll({ top: this.scrollHeight, behavior: "smooth"});
                        }else {
                            // trường hợp không tìm thấy thì cứ lấy bản ghi detail
                            this.searchReaquestDetail();
                        }
                    // }, 200);
                    // this.toScroll();
                }
            }
            else if (this.typeFilter == 2) {    //dataView
                let _doc = document.querySelector<HTMLElement>("#dataView");
                let _queryHtml = "li.e-list-item data-view-item[id=requestId_"+this.selectedRequest.id.toString()+"]";
                let item = document.querySelector<HTMLElement>(_queryHtml);
                this.isFirstLoadRequestId_exists = false; // tìm được hay không cũng set false

                if(item) {
                    // return;
                    this.isScroll = false;
                    let p_index = this.listDataFilter.findIndex(e => e.requestId == this.selectedRequest.id);
                    this.dataView.selectIndex(p_index);
                    let _scroll = _doc.querySelector('#listRequestFilter');
                    let _top = Number(p_index) * 100;
                    _scroll.scroll({ top: _top, behavior: "smooth"});
                }
                else {
                    if(this.isFirstLoadRequestId_exists) { // chỉ csroll 1 lần
                        this.isFirstLoadRequestId_exists = false; // tìm được hay không cũng set false
                        this.isScroll = true;
                        let p_item = _doc.querySelector<HTMLElement>("li.e-list-item data-view-item[id=requestId_"+this.selectedRequest.id.toString()+"]");
                        let _scroll = _doc.querySelector('#listRequestFilter');
                        this.scrollHeight = this.scrollHeight + Math.round(p_item.getBoundingClientRect().top);
                        _scroll.scroll({ top: this.scrollHeight, behavior: "smooth"});
                    }else {
                        this.searchReaquestDetail();
                    }
                }
            }
            else if (this.typeFilter == 3) {    //dataViewFollowUp

                let _doc = document.querySelector<HTMLElement>("#dataViewFollowUp");
                let _queryHtml = "li.e-list-item data-view-item[id=requestId_"+this.selectedRequest.id.toString()+"]";

                let item = document.querySelector<HTMLElement>(_queryHtml);
                this.isFirstLoadRequestId_exists = false; // tìm được hay không cũng set false
                if(item) {
                    this.isScroll = false;
                    let p_index = this.listDataFilter.findIndex(e => e.requestId == this.selectedRequest.id);
                    this.dataViewFollowUp.selectIndex(p_index);
                    let _scroll = _doc.querySelector('#listRequestFilter');
                    let _top = Number(p_index) * 100;
                    _scroll.scroll({ top: _top, behavior: "smooth"});
                }
                else {
                    if(this.isFirstLoadRequestId_exists) { // chỉ csroll 1 lần
                        this.isFirstLoadRequestId_exists = false; // tìm được hay không cũng set false
                        this.isScroll = true;
                        let p_item = _doc.querySelector<HTMLElement>("li.e-list-item data-view-item[id=requestId_"+this.selectedRequest.id.toString()+"]");
                        let _scroll = _doc.querySelector('#listRequestFilter');
                        this.scrollHeight = this.scrollHeight + Math.round(p_item.getBoundingClientRect().top);
                        _scroll.scroll({ top: this.scrollHeight, behavior: "smooth"});
                    }else {
                        this.searchReaquestDetail();
                    }
                }
            }
        }
        else {
            if(this.typeFilter == 1) this.dataViewGroup.selectFirstRow();
            else if (this.typeFilter == 2) this.dataView.selectFirstRow();
            else if (this.typeFilter == 3) this.dataViewFollowUp.selectFirstRow();
        }
    }

    round(num) {
        let num1 = Math.round(num)?.toString().slice(0, -3);
        return Number(num1);
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
    onComments() {
        this._commentService.getAllCommentsForRequestId(this.selectedRequest.id)
            .subscribe(res => {
                this.selectedRequest.comments = res.items;
            });
    }

    onRevoke() {
        this.searchRequest();
    }

    onReject() {
        this.searchRequest();
    }

    changeKey(event) {
        // if (event.keyCode === 13) {
            this.searchRequest();
        // }
    }

    onChangeSystems(event) {
        this.systemFilter = Number(event.item.properties.id);
        this.systemFilterContent = event.item.properties.text;
        this.searchRequest();
    }

    onOpenDialog() {
        this.filterUser('');
        this.requester.show();
    };

    closeDialog() {
        this.requester.hide();
    }
    submitSearchUser() {
        if (this.filterRequester) {
            this.filterRequesterName = this.filterRequester.fullName;
            this.filterRequesterIcon = 'e-icons e-filter-active';
        }
        else {
            this.filterRequesterName = this.l('Requester');
            this.filterRequesterIcon = 'e-icons e-filter';
        }
        this.requester.hide();
        this.searchRequest();
    }

    orderCreationTime() {
        if (this.orderCreationTimeFilter === 'asc') {
            this.orderCreationTimeFilter = 'desc';
            this.iconOrderCreationTime = 'pi pi-angle-down';
        }
        else if (this.orderCreationTimeFilter === 'desc') {
            this.orderCreationTimeFilter = '';
            this.iconOrderCreationTime = 'pi pi-bars';
        }
        else {
            this.orderCreationTimeFilter = 'asc';
            this.iconOrderCreationTime = 'pi pi-angle-up';
        }
        this.searchRequest();
    }

    orderModifiTime() {
        if (this.orderModifyTimeFilter === 'asc') {
            this.orderModifyTimeFilter = 'desc';
            this.iconOrderModifyTime = 'pi pi-angle-down';
        }
        else if (this.orderModifyTimeFilter === 'desc') {
            this.orderModifyTimeFilter = '';
            this.iconOrderModifyTime = 'pi pi-bars';
        }
        else {
            this.orderModifyTimeFilter = 'asc';
            this.iconOrderModifyTime = 'pi pi-angle-up';
        }
        this.searchRequest();
    }

    filterUser(event) {
        this.activeDirectoryService.getAllSignersForWeb(event.query, undefined, 0, 50).subscribe(res => {
            this.listUser = res.items;
        });
    }

    // gotoReview(event) {
    //     this.review.ngOnInit();
    //     this.selectDocumentId = event;
    //     let path = this.selectedRequest.documents.find(e => e.id == this.selectDocumentId)?.documentPath;
    //     let name = this.selectedRequest.documents.find(e => e.id == this.selectDocumentId)?.documentName;
    //     this.review.show(path, name, this.selectDocumentId);
    // }

    onReassign() {
        this.searchRequest();
    }

    onFollowUp(event) {
        if (this.typeFilter == 3) {
            this.searchRequest();
            setTimeout(() => {
                this.dataViewFollowUp.selectFirstRow();
            }, 100);
        }
    }

    onSignSuccess() {
        this.selectedRequest.id = null;
        this.searchRequest();
    }
    onDeleteRequest(){
        this.selectedRequest.id = 0;
        this.searchRequest();
    }

    onShareSuccess() {
        this.searchRequest();
    }

    onChangeRefDocument() {
        this.searchReaquestDetail();
    }

    onSelectSearchDetailType(event){
        if(event?.item?.parentObj?.id == 'Division'){
            this.searchFilterType = 2;
            this.searchFilter = event?.item?.text;
        }
        else if(event?.item?.text == this.l('All')){
            this.searchFilterType = 1;
            this.searchFilter = '';
        }
        else if(event?.item?.text == this.l('File')) {
            this.searchFilterType = 3;
            this.searchFilter = '';
        }
        else if(event?.item?.text == this.l('Division')) {
            this.searchFilterType = 2;
            this.searchFilter = '';
        }
        this.menuS.items[0].text = this.getSearchType();
        if((this.searchFilterType == 1 || this.searchFilterType == 3) && this.preSearchFilterType != this.searchFilterType && this.searchFilter != '' && this.searchFilter != undefined && this.searchFilter != null){
            this.searchRequest();
        }
        else if(this.searchFilterType == 2 && this.searchFilter != '' && this.searchFilter != undefined && this.searchFilter != null){
            this.searchRequest();
        }
        this.preSearchFilterType = this.searchFilterType;
    }

    getSearchType(){
        return this.searchFilterType === 1 ? this.l('All') : this.searchFilterType === 2 ? this.l('Division') : this.l('File');
    }

    public onBeforeOpen(args: BeforeOpenCloseMenuEventArgs): void {
        if (args.parentItem.id === 'Division') {
            (closest(args.element, '.e-menu-wrapper') as HTMLElement).style.height = '260px';
            (closest(args.element, '.e-menu-wrapper') as HTMLElement).style.overflowY = 'auto';
        }
    }

    refreshData(){
        this.searchFilter = '';
        this.filterRequester = undefined;
        this.searchUser.clear();
        this.searchReaquestDetail()
    }

    clearFilerRequester(){
        this.filterRequester = undefined;
        this.searchUser.clear();
    }
}
