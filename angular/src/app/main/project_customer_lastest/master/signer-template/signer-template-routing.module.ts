import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import { SignerTemplateComponent } from './signer-template.component';


const routes: Routes = [{
    path: '',
    component: SignerTemplateComponent,
    pathMatch: 'full'
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class SignerTemplateRoutingModule {}
