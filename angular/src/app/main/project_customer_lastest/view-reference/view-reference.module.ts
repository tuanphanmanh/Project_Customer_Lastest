import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ViewReferenceComponent } from './view-reference.component';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { EsignModule } from '../esign-modal/esign.module';
import { ViewReferenceRoutingModule } from './view-reference-routing.module';

@NgModule({
    imports: [
        CommonModule,
        AppSharedModule,
        AdminSharedModule,
        ViewReferenceRoutingModule,
        EsignModule
    ],
    declarations: [ViewReferenceComponent]
})
export class ViewReferenceModule { }
