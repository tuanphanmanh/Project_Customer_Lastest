import { BehaviorSubject } from "rxjs";

export class NotificationService {
    private notification$ = new BehaviorSubject<any>(null);
    changeNotification = this.notification$.asObservable();
    setNotification(notification: any) {
        this.notification$.next(notification);
    }
}
