import { NgModule } from '@angular/core';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { UtilsModule } from '@shared/utils/utils.module';
import { ActiveReportsComponent } from './active-reports.component';
import { ActiveReportsRoutingModule } from './active-reports-routing.module';

@NgModule({
  imports: [
        AdminSharedModule,
        UtilsModule,
        ActiveReportsRoutingModule
  ],
  declarations: [ActiveReportsComponent]
})
export class ActiveReportsModule { }
