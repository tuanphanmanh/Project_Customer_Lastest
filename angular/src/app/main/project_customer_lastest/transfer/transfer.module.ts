import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { TransferComponent } from './transfer.component';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { TransferRoutingModule } from './transfer-routing.module';
import { EsignModule } from '../esign-modal/esign.module';

@NgModule({
    imports: [
        CommonModule,
        AppSharedModule,
        AdminSharedModule,
        TransferRoutingModule,
        EsignModule
    ],
    declarations: [TransferComponent]
})
export class TransferModule { }
