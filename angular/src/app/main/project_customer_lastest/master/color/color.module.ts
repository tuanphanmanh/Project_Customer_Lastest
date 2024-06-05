import { NgModule } from '@angular/core';
import { ColorComponent } from './color.component';
import { ColorRoutingModule } from './color-routing.module';
import { CreateOrEditColorComponent } from './create-or-edit-color-modal.component';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { UtilsModule } from '@shared/utils/utils.module';

@NgModule({
    imports: [
        AdminSharedModule,
        ColorRoutingModule,
        UtilsModule
    ],
    declarations: [
        ColorComponent,
        CreateOrEditColorComponent
    ]
})
export class ColorModule { }
