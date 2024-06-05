import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { WinReferenceDocumentsComponent } from './win-reference-documents.component';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { WinReferenceDocumentsRoutingModule } from './win-reference-documents-routing.module';

@NgModule({
    imports: [
        CommonModule,
        AppSharedModule,
        AdminSharedModule,
        WinReferenceDocumentsRoutingModule,
    ],
    declarations: [WinReferenceDocumentsComponent]
})
export class WinReferenceDocumentsModule { }
