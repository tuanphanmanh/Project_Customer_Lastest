import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AccountSettingComponent } from './account-setting.component';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { AccountSettingRoutingModule } from './account-setting-routing.module';

@NgModule({
    imports: [
        CommonModule,
        AppSharedModule,
        AdminSharedModule,
        AccountSettingRoutingModule
    ],
    declarations: [AccountSettingComponent]
})
export class AccountSettingModule { }
