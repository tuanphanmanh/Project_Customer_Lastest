import { Component, EventEmitter, Injector, Output, ViewChild } from '@angular/core';
import { AppComponentBase } from '@shared/common/app-component-base';
import { ModalDirective } from 'ngx-bootstrap/modal';

@Component({
    selector: 'revoke-modal',
    templateUrl: './revoke-modal.component.html',
    styleUrls: ['./revoke-modal.component.less']
})
export class RevokeModalComponent extends AppComponentBase {
    @ViewChild('modal', { static: false }) modal!: ModalDirective;
    @Output() onRevoke: EventEmitter<any> = new EventEmitter<any>();
    dialogWidth: string = '35%';
    dialogHeight: string = '35%';
    visible: Boolean = false;
    messageText: string;
    animationSettings: Object = { effect: 'FadeZoom' };
    saving = false;
    constructor(injector: Injector) {
        super(injector);
    }

    ngOnInit() {
    }
    submit(){
        this.saving = true;
        this.onRevoke.emit(this.messageText);
        this.modal.hide();
        this.messageText = '';
    }

    show(){
        this.saving = false;
        this.messageText = '';
        this.modal.show();
    }

    hide(){
        this.saving = false;
        this.messageText = '';
        this.modal.hide();
    }
}
