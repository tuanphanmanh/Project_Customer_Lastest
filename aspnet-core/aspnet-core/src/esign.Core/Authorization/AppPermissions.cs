using Abp.MultiTenancy;

namespace esign.Authorization
{
    /// <summary>
    /// Defines string constants for application's permission names.
    /// <see cref="AppAuthorizationProvider"/> for permission definitions.
    /// </summary>
    public static class AppPermissions
    {
        //COMMON PERMISSIONS (FOR BOTH OF TENANTS AND HOST)

        public const string Pages = "Pages";

        public const string Pages_Administration = "Pages.Administration";

        #region TenantRegistration
        public const string Pages_Administration_TenantRegistration = "Pages.Administration.TenantRegistration";
        #endregion

        public const string Pages_Administration_Roles = "Pages.Administration.Roles";
        public const string Pages_Administration_Roles_GetRoles = "Pages.Administration.Roles.GetRoles";
        public const string Pages_Administration_Roles_CreateOrUpdateRole = "Pages.Administration.Roles.CreateOrUpdateRole";

        public const string Pages_Administration_Roles_Create = "Pages.Administration.Roles.Create";
        public const string Pages_Administration_Roles_Edit = "Pages.Administration.Roles.Edit";
        public const string Pages_Administration_Roles_Delete = "Pages.Administration.Roles.Delete";

        public const string Pages_Administration_Users = "Pages.Administration.Users";
        public const string Pages_Administration_Users_Create = "Pages.Administration.Users.Create";
        public const string Pages_Administration_Users_RevokeToken = "Pages.Administration.Users.RevokeToken";
        public const string Pages_Administration_Users_Edit = "Pages.Administration.Users.Edit";
        public const string Pages_Administration_Users_Delete = "Pages.Administration.Users.Delete";
        public const string Pages_Administration_Users_ChangePermissions = "Pages.Administration.Users.ChangePermissions";
        public const string Pages_Administration_Users_Impersonation = "Pages.Administration.Users.Impersonation";
        public const string Pages_Administration_Users_Unlock = "Pages.Administration.Users.Unlock";
        public const string Pages_Administration_Users_ChangeProfilePicture = "Pages.Administration.Users.ChangeProfilePicture";

        public const string Pages_Administration_Languages = "Pages.Administration.Languages";
        public const string Pages_Administration_Languages_Create = "Pages.Administration.Languages.Create";
        public const string Pages_Administration_Languages_Edit = "Pages.Administration.Languages.Edit";
        public const string Pages_Administration_Languages_Delete = "Pages.Administration.Languages.Delete";
        public const string Pages_Administration_Languages_ChangeTexts = "Pages.Administration.Languages.ChangeTexts";
        public const string Pages_Administration_Languages_ChangeDefaultLanguage = "Pages.Administration.Languages.ChangeDefaultLanguage";

        public const string Pages_Administration_AuditLogs = "Pages.Administration.AuditLogs";
        public const string Pages_Administration_AuditLogs_GetAuditLogs = "Pages.Administration.AuditLogs.GetAuditLogs";
        public const string Pages_Administration_AuditLogs_GetAuditLogsToExcel = "Pages.Administration.AuditLogs.GetAuditLogsToExcel";
        public const string Pages_Administration_AuditLogs_GetEntityHistoryObjectTypes = "Pages.Administration.AuditLogs.GetEntityHistoryObjectTypes";
        public const string Pages_Administration_AuditLogs_GetEntityChanges = "Pages.Administration.AuditLogs.GetEntityChanges";
        public const string Pages_Administration_AuditLogs_GetEntityTypeChanges = "Pages.Administration.AuditLogs.GetEntityTypeChanges";
        public const string Pages_Administration_AuditLogs_GetEntityChangesToExcel = "Pages.Administration.AuditLogs.GetEntityChangesToExcel";
        public const string Pages_Administration_AuditLogs_GetEntityPropertyChanges = "Pages.Administration.AuditLogs.GetEntityPropertyChanges";


        public const string Pages_Administration_OrganizationUnits = "Pages.Administration.OrganizationUnits";
        public const string Pages_Administration_OrganizationUnits_GetOrganizationUnits = "Pages.Administration.OrganizationUnits.GetOrganizationUnits";
        public const string Pages_Administration_OrganizationUnits_GetOrganizationUnitUsers = "Pages.Administration.OrganizationUnits.GetOrganizationUnitUsers";
        public const string Pages_Administration_OrganizationUnits_GetOrganizationUnitRoles = "Pages.Administration.OrganizationUnits.GetOrganizationUnitRoles";
        public const string Pages_Administration_OrganizationUnits_GetAll = "Pages.Administration.OrganizationUnits.GetAll";
        public const string Pages_Administration_OrganizationUnits_ManageOrganizationTree = "Pages.Administration.OrganizationUnits.ManageOrganizationTree";
        public const string Pages_Administration_OrganizationUnits_ManageMembers = "Pages.Administration.OrganizationUnits.ManageMembers";
        public const string Pages_Administration_OrganizationUnits_ManageRoles = "Pages.Administration.OrganizationUnits.ManageRoles";
        public const string Pages_Administration_AuditReports = "Pages.Administration.AuditReports";

        public const string Pages_Administration_WebhookSubscription = "Pages.Administration.WebhookSubscription";
        public const string Pages_Administration_WebhookSubscription_Create = "Pages.Administration.WebhookSubscription.Create";
        public const string Pages_Administration_WebhookSubscription_Edit = "Pages.Administration.WebhookSubscription.Edit";
        public const string Pages_Administration_WebhookSubscription_ChangeActivity = "Pages.Administration.WebhookSubscription.ChangeActivity";
        public const string Pages_Administration_WebhookSubscription_Detail = "Pages.Administration.WebhookSubscription.Detail";
        public const string Pages_Administration_Webhook_ListSendAttempts = "Pages.Administration.Webhook.ListSendAttempts";
        public const string Pages_Administration_Webhook_ResendWebhook = "Pages.Administration.Webhook.ResendWebhook";

        //public const string Pages_Administration_DynamicProperties = "Pages.Administration.DynamicProperties";
        //public const string Pages_Administration_DynamicProperties_Create = "Pages.Administration.DynamicProperties.Create";
        //public const string Pages_Administration_DynamicProperties_Edit = "Pages.Administration.DynamicProperties.Edit";
        //public const string Pages_Administration_DynamicProperties_Delete = "Pages.Administration.DynamicProperties.Delete";

        //public const string Pages_Administration_DynamicPropertyValue = "Pages.Administration.DynamicPropertyValue";
        //public const string Pages_Administration_DynamicPropertyValue_Create = "Pages.Administration.DynamicPropertyValue.Create";
        //public const string Pages_Administration_DynamicPropertyValue_Edit = "Pages.Administration.DynamicPropertyValue.Edit";
        //public const string Pages_Administration_DynamicPropertyValue_Delete = "Pages.Administration.DynamicPropertyValue.Delete";

        //public const string Pages_Administration_DynamicEntityProperties = "Pages.Administration.DynamicEntityProperties";
        //public const string Pages_Administration_DynamicEntityProperties_Create = "Pages.Administration.DynamicEntityProperties.Create";
        //public const string Pages_Administration_DynamicEntityProperties_Edit = "Pages.Administration.DynamicEntityProperties.Edit";
        //public const string Pages_Administration_DynamicEntityProperties_Delete = "Pages.Administration.DynamicEntityProperties.Delete";

        //public const string Pages_Administration_DynamicEntityPropertyValue = "Pages.Administration.DynamicEntityPropertyValue";
        //public const string Pages_Administration_DynamicEntityPropertyValue_Create = "Pages.Administration.DynamicEntityPropertyValue.Create";
        //public const string Pages_Administration_DynamicEntityPropertyValue_Edit = "Pages.Administration.DynamicEntityPropertyValue.Edit";
        //public const string Pages_Administration_DynamicEntityPropertyValue_Delete = "Pages.Administration.DynamicEntityPropertyValue.Delete";

        //public const string Pages_Administration_MassNotification = "Pages.Administration.MassNotification";
        //public const string Pages_Administration_MassNotification_Create = "Pages.Administration.MassNotification.Create";

        //public const string Pages_Administration_NewVersion_Create = "Pages_Administration_NewVersion_Create";

        //TENANT-SPECIFIC PERMISSIONS

        public const string Pages_Tenant_Dashboard = "Pages.Tenant.Dashboard";
        public const string Pages_Tenant_Customization = "Pages.Tenant.Customization";

        public const string Pages_Administration_Tenant_Settings = "Pages.Administration.Tenant.Settings";

        //public const string Pages_Administration_Tenant_SubscriptionManagement = "Pages.Administration.Tenant.SubscriptionManagement";

        //HOST-SPECIFIC PERMISSIONS

        //public const string Pages_Editions = "Pages.Editions";
        //public const string Pages_Editions_Create = "Pages.Editions.Create";
        //public const string Pages_Editions_Edit = "Pages.Editions.Edit";
        //public const string Pages_Editions_Delete = "Pages.Editions.Delete";
        //public const string Pages_Editions_MoveTenantsToAnotherEdition = "Pages.Editions.MoveTenantsToAnotherEdition";

        public const string Pages_Tenants = "Pages.Tenants";
        public const string Pages_Tenants_Create = "Pages.Tenants.Create";
        public const string Pages_Tenants_Edit = "Pages.Tenants.Edit";
        public const string Pages_Tenants_ChangeFeatures = "Pages.Tenants.ChangeFeatures";
        public const string Pages_Tenants_Delete = "Pages.Tenants.Delete";
        public const string Pages_Tenants_Impersonation = "Pages.Tenants.Impersonation";

        public const string Pages_Administration_Host_Maintenance = "Pages.Administration.Host.Maintenance";
        public const string Pages_Administration_Host_Settings = "Pages.Administration.Host.Settings";
        public const string Pages_Administration_Host_Dashboard = "Pages.Administration.Host.Dashboard";

        #region MASTER

        public const string Pages_Master = "Pages.Master";

        #region Master Status
        public const string Pages_Master_StatusApi = "Pages.Master.StatusApi";
        public const string Pages_Master_StatusApi_View = "Pages.Master.StatusApi.View";
        public const string Pages_Master_StatusApi_Add = "Pages.Master.StatusApi.Add";
        public const string Pages_Master_StatusApi_Edit = "Pages.Master.StatusApi.Edit";
        public const string Pages_Master_StatusApi_Delete = "Pages.Master.StatusApi.Delete";
        public const string Pages_Master_StatusApi_GetAllStatusByTypeId = "Pages.Master.StatusApi.GetAllStatusByTypeId";
        public const string Pages_Master_StatusApi_GetAllStatus = "Pages.Master.StatusApi.GetAllStatus";
        #endregion

        #region Status Sidebar
        public const string Pages_Master_Status = "Pages.Master.Status";
        #endregion

        #region Master Signature Template
        public const string Pages_Master_SignatureTemplate = "Pages.Master.SignatureTemplate";
        public const string Pages_Master_SignatureTemplate_View = "Pages.Master.SignatureTemplate.View";
        public const string Pages_Master_SignatureTemplate_Add = "Pages.Master.SignatureTemplate.Add";
        public const string Pages_Master_SignatureTemplate_Edit = "Pages.Master.SignatureTemplate.Edit";
        public const string Pages_Master_SignatureTemplate_Delete = "Pages.Master.SignatureTemplate.Delete";
        #endregion

        #region Master System
        public const string Pages_Master_SystemApi = "Pages.Master.SystemApi";
        public const string Pages_Master_SystemApi_View = "Pages.Master.SystemApi.View";
        public const string Pages_Master_SystemApi_Add = "Pages.Master.SystemApi.Add";
        public const string Pages_Master_SystemApi_Edit = "Pages.Master.SystemApi.Edit";
        public const string Pages_Master_SystemApi_Delete = "Pages.Master.SystemApi.Delete";
        public const string Pages_Master_SystemApi_GetSystem = "Pages.Master.SystemApi.GetSystem";
        public const string Pages_Master_SystemApi_GetAllSystems = "Pages.Master.SystemApi.GetAllSystems";

        #endregion

        #region System Sidebar
        public const string Pages_Master_System = "Pages.Master.System";
        #endregion

        #region MstEsignUserImage
        public const string Pages_Master_EsignUserImageApi = "Pages.Master.EsignUserImageApi";
        public const string Pages_Master_EsignUserImageApi_GetListSignatureByUserId = "Pages.Master.EsignUserImageApi.GetListSignatureByUserId";
        public const string Pages_Master_EsignUserImageApi_GetAllSignature = "Pages.Master.EsignUserImageApi.GetAllSignature";
        public const string Pages_Master_EsignUserImageApi_GetListSignatureByUserIdForWeb = "Pages.Master.EsignUserImageApi.GetListSignatureByUserIdForWeb";
        public const string Pages_Master_EsignUserImageApi_UpdateSignatureDefautlForWeb = "Pages.Master.EsignUserImageApi.UpdateSignatureDefautlForWeb";
        public const string Pages_Master_EsignUserImageApi_SaveImageSignature = "Pages.Master.EsignUserImageApi.SaveImageSignature";
        public const string Pages_Master_EsignUserImageApi_DeleteTemplateImageSignature = "Pages.Master.EsignUserImageApi.DeleteTemplateImageSignature";



        #endregion

        #region MstEsignUserImage Sidebar
        public const string Pages_Master_EsignUserImage = "Pages.Master.EsignUserImage";
        #endregion


        #region Master Category
        public const string Pages_Master_CategoryApi = "Pages.Master.CategoryApi";
        public const string Pages_Master_CategoryApi_View = "Pages.Master.CategoryApi.View";
        public const string Pages_Master_CategoryApi_CreateOrEdit = "Pages.Master.CategoryApi.CreateOrEdit";
        public const string Pages_Master_CategoryApi_Delete = "Pages.Master.CategoryApi.Delete";
        public const string Pages_Master_CategoryApi_GetCategoryByName = "Pages.Master.CategoryApi.GetCategoryByName";
        public const string Pages_Master_CategoryApi_GetAllMstEsignCategories = "Pages.Master.CategoryApi.GetAllMstEsignCategories";
        public const string Pages_Master_CategoryApi_GetSystem = "Pages.Master.CategoryApi.GetSystem";
        #endregion

        #region  Category Sidebar
        public const string Pages_Master_Category = "Pages.Master.Category";
        #endregion

        #region Master Division
        public const string Pages_Master_DivisionApi = "Pages.Master.DivisionApi";
        public const string Pages_Master_DivisionApi_View = "Pages.Master.DivisionApi.View";
        public const string Pages_Master_DivisionApi_CreateOrEdit = "Pages.Master.DivisionApi.CreateOrEdit";
        public const string Pages_Master_DivisionApi_Delete = "Pages.Master.DivisionApi.Delete";
        public const string Pages_Master_DivisionApi_GetDivisionExcel = "Pages.Master.DivisionApi.GetDivisionExcel";
        #endregion

        #region Division Sidebar
        public const string Pages_Master_Division = "Pages.Master.Division";
        #endregion

        #region Master Department
        public const string Pages_Master_Department = "Pages.Master.Department";
        public const string Pages_Master_Department_View = "Pages.Master.Department.View";
        public const string Pages_Master_Department_CreateOrEdit = "Pages.Master.Department.CreateOrEdi";
        public const string Pages_Master_Department_Delete = "Pages.Master.Department.Delete";
        public const string Pages_Master_Department_GetDepartmentExcel = "Pages.Master.Department.GetDepartmentExcel";
        #endregion

        #region Master Group Signers
        public const string Pages_Master_GroupSigners = "Pages.Master.GroupSigners";
        public const string Pages_Master_GroupSigners_View = "Pages.Master.GroupSigners.View";
        public const string Pages_Master_GroupSigners_Add = "Pages.Master.GroupSigners.Add";
        public const string Pages_Master_GroupSigners_Edit = "Pages.Master.GroupSigners.Edit";
        public const string Pages_Master_GroupSigners_Delete = "Pages.Master.GroupSigners.Delete";
        #endregion

        #region Master Activity
        public const string Pages_Master_Activity = "Pages.Master.Activity";
        public const string Pages_Master_Activity_View = "Pages.Master.Activity.View";
        public const string Pages_Master_Activity_Add = "Pages.Master.Activity.Add";
        public const string Pages_Master_Activity_Edit = "Pages.Master.Activity.Edit";
        public const string Pages_Master_Activity_Delete = "Pages.Master.Activity.Delete";
        #endregion

        #region Master Esign Color Sidebar
        public const string Pages_Master_EsignColor = "Pages.Master.EsignColor";
        #endregion

        #region Master Esign Color
        public const string Pages_Master_EsignColorApi = "Pages.Master.EsignColorApi";
        public const string Pages_Master_EsignColorApi_View = "Pages.Master.EsignColorApi.View";
        public const string Pages_Master_EsignColorApi_GetColorExcel = "Pages.Master.EsignColorApi.GetColorExcel";
        public const string Pages_Master_EsignColorApi_CreateOrEdit = "Pages.Master.EsignColorApi.CreateOrEdit";
        public const string Pages_Master_EsignColorApi_Delete = "Pages.Master.EsignColorApi.Delete";
        #endregion

        #region Master Esign Logo
        public const string Pages_Master_EsignLogoApi = "Pages.Master.LogoApi";
        public const string Pages_Master_EsignLogoApi_GetAllLogos = "Pages.Master.LogoApi.GetAllLogos";
        public const string Pages_Master_EsignLogoApi_Add = "Pages.Master.LogoApi.Add";
        public const string Pages_Master_EsignLogoApi_Edit = "Pages.Master.LogoApi.Edit";
        public const string Pages_Master_EsignLogoApi_Delete = "Pages.Master.LogoApi.Delete";
        public const string Pages_Master_EsignLogoApi_GetAllTenants = "Pages.Master.LogoApi.GetAllTenants";
        public const string Pages_Master_EsignLogoApi_GetMstEsignLogoByTenant = "Pages.Master.LogoApi.GetMstEsignLogoByTenant";


        #endregion

        #region Esign Logo Sidebar
        public const string Pages_Master_EsignLogo = "Pages.Master.Logo";
        #endregion

        #region Mst Esign Signer Template
        public const string Pages_Master_EsignSignerTemplateApi = "Pages_Master_EsignSignerTemplateApi";
        public const string Pages_Master_EsignSignerTemplateApi_GetListTemplateForUser = "Pages.Master.EsignSignerTemplateApi.GetListTemplateForUser";
        public const string Pages_Master_EsignSignerTemplateApi_DeleteTemplateForRequester = "Pages.Master.EsignSignerTemplateApi.DeleteTemplateForRequester";
        public const string Pages_Master_EsignSignerTemplateApi_GetAllSignatureTemplate = "Pages.Master.EsignSignerTemplateApi.GetAllSignatureTemplate";
        public const string Pages_Master_EsignSignerTemplateApi_GetAllSignatureTemplateLinkById = "Pages.Master.EsignSignerTemplateApi.GetAllSignatureTemplateLinkById";
        public const string Pages_Master_EsignSignerTemplateApi_GetListTemplateForUserWeb = "Pages.Master.EsignSignerTemplateApi.GetListTemplateForUserWeb";


        #endregion

        #region Esign Signer Template Sidebar
        public const string Pages_Master_EsignSignerTemplate = "Pages_Master_EsignSignerTemplate";

        #endregion

        #region Master Email Config
        public const string Pages_Master_EmailConfig = "Pages.Master.EmailConfig";
        public const string Pages_Master_EmailConfig_Add = "Pages.Master.EmailConfig.Add";
        public const string Pages_Master_EmailConfig_Edit = "Pages.Master.EmailConfig.Edit";
        public const string Pages_Master_EmailConfig_Delete = "Pages.Master.EmailConfig.Delete";
        #endregion

        #region Master Esign Config
        public const string Pages_Master_EsignConfig = "Pages.Master.EsignConfig";
        public const string Pages_Master_EsignConfig_CreateOrEdit = "Pages.Master.EsignConfig.CreateOrEdit";
        public const string Pages_Master_EsignConfig_Delete = "Pages.Master.EsignConfig.Delete";
        public const string Pages_Master_EsignConfig_View = "Pages.Master.EsignConfig.View";
        #endregion

        #region Master Affiliate
        public const string Pages_Master_Affiliate = "Pages.Master.Affiliate";
        public const string Pages_Master_Affiliate_View = "Pages.Master.Affiliate.View";
        public const string Pages_Master_Affiliate_CreateOrEdit = "Pages.Master.Affiliate.CreateOrEdit";
        public const string Pages_Master_Affiliate_Delete = "Pages.Master.Affiliate.Delete";
        public const string Pages_Master_Affiliate_Sync = "Pages.Master.Affiliate.Sync";
        public const string Pages_Master_Affiliate_ReceiveMultiAffiliateUsersInfo = "Pages.Master.Affiliate.ReceiveMultiAffiliateUsersInfo";
        #endregion

        #region MstEsignEmailTemplate
        public const string Pages_Master_MstEsignEmailTemplate = "Pages.Master.MstEsignEmailTemplate";
        public const string Pages_Master_MstEsignEmailTemplate_CreateOrEdit = "Pages.Master.MstEsignEmailTemplate.CreateOrEdit";
        public const string Pages_Master_MstEsignEmailTemplate_Delete = "Pages.Master.MstEsignEmailTemplate.Delete";
        public const string Pages_Master_MstEsignEmailTemplate_GetAllEmailTemplate = "Pages.Master.MstEsignEmailTemplate.GetAllEmailTemplate";
        #endregion


        #endregion

        #region BUSINESS

        public const string Pages_Business = "Pages.Business";
        public const string Pages_Business_CreateNewDocument = "Pages.Business.CreateNewDocument";
        public const string Pages_Business_ViewDocument = "Pages.Business.ViewDocument";
        public const string Pages_Business_Transfer = "Pages.Business.Transfer";
        public const string Pages_Business_Dashboard = "Pages.Business.Dashboard";
        public const string Pages_Business_ViewNotification = "Pages.Business.ViewNotification";
        public const string Pages_Business_ViewNotification_View = "Pages.Business.ViewNotification.View";
        public const string Pages_Business_ViewNotification_Delete = "Pages.Business.ViewNotification.Delete";
        public const string Pages_Business_ViewNotification_MarkAsReadAll = "Pages.Business.ViewNotification.MarkAsReadAll";

        #region ViewDocument
        public const string Pages_Business_ViewDocument_Edit = "Pages.Business.ViewDocument.Edit";
        public const string Pages_Business_ViewDocument_Delete = "Pages.Business.ViewDocument.Delete";
        public const string Pages_Business_ViewDocument_Submit = "Pages.Business.ViewDocument.Submit";
        public const string Pages_Business_ViewDocument_Remind = "Pages.Business.ViewDocument.Remind";
        public const string Pages_Business_ViewDocument_Revoke = "Pages.Business.ViewDocument.Revoke";
        public const string Pages_Business_ViewDocument_Reject = "Pages.Business.ViewDocument.Reject";
        public const string Pages_Business_ViewDocument_Transfer = "Pages.Business.ViewDocument.Transfer";
        public const string Pages_Business_ViewDocument_Reassign = "Pages.Business.ViewDocument.Reassign";
        public const string Pages_Business_ViewDocument_Share = "Pages.Business.ViewDocument.Share";
        public const string Pages_Business_ViewDocument_Comment = "Pages.Business.ViewDocument.Comment";
        public const string Pages_Business_ViewDocument_Search = "Pages.Business.ViewDocument.Search";

        #region ViewDocument - Sign
            public const string Pages_Business_ViewDocument_SignDocument = "Pages.Business.ViewDocument.SignDocument";
            public const string Pages_Business_ViewDocument_Sign = "Pages.Business.ViewDocument.Sign";
            public const string Pages_Business_ViewDocument_Sign_UploadSignature = "Pages.Business.ViewDocument.UploadSignature";
            public const string Pages_Business_ViewDocument_Sign_DrawSignature = "Pages.Business.ViewDocument.DrawSignature";
        #endregion

        #region EsignRequestWeb
        public const string Pages_EsignRequestWeb = "Pages.EsignRequestWeb";
        public const string Pages_EsignRequestWeb_GetListRequestsBySystemIdWeb = "Pages.EsignRequestWeb.GetListRequestsBySystemIdWeb";
        public const string Pages_EsignRequestWeb_GetListRequestsForDashboard = "Pages.EsignRequestWeb.GetListRequestsForDashboard";
        public const string Pages_EsignRequestWeb_GetRequestDetailDashboard = "Pages.EsignRequestWeb.GetRequestDetailDashboard";
        public const string Pages_EsignRequestWeb_GetListRequestsCanTransferWeb = "Pages.EsignRequestWeb.GetListRequestsCanTransferWeb";
        public const string Pages_EsignRequestWeb_GetRequestsByIdForSelectedItemWeb = "Pages.EsignRequestWeb.GetRequestsByIdForSelectedItemWeb";
        public const string Pages_EsignRequestWeb_GetEsignPositionsWebByDocumentId = "Pages.EsignRequestWeb.GetEsignPositionsWebByDocumentId";
        public const string Pages_EsignRequestWeb_GetTransferHistory = "Pages.EsignRequestWeb.GetTransferHistory";
        #endregion
        
        #region EsignRequestWeb
        public const string Pages_EsignSignerSearchHistory = "Pages.EsignSignerSearchHistory";
        public const string Pages_EsignSignerSearchHistory_GetSignerSearchHistory = "Pages.EsignSignerSearchHistory.GetSignerSearchHistory";
        #endregion

        #region EsignSignerList
        public const string Pages_EsignSignerList = "Pages.EsignSignerList";
        public const string Pages_EsignSignerList_GetListSignerByRequestId = "Pages.EsignSignerList.GetListSignerByRequestId";
        public const string Pages_EsignSignerList_GetListSignerByRequestIdForRequestInfo = "Pages.EsignSignerList.GetListSignerByRequestIdForRequestInfo";
        public const string Pages_EsignSignerList_UpdateSignOffStatus = "Pages.EsignSignerList.UpdateSignOffStatus";
        public const string Pages_EsignSignerList_GetListSignerByAttachmentId = "Pages.EsignSignerList.GetListSignerByAttachmentId";
        public const string Pages_EsignSignerList_DoRejectRequest = "Pages.EsignSignerList.DoRejectRequest";
        public const string Pages_EsignSignerList_CloneRequest = "Pages.EsignSignerList.CloneRequest";
        public const string Pages_EsignSignerList_CloneRequestWithoutFields = "Pages.EsignSignerList.CloneRequestWithoutFields";
        public const string Pages_EsignSignerList_DoRevokeRequest = "Pages.EsignSignerList.DoRevokeRequest";
        public const string Pages_EsignSignerList_GetListSignerAndRequestByRequestId = "Pages.EsignSignerList.GetListSignerAndRequestByRequestId";
        #endregion

        #region DynamicProperty

        public const string Pages_DynamicProperty = "Pages.DynamicProperty";
        public const string Pages_DynamicProperty_Get = "Pages.DynamicProperty.Get";
        public const string Pages_DynamicProperty_GetAll = "Pages.DynamicProperty.GetAll";
        public const string Pages_DynamicProperty_Add = "Pages.DynamicProperty.Add";
        public const string Pages_DynamicProperty_Update = "Pages.DynamicProperty.Update";
        public const string Pages_DynamicProperty_Delete = "Pages.DynamicProperty.Delete";
        public const string Pages_DynamicProperty_FindAllowedInputType = "Pages.DynamicProperty.FindAllowedInputType";
        #endregion
        #region DynamicPropertyValue

        public const string Pages_DynamicPropertyValue = "Pages.DynamicPropertyValue";
        public const string Pages_DynamicPropertyValue_Get = "Pages.DynamicPropertyValue.Get";
        public const string Pages_DynamicPropertyValue_GetAllValuesOfDynamicProperty = "Pages.DynamicPropertyValue.GetAllValuesOfDynamicProperty";
        public const string Pages_DynamicPropertyValue_Add = "Pages.DynamicPropertyValue.Add";
        public const string Pages_DynamicPropertyValue_Update = "Pages.DynamicPropertyValue.Update";
        public const string Pages_DynamicPropertyValue_Delete = "Pages.DynamicPropertyValue.Delete";
        #endregion
        #region Edition
        public const string Pages_Edition = "Pages.Edition";
        public const string Pages_Edition_GetEditions = "Pages.Edition.GetEditions";
        public const string Pages_Edition_GetEditionForEdit = "Pages.Edition.GetEditionForEdit";
        public const string Pages_Edition_CreateEdition = "Pages.Edition.CreateEdition";
        public const string Pages_Edition_UpdateEdition = "Pages.Edition.UpdateEdition";
        public const string Pages_Edition_DeleteEdition = "Pages.Edition.DeleteEdition";
        public const string Pages_Edition_MoveTenantsToAnotherEdition = "Pages.Edition.MoveTenantsToAnotherEdition";
        public const string Pages_Edition_GetEditionComboboxItems = "Pages.Edition.GetEditionComboboxItems";
        public const string Pages_Edition_GetTenantCount = "Pages.Edition.GetTenantCount";
        #endregion


        #region EsignActivityHistory
        public const string Pages_EsignActivityHistory = "Pages.EsignActivityHistory";
        public const string Pages_EsignActivityHistory_CreateSignerActivity = "Pages.EsignActivityHistory.CreateSignerActivity";
        public const string Pages_EsignActivityHistory_GetListActivityHistory = "Pages.EsignActivityHistory.GetListActivityHistory";
        public const string Pages_EsignActivityHistory_GetListActivityHistoryForVerifiedDocument = "Pages.EsignActivityHistory.GetListActivityHistoryForVerifiedDocument";
        public const string Pages_EsignActivityHistory_GetListActivityHistoryForUser = "Pages.EsignActivityHistory.GetListActivityHistoryForUser";
        #endregion
        #region EsignApiOtherSystem
        public const string Pages_EsignApiOtherSystem = "Pages.EsignApiOtherSystem";
        public const string Pages_EsignApiOtherSystem_CreateOrEditEsignRequestOtherSystem = "Pages.EsignApiOtherSystem.CreateOrEditEsignRequestOtherSystem";
        public const string Pages_EsignApiOtherSystem_ValidateFromOtherSystem = "Pages.EsignApiOtherSystem.ValidateFromOtherSystem";
        public const string Pages_EsignApiOtherSystem_RevokeRequestFromOrtherSystem = "Pages.EsignApiOtherSystem.RevokeRequestFromOrtherSystem";
        public const string Pages_EsignApiOtherSystem_SignDocumentFromOtherSystem = "Pages.EsignApiOtherSystem.SignDocumentFromOtherSystem";
        public const string Pages_EsignApiOtherSystem_RejectRequestFromOtherSystem = "Pages.EsignApiOtherSystem.RejectRequestFromOtherSystem";
        #endregion
        #region EsignComments

        public const string Pages_EsignComments = "Pages.EsignComments";
        public const string Pages_EsignComments_CreateOrEditEsignComments = "Pages.EsignComments.CreateOrEditEsignComments";
        public const string Pages_EsignComments_GetAllCommentsForRequestId = "Pages.EsignComments.GetAllCommentsForRequestId";
        public const string Pages_EsignComments_GetTotalUnreadComment = "Pages.EsignComments.GetTotalUnreadComment";

        #endregion

        #region EsignDocumentList
        public const string Pages_EsignDocumentList = "Pages.EsignDocumentList";
        public const string Pages_EsignDocumentList_GetEsignDocumentByRequestIdForRequestInfo = "Pages.EsignDocumentList.GetEsignDocumentByRequestIdForRequestInfo";
        public const string Pages_EsignDocumentList_GetEsignDocumentByRequestId =
            "Pages.EsignDocumentList.GetEsignDocumentByRequestId";
        public const string Pages_EsignDocumentList_UpdateDocumentNameById =
            "Pages.EsignDocumentList.UpdateDocumentNameById";
        #endregion
        #region EsignDocumentListWeb

        public const string Pages_EsignDocumentListWeb = "Pages.EsignDocumentListWeb";
        public const string Pages_EsignDocumentListWeb_GetEsignDocumentByRequestIdForRequestInfo =
            "Pages.EsignDocumentListWeb.GetEsignDocumentByRequestIdForRequestInfo";
        public const string Pages_EsignDocumentListWeb_GetEsignDocumentByRequestId =
            "Pages.EsignDocumentListWeb.GetEsignDocumentByRequestId";
        public const string Pages_EsignDocumentListWeb_UpdateDocumentNameById =
            "Pages.EsignDocumentListWeb.UpdateDocumentNameById";

        #endregion
        #region EsignFeedbackHub
        public const string Pages_EsignFeedbackHub = "Pages.EsignFeedbackHub";
        public const string Pages_EsignFeedbackHub_Feedback = "Pages.EsignFeedbackHub.Feedback";
        #endregion
        #region EsignKeywordSearchHistory
        public const string Pages_EsignKeywordSearchHistory = "Pages.EsignKeywordSearchHistory";
        public const string Pages_EsignKeywordSearchHistory_GetSearchKeywordHistory = "Pages.EsignKeywordSearchHistory.GetSearchKeywordHistory";
        #endregion

        #region EsignFollowUp

        public const string Pages_EsignFollowUp = "Pages.EsignFollowUp";
        public const string Pages_EsignFollowUp_FollowUpRequest = "Pages.EsignFollowUp.FollowUpRequest";
        #endregion
        #region EsignReferenceRequest

        public const string Pages_EsignReferenceRequest = "Pages.EsignReferenceRequest";
        public const string Pages_EsignReferenceRequest_CreateOrEditReferenceRequest = "Pages.EsignReferenceRequest.CreateOrEditReferenceRequest";
        public const string Pages_EsignReferenceRequest_CreateNewReferenceRequest = "Pages.EsignReferenceRequest.CreateNewReferenceRequest";
        public const string Pages_EsignReferenceRequest_AddAdditionalFile = "Pages.EsignReferenceRequest.AddAdditionalFile";
        public const string Pages_EsignReferenceRequest_DeleteReferenceRequest = "Pages.EsignReferenceRequest.DeleteReferenceRequest";
        public const string Pages_EsignReferenceRequest_GetReferenceRequestByRequestId = "Pages.EsignReferenceRequest.GetReferenceRequestByRequestId";
        #endregion

        #region EsignRequest

        public const string Pages_EsignRequest = "Pages.EsignRequest";
        public const string Pages_EsignRequest_GetTotalCountRequestsBySystemId = "Pages.EsignRequest.GetTotalCountRequestsBySystemId";
        public const string Pages_EsignRequest_GetRequestSummaryById = "Pages.EsignRequest.GetRequestSummaryById";
        public const string Pages_EsignRequest_GetRequestInfomationById = "Pages.EsignRequest.GetRequestInfomationById";
        public const string Pages_EsignRequest_GetEsignSignaturePageByDocumentId = "Pages.EsignRequest.GetEsignSignaturePageByDocumentId";
        public const string Pages_EsignRequest_GetEsignSignaturePageByRequestId = "Pages.EsignRequest.GetEsignSignaturePageByRequestId";
        public const string Pages_EsignRequest_GetEsignPositionsByRequestId = "Pages.EsignRequest.GetEsignPositionsByRequestId";
        public const string Pages_EsignRequest_GetEsignPositionsByDocumentId = "Pages.EsignRequest.GetEsignPositionsByDocumentId";
        public const string Pages_EsignRequest_ValidateDigitalSignature = "Pages.EsignRequest.ValidateDigitalSignature";
        public const string Pages_EsignRequest_CreateOrEditEsignRequest = "Pages.EsignRequest.CreateOrEditEsignRequest";
        public const string Pages_EsignRequest_GetSignatureOfRequester = "Pages.EsignRequest.GetSignatureOfRequester";
        public const string Pages_EsignRequest_SaveDraftRequest = "Pages.EsignRequest.SaveDraftRequest";
        public const string Pages_EsignRequest_GetListRequestsBySystemId = "Pages.EsignRequest.GetListRequestsBySystemId";
        public const string Pages_EsignRequest_GetListRequestsBySearchValue = "Pages.EsignRequest.GetListRequestsBySearchValue";
        public const string Pages_EsignRequest_GetMessageConfirmFinishAddSigner = "Pages.EsignRequest.GetMessageConfirmFinishAddSigner";
        public const string Pages_EsignRequest_EsignRequerstCreateField = "Pages.EsignRequest.EsignRequerstCreateField";
        public const string Pages_EsignRequest_SignerSign = "Pages.EsignRequest.SignerSign";
        public const string Pages_EsignRequest_ShareRequest = "Pages.EsignRequest.ShareRequest";
        public const string Pages_EsignRequest_ShareRequest_Web = "Pages.EsignRequest.ShareRequest_Web";
        public const string Pages_EsignRequest_CreateRemindRequestDto = "Pages.EsignRequest.CreateRemindRequestDto";
        public const string Pages_EsignRequest_DeleteDraftRequest = "Pages.EsignRequest.DeleteDraftRequest";
        #endregion
        #region EsignRequestMultiAffiliate

        public const string Pages_EsignRequestMultiAffiliate = "Pages.EsignRequestMultiAffiliate";
        public const string Pages_EsignRequestMultiAffiliate_GetMultiAffiliateAuthenToken = "Pages.EsignRequestMultiAffiliate.GetMultiAffiliateAuthenToken";
        public const string Pages_EsignRequestMultiAffiliate_GetMultiAffiliateUsersInfo = "Pages.EsignRequestMultiAffiliate.GetMultiAffiliateUsersInfo";
        public const string Pages_EsignRequestMultiAffiliate_ReceiveMultiAffiliateUsersInfo = "Pages.EsignRequestMultiAffiliate.ReceiveMultiAffiliateUsersInfo";
        public const string Pages_EsignRequestMultiAffiliate_GetRequestForMultiAffiliate = "Pages.EsignRequestMultiAffiliate.GetRequestForMultiAffiliate";
        public const string Pages_EsignRequestMultiAffiliate_SendMultiAffiliateEsignRequest = "Pages.EsignRequestMultiAffiliate.SendMultiAffiliateEsignRequest";
        public const string Pages_EsignRequestMultiAffiliate_CreateMultiAffiliateRequest = "Pages.EsignRequestMultiAffiliate.CreateMultiAffiliateRequest";
        public const string Pages_EsignRequestMultiAffiliate_GetRequestForMultiAffiliateSigningInfo = "Pages.EsignRequestMultiAffiliate.GetRequestForMultiAffiliateSigningInfo";
        public const string Pages_EsignRequestMultiAffiliate_SendMultiAffiliateEsignRequestSigningInfo = "Pages.EsignRequestMultiAffiliate.SendMultiAffiliateEsignRequestSigningInfo";
        public const string Pages_EsignRequestMultiAffiliate_UpdateMultiAffiliateRequestSigningInfo = "Pages.EsignRequestMultiAffiliate.UpdateMultiAffiliateRequestSigningInfo";
        public const string Pages_EsignRequestMultiAffiliate_GetRequestForMultiAffiliateRejectInfo = "Pages.EsignRequestMultiAffiliate.GetRequestForMultiAffiliateRejectInfo";
        public const string Pages_EsignRequestMultiAffiliate_SendMultiAffiliateEsignRequestRejectInfo = "Pages.EsignRequestMultiAffiliate.SendMultiAffiliateEsignRequestRejectInfo";
        public const string Pages_EsignRequestMultiAffiliate_UpdateMultiAffiliateRequestRejectInfo = "Pages.EsignRequestMultiAffiliate.UpdateMultiAffiliateRequestRejectInfo";
        public const string Pages_EsignRequestMultiAffiliate_GetRequestForMultiAffiliateCommentInfo = "Pages.EsignRequestMultiAffiliate.GetRequestForMultiAffiliateCommentInfo";
        public const string Pages_EsignRequestMultiAffiliate_SendMultiAffiliateEsignRequestCommentInfo = "Pages.EsignRequestMultiAffiliate.SendMultiAffiliateEsignRequestCommentInfo";
        public const string Pages_EsignRequestMultiAffiliate_UpdateMultiAffiliateRequestCommentInfo = "Pages.EsignRequestMultiAffiliate.UpdateMultiAffiliateRequestCommentInfo";
        public const string Pages_EsignRequestMultiAffiliate_GetRequestForMultiAffiliateReassignInfo = "Pages.EsignRequestMultiAffiliate.GetRequestForMultiAffiliateReassignInfo";
        public const string Pages_EsignRequestMultiAffiliate_SendMultiAffiliateEsignRequestReassignInfo = "Pages.EsignRequestMultiAffiliate.SendMultiAffiliateEsignRequestReassignInfo";
        public const string Pages_EsignRequestMultiAffiliate_UpdateMultiAffiliateRequestReassignInfo = "Pages.EsignRequestMultiAffiliate.UpdateMultiAffiliateRequestReassignInfo";
        public const string Pages_EsignRequestMultiAffiliate_GetRequestForMultiAffiliateRemindInfo = "Pages.EsignRequestMultiAffiliate.GetRequestForMultiAffiliateRemindInfo";
        public const string Pages_EsignRequestMultiAffiliate_SendMultiAffiliateEsignRequestRemindInfo = "Pages.EsignRequestMultiAffiliate.SendMultiAffiliateEsignRequestRemindInfo";
        public const string Pages_EsignRequestMultiAffiliate_UpdateMultiAffiliateRequestRemindInfo = "Pages.EsignRequestMultiAffiliate.UpdateMultiAffiliateRequestRemindInfo";
        public const string Pages_EsignRequestMultiAffiliate_GetRequestForMultiAffiliateRevokeInfo = "Pages.EsignRequestMultiAffiliate.GetRequestForMultiAffiliateRevokeInfo";
        public const string Pages_EsignRequestMultiAffiliate_SendMultiAffiliateEsignRequestRevokeInfo = "Pages.EsignRequestMultiAffiliate.SendMultiAffiliateEsignRequestRevokeInfo";
        public const string Pages_EsignRequestMultiAffiliate_UpdateMultiAffiliateRequestRevokeInfo = "Pages.EsignRequestMultiAffiliate.UpdateMultiAffiliateRequestRevokeInfo";
        public const string Pages_EsignRequestMultiAffiliate_GetRequestForMultiAffiliateTransferInfo = "Pages.EsignRequestMultiAffiliate.GetRequestForMultiAffiliateTransferInfo";
        public const string Pages_EsignRequestMultiAffiliate_SendMultiAffiliateEsignRequestTransferInfo = "Pages.EsignRequestMultiAffiliate.SendMultiAffiliateEsignRequestTransferInfo";
        public const string Pages_EsignRequestMultiAffiliate_UpdateMultiAffiliateRequestTransferInfo = "Pages.EsignRequestMultiAffiliate.UpdateMultiAffiliateRequestTransferInfo";
        public const string Pages_EsignRequestMultiAffiliate_GetRequestForMultiAffiliateShareInfo = "Pages.EsignRequestMultiAffiliate.GetRequestForMultiAffiliateShareInfo";
        public const string Pages_EsignRequestMultiAffiliate_SendMultiAffiliateEsignRequestShareInfo = "Pages.EsignRequestMultiAffiliate.SendMultiAffiliateEsignRequestShareInfo";
        public const string Pages_EsignRequestMultiAffiliate_UpdateMultiAffiliateRequestShareInfo = "Pages.EsignRequestMultiAffiliate.UpdateMultiAffiliateRequestShareInfo";
        public const string Pages_EsignRequestMultiAffiliate_GetRequestForMultiAffiliateAdditionFileInfo = "Pages.EsignRequestMultiAffiliate.GetRequestForMultiAffiliateAdditionFileInfo";
        public const string Pages_EsignRequestMultiAffiliate_SendMultiAffiliateEsignRequestAdditionFileInfo = "Pages.EsignRequestMultiAffiliate.SendMultiAffiliateEsignRequestAdditionFileInfo";
        public const string Pages_EsignRequestMultiAffiliate_UpdateMultiAffiliateRequestAdditionFileInfo = "Pages.EsignRequestMultiAffiliate.UpdateMultiAffiliateRequestAdditionFileInfo";
        #endregion




        #endregion

        #endregion

        //ductm
        #region EsignSignerTemplateLink
        public const string Pages_Business_EsignSignerTemplateLink = "Pages.Business.EsignSignerTemplateLink";
        public const string Pages_Business_EsignSignerTemplateLink_CreateNewTemplateForRequester = "Pages.Business.EsignSignerTemplateLink.CreateNewTemplateForRequester";
        public const string Pages_Business_EsignSignerTemplateLink_GetListSignerByTemplateId = "Pages.Business.EsignSignerTemplateLink.GetListSignerByTemplateId";
        public const string Pages_Business_EsignSignerTemplateLink_CreateNewTemplateForWebRequesterForWeb = "Pages.Business.EsignSignerTemplateLink.CreateNewTemplateForWebRequesterForWeb";
        #endregion

        #region EsignStatusSignerHistory
        public const string Pages_Business_EsignStatusSignerHistory = "Pages.Business.EsignStatusSignerHistory";
        public const string Pages_Business_EsignStatusSignerHistory_GetStatusHistoryByRequestId = "Pages.Business.EsignStatusSignerHistory.GetStatusHistoryByRequestId";
        #endregion

        #region EsignVersionApp
        public const string Pages_Business_EsignVersionApp = "Pages.Business.EsignVersionApp";
        public const string Pages_Business_EsignVersionApp_getEsignVersionApp = "Pages.Business.EsignVersionApp.getEsignVersionApp";
        public const string Pages_Business_EsignVersionApp_CreateEsignVersion = "Pages.Business.EsignVersionApp.CreateEsignVersion";
        #endregion

        #region Friendship
        public const string Pages_Friendship = "Pages.Friendship";
        //public const string Pages_Friendship_CreateFriendshipRequest = "Pages.Friendship.CreateFriendshipRequest";
        //public const string Pages_Friendship_CreateFriendshipRequestByUserName = "Pages.Friendship.CreateFriendshipRequestByUserName";
        //public const string Pages_Friendship_BlockUser = "Pages.Friendship.BlockUser";
        //public const string Pages_Friendship_UnblockUser = "Pages.Friendship.UnblockUser";
        //public const string Pages_Friendship_AcceptFriendshipRequest = "Pages.Friendship.AcceptFriendshipRequest";
        //public const string Pages_Friendship_RemoveFriend = "Pages.Friendship.RemoveFriend";
        #endregion

        #region Install
        public const string Pages_Install = "Pages.Install";
        public const string Pages_Install_Setup = "Pages.Install.Setup";
        public const string Pages_Install_GetAppSettingsJson = "Pages.Install.GetAppSettingsJson";
        public const string Pages_Install_CheckDatabase = "Pages.Install.CheckDatabase";
        #endregion

        #region Invoice
        public const string Pages_Business_Invoice = "Pages.Business.Invoice";
        public const string Pages_Business_Invoice_GetInvoiceInfo = "Pages.Business.Invoice.GetInvoiceInfo";
        public const string Pages_Business_Invoice_CreateInvoice = "Pages.Business.Invoice.CreateInvoice";
        #endregion

        #region MstActivityHistory
        public const string Pages_Master_MstActivityHistory = "Pages.Master.MstActivityHistory";
        public const string Pages_Master_MstActivityHistory_GetAllActivityHistory = "Pages.Master.MstActivityHistory.GetAllActivityHistory";
        public const string Pages_Master_MstActivityHistory_GetAllMstEsignActivityHistory = "Pages.Master.MstActivityHistory.GetAllMstEsignActivityHistory";
        public const string Pages_Master_MstActivityHistory_GetListStatusActivityHistory = "Pages.Master.MstActivityHistory.GetListStatusActivityHistory";
        public const string Pages_Master_MstActivityHistory_CreateOrEdit = "Pages.Master.MstActivityHistory.CreateOrEdit";
        public const string Pages_Master_MstActivityHistory_Delete = "Pages.Master.MstActivityHistory.Delete";
        public const string Pages_Master_MstActivityHistory_GetActivityHistory = "Pages.Master.MstActivityHistory.GetActivityHistory";
        #endregion

        #region MstEsignActiveDirectory
        public const string Pages_Master_MstEsignActiveDirectory = "Pages.Master.MstEsignActiveDirectory";
        public const string Pages_Master_MstEsignActiveDirectory_GetAllSigners = "Pages.Master.MstEsignActiveDirectory.GetAllSigners";
        public const string Pages_Master_MstEsignActiveDirectory_GetAllSignersForWeb = "Pages.Master.MstEsignActiveDirectory.GetAllSignersForWeb";
        public const string Pages_Master_MstEsignActiveDirectory_GetAllSignerByGroup = "Pages.Master.MstEsignActiveDirectory.GetAllSignerByGroup";
        public const string Pages_Master_MstEsignActiveDirectory_GetMyProfile = "Pages.Master.MstEsignActiveDirectory.GetMyProfile";
        public const string Pages_Master_MstEsignActiveDirectory_GetUserInformationById = "Pages.Master.MstEsignActiveDirectory.GetUserInformationById";
        public const string Pages_Master_MstEsignActiveDirectory_GetAllActiveDirectory = "Pages.Master.MstEsignActiveDirectory.GetAllActiveDirectory";
        public const string Pages_Master_MstEsignActiveDirectory_GetActiveDirectory = "Pages.Master.MstEsignActiveDirectory.GetActiveDirectory";
        public const string Pages_Master_MstEsignActiveDirectory_GetMyAccountInfomation = "Pages.Master.MstEsignActiveDirectory.GetMyAccountInfomation";
        public const string Pages_Master_MstEsignActiveDirectory_SaveAccountInfomation = "Pages.Master.MstEsignActiveDirectory.SaveAccountInfomation";
        public const string Pages_Master_MstEsignActiveDirectory_GetAllSignersForTransfer = "Pages.Master.MstEsignActiveDirectory.GetAllSignersForTransfer";
        #endregion

        #region SendEmail
        public const string Pages_SendEmail = "Pages.SendEmail";
        #endregion

        #region Session
        public const string Pages_Session = "Pages.Session";
        #endregion

        #region SettingsAppServiceBase
        public const string Pages_SettingsAppServiceBase = "Pages.SettingsAppServiceBase";
        #endregion

        #region Stripe
        public const string Pages_Stripe = "Pages.Stripe";
        #endregion

        #region StripeControllerBase
        public const string Pages_StripeControllerBase = "Pages.StripeControllerBase";
        #endregion

        #region StripePayment
        public const string Pages_StripePayment = "Pages.StripePayment";
        #endregion

        #region Subscription
        public const string Pages_Subscription = "Pages.Subscription";
        #endregion
        #region Notification
        public const string Pages_Notification = "Pages.Notification";
        public const string Pages_Notification_SendDeviceToken = "Pages.Notification.SendDeviceToken";
        public const string Pages_Notification_DeleteDeviceToken = "Pages.Notification.DeleteDeviceToken";
        public const string Pages_Notification_CreateEsignNotification = "Pages.Notification.CreateEsignNotification";
        public const string Pages_Notification_GetUserNotifications = "Pages.Notification.GetUserNotifications";
        public const string Pages_Notification_ShouldUserUpdateApp = "Pages.Notification_ShouldUserUpdateApp";
        public const string Pages_Notification_SetAllAvailableVersionNotificationAsRead = "Pages.Notification.SetAllAvailableVersionNotificationAsRead";
        public const string Pages_Notification_SetAllNotificationsAsRead = "Pages.Notification.SetAllNotificationsAsRead";
        public const string Pages_Notification_SetNotificationAsRead = "Pages.Notification.SetNotificationAsRead";
        public const string Pages_Notification_GetNotificationSettings = "Pages.Notification.GetNotificationSettings";
        public const string Pages_Notification_UpdateNotificationSettings = "Pages.Notification.UpdateNotificationSettings";
        public const string Pages_Notification_DeleteNotification = "Pages.Notification_DeleteNotification";
        public const string Pages_Notification_DeleteAllUserNotifications = "Pages.Notification.DeleteAllUserNotifications";
        public const string Pages_Notification_GetAllUserForLookupTable = "Pages.Notification.GetAllUserForLookupTable";
        public const string Pages_Notification_GetAllOrganizationUnitForLookupTable = "Pages.Notification.GetAllOrganizationUnitForLookupTable";
        public const string Pages_Notification_CreateMassNotification = "Pages.Notification.CreateMassNotification";
        public const string Pages_Notification_CreateNewVersionReleasedNotification = "Pages.Notification.CreateNewVersionReleasedNotification";
        public const string Pages_Notification_GetAllNotifiers = "Pages.Notification.GetAllNotifiers";
        public const string Pages_Notification_GetNotificationsPublishedByUser = "Pages.Notification.GetNotificationsPublishedByUser";
        #endregion

        #region Payment
        public const string Pages_Payment = "Pages.Payment";
        public const string Pages_Payment_GetPaymentInfo = "Pages.Payment.GetPaymentInfo";
        public const string Pages_Payment_CreatePayment = "Pages.Payment.CreatePayment";
        public const string Pages_Payment_CancelPayment = "Pages.Payment.CancelPayment";
        public const string Pages_Payment_GetPaymentHistory = "Pages.Payment.GetPaymentHistory";
        public const string Pages_Payment_GetActiveGateways = "Pages.Payment.GetActiveGateways";
        public const string Pages_Payment_GetPayment = "Pages.Payment.GetPayment";
        public const string Pages_Payment_GetLastCompletedPayment = "Pages.Payment.GetLastCompletedPayment";
        public const string Pages_Payment_BuyNowSucceed = "Pages.Payment.BuyNowSucceed";
        public const string Pages_Payment_NewRegistrationSucceed = "Pages.Payment.NewRegistrationSucceed";
        public const string Pages_Payment_UpgradeSucceed = "Pages.Payment.UpgradeSucceed";
        public const string Pages_Payment_ExtendSucceed = "Pages.Payment.ExtendSucceed";
        public const string Pages_Payment_PaymentFailed = "Pages.Payment.PaymentFailed";
        public const string Pages_Payment_SwitchBetweenFreeEditions = "Pages.Payment.SwitchBetweenFreeEditions";
        public const string Pages_Payment_UpgradeSubscriptionCostsLessThenMinAmount = "Pages.Payment.UpgradeSubscriptionCostsLessThenMinAmount";
        public const string Pages_Payment_HasAnyPayment = "Pages.Payment.HasAnyPayment";
        #endregion

        #region PayPalPayment
        public const string Pages_PayPalPayment = "Pages.PayPalPayment";
        public const string Pages_PayPalPayment_ConfirmPayment = "Pages.PayPalPayment.ConfirmPayment";
        public const string Pages_PayPalPayment_GetConfiguration = "Pages.PayPalPayment.GetConfiguration";

        #endregion

        #region PdfViewer
        public const string Pages_PdfViewer = "Pages.PdfViewer";
        public const string Pages_PdfViewer_Load = "Pages.PdfViewer.Load";
        public const string Pages_PdfViewer_Bookmarks = "Pages.PdfViewer.Bookmarks";
        public const string Pages_PdfViewer_RenderPdfPages = "Pages.PdfViewer.RenderPdfPages";
        public const string Pages_PdfViewer_RenderPdfTexts = "Pages.PdfViewer.RenderPdfTexts";
        public const string Pages_PdfViewer_RenderThumbnailImages = "Pages.PdfViewer.RenderThumbnailImages";
        public const string Pages_PdfViewer_RenderAnnotationComments = "Pages.PdfViewer.RenderAnnotationComments";
        public const string Pages_PdfViewer_ExportAnnotations = "Pages.PdfViewer.ExportAnnotations";
        public const string Pages_PdfViewer_ImportAnnotations = "Pages.PdfViewer.ImportAnnotations";
        public const string Pages_PdfViewer_ExportFormFields = "Pages.PdfViewer.ExportFormFields";
        public const string Pages_PdfViewer_ImportFormFields = "Pages.PdfViewer.ImportFormFields";
        public const string Pages_PdfViewer_Unload = "Pages.PdfViewer.Unload";
        public const string Pages_PdfViewer_Download = "Pages.PdfViewer.Download";
        public const string Pages_PdfViewer_PrintImages = "Pages.PdfViewer.PrintImages";
        #endregion

        #region Permission
        public const string Pages_Permission = "Pages.Permission";
        public const string Pages_Permission_GetAllPermissions = "Pages.Permission.GetAllPermissions";
        #endregion


        #region Profile
        public const string Pages_Profile = "Pages.Profile";
        public const string Pages_Profile_UploadProfilePicture = "Pages.Profile.UploadProfilePicture";
        public const string Pages_Profile_GetDefaultProfilePicture = "Pages.Profile.GetDefaultProfilePicture";
        public const string Pages_Profile_GetCurrentUserProfileForEdit = "Pages.Profile.GetCurrentUserProfileForEdit";
        public const string Pages_Profile_DisableGoogleAuthenticator = "Pages.Profile.DisableGoogleAuthenticator";
        public const string Pages_Profile_ViewRecoveryCodes = "Pages.Profile.ViewRecoveryCodes";
        public const string Pages_Profile_GenerateGoogleAuthenticatorKey = "Pages.Profile.GenerateGoogleAuthenticatorKey";
        public const string Pages_Profile_UpdateGoogleAuthenticatorKey = "Pages.Profile.UpdateGoogleAuthenticatorKey";
        public const string Pages_Profile_PrepareCollectedData = "Pages.Profile.PrepareCollectedData";
        public const string Pages_Profile_UpdateUserProfile = "Pages.Profile.UpdateUserProfile";
        public const string Pages_Profile_ChangePassword = "Pages.Profile.ChangePassword";
        public const string Pages_Profile_UpdateProfilePicture = "Pages.Profile.UpdateProfilePicture";
        public const string Pages_Profile_VerifyAuthenticatorCode = "Pages.Profile.VerifyAuthenticatorCode";
        public const string Pages_Profile_GetPasswordComplexitySetting = "Pages.Profile.GetPasswordComplexitySetting";
        public const string Pages_Profile_GetProfilePicture = "Pages.Profile.GetProfilePicture";
        public const string Pages_Profile_GetProfilePictureByUserName = "Pages.Profile.GetProfilePictureByUserName";
        public const string Pages_Profile_GetProfilePictureByUser = "Pages.Profile.GetProfilePictureByUser";
        public const string Pages_Profile_ChangeLanguage = "Pages.Profile.ChangeLanguage";
        #endregion

        #region Timing
        public const string Pages_Timing = "Pages.Timing";
        public const string Pages_Timing_GetTimezones = "Pages.Timing.GetTimezones";
        public const string Pages_Timing_GetTimezoneComboboxItems = "Pages.Timing.GetTimezoneComboboxItems";
        #endregion

		#region PERMISSION 22 MAY 2024
        #region ACCOUNT
        public const string Pages_Account = "Pages.Account";
        public const string Pages_Account_Register = "Pages.Account.Register";
        public const string Pages_Account_ForgotPassword = "Pages.Account.ForgotPassword";
        public const string Pages_Account_ResetPassword = "Pages.Account.ResetPassword";
        public const string Pages_Account_SendEmailActivationLink = "Pages.Account.SendEmailActivationLink";
        public const string Pages_Account_ActivateEmail = "Pages.Account.ActivateEmail";
        public const string Pages_Account_DelegatedImpersonate = "Pages.Account.DelegatedImpersonate";
        public const string Pages_Account_BackToImpersonator = "Pages.Account.BackToImpersonator";
        public const string Pages_Account_SwitchToLinkedAccount = "Pages.Account.SwitchToLinkedAccount";
        #endregion

        #region CACHE
        public const string Pages_Cache = "Pages.Cache";
        public const string Pages_Cache_GetAllCaches = "Pages.Cache.GetAllCaches";
        public const string Pages_Cache_ClearCache = "Pages.Cache.ClearCache";
        public const string Pages_Cache_ClearAllCaches = "Pages.Cache.ClearAllCaches";
        #endregion

        #region CHAT
        public const string Pages_Chat = "Pages.Chat";
        public const string Pages_Chat_GetUserChatFriendsWithSettings = "Pages.Chat.GetUserChatFriendsWithSettings";
        public const string Pages_Chat_GetUserChatMessages = "Pages.Chat.GetUserChatMessages";
        public const string Pages_Chat_MarkAllUnreadMessagesOfUserAsRead = "Pages.Chat.MarkAllUnreadMessagesOfUserAsRead";
        #endregion

        #region CommonCallApiOtherSystem
        public const string Pages_CommonCallApiOtherSystem = "Pages.CommonCallApiOtherSystem";
        public const string Pages_CommonCallApiOtherSystem_UpdateResultForOtherSystem = "Pages.CommonCallApiOtherSystem.UpdateResultForOtherSystem";
        public const string Pages_CommonCallApiOtherSystem_UpdateReassignForOtherSystem = "Pages.CommonCallApiOtherSystem.UpdateReassignForOtherSystem";
        public const string Pages_CommonCallApiOtherSystem_GetOtherSystemAuthenToken = "Pages.CommonCallApiOtherSystem.GetOtherSystemAuthenToken";
        #endregion

        #region CommonEsign
        public const string Pages_CommonEsign = "Pages.CommonEsign";
        public const string Pages_CommonEsign_SignDocument = "Pages.CommonEsign.SignDocument";
        public const string Pages_CommonEsign_ConvertTypeSignature = "Pages.CommonEsign.ConvertTypeSignature";
        public const string Pages_CommonEsign_SignDigitalToPdf = "Pages.CommonEsign.SignDigitalToPdf";
        public const string Pages_CommonEsign_AddImageToPdf = "Pages.CommonEsign.AddImageToPdf";
        public const string Pages_CommonEsign_AddImageToPdfWithDigitalSign = "Pages.CommonEsign.AddImageToPdfWithDigitalSign";
        public const string Pages_CommonEsign_GetInformation = "Pages.CommonEsign.GetInformation";
        public const string Pages_CommonEsign_GetFilePath = "Pages.CommonEsign.GetFilePath";
        public const string Pages_CommonEsign_GetFY = "Pages.CommonEsign.GetFY";
        public const string Pages_CommonEsign_RequestNextApproveForSign = "Pages.CommonEsign.RequestNextApproveForSign";
        public const string Pages_CommonEsign_RequestNextApprove = "Pages.CommonEsign.RequestNextApprove";
        public const string Pages_CommonEsign_RequestNextApproveV2 = "Pages.CommonEsign.RequestNextApproveV2";
        public const string Pages_CommonEsign_SendEmailEsignRequest = "Pages.CommonEsign.SendEmailEsignRequest";
        public const string Pages_CommonEsign_SendEmailEsignRequest_v21 = "Pages.CommonEsign.SendEmailEsignRequest_v21";
        public const string Pages_CommonEsign_SendNoti = "Pages.CommonEsign.SendNoti";
        public const string Pages_CommonEsign_ResizeImage = "Pages.CommonEsign.ResizeImage";
        public const string Pages_CommonEsign_CreateEsignNotification = "Pages.CommonEsign.CreateEsignNotification";
        public const string Pages_CommonEsign_UpdateResultForOtherSystem = "Pages.CommonEsign.UpdateResultForOtherSystem";
        public const string Pages_CommonEsign_GetOtherSystemAuthenToken = "Pages.CommonEsign.GetOtherSystemAuthenToken";
        #endregion

        #region CommonEsignWeb
        public const string Pages_CommonEsignWeb = "Pages.CommonEsignWeb";
        public const string Pages_CommonEsignWeb_SignDocument = "Pages.CommonEsignWeb.SignDocument";
        public const string Pages_CommonEsignWeb_SignDocumentWithOtp = "Pages.CommonEsignWeb.SignDocumentWithOtp";
        public const string Pages_CommonEsignWeb_AuthorizeOTP = "Pages.CommonEsignWeb.AuthorizeOTP";
        public const string Pages_CommonEsignWeb_SignDigitalToPdf = "Pages.CommonEsignWeb.SignDigitalToPdf";
        public const string Pages_CommonEsignWeb_AddImageToPdf = "Pages.CommonEsignWeb.AddImageToPdf";
        public const string Pages_CommonEsignWeb_AddImageToPdfWithDigitalSign = "Pages.CommonEsignWeb.AddImageToPdfWithDigitalSign";
        public const string Pages_CommonEsignWeb_GetInformation = "Pages.CommonEsignWeb.GetInformation";
        public const string Pages_CommonEsignWeb_GetFilePath = "Pages.CommonEsignWeb.GetFilePath";
        public const string Pages_CommonEsignWeb_GetFY = "Pages.CommonEsignWeb.GetFY";
        public const string Pages_CommonEsignWeb_RequestNextApproveForSign = "Pages.CommonEsignWeb.RequestNextApproveForSign";
        public const string Pages_CommonEsignWeb_RequestNextApprove = "Pages.CommonEsignWeb.RequestNextApprove";
        public const string Pages_CommonEsignWeb_RequestNextApproveV2 = "Pages.CommonEsignWeb.RequestNextApproveV2";
        public const string Pages_CommonEsignWeb_SendEmailEsignRequest = "Pages.CommonEsignWeb.SendEmailEsignRequest";
        public const string Pages_CommonEsignWeb_SendEmailWithContet = "Pages.CommonEsignWeb.SendEmailWithContet";
        public const string Pages_CommonEsignWeb_SendNoti = "Pages.CommonEsignWeb.SendNoti";
        public const string Pages_CommonEsignWeb_SendEmailEsignRequest_v21 = "Pages.CommonEsignWeb.SendEmailEsignRequest_v21";
        public const string Pages_CommonEsignWeb_ResizeImage = "Pages.CommonEsignWeb.ResizeImage";
        public const string Pages_CommonEsignWeb_CreateEsignNotification = "Pages.CommonEsignWeb.CreateEsignNotification";
        public const string Pages_CommonEsignWeb_TestSignSync = "Pages.CommonEsignWeb.TestSignSync";
        #endregion

        #region CommonLookup
        public const string Pages_CommonLookup = "Pages.CommonLookup";
        public const string Pages_CommonLookup_GetEditionsForCombobox = "Pages.CommonLookup.GetEditionsForCombobox";
        public const string Pages_CommonLookup_FindUsers = "Pages.CommonLookup.FindUsers";
        public const string Pages_CommonLookup_GetDefaultEditionName = "Pages.CommonLookup.GetDefaultEditionName";
        #endregion

        #region DashboardCustomization
        public const string Pages_DashboardCustomization = "Pages.DashboardCustomization";
        public const string Pages_DashboardCustomization_GetUserDashboard = "Pages.DashboardCustomization.GetUserDashboard";
        public const string Pages_DashboardCustomization_SavePage = "Pages.DashboardCustomization.SavePage";
        public const string Pages_DashboardCustomization_RenamePage = "Pages.DashboardCustomization.RenamePage";
        public const string Pages_DashboardCustomization_AddNewPage = "Pages.DashboardCustomization.AddNewPage";
        public const string Pages_DashboardCustomization_DeletePage = "Pages.DashboardCustomization.DeletePage";
        public const string Pages_DashboardCustomization_AddWidget = "Pages.DashboardCustomization.AddWidget";
        public const string Pages_DashboardCustomization_GetDashboardDefinition = "Pages.DashboardCustomization.GetDashboardDefinition";
        public const string Pages_DashboardCustomization_GetAllWidgetDefinitions = "Pages.DashboardCustomization.GetAllWidgetDefinitions";
        public const string Pages_DashboardCustomization_GetAllAvailableWidgetDefinitionsForPage = "Pages.DashboardCustomization.GetAllAvailableWidgetDefinitionsForPage";
        public const string Pages_DashboardCustomization_GetSettingName = "Pages.DashboardCustomization.GetSettingName";
        #endregion

        #region DemoUiComponents
        public const string Pages_DemoUiComponents = "Pages.DemoUiComponents";
        //public const string Pages_DemoUiComponents_SendAndGetDate = "Pages.DemoUiComponents.SendAndGetDate";
        //public const string Pages_DemoUiComponents_SendAndGetDateTime = "Pages.DemoUiComponents.SendAndGetDateTime";
        //public const string Pages_DemoUiComponents_SendAndGetDateRange = "Pages.DemoUiComponents.SendAndGetDateRange";
        //public const string Pages_DemoUiComponents_SendAndGetDateWithText = "Pages.DemoUiComponents.SendAndGetDateWithText";
        //public const string Pages_DemoUiComponents_GetCountries = "Pages.DemoUiComponents.GetCountries";
        //public const string Pages_DemoUiComponents_SendAndGetSelectedCountries = "Pages.DemoUiComponents.SendAndGetSelectedCountries";
        //public const string Pages_DemoUiComponents_SendAndGetValue = "Pages.DemoUiComponents.SendAndGetValue";
        #endregion

        #region DynamicEntityProperty
        public const string Pages_DynamicEntityProperty = "Pages.DynamicEntityProperty";
        public const string Pages_DynamicEntityProperty_Get = "Pages.DynamicEntityProperty.Get";
        public const string Pages_DynamicEntityProperty_GetAllPropertiesOfAnEntity = "Pages.DynamicEntityProperty.GetAllPropertiesOfAnEntity";
        public const string Pages_DynamicEntityProperty_GetAll = "Pages.DynamicEntityProperty.GetAll";
        public const string Pages_DynamicEntityProperty_Add = "Pages.DynamicEntityProperty.Add";
        public const string Pages_DynamicEntityProperty_Update = "Pages.DynamicEntityProperty.Update";
        public const string Pages_DynamicEntityProperty_Delete = "Pages.DynamicEntityProperty.Delete";
        public const string Pages_DynamicEntityProperty_GetAllEntitiesHasDynamicProperty = "Pages.DynamicEntityProperty.GetAllEntitiesHasDynamicProperty";
        #endregion

        #region DynamicEntityPropertyDefinition
        public const string Pages_DynamicEntityPropertyDefinition = "Pages.DynamicEntityPropertyDefinition";
        #endregion

        #region DynamicEntityPropertyValue
        public const string Pages_DynamicEntityPropertyValue = "Pages.DynamicEntityPropertyValue";
        public const string Pages_DynamicEntityPropertyValue_Add = "Pages.DynamicEntityPropertyValue.Add";
        public const string Pages_DynamicEntityPropertyValue_Update = "Pages.DynamicEntityPropertyValue.Update";
        public const string Pages_DynamicEntityPropertyValue_Delete = "Pages.DynamicEntityPropertyValue.Delete";
        public const string Pages_DynamicEntityPropertyValue_GetAllDynamicEntityPropertyValues = "Pages.DynamicEntityPropertyValue.GetAllDynamicEntityPropertyValues";
        public const string Pages_DynamicEntityPropertyValue_InsertOrUpdateAllValues = "Pages.DynamicEntityPropertyValue.InsertOrUpdateAllValues";
        public const string Pages_DynamicEntityPropertyValue_CleanValues = "Pages.DynamicEntityPropertyValue.CleanValues";

        #endregion
        #endregion

        #region "Fix pentest lam" 
        public const string Pages_UserLink = "Pages.UserLink";
        public const string Pages_UserDelegation = "Pages.UserDelegation";
        public const string Pages_Upload = "Pages.Upload";
        public const string Pages_Upload_UploadFile = "Pages.Upload.UploadFile";
        public const string Pages_Upload_UploadAdditionalFile = "Pages.Upload.UploadAdditionalFile";
        public const string Pages_Upload_DownloadFile = "Pages.Upload.DownloadFile";
        public const string Pages_Upload_UploadSignature = "Pages.Upload.UploadSignature"; 
        public const string Pages_UiCustomizationSettings = "Pages.Upload.UiCustomizationSettings";
        public const string Pages_Twitter = "Pages.Twitter";
        public const string Pages_SchedulerSync = "Pages.SchedulerSync";
        #endregion
    }
}
