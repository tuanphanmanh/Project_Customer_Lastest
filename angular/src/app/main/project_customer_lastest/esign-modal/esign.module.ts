import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { SignSuccessComponent } from './sign-success/sign-success.component';
import { RejectModalComponent } from './reject-modal/reject-modal.component';
import { AddMessageComponent } from './add-message/add-message.component';
import { RemoveDocumentComponent } from './remove-document/remove-document.component';
import { UtilsModule } from '@shared/utils/utils.module';
import { RevokeModalComponent } from './revoke-modal/revoke-modal.component';
import { RemindModalComponent } from './remind-modal/remind-modal.component';
import { ReassignModalComponent } from './reassign-modal/reassign-modal.component';
import { SignToPageModalComponent } from './sign-to-page-modal/sign-to-page-modal.component';
import { MissingSignatureComponent } from './missing-signature/missing-signature.component';
import { DataViewComponent } from './data-view/data-view.component';
import { ConfirmSendRequestComponent } from './confirm-send-request/confirm-send-request.component';

import { DataViewTransferComponent } from './data-view-transfer/data-view-transfer.component';
import { DataViewItemComponent } from './data-view/data-view-item/data-view-item.component';
import { DataViewGroupComponent } from './data-view-group/data-view-group.component';
import { ShareRequestComponent } from './share-request/share-request.component';
import { SearchRequestComponent } from './search-request/search-request.component';
import { RequestDetailComponent } from '../document-managerment/request-detail/request-detail.component';
import { TabSignerComponent } from '../document-managerment/request-detail/tab-signer/tab-signer.component';
import { TabHistoryComponent } from '../document-managerment/request-detail/tab-history/tab-history.component';
import { TabCommentComponent } from '../document-managerment/request-detail/tab-comment/tab-comment.component';
import { TabAttachmentComponent } from '../document-managerment/request-detail/tab-attachment/tab-attachment.component';
import { ReviewComponent } from '../document-managerment/review/review.component';
import { TabReferenceComponent } from '../document-managerment/request-detail/tab-reference/tab-reference.component';
import { ConfirmFileExistsComponent } from './confirm-file-exists/confirm-file-exists.component';

@NgModule({
    imports: [
        CommonModule,
        AppSharedModule,
        AdminSharedModule,
        UtilsModule
    ],
    declarations: [
        ConfirmFileExistsComponent,
        ConfirmSendRequestComponent,
        SignSuccessComponent,
        RejectModalComponent,
        AddMessageComponent,
        RemoveDocumentComponent,
        RevokeModalComponent,
        RemindModalComponent,
        ReassignModalComponent,
        SignToPageModalComponent,
        MissingSignatureComponent,
        DataViewComponent,
        DataViewTransferComponent,
        DataViewItemComponent,
        ShareRequestComponent,
        SearchRequestComponent,
        RequestDetailComponent,
        TabSignerComponent,
        TabHistoryComponent,
        TabCommentComponent,
        TabAttachmentComponent,
        ReviewComponent,
        TabReferenceComponent,
        DataViewGroupComponent
    ],
    exports: [
        ConfirmFileExistsComponent,
        ConfirmSendRequestComponent,
        SignSuccessComponent,
        RejectModalComponent,
        AddMessageComponent,
        RemoveDocumentComponent,
        RevokeModalComponent,
        RemindModalComponent,
        ReassignModalComponent,
        SignToPageModalComponent,
        MissingSignatureComponent,
        DataViewComponent,
        DataViewTransferComponent,
        DataViewItemComponent,
        ShareRequestComponent,
        SearchRequestComponent,
        RequestDetailComponent,
        TabSignerComponent,
        TabHistoryComponent,
        TabCommentComponent,
        TabAttachmentComponent,
        ReviewComponent,
        TabReferenceComponent,
        DataViewGroupComponent
    ]
})
export class EsignModule { }
