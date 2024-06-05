import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { PreviewComponent } from './preview.component';

@NgModule({
    imports: [
        RouterModule.forChild([
            {
                path: '',
                component: PreviewComponent
            },
        ]),
    ],
    exports: [RouterModule],
})
export class PreviewRoutingModule { }
