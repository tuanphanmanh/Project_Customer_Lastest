import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import { ConfigEmailTemplateComponent } from './config-email-template.component';


const routes: Routes = [{
    path: '',
    component: ConfigEmailTemplateComponent,
    pathMatch: 'full'
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class ConfigEmailTemplateRoutingModule {}
