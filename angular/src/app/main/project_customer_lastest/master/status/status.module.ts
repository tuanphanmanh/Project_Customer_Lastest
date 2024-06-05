import { NgModule } from '@angular/core';
import { StatusComponent } from './status.component';
import { StatusRoutingModule } from './status-routing.module';
import { UtilsModule } from '@shared/utils/utils.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { CreateOrEditStatusComponent } from './create-or-edit-status-modal.component';

@NgModule({
    imports: [
        AdminSharedModule,
        UtilsModule,
        StatusRoutingModule
    ],
    declarations: [StatusComponent, CreateOrEditStatusComponent]
})
export class StatusModule { }
