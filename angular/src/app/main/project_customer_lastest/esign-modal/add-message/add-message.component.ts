import { Component, ElementRef, EventEmitter, Injector, OnInit, Output, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { AppComponentBase } from '@shared/common/app-component-base';
import { GlobalValidator } from '@shared/utils/validators';
import { ModalDirective } from 'ngx-bootstrap/modal';

@Component({
    selector: 'add-message',
    templateUrl: './add-message.component.html',
    styleUrls: ['./add-message.component.less']
})
export class AddMessageComponent extends AppComponentBase {
    @ViewChild('modal', { static: false }) modal!: ModalDirective;
    @Output() modalSave = new EventEmitter();
    dialogWidth: string = '35%';
    visible: Boolean = false;
    animationSettings: Object = { effect: 'FadeZoom' };
    message;
    id;
    constructor(
        injector: Injector,
        ) {
        super(injector);
    }

    ngOnInit() {

    }



    submit(){
        this.modalSave.emit({message: this.message, id: this.id});
        this.hide();
    }

    show(message, id){
        this.message = message;
        this.id = id;
        this.modal.show();
    }

    hide(){
        this.modal.hide();
        this.message = '';
    }

    changeKey(event){
        if(event.keyCode === 13 && this.message){
            this.submit();
        }
    }
}
