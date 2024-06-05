import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import { LogoConfigComponent } from './logo-config.component';

const routes: Routes = [{
    path: '',
    component: LogoConfigComponent,
    pathMatch: 'full'
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class LogoConfigRoutingModule {}
