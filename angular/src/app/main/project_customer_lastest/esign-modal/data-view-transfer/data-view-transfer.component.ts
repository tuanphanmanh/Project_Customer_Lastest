// import { API_BASE_URL, EsignRequestServiceProxy } from '@shared/service-proxies/service-proxies';
import { Component, EventEmitter, Injector, Input, OnInit, Output, ViewChild } from '@angular/core';
import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { GridComponent, SelectionSettingsModel } from '@syncfusion/ej2-angular-grids';
// import { DataManager, ODataV4Adaptor, Query, WebApiAdaptor  } from '@syncfusion/ej2-data';
@Component({
    selector: 'data-view-transfer',
    templateUrl: './data-view-transfer.component.html',
    styleUrls: ['./data-view-transfer.component.less'],
})
export class DataViewTransferComponent extends AppComponentBase {
    @ViewChild('listviewInstance') listviewInstance: GridComponent;
    @Input() dataInput: any;
    @Input() isActiveTab: number; // value: 1,2,3 -> index: 0,1,2 -->  Action Required: 1, Waiting For Others: 2
    @Output() onChangeSelection: EventEmitter<any> = new EventEmitter<any>();
    public selectionOptions?: SelectionSettingsModel = { type: 'Multiple', checkboxMode: 'ResetOnRowClick', enableSimpleMultiRowSelection: true };
    constructor(
        private injector: Injector,
        private readonly dateFormat: DateTimeService
    ) {
        super(injector);
    }

    ngOnInit() {
    }

    calculateOffset(index: number, items): number {
        let offset = 0;
        for (let i = 0; i < index; i++) {
            offset += items[i].signers.length;
        }
        return offset;
    }

    calculateTotalSigners(items): number {
        let total = 0;
        items.forEach((item) => {
            total += item.signers.length;
        });
        return total;
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
                return this.dateFormat.formatDate(date, 'd/MMM');
            }
            else {
                //return type Oct 21, 2020
                return this.dateFormat.formatDate(input as Date, 'd/MMM/yyyy');
            }
        }
        else return '';
    }
    checkHandler(event, requestId) {
        if (event.checked) {
            this.listviewInstance.selectRows([...this.listviewInstance.getSelectedRowIndexes(), this.listviewInstance.getRowIndexByPrimaryKey(requestId)]);
        }
        this.onChangeSelection.emit({ checked: event.checked, requestId: requestId });
    }

    onRowSelected(event) {
        this.onChangeSelection.emit({ checked: true, requestId: event.data.requestId });
    }

    onRowDeSelected(event) {
        this.onChangeSelection.emit({ checked: false, requestId: event.data.requestId });
    }

    dueDate(request: any) {
        // isActiveTab hiển thị 2 format khác nhau
        if(request && request?.expectedDate && this.isActiveTab == 1){
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
        else if(request && request?.expectedDate && this.isActiveTab == 2){

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
