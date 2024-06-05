import { NgModule } from '@angular/core';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { UtilsModule } from '@shared/utils/utils.module';
import { LogoConfigRoutingModule } from './logo-config-routing.module';
import { LogoConfigComponent } from './logo-config.component';
import { CreateOrEditLogoComponent } from './create-or-edit-logo-modal.component';

@NgModule({
  imports: [
        AdminSharedModule,
        UtilsModule,
        LogoConfigRoutingModule
  ],
  declarations: [
    LogoConfigComponent,
    CreateOrEditLogoComponent
  ]
})
export class LogoConfigModule { }
