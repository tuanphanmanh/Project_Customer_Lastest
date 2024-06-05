import { CUSTOM_ELEMENTS_SCHEMA, NgModule } from '@angular/core';
import {  DepartmentComponent } from './department.component';
import { DepartmentRoutingModule } from './department-routing.module';
import { CreateOrEditDepartmentComponent } from './create-or-edit-department-modal.component';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { UtilsModule } from '@shared/utils/utils.module';

@NgModule({
    declarations: [
        DepartmentComponent,
        CreateOrEditDepartmentComponent
    ],
    imports: [
        AdminSharedModule,
        DepartmentRoutingModule,
        UtilsModule
    ],
    schemas: [ CUSTOM_ELEMENTS_SCHEMA ]
})
export class DepartmentModule { }




