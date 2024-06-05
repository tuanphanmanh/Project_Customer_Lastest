import { CreateNewRequestComponent } from './create-new-request.component';
import { CUSTOM_ELEMENTS_SCHEMA, NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { PdfViewerModule } from '@syncfusion/ej2-angular-pdfviewer';
import { SwitchModule } from '@syncfusion/ej2-angular-buttons';
import { CreateNewRequestRoutingModule } from './create-new-request-routing.module';
import { MainModule } from '@app/main/main.module';
import { AdminModule } from '@app/admin/admin.module';
import { DemoUIComponentsModule } from '@app/admin/demo-ui-components/demo-ui-components.module';
import {CdkDragDrop, CdkDropList, CdkDrag, moveItemInArray} from '@angular/cdk/drag-drop';
import { EsignModule } from '../esign-modal/esign.module';
import { FormsModule } from '@angular/forms';
import { CurrencyPipe } from '@angular/common';
import { CurrencyFormatModule } from '@shared/common/currency/currency-format.module';
import { SearchSignerComponent } from './search-signer/search-signer.component';

@NgModule({
    imports: [
        PdfViewerModule,
        SwitchModule,
        AppSharedModule,
        AdminSharedModule,
        CreateNewRequestRoutingModule,
        MainModule,
        AdminModule,
        DemoUIComponentsModule,
        CdkDropList,
        CdkDrag,
        EsignModule,
        FormsModule,
    ],
    declarations: [CreateNewRequestComponent, SearchSignerComponent],
    providers: [CurrencyPipe]
})
export class CreateNewRequestModule { }
