import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PreviewComponent } from './preview.component';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { PreviewRoutingModule } from './preview-routing.module';
import { EsignModule } from '../esign-modal/esign.module';

@NgModule({
  imports: [
    CommonModule,
    AppSharedModule,
    AdminSharedModule,
    PreviewRoutingModule,
    EsignModule
  ],
  declarations: [PreviewComponent]
})
export class PreviewModule { }
