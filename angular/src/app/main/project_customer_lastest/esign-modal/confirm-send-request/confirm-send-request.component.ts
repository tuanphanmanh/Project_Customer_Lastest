import { Component, EventEmitter, Injector, Output, ViewChild } from '@angular/core';
import { AppComponentBase } from '@shared/common/app-component-base';
import { CreateOrEditEsignRequestDto } from '@shared/service-proxies/service-proxies';
import { ModalDirective } from 'ngx-bootstrap/modal';

@Component({
    selector: 'confirm-send-request',
    templateUrl: './confirm-send-request.component.html',
    styleUrls: ['./confirm-send-request.component.less']
})
export class ConfirmSendRequestComponent extends AppComponentBase {
    @ViewChild('modal', { static: false }) modal!: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    documentData : any;

    dialogWidth: string = '35%';
    dialogHeight: string = '71%';
    visible: Boolean = false;
    animationSettings: Object = { effect: 'FadeZoom' };

    constructor(injector: Injector) {
        super(injector);
    }


    ngOnInit() {

    }

    submit(){
        this.modalSave.emit(null)
        this.modal.hide();
    }

    show(data?: any){
        this.documentData = data ;
        this.modal.show();
    }

    hide(){
        this.modal.hide();
    }
}
