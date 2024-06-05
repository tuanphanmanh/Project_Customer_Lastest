import { NO_ERRORS_SCHEMA, NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AddSignatureComponent } from './add-signature.component';
import { AddSignatureRoutingModule } from './add-signature-routing.module';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { CdkDrag, CdkDropList } from '@angular/cdk/drag-drop';
import { EsignModule } from '../esign-modal/esign.module';
import { ContextMenuModule } from '@syncfusion/ej2-angular-navigations';
import { SignNowModule } from '../sign-now/sign-now.module';

@NgModule({
  imports: [
    CommonModule,
    AddSignatureRoutingModule,
    AppSharedModule,
    AdminSharedModule,
    CdkDropList,
    CdkDrag,
    EsignModule,
    ContextMenuModule,
    SignNowModule,
  ],
  declarations: [AddSignatureComponent],

})
export class TransferModule { }
