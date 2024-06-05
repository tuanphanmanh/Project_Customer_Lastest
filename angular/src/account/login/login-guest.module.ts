import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AccountSharedModule } from '@account/shared/account-shared.module';
import { LoginGuestComponent } from './login-guest.component';
import { LoginGuestRoutingModule } from './login-guest-routing.module';

@NgModule({
    declarations: [LoginGuestComponent],
    imports: [AppSharedModule, AccountSharedModule, LoginGuestRoutingModule],
})
export class LoginGuestModule {}
