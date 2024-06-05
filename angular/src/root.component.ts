import { Component, ViewChild } from '@angular/core';
import { NgxSpinnerTextService } from '@app/shared/ngx-spinner-text.service';
import { MessageCustomComponent } from 'message-custom/message-custom.component';

@Component({
    selector: 'app-root',
    template: `
        <router-outlet></router-outlet>
        <message-custom #messageCustom id="messageCustom" style="display: none;"></message-custom>
        <ngx-spinner type="ball-clip-rotate" size="medium" color="#5ba7ea">
            <!-- <p *ngIf="ngxSpinnerText">{{ getSpinnerText() }}</p> -->
            <img style="width: 100px" src="/assets/common/images/Timer.gif"  *ngIf="ngxSpinnerText" alt="">
        </ngx-spinner>
        <style>
            #messageCustom {
                width: 100vw;
                height: 100vh;
                top: 0;
                position: absolute;
                left: 0;
                background-color: #808080a3;
                .pop-up.popup-confirm {
                    min-height: 200px;
                    height: auto;
                    position: absolute;
                    top: 30%;
                    left: calc(50% - 250px);
                    width: 500px;
                    z-index: 2;
                }
            }
        </style>
    `,
})
export class RootComponent {
    @ViewChild('mesageCustom') mesageCustom: MessageCustomComponent;
    ngxSpinnerText: NgxSpinnerTextService;

    constructor(_ngxSpinnerText: NgxSpinnerTextService) {
        this.ngxSpinnerText = _ngxSpinnerText;

        let url = window.location.href;
        let requestIdIndex = url.indexOf('RequestId=');
        if (requestIdIndex > -1) {
            let ids = url.substring(requestIdIndex + 'RequestId='.length).split('&')[0];
            window.location.href = "/app/main/view-detail/" + ids;
        }

    }

    getSpinnerText(): string {
        return this.ngxSpinnerText.getText();
    }
}
