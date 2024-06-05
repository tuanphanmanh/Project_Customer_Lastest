import { ControlContainer, FormGroupDirective, FormGroupName, FormGroup, AbstractControl, FormArrayName, FormArray} from '@angular/forms';
import { Directive, OnInit, Input, SkipSelf, Host, Renderer2, ElementRef } from '@angular/core';
import { combineLatest, of, merge } from 'rxjs';
import { map } from 'rxjs/operators';

@Directive({
  selector: '[hasError]',
})

export class HasErrorDirective implements OnInit {
  @Input('hasError') hasError: string;
  @Input() errorClassName = 'invalid-control';
  @Input() ignoreCss: Array<string>;
  control: AbstractControl;
  error$;
  formDirective: FormGroupDirective;
  formGroup: FormGroup | FormArray;
  path: string[];

  constructor(
    @Host() @SkipSelf() private controlContainer: ControlContainer,
    private renderer: Renderer2,
    private elementRef: ElementRef
  ) {
  }

  ngOnInit(): void {
    this.getControl();

    const controlError$ = merge(of(undefined), this.formDirective.statusChanges)
      .pipe(map(() => this.control && this.control.errors));
    const isSubmitted$ = merge(of(false), this.formDirective.ngSubmit);

    const reducer = ([...args]) => {
      const [isSubmitted, ce] = args;
      if (!isSubmitted) { return; }
      return { controlError: ce, };
    };

    this.error$ = combineLatest([isSubmitted$, controlError$]).pipe(map(reducer));
    this.error$.subscribe(this.setClass.bind(this));
  }

  private setClass(error: any) {
    const isSet = error && error.controlError;
    if (isSet) {
      if (this.ignoreCss && this.ignoreCss.includes(Object.keys(isSet)[0])) {
        this.renderer.removeClass(this.elementRef.nativeElement, this.errorClassName);
        return;
      }
      this.renderer.addClass(this.elementRef.nativeElement, this.errorClassName);
    } else {
      this.renderer.removeClass(this.elementRef.nativeElement, this.errorClassName);
    }
  }

  private getControl() {
    if (this.controlContainer instanceof FormGroupName) {
      this.formGroup = this.controlContainer.control;
      this.formDirective = this.controlContainer.formDirective as FormGroupDirective;
    } else if (this.controlContainer instanceof FormGroupDirective) {
      this.formGroup = this.controlContainer.form;
      this.formDirective = this.controlContainer;
    } else if (this.controlContainer instanceof FormArrayName) {
      this.formGroup = this.controlContainer.control;
      this.formDirective = this.controlContainer.formDirective as FormGroupDirective;
    }

    if (!this.hasError) {
      this.control = this.formGroup;
    } else {
      this.path = this.hasError.split('.');
      this.control = this.formGroup.get(this.path);
    }

    if (!this.control) { 
      return; 
    }
  }
}