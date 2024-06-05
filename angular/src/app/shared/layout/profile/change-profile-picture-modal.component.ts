import { IAjaxResponse, TokenService } from 'abp-ng2-module';
import { Component, ElementRef, EventEmitter, Injector, Output, ViewChild } from '@angular/core';
import { AppConsts } from '@shared/AppConsts';
import { AppComponentBase } from '@shared/common/app-component-base';
import { FileParameter, ProfileServiceProxy } from '@shared/service-proxies/service-proxies';
import { FileUploader, FileUploaderOptions, FileItem } from 'ng2-file-upload';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import { ImageCroppedEvent, base64ToFile } from 'ngx-image-cropper';

@Component({
    selector: 'changeProfilePictureModal',
    templateUrl: './change-profile-picture-modal.component.html',
})
export class ChangeProfilePictureModalComponent extends AppComponentBase {
    @ViewChild('changeProfilePictureModal', { static: true }) modal: ModalDirective;
    @ViewChild('uploadProfilePictureInputLabel') uploadProfilePictureInputLabel: ElementRef;

    @Output() modalSave: EventEmitter<number> = new EventEmitter<number>();

    public active = false;
    public temporaryPictureUrl: string;
    public saving = false;
    public maxProfilPictureBytesUserFriendlyValue = 5;
    imageChangedEvent: any = '';
    userId: number = null;
    image: FileParameter = {data: null, fileName: null};

    constructor(injector: Injector, private _profileService: ProfileServiceProxy, private _tokenService: TokenService) {
        super(injector);
    }

    initializeModal(): void {
        this.active = true;
        this.temporaryPictureUrl = '';
    }

    show(userId?: number): void {
        this.initializeModal();
        this.modal.show();
        this.userId = userId;
    }

    close(): void {
        this.active = false;
        this.imageChangedEvent = '';
        this.modal.hide();
    }

    fileChangeEvent(event: any): void {
        if (event.target.files[0].size > 5242880) {
            this.message.warn(this.l('ProfilePicture_Warn_SizeLimit', this.maxProfilPictureBytesUserFriendlyValue));
            return;
        }
        this.uploadProfilePictureInputLabel.nativeElement.innerText = event.target.files[0].name;
        this.image.fileName = event.target.files[0].name;
        this.imageChangedEvent = event;
    }

    imageCroppedFile(event: ImageCroppedEvent) {
        this.image.data = base64ToFile(event.base64);
    }

    updateProfilePicture(file): void {
        if(this.image.data == null || this.image.fileName == null) {
            this.notify.warn(this.l('ProfilePicture_Warn_Required'));
            return;
        }
        this.saving = true;
        this._profileService
            .updateProfilePicture(file)
            .pipe(
                finalize(() => {
                    this.saving = false;
                })
            )
            .subscribe(() => {
                abp.event.trigger('profilePictureChanged');
                this.modalSave.emit(this.userId);
                this.close();
            });
    }

    save(): void {
        if(this.imageChangedEvent == '') {
            this.close();
            return;
        }
        this.updateProfilePicture(this.image)
    }
}
