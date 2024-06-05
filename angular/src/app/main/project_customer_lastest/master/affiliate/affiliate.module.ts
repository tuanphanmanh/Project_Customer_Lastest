import { NgModule } from '@angular/core';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { UtilsModule } from '@shared/utils/utils.module';
import { AffiliateRoutingModule } from './affiliate-routing.module';
import { AffiliateComponent } from './affiliate.component';
import { CreateOrEditAffiliateComponent } from './create-or-edit-affiliate-modal.component';

@NgModule({
    imports: [
        AdminSharedModule,
        UtilsModule,
        AffiliateRoutingModule
    ],
    declarations: [
        AffiliateComponent,
        CreateOrEditAffiliateComponent
    ]
})
export class AffiliateModule { }
