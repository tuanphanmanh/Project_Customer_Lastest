import { Component, Injector, OnInit } from '@angular/core';
import { AppComponentBase } from '@shared/common/app-component-base';
import { LoginService } from './login.service';

@Component({
    selector: 'login-guest',
    templateUrl: './login-guest.component.html',
    styleUrls: ['./login-guest.component.less']
})
export class LoginGuestComponent extends AppComponentBase implements OnInit {
    submitting = false;

    constructor(
        injector: Injector,
        public loginService: LoginService,
    ) {
        super(injector);
    }

    ngOnInit() {
    }

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
        recaptchaCallback(null);
    }
    password_eye_click(type:number) {

        let _eyeOpen = document.querySelector<HTMLElement>(".login-account .login_content .login-form .form-group img.password-eye.password-eye-open");
        let _eyeOff = document.querySelector<HTMLElement>(".login-account .login_content .login-form .form-group img.password-eye.password-eye-off");
        let _pass = document.querySelector<HTMLElement>(".login-account .login_content .login-form .form-group input[name=password].input-login");

        if(type == 1) {
            _eyeOff.style.display = "none";
            _eyeOpen.style.display = "block";
            _pass.setAttribute("type","text");
        }
        else {
            _eyeOff.style.display = "block";
            _eyeOpen.style.display = "none";
            _pass.setAttribute("type","password");
        }
    }
}
