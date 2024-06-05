import { ChangeDetectionStrategy, Component, EventEmitter, Injector, Input, OnInit, Output, ViewChild } from '@angular/core';
import { NavigationEnd, NavigationStart, Router } from '@angular/router';
import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { filter } from 'rxjs';
import { ScrolledEventArgs, ListViewComponent, VirtualizationService } from '@syncfusion/ej2-angular-lists';
@Component({
    selector: 'data-view-group',
    templateUrl: './data-view-group.component.html',
    styleUrls: ['./data-view-group.component.less'],
    changeDetection: ChangeDetectionStrategy.OnPush
})
export class DataViewGroupComponent extends AppComponentBase {
    @ViewChild('listviewGroup') listviewInstance: ListViewComponent;
    @Input() dataInput: any;
    @Output() onChangeSelection: EventEmitter<any> = new EventEmitter<any>();
    @Output() getNewData: EventEmitter<any> = new EventEmitter<any>();
    lastRoute: string;
    lastPosition: any;
    constructor(
        private injecter: Injector,
        private readonly dateFormat: DateTimeService,
        private router: Router,
    ) {
        super(injecter);
    }

    ngOnInit() {
        this.router.events.pipe(
            filter((events) => events instanceof NavigationStart || events instanceof NavigationEnd)
        ).subscribe(event => {
            if (event instanceof NavigationStart && event.url !== this.lastRoute) {
                this.lastRoute = this.router.url;
                if (document.getElementById('listviewGroup'))
                this.lastPosition = document.getElementById('listviewGroup').scrollTop; // get the scrollTop property
                // this.lastPosition = this.grid.nativeElement.scrollTop
            }
            else if (event instanceof NavigationEnd && event.url === this.lastRoute) {
                // (this.listviewInstance as ListViewComponent).getContent().children[0].scrollTop = this.lastPosition;
                if(document.querySelector<HTMLElement>('#listviewGroup'))
                document.querySelector<HTMLElement>('#listviewGroup').scrollTop = this.lastPosition;
                // this.grid.nativeElement.firstChild.scrollTop  = this.lastPosition
            }
        })
    }

    onChangeSelectionRow(event: any) {
        this.onChangeSelection?.emit(this.listviewInstance.getSelectedItems());
        // const rowHeight: number = (this.grid as any).getRows()[(this.grid as GridComponent).getSelectedRowIndexes()[0]].scrollHeight;
        // (this.grid as GridComponent).getContent().children[0].scrollTop = rowHeight * (this.grid as GridComponent).getSelectedRowIndexes()[0];

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

    selectFirstRow() {
        //this.listviewInstance.selectItem(this.listviewInstance.dataSource[0]);
    }

    selectIndex(_index) {
        this.listviewInstance.selectItem(this.listviewInstance.dataSource[_index]);
    }

    formatDate(input) {
        try {
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
        catch (e) {
            console.log(e);
            console.log('formatDate ', input);
            return '';
        }
    }

    onScroll(event: ScrolledEventArgs) {
        let posGetNew = this.listviewInstance.getRootElement().scrollHeight * 0.3;
        if (event.scrollDirection === "Bottom" && event.distanceY < posGetNew) {
            this.getNewData.emit(null);
        }
    }
}
