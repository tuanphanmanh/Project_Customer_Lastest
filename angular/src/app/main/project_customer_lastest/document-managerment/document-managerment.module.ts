import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DocumentManagermentComponent } from './document-managerment.component';
import { DocumentManagermentRoutingModule } from './document-managerment-routing.module';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { EsignModule } from '../esign-modal/esign.module';
import { SignatureModule, UploaderModule } from '@syncfusion/ej2-angular-inputs';

@NgModule({
    imports: [
        CommonModule,
        DocumentManagermentRoutingModule,
        AppSharedModule,
        AdminSharedModule,
        EsignModule, SignatureModule, UploaderModule
    ],
    declarations: [DocumentManagermentComponent, ]
})
export class DocumentManagermentModule { }
