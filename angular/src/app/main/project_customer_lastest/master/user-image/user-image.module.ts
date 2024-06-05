import { NgModule } from '@angular/core';
import { UserImageComponent } from './user-image.component';
import { UserImageRoutingModule } from './user-image-routing.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';

@NgModule({
    imports: [
        UserImageRoutingModule,
        AdminSharedModule
    ],
    declarations: [UserImageComponent]
})
export class UserImageModule { }
