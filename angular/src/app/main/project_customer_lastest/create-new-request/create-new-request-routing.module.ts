import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { CreateNewRequestComponent } from './create-new-request.component';

@NgModule({
    imports: [
        RouterModule.forChild([
            {
                path: '',
                component: CreateNewRequestComponent
            },
        ]),
    ],
    exports: [RouterModule],
})
export class CreateNewRequestRoutingModule { }
