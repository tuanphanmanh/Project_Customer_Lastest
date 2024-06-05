import { AbpSessionService } from 'abp-ng2-module';
import { Component, Injector, OnInit, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { accountModuleAnimation } from '@shared/animations/routerTransition';
import { AppComponentBase } from '@shared/common/app-component-base';
import { SessionServiceProxy, UpdateUserSignInTokenOutput } from '@shared/service-proxies/service-proxies';
import { UrlHelper } from 'shared/helpers/UrlHelper';
import { LoginService } from './login.service';
// import { ReCaptchaV3Service } from 'ng-recaptcha';
import { AppConsts } from '@shared/AppConsts';
import { filter as _filter } from 'lodash-es';
import { AppPreBootstrap } from 'AppPreBootstrap';

@Component({
    templateUrl: './login.component.html',
    animations: [accountModuleAnimation()],
    styleUrls: ['./login.component.less'],
})
export class LoginComponent extends AppComponentBase implements OnInit {
    submitting = false;
    isMultiTenancyEnabled: boolean = this.multiTenancy.isEnabled;
    tenantChangeDisabledRoutes: string[] = [
        'select-edition',
        'buy',
        'upgrade',
        'extend',
        'register-tenant',
        'stripe-purchase',
        'stripe-subscribe',
        'stripe-update-subscription',
        'paypal-purchase',
        'stripe-payment-result',
        'payment-completed',
        'stripe-cancel-payment',
        'session-locked',
    ];
    constructor(
        injector: Injector,
        public loginService: LoginService,
        private _router: Router,
        private _sessionService: AbpSessionService,
        private _sessionAppService: SessionServiceProxy,
        // private _reCaptchaV3Service: ReCaptchaV3Service
    ) {
        super(injector);
    }

    get multiTenancySideIsTeanant(): boolean {
        return this._sessionService.tenantId > 0;
    }

    get isTenantSelfRegistrationAllowed(): boolean {
        return this.setting.getBoolean('App.TenantManagement.AllowSelfRegistration');
    }

    get isSelfRegistrationAllowed(): boolean {
        if (!this._sessionService.tenantId) {
            return false;
        }

        return this.setting.getBoolean('App.UserManagement.AllowSelfRegistration');
    }

    get useCaptcha(): boolean {
        return this.setting.getBoolean('App.UserManagement.UseCaptchaOnLogin');
    }
    startAuthentication() {
        // Call the openAuthWindow method from the AuthService
        this.loginService.openAuthWindow();
      }
    ngOnInit(): void {
        if (this._sessionService.userId > 0 && UrlHelper.getReturnUrl() && UrlHelper.getSingleSignIn()) {
            this._sessionAppService.updateUserSignInToken().subscribe((result: UpdateUserSignInTokenOutput) => {
                const initialReturnUrl = UrlHelper.getReturnUrl();
                const returnUrl =
                    initialReturnUrl +
                    (initialReturnUrl.indexOf('?') >= 0 ? '&' : '?') +
                    'accessToken=' +
                    result.signInToken +
                    '&userId=' +
                    result.encodedUserId +
                    '&tenantId=' +
                    result.encodedTenantId;

                location.href = returnUrl;
            });
        }

        // this.handleExternalLoginCallbacks();
    }


    // handleExternalLoginCallbacks(): void {
    //     let state = UrlHelper.getQueryParametersUsingHash().state;
    //     let queryParameters = UrlHelper.getQueryParameters();

    //     if (state && state.indexOf('openIdConnect') >= 0) {
    //         this.loginService.openIdConnectLoginCallback({});
    //     }

    //     if (queryParameters.state && queryParameters.state.indexOf('openIdConnect') >= 0) {
    //         this.loginService.openIdConnectLoginCallback({});
    //     }

    //     if (queryParameters.twitter && queryParameters.twitter === '1') {
    //         let parameters = UrlHelper.getQueryParameters();
    //         let token = parameters['oauth_token'];
    //         let verifier = parameters['oauth_verifier'];
    //         this.loginService.twitterLoginCallback(token, verifier);
    //     }
    // }

    login(): void {
        let recaptchaCallback = (token: string) => {
            this.showMainSpinner();

            this.submitting = true;
            this.loginService.authenticate(
                () => {
                    this.submitting = false;
                    this.hideMainSpinner();
                },
                null,
                token
            );
        };

        // if (this.useCaptcha) {
        //     this._reCaptchaV3Service.execute('login')
        //         .subscribe((token) => recaptchaCallback(token));
        // } else {
            recaptchaCallback(null);
        // }
    }
    login365(): void {
        this.loginService.openAuthWindow(
            () => {
                this.submitting = false;
                this.hideMainSpinner();
            }
        );
    }

    // externalLogin(provider: ExternalLoginProvider) {
    //     this.loginService.externalAuthenticate(provider);
    // }

    showTenantChange(): boolean {
        if (!this._router.url) {
            return false;
        }

        if (
            _filter(this.tenantChangeDisabledRoutes, (route) => this._router.url.indexOf('/account/' + route) >= 0)
                .length
        ) {
            return false;
        }

        return abp.multiTenancy.isEnabled && !this.supportsTenancyNameInUrl();
    }

    private supportsTenancyNameInUrl() {
        return AppPreBootstrap.resolveTenancyName(AppConsts.appBaseUrlFormat) != null;
    }
}
