import { Component, ViewChild, Injector, Output, EventEmitter, ElementRef } from '@angular/core';
import { AppComponentBase } from '@shared/common/app-component-base';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { CreateOrEditMstEsignStatusInputDto, MstEsignStatusServiceProxy } from '@shared/service-proxies/service-proxies';
import { finalize } from 'rxjs';
import { FormBuilder, FormGroup } from '@angular/forms';
import { GlobalValidator } from '@shared/utils/validators';
@Component({
    selector: 'create-or-edit-status-modal',
    templateUrl: './create-or-edit-status-modal.component.html',
})
export class CreateOrEditStatusComponent extends AppComponentBase {
    @ViewChild('createOrEditStatus', { static: true }) modal: ModalDirective | undefined;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();
    @Output() modalClose: EventEmitter<any> = new EventEmitter<any>();
    //form
    createOrEditForm: FormGroup;
    isSubmit = false;
    @ViewChild("submitBtn", { static: false }) submitBtn: ElementRef;
    //end form

    dialogCloseIcon = true;
    dialogWidth: string = '60%';
    isModal: Boolean = true;
    showCloseIcon: Boolean = false;
    visible: Boolean = false;
    animationSettings: Object = { effect: 'FadeZoom'};
    dialogHeader = this.l('CreateStatus');

    rowdata: CreateOrEditMstEsignStatusInputDto = new CreateOrEditMstEsignStatusInputDto()

    saving: boolean = false;
    typeItems: any[] = [
        { value: 0, label: this.l('Mobile') },
        { value: 1, label: this.l('Website') },
    ];
    constructor(
        injector: Injector,
        private _service: MstEsignStatusServiceProxy,
        private formBuilder: FormBuilder
    ) {
        super(injector);
    }

    buildForm() {
        this.createOrEditForm = this.formBuilder.group({
            id: [0],
            code : [undefined, GlobalValidator.required],
            typeId: [undefined, GlobalValidator.required],
            localName: [undefined],
            internationalName: [undefined],
            localDescription: [undefined],
            internationalDescription: [undefined],
        });
    }

    showModalCreateEdit(rowdata?: CreateOrEditMstEsignStatusInputDto): void {
        if (!rowdata) {
            this.rowdata = new CreateOrEditMstEsignStatusInputDto();
            this.dialogHeader = this.l('CreateStatus');
        } else {
            this.rowdata = rowdata;
            this.dialogHeader = this.l('UpdateStatus')
        }
        this.buildForm();
        this.modal.show();
    }
    hideModal() {
        this.buildForm();
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

    onCloseDialog() {
        this.createOrEditForm = undefined;
    }
}
