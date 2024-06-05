import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DashboardComponent } from './dashboard.component';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { DashboardRoutingModule } from './dashboard-routing.module';

@NgModule({
    imports: [
        CommonModule,
        AppSharedModule,
        AdminSharedModule,
        DashboardRoutingModule,
    ],
    declarations: [DashboardComponent]
})
export class DashboardModule { }
