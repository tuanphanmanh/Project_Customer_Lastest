import { enableProdMode } from '@angular/core';
import { platformBrowserDynamic } from '@angular/platform-browser-dynamic';
import { environment } from './environments/environment';
import { hmrBootstrap } from './hmr';
import './polyfills';
import { RootModule } from './root.module';
// import { LicenseManager } from "@ag-grid-enterprise/core";
// import "@ag-grid-enterprise/all-modules";
import {registerLicense} from '@syncfusion/ej2-base';
registerLicense("Ngo9BigBOggjHTQxAR8/V1NHaF5cXmVCf1FpRmJGdld5fUVHYVZUTXxaS00DNHVRdkdgWXZceXVTQmhdU0ZxXUs=");

// set ag-grid-enterprise license
// LicenseManager.setLicenseKey(
//     "[v2]-_TMV_TMSS_18_May_2033_MjAwMDAwMDAwMDAwMA==70837a4d6bc7393fe5e607db4c051b42"
// );

if (environment.production) {
    enableProdMode();
}

const bootstrap = () => platformBrowserDynamic().bootstrapModule(RootModule);
/* "Hot Module Replacement" is enabled as described on
 * https://medium.com/@beeman/tutorial-enable-hrm-in-angular-cli-apps-1b0d13b80130#.sa87zkloh
 */

if (environment.hmr) {
    if (module['hot']) {
        hmrBootstrap(module, bootstrap); //HMR enabled bootstrap
    } else {
        console.error('HMR is not enabled for webpack-dev-server!');
        console.log('Are you using the --hmr flag for ng serve?');
    }
} else {
    bootstrap(); //Regular bootstrap
}

