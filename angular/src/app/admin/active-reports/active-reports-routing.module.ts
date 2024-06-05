import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import { ActiveReportsComponent } from './active-reports.component';

const routes: Routes = [{
    path: '',
    component: ActiveReportsComponent,
    pathMatch: 'full'
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class ActiveReportsRoutingModule {}
