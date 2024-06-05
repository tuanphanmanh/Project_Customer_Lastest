import { Directive, TemplateRef, ViewContainerRef, DoCheck, Host, Input, EmbeddedViewRef } from '@angular/core';
import { ErrorMessageComponent } from './error-message.component';

class ErrorDirectiveContext {
  $implicit: any;
}

@Directive({
  selector: '[ifError]',
})

export class ErrorDirective implements DoCheck {
  @Input('ifError') ifError: string;
  private created;
  private view: EmbeddedViewRef<ErrorDirectiveContext>;

  constructor(
    @Host() private host: ErrorMessageComponent,
    private templateRef: TemplateRef<ErrorDirectiveContext>,
    private viewContainerRef: ViewContainerRef,
  ) {
  }

  ngDoCheck() {
    const error = this.host.controlError && this.host.controlError[this.ifError];
    if (error) {
      this.create(error);
      return;
    }

    this.destroy();
  }

  private create(error) {
    if (this.created) {
      this.view.context.$implicit = error;
      return;
    }
    this.view = this.viewContainerRef.createEmbeddedView(this.templateRef, {
      $implicit: error,
    });
    this.created = true;
  }

  private destroy() {
    if (!this.created) {
      return;
    }
    this.created = false;
    this.viewContainerRef.clear();
  }
}