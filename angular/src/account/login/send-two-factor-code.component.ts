import { Component, Injector, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { accountModuleAnimation } from '@shared/animations/routerTransition';
import { AppComponentBase } from '@shared/common/app-component-base';
import { SendTwoFactorAuthCodeModel, TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { LoginService } from './login.service';
import { finalize } from 'rxjs/operators';
import { ValidateTwoFactorCodeComponent } from '@account/login/validate-two-factor-code.component';
import { AppConsts } from '@shared/AppConsts';

@Component({
    templateUrl: './send-two-factor-code.component.html',
    animations: [accountModuleAnimation()],
})
export class SendTwoFactorCodeComponent extends AppComponentBase  implements OnInit {
    selectedTwoFactorProvider: string;
    submitting = false;

    constructor(
        injector: Injector,
        public loginService: LoginService,
        private _tokenAuthService: TokenAuthServiceProxy,
        private _router: Router
    ) {
        super(injector);
    }

    canActivate(): boolean {
        if (
            this.loginService.authenticateModel &&
            this.loginService.authenticateResult
        ) {
            return true;
        }

        return false;
    }

    ngOnInit(): void {
        this.selectedTwoFactorProvider = this.loginService.authenticateResult.twoFactorAuthProviders[0];
        if (!this.canActivate()) {
            this._router.navigate(['account/login']);
            return;
        }
    }

    submit(): void {
        const model = new SendTwoFactorAuthCodeModel();
        model.userId = this.loginService.authenticateResult.userId;
        model.provider = this.selectedTwoFactorProvider;
        model.tenancyName = AppConsts.tenancyName;

        this.submitting = true;
        this._tokenAuthService
            .sendTwoFactorAuthCode(model)
            .pipe(finalize(() => (this.submitting = false)))
            .subscribe(() => {
                this._router.navigate(['account/verify-code']);
            });
    }
}
