import { Component, EventEmitter, Injector, OnInit, Output, ViewChild } from '@angular/core';
import { AppComponentBase } from '@shared/common/app-component-base';
import { ModalDirective } from 'ngx-bootstrap/modal';

@Component({
    selector: 'missing-signature',
    templateUrl: './missing-signature.component.html',
    styleUrls: ['./missing-signature.component.less']
})
export class MissingSignatureComponent extends AppComponentBase {
    @ViewChild('modal', { static: false }) modal!: ModalDirective;
    @Output() signAnyway: EventEmitter<any> = new EventEmitter<any>();
    dialogWidth: string = '40%';
    visible: Boolean = false;
    animationSettings: Object = { effect: 'FadeZoom' };
    listMissingSignature;
    constructor(injector: Injector) {
        super(injector);
    }

    ngOnInit() {
    }

    submit(){
        this.signAnyway.emit(null);
        this.listMissingSignature = [];
        this.modal.hide();
    }

    show(listMissing: any[]){
        this.listMissingSignature = listMissing;
        this.modal.show();
    }

    hide(){
        this.listMissingSignature = [];
        this.modal.hide();
    }

}
