import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import { ColorComponent } from './color.component';

const routes: Routes = [{
    path: '',
    component: ColorComponent,
    pathMatch: 'full'
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class ColorRoutingModule {}
