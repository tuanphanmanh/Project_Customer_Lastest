﻿import { AfterViewInit, Component, Input } from '@angular/core';
import { AppConsts } from '@shared/AppConsts';
import { ProfileServiceProxy } from '@shared/service-proxies/service-proxies';

@Component({
    selector: 'friend-profile-picture',
    template: `
        <img [src]="profilePicture" alt="..." />
    `,
})
export class FriendProfilePictureComponent implements AfterViewInit {
    @Input() userId: number;
    @Input() tenantId: number;

    profilePicture = AppConsts.appBaseUrl + '/assets/common/images/default-profile-picture.png';

    constructor(private _profileService: ProfileServiceProxy) {}

    ngAfterViewInit(): void {
        this.setProfileImage();
    }

    private setProfileImage(): void {
        if (!this.tenantId) {
            this.tenantId = undefined;
        }

        this._profileService.getProfilePicture().subscribe((result) => {
            if (result && result.profilePicture) {
                this.profilePicture = 'data:image/jpeg;base64,' + result.profilePicture;
            }
        });
    }
}
