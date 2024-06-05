import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef } from '@angular/core';
import { AppComponentBase } from '@shared/common/app-component-base';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { CreateOrEditMstEsignCategoryDto, MstEsignCategoryServiceProxy, MstEsignDivisionGetAllDivisionBySearchValueDto, MstEsignDivisionServiceProxy } from '@shared/service-proxies/service-proxies';
import { finalize } from 'rxjs';
import { FormBuilder, FormGroup } from '@angular/forms';
import { GlobalValidator } from '@shared/utils/validators';
@Component({
    selector: 'create-or-edit-category-modal',
    templateUrl: './create-or-edit-category-modal.component.html'
})
export class CreateOrEditCategoryComponent extends AppComponentBase  implements OnInit {
    @ViewChild('createOrEditCategory', { static: true }) modal: ModalDirective | undefined;

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
    dialogHeader = this.l('CreateCategory');
    rowdata: CreateOrEditMstEsignCategoryDto = new CreateOrEditMstEsignCategoryDto()
    saving: boolean = false;
    listDivision: MstEsignDivisionGetAllDivisionBySearchValueDto[] = [];
    constructor(
        injector: Injector,
        private _service: MstEsignCategoryServiceProxy,
        private _division: MstEsignDivisionServiceProxy,
        private formBuilder: FormBuilder
    ) {
        super(injector);
    }

    ngOnInit(): void {
        this._division.getAllDivisionBySearchValue('').subscribe(res => {
            this.listDivision = res.items;
        });
    }

    buildForm() {
        this.createOrEditForm = this.formBuilder.group({
            id: [0],
            code : [undefined, GlobalValidator.required],
            localName : [undefined],
            internationalName: [undefined],
            localDescription: [undefined],
            internationalDescription: [undefined],
            isMadatory: [false],
            divisionId: [undefined],
        });
    }

    showModalCreateEdit(rowdata?: CreateOrEditMstEsignCategoryDto): void {
        if (!rowdata) {
            this.dialogHeader = this.l('CreateCategory');
            this.rowdata = new CreateOrEditMstEsignCategoryDto();
        } else {
            this.rowdata = rowdata;
            this.dialogHeader = this.l('UpdateCategory')
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
