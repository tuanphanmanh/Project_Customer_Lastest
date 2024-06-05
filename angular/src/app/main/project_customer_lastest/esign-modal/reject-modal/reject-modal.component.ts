import { Component, EventEmitter, Injector, Output, ViewChild } from '@angular/core';
import { AppComponentBase } from '@shared/common/app-component-base';
import { ModalDirective } from 'ngx-bootstrap/modal';

@Component({
    selector: 'reject-modal',
    templateUrl: './reject-modal.component.html',
    styleUrls: ['./reject-modal.component.less']
})
export class RejectModalComponent extends AppComponentBase {
    @ViewChild('modal', { static: false }) modal!: ModalDirective;
    @Output() onReject: EventEmitter<any> = new EventEmitter<any>();
    dialogWidth: string = '35%';
    visible: Boolean = false;
    animationSettings: Object = { effect: 'FadeZoom'};
    reason: string = '';
    loading = false;
    constructor(injector: Injector) {
        super(injector);
    }

    ngOnInit() {
    }
    submit(){
        this.loading = true;
        this.onReject.emit(this.reason);
        this.reason = '';
        this.modal.hide();
        this.loading = false;
    }

    show(){
        this.loading = false;
        this.modal.show();
    }

    hide(){
        this.modal.hide();
        this.loading = false;
    }
}
