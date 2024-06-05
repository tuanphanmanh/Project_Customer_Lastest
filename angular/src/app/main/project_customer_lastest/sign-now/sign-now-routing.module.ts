import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { SignNowComponent } from './sign-now.component';

@NgModule({
    imports: [
        RouterModule.forChild([
            {
                path: '',
                component: SignNowComponent
            },
        ]),
    ],
    exports: [RouterModule],
})
export class SignNowRoutingModule { }
