import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { AddSignatureComponent } from './add-signature.component';

@NgModule({
    imports: [
        RouterModule.forChild([
            {
                path: '',
                component: AddSignatureComponent
            },
        ]),
    ],
    exports: [RouterModule],
})
export class AddSignatureRoutingModule { }
