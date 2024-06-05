import { Component, EventEmitter, Injector, OnInit, Output, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { AppComponentBase } from '@shared/common/app-component-base';
import { ModalDirective } from 'ngx-bootstrap/modal';

@Component({
    selector: 'sign-success',
    templateUrl: './sign-success.component.html',
    styleUrls: ['./sign-success.component.less']
})
export class SignSuccessComponent extends AppComponentBase {
    @ViewChild('modal', { static: false }) modal!: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();
    dialogWidth: string = '35%';
    visible: Boolean = false;
    animationSettings: Object = { effect: 'FadeZoom' };

    isProcessing:boolean = false;

    constructor(
        injector: Injector,
        private router: Router,
        ) {
        super(injector);
    }

    ngOnInit() {
    }

    submit(){
        let path = window.location.pathname;
        if(path.includes('document-management')){
            this.modalSave.emit(null);
            this.modal.hide();
        }
        else {
            this.router.navigate(['/app/main/document-management']);
        }
    }

    show(isProcessing){ // true : đang xử lý/ false: complete
        this.isProcessing = isProcessing;
        this.modal.show();
    }

    hide(){

        this.modal.hide();
    }
}
