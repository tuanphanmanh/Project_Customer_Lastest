import { CommonModule } from '@angular/common';
import { ModuleWithProviders, NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { AppLocalizationService } from '@app/shared/common/localization/app-localization.service';
import { AppNavigationService } from '@app/shared/layout/nav/app-navigation.service';
import { esignCommonModule } from '@shared/common/common.module';
import { UtilsModule } from '@shared/utils/utils.module';
import { ModalModule } from 'ngx-bootstrap/modal';
import { TabsModule } from 'ngx-bootstrap/tabs';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import {
    BsDatepickerModule,
    BsDatepickerConfig,
    BsDaterangepickerConfig,
    BsLocaleService,
} from 'ngx-bootstrap/datepicker';
import { PaginatorModule } from 'primeng/paginator';
import { TableModule } from 'primeng/table';
import { AppAuthService } from './auth/app-auth.service';
import { AppRouteGuard } from './auth/auth-route-guard';
import { CommonLookupModalComponent } from './lookup/common-lookup-modal.component';
import { EntityTypeHistoryModalComponent } from './entityHistory/entity-type-history-modal.component';
import { EntityChangeDetailModalComponent } from './entityHistory/entity-change-detail-modal.component';
import { DateRangePickerInitialValueSetterDirective } from './timing/date-range-picker-initial-value.directive';
import { DatePickerInitialValueSetterDirective } from './timing/date-picker-initial-value.directive';
import { DateTimeService } from './timing/date-time.service';
import { TimeZoneComboComponent } from './timing/timezone-combo.component';
import { NgxBootstrapDatePickerConfigService } from 'assets/ngx-bootstrap/ngx-bootstrap-datepicker-config.service';
import { PerfectScrollbarModule } from 'ngx-om-perfect-scrollbar';
import { Angular2CountoModule } from '@awaismirza/angular2-counto';
import { AppBsModalModule } from '@shared/common/appBsModal/app-bs-modal.module';
import { SingleLineStringInputTypeComponent } from './input-types/single-line-string-input-type/single-line-string-input-type.component';
import { ComboboxInputTypeComponent } from './input-types/combobox-input-type/combobox-input-type.component';
import { CheckboxInputTypeComponent } from './input-types/checkbox-input-type/checkbox-input-type.component';
import { MultipleSelectComboboxInputTypeComponent } from './input-types/multiple-select-combobox-input-type/multiple-select-combobox-input-type.component';
import { AutoCompleteModule } from 'primeng/autocomplete';
import { PasswordInputWithShowButtonComponent } from './password-input-with-show-button/password-input-with-show-button.component';
import { KeyValueListManagerComponent } from './key-value-list-manager/key-value-list-manager.component';
import { DashboardViewConfigurationService } from './customizable-dashboard/dashboard-view-configuration.service';
import { DataFormatService } from './services/data-format.service';
import { EsignDatepickerComponent } from './input-types/esign-datepicker/esign-datepicker.component';
import { EsignComboboxComponent } from './input-types/esign-combobox/esign-combobox.component';
import { EsignCheckboxComponent } from './input-types/esign-checkbox/esign-checkbox.component';
import { EsignTextareaComponent } from './input-types/esign-textarea/esign-textarea.component';
import { EsignSearchInputComponent } from './input-types/esign-search-input/esign-search-input.component';
import { NewCheckboxComponent } from './input-types/new-checkbox/new-checkbox.component';
import { EsignTooltipComponent } from './esign-tooltip/esign-tooltip.component';
import { InputText, InputTextModule } from 'primeng/inputtext';
import { ButtonComponent, ButtonModule, CheckBoxModule, CheckBoxComponent, ButtonAllModule, SwitchModule, SwitchAllModule, ChipListModule, ChipListAllModule } from '@syncfusion/ej2-angular-buttons';
import { ComboBoxAllModule, DropDownListComponent, DropDownListModule } from '@syncfusion/ej2-angular-dropdowns';
import { AccordionAllModule, MenuAllModule, TabAllModule, TabComponent, ToolbarAllModule, ToolbarComponent, ToolbarModule } from '@syncfusion/ej2-angular-navigations';
import { PdfViewerComponent, PdfViewerModule } from '@syncfusion/ej2-angular-pdfviewer';
import { InputTextarea, InputTextareaModule } from 'primeng/inputtextarea';
import { ListViewComponent, ListViewModule, VirtualizationService } from '@syncfusion/ej2-angular-lists';
import { DataViewModule, DataView } from 'primeng/dataview';
import { ProgressBarComponent, ProgressBarModule } from '@syncfusion/ej2-angular-progressbar';
import { TabModule } from '@syncfusion/ej2-angular-navigations';
import { GridAllModule, GridComponent, GridModule, PagerComponent } from '@syncfusion/ej2-angular-grids';
import {ColorPickerAllModule, UploaderAllModule, UploaderModule } from '@syncfusion/ej2-angular-inputs';
import { MenuModule } from '@syncfusion/ej2-angular-navigations';
import { PagerModule } from '@syncfusion/ej2-angular-grids';
import { ComboBoxModule } from '@syncfusion/ej2-angular-dropdowns';
import { EsignGridComponent } from './esign-grid/esign-grid.component';
import { ColorPickerModule } from '@syncfusion/ej2-angular-inputs';
import { Image, ImageModule } from 'primeng/image';
import { SplitButtonModule, SplitButtonComponent, DropDownButtonModule, DropDownButtonComponent  } from '@syncfusion/ej2-angular-splitbuttons';
import { OrderList, OrderListModule } from 'primeng/orderlist';
import { CurrencyFormatModule } from '@shared/common/currency/currency-format.module';
import { TabPanel, TabView, TabViewModule } from 'primeng/tabview';
import { Accordion, AccordionModule, AccordionTab } from 'primeng/accordion';
import { SelectButton, SelectButtonModule } from 'primeng/selectbutton';
import { OverlayPanel, OverlayPanelModule } from 'primeng/overlaypanel';
import { Dialog, DialogModule } from 'primeng/dialog';
import { TieredMenu, TieredMenuModule } from 'primeng/tieredmenu';
import { Listbox, ListboxModule } from 'primeng/listbox';
import { CascadeSelect, CascadeSelectModule } from 'primeng/cascadeselect';
import { InputSwitch, InputSwitchModule } from 'primeng/inputswitch';
import { Button, ButtonModule as PrimeNGButton } from 'primeng/button';
import { RichTextEditorAllModule, RichTextEditorModule } from '@syncfusion/ej2-angular-richtexteditor';
@NgModule({
    imports: [
        CommonModule,
        FormsModule,
        ReactiveFormsModule,
        ModalModule.forRoot(),
        UtilsModule,
        esignCommonModule,
        TableModule,
        PaginatorModule,
        TabsModule.forRoot(),
        BsDropdownModule.forRoot(),
        BsDatepickerModule.forRoot(),
        PerfectScrollbarModule,
        Angular2CountoModule,
        AppBsModalModule,
        CurrencyFormatModule,
        AutoCompleteModule,
        InputTextModule,
        ButtonModule,
        DropDownListModule,
        CheckBoxModule,
        ToolbarModule,
        ToolbarAllModule,
        PdfViewerModule,
        InputTextareaModule,
        AccordionModule,
        ListViewModule,
        DataViewModule,
        ProgressBarModule,
        TabModule,
        GridModule,
        SwitchModule,
        MenuModule,
        PagerModule,
        ComboBoxModule,
        ColorPickerModule,
        UploaderModule,
        ImageModule,
        SplitButtonModule,
        DropDownButtonModule,
        OrderListModule,
        TabViewModule,
        SelectButtonModule,
        OverlayPanelModule,
        ChipListModule,
        DialogModule,
        TieredMenuModule,
        ListboxModule,
        CascadeSelectModule,
        InputSwitchModule,
        PrimeNGButton,
        RichTextEditorModule
    ],
    declarations: [
        TimeZoneComboComponent,
        CommonLookupModalComponent,
        EntityTypeHistoryModalComponent,
        EntityChangeDetailModalComponent,
        DateRangePickerInitialValueSetterDirective,
        DatePickerInitialValueSetterDirective,
        SingleLineStringInputTypeComponent,
        ComboboxInputTypeComponent,
        CheckboxInputTypeComponent,
        MultipleSelectComboboxInputTypeComponent,
        PasswordInputWithShowButtonComponent,
        KeyValueListManagerComponent,
        EsignComboboxComponent,
        EsignCheckboxComponent,
        NewCheckboxComponent,
        EsignTextareaComponent,
        EsignSearchInputComponent,
        EsignTooltipComponent,
        EsignDatepickerComponent,
        EsignComboboxComponent,
        EsignCheckboxComponent,
        EsignTextareaComponent,
        EsignSearchInputComponent,
        NewCheckboxComponent,
        EsignGridComponent,
    ],
    exports: [
        TimeZoneComboComponent,
        CommonLookupModalComponent,
        EntityTypeHistoryModalComponent,
        EntityChangeDetailModalComponent,
        DateRangePickerInitialValueSetterDirective,
        DatePickerInitialValueSetterDirective,
        PasswordInputWithShowButtonComponent,
        KeyValueListManagerComponent,
        EsignComboboxComponent,
        EsignCheckboxComponent,
        NewCheckboxComponent,
        EsignTextareaComponent,
        EsignSearchInputComponent,
        EsignTooltipComponent,
        EsignDatepickerComponent,
        EsignComboboxComponent,
        EsignCheckboxComponent,
        EsignTextareaComponent,
        EsignSearchInputComponent,
        NewCheckboxComponent,
        InputText,
        ButtonComponent,
        SwitchAllModule,
        ButtonAllModule,
        DropDownListComponent,
        CheckBoxComponent,
        ToolbarComponent,
        ToolbarAllModule,
        PdfViewerComponent,
        InputTextarea,
        Accordion,
        AccordionTab,
        AccordionAllModule,
        ListViewComponent,
        DataView,
        ProgressBarComponent,
        TabComponent,
        TabAllModule,
        GridComponent,
        GridAllModule,
        MenuAllModule,
        PagerComponent,
        ComboBoxAllModule,
        EsignGridComponent,
        ColorPickerAllModule,
        UploaderAllModule,
        Image,
        SplitButtonComponent,
        DropDownButtonComponent,
        OrderList,
        TabView,
        TabPanel,
        SelectButton,
        OverlayPanel,
        ChipListAllModule,
        Dialog,
        TieredMenu,
        Listbox,
        CascadeSelect,
        InputSwitch,
        Button,
        RichTextEditorAllModule
    ],
    providers: [
        DateTimeService,
        AppLocalizationService,
        AppNavigationService,
        DashboardViewConfigurationService,
        DataFormatService,
        VirtualizationService,
        { provide: BsDatepickerConfig, useFactory: NgxBootstrapDatePickerConfigService.getDatepickerConfig },
        { provide: BsDaterangepickerConfig, useFactory: NgxBootstrapDatePickerConfigService.getDaterangepickerConfig },
        { provide: BsLocaleService, useFactory: NgxBootstrapDatePickerConfigService.getDatepickerLocale },
    ]
})
export class AppCommonModule {
    static forRoot(): ModuleWithProviders<AppCommonModule> {
        return {
            ngModule: AppCommonModule,
            providers: [AppAuthService, AppRouteGuard],
        };
    }
}
