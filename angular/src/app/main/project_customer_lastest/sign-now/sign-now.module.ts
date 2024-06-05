import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SignNowComponent } from './sign-now.component';
import { SignNowRoutingModule } from './sign-now-routing.module';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { EsignModule } from '../esign-modal/esign.module';
import { SignatureModule, UploaderModule } from '@syncfusion/ej2-angular-inputs';
import { PopupSignatureComponent } from './popup-signature/popup-signature.component';


@NgModule({
    imports: [
        CommonModule,
        SignNowRoutingModule,
        AppSharedModule,
        AdminSharedModule,
        EsignModule, SignatureModule, UploaderModule
    ],
    declarations: [SignNowComponent, PopupSignatureComponent],
    exports:[PopupSignatureComponent],
})
export class SignNowModule { }
