import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import { ActiveDirectoryComponent } from './active-directory.component';

const routes: Routes = [{
    path: '',
    component: ActiveDirectoryComponent,
    pathMatch: 'full'
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class ActiveDirectoryRoutingModule {}
