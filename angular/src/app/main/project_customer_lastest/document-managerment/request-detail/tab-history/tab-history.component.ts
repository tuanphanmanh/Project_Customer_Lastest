import { Component, Injector, Input, OnInit } from '@angular/core';
import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { AppComponentBase } from '@shared/common/app-component-base';

@Component({
    selector: 'tab-history',
    templateUrl: './tab-history.component.html',
    styleUrls: ['./tab-history.component.less']
})
export class TabHistoryComponent extends AppComponentBase {
    @Input() history: any;
    constructor(
        private injector: Injector,
        private readonly dateFormat: DateTimeService
    ) {
        super(injector);
    }

    ngOnInit() {
    }

    formatDate(input) {
        if (input) {
            //return type 29/11/2020 10:00:00
            return this.dateFormat.formatDate(input, 'dd/MMM/yyyy HH:mm:ss');
        }
        else return '';
    }
}
