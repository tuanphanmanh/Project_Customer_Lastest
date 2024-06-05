import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import { SystemsComponent } from './systems.component';

const routes: Routes = [{
    path: '',
    component: SystemsComponent,
    pathMatch: 'full'
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class SystemsRoutingModule {}
