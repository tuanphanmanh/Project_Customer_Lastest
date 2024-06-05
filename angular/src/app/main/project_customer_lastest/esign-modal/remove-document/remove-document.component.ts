import { Component, EventEmitter, Injector, OnInit, Output, ViewChild } from '@angular/core';
import { AppComponentBase } from '@shared/common/app-component-base';
import { ModalDirective } from 'ngx-bootstrap/modal';

@Component({
    selector: 'remove-document',
    templateUrl: './remove-document.component.html',
    styleUrls: ['./remove-document.component.less']
})
export class RemoveDocumentComponent extends AppComponentBase {
    @ViewChild('modal', { static: false }) modal!: ModalDirective;
    @Output() modalSave = new EventEmitter();
    index: any;
    dialogWidth: string = '35%';
    dialogHeight: string = '26%';
    visible: Boolean = false;
    animationSettings: Object = { effect: 'FadeZoom' };
    messageText: string = 'Are you sure, you want to delete this document? This action cannot be undone and also if you have configured any fields in this document, it will also get removed.';
    constructor(injector: Injector) {
        super(injector);
    }

    ngOnInit() {
    }
    submit() {
        this.modal.hide();
        this.modalSave.emit(this.index);
    }

    show(index: any) {
        this.index = index;
        if(index?.length > 1){
            this.messageText = 'Are you sure, you want to delete all documents? This action cannot be undone and also if you have configured any fields in these documents, it will also get removed.';
        }
        else{
            this.messageText = 'Are you sure, you want to delete this document? This action cannot be undone and also if you have configured any fields in this document, it will also get removed.';
        }
        this.modal.show();
    }

    hide() {
        this.modal.hide();
    }
}
