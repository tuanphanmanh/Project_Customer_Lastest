import { Component, EventEmitter, Injector, Output, ViewChild } from '@angular/core';
import { AppComponentBase } from '@shared/common/app-component-base';
import { ModalDirective } from 'ngx-bootstrap/modal';

@Component({
    selector: 'sign-to-page-modal',
    templateUrl: './sign-to-page-modal.component.html',
    styleUrls: ['./sign-to-page-modal.component.less']
})
export class SignToPageModalComponent extends AppComponentBase {
    @ViewChild('modal', { static: false }) modal!: ModalDirective;
    @Output() modalSave = new EventEmitter();
    dialogHeight: string = 'max-content';
    visible: Boolean = false;
    animationSettings: Object = { effect: 'FadeZoom' };
    page = "";
    constructor(
        injector: Injector,
        ) {
        super(injector);
    }

    ngOnInit() {

    }


    checkValidity(inputString) {
        const regex = /^(\d+(-\d+)?,)*\d+(-\d+)?$/;

        return regex.test(inputString);
    }

    submit(){
        let pages = []
        if (this.page?.trim() == "") return this.notify.warn("Pages input cannot be empty");
        if (!this.checkValidity(this.page)) return this.notify.warn("Incorrect condition");
        // if ( this.page?.includes(",")){
            this.page?.split(",")?.forEach(e => {
                if (e.includes("-")){
                    for(var i = Number(e?.split("-")[0]);i <= Number(e?.split("-")[1]);i++){
                        if (!pages.some(p => p == i)) pages.push(i)
                    }
                }
                else {
                    if (!pages.some(p => p == Number(e))) pages.push(Number(e))
                }
            })
        // }
        // else {
        //     if (this.page?.includes("-")){
        //         for(var i = Number(this.page?.split("-")[0]);i <= Number(this.page?.split("-")[1]);i++){
        //             if (!pages.some(p => p == i)) pages.push(i)
        //         }
        //     }
        //     else {
        //         if (!pages.some(p => p == Number(this.page))) pages.push(Number(this.page))
        //     }
        // }


        // if(this.page?.includes("-") && !this.page?.includes(";") && !this.page?.includes(",")){
        //     for(var i = Number(this.page?.includes("-")[0]);i <= Number(this.page?.includes("-")[1]);i++){
        //         pages.push(i)
        //     }
        // }
        // // else if (!this.page?.includes("-") && this.page?.includes(";") && !this.page?.includes(",")){
        // //     this.page?.split(";")?.forEach(e => {
        // //         pages.push(Number(e))
        // //     })
        // // }
        // else if (!this.page?.includes("-") && !this.page?.includes(";") && this.page?.includes(",")){
        //     this.page?.split(",")?.forEach(e => {
        //         pages.push(Number(e))
        //     })
        // }
        // else {
        //     pages.push(Number(this.page))
        // }

        this.modalSave.emit(pages);
        this.hide();
    }

    show(){
        this.modal.show();
    }

    hide(){
        this.modal.hide();
        this.page = "";
    }
}
