import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { DocumentManagermentComponent } from './document-managerment.component';

@NgModule({
    imports: [
        RouterModule.forChild([
            {
                path: '',
                component: DocumentManagermentComponent
            },
        ]),
    ],
    exports: [RouterModule],
})
export class DocumentManagermentRoutingModule { }
