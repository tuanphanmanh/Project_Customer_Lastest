import { Component, EventEmitter, Injector, Output, ViewChild } from '@angular/core';
import { AppComponentBase } from '@shared/common/app-component-base';
import { ModalDirective } from 'ngx-bootstrap/modal';

@Component({
    selector: 'confirm-file-exists',
    templateUrl: './confirm-file-exists.component.html',
    styleUrls: ['./confirm-file-exists.component.less']
})
export class ConfirmFileExistsComponent extends AppComponentBase {
    @ViewChild('modal', { static: false }) modal!: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();
    @Output() modalNo: EventEmitter<any> = new EventEmitter<any>();
    @Output() signAnyway: EventEmitter<any> = new EventEmitter<any>();

    // documentData : any;

    Mode:string = "File";
    Title:string = "";
    Content:string = "";
    ListSigner: string[];

    UserId:string;
    isExistsSignatureMe:boolean = false;

    width:number = 400
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

    submitChange() {
        this.signAnyway.emit('submitChange');
        this.modal.hide();
    }
    submitAnyway() {
        this.signAnyway.emit('submitAnyway');
        this.modal.hide();
    }

    show(_title?: any, _content?: any, _mode?:any){
        // this.documentData = data ;
        if(_title) this.Title = _title;
        if(_content) this.Content = _content;
        if(_mode) this.Mode = _mode;

        if(this.Mode == 'DigitalSignature') {
            this.width = 560;
            this.ListSigner = this.Content.split(',');
            this.UserId = this.appSession.userId.toString();
            this.ListSigner.forEach(e => {
                let u = e.split('_')[0];
                if(u == this.UserId) {
                    this.isExistsSignatureMe = true;
                    return;
                }
            });
        }

        this.modal.show();
    }

    hide(){
        this.modalNo.emit(null)
        this.modal.hide();
    }
}
