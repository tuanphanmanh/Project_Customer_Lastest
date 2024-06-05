import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef } from '@angular/core';
import { AppComponentBase } from '@shared/common/app-component-base';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { CreateOrEditMstEsignCategoryDto, CreateOrEditMstEsignConfigDto, MstEsignConfigServiceProxy } from '@shared/service-proxies/service-proxies';
import { finalize } from 'rxjs';
import { FormBuilder, FormGroup } from '@angular/forms';
import { GlobalValidator } from '@shared/utils/validators';
@Component({
    selector: 'create-or-edit-esign-config-modal',
    templateUrl: './create-or-edit-esign-config-modal.component.html'
})
export class CreateOrEditEsignConfigComponent extends AppComponentBase  implements OnInit {
    @ViewChild('createOrEditConfig', { static: true }) modal: ModalDirective | undefined;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();
    @Output() modalClose: EventEmitter<any> = new EventEmitter<any>();
    @ViewChild("submitBtn", { static: false }) submitBtn: ElementRef;

    createOrEditForm: FormGroup;
    isSubmit = false;

    dialogCloseIcon = true;
    dialogWidth: string = '50%';
    isModal: Boolean = true;
    showCloseIcon: Boolean = false;
    visible: Boolean = false;
    animationSettings: Object = { effect: 'FadeZoom' };
    dialogHeader = this.l('CreateConfig');
    rowdata: CreateOrEditMstEsignConfigDto = new CreateOrEditMstEsignConfigDto()
    saving: boolean = false;
    constructor(
        injector: Injector,
        private _service: MstEsignConfigServiceProxy,
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
            value : [undefined, GlobalValidator.required],
            stringValue: [undefined],
            description: [undefined],
        });
    }

    showModalCreateEdit(rowdata?: CreateOrEditMstEsignConfigDto): void {
        if (!rowdata) {
            this.dialogHeader = this.l('CreateConfig');
            this.rowdata = new CreateOrEditMstEsignConfigDto();
        } else {
            this.rowdata = rowdata;
            this.dialogHeader = this.l('UpdateConfig')
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

    onCloseDialog() {
        this.createOrEditForm = undefined;
    }

}
