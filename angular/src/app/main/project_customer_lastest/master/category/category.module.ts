import { NgModule } from '@angular/core';
import { CategoryComponent } from './category.component';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { UtilsModule } from '@shared/utils/utils.module';
import { CategoryRoutingModule } from './category-routing.module';
import { CreateOrEditCategoryComponent } from './create-or-edit-category-modal.component';

@NgModule({
  imports: [
        AdminSharedModule,
        UtilsModule,
        CategoryRoutingModule
  ],
  declarations: [
    CategoryComponent,
    CreateOrEditCategoryComponent
  ]
})
export class CategoryModule { }
