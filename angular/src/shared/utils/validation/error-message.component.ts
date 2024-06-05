import { Component, OnInit, Host, Optional } from '@angular/core';
import { HasErrorDirective } from './has-error.directive';

@Component({
  selector: '[errorMessage]',
  template: '<ng-content></ng-content>',
})

export class ErrorMessageComponent implements OnInit {
  controlError: { [key: string]: any };

  constructor(
    @Optional() @Host() private hasErrorDirective: HasErrorDirective,
  ) {
  }

  ngOnInit() {
    this.hasErrorDirective.error$.subscribe(err => {
      if (!err) {
        this.controlError = {};
        return;
      }

      this.controlError = err.controlError;
    });
  }
}