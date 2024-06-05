import { CUSTOM_ELEMENTS_SCHEMA, NgModule } from '@angular/core';
import {  DivisionComponent } from './division.component';
import { DivisionRoutingModule } from './division-routing.module';
import { CreateOrEditDivisionComponent } from './create-or-edit-division-modal.component';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { UtilsModule } from '@shared/utils/utils.module';

@NgModule({
    declarations: [
        DivisionComponent,
        CreateOrEditDivisionComponent
    ],
    imports: [
        AdminSharedModule,
        UtilsModule,
        DivisionRoutingModule,
    ],
    schemas: [ CUSTOM_ELEMENTS_SCHEMA ]
})
export class DivisionModule { }




