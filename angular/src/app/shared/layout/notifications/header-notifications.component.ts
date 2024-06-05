import { NotificationService } from './notification.service';
import { Component, Injector, OnInit, ViewEncapsulation, NgZone, Input, ViewChild } from '@angular/core';
import { AppComponentBase } from '@shared/common/app-component-base';
import { EsignSignerNotificationDto, EsignSignerNotificationServiceProxy, UpdateNotificationStatusInput } from '@shared/service-proxies/service-proxies';
import { IFormattedUserNotification, UserNotificationHelper } from './UserNotificationHelper';
import { forEach as _forEach } from 'lodash-es';
import { UrlHelper } from '@shared/helpers/UrlHelper';
import { PaginationParamCustom } from '@app/shared/common/models/base.model';
import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { finalize } from 'rxjs';
import { Listbox } from 'primeng/listbox';

@Component({
    templateUrl: './header-notifications.component.html',
    selector: 'header-notifications',
    styleUrls: ['./header-notifications.component.less'],
    encapsulation: ViewEncapsulation.None,
})
export class HeaderNotificationsComponent extends AppComponentBase implements OnInit {
    @Input() customStyle = 'btn btn-active-color-primary btn-active-light btn-custom btn-icon btn-icon-muted h-35px h-md-40px position-relative w-35px ';
    @Input() iconStyle = 'fa-regular fa-bell unread-notification fs-4';
    @Input() isRight = true;
    @ViewChild('listViewNotification') listViewNotification: Listbox;
    notifications: EsignSignerNotificationDto[] = [];
    unreadNotificationCount = 0;
    totalUnreadW = 0;
    totalUnreadA = 0;
    activeType = 0;
    pagination: PaginationParamCustom = { pageNum: 1, pageSize: 50, totalCount: 0 };
    pending = false;
    typeRead = 0;
    textState = 'Show Read'
    constructor(
        injector: Injector,
        private _notificationService: EsignSignerNotificationServiceProxy,
        private _userNotificationHelper: UserNotificationHelper,
        public _zone: NgZone,
        private readonly dateFormat: DateTimeService,
        private changeNoti: NotificationService
    ) {
        super(injector);
    }

    ngOnInit(): void {
        this.loadNotifications();
        this.registerToEvents();
    }

    loadNotifications(): void {
        if (UrlHelper.isInstallUrl(location.href)) {
            return;
        }

        this._notificationService.getUserNotification(
            this.typeRead,
            this.activeType,
            0,
            this.pagination.pageSize).subscribe((result) => {
                this.unreadNotificationCount = result.totalAllUnread;
                this.pagination.totalCount = result.totalCount;
                this.totalUnreadA = result.totalArUnread;
                this.totalUnreadW = result.totalWfoUnread;
                this.notifications = [];
                _forEach(result.notifications, (item: EsignSignerNotificationDto) => {
                    this.notifications.push(item);
                });
            });

        this.shouldUserUpdateApp();
    }
    getNewData(isAll = false) {
        this.pending = true;
        this._notificationService.getUserNotification(
            0,
            this.activeType,
            this.notifications.length,
            isAll ? this.pagination.totalCount : this.pagination.pageSize)
            .pipe(finalize(() => {
                this.pending = false;
            }))
            .subscribe((result) => {
                this.unreadNotificationCount = result.totalAllUnread;
                _forEach(result.notifications, (item: EsignSignerNotificationDto) => {
                    this.notifications.push(item);
                });
            });
    }
    onChangeType(event) {
        this.activeType = event.selectedIndex;
        this.loadNotifications();
    }
    onScroll() {
        let pos = this.round((document.getElementById('listViewNotification').scrollTop || document.body.scrollTop) + document.getElementById('listViewNotification').offsetHeight);
        let match = this.round(document.getElementById('listViewNotification').scrollHeight * 0.68);
        let max = this.round(document.getElementById('listViewNotification').scrollHeight);
        if (pos == match && this.notifications.length < this.pagination.totalCount && this.pending == false) {
            this.getNewData();
        }
        else if (pos == max && this.notifications.length < this.pagination.totalCount && this.pending == false) {
            this.getNewData(true);
        }
    }

    round(num) {
        let num1 = Math.round(num)?.toString().slice(0, -3);
        return Number(num1);
    }
    registerToEvents() {
        let self = this;

        function onNotificationReceived(userNotification) {
            self._userNotificationHelper.show(userNotification);
            self.notify.info(userNotification, 'New Notification');
            self.changeNoti.setNotification(true);
            self.loadNotifications();
        }

        this.subscribeToEvent('abp.notifications.received', (userNotification) => {
            self._zone.run(() => {
                onNotificationReceived(userNotification);
            });
        });

        function onNotificationsRefresh() {
            self.loadNotifications();
        }

        this.subscribeToEvent('app.notifications.refresh', () => {
            self._zone.run(() => {
                onNotificationsRefresh();
            });
        });

        // function onNotificationsRead(userNotificationId, success) {
        //     for (let i = 0; i < self.notifications.length; i++) {
        //         if (self.notifications[i].userNotificationId === userNotificationId) {
        //             self.notifications[i].state = 'READ';
        //             self.notifications[i].isUnread = false;
        //         }
        //     }

        //     if (success){
        //         self.unreadNotificationCount -= 1;
        //     }
        // }

        // this.subscribeToEvent('app.notifications.read', (userNotificationId, success) => {
        //     self._zone.run(() => {
        //         onNotificationsRead(userNotificationId, success);
        //     });
        // });
    }

    shouldUserUpdateApp(): void {
        this._userNotificationHelper.shouldUserUpdateApp();
    }

    setAllNotificationsAsRead(): void {
        this._userNotificationHelper.setAllAsRead();
    }

    openNotificationSettingsModal(): void {
        this._userNotificationHelper.openSettingsModal();
    }

    setNotificationAsRead(userNotification: IFormattedUserNotification): void {
        if (userNotification.state !== 'READ') {
            this._userNotificationHelper.setAsRead(userNotification.userNotificationId);
        }
    }

    gotoUrl(url): void {
        if (url) {
            location.href = url;
        }
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

    changeTypeRead() {
        if (this.typeRead == 0) {
            this.typeRead = 1;
            this.textState = this.l('ShowUnread');
        }
        else if (this.typeRead == 1) {
            this.typeRead = 2;
            this.textState =  this.l('ShowAll');
        }
        else {
            this.typeRead = 0;
            this.textState =  this.l('ShowRead');
        }
        this.loadNotifications();
    }

    readNotification(event) {
        let id = event?.option?.id;
        if (id && event?.option?.isRead == false) {
            this._notificationService.updateNotificationStatus(
                new UpdateNotificationStatusInput({
                    isRead: true,
                    isUpdateAll: false,
                    notificationId: id,
                    tabTypeId: this.activeType
                })
            )
                .pipe(finalize(() => {
                    this.notifications = this.notifications.map(x => {
                        if (x.id == id) {
                            x.isRead = true;
                        }
                        return x;
                    });
                    // this.listViewNotification.autoUpdateModel();
                }))
                .subscribe(() => {
                    // this.notify.success(this.l('MarkAsReadSuccess'));
                });
        }
    }

    readAllNotification() {
        this._notificationService.updateNotificationStatus(
            new UpdateNotificationStatusInput({
                isRead: true,
                isUpdateAll: true,
                notificationId: 0,
                tabTypeId: this.activeType
            })
        )
            .pipe(finalize(() => {
                this.notify.success(this.l('MarkAllAsReadSuccess'));
            }))
            .subscribe(() => {
                this.loadNotifications();
            });
    }
}
