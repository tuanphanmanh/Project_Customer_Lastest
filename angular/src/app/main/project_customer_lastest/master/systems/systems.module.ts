import { NgModule } from '@angular/core';
import { SystemsComponent } from './systems.component';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { UtilsModule } from '@shared/utils/utils.module';
import { SystemsRoutingModule } from './systems-routing.module';
import { CreateOrEditSystemsComponent } from './create-or-edit-systems-modal.component';

@NgModule({
  imports: [
        AdminSharedModule,
        UtilsModule,
        SystemsRoutingModule
  ],
  declarations: [
    SystemsComponent,
    CreateOrEditSystemsComponent
  ]
})
export class SystemsModule { }
