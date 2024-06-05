import { Component, Injector, Output, EventEmitter, ViewChild } from '@angular/core';
import { AppComponentBase } from '@shared/common/app-component-base';
import { ModalDirective } from 'ngx-bootstrap/modal';

@Component({
    selector: 'remind-modal',
    templateUrl: './remind-modal.component.html',
    styleUrls: ['./remind-modal.component.less']
})
export class RemindModalComponent extends AppComponentBase {
    @ViewChild('modal', { static: false }) modal!: ModalDirective;
    @Output() onRemind: EventEmitter<any> = new EventEmitter<any>();

    dialogWidth: string = '35%';
    visible: Boolean = false;
    animationSettings: Object = { effect: 'FadeZoom' };
    saving = false;
    constructor(injector: Injector) {
        super(injector);
    }

    ngOnInit() {
    }

    submit(){
        this.saving = true;
        this.onRemind.emit(null);
        this.modal.hide();
    }

    show(){
        this.saving = false;
        this.modal.show();
    }

    hide(){
        this.saving = false;
        this.modal.hide();
    }
}
