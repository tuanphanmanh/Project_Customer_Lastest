import { Injectable, Injector } from '@angular/core';
import { AppConsts } from '@shared/AppConsts';
import { AppComponentBase } from '@shared/common/app-component-base';
import { XmlHttpRequestHelper } from '@shared/helpers/XmlHttpRequestHelper';
import { LocalStorageService } from '@shared/utils/local-storage.service';

@Injectable()
export class AppAuthService extends AppComponentBase {
    constructor(
        ịnjector: Injector,
    ){
        super(ịnjector);
    }
    logout(reload?: boolean, returnUrl?: string): void {
        let customHeaders = {
            [abp.multiTenancy.tenantIdCookieName]: abp.multiTenancy.getTenantIdCookie(),
            Authorization: 'Bearer ' + abp.auth.getToken(),
        };

        XmlHttpRequestHelper.ajax(
            'GET',
            AppConsts.remoteServiceBaseUrl + '/api/v1/TokenAuth/LogOut',
            customHeaders,
            null,
            () => {
                abp.auth.clearToken();
                abp.auth.clearRefreshToken();
                new LocalStorageService().removeItem(AppConsts.authorization.encrptedAuthTokenName, () => {
                    if (reload !== false) {
                        if (returnUrl) {
                            location.href = returnUrl;
                        } else {
                            if(this.appSession.user?.isAD === true){
                                location.href = 'https://login.microsoftonline.com/common/oauth2/logout?post_logout_redirect_uri=' + AppConsts.appBaseUrl;
                            }
                            else
                                location.href = '';
                        }
                    }
                });
            }
        );
    }
}
