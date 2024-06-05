import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ViewDetailComponent } from './view-detail.component';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { EsignModule } from '../esign-modal/esign.module';
import { ViewDetailRoutingModule } from './view-detail-routing.module';

@NgModule({
    imports: [
        CommonModule,
        AppSharedModule,
        AdminSharedModule,
        ViewDetailRoutingModule,
        EsignModule
    ],
    declarations: [ViewDetailComponent]
})
export class ViewDetailModule { }
