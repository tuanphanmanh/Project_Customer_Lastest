import { UserAccountInfomationDto, MstEsignActiveDirectoryServiceProxy, ChangeUserLanguageDto, ProfileServiceProxy } from '@shared/service-proxies/service-proxies';
import { AfterViewInit, Component, Injector, OnInit } from '@angular/core';
import { AppComponentBase } from '@shared/common/app-component-base';
import { finalize } from 'rxjs';
import { filter as _filter, orderBy } from 'lodash-es';

@Component({
    selector: 'app-account-setting',
    templateUrl: './account-setting.component.html',
    styleUrls: ['./account-setting.component.less']
})
export class AccountSettingComponent extends AppComponentBase implements OnInit, AfterViewInit {
    myProfile: UserAccountInfomationDto = new UserAccountInfomationDto();
    isLoading = false;
    languages: abp.localization.ILanguageInfo[] = [];
    selectedLanguage: abp.localization.ILanguageInfo;
    constructor(
        injector: Injector,
        private _profileService: MstEsignActiveDirectoryServiceProxy,
        private _profileServiceProxy: ProfileServiceProxy
        ) {
        super(injector);
    }

    ngOnInit() {
        this.getMyProfile();
    }

    ngAfterViewInit() {
        this.getMyProfile();
    }

    getMyProfile(){
        this.isLoading = true;
        this._profileService.getMyAccountInfomation()
        .pipe( finalize(() => {
            this.languages = orderBy(_filter(this.localization.languages, (l) => l.isDisabled === false), [l => l.name.toLocaleLowerCase()]);
            this.selectedLanguage = this.languages.find(x => x.name === this.myProfile.language) ?? this.localization.currentLanguage;
            this.isLoading = false;
        }))
        .subscribe(result => {
            this.myProfile = result;
            this.isLoading = false;
        });
    }

    onChangeDigitalSignature(event){
        if(!event?.checked){
            this.myProfile.isDigitalSignatureOtp = false;
        }
    }

    saveAccountInfo(){
        if(!this.myProfile.surname || !this.myProfile.givenName){
            this.notify.warn(this.l('PleaseEnterYourName'));
            return;
        }
        this.isLoading = true;
        this._profileService.saveAccountInfomation(this.myProfile)
        .pipe( finalize(() => {
            if(this.selectedLanguage?.name !== this.localization.currentLanguage.name){
                abp.utils.setCookieValue(
                    'Abp.Localization.CultureName',
                    this.selectedLanguage?.name,
                    new Date(new Date().getTime() + 5 * 365 * 86400000), //5 year
                    abp.appPath
                );
                window.location.reload();
            }
            this.isLoading = false;
        }))
        .subscribe(() => {
            this.notify.success(this.l('SavedSuccessfully'));
        });
    }

    onChangeLanguage(event){
        this.myProfile.language = event.value.name;
        this.changeLanguage(event.value.name);
    }

    changeLanguage(languageName: string): void {
        const input = new ChangeUserLanguageDto();
        input.languageName = languageName;

        this._profileServiceProxy.changeLanguage(input).subscribe(() => {
            abp.utils.setCookieValue(
                'Abp.Localization.CultureName',
                languageName,
                new Date(new Date().getTime() + 5 * 365 * 86400000), //5 year
                abp.appPath
            );

            window.location.reload();
        });
    }
}
