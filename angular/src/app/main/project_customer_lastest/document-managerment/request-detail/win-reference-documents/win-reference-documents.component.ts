import { Component, Injector, Input, NgZone, OnInit } from '@angular/core';
import { AppComponentBase } from '@shared/common/app-component-base';
import { LocalStorageService } from '@shared/utils/local-storage.service';

@Component({
    selector: 'app-win-reference-documents',
    templateUrl: './win-reference-documents.component.html',
    styleUrls: ['./win-reference-documents.component.less'],
})
export class WinReferenceDocumentsComponent extends AppComponentBase implements OnInit {


    @Input() references: any;
    @Input() isRefDocument: boolean = false;
    @Input() allowAction: boolean = true;


    constructor(
        injector: Injector,
        private local: LocalStorageService,
    ) {
        super(injector);


        this.local.getItem("openwindowRefDocuments",(err, data)=>{
            this.references = data.refDocument;
            this.isRefDocument = data.isRefDocument;
            this.allowAction = data.allowAction;
            console.log(data);
            console.log(this.references);
        });

    }

    ngOnInit() {


    }

}
