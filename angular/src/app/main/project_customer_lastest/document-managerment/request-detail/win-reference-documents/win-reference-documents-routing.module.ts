import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { WinReferenceDocumentsComponent } from './win-reference-documents.component';

@NgModule({
    imports: [
        RouterModule.forChild([
            {
                path: '',
                component: WinReferenceDocumentsComponent
            },
        ]),
    ],
    exports: [RouterModule],
})
export class WinReferenceDocumentsRoutingModule { }
