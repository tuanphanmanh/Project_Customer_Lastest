import { Injector, Component, ViewEncapsulation, Inject, Input, OnInit } from '@angular/core';
import { AppConsts } from '@shared/AppConsts';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DOCUMENT } from '@angular/common';
import * as KTUtil from '@metronic/app/kt/_utils';
import { MstEsignLogoServiceProxy } from '@shared/service-proxies/service-proxies';

@Component({
    templateUrl: './default-logo.component.html',
    selector: 'default-logo',
    encapsulation: ViewEncapsulation.None,
})
export class DefaultLogoComponent extends AppComponentBase implements OnInit {
    @Input() customHrefClass = '';
    @Input() skin = null;

    defaultLogo = '';
    defaultSmallLogo = '';
    remoteServiceBaseUrl: string = AppConsts.remoteServiceBaseUrl;

    constructor(
        injector: Injector,
        @Inject(DOCUMENT) private document: Document,
        private readonly _logo: MstEsignLogoServiceProxy,
    ) {
        super(injector);
    }

    ngOnInit(): void {
        this._logo.getMstEsignLogoByTenant(this.appSession.tenant.id).subscribe(result => {
            if (result != null) {
                this.defaultLogo = this.remoteServiceBaseUrl + '/' + result.logoMaxUrl;
                this.defaultSmallLogo = this.remoteServiceBaseUrl + '/' + result.logoMinUrl;
            }
            else {
                this.setLogoUrl();
            }
        });
    }

    onResize() {
        this.setLogoUrl();
    }

    setLogoUrl(): void {
        this.defaultLogo = AppConsts.appBaseUrl + '/assets/common/images/logo-default.jpg';
        this.defaultSmallLogo = AppConsts.appBaseUrl + '/assets/common/images/logo-default-minimize.jpg';
    }
}
