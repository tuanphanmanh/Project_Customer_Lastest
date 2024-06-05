import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import { StatusComponent } from './status.component';

const routes: Routes = [{
    path: '',
    component: StatusComponent,
    pathMatch: 'full'
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class StatusRoutingModule {}
