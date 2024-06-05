import { Component, Injector, OnDestroy, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { accountModuleAnimation } from '@shared/animations/routerTransition';
import { AppComponentBase } from '@shared/common/app-component-base';
import { Subscription, Observable } from 'rxjs';
import { timer } from 'rxjs';
import { LoginService } from './login.service';
// import { ReCaptchaV3Service } from 'ng-recaptcha';
import { AppConsts } from '@shared/AppConsts';
import { TwoFactModel, VerifyOTPInput } from '@shared/service-proxies/service-proxies';

@Component({
    templateUrl: './validate-two-factor-code.component.html',
    styleUrls: ['./validate-two-factor-code.component.less'],
    animations: [accountModuleAnimation()],
})
export class ValidateTwoFactorCodeComponent extends AppComponentBase  implements OnInit, OnDestroy {
    code: string;
    submitting = false;
    remainingSeconds = 90;
    timerSubscription: Subscription;

    constructor(
        injector: Injector,
        public loginService: LoginService,
        // private _reCaptchaV3Service: ReCaptchaV3Service,
        private _router: Router
    ) {
        super(injector);
    }

    get useCaptcha(): boolean {
        return this.setting.getBoolean('App.UserManagement.UseCaptchaOnLogin');
    }

    canActivate(): boolean {
        if (this.loginService.authenticateModel && this.loginService.authenticateResult) {
            return true;
        }
        return false;
    }

    ngOnInit(): void {
        // if (!this.canActivate()) {
        //     this._router.navigate(['account/login']);
        //     return;
        // }

        this.remainingSeconds = this.appSession.application.twoFactorCodeExpireSeconds;

        const timerSource = timer(1000, 1000);
        this.timerSubscription = timerSource.subscribe(() => {
            this.remainingSeconds = this.remainingSeconds - 1;
            if (this.remainingSeconds === 0) {
                this.message.warn(this.l('TimeoutPleaseTryAgain')).then(() => {
                    this._router.navigate(['account/login']);
                });
            }
        });
    }

    ngOnDestroy(): void {
        if (this.timerSubscription) {
            this.timerSubscription.unsubscribe();
            this.timerSubscription = null;
        }
    }

    submit(): void {
        let recaptchaCallback = (token: string) => {
            this.loginService.authenticateModel.twoFactorVerificationCode = this.code;
            this.loginService.authenticate(() => { }, null, token);
        };
        recaptchaCallback(null);
    }
}
