import { Component, Injector } from '@angular/core';
import { AppComponentBase } from '@shared/common/app-component-base';

@Component({
    selector: 'message-custom',
    templateUrl: './message-custom.component.html',
    styleUrls: ['./message-custom.component.less']
})
export class MessageCustomComponent extends AppComponentBase {
    Title:string = "";
    Content:string = "";
    constructor(injector: Injector) {
        super(injector);
    }

    ngOnInit() {

    }

    hide(){
        document.getElementById('messageCustom').style.display = 'none';
    }
}
