import { CUSTOM_ELEMENTS_SCHEMA, NgModule } from '@angular/core';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { UtilsModule } from '@shared/utils/utils.module';
import { CreateOrEditEmailTemplateComponent } from './create-or-edit-email-template-modal.component';
import { ConfigEmailTemplateComponent } from './config-email-template.component';
import { ConfigEmailTemplateRoutingModule } from './config-email-template-routing.module';

@NgModule({
    declarations: [
        ConfigEmailTemplateComponent,
        CreateOrEditEmailTemplateComponent
    ],
    imports: [
        AdminSharedModule,
        ConfigEmailTemplateRoutingModule,
        UtilsModule
    ],
    schemas: [ CUSTOM_ELEMENTS_SCHEMA ]
})
export class ConfigEmailTemplateModule { }




