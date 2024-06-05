import { AccountSettingComponent } from './account-setting.component';
import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';

@NgModule({
    imports: [
        RouterModule.forChild([
            {
                path: '',
                component: AccountSettingComponent
            },
        ]),
    ],
    exports: [RouterModule],
})
export class AccountSettingRoutingModule { }
