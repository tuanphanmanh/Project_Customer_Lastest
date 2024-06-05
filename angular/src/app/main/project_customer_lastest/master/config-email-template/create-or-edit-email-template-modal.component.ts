import { Component, ViewChild, Injector, Output, EventEmitter, ElementRef } from '@angular/core';
import { AppComponentBase } from '@shared/common/app-component-base';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { MstEsignEmailTemplateOutputDto, MstEsignEmailTemplateServiceProxy } from '@shared/service-proxies/service-proxies';
import { finalize } from 'rxjs';
import { FormBuilder, FormGroup } from '@angular/forms';
import { GlobalValidator } from '@shared/utils/validators';
import { RichTextEditorComponent } from '@syncfusion/ej2-angular-richtexteditor';
import { ToolbarService, LinkService, ImageService, HtmlEditorService } from '@syncfusion/ej2-angular-richtexteditor';
import { ToolbarModule } from '@syncfusion/ej2-angular-navigations';
@Component({
    selector: 'create-or-edit-email-template-modal',
    templateUrl: './create-or-edit-email-template-modal.component.html',
    providers: [ToolbarService, LinkService, ImageService, HtmlEditorService]
})
export class CreateOrEditEmailTemplateComponent extends AppComponentBase {
    @ViewChild('createOrEditDepartment', { static: true }) modal: ModalDirective | undefined;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();
    @Output() modalClose: EventEmitter<any> = new EventEmitter<any>();
    //form
    createOrEditForm: FormGroup;
    isSubmit = false;
    @ViewChild("submitBtn", { static: false }) submitBtn: ElementRef;
    //end form
    @ViewChild('toolsRTE') public rteObj: RichTextEditorComponent;
    dialogCloseIcon = true;
    dialogWidth: string = '50%';
    isModal: Boolean = true;
    showCloseIcon: Boolean = false;
    visible: Boolean = false;
    animationSettings: Object = { effect: 'FadeZoom' };
    dialogHeader = this.l('Create');

    rowdata: MstEsignEmailTemplateOutputDto = new MstEsignEmailTemplateOutputDto()

    saving: boolean = false;
    public tools: object = {
        items: ['Undo', 'Redo', '|',
            'Bold', 'Italic', 'Underline', 'StrikeThrough', '|',
            'FontName', 'FontSize', 'FontColor', 'BackgroundColor', '|',
            'Formats', 'Alignments', '|', 'OrderedList', 'UnorderedList', '|',
            'Indent', 'Outdent', '|', 'CreateLink',
            'Image']
    };
    public iframe: object = { enable: true };
    public height: number = 500;
    constructor(
        injector: Injector,
        private _service: MstEsignEmailTemplateServiceProxy,
        private formBuilder: FormBuilder
    ) {
        super(injector);
    }

    buildForm() {
        this.createOrEditForm = this.formBuilder.group({
            id: [0],
            templateCode : [undefined, GlobalValidator.required],
            title: [undefined, GlobalValidator.required],
            message: [undefined, GlobalValidator.required],
        });
    }

    showModalCreateEdit(rowdata?: MstEsignEmailTemplateOutputDto): void {
        if (!rowdata) {
            this.dialogHeader = this.l('Create');
            this.rowdata = new MstEsignEmailTemplateOutputDto();
        } else {
            this.rowdata = rowdata;
            this.dialogHeader = this.l('Update')
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
