import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import { EsignConfigComponent } from './esign-config.component';

const routes: Routes = [{
    path: '',
    component: EsignConfigComponent,
    pathMatch: 'full'
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class EsignConfigRoutingModule {}
