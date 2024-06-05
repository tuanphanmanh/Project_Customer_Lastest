import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LoginGuestComponent } from './login-guest.component';

const routes: Routes = [
    {
        path: '',
        component: LoginGuestComponent,
        pathMatch: 'full',
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class LoginGuestRoutingModule {}
