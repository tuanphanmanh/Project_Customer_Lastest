import { NgModule } from '@angular/core';
import { ActiveDirectoryComponent } from './active-directory.component';
import { ActiveDirectoryRoutingModule } from './active-directory-routing.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { UtilsModule } from '@shared/utils/utils.module';

@NgModule({
  imports: [
        AdminSharedModule,
        UtilsModule,
        ActiveDirectoryRoutingModule
  ],
  declarations: [ActiveDirectoryComponent]
})
export class ActiveDirectoryModule { }
