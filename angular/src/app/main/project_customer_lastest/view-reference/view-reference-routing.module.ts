import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { ViewReferenceComponent } from './view-reference.component';

@NgModule({
    imports: [
        RouterModule.forChild([
            {
                path: '',
                component: ViewReferenceComponent
            },
        ]),
    ],
    exports: [RouterModule],
})
export class ViewReferenceRoutingModule { }
