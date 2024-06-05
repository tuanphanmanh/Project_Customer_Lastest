import { TwoFactModel, VerifyOTPInput, Page } from './../../shared/service-proxies/service-proxies';
import { TokenService, LogService, MessageService, LocalizationService } from 'abp-ng2-module';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { AppConsts } from '@shared/AppConsts';
import { UrlHelper } from '@shared/helpers/UrlHelper';
import {
    AuthenticateModel,
    AuthenticateResultModel,
    // ExternalAuthenticateModel,
    // ExternalAuthenticateResultModel,
    // ExternalLoginProviderInfoModel,
    TokenAuthServiceProxy,
    TwitterServiceProxy,
} from '@shared/service-proxies/service-proxies';
import { ScriptLoaderService } from '@shared/utils/script-loader.service';
import { map as _map, filter as _filter } from 'lodash-es';
import { finalize } from 'rxjs/operators';
import { OAuthService, AuthConfig } from 'angular-oauth2-oidc';
import * as AuthenticationContext from 'adal-angular/lib/adal';
import { NgxSpinnerService } from 'ngx-spinner';
import { LocalStorageService } from '@shared/utils/local-storage.service';
import { AuthenticationResult, PublicClientApplication } from '@azure/msal-browser';

declare const FB: any; // Facebook API
declare const gapi: any; // google API
declare const google: any; // google

// export class ExternalLoginProvider extends ExternalLoginProviderInfoModel {
//     static readonly FACEBOOK: string = 'Facebook';
//     static readonly GOOGLE: string = 'Google';
//     static readonly MICROSOFT: string = 'Microsoft';
//     static readonly OPENID: string = 'OpenIdConnect';
//     static readonly WSFEDERATION: string = 'WsFederation';
//     static readonly TWITTER: string = 'Twitter';

//     icon: string;
//     initialized = false;

//     constructor(providerInfo: ExternalLoginProviderInfoModel) {
//         super();

//         this.name = providerInfo.name;
//         this.clientId = providerInfo.clientId;
//         this.additionalParams = providerInfo.additionalParams;
//         this.icon = providerInfo.name.toLowerCase();
//     }
// }

@Injectable()
export class LoginService  {
    static readonly twoFactorRememberClientTokenName = 'TwoFactorRememberClientToken';

    authenticateModel: AuthenticateModel;
    authenticateResult: AuthenticateResultModel;
    // externalLoginProviders: ExternalLoginProvider[] = [];
    rememberMe: boolean;
    wsFederationAuthenticationContext: any;
    localizationSourceName = AppConsts.localization.defaultLocalizationSourceName;

    localization: LocalizationService;
    private MSAL: PublicClientApplication;
    private authWindow: Window | null = null;
    //Old Azure AD
    private config = {
      tenantId: '7f5f18eb-e49c-42fc-a52f-2731d1674ab1',
      clientId: '701efa1a-6997-442e-b8ea-3f87cb370c88',
      redirectUri: 'https://esign-standby.toyotavn.com.vn/callback',
      scope: 'openid profile email',
    };
    //New Azure Ad
    // private config = {
    //     tenantId: '7f5f18eb-e49c-42fc-a52f-2731d1674ab1',
    //     clientId: 'ba8c4325-3de3-4228-a2ff-dbf0bf9e2862',
    //     redirectUri: 'https://esign-standby.toyotavn.com.vn:8888/callback',
    //     scope: 'openid profile email',
    //   };
    constructor(
        private _tokenAuthService: TokenAuthServiceProxy,
        private _router: Router,
        private _messageService: MessageService,
        private _tokenService: TokenService,
        private _logService: LogService,
        private oauthService: OAuthService,
        private spinnerService: NgxSpinnerService,
        private _localStorageService: LocalStorageService,
        private _twitterService: TwitterServiceProxy
    ) {
        this.clear();
    }

    authenticate(finallyCallback?: () => void, redirectUrl?: string, captchaResponse?: string): void {
        finallyCallback =
            finallyCallback ||
            (() => {
                this.spinnerService.hide();
            });
            finallyCallback();
        const self = this;
        this._localStorageService.getItem(LoginService.twoFactorRememberClientTokenName, function (err, value) {
            self._tokenAuthService.loginGuest(self.authenticateModel).subscribe({
                next: (result: AuthenticateResultModel) => {
                    self.processAuthenticateResult(result, redirectUrl);
                    finallyCallback();
                },
                error: (err: any) => {
                    finallyCallback();
                },
            });
        });
    }
    authenticate365(finallyCallback?: () => void, redirectUrl?: string, accessCode?: string): void {
        finallyCallback =
            finallyCallback ||
            (() => {
                this.spinnerService.hide();
            });
            finallyCallback();
        const self = this;
        self.authenticateModel.userName = '*********';
        self.authenticateModel.password = '*********';
        self.authenticateModel.accessCode = accessCode;
        this._localStorageService.getItem(LoginService.twoFactorRememberClientTokenName, function (err, value) {
            self._tokenAuthService.login365(self.authenticateModel).subscribe({
                next: (result: AuthenticateResultModel) => {
                    self.processAuthenticateResult(result, redirectUrl);
                    self.authenticateModel.userName = '';
                    self.authenticateModel.password = '';
                    finallyCallback();
                },
                error: (err: any) => {
                    self.authenticateModel.userName = '';
                    self.authenticateModel.password = '';
                    finallyCallback();
                },
            });
        });
    }

    verifyOtp({ finallyCallback, input }: { finallyCallback?: () => void; input: VerifyOTPInput; }): void {
        finallyCallback =
            finallyCallback ||
            (() => {
                this.spinnerService.hide();
            });

        const self = this;

        self._tokenAuthService.verifyOTP(input).subscribe({
            next: (result: AuthenticateResultModel) => {
                self.processAuthenticateResult(result);
                finallyCallback();
            },
            error: (err: any) => {
                finallyCallback();
            },
        });
    }

    // ensureExternalLoginProviderInitialized(loginProvider: ExternalLoginProvider, callback: () => void) {
    //     if (loginProvider.initialized) {
    //         callback();
    //         return;
    //     }

    //     if (loginProvider.name === ExternalLoginProvider.FACEBOOK) {
    //         new ScriptLoaderService().load('//connect.facebook.net/en_US/sdk.js').then(() => {
    //             FB.init({
    //                 appId: loginProvider.clientId,
    //                 cookie: false,
    //                 xfbml: true,
    //                 version: 'v2.5',
    //             });

    //             FB.getLoginStatus((response) => {
    //                 this.facebookLoginStatusChangeCallback(response);
    //                 if (response.status !== 'connected') {
    //                     callback();
    //                 }
    //             });
    //         });
    //     } else if (loginProvider.name === ExternalLoginProvider.GOOGLE) {
    //         new ScriptLoaderService()
    //             .load(
    //                 'https://apis.google.com/js/api.js',
    //                 'https://accounts.google.com/gsi/client'
    //             )
    //             .then(() => {
    //                 gapi.load('client', () => {
    //                     gapi.client
    //                         .init({})
    //                         .then(() => {
    //                             gapi.client.load('oauth2', 'v2', ()=>{
    //                                 callback();
    //                             });
    //                         });
    //                 });
    //             });
    //     } else if (loginProvider.name === ExternalLoginProvider.MICROSOFT) {
    //         this.MSAL = new PublicClientApplication({
    //             auth: {
    //                 clientId: loginProvider.clientId,
    //                 redirectUri: AppConsts.appBaseUrl,
    //             },
    //         });
    //         callback();
    //     } else if (loginProvider.name === ExternalLoginProvider.OPENID) {
    //         const authConfig = this.getOpenIdConnectConfig(loginProvider);
    //         this.oauthService.configure(authConfig);
    //         this.oauthService.initLoginFlow('openIdConnect=1');
    //     } else if (loginProvider.name === ExternalLoginProvider.WSFEDERATION) {
    //         let config = this.getWsFederationConnectConfig(loginProvider);
    //         this.wsFederationAuthenticationContext = new AuthenticationContext(config);
    //         this.wsFederationAuthenticationContext.login();
    //     } else if (loginProvider.name === ExternalLoginProvider.TWITTER) {
    //         callback();
    //     }
    // }

    // public twitterLoginCallback(token: string, verifier: string) {
    //     this.spinnerService.show();

    //     this._twitterService.getAccessToken(token, verifier).subscribe((response) => {
    //         const model = new ExternalAuthenticateModel();
    //         model.authProvider = ExternalLoginProvider.TWITTER;
    //         model.providerAccessCode = response.accessToken + '&' + response.accessTokenSecret;
    //         model.providerKey = response.userId;
    //         model.singleSignIn = UrlHelper.getSingleSignIn();
    //         model.returnUrl = UrlHelper.getReturnUrl();

    //         this._tokenAuthService
    //             .externalAuthenticate(model)
    //             .pipe(
    //                 finalize(() => {
    //                     this.spinnerService.hide();
    //                 })
    //             )
    //             .subscribe((result: ExternalAuthenticateResultModel) => {
    //                 if (result.waitingForActivation) {
    //                     this._messageService.info('You have successfully registered. Waiting for activation!');
    //                     return;
    //                 }

    //                 this.login(
    //                     result.accessToken,
    //                     result.encryptedAccessToken,
    //                     result.expireInSeconds,
    //                     result.refreshToken,
    //                     result.refreshTokenExpireInSeconds,
    //                     false,
    //                     '',
    //                     result.returnUrl
    //                 );
    //             });
    //     });
    // }

    // public wsFederationLoginStatusChangeCallback(errorDesc, token, error, tokenType) {
    //     let user = this.wsFederationAuthenticationContext.getCachedUser();

    //     const model = new ExternalAuthenticateModel();
    //     model.authProvider = ExternalLoginProvider.WSFEDERATION;
    //     model.providerAccessCode = token;
    //     model.providerKey = user.profile.sub;
    //     model.singleSignIn = UrlHelper.getSingleSignIn();
    //     model.returnUrl = UrlHelper.getReturnUrl();

    //     this._tokenAuthService.externalAuthenticate(model).subscribe((result: ExternalAuthenticateResultModel) => {
    //         if (result.waitingForActivation) {
    //             this._messageService.info('You have successfully registered. Waiting for activation!');
    //             this._router.navigate(['account/login']);
    //             return;
    //         }

    //         this.login(
    //             result.accessToken,
    //             result.encryptedAccessToken,
    //             result.expireInSeconds,
    //             result.refreshToken,
    //             result.refreshTokenExpireInSeconds,
    //             false,
    //             '',
    //             result.returnUrl
    //         );
    //     });
    // }

    // public openIdConnectLoginCallback(resp) {
    //     this.initExternalLoginProviders(() => {
    //         let openIdProvider = _filter(this.externalLoginProviders, {
    //             name: 'OpenIdConnect',
    //         })[0];
    //         let authConfig = this.getOpenIdConnectConfig(openIdProvider);

    //         this.oauthService.configure(authConfig);
    //         this.spinnerService.show();

    //         this.oauthService.loadDiscoveryDocumentAndTryLogin().then(() => {
    //             let claims = this.oauthService.getIdentityClaims();

    //             const model = new ExternalAuthenticateModel();
    //             model.authProvider = ExternalLoginProvider.OPENID;
    //             model.providerAccessCode = this.oauthService.getIdToken();
    //             model.providerKey = claims['sub'];
    //             model.singleSignIn = UrlHelper.getSingleSignIn();
    //             model.returnUrl = UrlHelper.getReturnUrl();

    //             this._tokenAuthService
    //                 .externalAuthenticate(model)
    //                 .pipe(
    //                     finalize(() => {
    //                         this.spinnerService.hide();
    //                     })
    //                 )
    //                 .subscribe((result: ExternalAuthenticateResultModel) => {
    //                     if (result.waitingForActivation) {
    //                         this._messageService.info('You have successfully registered. Waiting for activation!');
    //                         return;
    //                     }

    //                     this.login(
    //                         result.accessToken,
    //                         result.encryptedAccessToken,
    //                         result.expireInSeconds,
    //                         result.refreshToken,
    //                         result.refreshTokenExpireInSeconds,
    //                         false,
    //                         '',
    //                         result.returnUrl
    //                     );
    //                 });
    //         });
    //     });
    // }

    // externalAuthenticate(provider: ExternalLoginProvider): void {
    //     this.ensureExternalLoginProviderInitialized(provider, () => {
    //         if (provider.name === ExternalLoginProvider.FACEBOOK) {
    //             FB.login(
    //                 (response) => {
    //                     this.facebookLoginStatusChangeCallback(response);
    //                 },
    //                 { scope: 'email' }
    //             );
    //         } else if (provider.name === ExternalLoginProvider.GOOGLE) {
    //             let gisGoogleTokenClient = google.accounts.oauth2.initTokenClient({
    //                 client_id: provider.clientId,
    //                 scope: 'openid profile email',
    //                 callback: (resp) => {
    //                     if (resp.error !== undefined) {
    //                       throw(resp);
    //                     }

    //                     // GIS has automatically updated gapi.client with the newly issued access token
    //                     this.googleLoginStatusChangeCallback(resp);
    //                   }
    //             });

    //             // Conditionally ask users to select the Google Account they'd like to use,
    //             // and explicitly obtain their consent to fetch their Calendar.
    //             // NOTE: To request an access token a user gesture is necessary.
    //             if (gapi.client.getToken() === null) {
    //                 // Prompt the user to select an Google Account and asked for consent to share their data
    //                 // when establishing a new session.
    //                 gisGoogleTokenClient.requestAccessToken({prompt: 'consent'});
    //             } else {
    //                 // Skip display of account chooser and consent dialog for an existing session.
    //                 gisGoogleTokenClient.requestAccessToken({prompt: ''});
    //             }

    //         } else if (provider.name === ExternalLoginProvider.MICROSOFT) {
    //             let scopes = ['user.read'];
    //             this.spinnerService.show();
    //             this.MSAL.loginPopup({
    //                 scopes: scopes,
    //             }).then((idTokenResponse: AuthenticationResult) => {
    //                 this.MSAL.acquireTokenSilent({
    //                     account: this.MSAL.getAllAccounts()[0],
    //                     scopes: scopes
    //                 }).then((accessTokenResponse: AuthenticationResult) => {
    //                     this.microsoftLoginCallback(accessTokenResponse);
    //                     this.spinnerService.hide();
    //                 }).catch((error) => {
    //                     abp.log.error(error);
    //                     abp.message.error(
    //                         this.localization.localize('CouldNotValidateExternalUser', this.localizationSourceName)
    //                     );
    //                 });
    //             });
    //         } else if (provider.name === ExternalLoginProvider.TWITTER) {
    //             this.startTwitterLogin();
    //         }
    //     });
    // }

    init(): void {
        // this.initExternalLoginProviders();
    }

    private processAuthenticateResult(authenticateResult: AuthenticateResultModel, redirectUrl?: string) {
        this.authenticateResult = authenticateResult;

        if (authenticateResult.shouldResetPassword) {
            // Password reset

            this._router.navigate(['account/reset-password'], {
                queryParams: {
                    tenantId: abp.session.tenantId,
                    inputCode: authenticateResult.tokenTemp,
                },
            });

            this.clear();
        }
        else if(authenticateResult.reqVerifyCode){
            this._router.navigate(['account/send-code']);
        }

        else if (authenticateResult.accessToken) {
            // Successfully logged in

            this.login(
                authenticateResult.accessToken,
                authenticateResult.encryptedAccessToken,
                authenticateResult.expireInSeconds,
                authenticateResult.refreshToken,
                authenticateResult.refreshTokenExpireInSeconds,
                this.rememberMe,
                redirectUrl
            );
        } else {
            // Unexpected result!

            this._logService.warn('Unexpected authenticateResult!');
            this._router.navigate(['account/login']);
        }
    }

    private login(
        accessToken: string,
        encryptedAccessToken: string,
        expireInSeconds: number,
        refreshToken: string,
        refreshTokenExpireInSeconds: number,
        rememberMe?: boolean,
        twoFactorRememberClientToken?: string,
        redirectUrl?: string
    ): void {
        let tokenExpireDate = rememberMe ? new Date(new Date().getTime() + 1000 * expireInSeconds) : undefined;

        this._tokenService.setToken(accessToken, tokenExpireDate);

        if (refreshToken && rememberMe) {
            let refreshTokenExpireDate = rememberMe
                ? new Date(new Date().getTime() + 1000 * refreshTokenExpireInSeconds)
                : undefined;
            this._tokenService.setRefreshToken(refreshToken, refreshTokenExpireDate);
        }

        let self = this;
        this._localStorageService.setItem(
            AppConsts.authorization.encrptedAuthTokenName,
            {
                token: encryptedAccessToken,
                expireDate: tokenExpireDate,
            },
            () => {
                if (twoFactorRememberClientToken) {
                    self._localStorageService.setItem(
                        LoginService.twoFactorRememberClientTokenName,
                        {
                            token: twoFactorRememberClientToken,
                            expireDate: new Date(new Date().getTime() + 365 * 86400000), // 1 year
                        },
                        () => {
                            self.redirectToLoginResult(redirectUrl);
                        }
                    );
                } else {
                    self.redirectToLoginResult(redirectUrl);
                }
            }
        );
    }

    private redirectToLoginResult(redirectUrl?: string): void {
        if (redirectUrl) {
            location.href = redirectUrl;
        } else {
            let initialUrl = UrlHelper.initialUrl;

            if (initialUrl.indexOf('/account') > 0) {
                initialUrl = AppConsts.appBaseUrl;
            }

            location.href = initialUrl;
        }
    }

    private clear(): void {
        this.authenticateModel = new AuthenticateModel();
        this.authenticateModel.tenancyName = AppConsts.tenancyName;
        this.authenticateResult = null;
        this.rememberMe = false;
    }

    // private initExternalLoginProviders(callback?: any) {
    //     this._tokenAuthService
    //         .getExternalAuthenticationProviders()
    //         .subscribe((providers: ExternalLoginProviderInfoModel[]) => {
    //             this.externalLoginProviders = _map(providers, (p) => new ExternalLoginProvider(p));
    //             if (callback) {
    //                 callback();
    //             }
    //         });
    // }

    // private getWsFederationConnectConfig(loginProvider: ExternalLoginProvider): any {
    //     let config = {
    //         clientId: loginProvider.clientId,
    //         popUp: true,
    //         callback: this.wsFederationLoginStatusChangeCallback.bind(this),
    //     } as any;

    //     if (loginProvider.additionalParams['Tenant']) {
    //         config.tenant = loginProvider.additionalParams['Tenant'];
    //     }

    //     return config;
    // }

    // private getOpenIdConnectConfig(loginProvider: ExternalLoginProvider): AuthConfig {
    //     let authConfig = new AuthConfig();
    //     authConfig.loginUrl = loginProvider.additionalParams['LoginUrl'];
    //     authConfig.issuer = loginProvider.additionalParams['Authority'];
    //     authConfig.skipIssuerCheck = loginProvider.additionalParams['ValidateIssuer'] === 'false';
    //     authConfig.clientId = loginProvider.clientId;
    //     authConfig.responseType = loginProvider.additionalParams['ResponseType'];
    //     authConfig.strictDiscoveryDocumentValidation = false;
    //     authConfig.redirectUri = window.location.origin + '/account/login';
    //     authConfig.scope = 'openid profile';
    //     authConfig.requestAccessToken = false;
    //     return authConfig;
    // }

    // private facebookLoginStatusChangeCallback(resp) {
    //     if (resp.status === 'connected') {
    //         const model = new ExternalAuthenticateModel();
    //         model.authProvider = ExternalLoginProvider.FACEBOOK;
    //         model.providerAccessCode = resp.authResponse.accessToken;
    //         model.providerKey = resp.authResponse.userID;
    //         model.singleSignIn = UrlHelper.getSingleSignIn();
    //         model.returnUrl = UrlHelper.getReturnUrl();

    //         this._tokenAuthService.externalAuthenticate(model).subscribe((result: ExternalAuthenticateResultModel) => {
    //             if (result.waitingForActivation) {
    //                 this._messageService.info('You have successfully registered. Waiting for activation!');
    //                 return;
    //             }

    //             this.login(
    //                 result.accessToken,
    //                 result.encryptedAccessToken,
    //                 result.expireInSeconds,
    //                 result.refreshToken,
    //                 result.refreshTokenExpireInSeconds,
    //                 false,
    //                 '',
    //                 result.returnUrl
    //             );
    //         });
    //     }
    // }

    private startTwitterLogin() {
        this.spinnerService.show();
        this._twitterService
            .getRequestToken()
            .pipe(finalize(() => this.spinnerService.hide()))
            .subscribe((result) => {
                if (result.confirmed) {
                    window.location.href = result.redirectUrl;
                } else {
                    this._messageService.error('Couldn\'t get twitter request token !');
                }
            });
    }

    // private googleLoginStatusChangeCallback(response) {
    //     if (response.error !== undefined) {
    //         return;
    //     }
    //     var _$this=this;

    //     gapi.client.oauth2.userinfo.get().execute(function(resp) {
    //         const model = new ExternalAuthenticateModel();
    //         model.authProvider = ExternalLoginProvider.GOOGLE;
    //         model.providerAccessCode = response.access_token;
    //         model.providerKey = resp.id;
    //         model.singleSignIn = UrlHelper.getSingleSignIn();
    //         model.returnUrl = UrlHelper.getReturnUrl();

    //         _$this._tokenAuthService.externalAuthenticate(model).subscribe((result: ExternalAuthenticateResultModel) => {
    //             if (result.waitingForActivation) {
    //                 _$this._messageService.info('You have successfully registered. Waiting for activation!');
    //                 return;
    //             }

    //             _$this.login(
    //                 result.accessToken,
    //                 result.encryptedAccessToken,
    //                 result.expireInSeconds,
    //                 result.refreshToken,
    //                 result.refreshTokenExpireInSeconds,
    //                 false,
    //                 '',
    //                 result.returnUrl
    //             );
    //         });
    //       })
    // }

    // private microsoftLoginCallback(response: AuthenticationResult) {
    //     const model = new ExternalAuthenticateModel();
    //     model.authProvider = ExternalLoginProvider.MICROSOFT;
    //     model.providerAccessCode = response.accessToken;
    //     model.providerKey = response.uniqueId;
    //     model.singleSignIn = UrlHelper.getSingleSignIn();
    //     model.returnUrl = UrlHelper.getReturnUrl();

    //     this.spinnerService.show();

    //     this._tokenAuthService.externalAuthenticate(model).subscribe((result: ExternalAuthenticateResultModel) => {
    //         if (result.waitingForActivation) {
    //             this._messageService.info('You have successfully registered. Waiting for activation!');
    //             return;
    //         }

    //         this.login(
    //             result.accessToken,
    //             result.encryptedAccessToken,
    //             result.expireInSeconds,
    //             result.refreshToken,
    //             result.refreshTokenExpireInSeconds,
    //             false,
    //             '',
    //             result.returnUrl
    //         );
    //         this.spinnerService.hide();
    //     });
    // }
    openAuthWindow(finallyCallback?: () => void): Promise<string | null> {
        return new Promise<string | null>((resolve, reject) => {
            // Generate the URL for the Microsoft login page
            //const authUrl = `https://login.microsoftonline.com/${this.config.tenantId}/oauth2/v2.0/authorize?client_id=${this.config.clientId}&response_type=code&redirect_uri=${this.config.redirectUri}&response_mode=query&scope=${this.config.scope}`;
            const authUrl = `https://login.microsoftonline.com/${AppConsts.tenantId}/oauth2/v2.0/authorize?client_id=${AppConsts.clientId}&response_type=code&redirect_uri=${AppConsts.redirectUri}&response_mode=query&scope=${AppConsts.scope}`;
            
            // Open a pop-up window
            const width = 600;
            const height = 600;
            const left = window.innerWidth / 2 - width / 2;
            const top = window.innerHeight / 2 - height / 2;
            const options = `width=${width}, height=${height}, left=${left}, top=${top}`;

            this.authWindow = window.open(authUrl, '_blank', options);

            // Listen for changes in the pop-up window's location
            const pollAuthWindow = setInterval(() => {
                try {
                    //if (this.authWindow && this.authWindow.location.href.startsWith(this.config.redirectUri)) {
                    if (this.authWindow && this.authWindow.location.href.startsWith(AppConsts.redirectUri)) {
                        clearInterval(pollAuthWindow);
                        // Handle the response from the authentication
                        const accessCode = this.handleAuthResponse(this.authWindow.location.search);

                        // Close the pop-up window
                        this.authWindow.close();
                        this.authenticate365(
                            () => {

                            },
                            null,
                            accessCode
                        );
                        // Resolve the promise with the access code
                        finallyCallback();
                    }
                } catch (error) {
                    // Handle any errors or cancel button clicks
                    finallyCallback();
                }
            }, 1000);
        });
    }

    handleAuthResponse(queryParameters: string): string | null {
        const params = new URLSearchParams(queryParameters);
        const accessCode = params.get('code');
        if (accessCode) {
            return accessCode;
        } else {
            return null;
        }
    }


}
