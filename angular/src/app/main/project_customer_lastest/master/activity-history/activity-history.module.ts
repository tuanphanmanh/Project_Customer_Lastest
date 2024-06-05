import { NgModule } from '@angular/core';
import { ActivityHistoryComponent } from './activity-history.component';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { UtilsModule } from '@shared/utils/utils.module';
import { CreateOrEditActivityHistoryComponent } from './create-or-edit-activity-history-modal.component';
import { ActivityHistoryRoutingModule } from './activity-history-routing.module';

@NgModule({
  imports: [
        AdminSharedModule,
        UtilsModule,
        ActivityHistoryRoutingModule
  ],
  declarations: [
    ActivityHistoryComponent,
    CreateOrEditActivityHistoryComponent
  ]
})
export class ActivityHistoryModule { }
