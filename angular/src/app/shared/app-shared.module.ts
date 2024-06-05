import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpClientModule, HttpClientJsonpModule } from '@angular/common/http';
import { FormsModule } from '@angular/forms';
import { AppRoutingModule } from '@app/app-routing.module';
import { AppBsModalModule } from '@shared/common/appBsModal/app-bs-modal.module';
import { ServiceProxyModule } from '@shared/service-proxies/service-proxy.module';
import { UtilsModule } from '@shared/utils/utils.module';
import { TextMaskModule } from '@awaismirza/angular2-text-mask';
import { FileUploadModule } from 'ng2-file-upload';
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { ModalModule } from 'ngx-bootstrap/modal';
import { PopoverModule } from 'ngx-bootstrap/popover';
import { TabsModule } from 'ngx-bootstrap/tabs';
import { ImageCropperModule } from 'ngx-image-cropper';
import { PerfectScrollbarModule } from 'ngx-om-perfect-scrollbar';
import { NgxSpinnerModule } from 'ngx-spinner';
import { AutoCompleteModule } from 'primeng/autocomplete';
import { PaginatorModule } from 'primeng/paginator';
import { ProgressBarModule } from 'primeng/progressbar';
import { TableModule } from 'primeng/table';
import { AppCommonModule } from './common/app-common.module';
import { ThemesLayoutBaseComponent } from './layout/themes/themes-layout-base.component';
import { DialogModule } from '@syncfusion/ej2-angular-popups';
import { ButtonModule  } from '@syncfusion/ej2-angular-buttons';
import { CoreModule } from '@metronic/app/core/core.module';
import { CheckBoxSelectionService, MultiSelectModule } from '@syncfusion/ej2-angular-dropdowns';
import { CalendarModule } from 'primeng/calendar';
import { CurrencyFormatModule } from '@shared/common/currency/currency-format.module';
// import { DatePickerModule, DateRangePickerModule } from '@syncfusion/ej2-angular-calendars';

const imports = [
    CommonModule,
    FormsModule,
    HttpClientModule,
    HttpClientJsonpModule,
    ModalModule,
    TabsModule,
    ButtonModule,
    BsDropdownModule,
    PopoverModule,
    BsDatepickerModule,
    AppCommonModule,
    FileUploadModule,
    AppRoutingModule,
    UtilsModule,
    ServiceProxyModule,
    TableModule,
    PaginatorModule,
    ProgressBarModule,
    PerfectScrollbarModule,
    TextMaskModule,
    ImageCropperModule,
    AutoCompleteModule,
    NgxSpinnerModule,
    AppBsModalModule,
    CurrencyFormatModule,
    DialogModule,
    CoreModule,
    MultiSelectModule,
    CalendarModule,
    // DateRangePickerModule,
    // DatePickerModule,
];

@NgModule({
    imports: [...imports],
    exports: [...imports],
    providers:[CheckBoxSelectionService],
    declarations: [ThemesLayoutBaseComponent],
})
export class AppSharedModule {}
