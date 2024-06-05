import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { ViewDetailComponent } from './view-detail.component';

@NgModule({
    imports: [
        RouterModule.forChild([
            {
                path: '',
                component: ViewDetailComponent
            },
        ]),
    ],
    exports: [RouterModule],
})
export class ViewDetailRoutingModule { }
