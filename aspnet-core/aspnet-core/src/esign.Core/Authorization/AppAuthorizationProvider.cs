using Abp.Authorization;
using Abp.Configuration.Startup;
using Abp.Localization;
using Abp.MultiTenancy;
using esign.Master;
using System.Drawing;
using System.Text.RegularExpressions;
using Twilio.Rest.Api.V2010.Account;

namespace esign.Authorization
{
    /// <summary>
    /// Application's authorization provider.
    /// Defines permissions for the application.
    /// See <see cref="AppPermissions"/> for all permission names.
    /// </summary>
    public class AppAuthorizationProvider : AuthorizationProvider
    {
        private readonly bool _isMultiTenancyEnabled;

        public AppAuthorizationProvider(bool isMultiTenancyEnabled)
        {
            _isMultiTenancyEnabled = isMultiTenancyEnabled;
        }

        public AppAuthorizationProvider(IMultiTenancyConfig multiTenancyConfig)
        {
            _isMultiTenancyEnabled = multiTenancyConfig.IsEnabled;
        }

        public override void SetPermissions(IPermissionDefinitionContext context)
        {
            //COMMON PERMISSIONS (FOR BOTH OF TENANTS AND HOST)
            #region
            var pages = context.GetPermissionOrNull(AppPermissions.Pages) ?? context.CreatePermission(AppPermissions.Pages, L("Pages"));
            pages.CreateChildPermission(AppPermissions.Pages_DemoUiComponents, L("DemoUiComponents"));

            var administration = pages.CreateChildPermission(AppPermissions.Pages_Administration, L("Administration"));
            administration.CreateChildPermission(AppPermissions.Pages_Administration_TenantRegistration, L("TenantRegistration"));


            var roles = administration.CreateChildPermission(AppPermissions.Pages_Administration_Roles, L("Roles"));
            roles.CreateChildPermission(AppPermissions.Pages_Administration_Roles_CreateOrUpdateRole, L("CreateOrUpdateRole"));
            roles.CreateChildPermission(AppPermissions.Pages_Administration_Roles_GetRoles, L("GetRoles"));
            roles.CreateChildPermission(AppPermissions.Pages_Administration_Roles_Create, L("CreatingNewRole"));
            roles.CreateChildPermission(AppPermissions.Pages_Administration_Roles_Edit, L("EditingRole"));
            roles.CreateChildPermission(AppPermissions.Pages_Administration_Roles_Delete, L("DeletingRole"));

            var users = administration.CreateChildPermission(AppPermissions.Pages_Administration_Users, L("Pages_Administration_Users"));
            users.CreateChildPermission(AppPermissions.Pages_Administration_Users_Create, L("Pages_Administration_Users_Create"));
            users.CreateChildPermission(AppPermissions.Pages_Administration_Users_RevokeToken, L("Pages_Administration_Users_RevokeToken"));
            users.CreateChildPermission(AppPermissions.Pages_Administration_Users_Edit, L("Pages_Administration_Users_Edit"));
            users.CreateChildPermission(AppPermissions.Pages_Administration_Users_Delete, L("Pages_Administration_Users_Delete"));
            users.CreateChildPermission(AppPermissions.Pages_Administration_Users_ChangePermissions, L("Pages_Administration_Users_ChangePermissions"));
            users.CreateChildPermission(AppPermissions.Pages_Administration_Users_Impersonation, L("LoginForUsers"));
            users.CreateChildPermission(AppPermissions.Pages_Administration_Users_Unlock, L("Unlock"));
            users.CreateChildPermission(AppPermissions.Pages_Administration_Users_ChangeProfilePicture, L("Pages_Administration_Users_ChangeProfilePicture"));

            var languages = administration.CreateChildPermission(AppPermissions.Pages_Administration_Languages, L("Pages_Administration_Languages"));
            languages.CreateChildPermission(AppPermissions.Pages_Administration_Languages_Create, L("CreatingNewLanguage"), multiTenancySides: _isMultiTenancyEnabled ? MultiTenancySides.Host : MultiTenancySides.Tenant);
            languages.CreateChildPermission(AppPermissions.Pages_Administration_Languages_Edit, L("EditingLanguage"), multiTenancySides: _isMultiTenancyEnabled ? MultiTenancySides.Host : MultiTenancySides.Tenant);
            languages.CreateChildPermission(AppPermissions.Pages_Administration_Languages_Delete, L("DeletingLanguages"), multiTenancySides: _isMultiTenancyEnabled ? MultiTenancySides.Host : MultiTenancySides.Tenant);
            languages.CreateChildPermission(AppPermissions.Pages_Administration_Languages_ChangeTexts, L("ChangingTexts"));
            languages.CreateChildPermission(AppPermissions.Pages_Administration_Languages_ChangeDefaultLanguage, L("Pages_Administration_Languages_ChangeDefaultLanguage"));
            
            var auditLogs = administration.CreateChildPermission(AppPermissions.Pages_Administration_AuditLogs, L("AuditLogs"));
            auditLogs.CreateChildPermission(AppPermissions.Pages_Administration_AuditLogs_GetAuditLogs, L("GetAuditLogs"));
            auditLogs.CreateChildPermission(AppPermissions.Pages_Administration_AuditLogs_GetAuditLogsToExcel, L("GetAuditLogsToExcel"));
            auditLogs.CreateChildPermission(AppPermissions.Pages_Administration_AuditLogs_GetEntityHistoryObjectTypes, L("GetEntityHistoryObjectTypes"));
            auditLogs.CreateChildPermission(AppPermissions.Pages_Administration_AuditLogs_GetEntityChanges, L("GetEntityChanges"));
            auditLogs.CreateChildPermission(AppPermissions.Pages_Administration_AuditLogs_GetEntityTypeChanges, L("GetEntityTypeChanges"));
            auditLogs.CreateChildPermission(AppPermissions.Pages_Administration_AuditLogs_GetEntityChangesToExcel, L("GetEntityChangesToExcel"));
            auditLogs.CreateChildPermission(AppPermissions.Pages_Administration_AuditLogs_GetEntityPropertyChanges, L("GetEntityPropertyChanges"));

            administration.CreateChildPermission(AppPermissions.Pages_Administration_AuditReports, L("AuditReports"));

            var organizationUnits = administration.CreateChildPermission(AppPermissions.Pages_Administration_OrganizationUnits, L("OrganizationUnits"));
            organizationUnits.CreateChildPermission(AppPermissions.Pages_Administration_OrganizationUnits_GetOrganizationUnits, L("Pages_Administration_OrganizationUnits_GetOrganizationUnits"));
            organizationUnits.CreateChildPermission(AppPermissions.Pages_Administration_OrganizationUnits_GetOrganizationUnitUsers, L("Pages_Administration_OrganizationUnits_GetOrganizationUnitUsers"));
            organizationUnits.CreateChildPermission(AppPermissions.Pages_Administration_OrganizationUnits_GetOrganizationUnitRoles, L("Pages_Administration_OrganizationUnits_GetOrganizationUnitRoles"));
            organizationUnits.CreateChildPermission(AppPermissions.Pages_Administration_OrganizationUnits_GetAll, L("Pages_Administration_OrganizationUnits_GetAll"));
            organizationUnits.CreateChildPermission(AppPermissions.Pages_Administration_OrganizationUnits_ManageOrganizationTree, L("ManagingOrganizationTree"));
            organizationUnits.CreateChildPermission(AppPermissions.Pages_Administration_OrganizationUnits_ManageMembers, L("ManagingMembers"));
            organizationUnits.CreateChildPermission(AppPermissions.Pages_Administration_OrganizationUnits_ManageRoles, L("ManagingRoles"));

            //administration.CreateChildPermission(AppPermissions.Pages_Administration_UiCustomization, L("VisualSettings"));

            var webhooks = administration.CreateChildPermission(AppPermissions.Pages_Administration_WebhookSubscription, L("Pages_Administration_WebhookSubscription"));
            //webhooks.CreateChildPermission(AppPermissions.Pages_Administration_WebhookSubscription_Create, L("CreatingWebhooks"));
            //webhooks.CreateChildPermission(AppPermissions.Pages_Administration_WebhookSubscription_Edit, L("EditingWebhooks"));
            //webhooks.CreateChildPermission(AppPermissions.Pages_Administration_WebhookSubscription_ChangeActivity, L("ChangingWebhookActivity"));
            //webhooks.CreateChildPermission(AppPermissions.Pages_Administration_WebhookSubscription_Detail, L("DetailingSubscription"));
            webhooks.CreateChildPermission(AppPermissions.Pages_Administration_Webhook_ListSendAttempts, L("Pages_Administration_Webhook_ListSendAttempts"));
            //webhooks.CreateChildPermission(AppPermissions.Pages_Administration_Webhook_ResendWebhook, L("ResendingWebhook"));

            //var dynamicProperties = administration.CreateChildPermission(AppPermissions.Pages_Administration_DynamicProperties, L("DynamicProperties"));
            //dynamicProperties.CreateChildPermission(AppPermissions.Pages_Administration_DynamicProperties_Create, L("CreatingDynamicProperties"));
            //dynamicProperties.CreateChildPermission(AppPermissions.Pages_Administration_DynamicProperties_Edit, L("EditingDynamicProperties"));
            //dynamicProperties.CreateChildPermission(AppPermissions.Pages_Administration_DynamicProperties_Delete, L("DeletingDynamicProperties"));

            //var dynamicPropertyValues = dynamicProperties.CreateChildPermission(AppPermissions.Pages_Administration_DynamicPropertyValue, L("DynamicPropertyValue"));
            //dynamicPropertyValues.CreateChildPermission(AppPermissions.Pages_Administration_DynamicPropertyValue_Create, L("CreatingDynamicPropertyValue"));
            //dynamicPropertyValues.CreateChildPermission(AppPermissions.Pages_Administration_DynamicPropertyValue_Edit, L("EditingDynamicPropertyValue"));
            //dynamicPropertyValues.CreateChildPermission(AppPermissions.Pages_Administration_DynamicPropertyValue_Delete, L("DeletingDynamicPropertyValue"));

            //var dynamicEntityProperties = dynamicProperties.CreateChildPermission(AppPermissions.Pages_Administration_DynamicEntityProperties, L("DynamicEntityProperties"));
            //dynamicEntityProperties.CreateChildPermission(AppPermissions.Pages_Administration_DynamicEntityProperties_Create, L("CreatingDynamicEntityProperties"));
            //dynamicEntityProperties.CreateChildPermission(AppPermissions.Pages_Administration_DynamicEntityProperties_Edit, L("EditingDynamicEntityProperties"));
            //dynamicEntityProperties.CreateChildPermission(AppPermissions.Pages_Administration_DynamicEntityProperties_Delete, L("DeletingDynamicEntityProperties"));

            //var dynamicEntityPropertyValues = dynamicProperties.CreateChildPermission(AppPermissions.Pages_Administration_DynamicEntityPropertyValue, L("EntityDynamicPropertyValue"));
            //dynamicEntityPropertyValues.CreateChildPermission(AppPermissions.Pages_Administration_DynamicEntityPropertyValue_Create, L("CreatingDynamicEntityPropertyValue"));
            //dynamicEntityPropertyValues.CreateChildPermission(AppPermissions.Pages_Administration_DynamicEntityPropertyValue_Edit, L("EditingDynamicEntityPropertyValue"));
            //dynamicEntityPropertyValues.CreateChildPermission(AppPermissions.Pages_Administration_DynamicEntityPropertyValue_Delete, L("DeletingDynamicEntityPropertyValue"));

            //var massNotification = administration.CreateChildPermission(AppPermissions.Pages_Administration_MassNotification, L("MassNotifications"));
            //massNotification.CreateChildPermission(AppPermissions.Pages_Administration_MassNotification_Create, L("MassNotificationCreate"));

            //TENANT-SPECIFIC PERMISSIONS

            //pages.CreateChildPermission(AppPermissions.Pages_Tenant_Dashboard, L("Dashboard"), multiTenancySides: MultiTenancySides.Tenant);

            administration.CreateChildPermission(AppPermissions.Pages_Administration_Tenant_Settings, L("Pages_Administration_Tenant_Settings"), multiTenancySides: MultiTenancySides.Tenant);
            //administration.CreateChildPermission(AppPermissions.Pages_Administration_Tenant_SubscriptionManagement, L("Subscription"), multiTenancySides: MultiTenancySides.Tenant);

            //HOST-SPECIFIC PERMISSIONS

            //var editions = pages.CreateChildPermission(AppPermissions.Pages_Editions, L("Editions"), multiTenancySides: MultiTenancySides.Host);
            //editions.CreateChildPermission(AppPermissions.Pages_Editions_Create, L("CreatingNewEdition"), multiTenancySides: MultiTenancySides.Host);
            //editions.CreateChildPermission(AppPermissions.Pages_Editions_Edit, L("EditingEdition"), multiTenancySides: MultiTenancySides.Host);
            //editions.CreateChildPermission(AppPermissions.Pages_Editions_Delete, L("DeletingEdition"), multiTenancySides: MultiTenancySides.Host);
            //editions.CreateChildPermission(AppPermissions.Pages_Editions_MoveTenantsToAnotherEdition, L("MoveTenantsToAnotherEdition"), multiTenancySides: MultiTenancySides.Host);

            var tenants = pages.CreateChildPermission(AppPermissions.Pages_Tenants, L("Tenants"), multiTenancySides: MultiTenancySides.Host);
            tenants.CreateChildPermission(AppPermissions.Pages_Tenants_Create, L("CreatingNewTenant"), multiTenancySides: MultiTenancySides.Host);
            tenants.CreateChildPermission(AppPermissions.Pages_Tenants_Edit, L("EditingTenant"), multiTenancySides: MultiTenancySides.Host);
            tenants.CreateChildPermission(AppPermissions.Pages_Tenants_ChangeFeatures, L("ChangingFeatures"), multiTenancySides: MultiTenancySides.Host);
            tenants.CreateChildPermission(AppPermissions.Pages_Tenants_Delete, L("DeletingTenant"), multiTenancySides: MultiTenancySides.Host);
            tenants.CreateChildPermission(AppPermissions.Pages_Tenants_Impersonation, L("LoginForTenants"), multiTenancySides: MultiTenancySides.Host);
            tenants.CreateChildPermission(AppPermissions.Pages_Tenant_Customization, L("Customization"), multiTenancySides: MultiTenancySides.Host);
            tenants.CreateChildPermission(AppPermissions.Pages_Tenant_Dashboard, L("Pages_Tenant_Dashboard"), multiTenancySides: MultiTenancySides.Host);

            administration.CreateChildPermission(AppPermissions.Pages_Administration_Host_Settings, L("Settings"), multiTenancySides: MultiTenancySides.Host);

            //var maintenance = administration.CreateChildPermission(AppPermissions.Pages_Administration_Host_Maintenance, L("Pages_Administration_Host_Maintenance"), multiTenancySides: _isMultiTenancyEnabled ? MultiTenancySides.Host : MultiTenancySides.Tenant);
            //maintenance.CreateChildPermission(AppPermissions.Pages_Administration_NewVersion_Create, L("SendNewVersionNotification"));

            //administration.CreateChildPermission(AppPermissions.Pages_Administration_HangfireDashboard, L("HangfireDashboard"), multiTenancySides: _isMultiTenancyEnabled ? MultiTenancySides.Host : MultiTenancySides.Tenant);
            administration.CreateChildPermission(AppPermissions.Pages_Administration_Host_Dashboard, L("Dashboard"), multiTenancySides: MultiTenancySides.Host);
            #endregion

            //NEW

            #region Business
            var business = pages.CreateChildPermission(AppPermissions.Pages_Business, L("ESign"), multiTenancySides: MultiTenancySides.Tenant);
            business.CreateChildPermission(AppPermissions.Pages_Business_CreateNewDocument, L("CreateNewDocument"), multiTenancySides: MultiTenancySides.Tenant);
            business.CreateChildPermission(AppPermissions.Pages_Business_Dashboard, L("Dashboard"), multiTenancySides: MultiTenancySides.Tenant);
            business.CreateChildPermission(AppPermissions.Pages_Business_Transfer, L("Transfer"), multiTenancySides: MultiTenancySides.Tenant);

            var viewDocument = business.CreateChildPermission(AppPermissions.Pages_Business_ViewDocument, L("ViewDocument"), multiTenancySides: MultiTenancySides.Tenant);
            viewDocument.CreateChildPermission(AppPermissions.Pages_Business_ViewDocument_Edit, L("Pages_Business_ViewDocument_Edit"), multiTenancySides: MultiTenancySides.Tenant);
            viewDocument.CreateChildPermission(AppPermissions.Pages_Business_ViewDocument_Delete, L("Pages_Business_ViewDocument_Delete"), multiTenancySides: MultiTenancySides.Tenant);
            viewDocument.CreateChildPermission(AppPermissions.Pages_Business_ViewDocument_Submit, L("Pages_Business_ViewDocument_Submit"), multiTenancySides: MultiTenancySides.Tenant);
            viewDocument.CreateChildPermission(AppPermissions.Pages_Business_ViewDocument_Remind, L("Pages_Business_ViewDocument_Remind"), multiTenancySides: MultiTenancySides.Tenant);
            viewDocument.CreateChildPermission(AppPermissions.Pages_Business_ViewDocument_Revoke, L("Pages_Business_ViewDocument_Revoke"), multiTenancySides: MultiTenancySides.Tenant);
            viewDocument.CreateChildPermission(AppPermissions.Pages_Business_ViewDocument_Reject, L("Pages_Business_ViewDocument_Reject"), multiTenancySides: MultiTenancySides.Tenant);
            viewDocument.CreateChildPermission(AppPermissions.Pages_Business_ViewDocument_Transfer, L("Pages_Business_ViewDocument_Transfer"), multiTenancySides: MultiTenancySides.Tenant);
            viewDocument.CreateChildPermission(AppPermissions.Pages_Business_ViewDocument_Reassign, L("Pages_Business_ViewDocument_Reassign"), multiTenancySides: MultiTenancySides.Tenant);
            viewDocument.CreateChildPermission(AppPermissions.Pages_Business_ViewDocument_Share, L("Pages_Business_ViewDocument_Share"), multiTenancySides: MultiTenancySides.Tenant);
            viewDocument.CreateChildPermission(AppPermissions.Pages_Business_ViewDocument_Comment, L("Pages_Business_ViewDocument_Comment"), multiTenancySides: MultiTenancySides.Tenant);
            viewDocument.CreateChildPermission(AppPermissions.Pages_Business_ViewDocument_Search, L("Pages_Business_ViewDocument_Search"), multiTenancySides: MultiTenancySides.Tenant);

            var signDocument = viewDocument.CreateChildPermission(AppPermissions.Pages_Business_ViewDocument_SignDocument, L("Pages_Business_ViewDocument_SignDocument"), multiTenancySides: MultiTenancySides.Tenant);
            signDocument.CreateChildPermission(AppPermissions.Pages_Business_ViewDocument_Sign, L("Pages_Business_ViewDocument_Sign"), multiTenancySides: MultiTenancySides.Tenant);
            signDocument.CreateChildPermission(AppPermissions.Pages_Business_ViewDocument_Sign_UploadSignature, L("Pages_Business_ViewDocument_Sign_UploadSignature"), multiTenancySides: MultiTenancySides.Tenant);
            signDocument.CreateChildPermission(AppPermissions.Pages_Business_ViewDocument_Sign_DrawSignature, L("Pages_Business_ViewDocument_Sign_DrawSignature"), multiTenancySides: MultiTenancySides.Tenant);


            var viewNotificationt = business.CreateChildPermission(AppPermissions.Pages_Business_ViewNotification, L("ViewNotification"), multiTenancySides: MultiTenancySides.Tenant);
            viewNotificationt.CreateChildPermission(AppPermissions.Pages_Business_ViewNotification_View, L("Pages_Business_ViewNotification_View"), multiTenancySides: MultiTenancySides.Tenant);
            viewNotificationt.CreateChildPermission(AppPermissions.Pages_Business_ViewNotification_Delete, L("Pages_Business_ViewNotification_DeleteNotification"), multiTenancySides: MultiTenancySides.Tenant);
            viewNotificationt.CreateChildPermission(AppPermissions.Pages_Business_ViewNotification_MarkAsReadAll, L("Pages_Business_ViewNotification_MarkAsReadAllNotifications"), multiTenancySides: MultiTenancySides.Tenant);

            //ductm
            #region EsignSignerTemplateLink
            var esignSignerTemplateLin = business.CreateChildPermission(AppPermissions.Pages_Business_EsignSignerTemplateLink, L("Pages_Business_EsignSignerTemplateLink"), multiTenancySides: MultiTenancySides.Tenant);
            esignSignerTemplateLin.CreateChildPermission(AppPermissions.Pages_Business_EsignSignerTemplateLink_CreateNewTemplateForRequester, L("Pages_Business_EsignSignerTemplateLink_CreateNewTemplateForRequester"), multiTenancySides: MultiTenancySides.Tenant);
            esignSignerTemplateLin.CreateChildPermission(AppPermissions.Pages_Business_EsignSignerTemplateLink_GetListSignerByTemplateId, L("Pages_Business_EsignSignerTemplateLink_GetListSignerByTemplateId"), multiTenancySides: MultiTenancySides.Tenant);
            esignSignerTemplateLin.CreateChildPermission(AppPermissions.Pages_Business_EsignSignerTemplateLink_CreateNewTemplateForWebRequesterForWeb, L("Pages_Business_EsignSignerTemplateLink_CreateNewTemplateForWebRequesterForWeb"), multiTenancySides: MultiTenancySides.Tenant);
            #endregion

            #region EsignSignerList
            var esignSignerList = pages.CreateChildPermission(AppPermissions.Pages_EsignSignerList, L("Pages_EsignSignerList"), multiTenancySides: MultiTenancySides.Tenant);
            esignSignerList.CreateChildPermission(AppPermissions.Pages_EsignSignerList_GetListSignerByRequestId, L("Pages_EsignSignerList_GetListSignerByRequestId"), multiTenancySides: MultiTenancySides.Tenant);
            esignSignerList.CreateChildPermission(AppPermissions.Pages_EsignSignerList_GetListSignerByRequestIdForRequestInfo, L("Pages_EsignSignerList_GetListSignerByRequestIdForRequestInfo"), multiTenancySides: MultiTenancySides.Tenant);
            esignSignerList.CreateChildPermission(AppPermissions.Pages_EsignSignerList_GetListSignerByAttachmentId, L("Pages_EsignSignerList_GetListSignerByAttachmentId"), multiTenancySides: MultiTenancySides.Tenant);
            esignSignerList.CreateChildPermission(AppPermissions.Pages_EsignSignerList_UpdateSignOffStatus, L("Pages_EsignSignerList_UpdateSignOffStatus"), multiTenancySides: MultiTenancySides.Tenant);
            esignSignerList.CreateChildPermission(AppPermissions.Pages_EsignSignerList_DoRejectRequest, L("Pages_EsignSignerList_DoRejectRequest"), multiTenancySides: MultiTenancySides.Tenant);
            esignSignerList.CreateChildPermission(AppPermissions.Pages_EsignSignerList_CloneRequest, L("Pages_EsignSignerList_CloneRequest"), multiTenancySides: MultiTenancySides.Tenant);
            esignSignerList.CreateChildPermission(AppPermissions.Pages_EsignSignerList_CloneRequestWithoutFields, L("Pages_EsignSignerList_CloneRequestWithoutFields"), multiTenancySides: MultiTenancySides.Tenant);
            esignSignerList.CreateChildPermission(AppPermissions.Pages_EsignSignerList_DoRevokeRequest, L("Pages_EsignSignerList_DoRevokeRequest"), multiTenancySides: MultiTenancySides.Tenant);
            esignSignerList.CreateChildPermission(AppPermissions.Pages_EsignSignerList_GetListSignerAndRequestByRequestId, L("Pages_EsignSignerList_GetListSignerAndRequestByRequestId"), multiTenancySides: MultiTenancySides.Tenant);
            #endregion

            #region EsignSignerList
            var esignRequestWeb = pages.CreateChildPermission(AppPermissions.Pages_EsignRequestWeb, L("Pages_EsignRequestWeb"), multiTenancySides: MultiTenancySides.Tenant);
            esignRequestWeb.CreateChildPermission(AppPermissions.Pages_EsignRequestWeb_GetListRequestsBySystemIdWeb, L("Pages_EsignRequestWeb_GetListRequestsBySystemIdWeb"), multiTenancySides: MultiTenancySides.Tenant);
            esignRequestWeb.CreateChildPermission(AppPermissions.Pages_EsignRequestWeb_GetListRequestsForDashboard, L("Pages_EsignRequestWeb_GetListRequestsForDashboard"), multiTenancySides: MultiTenancySides.Tenant);
            esignRequestWeb.CreateChildPermission(AppPermissions.Pages_EsignRequestWeb_GetRequestDetailDashboard, L("Pages_EsignRequestWeb_GetRequestDetailDashboard"), multiTenancySides: MultiTenancySides.Tenant);
            esignRequestWeb.CreateChildPermission(AppPermissions.Pages_EsignRequestWeb_GetListRequestsCanTransferWeb, L("Pages_EsignRequestWeb_GetListRequestsCanTransferWeb"), multiTenancySides: MultiTenancySides.Tenant);
            esignRequestWeb.CreateChildPermission(AppPermissions.Pages_EsignRequestWeb_GetRequestsByIdForSelectedItemWeb, L("Pages_EsignRequestWeb_GetRequestsByIdForSelectedItemWeb"), multiTenancySides: MultiTenancySides.Tenant);
            esignRequestWeb.CreateChildPermission(AppPermissions.Pages_EsignRequestWeb_GetEsignPositionsWebByDocumentId, L("Pages_EsignRequestWeb_GetEsignPositionsWebByDocumentId"), multiTenancySides: MultiTenancySides.Tenant);
            esignRequestWeb.CreateChildPermission(AppPermissions.Pages_EsignRequestWeb_GetTransferHistory, L("Pages_EsignRequestWeb_GetTransferHistory"), multiTenancySides: MultiTenancySides.Tenant);

            #endregion

            #region EsignSignerSearchHistory
            var esignSignerSearchHistory = pages.CreateChildPermission(AppPermissions.Pages_EsignSignerSearchHistory, L("Pages_EsignSignerSearchHistory"), multiTenancySides: MultiTenancySides.Tenant);
            esignSignerSearchHistory.CreateChildPermission(AppPermissions.Pages_EsignSignerSearchHistory_GetSignerSearchHistory, L("Pages_EsignSignerSearchHistory_GetListRequestsBySystemIdWeb"), multiTenancySides: MultiTenancySides.Tenant);
            #endregion
            #region Notification
            var notification = pages.CreateChildPermission(AppPermissions.Pages_Notification, L("Notification"), multiTenancySides: MultiTenancySides.Tenant);
            notification.CreateChildPermission(AppPermissions.Pages_Notification_SendDeviceToken, L("Pages_Notification_SendDeviceToken"), multiTenancySides: MultiTenancySides.Tenant);
            notification.CreateChildPermission(AppPermissions.Pages_Notification_DeleteDeviceToken, L("Pages_Notification_DeleteDeviceToken"), multiTenancySides: MultiTenancySides.Tenant);
            notification.CreateChildPermission(AppPermissions.Pages_Notification_CreateEsignNotification, L("Pages_Notification_CreateEsignNotification"), multiTenancySides: MultiTenancySides.Tenant);
            notification.CreateChildPermission(AppPermissions.Pages_Notification_GetUserNotifications, L("Pages_Notification_GetUserNotifications"), multiTenancySides: MultiTenancySides.Tenant);
            notification.CreateChildPermission(AppPermissions.Pages_Notification_ShouldUserUpdateApp, L("Pages_Notification_ShouldUserUpdateApp"), multiTenancySides: MultiTenancySides.Tenant);
            notification.CreateChildPermission(AppPermissions.Pages_Notification_SetAllAvailableVersionNotificationAsRead, L("Pages_Notification_SetAllAvailableVersionNotificationAsRead"), multiTenancySides: MultiTenancySides.Tenant);
            notification.CreateChildPermission(AppPermissions.Pages_Notification_SetAllNotificationsAsRead, L("Pages_Notification_SetAllNotificationsAsRead"), multiTenancySides: MultiTenancySides.Tenant);
            notification.CreateChildPermission(AppPermissions.Pages_Notification_SetNotificationAsRead, L("Pages_Notification_SetNotificationAsRead"), multiTenancySides: MultiTenancySides.Tenant);
            notification.CreateChildPermission(AppPermissions.Pages_Notification_GetNotificationSettings, L("Pages_Notification_GetNotificationSettings"), multiTenancySides: MultiTenancySides.Tenant);
            notification.CreateChildPermission(AppPermissions.Pages_Notification_UpdateNotificationSettings, L("Pages_Notification_UpdateNotificationSettings"), multiTenancySides: MultiTenancySides.Tenant);
            notification.CreateChildPermission(AppPermissions.Pages_Notification_DeleteNotification, L("Pages_Notification_DeleteNotification"), multiTenancySides: MultiTenancySides.Tenant);
            notification.CreateChildPermission(AppPermissions.Pages_Notification_DeleteAllUserNotifications, L("Pages_Notification_DeleteAllUserNotifications"), multiTenancySides: MultiTenancySides.Tenant);
            notification.CreateChildPermission(AppPermissions.Pages_Notification_GetAllUserForLookupTable, L("Pages_Notification_GetAllUserForLookupTable"), multiTenancySides: MultiTenancySides.Tenant);
            notification.CreateChildPermission(AppPermissions.Pages_Notification_GetAllOrganizationUnitForLookupTable, L("Pages_Notification_GetAllOrganizationUnitForLookupTable"), multiTenancySides: MultiTenancySides.Tenant);
            notification.CreateChildPermission(AppPermissions.Pages_Notification_CreateMassNotification, L("Pages_Notification_CreateMassNotification"), multiTenancySides: MultiTenancySides.Tenant);
            notification.CreateChildPermission(AppPermissions.Pages_Notification_CreateNewVersionReleasedNotification, L("Pages_Notification_CreateNewVersionReleasedNotification"), multiTenancySides: MultiTenancySides.Tenant);
            notification.CreateChildPermission(AppPermissions.Pages_Notification_GetAllNotifiers, L("Pages_Notification_GetAllNotifiers"), multiTenancySides: MultiTenancySides.Tenant);
            notification.CreateChildPermission(AppPermissions.Pages_Notification_GetNotificationsPublishedByUser, L("Pages_Notification_GetNotificationsPublishedByUser"), multiTenancySides: MultiTenancySides.Tenant);

            #endregion

            #region Payment
            var payment = pages.CreateChildPermission(AppPermissions.Pages_Payment, L("Payment"), multiTenancySides: MultiTenancySides.Tenant);
            payment.CreateChildPermission(AppPermissions.Pages_Payment_GetPaymentInfo, L("Pages_Payment_GetPaymentInfo"), multiTenancySides: MultiTenancySides.Tenant);
            payment.CreateChildPermission(AppPermissions.Pages_Payment_CreatePayment, L("Pages_Payment_CreatePayment"), multiTenancySides: MultiTenancySides.Tenant);
            payment.CreateChildPermission(AppPermissions.Pages_Payment_CancelPayment, L("Pages_Payment_CancelPayment"), multiTenancySides: MultiTenancySides.Tenant);
            payment.CreateChildPermission(AppPermissions.Pages_Payment_GetPaymentHistory, L("Pages_Payment_GetPaymentHistory"), multiTenancySides: MultiTenancySides.Tenant);
            payment.CreateChildPermission(AppPermissions.Pages_Payment_GetActiveGateways, L("Pages_Payment_GetActiveGateways"), multiTenancySides: MultiTenancySides.Tenant);
            payment.CreateChildPermission(AppPermissions.Pages_Payment_GetPayment, L("Pages_Payment_GetPayment"), multiTenancySides: MultiTenancySides.Tenant);
            payment.CreateChildPermission(AppPermissions.Pages_Payment_GetLastCompletedPayment, L("Pages_Payment_GetLastCompletedPayment"), multiTenancySides: MultiTenancySides.Tenant);
            payment.CreateChildPermission(AppPermissions.Pages_Payment_BuyNowSucceed, L("Pages_Payment_BuyNowSucceed"), multiTenancySides: MultiTenancySides.Tenant);
            payment.CreateChildPermission(AppPermissions.Pages_Payment_NewRegistrationSucceed, L("Pages_Payment_NewRegistrationSucceed"), multiTenancySides: MultiTenancySides.Tenant);
            payment.CreateChildPermission(AppPermissions.Pages_Payment_UpgradeSucceed, L("Pages_Payment_UpgradeSucceed"), multiTenancySides: MultiTenancySides.Tenant);
            payment.CreateChildPermission(AppPermissions.Pages_Payment_ExtendSucceed, L("Pages_Payment_ExtendSucceed"), multiTenancySides: MultiTenancySides.Tenant);
            payment.CreateChildPermission(AppPermissions.Pages_Payment_PaymentFailed, L("Pages_Payment_PaymentFailed"), multiTenancySides: MultiTenancySides.Tenant);
            payment.CreateChildPermission(AppPermissions.Pages_Payment_SwitchBetweenFreeEditions, L("Pages_Payment_SwitchBetweenFreeEditions"), multiTenancySides: MultiTenancySides.Tenant);
            payment.CreateChildPermission(AppPermissions.Pages_Payment_UpgradeSubscriptionCostsLessThenMinAmount, L("Pages_Payment_UpgradeSubscriptionCostsLessThenMinAmount"), multiTenancySides: MultiTenancySides.Tenant);
            payment.CreateChildPermission(AppPermissions.Pages_Payment_HasAnyPayment, L("Pages_Payment_HasAnyPayment"), multiTenancySides: MultiTenancySides.Tenant);

            #endregion

            #region PayPalPayment
            var payPalPayment = pages.CreateChildPermission(AppPermissions.Pages_PayPalPayment, L("PayPalPayment"), multiTenancySides: MultiTenancySides.Tenant);
            payPalPayment.CreateChildPermission(AppPermissions.Pages_PayPalPayment_ConfirmPayment, L("Pages_PayPalPayment_ConfirmPayment"), multiTenancySides: MultiTenancySides.Tenant);
            payPalPayment.CreateChildPermission(AppPermissions.Pages_PayPalPayment_GetConfiguration, L("Pages_PayPalPayment_GetConfiguration"), multiTenancySides: MultiTenancySides.Tenant);
            #endregion


            #region Profile
            var profile = pages.CreateChildPermission(AppPermissions.Pages_Profile, L("Profile"), multiTenancySides: MultiTenancySides.Tenant);
            profile.CreateChildPermission(AppPermissions.Pages_Profile_UploadProfilePicture, L("Pages_Profile_UploadProfilePicture"), multiTenancySides: MultiTenancySides.Tenant);
            profile.CreateChildPermission(AppPermissions.Pages_Profile_GetDefaultProfilePicture, L("Pages_Profile_GetDefaultProfilePicture"), multiTenancySides: MultiTenancySides.Tenant);
            profile.CreateChildPermission(AppPermissions.Pages_Profile_GetCurrentUserProfileForEdit, L("Pages_Profile_GetCurrentUserProfileForEdit"), multiTenancySides: MultiTenancySides.Tenant);
            profile.CreateChildPermission(AppPermissions.Pages_Profile_DisableGoogleAuthenticator, L("Pages_Profile_DisableGoogleAuthenticator"), multiTenancySides: MultiTenancySides.Tenant);
            profile.CreateChildPermission(AppPermissions.Pages_Profile_ViewRecoveryCodes, L("Pages_Profile_ViewRecoveryCodes"), multiTenancySides: MultiTenancySides.Tenant);
            profile.CreateChildPermission(AppPermissions.Pages_Profile_GenerateGoogleAuthenticatorKey, L("Pages_Profile_GenerateGoogleAuthenticatorKey"), multiTenancySides: MultiTenancySides.Tenant);
            profile.CreateChildPermission(AppPermissions.Pages_Profile_UpdateGoogleAuthenticatorKey, L("Pages_Profile_UpdateGoogleAuthenticatorKey"), multiTenancySides: MultiTenancySides.Tenant);
            profile.CreateChildPermission(AppPermissions.Pages_Profile_PrepareCollectedData, L("Pages_Profile_PrepareCollectedData"), multiTenancySides: MultiTenancySides.Tenant);
            profile.CreateChildPermission(AppPermissions.Pages_Profile_UpdateUserProfile, L("Pages_Profile_UpdateUserProfile"), multiTenancySides: MultiTenancySides.Tenant);
            profile.CreateChildPermission(AppPermissions.Pages_Profile_ChangePassword, L("Pages_Profile_ChangePassword"), multiTenancySides: MultiTenancySides.Tenant);
            profile.CreateChildPermission(AppPermissions.Pages_Profile_UpdateProfilePicture, L("Pages_Profile_UpdateProfilePicture"), multiTenancySides: MultiTenancySides.Tenant);
            profile.CreateChildPermission(AppPermissions.Pages_Profile_VerifyAuthenticatorCode, L("Pages_Profile_VerifyAuthenticatorCode"), multiTenancySides: MultiTenancySides.Tenant);
            profile.CreateChildPermission(AppPermissions.Pages_Profile_GetPasswordComplexitySetting, L("Pages_Profile_GetPasswordComplexitySetting"), multiTenancySides: MultiTenancySides.Tenant);
            profile.CreateChildPermission(AppPermissions.Pages_Profile_GetProfilePicture, L("Pages_Profile_GetProfilePicture"), multiTenancySides: MultiTenancySides.Tenant);
            profile.CreateChildPermission(AppPermissions.Pages_Profile_GetProfilePictureByUserName, L("Pages_Profile_GetProfilePictureByUserName"), multiTenancySides: MultiTenancySides.Tenant);
            profile.CreateChildPermission(AppPermissions.Pages_Profile_GetProfilePictureByUser, L("Pages_Profile_GetProfilePictureByUser"), multiTenancySides: MultiTenancySides.Tenant);
            profile.CreateChildPermission(AppPermissions.Pages_Profile_ChangeLanguage, L("Pages_Profile_ChangeLanguage"), multiTenancySides: MultiTenancySides.Tenant);
            #endregion

            #region Timing
            var Timing = pages.CreateChildPermission(AppPermissions.Pages_Timing, L("Pages_Timing"), multiTenancySides: MultiTenancySides.Tenant);
            profile.CreateChildPermission(AppPermissions.Pages_Timing_GetTimezones, L("Pages_Timing_GetTimezones"), multiTenancySides: MultiTenancySides.Tenant);
            profile.CreateChildPermission(AppPermissions.Pages_Timing_GetTimezoneComboboxItems, L("Pages_Timing_GetTimezoneComboboxItems"), multiTenancySides: MultiTenancySides.Tenant);
            #endregion

            #region EsignStatusSignerHistory
            var esignStatusSignerHistory = business.CreateChildPermission(AppPermissions.Pages_Business_EsignStatusSignerHistory, L("Pages_Business_EsignStatusSignerHistory"), multiTenancySides: MultiTenancySides.Tenant);
            esignStatusSignerHistory.CreateChildPermission(AppPermissions.Pages_Business_EsignStatusSignerHistory_GetStatusHistoryByRequestId, L("Pages_Business_EsignStatusSignerHistory_GetStatusHistoryByRequestId"), multiTenancySides: MultiTenancySides.Tenant);
            #endregion

            #region EsignVersionApp
            var esignVersionApp = business.CreateChildPermission(AppPermissions.Pages_Business_EsignVersionApp, L("Pages_Business_EsignVersionApp"), multiTenancySides: MultiTenancySides.Tenant);
            esignVersionApp.CreateChildPermission(AppPermissions.Pages_Business_EsignVersionApp_getEsignVersionApp, L("Pages_Business_EsignVersionApp_getEsignVersionApp"), multiTenancySides: MultiTenancySides.Tenant);
            esignVersionApp.CreateChildPermission(AppPermissions.Pages_Business_EsignVersionApp_CreateEsignVersion, L("Pages_Business_EsignVersionApp_CreateEsignVersion"), multiTenancySides: MultiTenancySides.Tenant);
            #endregion

            #region Friendship
            var friendship = pages.CreateChildPermission(AppPermissions.Pages_Friendship, L("Pages_Friendship"), multiTenancySides: MultiTenancySides.Tenant);
            #endregion

            #region SendEmail
            var sendEmail = pages.CreateChildPermission(AppPermissions.Pages_SendEmail, L("Pages_SendEmail"), multiTenancySides: MultiTenancySides.Tenant);
            #endregion

            #region Session
            var session = pages.CreateChildPermission(AppPermissions.Pages_Session, L("Pages_Session"), multiTenancySides: MultiTenancySides.Tenant);
            #endregion

            #region SettingsAppServiceBase
            var settingsAppServiceBase = pages.CreateChildPermission(AppPermissions.Pages_SettingsAppServiceBase, L("Pages_SettingsAppServiceBase"), multiTenancySides: MultiTenancySides.Tenant);
            #endregion

            #region Stripe
            var stripe = pages.CreateChildPermission(AppPermissions.Pages_Stripe, L("Pages_Stripe"), multiTenancySides: MultiTenancySides.Tenant);
            #endregion

            #region StripeControllerBase
            var stripeControllerBase = pages.CreateChildPermission(AppPermissions.Pages_StripeControllerBase, L("Pages_StripeControllerBase"), multiTenancySides: MultiTenancySides.Tenant);
            #endregion

            #region StripePayment
            var stripePayment = pages.CreateChildPermission(AppPermissions.Pages_StripePayment, L("Pages_StripePayment"), multiTenancySides: MultiTenancySides.Tenant);
            #endregion

            #region Subscription
            var subscription = pages.CreateChildPermission(AppPermissions.Pages_Subscription, L("Pages_Subscription"), multiTenancySides: MultiTenancySides.Tenant);
            #endregion

            #region Install
            var install = pages.CreateChildPermission(AppPermissions.Pages_Install, L("Pages_Install"), multiTenancySides: MultiTenancySides.Tenant);
            install.CreateChildPermission(AppPermissions.Pages_Install_Setup, L("Pages_Install_Setup"), multiTenancySides: MultiTenancySides.Tenant);
            install.CreateChildPermission(AppPermissions.Pages_Install_GetAppSettingsJson, L("Pages_Install_GetAppSettingsJson"), multiTenancySides: MultiTenancySides.Tenant);
            install.CreateChildPermission(AppPermissions.Pages_Install_CheckDatabase, L("Pages_Install_CheckDatabase"), multiTenancySides: MultiTenancySides.Tenant);
            #endregion

            #region Invoice
            var invoice = business.CreateChildPermission(AppPermissions.Pages_Business_Invoice, L("Pages_Business_Invoice"), multiTenancySides: MultiTenancySides.Tenant);
            invoice.CreateChildPermission(AppPermissions.Pages_Business_Invoice_GetInvoiceInfo, L("Pages_Business_Invoice_GetInvoiceInfo"), multiTenancySides: MultiTenancySides.Tenant);
            invoice.CreateChildPermission(AppPermissions.Pages_Business_Invoice_CreateInvoice, L("Pages_Business_Invoice_CreateInvoice"), multiTenancySides: MultiTenancySides.Tenant);
            #endregion


            #endregion

            #region Permission
            var permission = pages.CreateChildPermission(AppPermissions.Pages_Permission, L("Permission"), multiTenancySides: MultiTenancySides.Tenant);
            permission.CreateChildPermission(AppPermissions.Pages_Permission_GetAllPermissions, L("Permission_GetAllPermissions"), multiTenancySides: MultiTenancySides.Tenant);

            #endregion

            #region Master
            var master = pages.CreateChildPermission(AppPermissions.Pages_Master, L("Master"), multiTenancySides: MultiTenancySides.Tenant);

            #region Master Status
            var masterStatus = master.CreateChildPermission(AppPermissions.Pages_Master_StatusApi, L("Pages_Master_StatusApi"), multiTenancySides: MultiTenancySides.Tenant);
            masterStatus.CreateChildPermission(AppPermissions.Pages_Master_StatusApi_View, L("Pages_Master_StatusApi_View"), multiTenancySides: MultiTenancySides.Tenant);
            masterStatus.CreateChildPermission(AppPermissions.Pages_Master_StatusApi_Add, L("Pages_Master_StatusApi_Add"), multiTenancySides: MultiTenancySides.Tenant);
            masterStatus.CreateChildPermission(AppPermissions.Pages_Master_StatusApi_Edit, L("Pages_Master_StatusApi_Edit"), multiTenancySides: MultiTenancySides.Tenant);
            masterStatus.CreateChildPermission(AppPermissions.Pages_Master_StatusApi_Delete, L("Pages_Master_StatusApi_Delete"), multiTenancySides: MultiTenancySides.Tenant);
            masterStatus.CreateChildPermission(AppPermissions.Pages_Master_StatusApi_GetAllStatusByTypeId, L("Pages_Master_StatusApi_GetAllStatusByTypeId"), multiTenancySides: MultiTenancySides.Tenant);
            masterStatus.CreateChildPermission(AppPermissions.Pages_Master_StatusApi_GetAllStatus, L("Pages_Master_StatusApi_GetAllStatus"), multiTenancySides: MultiTenancySides.Tenant);
            #endregion

            #region Status Sidebar
            var statusSidebar = master.CreateChildPermission(AppPermissions.Pages_Master_Status, L("Pages_Master_Status"), multiTenancySides: MultiTenancySides.Tenant);
            #endregion

            #region Master Signature Template
            var masterSignatureTemplate = master.CreateChildPermission(AppPermissions.Pages_Master_SignatureTemplate, L("Pages_Master_SignatureTemplate"), multiTenancySides: MultiTenancySides.Tenant);
            masterSignatureTemplate.CreateChildPermission(AppPermissions.Pages_Master_SignatureTemplate_View, L("Pages_Master_SignatureTemplate_View"), multiTenancySides: MultiTenancySides.Tenant);
            masterSignatureTemplate.CreateChildPermission(AppPermissions.Pages_Master_SignatureTemplate_Add, L("Pages_Master_SignatureTemplate_Add"), multiTenancySides: MultiTenancySides.Tenant);
            masterSignatureTemplate.CreateChildPermission(AppPermissions.Pages_Master_SignatureTemplate_Edit, L("Pages_Master_SignatureTemplate_Edit"), multiTenancySides: MultiTenancySides.Tenant);
            masterSignatureTemplate.CreateChildPermission(AppPermissions.Pages_Master_SignatureTemplate_Delete, L("Pages_Master_SignatureTemplate_Delete"), multiTenancySides: MultiTenancySides.Tenant);
            #endregion

            #region Master System
            var masterSystem = master.CreateChildPermission(AppPermissions.Pages_Master_SystemApi, L("Pages_Master_SystemApi"), multiTenancySides: MultiTenancySides.Tenant);
            masterSystem.CreateChildPermission(AppPermissions.Pages_Master_SystemApi_View, L("Pages_Master_SystemApi_View"), multiTenancySides: MultiTenancySides.Tenant);
            masterSystem.CreateChildPermission(AppPermissions.Pages_Master_SystemApi_Add, L("Pages_Master_SystemApi_Add"), multiTenancySides: MultiTenancySides.Tenant);
            masterSystem.CreateChildPermission(AppPermissions.Pages_Master_SystemApi_Edit, L("Pages_Master_SystemApi_Edit"), multiTenancySides: MultiTenancySides.Tenant);
            masterSystem.CreateChildPermission(AppPermissions.Pages_Master_SystemApi_Delete, L("Pages_Master_SystemApi_Delete"), multiTenancySides: MultiTenancySides.Tenant);
            masterSystem.CreateChildPermission(AppPermissions.Pages_Master_SystemApi_GetSystem, L("Pages_Master_SystemApi_GetSystem"), multiTenancySides: MultiTenancySides.Tenant);
            masterSystem.CreateChildPermission(AppPermissions.Pages_Master_SystemApi_GetAllSystems, L("Pages_Master_SystemApi_GetAllSystems"), multiTenancySides: MultiTenancySides.Tenant);
            #endregion

            #region Master System
            var systemSidebar = master.CreateChildPermission(AppPermissions.Pages_Master_System, L("Pages_Master_System"), multiTenancySides: MultiTenancySides.Tenant);
            #endregion

            #region MstEsignUserImage
            var masterEsignUserImage = master.CreateChildPermission(AppPermissions.Pages_Master_EsignUserImageApi, L("Pages_Master_EsignUserImageApi"), multiTenancySides: MultiTenancySides.Tenant);
            masterSystem.CreateChildPermission(AppPermissions.Pages_Master_EsignUserImageApi_GetListSignatureByUserId, L("Pages_Master_EsignUserImageApi_GetListSignatureByUserId"), multiTenancySides: MultiTenancySides.Tenant);
            masterSystem.CreateChildPermission(AppPermissions.Pages_Master_EsignUserImageApi_GetAllSignature, L("Pages_Master_EsignUserImageApi_GetAllSignature"), multiTenancySides: MultiTenancySides.Tenant);
            masterSystem.CreateChildPermission(AppPermissions.Pages_Master_EsignUserImageApi_GetListSignatureByUserIdForWeb, L("Pages_Master_EsignUserImageApi_GetListSignatureByUserIdForWeb"), multiTenancySides: MultiTenancySides.Tenant);
            masterSystem.CreateChildPermission(AppPermissions.Pages_Master_EsignUserImageApi_UpdateSignatureDefautlForWeb, L("Pages_Master_EsignUserImageApi_UpdateSignatureDefautlForWeb"), multiTenancySides: MultiTenancySides.Tenant);
            masterSystem.CreateChildPermission(AppPermissions.Pages_Master_EsignUserImageApi_SaveImageSignature, L("Pages_Master_EsignUserImageApi_SaveImageSignature"), multiTenancySides: MultiTenancySides.Tenant);
            masterSystem.CreateChildPermission(AppPermissions.Pages_Master_EsignUserImageApi_DeleteTemplateImageSignature, L("Pages_Master_EsignUserImageApi_DeleteTemplateImageSignature"), multiTenancySides: MultiTenancySides.Tenant);
            #endregion

            #region EsignUserImage Sidebar
            var eignUserImageSidebar = master.CreateChildPermission(AppPermissions.Pages_Master_EsignUserImage, L("Pages_Master_EsignUserImage"), multiTenancySides: MultiTenancySides.Tenant);
            #endregion

            //#region Master Active Directory
            //var masterActiveDirectory = master.CreateChildPermission(AppPermissions.Pages_Master_ActiveDirectory, L("Pages_Master_ActiveDirectory"), multiTenancySides: MultiTenancySides.Tenant);
            //masterActiveDirectory.CreateChildPermission(AppPermissions.Pages_Master_ActiveDirectory_View, L("Pages_Master_ActiveDirectory_View"), multiTenancySides: MultiTenancySides.Tenant);
            //masterActiveDirectory.CreateChildPermission(AppPermissions.Pages_Master_ActiveDirectory_Add, L("Pages_Master_ActiveDirectory_Add"), multiTenancySides: MultiTenancySides.Tenant);
            //masterActiveDirectory.CreateChildPermission(AppPermissions.Pages_Master_ActiveDirectory_Edit, L("Pages_Master_ActiveDirectory_Edit"), multiTenancySides: MultiTenancySides.Tenant);
            //masterActiveDirectory.CreateChildPermission(AppPermissions.Pages_Master_ActiveDirectory_Delete, L("Pages_Master_ActiveDirectory_Delete"), multiTenancySides: MultiTenancySides.Tenant);
            //#endregion

            #region Master Category
            var masterCategory = master.CreateChildPermission(AppPermissions.Pages_Master_CategoryApi, L("Pages_Master_CategoryApi"), multiTenancySides: MultiTenancySides.Tenant);
            masterCategory.CreateChildPermission(AppPermissions.Pages_Master_CategoryApi_View, L("Pages_Master_CategoryApi_View"), multiTenancySides: MultiTenancySides.Tenant);
            masterCategory.CreateChildPermission(AppPermissions.Pages_Master_CategoryApi_CreateOrEdit, L("Pages_Master_CategoryApi_CreateOrEdit"), multiTenancySides: MultiTenancySides.Tenant);
            masterCategory.CreateChildPermission(AppPermissions.Pages_Master_CategoryApi_Delete, L("Pages_Master_CategoryApi_Delete"), multiTenancySides: MultiTenancySides.Tenant);
            masterCategory.CreateChildPermission(AppPermissions.Pages_Master_CategoryApi_GetCategoryByName, L("Pages_Master_CategoryApi_GetCategoryByName"), multiTenancySides: MultiTenancySides.Tenant);
            masterCategory.CreateChildPermission(AppPermissions.Pages_Master_CategoryApi_GetAllMstEsignCategories, L("Pages_Master_CategoryApi_GetAllMstEsignCategories"), multiTenancySides: MultiTenancySides.Tenant);
            masterCategory.CreateChildPermission(AppPermissions.Pages_Master_CategoryApi_GetSystem, L("Pages_Master_CategoryApi_GetSystem"), multiTenancySides: MultiTenancySides.Tenant);
            #endregion

            #region Category Sidebar
            var CategorySidebar = master.CreateChildPermission(AppPermissions.Pages_Master_Category, L("Pages_Master_Category"), multiTenancySides: MultiTenancySides.Tenant);

            #endregion

            #region Master Affiliate
            var masterAffiliate = master.CreateChildPermission(AppPermissions.Pages_Master_Affiliate, L("Pages_Master_Affiliate"), multiTenancySides: MultiTenancySides.Tenant);
            masterAffiliate.CreateChildPermission(AppPermissions.Pages_Master_Affiliate_View, L("Pages_Master_Affiliate_View"), multiTenancySides: MultiTenancySides.Tenant);
            masterAffiliate.CreateChildPermission(AppPermissions.Pages_Master_Affiliate_CreateOrEdit, L("Pages_Master_Affiliate_CreateOrEdit"), multiTenancySides: MultiTenancySides.Tenant);
            masterAffiliate.CreateChildPermission(AppPermissions.Pages_Master_Affiliate_Delete, L("Pages_Master_Affiliate_Delete"), multiTenancySides: MultiTenancySides.Tenant);
            masterAffiliate.CreateChildPermission(AppPermissions.Pages_Master_Affiliate_Sync, L("Pages_Master_Affiliate_Sync"), multiTenancySides: MultiTenancySides.Tenant);
            masterAffiliate.CreateChildPermission(AppPermissions.Pages_Master_Affiliate_ReceiveMultiAffiliateUsersInfo, L("Pages_Master_Affiliate_ReceiveMultiAffiliateUsersInfo"), multiTenancySides: MultiTenancySides.Tenant);
            #endregion

            #region Master Division
            var masterDivision = master.CreateChildPermission(AppPermissions.Pages_Master_DivisionApi, L("Pages_Master_DivisionApi"), multiTenancySides: MultiTenancySides.Tenant);
            masterDivision.CreateChildPermission(AppPermissions.Pages_Master_DivisionApi_View, L("Pages_Master_DivisionApi_View"), multiTenancySides: MultiTenancySides.Tenant);
            masterDivision.CreateChildPermission(AppPermissions.Pages_Master_DivisionApi_CreateOrEdit, L("Pages_Master_DivisionApi_CreateOrEdit"), multiTenancySides: MultiTenancySides.Tenant);
            masterDivision.CreateChildPermission(AppPermissions.Pages_Master_DivisionApi_Delete, L("Pages_Master_DivisionApi_Delete"), multiTenancySides: MultiTenancySides.Tenant);
            masterDivision.CreateChildPermission(AppPermissions.Pages_Master_DivisionApi_GetDivisionExcel, L("Pages_Master_DivisionApi_GetDivisionExcel"), multiTenancySides: MultiTenancySides.Tenant);
            #endregion

            #region Division Sidebar
            var divisionSidebar = master.CreateChildPermission(AppPermissions.Pages_Master_Division, L("Pages_Master_Division"), multiTenancySides: MultiTenancySides.Tenant);
            #endregion

            #region Master Department
            var masterDepartment = master.CreateChildPermission(AppPermissions.Pages_Master_Department, L("Pages_Master_Department"), multiTenancySides: MultiTenancySides.Tenant);
            masterDepartment.CreateChildPermission(AppPermissions.Pages_Master_Department_View, L("Pages_Master_Department_View"), multiTenancySides: MultiTenancySides.Tenant);
            masterDepartment.CreateChildPermission(AppPermissions.Pages_Master_Department_CreateOrEdit, L("Pages_Master_Department_CreateOrEdit"), multiTenancySides: MultiTenancySides.Tenant);
            masterDepartment.CreateChildPermission(AppPermissions.Pages_Master_Department_Delete, L("Pages_Master_Department_Delete"), multiTenancySides: MultiTenancySides.Tenant);
            masterDepartment.CreateChildPermission(AppPermissions.Pages_Master_Department_GetDepartmentExcel, L("Pages_Master_Department_GetDepartmentExcel"), multiTenancySides: MultiTenancySides.Tenant);
            #endregion

            #region Master Group Signers
            var masterGroupSigners = master.CreateChildPermission(AppPermissions.Pages_Master_GroupSigners, L("Pages_Master_GroupSigners"), multiTenancySides: MultiTenancySides.Tenant);
            masterGroupSigners.CreateChildPermission(AppPermissions.Pages_Master_GroupSigners_View, L("Pages_Master_GroupSigners_View"), multiTenancySides: MultiTenancySides.Tenant);
            masterGroupSigners.CreateChildPermission(AppPermissions.Pages_Master_GroupSigners_Add, L("Pages_Master_GroupSigners_Add"), multiTenancySides: MultiTenancySides.Tenant);
            masterGroupSigners.CreateChildPermission(AppPermissions.Pages_Master_GroupSigners_Edit, L("Pages_Master_GroupSigners_Edit"), multiTenancySides: MultiTenancySides.Tenant);
            masterGroupSigners.CreateChildPermission(AppPermissions.Pages_Master_GroupSigners_Delete, L("Pages_Master_GroupSigners_Delete"), multiTenancySides: MultiTenancySides.Tenant);
            #endregion

            #region Master Activity
            var masterActivity = master.CreateChildPermission(AppPermissions.Pages_Master_Activity, L("Pages_Master_Activity"), multiTenancySides: MultiTenancySides.Tenant);
            masterActivity.CreateChildPermission(AppPermissions.Pages_Master_Activity_View, L("Pages_Master_Activity_View"), multiTenancySides: MultiTenancySides.Tenant);
            masterActivity.CreateChildPermission(AppPermissions.Pages_Master_Activity_Add, L("Pages_Master_Activity_Add"), multiTenancySides: MultiTenancySides.Tenant);
            masterActivity.CreateChildPermission(AppPermissions.Pages_Master_Activity_Edit, L("Pages_Master_Activity_Edit"), multiTenancySides: MultiTenancySides.Tenant);
            masterActivity.CreateChildPermission(AppPermissions.Pages_Master_Activity_Delete, L("Pages_Master_Activity_Delete"), multiTenancySides: MultiTenancySides.Tenant);
            #endregion

            #region Master Esign Color
            var masterEsignColor = master.CreateChildPermission(AppPermissions.Pages_Master_EsignColorApi, L("Pages_Master_EsignColorApi"), multiTenancySides: MultiTenancySides.Tenant);
            masterEsignColor.CreateChildPermission(AppPermissions.Pages_Master_EsignColorApi_View, L("Pages_Master_EsignColorApi_View"), multiTenancySides: MultiTenancySides.Tenant);
            masterEsignColor.CreateChildPermission(AppPermissions.Pages_Master_EsignColorApi_GetColorExcel, L("Pages_Master_EsignColorApi_GetColorExcel"), multiTenancySides: MultiTenancySides.Tenant);
            masterEsignColor.CreateChildPermission(AppPermissions.Pages_Master_EsignColorApi_CreateOrEdit, L("Pages_Master_EsignColorApi_CreateOrEdit"), multiTenancySides: MultiTenancySides.Tenant);
            masterEsignColor.CreateChildPermission(AppPermissions.Pages_Master_EsignColorApi_Delete, L("Pages_Master_EsignColorApi_Delete"), multiTenancySides: MultiTenancySides.Tenant);
            #endregion

            #region Master Esign Color Sidebar
            var esignColorSidebar = master.CreateChildPermission(AppPermissions.Pages_Master_EsignColor, L("Pages_Master_EsignColor"), multiTenancySides: MultiTenancySides.Tenant);
            #endregion

            #region Master Esign Logo
            var masterEsignLogo = master.CreateChildPermission(AppPermissions.Pages_Master_EsignLogoApi, L("Pages_Master_EsignLogoApi"), multiTenancySides: MultiTenancySides.Tenant);
            masterEsignLogo.CreateChildPermission(AppPermissions.Pages_Master_EsignLogoApi_GetAllLogos, L("Pages_Master_EsignLogoApi_GetAllLogos"), multiTenancySides: MultiTenancySides.Tenant);
            masterEsignLogo.CreateChildPermission(AppPermissions.Pages_Master_EsignLogoApi_Add, L("Pages_Master_EsignLogoApi_Add"), multiTenancySides: MultiTenancySides.Tenant);
            masterEsignLogo.CreateChildPermission(AppPermissions.Pages_Master_EsignLogoApi_Edit, L("Pages_Master_EsignLogoApi_Edit"), multiTenancySides: MultiTenancySides.Tenant);
            masterEsignLogo.CreateChildPermission(AppPermissions.Pages_Master_EsignLogoApi_Delete, L("Pages_Master_EsignLogoApi_Delete"), multiTenancySides: MultiTenancySides.Tenant);
            masterEsignLogo.CreateChildPermission(AppPermissions.Pages_Master_EsignLogoApi_GetAllTenants, L("Pages_Master_EsignLogoApi_GetAllTenants"), multiTenancySides: MultiTenancySides.Tenant);
            masterEsignLogo.CreateChildPermission(AppPermissions.Pages_Master_EsignLogoApi_GetMstEsignLogoByTenant, L("Pages_Master_EsignLogoApi_GetMstEsignLogoByTenant"), multiTenancySides: MultiTenancySides.Tenant);
            #endregion

            #region Esign Logo Sidebar
            var EsignLogoSidebar = master.CreateChildPermission(AppPermissions.Pages_Master_EsignLogo, L("Pages_Master_EsignLogo"), multiTenancySides: MultiTenancySides.Tenant);
            #endregion

            #region MstEsignSignerTemplate
            var masterEsignSignerTemplate = master.CreateChildPermission(AppPermissions.Pages_Master_EsignSignerTemplateApi, L("Pages_Master_EsignSignerTemplateApi"), multiTenancySides: MultiTenancySides.Tenant);
            masterEsignSignerTemplate.CreateChildPermission(AppPermissions.Pages_Master_EsignSignerTemplateApi_GetListTemplateForUser, L("Pages_Master_EsignSignerTemplateApi_GetListTemplateForUser"), multiTenancySides: MultiTenancySides.Tenant);
            masterEsignSignerTemplate.CreateChildPermission(AppPermissions.Pages_Master_EsignSignerTemplateApi_DeleteTemplateForRequester, L("Pages_Master_EsignSignerTemplateApi_DeleteTemplateForRequester"), multiTenancySides: MultiTenancySides.Tenant);
            masterEsignSignerTemplate.CreateChildPermission(AppPermissions.Pages_Master_EsignSignerTemplateApi_GetAllSignatureTemplate, L("Pages_Master_EsignSignerTemplateApi_GetAllSignatureTemplate"), multiTenancySides: MultiTenancySides.Tenant);
            masterEsignSignerTemplate.CreateChildPermission(AppPermissions.Pages_Master_EsignSignerTemplateApi_GetAllSignatureTemplateLinkById, L("Pages_Master_EsignSignerTemplateApi_GetAllSignatureTemplateLinkById"), multiTenancySides: MultiTenancySides.Tenant);
            masterEsignSignerTemplate.CreateChildPermission(AppPermissions.Pages_Master_EsignSignerTemplateApi_GetListTemplateForUserWeb, L("Pages_Master_EsignSignerTemplateApi_GetListTemplateForUserWeb"), multiTenancySides: MultiTenancySides.Tenant);
            #endregion

            #region EsignSignerTemplate Sidebar
            var esignSignerTemplateSidebar = master.CreateChildPermission(AppPermissions.Pages_Master_EsignSignerTemplate, L("Pages_Master_EsignSignerTemplate"), multiTenancySides: MultiTenancySides.Tenant);
            #endregion

            #region Master Esign Config
            var masterEsignConfig = master.CreateChildPermission(AppPermissions.Pages_Master_EsignConfig, L("Pages_Master_EsignConfig"), multiTenancySides: MultiTenancySides.Tenant);
            masterEsignConfig.CreateChildPermission(AppPermissions.Pages_Master_EsignConfig_CreateOrEdit, L("Pages_Master_EsignConfig_CreateOrEdit"), multiTenancySides: MultiTenancySides.Tenant);
            masterEsignConfig.CreateChildPermission(AppPermissions.Pages_Master_EsignConfig_Delete, L("Pages_Master_EsignConfig_Delete"), multiTenancySides: MultiTenancySides.Tenant);
            masterEsignConfig.CreateChildPermission(AppPermissions.Pages_Master_EsignConfig_View, L("Pages_Master_EsignConfig_View"), multiTenancySides: MultiTenancySides.Tenant);
            #endregion

            #region Master Email Config
            var masterEmailConfig = master.CreateChildPermission(AppPermissions.Pages_Master_EmailConfig, L("Pages_Master_EmailConfig"), multiTenancySides: MultiTenancySides.Tenant);
            masterEmailConfig.CreateChildPermission(AppPermissions.Pages_Master_EmailConfig_Add, L("Pages_Master_EmailConfig_Add"), multiTenancySides: MultiTenancySides.Tenant);
            masterEmailConfig.CreateChildPermission(AppPermissions.Pages_Master_EmailConfig_Edit, L("Pages_Master_EmailConfig_Edit"), multiTenancySides: MultiTenancySides.Tenant);
            masterEmailConfig.CreateChildPermission(AppPermissions.Pages_Master_EmailConfig_Delete, L("Pages_Master_EmailConfig_Delete"), multiTenancySides: MultiTenancySides.Tenant);
            #endregion
            #endregion
            #region "Permission Pentest Lam"
            var userLink = pages.CreateChildPermission(AppPermissions.Pages_UserLink, L("Pages_UserLink"), multiTenancySides: MultiTenancySides.Tenant);
            var userDelegation = pages.CreateChildPermission(AppPermissions.Pages_UserDelegation, L("Pages_UserDelegation"), multiTenancySides: MultiTenancySides.Tenant);
            var upload = pages.CreateChildPermission(AppPermissions.Pages_Upload, L("Upload"), multiTenancySides: MultiTenancySides.Tenant);
            var uploadFile = upload.CreateChildPermission(AppPermissions.Pages_Upload_UploadFile, L("Pages_Upload_UploadFile"), multiTenancySides: MultiTenancySides.Tenant);
            var uploadAdditionalFile = upload.CreateChildPermission(AppPermissions.Pages_Upload_UploadAdditionalFile, L("Pages_Upload_UploadAdditionalFile"), multiTenancySides: MultiTenancySides.Tenant);
            var downloadFile = upload.CreateChildPermission(AppPermissions.Pages_Upload_DownloadFile, L("Pages_Upload_DownloadFile"), multiTenancySides: MultiTenancySides.Tenant);
            var uploadSignature = upload.CreateChildPermission(AppPermissions.Pages_Upload_UploadSignature, L("Pages_Upload_UploadSignature"), multiTenancySides: MultiTenancySides.Tenant);
            var uiCustomizationSettings = pages.CreateChildPermission(AppPermissions.Pages_UiCustomizationSettings, L("Pages_UiCustomizationSettings"), multiTenancySides: MultiTenancySides.Tenant);
            var twitter = pages.CreateChildPermission(AppPermissions.Pages_Twitter, L("Pages_Twitter"), multiTenancySides: MultiTenancySides.Tenant);
            var schedulerSync = pages.CreateChildPermission(AppPermissions.Pages_SchedulerSync, L("Pages_SchedulerSync"), multiTenancySides: MultiTenancySides.Tenant);

            #endregion

            #region MstActivityHistory
            var mstActivityHistory = master.CreateChildPermission(AppPermissions.Pages_Master_MstActivityHistory, L("Pages_Master_MstActivityHistory"), multiTenancySides: MultiTenancySides.Tenant);
            mstActivityHistory.CreateChildPermission(AppPermissions.Pages_Master_MstActivityHistory_GetAllActivityHistory, L("Pages_Master_MstActivityHistory_GetAllActivityHistory"), multiTenancySides: MultiTenancySides.Tenant);
            mstActivityHistory.CreateChildPermission(AppPermissions.Pages_Master_MstActivityHistory_GetAllMstEsignActivityHistory, L("Pages_Master_MstActivityHistory_GetAllMstEsignActivityHistory"), multiTenancySides: MultiTenancySides.Tenant);
            mstActivityHistory.CreateChildPermission(AppPermissions.Pages_Master_MstActivityHistory_GetListStatusActivityHistory, L("Pages_Master_MstActivityHistory_GetListStatusActivityHistory"), multiTenancySides: MultiTenancySides.Tenant);
            mstActivityHistory.CreateChildPermission(AppPermissions.Pages_Master_MstActivityHistory_CreateOrEdit, L("Pages_Master_MstActivityHistory_CreateOrEdit"), multiTenancySides: MultiTenancySides.Tenant);
            mstActivityHistory.CreateChildPermission(AppPermissions.Pages_Master_MstActivityHistory_Delete, L("Pages_Master_MstActivityHistory_Delete"), multiTenancySides: MultiTenancySides.Tenant);
            mstActivityHistory.CreateChildPermission(AppPermissions.Pages_Master_MstActivityHistory_GetActivityHistory, L("Pages_Master_MstActivityHistory_GetActivityHistory"), multiTenancySides: MultiTenancySides.Tenant);
            #endregion

            #region MstEsignActiveDirectory
            var mstEsignActiveDirectory = master.CreateChildPermission(AppPermissions.Pages_Master_MstEsignActiveDirectory, L("Pages_Master_MstEsignActiveDirectory"), multiTenancySides: MultiTenancySides.Tenant);
            mstEsignActiveDirectory.CreateChildPermission(AppPermissions.Pages_Master_MstEsignActiveDirectory_GetAllSigners, L("Pages_Master_MstEsignActiveDirectory_GetAllSigners"), multiTenancySides: MultiTenancySides.Tenant);
            mstEsignActiveDirectory.CreateChildPermission(AppPermissions.Pages_Master_MstEsignActiveDirectory_GetAllSignersForWeb, L("Pages_Master_MstEsignActiveDirectory_GetAllSignersForWeb"), multiTenancySides: MultiTenancySides.Tenant);
            mstEsignActiveDirectory.CreateChildPermission(AppPermissions.Pages_Master_MstEsignActiveDirectory_GetAllSignerByGroup, L("Pages_Master_MstEsignActiveDirectory_GetAllSignerByGroup"), multiTenancySides: MultiTenancySides.Tenant);
            mstEsignActiveDirectory.CreateChildPermission(AppPermissions.Pages_Master_MstEsignActiveDirectory_GetMyProfile, L("Pages_Master_MstEsignActiveDirectory_GetMyProfile"), multiTenancySides: MultiTenancySides.Tenant);
            mstEsignActiveDirectory.CreateChildPermission(AppPermissions.Pages_Master_MstEsignActiveDirectory_GetUserInformationById, L("Pages_Master_MstEsignActiveDirectory_GetUserInformationById"), multiTenancySides: MultiTenancySides.Tenant);
            mstEsignActiveDirectory.CreateChildPermission(AppPermissions.Pages_Master_MstEsignActiveDirectory_GetAllActiveDirectory, L("Pages_Master_MstEsignActiveDirectory_GetAllActiveDirectory"), multiTenancySides: MultiTenancySides.Tenant);
            mstEsignActiveDirectory.CreateChildPermission(AppPermissions.Pages_Master_MstEsignActiveDirectory_GetActiveDirectory, L("Pages_Master_MstEsignActiveDirectory_GetActiveDirectory"), multiTenancySides: MultiTenancySides.Tenant);
            mstEsignActiveDirectory.CreateChildPermission(AppPermissions.Pages_Master_MstEsignActiveDirectory_GetMyAccountInfomation, L("Pages_Master_MstEsignActiveDirectory_GetMyAccountInfomation"), multiTenancySides: MultiTenancySides.Tenant);
            mstEsignActiveDirectory.CreateChildPermission(AppPermissions.Pages_Master_MstEsignActiveDirectory_SaveAccountInfomation, L("Pages_Master_MstEsignActiveDirectory_SaveAccountInfomation"), multiTenancySides: MultiTenancySides.Tenant);
            mstEsignActiveDirectory.CreateChildPermission(AppPermissions.Pages_Master_MstEsignActiveDirectory_GetAllSignersForTransfer, L("Pages_Master_MstEsignActiveDirectory_GetAllSignersForTransfer"), multiTenancySides: MultiTenancySides.Tenant);
            #endregion

            #region MstEsignEmailTemplate
            var mstEsignEmailTemplate = master.CreateChildPermission(AppPermissions.Pages_Master_MstEsignEmailTemplate, L("Pages_Master_MstEsignEmailTemplate"), multiTenancySides: MultiTenancySides.Tenant);
            mstEsignEmailTemplate.CreateChildPermission(AppPermissions.Pages_Master_MstEsignEmailTemplate_CreateOrEdit, L("Pages_Master_MstEsignEmailTemplate_CreateOrEdit"), multiTenancySides: MultiTenancySides.Tenant);
            mstEsignEmailTemplate.CreateChildPermission(AppPermissions.Pages_Master_MstEsignEmailTemplate_Delete, L("Pages_Master_MstEsignEmailTemplate_Delete"), multiTenancySides: MultiTenancySides.Tenant);
            mstEsignEmailTemplate.CreateChildPermission(AppPermissions.Pages_Master_MstEsignEmailTemplate_GetAllEmailTemplate, L("Pages_Master_MstEsignEmailTemplate_GetAllEmailTemplate"), multiTenancySides: MultiTenancySides.Tenant);
            #endregion

            #region DynamicProperty
            var dynamicProperty = pages.CreateChildPermission(AppPermissions.Pages_DynamicProperty, L("DynamicProperty"), multiTenancySides: MultiTenancySides.Tenant);
            dynamicProperty.CreateChildPermission(AppPermissions.Pages_DynamicProperty_Get, L("Pages_DynamicProperty_Get"), multiTenancySides: MultiTenancySides.Tenant);
            dynamicProperty.CreateChildPermission(AppPermissions.Pages_DynamicProperty_GetAll, L("Pages_DynamicProperty_GetAll"), multiTenancySides: MultiTenancySides.Tenant);
            dynamicProperty.CreateChildPermission(AppPermissions.Pages_DynamicProperty_Add, L("Pages_DynamicProperty_Add"), multiTenancySides: MultiTenancySides.Tenant);
            dynamicProperty.CreateChildPermission(AppPermissions.Pages_DynamicProperty_Update, L("Pages_DynamicProperty_Update"), multiTenancySides: MultiTenancySides.Tenant);
            dynamicProperty.CreateChildPermission(AppPermissions.Pages_DynamicProperty_Delete, L("Pages_DynamicProperty_Delete"), multiTenancySides: MultiTenancySides.Tenant);
            dynamicProperty.CreateChildPermission(AppPermissions.Pages_DynamicProperty_FindAllowedInputType, L("Pages_DynamicProperty_FindAllowedInputType"), multiTenancySides: MultiTenancySides.Tenant);
            #endregion
            #region DynamicPropertyValue

            var dynamicPropertyValue = pages.CreateChildPermission(AppPermissions.Pages_DynamicPropertyValue, L("DynamicPropertyValue"), multiTenancySides: MultiTenancySides.Tenant);
            dynamicPropertyValue.CreateChildPermission(AppPermissions.Pages_DynamicPropertyValue_Get, L("Pages_DynamicPropertyValue_Get"), multiTenancySides: MultiTenancySides.Tenant);
            dynamicPropertyValue.CreateChildPermission(AppPermissions.Pages_DynamicPropertyValue_GetAllValuesOfDynamicProperty, L("Pages_DynamicPropertyValue_GetAllValuesOfDynamicProperty"), multiTenancySides: MultiTenancySides.Tenant);
            dynamicPropertyValue.CreateChildPermission(AppPermissions.Pages_DynamicPropertyValue_Add, L("Pages_DynamicPropertyValue_Add"), multiTenancySides: MultiTenancySides.Tenant);
            dynamicPropertyValue.CreateChildPermission(AppPermissions.Pages_DynamicPropertyValue_Update, L("Pages_DynamicPropertyValue_Update"), multiTenancySides: MultiTenancySides.Tenant);
            dynamicPropertyValue.CreateChildPermission(AppPermissions.Pages_DynamicPropertyValue_Delete, L("Pages_DynamicPropertyValue_Delete"), multiTenancySides: MultiTenancySides.Tenant);
            #endregion
            #region Edition

            var edition = pages.CreateChildPermission(AppPermissions.Pages_Edition, L("Edition"), multiTenancySides: MultiTenancySides.Tenant);
            edition.CreateChildPermission(AppPermissions.Pages_Edition_GetEditions, L("Pages_Edition_GetEditions"), multiTenancySides: MultiTenancySides.Tenant);
            edition.CreateChildPermission(AppPermissions.Pages_Edition_GetEditionForEdit, L("Pages_Edition_GetEditionForEdit"), multiTenancySides: MultiTenancySides.Tenant);
            edition.CreateChildPermission(AppPermissions.Pages_Edition_CreateEdition, L("Pages_Edition_CreateEdition"), multiTenancySides: MultiTenancySides.Tenant);
            edition.CreateChildPermission(AppPermissions.Pages_Edition_UpdateEdition, L("Pages_Edition_UpdateEdition"), multiTenancySides: MultiTenancySides.Tenant);
            edition.CreateChildPermission(AppPermissions.Pages_Edition_DeleteEdition, L("Pages_Edition_DeleteEdition"), multiTenancySides: MultiTenancySides.Tenant);
            edition.CreateChildPermission(AppPermissions.Pages_Edition_MoveTenantsToAnotherEdition, L("Pages_Edition_MoveTenantsToAnotherEdition"), multiTenancySides: MultiTenancySides.Tenant);
            edition.CreateChildPermission(AppPermissions.Pages_Edition_GetEditionComboboxItems, L("Pages_Edition_GetEditionComboboxItems"), multiTenancySides: MultiTenancySides.Tenant);
            edition.CreateChildPermission(AppPermissions.Pages_Edition_GetTenantCount, L("Pages_Edition_GetTenantCount"), multiTenancySides: MultiTenancySides.Tenant);

            #endregion
            #region EsignActivityHistory

            var esignActivityHistory = pages.CreateChildPermission(AppPermissions.Pages_EsignActivityHistory, L("EsignActivityHistory"), multiTenancySides: MultiTenancySides.Tenant);
            esignActivityHistory.CreateChildPermission(AppPermissions.Pages_EsignActivityHistory_CreateSignerActivity, L("Pages_EsignActivityHistory_CreateSignerActivity"), multiTenancySides: MultiTenancySides.Tenant);
            esignActivityHistory.CreateChildPermission(AppPermissions.Pages_EsignActivityHistory_GetListActivityHistory, L("Pages_EsignActivityHistory_GetListActivityHistory"), multiTenancySides: MultiTenancySides.Tenant);
            esignActivityHistory.CreateChildPermission(AppPermissions.Pages_EsignActivityHistory_GetListActivityHistoryForVerifiedDocument, L("Pages_EsignActivityHistory_GetListActivityHistoryForVerifiedDocument"), multiTenancySides: MultiTenancySides.Tenant);
            esignActivityHistory.CreateChildPermission(AppPermissions.Pages_EsignActivityHistory_GetListActivityHistoryForUser, L("Pages_EsignActivityHistory_GetListActivityHistoryForUser"), multiTenancySides: MultiTenancySides.Tenant);

            #endregion
            #region EsignApiOtherSystem

            var esignApiOtherSystem = pages.CreateChildPermission(AppPermissions.Pages_EsignApiOtherSystem, L("EsignApiOtherSystem"), multiTenancySides: MultiTenancySides.Tenant);
            esignApiOtherSystem.CreateChildPermission(AppPermissions.Pages_EsignApiOtherSystem_CreateOrEditEsignRequestOtherSystem, L("Pages_EsignApiOtherSystem_CreateOrEditEsignRequestOtherSystem"), multiTenancySides: MultiTenancySides.Tenant);
            esignApiOtherSystem.CreateChildPermission(AppPermissions.Pages_EsignApiOtherSystem_ValidateFromOtherSystem, L("Pages_EsignApiOtherSystem_ValidateFromOtherSystem"), multiTenancySides: MultiTenancySides.Tenant);
            esignApiOtherSystem.CreateChildPermission(AppPermissions.Pages_EsignApiOtherSystem_RevokeRequestFromOrtherSystem, L("Pages_EsignApiOtherSystem_RevokeRequestFromOrtherSystem"), multiTenancySides: MultiTenancySides.Tenant);
            esignApiOtherSystem.CreateChildPermission(AppPermissions.Pages_EsignApiOtherSystem_SignDocumentFromOtherSystem, L("Pages_EsignApiOtherSystem_SignDocumentFromOtherSystem"), multiTenancySides: MultiTenancySides.Tenant);
            esignApiOtherSystem.CreateChildPermission(AppPermissions.Pages_EsignApiOtherSystem_RejectRequestFromOtherSystem, L("Pages_EsignApiOtherSystem_RejectRequestFromOtherSystem"), multiTenancySides: MultiTenancySides.Tenant);

            #endregion
            #region EsignComments

            var esignComments = pages.CreateChildPermission(AppPermissions.Pages_EsignComments, L("Pages_EsignComments"), multiTenancySides: MultiTenancySides.Tenant);
            esignComments.CreateChildPermission(AppPermissions.Pages_EsignComments_CreateOrEditEsignComments, L("Pages_EsignComments_CreateOrEditEsignComments"), multiTenancySides: MultiTenancySides.Tenant);
            esignComments.CreateChildPermission(AppPermissions.Pages_EsignComments_GetAllCommentsForRequestId, L("Pages_EsignComments_GetAllCommentsForRequestId"), multiTenancySides: MultiTenancySides.Tenant);
            esignComments.CreateChildPermission(AppPermissions.Pages_EsignComments_GetTotalUnreadComment, L("Pages_EsignComments_GetTotalUnreadComment"), multiTenancySides: MultiTenancySides.Tenant);

            #endregion

            #region EsignDocument

            var esignDocument = pages.CreateChildPermission(AppPermissions.Pages_EsignDocumentList, L("Pages_EsignDocumentList"), multiTenancySides: MultiTenancySides.Tenant);
            esignDocument.CreateChildPermission(AppPermissions.Pages_EsignDocumentList_GetEsignDocumentByRequestIdForRequestInfo, L("Pages_EsignDocumentList_GetEsignDocumentByRequestIdForRequestInfo"), multiTenancySides: MultiTenancySides.Tenant);
            esignDocument.CreateChildPermission(AppPermissions.Pages_EsignDocumentList_GetEsignDocumentByRequestId, L("Pages_EsignDocumentList_GetEsignDocumentByRequestId"), multiTenancySides: MultiTenancySides.Tenant);
            esignDocument.CreateChildPermission(AppPermissions.Pages_EsignDocumentList_UpdateDocumentNameById, L("Pages_EsignDocumentList_UpdateDocumentNameById"), multiTenancySides: MultiTenancySides.Tenant);
            #endregion

            #region EsignDocumentListWWeb

            var esignDocumentListWWeb = pages.CreateChildPermission(AppPermissions.Pages_EsignDocumentListWeb, L("Pages_EsignDocumentListWeb"), multiTenancySides: MultiTenancySides.Tenant);
            esignDocumentListWWeb.CreateChildPermission(AppPermissions.Pages_EsignDocumentListWeb_GetEsignDocumentByRequestIdForRequestInfo, L("Pages_EsignDocumentListWeb_GetEsignDocumentByRequestIdForRequestInfo"), multiTenancySides: MultiTenancySides.Tenant);
            esignDocumentListWWeb.CreateChildPermission(AppPermissions.Pages_EsignDocumentListWeb_GetEsignDocumentByRequestId, L("Pages_EsignDocumentListWeb_GetEsignDocumentByRequestId"), multiTenancySides: MultiTenancySides.Tenant);
            esignDocumentListWWeb.CreateChildPermission(AppPermissions.Pages_EsignDocumentListWeb_UpdateDocumentNameById, L("Pages_EsignDocumentListWeb_UpdateDocumentNameById"));
            #endregion
            #region FeedBackhub

            var feedBackhub = pages.CreateChildPermission(AppPermissions.Pages_EsignFeedbackHub, L("Pages_EsignFeedbackHub"), multiTenancySides: MultiTenancySides.Tenant);
            feedBackhub.CreateChildPermission(AppPermissions.Pages_EsignFeedbackHub_Feedback, L("Pages_EsignFeedbackHub_Feedback"), multiTenancySides: MultiTenancySides.Tenant);
            #endregion
            #region EsignKeywordSearchHistory
            var esignKeywordSearchHistory = pages.CreateChildPermission(AppPermissions.Pages_EsignKeywordSearchHistory, L("Pages_EsignKeywordSearchHistory"), multiTenancySides: MultiTenancySides.Tenant);
            esignKeywordSearchHistory.CreateChildPermission(AppPermissions.Pages_EsignKeywordSearchHistory_GetSearchKeywordHistory, L("Pages_EsignKeywordSearchHistory_GetSearchKeywordHistory"), multiTenancySides: MultiTenancySides.Tenant);
            #endregion
            #region EsignFollowUp
            var esignFollowUp = pages.CreateChildPermission(AppPermissions.Pages_EsignFollowUp, L("Pages_EsignFollowUp"), multiTenancySides: MultiTenancySides.Tenant);
            esignFollowUp.CreateChildPermission(AppPermissions.Pages_EsignFollowUp_FollowUpRequest, L("Pages_EsignFollowUp_FollowUpRequest"), multiTenancySides: MultiTenancySides.Tenant);
            #endregion
            #region EsignReferenceRequest
            var esignReferenceRequest = pages.CreateChildPermission(AppPermissions.Pages_EsignReferenceRequest, L("Pages_EsignReferenceRequest"), multiTenancySides: MultiTenancySides.Tenant);
            esignReferenceRequest.CreateChildPermission(AppPermissions.Pages_EsignReferenceRequest_CreateOrEditReferenceRequest, L("Pages_EsignReferenceRequest_CreateOrEditReferenceRequest"), multiTenancySides: MultiTenancySides.Tenant);
            esignReferenceRequest.CreateChildPermission(AppPermissions.Pages_EsignReferenceRequest_CreateNewReferenceRequest, L("Pages_EsignReferenceRequest_CreateNewReferenceRequest"), multiTenancySides: MultiTenancySides.Tenant);
            esignReferenceRequest.CreateChildPermission(AppPermissions.Pages_EsignReferenceRequest_AddAdditionalFile, L("Pages_EsignReferenceRequest_AddAdditionalFile"), multiTenancySides: MultiTenancySides.Tenant);
            esignReferenceRequest.CreateChildPermission(AppPermissions.Pages_EsignReferenceRequest_DeleteReferenceRequest, L("Pages_EsignReferenceRequest_DeleteReferenceRequest"), multiTenancySides: MultiTenancySides.Tenant);
            esignReferenceRequest.CreateChildPermission(AppPermissions.Pages_EsignReferenceRequest_GetReferenceRequestByRequestId, L("Pages_EsignReferenceRequest_GetReferenceRequestByRequestId"), multiTenancySides: MultiTenancySides.Tenant);
            #endregion

            #region EsignRequest

            var esignRequest = pages.CreateChildPermission(AppPermissions.Pages_EsignRequest, L("EsignRequest"), multiTenancySides: MultiTenancySides.Tenant);
            esignRequest.CreateChildPermission(AppPermissions.Pages_EsignRequest_GetTotalCountRequestsBySystemId, L("Pages_EsignRequest_GetTotalCountRequestsBySystemId"), multiTenancySides: MultiTenancySides.Tenant);
            esignRequest.CreateChildPermission(AppPermissions.Pages_EsignRequest_GetRequestSummaryById, L("Pages_EsignRequest_GetRequestSummaryById"), multiTenancySides: MultiTenancySides.Tenant);
            esignRequest.CreateChildPermission(AppPermissions.Pages_EsignRequest_GetRequestInfomationById, L("Pages_EsignRequest_GetRequestInfomationById"), multiTenancySides: MultiTenancySides.Tenant);
            esignRequest.CreateChildPermission(AppPermissions.Pages_EsignRequest_GetEsignSignaturePageByDocumentId, L("Pages_EsignRequest_GetEsignSignaturePageByDocumentId"), multiTenancySides: MultiTenancySides.Tenant);
            esignRequest.CreateChildPermission(AppPermissions.Pages_EsignRequest_GetEsignSignaturePageByRequestId, L("Pages_EsignRequest_GetEsignSignaturePageByRequestId"), multiTenancySides: MultiTenancySides.Tenant);
            esignRequest.CreateChildPermission(AppPermissions.Pages_EsignRequest_GetEsignPositionsByRequestId, L("Pages_EsignRequest_GetEsignPositionsByRequestId"), multiTenancySides: MultiTenancySides.Tenant);
            esignRequest.CreateChildPermission(AppPermissions.Pages_EsignRequest_GetEsignPositionsByDocumentId, L("Pages_EsignRequest_GetEsignPositionsByDocumentId"), multiTenancySides: MultiTenancySides.Tenant);
            esignRequest.CreateChildPermission(AppPermissions.Pages_EsignRequest_ValidateDigitalSignature, L("Pages_EsignRequest_ValidateDigitalSignature"), multiTenancySides: MultiTenancySides.Tenant);
            esignRequest.CreateChildPermission(AppPermissions.Pages_EsignRequest_CreateOrEditEsignRequest, L("Pages_EsignRequest_CreateOrEditEsignRequest"), multiTenancySides: MultiTenancySides.Tenant);
            esignRequest.CreateChildPermission(AppPermissions.Pages_EsignRequest_GetSignatureOfRequester, L("Pages_EsignRequest_GetSignatureOfRequester"), multiTenancySides: MultiTenancySides.Tenant);
            esignRequest.CreateChildPermission(AppPermissions.Pages_EsignRequest_SaveDraftRequest, L("Pages_EsignRequest_SaveDraftRequest"), multiTenancySides: MultiTenancySides.Tenant);
            esignRequest.CreateChildPermission(AppPermissions.Pages_EsignRequest_GetListRequestsBySystemId, L("Pages_EsignRequest_GetListRequestsBySystemId"), multiTenancySides: MultiTenancySides.Tenant);
            esignRequest.CreateChildPermission(AppPermissions.Pages_EsignRequest_GetListRequestsBySearchValue, L("Pages_EsignRequest_GetListRequestsBySearchValue"), multiTenancySides: MultiTenancySides.Tenant);
            esignRequest.CreateChildPermission(AppPermissions.Pages_EsignRequest_GetMessageConfirmFinishAddSigner, L("Pages_EsignRequest_GetMessageConfirmFinishAddSigner"), multiTenancySides: MultiTenancySides.Tenant);
            esignRequest.CreateChildPermission(AppPermissions.Pages_EsignRequest_EsignRequerstCreateField, L("Pages_EsignRequest_EsignRequerstCreateField"), multiTenancySides: MultiTenancySides.Tenant);
            esignRequest.CreateChildPermission(AppPermissions.Pages_EsignRequest_SignerSign, L("Pages_EsignRequest_SignerSign"), multiTenancySides: MultiTenancySides.Tenant);
            esignRequest.CreateChildPermission(AppPermissions.Pages_EsignRequest_ShareRequest, L("Pages_EsignRequest_ShareRequest"), multiTenancySides: MultiTenancySides.Tenant);
            esignRequest.CreateChildPermission(AppPermissions.Pages_EsignRequest_ShareRequest_Web, L("Pages_EsignRequest_ShareRequest_Web"), multiTenancySides: MultiTenancySides.Tenant);
            esignRequest.CreateChildPermission(AppPermissions.Pages_EsignRequest_CreateRemindRequestDto, L("Pages_EsignRequest_CreateRemindRequestDto"), multiTenancySides: MultiTenancySides.Tenant);
            esignRequest.CreateChildPermission(AppPermissions.Pages_EsignRequest_DeleteDraftRequest, L("Pages_EsignRequest_DeleteDraftRequest"), multiTenancySides: MultiTenancySides.Tenant);


            #endregion

            #region EsignRequestMultiAffiliate

            var esignRequestMultiAffiliate = pages.CreateChildPermission(AppPermissions.Pages_EsignRequestMultiAffiliate, L("EsignRequestMultiAffiliate"), multiTenancySides: MultiTenancySides.Tenant);
            esignRequestMultiAffiliate.CreateChildPermission(AppPermissions.Pages_EsignRequestMultiAffiliate_GetMultiAffiliateAuthenToken, L("Pages_EsignRequestMultiAffiliate_GetMultiAffiliateAuthenToken"), multiTenancySides: MultiTenancySides.Tenant);
            esignRequestMultiAffiliate.CreateChildPermission(AppPermissions.Pages_EsignRequestMultiAffiliate_GetMultiAffiliateUsersInfo, L("Pages_EsignRequestMultiAffiliate_GetMultiAffiliateUsersInfo"), multiTenancySides: MultiTenancySides.Tenant);
            esignRequestMultiAffiliate.CreateChildPermission(AppPermissions.Pages_EsignRequestMultiAffiliate_ReceiveMultiAffiliateUsersInfo, L("Pages_EsignRequestMultiAffiliate_ReceiveMultiAffiliateUsersInfo"), multiTenancySides: MultiTenancySides.Tenant);
            esignRequestMultiAffiliate.CreateChildPermission(AppPermissions.Pages_EsignRequestMultiAffiliate_GetRequestForMultiAffiliate, L("Pages_EsignRequestMultiAffiliate_GetRequestForMultiAffiliate"), multiTenancySides: MultiTenancySides.Tenant);
            esignRequestMultiAffiliate.CreateChildPermission(AppPermissions.Pages_EsignRequestMultiAffiliate_SendMultiAffiliateEsignRequest, L("Pages_EsignRequestMultiAffiliate_SendMultiAffiliateEsignRequest"), multiTenancySides: MultiTenancySides.Tenant);
            esignRequestMultiAffiliate.CreateChildPermission(AppPermissions.Pages_EsignRequestMultiAffiliate_CreateMultiAffiliateRequest, L("Pages_EsignRequestMultiAffiliate_CreateMultiAffiliateRequest"), multiTenancySides: MultiTenancySides.Tenant);
            esignRequestMultiAffiliate.CreateChildPermission(AppPermissions.Pages_EsignRequestMultiAffiliate_GetRequestForMultiAffiliateSigningInfo, L("Pages_EsignRequestMultiAffiliate_GetRequestForMultiAffiliateSigningInfo"), multiTenancySides: MultiTenancySides.Tenant);
            esignRequestMultiAffiliate.CreateChildPermission(AppPermissions.Pages_EsignRequestMultiAffiliate_SendMultiAffiliateEsignRequestSigningInfo, L("Pages_EsignRequestMultiAffiliate_SendMultiAffiliateEsignRequestSigningInfo"), multiTenancySides: MultiTenancySides.Tenant);
            esignRequestMultiAffiliate.CreateChildPermission(AppPermissions.Pages_EsignRequestMultiAffiliate_UpdateMultiAffiliateRequestSigningInfo, L("Pages_EsignRequestMultiAffiliate_UpdateMultiAffiliateRequestSigningInfo"), multiTenancySides: MultiTenancySides.Tenant);
            esignRequestMultiAffiliate.CreateChildPermission(AppPermissions.Pages_EsignRequestMultiAffiliate_GetRequestForMultiAffiliateRejectInfo, L("Pages_EsignRequestMultiAffiliate_GetRequestForMultiAffiliateRejectInfo"), multiTenancySides: MultiTenancySides.Tenant);
            esignRequestMultiAffiliate.CreateChildPermission(AppPermissions.Pages_EsignRequestMultiAffiliate_SendMultiAffiliateEsignRequestRejectInfo, L("Pages_EsignRequestMultiAffiliate_SendMultiAffiliateEsignRequestRejectInfo"), multiTenancySides: MultiTenancySides.Tenant);
            esignRequestMultiAffiliate.CreateChildPermission(AppPermissions.Pages_EsignRequestMultiAffiliate_UpdateMultiAffiliateRequestRejectInfo, L("Pages_EsignRequestMultiAffiliate_UpdateMultiAffiliateRequestRejectInfo"), multiTenancySides: MultiTenancySides.Tenant);
            esignRequestMultiAffiliate.CreateChildPermission(AppPermissions.Pages_EsignRequestMultiAffiliate_GetRequestForMultiAffiliateCommentInfo, L("Pages_EsignRequestMultiAffiliate_GetRequestForMultiAffiliateCommentInfo"), multiTenancySides: MultiTenancySides.Tenant);
            esignRequestMultiAffiliate.CreateChildPermission(AppPermissions.Pages_EsignRequestMultiAffiliate_SendMultiAffiliateEsignRequestCommentInfo, L("Pages_EsignRequestMultiAffiliate_SendMultiAffiliateEsignRequestCommentInfo"), multiTenancySides: MultiTenancySides.Tenant);
            esignRequestMultiAffiliate.CreateChildPermission(AppPermissions.Pages_EsignRequestMultiAffiliate_UpdateMultiAffiliateRequestCommentInfo, L("Pages_EsignRequestMultiAffiliate_UpdateMultiAffiliateRequestCommentInfo"), multiTenancySides: MultiTenancySides.Tenant);
            esignRequestMultiAffiliate.CreateChildPermission(AppPermissions.Pages_EsignRequestMultiAffiliate_GetRequestForMultiAffiliateReassignInfo, L("Pages_EsignRequestMultiAffiliate_GetRequestForMultiAffiliateReassignInfo"), multiTenancySides: MultiTenancySides.Tenant);
            esignRequestMultiAffiliate.CreateChildPermission(AppPermissions.Pages_EsignRequestMultiAffiliate_SendMultiAffiliateEsignRequestReassignInfo, L("Pages_EsignRequestMultiAffiliate_SendMultiAffiliateEsignRequestReassignInfo"), multiTenancySides: MultiTenancySides.Tenant);
            esignRequestMultiAffiliate.CreateChildPermission(AppPermissions.Pages_EsignRequestMultiAffiliate_UpdateMultiAffiliateRequestReassignInfo, L("Pages_EsignRequestMultiAffiliate_UpdateMultiAffiliateRequestReassignInfo"), multiTenancySides: MultiTenancySides.Tenant);
            esignRequestMultiAffiliate.CreateChildPermission(AppPermissions.Pages_EsignRequestMultiAffiliate_GetRequestForMultiAffiliateRemindInfo, L("Pages_EsignRequestMultiAffiliate_GetRequestForMultiAffiliateRemindInfo"), multiTenancySides: MultiTenancySides.Tenant);
            esignRequestMultiAffiliate.CreateChildPermission(AppPermissions.Pages_EsignRequestMultiAffiliate_SendMultiAffiliateEsignRequestRemindInfo, L("Pages_EsignRequestMultiAffiliate_SendMultiAffiliateEsignRequestRemindInfo"), multiTenancySides: MultiTenancySides.Tenant);
            esignRequestMultiAffiliate.CreateChildPermission(AppPermissions.Pages_EsignRequestMultiAffiliate_UpdateMultiAffiliateRequestRemindInfo, L("Pages_EsignRequestMultiAffiliate_UpdateMultiAffiliateRequestRemindInfo"), multiTenancySides: MultiTenancySides.Tenant);
            esignRequestMultiAffiliate.CreateChildPermission(AppPermissions.Pages_EsignRequestMultiAffiliate_GetRequestForMultiAffiliateRevokeInfo, L("Pages_EsignRequestMultiAffiliate_GetRequestForMultiAffiliateRevokeInfo"), multiTenancySides: MultiTenancySides.Tenant);
            esignRequestMultiAffiliate.CreateChildPermission(AppPermissions.Pages_EsignRequestMultiAffiliate_SendMultiAffiliateEsignRequestRevokeInfo, L("Pages_EsignRequestMultiAffiliate_SendMultiAffiliateEsignRequestRevokeInfo"), multiTenancySides: MultiTenancySides.Tenant);
            esignRequestMultiAffiliate.CreateChildPermission(AppPermissions.Pages_EsignRequestMultiAffiliate_UpdateMultiAffiliateRequestRevokeInfo, L("Pages_EsignRequestMultiAffiliate_UpdateMultiAffiliateRequestRevokeInfo"), multiTenancySides: MultiTenancySides.Tenant);
            esignRequestMultiAffiliate.CreateChildPermission(AppPermissions.Pages_EsignRequestMultiAffiliate_GetRequestForMultiAffiliateTransferInfo, L("Pages_EsignRequestMultiAffiliate_GetRequestForMultiAffiliateTransferInfo"), multiTenancySides: MultiTenancySides.Tenant);
            esignRequestMultiAffiliate.CreateChildPermission(AppPermissions.Pages_EsignRequestMultiAffiliate_SendMultiAffiliateEsignRequestTransferInfo, L("Pages_EsignRequestMultiAffiliate_SendMultiAffiliateEsignRequestTransferInfo"), multiTenancySides: MultiTenancySides.Tenant);
            esignRequestMultiAffiliate.CreateChildPermission(AppPermissions.Pages_EsignRequestMultiAffiliate_UpdateMultiAffiliateRequestTransferInfo, L("Pages_EsignRequestMultiAffiliate_UpdateMultiAffiliateRequestTransferInfo"), multiTenancySides: MultiTenancySides.Tenant);
            esignRequestMultiAffiliate.CreateChildPermission(AppPermissions.Pages_EsignRequestMultiAffiliate_GetRequestForMultiAffiliateShareInfo, L("Pages_EsignRequestMultiAffiliate_GetRequestForMultiAffiliateShareInfo"), multiTenancySides: MultiTenancySides.Tenant);
            esignRequestMultiAffiliate.CreateChildPermission(AppPermissions.Pages_EsignRequestMultiAffiliate_SendMultiAffiliateEsignRequestShareInfo, L("Pages_EsignRequestMultiAffiliate_SendMultiAffiliateEsignRequestShareInfo"), multiTenancySides: MultiTenancySides.Tenant);
            esignRequestMultiAffiliate.CreateChildPermission(AppPermissions.Pages_EsignRequestMultiAffiliate_UpdateMultiAffiliateRequestShareInfo, L("Pages_EsignRequestMultiAffiliate_UpdateMultiAffiliateRequestShareInfo"), multiTenancySides: MultiTenancySides.Tenant);
            esignRequestMultiAffiliate.CreateChildPermission(AppPermissions.Pages_EsignRequestMultiAffiliate_GetRequestForMultiAffiliateAdditionFileInfo, L("Pages_EsignRequestMultiAffiliate_GetRequestForMultiAffiliateAdditionFileInfo"), multiTenancySides: MultiTenancySides.Tenant);
            esignRequestMultiAffiliate.CreateChildPermission(AppPermissions.Pages_EsignRequestMultiAffiliate_SendMultiAffiliateEsignRequestAdditionFileInfo, L("Pages_EsignRequestMultiAffiliate_SendMultiAffiliateEsignRequestAdditionFileInfo"), multiTenancySides: MultiTenancySides.Tenant);
            esignRequestMultiAffiliate.CreateChildPermission(AppPermissions.Pages_EsignRequestMultiAffiliate_UpdateMultiAffiliateRequestAdditionFileInfo, L("Pages_EsignRequestMultiAffiliate_UpdateMultiAffiliateRequestAdditionFileInfo"), multiTenancySides: MultiTenancySides.Tenant);
            #endregion
			#region Permission 22 May 2024
            #region Account Group
            var account = pages.CreateChildPermission(AppPermissions.Pages_Account, L("Pages_Account"), multiTenancySides: MultiTenancySides.Tenant);
            account.CreateChildPermission(AppPermissions.Pages_Account_Register, L("Register"), multiTenancySides: MultiTenancySides.Tenant);
            account.CreateChildPermission(AppPermissions.Pages_Account_ForgotPassword, L("ForgotPassword"), multiTenancySides: MultiTenancySides.Tenant);
            account.CreateChildPermission(AppPermissions.Pages_Account_ResetPassword, L("ResetPassword"), multiTenancySides: MultiTenancySides.Tenant);
            account.CreateChildPermission(AppPermissions.Pages_Account_SendEmailActivationLink, L("SendEmailActivationLink"), multiTenancySides: MultiTenancySides.Tenant);
            account.CreateChildPermission(AppPermissions.Pages_Account_ActivateEmail, L("Pages_Account_ActivateEmail"), multiTenancySides: MultiTenancySides.Tenant);
            account.CreateChildPermission(AppPermissions.Pages_Account_DelegatedImpersonate, L("DelegatedImpersonate"), multiTenancySides: MultiTenancySides.Tenant);
            account.CreateChildPermission(AppPermissions.Pages_Account_BackToImpersonator, L("BackToImpersonator"), multiTenancySides: MultiTenancySides.Tenant);
            account.CreateChildPermission(AppPermissions.Pages_Account_SwitchToLinkedAccount, L("Pages_Account_SwitchToLinkedAccount"), multiTenancySides: MultiTenancySides.Tenant);
            #endregion
            #region Cache Group
            var cache = pages.CreateChildPermission(AppPermissions.Pages_Cache, L("Caching"));
            cache.CreateChildPermission(AppPermissions.Pages_Cache_GetAllCaches, L("GetAllCaches"));
            cache.CreateChildPermission(AppPermissions.Pages_Cache_ClearCache, L("ClearCache"));
            cache.CreateChildPermission(AppPermissions.Pages_Cache_ClearAllCaches, L("ClearAllCaches"));
            #endregion
            #region Chat Group
            var chat = pages.CreateChildPermission(AppPermissions.Pages_Chat, L("Chat"));
            chat.CreateChildPermission(AppPermissions.Pages_Chat_GetUserChatFriendsWithSettings, L("GetUserChatFriendsWithSettings"));
            chat.CreateChildPermission(AppPermissions.Pages_Chat_GetUserChatMessages, L("GetUserChatMessages"));
            chat.CreateChildPermission(AppPermissions.Pages_Chat_MarkAllUnreadMessagesOfUserAsRead, L("MarkAllUnreadMessagesOfUserAsRead"));
            #endregion
            #region CommonCallApiOtherSystem Group
            var commonCallApiOtherSystem = pages.CreateChildPermission(AppPermissions.Pages_CommonCallApiOtherSystem, L("CommonCallApiOtherSystem"));
            commonCallApiOtherSystem.CreateChildPermission(AppPermissions.Pages_CommonCallApiOtherSystem_UpdateResultForOtherSystem, L("UpdateResultForOtherSystem"));
            commonCallApiOtherSystem.CreateChildPermission(AppPermissions.Pages_CommonCallApiOtherSystem_UpdateReassignForOtherSystem, L("UpdateReassignForOtherSystem"));
            commonCallApiOtherSystem.CreateChildPermission(AppPermissions.Pages_CommonCallApiOtherSystem_GetOtherSystemAuthenToken, L("GetOtherSystemAuthenToken"));
            #endregion
            #region CommonEsign Group
            var commonEsign = pages.CreateChildPermission(AppPermissions.Pages_CommonEsign, L("CommonEsign"));
            commonEsign.CreateChildPermission(AppPermissions.Pages_CommonEsign_SignDocument, L("SignDocument"));
            commonEsign.CreateChildPermission(AppPermissions.Pages_CommonEsign_ConvertTypeSignature, L("ConvertTypeSignature"));
            commonEsign.CreateChildPermission(AppPermissions.Pages_CommonEsign_SignDigitalToPdf, L("SignDigitalToPdf"));
            commonEsign.CreateChildPermission(AppPermissions.Pages_CommonEsign_AddImageToPdf, L("AddImageToPdf"));
            commonEsign.CreateChildPermission(AppPermissions.Pages_CommonEsign_AddImageToPdfWithDigitalSign, L("AddImageToPdfWithDigitalSign"));
            commonEsign.CreateChildPermission(AppPermissions.Pages_CommonEsign_GetInformation, L("GetInformation"));
            commonEsign.CreateChildPermission(AppPermissions.Pages_CommonEsign_GetFilePath, L("GetFilePath"));
            commonEsign.CreateChildPermission(AppPermissions.Pages_CommonEsign_GetFY, L("GetFY"));
            commonEsign.CreateChildPermission(AppPermissions.Pages_CommonEsign_RequestNextApproveForSign, L("RequestNextApproveForSign"));
            commonEsign.CreateChildPermission(AppPermissions.Pages_CommonEsign_RequestNextApprove, L("RequestNextApprove"));
            commonEsign.CreateChildPermission(AppPermissions.Pages_CommonEsign_RequestNextApproveV2, L("RequestNextApproveV2"));
            commonEsign.CreateChildPermission(AppPermissions.Pages_CommonEsign_SendEmailEsignRequest, L("SendEmailEsignRequest"));
            commonEsign.CreateChildPermission(AppPermissions.Pages_CommonEsign_SendEmailEsignRequest_v21, L("SendEmailEsignRequest_v21"));
            commonEsign.CreateChildPermission(AppPermissions.Pages_CommonEsign_SendNoti, L("SendNoti"));
            commonEsign.CreateChildPermission(AppPermissions.Pages_CommonEsign_ResizeImage, L("ResizeImage"));
            commonEsign.CreateChildPermission(AppPermissions.Pages_CommonEsign_CreateEsignNotification, L("CreateEsignNotification"));
            commonEsign.CreateChildPermission(AppPermissions.Pages_CommonEsign_UpdateResultForOtherSystem, L("UpdateResultForOtherSystem"));
            commonEsign.CreateChildPermission(AppPermissions.Pages_CommonEsign_GetOtherSystemAuthenToken, L("GetOtherSystemAuthenToken"));
            #endregion

            #region CommonEsignWeb Group
            var commonEsignWeb = pages.CreateChildPermission(AppPermissions.Pages_CommonEsignWeb, L("CommonEsignWeb"));
            commonEsignWeb.CreateChildPermission(AppPermissions.Pages_CommonEsignWeb_SignDocument, L("SignDocument"));
            commonEsignWeb.CreateChildPermission(AppPermissions.Pages_CommonEsignWeb_SignDocumentWithOtp, L("SignDocumentWithOtp"));
            commonEsignWeb.CreateChildPermission(AppPermissions.Pages_CommonEsignWeb_AuthorizeOTP, L("AuthorizeOTP"));
            commonEsignWeb.CreateChildPermission(AppPermissions.Pages_CommonEsignWeb_SignDigitalToPdf, L("SignDigitalToPdf"));
            commonEsignWeb.CreateChildPermission(AppPermissions.Pages_CommonEsignWeb_AddImageToPdf, L("AddImageToPdf"));
            commonEsignWeb.CreateChildPermission(AppPermissions.Pages_CommonEsignWeb_AddImageToPdfWithDigitalSign, L("AddImageToPdfWithDigitalSign"));
            commonEsignWeb.CreateChildPermission(AppPermissions.Pages_CommonEsignWeb_GetInformation, L("GetInformation"));
            commonEsignWeb.CreateChildPermission(AppPermissions.Pages_CommonEsignWeb_GetFilePath, L("GetFilePath"));
            commonEsignWeb.CreateChildPermission(AppPermissions.Pages_CommonEsignWeb_GetFY, L("GetFY"));
            commonEsignWeb.CreateChildPermission(AppPermissions.Pages_CommonEsignWeb_RequestNextApproveForSign, L("RequestNextApproveForSign"));
            commonEsignWeb.CreateChildPermission(AppPermissions.Pages_CommonEsignWeb_RequestNextApprove, L("RequestNextApprove"));
            commonEsignWeb.CreateChildPermission(AppPermissions.Pages_CommonEsignWeb_RequestNextApproveV2, L("RequestNextApproveV2"));
            commonEsignWeb.CreateChildPermission(AppPermissions.Pages_CommonEsignWeb_SendEmailEsignRequest, L("SendEmailEsignRequest"));
            commonEsignWeb.CreateChildPermission(AppPermissions.Pages_CommonEsignWeb_SendEmailWithContet, L("SendEmailWithContet"));
            commonEsignWeb.CreateChildPermission(AppPermissions.Pages_CommonEsignWeb_SendNoti, L("SendNoti"));
            commonEsignWeb.CreateChildPermission(AppPermissions.Pages_CommonEsignWeb_SendEmailEsignRequest_v21, L("SendEmailEsignRequest_v21"));
            commonEsignWeb.CreateChildPermission(AppPermissions.Pages_CommonEsignWeb_ResizeImage, L("ResizeImage"));
            commonEsignWeb.CreateChildPermission(AppPermissions.Pages_CommonEsignWeb_CreateEsignNotification, L("CreateEsignNotification"));
            commonEsignWeb.CreateChildPermission(AppPermissions.Pages_CommonEsignWeb_TestSignSync, L("TestSignSync"));
            #endregion

            #region CommonLookup Group
            var commonLookup = pages.CreateChildPermission(AppPermissions.Pages_CommonLookup, L("CommonLookup"));
            commonLookup.CreateChildPermission(AppPermissions.Pages_CommonLookup_GetEditionsForCombobox, L("GetEditionsForCombobox"));
            commonLookup.CreateChildPermission(AppPermissions.Pages_CommonLookup_FindUsers, L("FindUsers"));
            commonLookup.CreateChildPermission(AppPermissions.Pages_CommonLookup_GetDefaultEditionName, L("GetDefaultEditionName"));
            #endregion

            #region DashboardCustomization Group
            var dashboardCustomization = pages.CreateChildPermission(AppPermissions.Pages_DashboardCustomization, L("DashboardCustomization"));
            dashboardCustomization.CreateChildPermission(AppPermissions.Pages_DashboardCustomization_GetUserDashboard, L("GetUserDashboard"));
            dashboardCustomization.CreateChildPermission(AppPermissions.Pages_DashboardCustomization_SavePage, L("SavePage"));
            dashboardCustomization.CreateChildPermission(AppPermissions.Pages_DashboardCustomization_RenamePage, L("RenamePage"));
            dashboardCustomization.CreateChildPermission(AppPermissions.Pages_DashboardCustomization_AddNewPage, L("AddNewPage"));
            dashboardCustomization.CreateChildPermission(AppPermissions.Pages_DashboardCustomization_DeletePage, L("DeletePage"));
            dashboardCustomization.CreateChildPermission(AppPermissions.Pages_DashboardCustomization_AddWidget, L("AddWidget"));
            dashboardCustomization.CreateChildPermission(AppPermissions.Pages_DashboardCustomization_GetDashboardDefinition, L("GetDashboardDefinition"));
            dashboardCustomization.CreateChildPermission(AppPermissions.Pages_DashboardCustomization_GetAllWidgetDefinitions, L("GetAllWidgetDefinitions"));
            dashboardCustomization.CreateChildPermission(AppPermissions.Pages_DashboardCustomization_GetAllAvailableWidgetDefinitionsForPage, L("GetAllAvailableWidgetDefinitionsForPage"));
            dashboardCustomization.CreateChildPermission(AppPermissions.Pages_DashboardCustomization_GetSettingName, L("GetSettingName"));
            #endregion

            #region Pages_PdfViewer
            var PdfViewer = pages.CreateChildPermission(AppPermissions.Pages_PdfViewer, L("Pages_PdfViewer"));
            PdfViewer.CreateChildPermission(AppPermissions.Pages_PdfViewer_Load, L("Pages_PdfViewer_Load"));
            PdfViewer.CreateChildPermission(AppPermissions.Pages_PdfViewer_Bookmarks, L("Pages_PdfViewer_Bookmarks"));
            PdfViewer.CreateChildPermission(AppPermissions.Pages_PdfViewer_RenderPdfPages, L("Pages_PdfViewer_RenderPdfPages"));
            PdfViewer.CreateChildPermission(AppPermissions.Pages_PdfViewer_RenderPdfTexts, L("Pages_PdfViewer_RenderPdfTexts"));
            PdfViewer.CreateChildPermission(AppPermissions.Pages_PdfViewer_RenderThumbnailImages, L("Pages_PdfViewer_RenderThumbnailImages"));
            PdfViewer.CreateChildPermission(AppPermissions.Pages_PdfViewer_RenderAnnotationComments, L("Pages_PdfViewer_RenderAnnotationComments"));
            PdfViewer.CreateChildPermission(AppPermissions.Pages_PdfViewer_ExportAnnotations, L("Pages_PdfViewer_ExportAnnotations"));
            PdfViewer.CreateChildPermission(AppPermissions.Pages_PdfViewer_ImportAnnotations, L("Pages_PdfViewer_ImportAnnotations"));
            PdfViewer.CreateChildPermission(AppPermissions.Pages_PdfViewer_ExportFormFields, L("Pages_PdfViewer_ExportFormFields"));
            PdfViewer.CreateChildPermission(AppPermissions.Pages_PdfViewer_ImportFormFields, L("Pages_PdfViewer_ImportFormFields"));
            PdfViewer.CreateChildPermission(AppPermissions.Pages_PdfViewer_Unload, L("Pages_PdfViewer_Unload"));
            PdfViewer.CreateChildPermission(AppPermissions.Pages_PdfViewer_Download, L("Pages_PdfViewer_Download"));
            PdfViewer.CreateChildPermission(AppPermissions.Pages_PdfViewer_PrintImages, L("Pages_PdfViewer_PrintImages"));
            #endregion

            #region DynamicEntityProperty
            var dynamicEntityProperty = pages.CreateChildPermission(AppPermissions.Pages_DynamicEntityProperty, L("DynamicEntityProperty"));
            dynamicEntityProperty.CreateChildPermission(AppPermissions.Pages_DynamicEntityProperty_Get, L("Get"));
            dynamicEntityProperty.CreateChildPermission(AppPermissions.Pages_DynamicEntityProperty_GetAllPropertiesOfAnEntity, L("GetAllPropertiesOfAnEntity"));
            dynamicEntityProperty.CreateChildPermission(AppPermissions.Pages_DynamicEntityProperty_GetAll, L("GetAll"));
            dynamicEntityProperty.CreateChildPermission(AppPermissions.Pages_DynamicEntityProperty_Add, L("Add"));
            dynamicEntityProperty.CreateChildPermission(AppPermissions.Pages_DynamicEntityProperty_Update, L("Update"));
            dynamicEntityProperty.CreateChildPermission(AppPermissions.Pages_DynamicEntityProperty_Delete, L("Delete"));
            dynamicEntityProperty.CreateChildPermission(AppPermissions.Pages_DynamicEntityProperty_GetAllEntitiesHasDynamicProperty, L("GetAllEntitiesHasDynamicProperty"));
            #endregion

            #region DynamicEntityPropertyDefinition
            pages.CreateChildPermission(AppPermissions.Pages_DynamicEntityPropertyDefinition, L("DynamicEntityPropertyDefinition"));
            #endregion

            #region DynamicEntityPropertyValue
            var dynamicEntityPropertyValue = pages.CreateChildPermission(AppPermissions.Pages_DynamicEntityPropertyValue, L("DynamicEntityPropertyValue"));
            dynamicEntityPropertyValue.CreateChildPermission(AppPermissions.Pages_DynamicEntityPropertyValue_Add, L("Add"));
            dynamicEntityPropertyValue.CreateChildPermission(AppPermissions.Pages_DynamicEntityPropertyValue_Update, L("Update"));
            dynamicEntityPropertyValue.CreateChildPermission(AppPermissions.Pages_DynamicEntityPropertyValue_Delete, L("Delete"));
            dynamicEntityPropertyValue.CreateChildPermission(AppPermissions.Pages_DynamicEntityPropertyValue_GetAllDynamicEntityPropertyValues, L("GetAllDynamicEntityPropertyValues"));
            dynamicEntityPropertyValue.CreateChildPermission(AppPermissions.Pages_DynamicEntityPropertyValue_InsertOrUpdateAllValues, L("InsertOrUpdateAllValues"));
            dynamicEntityPropertyValue.CreateChildPermission(AppPermissions.Pages_DynamicEntityPropertyValue_CleanValues, L("CleanValues"));
            #endregion
            #endregion

        }
        private static ILocalizableString L(string name)
        {
            return new LocalizableString(name, esignConsts.LocalizationSourceName);
        }
    }
}

