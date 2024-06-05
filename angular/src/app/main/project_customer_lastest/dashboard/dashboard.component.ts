import { Component, Injector, NgZone, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { CommonFunction } from '@app/main/commonfuncton.component';
import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { AppSessionService } from '@shared/common/session/app-session.service';
import { EsignRequestWebServiceProxy, GetDataForDashboardDto, SessionServiceProxy } from '@shared/service-proxies/service-proxies';
import { LocalStorageService } from '@shared/utils/local-storage.service';
import * as moment from 'moment';

@Component({
    selector: 'app-dashboard',
    templateUrl: './dashboard.component.html',
    styleUrls: ['./dashboard.component.less'],
})
export class DashboardComponent extends AppComponentBase implements OnInit {
    date = moment().format('ddd DD/MM/YYYY HH:mm');
    user: any;
    public listData = new GetDataForDashboardDto();
    _fn: CommonFunction = new CommonFunction();
    appSession: AppSessionService;

    constructor(
        injector: Injector,
        private _sessionService: SessionServiceProxy,
        private _requestService: EsignRequestWebServiceProxy,
        private dateFormat: DateTimeService,
        private router: Router,
        private local: LocalStorageService,
        private zone: NgZone
    ) {
        super(injector);

        this._fn.isShowUserProfile();
        this.appSession = injector.get(AppSessionService);
    }

    ngOnInit() {
        this._sessionService.getCurrentLoginInformations().subscribe((result) => {
            this.user = result.user;
        });
        this.searchData();

    }

    searchData(){
        this._requestService.getListRequestsForDashboard().subscribe(res => {
            this.listData = res;
        });
    }

    onClickRequest(event,_typeid) {
        let requestId = event?.value[0]?.requestId;
        let _createdRequestId =  Number(event?.value[0]?.createdRequesterId);
        let _CC = event?.value[0]?.cc;
        this.setLocalSearchFilter(_typeid, _createdRequestId,_CC);
        setTimeout(() => {
            // không dùng thằng này, trường hợp từ màn document vào màn dashboard , xong từ dashbord vào lại màn document
            // this.router.navigate(['app/main/document-management'], { queryParams: { requestid: requestId } });
            this.local.setItem("selectRequestFromDashboard",requestId )
            this.router.navigate(['app/main/document-management']);

            // window.location.href = "/app/main/document-management?requestid=" + requestId;
        }, 100);
    }

    typeFilter = 1; // value: 1,2,3 -> index: 0,1,2
    systemFilter = 1;
    statusFilter = '';
    searchFilter;
    searchFilterType = 1;
    isFollowUp: number = 0;
    filterRequester: any;
    orderCreationTimeFilter = 'desc';
    orderModifyTimeFilter = 'desc';
    pagination = { pageNum: 1, pageSize: 50, totalCount: 0, totalPage: 1 };

    setLocalSearchFilter(_typeid, _createdRequestId?, _CC?) {


        this.local.getItem("localSearchFilter" + abp.session.userId, (err, data) =>{
            if(data) {

                if(_typeid == 1 || _typeid == 2) this.typeFilter = _typeid; // waiting for orther/ Action Required
                else if(_typeid == 4) {  //transfer
                    this.typeFilter = 2;
                    this.statusFilter = "Transfer";
                }
                else if(_typeid == 3) {  //complete/rejected

                    if (_createdRequestId == abp.session.userId)  {
                        this.typeFilter = 2;
                        this.statusFilter = "";
                    }
                    else if (_createdRequestId != abp.session.userId &&
                        _CC!= null && _CC != undefined &&
                        _CC.indexOf(this.appSession.user.emailAddress.toLowerCase()) != -1 ){
                        this.typeFilter = 2;
                        this.statusFilter = "";

                    }else {
                        this.typeFilter = 1;
                        this.statusFilter = "";
                    }
                }

                this.orderCreationTimeFilter = data.orderCreationTimeFilter;
                this.orderModifyTimeFilter = data.orderModifyTimeFilter;
                this.pagination.pageSize = data.pageSize;
            }

            this.local.setItem('localSearchFilter' + abp.session.userId,Object.assign({}, {
                //requestId: this.selectedRequest.id,
                typeFilter: this.typeFilter,
                systemFilter: this.systemFilter,
                statusFilter: this.statusFilter,
                searchFilter: this.searchFilter,
                searchFilterType: this.searchFilterType,
                isFollowUp: this.isFollowUp,
                filterRequester: this.filterRequester,
                orderCreationTimeFilter: this.orderCreationTimeFilter,
                orderModifyTimeFilter: this.orderModifyTimeFilter,
                pageSize: this.pagination.pageSize,
            }));

        });



        // setTimeout(() => {
            // save

        // }, 200);
    }



    formatDate(input){
        if(input){
            //check in day
            let date = new Date(input);
            let now = new Date();
            if(date.getDate() === now.getDate() && date.getMonth() === now.getMonth() && date.getFullYear() === now.getFullYear()){
                //return 10 minutes ago
                let minutes = Math.floor((now.getTime() - date.getTime()) / 60000);
                if(minutes === 0){
                    return 'Just now';
                }
                else if(minutes < 60){
                    return minutes + 'm ago';
                }else{
                    let hours = Math.floor(minutes / 60);
                    return hours + 'h ago';
                }
            }
            //in year return type Oct 21
            else if(date.getFullYear() === now.getFullYear()){
                return this.dateFormat.formatDate(date, 'MMM d');
            }
            else{
                //return type Oct 21, 2020
                return this.dateFormat.formatDate(input as Date, 'MMM d, yyyy');
            }
        }
        else return '';
    }


    dueDate(request: any, _activeTab?) {
        // isActiveTab hiển thị 2 format khác nhau
        if(request && request?.expectedDate && _activeTab == 1){
            let date = new Date(request.expectedDate); // chỉ có ngày. không có giờ
            let timenow = new Date(); //có cả giờ phút giây, nếu cùng ngày thì thằng này thành quá hạn vì có cả giờ
            let daynow = new Date(timenow.getFullYear(), timenow.getMonth(), timenow.getDate()); // chỉ có ngày. không có giờ
            // console.log(timenow.getMonth(), timenow.getDay())

            let diff = date.getTime() - daynow.getTime();
            let days = Math.floor(diff / (1000 * 60 * 60 * 24));



            if(days > 0) {  // còn hạn (DaysLeft)
                return `<span class="date-due-black"> <span class="text-due">` + (days) + ` </span>`+ this.l('DaysLeft').toString().toUpperCase() + `</span>`;
            }
            else if ( days == 0) { // ngày cuối (LastDay)
                return `<span class="date-due-black">` + this.l('LastDay').toString().toUpperCase() + `</span>`;
            }
            else if (days < 0){ // quá hạn (PastDue)
                return `<span class="text-due">` + this.l('PastDue').toString().toUpperCase() + `</span>`;
                // if (days === 0) {
                //     return `<span class="text-due-black">` + this.l('PastDue').toString().toUpperCase() + `</span>`;
                // }
                // //check in year
                // else if (date.getFullYear() === now.getFullYear()) {
                //     return `<span class="text-due">`+days+`</span>` + ' <span class="text-due-black">' + this.l('DaysLeft').toString().toUpperCase() + '</span>';
                // }
                // else {
                //     return '<span class="date-due-black">'+ this.dateFormat.formatDate(request.expectedDate as Date, 'd/MMM').toString().toUpperCase() +'</span>';
                // }
            }
        }
        else if(request && request?.expectedDate && _activeTab == 2){

            let date = new Date(request.expectedDate); // chỉ có ngày. không có giờ
            let timenow = new Date(); //có cả giờ phút giây, nếu cùng ngày thì thằng này thành quá hạn vì có cả giờ
            let daynow = new Date(timenow.getFullYear(), timenow.getMonth(), timenow.getDate()); // chỉ có ngày. không có giờ
            // console.log(timenow.getMonth(), timenow.getDay(), daynow)

            let diff = date.getTime() - daynow.getTime();
            let days = Math.floor(diff / (1000 * 60 * 60 * 24));

            let cssclass = '';
            if (days < 0)  cssclass = 'text-due';
            else cssclass = 'date-due-black';

            if (date.getFullYear() === daynow.getFullYear()) {
                return '<span class="'+cssclass+'">'+ this.dateFormat.formatDate(request.expectedDate as Date, 'd/MMM').toString().toUpperCase() +'</span>';
            }
            else {
                return '<span class="'+cssclass+'">'+ this.dateFormat.formatDate(request.expectedDate as Date, 'd/MMM/yyyy').toString().toUpperCase() +'</span>';
            }
        }
    }


}
