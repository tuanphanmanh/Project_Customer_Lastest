import { NgModule } from '@angular/core';
import { SignerTemplateComponent } from './signer-template.component';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { UtilsModule } from '@shared/utils/utils.module';
import { SignerTemplateRoutingModule } from './signer-template-routing.module';

@NgModule({
    imports: [
        AdminSharedModule,
        UtilsModule,
        SignerTemplateRoutingModule
    ],
    declarations: [SignerTemplateComponent]
})
export class SignerTemplateModule { }
