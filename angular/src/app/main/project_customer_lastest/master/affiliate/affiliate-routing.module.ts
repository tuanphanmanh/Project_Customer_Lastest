import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import { AffiliateComponent } from './affiliate.component';

const routes: Routes = [{
    path: '',
    component: AffiliateComponent,
    pathMatch: 'full'
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class AffiliateRoutingModule {}
