import { filter, map } from 'rxjs/operators';
import { DataViewTransferComponent } from './../esign-modal/data-view-transfer/data-view-transfer.component';
import { Component, Injector, OnInit, ViewChild } from '@angular/core';
import { CommonFunction } from '@app/main/commonfuncton.component';
import { CustomColumn, PaginationParamCustom } from '@app/shared/common/models/base.model';
import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { EsignRequestWebServiceProxy, EsignSignerListServiceProxy, MstEsignActiveDirectoryServiceProxy, MstEsignSystemsServiceProxy, TransferInputDto, TransferType } from '@shared/service-proxies/service-proxies';
import { ILoadedEventArgs, ProgressTheme } from '@syncfusion/ej2-progressbar';
import { finalize, forkJoin } from 'rxjs';
import { CheckBoxComponent } from '@syncfusion/ej2-angular-buttons';
import { ItemModel } from '@syncfusion/ej2-angular-splitbuttons';
import { DialogComponent } from '@syncfusion/ej2-angular-popups';
import { DateTime } from 'luxon';
import { AutoComplete } from 'primeng/autocomplete';

@Component({
    selector: 'app-transfer',
    templateUrl: './transfer.component.html',
    styleUrls: ['./transfer.component.less'],
})
export class TransferComponent extends AppComponentBase implements OnInit {
    @ViewChild('listviewInstanceL') listViewInstanceL?: DataViewTransferComponent;
    @ViewChild('listviewInstanceR') listViewInstanceR?: DataViewTransferComponent;
    @ViewChild('checkboxL') checkboxL: CheckBoxComponent;
    @ViewChild('checkboxR') checkboxR: CheckBoxComponent;
    @ViewChild('requester') requester: DialogComponent | any;
    @ViewChild('searchUser') searchUser: AutoComplete;
    animationSettings: Object = { effect: 'FadeZoom' };
    selectedLeftCount: number = 0;
    selectedRightCount: number = 0;
    systemFilter = 1;
    typeFilter: number = 1;
    statusFilter = 'OnProgress';
    statusFilter_tmp = 'OnProgress';
    isSelectTab: boolean = true;
    searchFilter;
    isFollowUp: number = 0;
    pagination: PaginationParamCustom = { pageNum: 1, pageSize: 50, totalCount: 0, totalPage: 1 };
    systemFilterContent = this.l('AllSystems');
    filterRequester: any;
    filterRequesterName = this.l('Requester');
    filterRequesterIcon = 'e-icons e-filter';
    iconOrderRequestDate = 'pi pi-angle-down'
    iconOrderExpectedDate = 'pi pi-angle-down'
    orderRequestDateFilter = 'desc';
    orderExpectedDateFilter = 'desc';
    listUser: any;
    public itemSystems: ItemModel[] = [];
    pending = false;
    statusOptions: any[] = [
        {
            id: 1,
            name: 'OnProgress',
        },
        {
            id: 2,
            name: 'Rejected',
        },
        {
            id: 3,
            name: 'Completed',
        },
    ];
    transferFromOptions: any[] = [
        {
            value: 0,
            label: this.l('All'),
        },
        {
            value: 1,
            label: this.l('Me'),
        },
        {
            value: 2,
            label: this.l('Other'),
        },
    ];
    public listLeft: any = [];
    transferTo: any;
    listRight = [];
    searchDataUser = [];
    searchResult;
    _fn: CommonFunction = new CommonFunction();
    rowDataHistory;
    columnDeftHistory: CustomColumn[] = [
        {
            header: this.l('DocumentTitle'),
            field: 'requestTitle',
            width: 150,
            textAlign: 'Left',
        },
        {
            header: this.l('TransferDate'),
            field: 'creationTime',
            width: 80,
            type: 'date',
            format: { type: 'date', format: 'd/MMM/yyyy' },
            textAlign: 'Center',
        },
        {
            header: this.l('Status'),
            field: 'transferStatus',
            width: 60,
            textAlign: 'Left',
        },
        {
            header: this.l('TransferFrom'),
            field: 'fromUser',
            width: 100,
            textAlign: 'Left',
        },
        {
            header: this.l('TransferTo'),
            field: 'toUser',
            width: 100,
            textAlign: 'Left',
        }
    ]
    selectionRow;//: MstEsignEmailTemplateOutputDto = new MstEsignEmailTemplateOutputDto();
    selectionSettings: { type: 'Single' };
    filterSettings: { type: 'FilterBar', mode: 'Immediate', immediateModeDelay: 150 };
    paginationHistory: PaginationParamCustom = { totalCount: 0, totalPage: 0, pageNum: 1, pageSize: 50 };
    requestFilter: string = '';
    transferFrom: TransferType.All | TransferType.Me | TransferType.Others = TransferType.All;
    transferFromDate: DateTime | undefined = undefined;
    transferToDate: DateTime | undefined = undefined;
    removeUserId: any[] = [];
    removeUserIdTemp: any[] = [];
    loading: boolean = false;
    constructor(
        injector: Injector,
        private _requestServiceWeb: EsignRequestWebServiceProxy,
        private activeDirectoryService: MstEsignActiveDirectoryServiceProxy,
        private dateFormat: DateTimeService,
        private _systemService: MstEsignSystemsServiceProxy,
        private _esignSignerList: EsignSignerListServiceProxy,
    ) {
        super(injector);

        this._fn.isShowUserProfile();
    }

    ngOnInit() {
        this.isSelectTab = false;
        this.statusFilter = '';
        this.searchRequest();
        this.getAllSystem();
        this.searchHistory();
        setTimeout(() => {
            let _tabFilterSelect = document.querySelector('#tabFilter .e-toolbar-item.e-icon.e-template.e-active');
            if (_tabFilterSelect) _tabFilterSelect.classList.add('tab_unselect');
        }, 100);
    }

    getAllSystem() {
        this._systemService.getAllSystems('', '', 100, 0)
            .subscribe(res => {
                this.itemSystems = [{ id: '0', text: this.l('AllSystems') }];
                res.items.map((item) => {
                    this.itemSystems.push({ id: item.id.toString(), text: '<img src="' + item.imgUrl + '" alt="logo" style="height: 35px;"> ' + item.localName });
                });
            });
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

    onSelectTabStatus(event) {
        this.statusFilter = this.statusOptions[event.selectedIndex].name;
        this.isSelectTab = false;
        this.statusFilter_tmp = this.statusOptions[event.selectedIndex].name;
        // this.searchRequest();
    }
    TabFilterClick(event) {
        if (event.srcElement.className == "e-nav-right-arrow e-nav-arrow e-icons" ||
            event.srcElement.className == "e-nav-left-arrow e-nav-arrow e-icons" ||
            event.srcElement.className == "e-toolbar-items e-lib e-hscroll e-control e-touch") {
            return;
        }

        if (this.isSelectTab) {
            this.isSelectTab = false;
            this.statusFilter = '';
            let _tabFilterSelect = document.querySelector('#tabFilter .e-toolbar-item.e-icon.e-template.e-active');
            if (_tabFilterSelect) _tabFilterSelect.classList.add('tab_unselect');
            this.searchRequest();
        } else {
            this.isSelectTab = true;
            let _tabFilterSelect = document.querySelector('#tabFilter .e-toolbar-item.e-icon.e-template.e-active');
            if (_tabFilterSelect) _tabFilterSelect.classList.remove('tab_unselect');
            this.statusFilter = this.statusFilter_tmp;
            this.searchRequest();
        }
    }


    onSelectTab(event) {
        this.typeFilter = event.selectedIndex + 1;
        if (this.typeFilter == 3) {
            this.isFollowUp = 1;
        }
        else {
            this.isFollowUp = 0;
        }
        setTimeout(() => {
            this.isSelectTab = false;
            this.statusFilter = '';
            let _tabFilterSelect = document.querySelector('#tabFilter .e-toolbar-item.e-icon.e-template.e-active');
            if (_tabFilterSelect) _tabFilterSelect.classList.add('tab_unselect');
        }, 10);
        this.searchRequest();
    }

    checkAllHandlerL(event) {
        if (this.listLeft)
            if (event.checked) {
                this.listViewInstanceL.listviewInstance.selectRows(this.listLeft.map((item, index) => index));
                this.listLeft.map(item => item.checked = true);
                this.selectedLeftCount = this.listLeft.length;
            }
            else {
                this.listViewInstanceL.listviewInstance.clearRowSelection();
                this.listLeft.map(item => item.checked = false);
                this.selectedLeftCount = 0;
            }
    }

    checkAllHandlerR(event) {
        if (this.listRight)
            if (event.checked) {
                this.listViewInstanceR.listviewInstance.selectRows(this.listRight.map((item, index) => index));
                this.listRight.map(item => item.checked = true);
                this.selectedRightCount = this.listRight.length;
            }
            else {
                this.listViewInstanceR.listviewInstance.clearRowSelection();
                this.listRight.map(item => item.checked = false);
                this.selectedRightCount = 0;
            }
    }

    onSelectL(event) {
        this.listLeft.forEach(item => {
            if (item.requestId == event.requestId) {
                item.checked = event.checked;
                this.selectedLeftCount = this.listLeft.filter(item => item.checked).length;
                return;
            }
        });
    }

    onSelectR(event) {
        this.listRight.map(item => {
            if (item.requestId == event.requestId) {
                item.checked = event.checked;
                this.selectedRightCount = this.listRight.filter(item => item.checked).length;
                return;
            }
        });
    }

    add() {
        this.changeList();
    }

    remove() {
        if (this.selectedRightCount > 0) {
            this.listLeft = this.listLeft.concat(this.listRight.filter(item => item.checked));
            this.listRight = this.listRight.filter(item => !item.checked);
            this.selectedLeftCount = this.listLeft.filter(item => item.checked).length;
            this.selectedRightCount = this.listRight.filter(item => item.checked).length;
            setTimeout(() => {
                this.checkAllHandlerL({ checked: false });
            }, 50);
            this.checkboxL.checked = false;
            this.checkboxR.checked = false;
        }
    }

    searchRequest() {
        this._requestServiceWeb.getListRequestsCanTransferWeb(
            this.isFollowUp == 1 ? 0 : this.typeFilter,
            this.systemFilter,
            this.statusFilter,
            this.searchFilter,
            1,
            this.isFollowUp,
            0,
            'desc',
            'desc',
            this.pagination.pageSize,
            0,
        )
            .pipe(finalize(() => {
                this.listLeft.map(item => item.checked = false);
            }))
            .subscribe(res => {
                this.listLeft = res.items
                    .filter(item => !this.listRight.some(item2 => item2.requestId == item.requestId))
                    .filter(x => x.statusCode !== 'Draft');
                this.pagination.totalCount = res.totalCount;
            });
    }
    formatDate(input) {
        if (input) {
            //check in day
            let date = new Date(input);
            let now = new Date();
            if (date.getDate() === now.getDate() && date.getMonth() === now.getMonth() && date.getFullYear() === now.getFullYear()) {
                //return 10 minutes ago
                let minutes = Math.floor((now.getTime() - date.getTime()) / 60000);
                if (minutes === 0) {
                    return 'Just now';
                }
                else if (minutes < 60) {
                    return minutes + 'm ago';
                } else {
                    let hours = Math.floor(minutes / 60);
                    return hours + 'h ago';
                }
            }
            //in year return type Oct 21
            else if (date.getFullYear() === now.getFullYear()) {
                return this.dateFormat.formatDate(date, 'MMM d');
            }
            else {
                //return type Oct 21, 2020
                return this.dateFormat.formatDate(input as Date, 'MMM d, yyyy');
            }
        }
        else return '';
    }
    onChangeSystems(event) {
        this.systemFilter = Number(event.item.properties.id);
        this.systemFilterContent = event.item.properties.text;
        this.searchRequest();
    }
    public onOpenDialog = function (event: any): void {
        this.requester.show();
    };

    filterUser(event) {
        this.activeDirectoryService.getAllSignersForWeb(event.query, undefined, 0, 10000).subscribe(res => {
            this.listUser = res.items;
        });
    }
    filterTransferUser(event) {
        this.activeDirectoryService.getAllSignersForTransfer(event.query, undefined, 0, 100).subscribe(res => {
            this.searchResult = res.items.filter(item => !this.removeUserId.includes(item.id));
        });
    }

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
    orderRequestDate() {
        if (this.orderRequestDateFilter === 'asc') {
            this.orderRequestDateFilter = 'desc';
            this.iconOrderRequestDate = 'pi pi-angle-down';
        }
        else if (this.orderRequestDateFilter === 'desc') {
            this.orderRequestDateFilter = '';
            this.iconOrderRequestDate = 'pi pi-bars';
        }
        else {
            this.orderRequestDateFilter = 'asc';
            this.iconOrderRequestDate = 'pi pi-angle-up';
        }
        this.searchRequest();
    }

    orderExpectedDate() {
        if (this.orderExpectedDateFilter === 'asc') {
            this.orderExpectedDateFilter = 'desc';
            this.iconOrderExpectedDate = 'pi pi-angle-down';
        }
        else if (this.orderExpectedDateFilter === 'desc') {
            this.orderExpectedDateFilter = '';
            this.iconOrderExpectedDate = 'pi pi-bars';
        }
        else {
            this.orderExpectedDateFilter = 'asc';
            this.iconOrderExpectedDate = 'pi pi-angle-up';
        }
        this.searchRequest();
    }

    changeKey(event) {
        if (event.keyCode === 13) {
            this.searchRequest();
        }
    }

    onScroll() {
        // let pos = (document.getElementById('listviewInstanceL').scrollTop || document.body.scrollTop) + document.getElementById('listviewInstanceL').offsetHeight;
        // let max = document.getElementById('listviewInstanceL').scrollHeight;
        // if(pos == max && this.listData?.length < this.pagination.totalCount)   {
        //     this.getNewData();
        // }
        let pos = this.round((document.getElementById('listviewInstanceL').scrollTop || document.body.scrollTop) + document.getElementById('listviewInstanceL').offsetHeight);
        let match = this.round(document.getElementById('listviewInstanceL').scrollHeight * 0.68);
        if (pos == match && this.listLeft.length < this.pagination.totalCount && !this.pending) {
            this.getNewData();
        }
    }

    round(num) {
        let num1 = Math.round(num)?.toString().slice(0, -3);
        return Number(num1);
    }

    getNewData() {
        if (this.pending || this.listLeft.length >= this.pagination.totalCount) return;
        this.pending = true;
        this._requestServiceWeb.getListRequestsCanTransferWeb(
            this.isFollowUp == 1 ? 0 : this.typeFilter,
            this.systemFilter,
            this.statusFilter,
            this.searchFilter,
            1,
            this.isFollowUp,
            this.filterRequester?.id ?? 0,
            this.orderRequestDateFilter,
            this.orderExpectedDateFilter,
            10,
            this.listLeft?.length,
        )
            .pipe(finalize(() => {
                this.pending = false;
            }))
            .subscribe(res => {
                if (res.items.length) {
                    this.listLeft = this.listLeft.concat(res.items)
                        .filter(item => !this.listRight.some(item2 => item2.requestId == item.requestId))
                        .filter(x => x.statusCode !== 'Draft');
                    this.pagination.totalCount = res.totalCount;
                }
            });
    }

    submitTransfer() {
        this.loading = true;
        if (!this.transferTo?.id) {
            this.loading = false;
            this.notify.warn(this.l('PleaseSelectSigner'));
            return;
        }
        if (!this.listRight?.length) {
            this.loading = false;
            return;
        }
        let body = new TransferInputDto({
            note: '',
            transferUserId: this.transferTo.id,
            requestId: this.listRight.map(item => item.requestId)
        });
        this._esignSignerList.transferRequest(body)
            .pipe(finalize(() => {
                this.loading = false;
            }))
            .subscribe(() => {
                this.notify.success('Transfer request successfully');
                this.listRight = [];
                this.searchRequest();
            })
    }

    clearTextSearch() {
        this.requestFilter = '';
        this.transferFrom = TransferType.All;
        this.transferFromDate = undefined;
        this.transferToDate = undefined;
        this.searchHistory();
    }

    changeSelection(e: any) {
        this.selectionRow = e.data
    }
    changePager(event: any) {
        this.paginationHistory = event;
        this.searchHistory();
    }

    searchHistory() {
        this._requestServiceWeb.getTransferHistory(
            this.requestFilter,
            this.transferFrom,
            this.transferFromDate,
            this.transferToDate,
            this.paginationHistory.pageSize,
            (this.paginationHistory.pageNum - 1) * this.paginationHistory.pageSize,
        )
            .subscribe(res => {
                this.rowDataHistory = res.items;
                this.paginationHistory.totalCount = res.totalCount;
                this.paginationHistory.totalPage = Math.ceil(res.totalCount / this.paginationHistory.pageSize);
            });
    }

    changeTabView(event) {
        if (event == 1) {
            this.paginationHistory = { totalCount: 0, totalPage: 0, pageNum: 1, pageSize: 50 };
            this.clearTextSearch();
        }
    }

    dialogClose() {
    }

    cancel() {
        this.removeUserId = this.removeUserId.filter(item => !this.removeUserIdTemp.includes(item));
    }

    submitAnyway() {
        this.submitTransfer();
        this.removeUserIdTemp = [];
    }

    changeList() {
        this.listRight = this.listRight.concat(this.listLeft.filter(item => item.checked));
        this.listLeft = this.listLeft.filter(item => !item.checked);
        this.selectedLeftCount = this.listLeft.filter(item => item.checked).length;
        this.selectedRightCount = this.listRight.filter(item => item.checked).length;
        setTimeout(() => {
            this.checkAllHandlerR({ checked: false });
        }, 50);
        this.checkboxL.checked = false;
        this.checkboxR.checked = false;
    }
}
