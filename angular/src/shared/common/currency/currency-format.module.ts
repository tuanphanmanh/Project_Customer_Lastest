import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CurrencyFormatDirective } from './currency-format.directive';

@NgModule({
    declarations: [CurrencyFormatDirective],
    imports: [CommonModule],
    exports: [CurrencyFormatDirective],
})
export class CurrencyFormatModule {}
