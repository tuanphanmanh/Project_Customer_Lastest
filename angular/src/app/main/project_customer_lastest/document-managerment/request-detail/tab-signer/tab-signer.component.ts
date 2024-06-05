import { Component, Input, OnInit, ViewChild } from '@angular/core';
import { AddMessageComponent } from '@app/main/project_customer_lastest/esign-modal/add-message/add-message.component';
import { AnimationModel, ILoadedEventArgs, ProgressBar, ProgressTheme } from '@syncfusion/ej2-angular-progressbar';

@Component({
    selector: 'tab-signer',
    templateUrl: './tab-signer.component.html',
    styleUrls: ['./tab-signer.component.less']
})
export class TabSignerComponent implements OnInit {
    @Input() signers: any;
    @Input() totalSignersSigned: any;
    @Input() totalSigners: any;
    @Input() isRequester: boolean;
    @ViewChild('addMessage') addMessage!: AddMessageComponent;
    @ViewChild('linear') public linear: ProgressBar;
    private _addCC: string;
    @Input() set addCC(value) {
        if (value)
            this._addCC = value.replace(/;/g, ', ');
        else
            this._addCC = value;
    };
    get addCC() {
        return this._addCC;
    }

    public animation: AnimationModel = { enable: true, duration: 1000, delay: 0 };
    constructor() {
    }

    ngOnInit() {
    }

    public load(args: ILoadedEventArgs): void {
        let div: HTMLCollection = document.getElementsByClassName('progressbar-label');
        let selectedTheme: string = location.hash.split('/')[1];
        selectedTheme = selectedTheme ? selectedTheme : 'Material';
        args.progressBar.theme = <ProgressTheme>(selectedTheme.charAt(0).toUpperCase() +
            selectedTheme.slice(1)).replace(/-dark/i, 'Dark').replace(/contrast/i, 'Contrast');
        if (args.progressBar.theme === 'HighContrast' || args.progressBar.theme === 'Bootstrap5Dark' || args.progressBar.theme === 'BootstrapDark' || args.progressBar.theme === 'FabricDark'
            || args.progressBar.theme === 'TailwindDark' || args.progressBar.theme === 'MaterialDark' || args.progressBar.theme === 'FluentDark' || args.progressBar.theme === 'Material3Dark') {
            for (let i = 0; i < div.length; i++) {
                div[i].setAttribute('style', 'color:white');
            }
        } else if (selectedTheme.indexOf('bootstrap') > -1) {
            for (let i = 0; i < div.length; i++) {
                div[i].setAttribute('style', 'top: 0px');
            }
        }
    }

    saveMessage(event) {
        this.signers.map((signer) => {
            signer.signers.map((item) => {
                if (item.userId === event.id) {
                    item.privateMessage = event.message;
                }
            });
        });
    }

    number(number){
        return Number(number);
    }
}
