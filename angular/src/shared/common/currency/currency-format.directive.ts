// currency-format.directive.ts

import { Directive, HostListener, ElementRef, Renderer2 } from '@angular/core';
import { NgControl } from '@angular/forms';
import { DataFormatService } from '@app/shared/common/services/data-format.service';

@Directive({
  selector: '[appCurrencyFormat]'
})
export class CurrencyFormatDirective {

  constructor(private el: ElementRef, private renderer: Renderer2,private dataFormatService : DataFormatService,private ngControl: NgControl) { }


  @HostListener('input', ['$event']) onInput(event: Event): void {
    const inputValue = this.el.nativeElement.value?.replace(/[^\d.]/g, '');
    // Format the input value as currency
    const formattedValue = this.formatCurrency(inputValue);

    // Update the input value with the formatted currency
    this.ngControl.control.setValue(formattedValue, { emitEvent: false })
  }

  private formatCurrency(value: string): string {
    // Implement your currency formatting logic here
    // For simplicity, let's assume the value is a number and format it as currency
    const formattedValue = this.dataFormatService.floatMoneyFormat(value);
    return formattedValue;
  }
}
