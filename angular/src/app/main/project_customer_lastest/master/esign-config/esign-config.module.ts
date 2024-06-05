import { NgModule } from '@angular/core';
import { EsignConfigComponent } from './esign-config.component';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { UtilsModule } from '@shared/utils/utils.module';
import { EsignConfigRoutingModule } from './esign-config-routing.module';
import { CreateOrEditEsignConfigComponent } from './create-or-edit-esign-config-modal.component';

@NgModule({
    imports: [
        AdminSharedModule,
        UtilsModule,
        EsignConfigRoutingModule
    ],
    declarations: [
        EsignConfigComponent,
        CreateOrEditEsignConfigComponent
    ]
})
export class EsignConfigModule { }
