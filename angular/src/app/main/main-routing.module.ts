
import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';

@NgModule({
    imports: [
        RouterModule.forChild([
            {
                path: '',
                children: [
                    {
                        path: 'dashboard',
                        loadChildren: () => import('./project_customer_lastest/dashboard/dashboard.module').then((m) => m.DashboardModule),
                        data: { permission: 'Pages.Business.Dashboard' },
                    },
                    {
                        path: 'document-management',
                        loadChildren: () => import('./project_customer_lastest/document-managerment/document-managerment.module').then((m) => m.DocumentManagermentModule),
                        data: { permission: 'Pages.Business.ViewDocument' },
                    },
                    {
                        path: 'document-management/view-refDocument',
                        loadChildren: () => import('./project_customer_lastest/document-managerment/request-detail/win-reference-documents/win-reference-documents.module').then((m) => m.WinReferenceDocumentsModule),
                        data: { permission: 'Pages.Business.ViewDocument' },
                    },
                    //region Master
                    {
                        path: 'esign/master/division',
                        loadChildren: () => import('./project_customer_lastest/master/division/division.module').then((m) => m.DivisionModule),
                        data: { permission: 'Pages.Master.Division' },
                    },
                    {
                        path: 'esign/master/department',
                        loadChildren: () => import('./project_customer_lastest/master/department/department.module').then((m) => m.DepartmentModule),
                        data: { permission: 'Pages.Master.Department' },
                    },
                    {
                        path: 'esign/master/color',
                        loadChildren: () => import('./project_customer_lastest/master/color/color.module').then((m) => m.ColorModule),
                        data: { permission: 'Pages.Master.EsignColor' },
                    },
                    {
                        path: 'esign/master/status',
                        loadChildren: () => import('./project_customer_lastest/master/status/status.module').then((m) => m.StatusModule),
                        data: { permission: 'Pages.Master.Status' },
                    },
                    {
                        path: 'esign/master/systems',
                        loadChildren: () => import('./project_customer_lastest/master/systems/systems.module').then((m) => m.SystemsModule),
                        data: { permission: 'Pages.Master.System' },
                    },
                    {
                        path: 'esign/master/category',
                        loadChildren: () => import('./project_customer_lastest/master/category/category.module').then((m) => m.CategoryModule),
                        data: { permission: 'Pages.Master.Category' },
                    },
                    {
                        path: 'esign/master/activity-history',
                        loadChildren: () => import('./project_customer_lastest/master/activity-history/activity-history.module').then((m) => m.ActivityHistoryModule),
                        data: { permission: 'Pages.Master.Activity' },
                    },
                    {
                        path: 'esign/master/active-directory',
                        loadChildren: () => import('./project_customer_lastest/master/active-directory/active-directory.module').then((m) => m.ActiveDirectoryModule),
                        data: { permission: 'Pages.Master.Activity' },
                    },
                    {
                        path: 'esign/master/user-image',
                        loadChildren: () => import('./project_customer_lastest/master/user-image/user-image.module').then((m) => m.UserImageModule),
                        data: { permission: 'Pages.Master.SignatureTemplate' },
                    },
                    {
                        path: 'esign/master/signer-template',
                        loadChildren: () => import('./project_customer_lastest/master/signer-template/signer-template.module').then((m) => m.SignerTemplateModule),
                        data: { permission: 'Pages.Master.GroupSigners' },
                    },
                    {
                        path: 'esign/master/config-logo',
                        loadChildren: () => import('./project_customer_lastest/master/logo-config/logo-config.module').then((m) => m.LogoConfigModule),
                        data: { permission: 'Pages.Master.Logo' },
                    },
                    {
                        path: 'esign/master/config-email-template',
                        loadChildren: () => import('./project_customer_lastest/master/config-email-template/config-email-template.module').then((m) => m.ConfigEmailTemplateModule),
                        data: { permission: 'Pages.Master.EmailConfig' },
                    },
                    {
                        path: 'esign/master/esign-config',
                        loadChildren: () => import('./project_customer_lastest/master/esign-config/esign-config.module').then((m) => m.EsignConfigModule),
                        data: { permission: 'Pages.Master.EsignConfig' },
                    },
                    {
                        path: 'esign/master/affiliate',
                        loadChildren: () => import('./project_customer_lastest/master/affiliate/affiliate.module').then((m) => m.AffiliateModule),
                        data: { permission: 'Pages.Master.Category' },
                    },
                    //endregion Master
                    {
                        path: 'create-new-request',
                        loadChildren: () => import('./project_customer_lastest/create-new-request/create-new-request.module').then((m) => m.CreateNewRequestModule),
                        data: { permission: 'Pages.Business.CreateNewDocument' },
                    },
                    {
                        path: 'add-signature',
                        loadChildren: () => import('./project_customer_lastest/add-signature/add-signature.module').then((m) => m.TransferModule),
                        data: { permission: 'Pages.Business.CreateNewDocument' },
                    },
                    {
                        path: 'preview',
                        loadChildren: () => import('./project_customer_lastest/preview/preview.module').then((m) => m.PreviewModule),
                        data: { permission: 'Pages.Business.ViewDocument' },
                    },
                    {
                        path: 'sign-now',
                        loadChildren: () => import('./project_customer_lastest/sign-now/sign-now.module').then((m) => m.SignNowModule),
                        data: { permission: 'Pages.Business.ViewDocument' },
                    },
                    {
                        path: 'transfer',
                        loadChildren: () => import('./project_customer_lastest/transfer/transfer.module').then((m) => m.TransferModule),
                        data: { permission: 'Pages.Business.Transfer' },
                    },
                    {
                        path: 'account-setting',
                        loadChildren: () => import('./project_customer_lastest/account-setting/account-setting.module').then((m) => m.AccountSettingModule),
                        // data: { permission: 'Pages' },
                    },
                    {
                        path: 'view-reference/:id',
                        loadChildren: () => import('./project_customer_lastest/view-reference/view-reference.module').then((m) => m.ViewReferenceModule),
                        data: { permission: 'Pages.Business.ViewDocument' },
                    },
                    {
                        path: 'view-detail/:id',
                        loadChildren: () => import('./project_customer_lastest/view-detail/view-detail.module').then((m) => m.ViewDetailModule),
                        data: { permission: 'Pages.Business.ViewDocument' },
                    },
                    { path: '', redirectTo: 'account-setting', pathMatch: 'full' },
                    { path: '**', redirectTo: 'account-setting' },
                ],
            },
        ]),
    ],
    exports: [RouterModule],
})
export class MainRoutingModule {}
