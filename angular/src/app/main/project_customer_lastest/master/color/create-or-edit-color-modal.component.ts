import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef } from '@angular/core';
import { AppComponentBase } from '@shared/common/app-component-base';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { CreateOrEditMstEsignColorInputDto, MstEsignColorServiceProxy } from '@shared/service-proxies/service-proxies';
import { finalize } from 'rxjs';
import { FormBuilder, FormGroup } from '@angular/forms';
import { GlobalValidator } from '@shared/utils/validators';
@Component({
    selector: 'create-or-edit-color-modal',
    templateUrl: './create-or-edit-color-modal.component.html'
})
export class CreateOrEditColorComponent extends AppComponentBase  implements OnInit {
    @ViewChild('createOrEditColor', { static: true }) modal: ModalDirective | undefined;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();
    @Output() modalClose: EventEmitter<any> = new EventEmitter<any>();
    //form
    createOrEditForm: FormGroup;
    isSubmit = false;
    @ViewChild("submitBtn", { static: false }) submitBtn: ElementRef;
    //end form

    dialogCloseIcon = true;
    dialogWidth: string = '30%';
    isModal: Boolean = true;
    showCloseIcon: Boolean = false;
    visible: Boolean = false;
    animationSettings: Object = { effect: 'FadeZoom' };
    dialogHeader = this.l('CreateColor');
    rowdata: CreateOrEditMstEsignColorInputDto = new CreateOrEditMstEsignColorInputDto()
    saving: boolean = false;
    constructor(
        injector: Injector,
        private _service: MstEsignColorServiceProxy,
        private formBuilder: FormBuilder
    ) {
        super(injector);
    }

    ngOnInit(): void {

    }

    buildForm() {
        this.createOrEditForm = this.formBuilder.group({
            id: [0],
            code : [undefined, GlobalValidator.required],
            name : [undefined],
            order: [0, GlobalValidator.required],
        });
    }


    showModalCreateEdit(rowdata?: CreateOrEditMstEsignColorInputDto): void {
        if (!rowdata) {
            this.dialogHeader = this.l('CreateColor');
            this.rowdata = new CreateOrEditMstEsignColorInputDto();
        } else {
            this.rowdata = rowdata;
            this.dialogHeader = this.l('UpdateColor')
        }
        this.buildForm();
        this.modal.show();
    }
    hideModal() {
        this.createOrEditForm = undefined;
        this.modal.hide();
    }
    save() {
        this.isSubmit = true;
        if (this.submitBtn) {
            this.submitBtn.nativeElement.click();
        }
        if (this.createOrEditForm.invalid) {
            return;
        }
        this.saving = true;
        this._service.createOrEdit(this.rowdata)
            .pipe(finalize(() => this.saving = false))
            .subscribe(() => {
                this.notify.info(this.l('SavedSuccessfully'));
                this.hideModal();
                this.modalSave.emit(this.rowdata);
            });
        this.saving = false;
    }

    changeColor(event: any) {
        this.rowdata.code = event.currentValue.hex;
    }

    onCloseDialog() {
        this.createOrEditForm = undefined;
    }
}
