using Abp.Application.Editions;
using Abp.Application.Features;
using Abp.Auditing;
using Abp.Authorization;
using Abp.Authorization.Users;
using Abp.DynamicEntityProperties;
using Abp.EntityHistory;
using Abp.Localization;
using Abp.Notifications;
using Abp.Organizations;
using Abp.UI.Inputs;
using Abp.Webhooks;
using AutoMapper;
using IdentityServer4.Extensions;
using esign.Auditing.Dto.Ver1;
using esign.Authorization.Accounts.Dto.Ver1;
using esign.Authorization.Delegation;
using esign.Authorization.Permissions.Dto.Ver1;
using esign.Authorization.Roles;
using esign.Authorization.Roles.Dto.Ver1;
using esign.Authorization.Users;
using esign.Authorization.Users.Delegation.Dto.Ver1;
using esign.Authorization.Users.Dto.Ver1;
using esign.Authorization.Users.Importing.Dto.Ver1;
using esign.Authorization.Users.Profile.Dto.Ver1;
using esign.Chat;
using esign.Chat.Dto.Ver1;
using esign.DynamicEntityProperties.Dto.Ver1;
using esign.Editions;
using esign.Editions.Dto.Ver1;
using esign.Friendships;
using esign.Friendships.Cache;
using esign.Friendships.Dto.Ver1;
using esign.Localization.Dto.Ver1;
using esign.MultiTenancy;
using esign.MultiTenancy.Dto.Ver1;
using esign.MultiTenancy.HostDashboard.Dto.Ver1;
using esign.MultiTenancy.Payments;
using esign.MultiTenancy.Payments.Dto.Ver1;
using esign.Notifications.Dto.Ver1;
using esign.Organizations.Dto.Ver1;
using esign.Sessions.Dto.Ver1;
using esign.WebHooks.Dto.Ver1;
using NUglify.Helpers;
using esign.Master.Dto.Ver1;
using esign.Master;
using esign.Esign.Business.EsignRequest.Dto.Ver1;
using esign.Esign;
using esign.Business.Dto.Ver1;
using esign.Esign.Master.Ver1;
using esign.Business.Ver1;
using esign.Esign.Master.MstActivityHistory.Dto.Ver1;
using esign.Esign.Master.MstEsignCategory.Dto.Ver1;
using esign.Esign.Master.MstEsignSystems.Dto.Ver1;
using System.IO;
using esign.Esign.Master.MstEsignLogo.Dto.Ver1;
using esign.Esign.Business.EsignSignerNotification.Dto.Ver1;
using esign.Esign.Master.MstEsignConfig.Dto.Ver1;
using esign.Esign.Master.MstEsignAffiliate.Dto.Ver1;

namespace esign.Ver1
{
    internal static class CustomDtoMapper
    {
        public static void CreateMappings(IMapperConfigurationExpression configuration)
        {
            //Inputs
            configuration.CreateMap<CheckboxInputType, FeatureInputTypeDto>();
            configuration.CreateMap<SingleLineStringInputType, FeatureInputTypeDto>();
            configuration.CreateMap<ComboboxInputType, FeatureInputTypeDto>();
            configuration.CreateMap<IInputType, FeatureInputTypeDto>()
                .Include<CheckboxInputType, FeatureInputTypeDto>()
                .Include<SingleLineStringInputType, FeatureInputTypeDto>()
                .Include<ComboboxInputType, FeatureInputTypeDto>();
            configuration.CreateMap<StaticLocalizableComboboxItemSource, LocalizableComboboxItemSourceDto>();
            configuration.CreateMap<ILocalizableComboboxItemSource, LocalizableComboboxItemSourceDto>()
                .Include<StaticLocalizableComboboxItemSource, LocalizableComboboxItemSourceDto>();
            configuration.CreateMap<LocalizableComboboxItem, LocalizableComboboxItemDto>();
            configuration.CreateMap<ILocalizableComboboxItem, LocalizableComboboxItemDto>()
                .Include<LocalizableComboboxItem, LocalizableComboboxItemDto>();

            //Chat
            configuration.CreateMap<ChatMessage, ChatMessageDto>();
            configuration.CreateMap<ChatMessage, ChatMessageExportDto>();

            //Feature
            configuration.CreateMap<FlatFeatureSelectDto, Feature>().ReverseMap();
            configuration.CreateMap<Feature, FlatFeatureDto>();

            //Role
            configuration.CreateMap<RoleEditDto, Role>().ReverseMap();
            configuration.CreateMap<Role, RoleListDto>();
            configuration.CreateMap<UserRole, UserListRoleDto>();



            //Edition
            configuration.CreateMap<EditionEditDto, SubscribableEdition>().ReverseMap();
            configuration.CreateMap<EditionCreateDto, SubscribableEdition>();
            configuration.CreateMap<EditionSelectDto, SubscribableEdition>().ReverseMap();
            configuration.CreateMap<SubscribableEdition, EditionInfoDto>();

            configuration.CreateMap<Edition, EditionInfoDto>().Include<SubscribableEdition, EditionInfoDto>();

            configuration.CreateMap<SubscribableEdition, EditionListDto>();
            configuration.CreateMap<Edition, EditionEditDto>();
            configuration.CreateMap<Edition, SubscribableEdition>();
            configuration.CreateMap<Edition, EditionSelectDto>();


            //Payment
            configuration.CreateMap<SubscriptionPaymentDto, SubscriptionPayment>().ReverseMap();
            configuration.CreateMap<SubscriptionPaymentListDto, SubscriptionPayment>().ReverseMap();
            configuration.CreateMap<SubscriptionPayment, SubscriptionPaymentInfoDto>();

            //Permission
            configuration.CreateMap<Permission, FlatPermissionDto>();
            configuration.CreateMap<Permission, FlatPermissionWithLevelDto>();

            //Language
            configuration.CreateMap<ApplicationLanguage, ApplicationLanguageEditDto>();
            configuration.CreateMap<ApplicationLanguage, ApplicationLanguageListDto>();
            configuration.CreateMap<NotificationDefinition, NotificationSubscriptionWithDisplayNameDto>();
            configuration.CreateMap<ApplicationLanguage, ApplicationLanguageEditDto>()
                .ForMember(ldto => ldto.IsEnabled, options => options.MapFrom(l => !l.IsDisabled));

            //Tenant
            configuration.CreateMap<Tenant, RecentTenant>();
            configuration.CreateMap<Tenant, TenantLoginInfoDto>();
            configuration.CreateMap<Tenant, TenantListDto>();
            configuration.CreateMap<TenantEditDto, Tenant>().ReverseMap();
            configuration.CreateMap<CurrentTenantInfoDto, Tenant>().ReverseMap();

            //User
            configuration.CreateMap<User, UserEditDto>()
                .ForMember(dto => dto.Password, options => options.Ignore())
                .ReverseMap()
                .ForMember(user => user.Password, options => options.Ignore());
            configuration.CreateMap<User, UserLoginInfoDto>();
            configuration.CreateMap<User, UserListDto>();
            configuration.CreateMap<User, ChatUserDto>();
            configuration.CreateMap<User, OrganizationUnitUserListDto>();
            configuration.CreateMap<Role, OrganizationUnitRoleListDto>();
            configuration.CreateMap<CurrentUserProfileEditDto, User>().ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            configuration.CreateMap<User, CurrentUserProfileEditDto>();
            configuration.CreateMap<UserLoginAttemptDto, UserLoginAttempt>().ReverseMap();
            configuration.CreateMap<ImportUserDto, User>();

            //AuditLog
            configuration.CreateMap<AuditLog, AuditLogListDto>();
            configuration.CreateMap<EntityChange, EntityChangeListDto>();
            configuration.CreateMap<EntityPropertyChange, EntityPropertyChangeDto>();

            //Friendship
            configuration.CreateMap<Friendship, FriendDto>();
            configuration.CreateMap<FriendCacheItem, FriendDto>();

            //OrganizationUnit
            configuration.CreateMap<OrganizationUnit, OrganizationUnitDto>();

            //Webhooks
            configuration.CreateMap<WebhookSubscription, GetAllSubscriptionsOutput>();
            configuration.CreateMap<WebhookSendAttempt, GetAllSendAttemptsOutput>()
                .ForMember(webhookSendAttemptListDto => webhookSendAttemptListDto.WebhookName,
                    options => options.MapFrom(l => l.WebhookEvent.WebhookName))
                .ForMember(webhookSendAttemptListDto => webhookSendAttemptListDto.Data,
                    options => options.MapFrom(l => l.WebhookEvent.Data));

            configuration.CreateMap<WebhookSendAttempt, GetAllSendAttemptsOfWebhookEventOutput>();

            configuration.CreateMap<DynamicProperty, DynamicPropertyDto>().ReverseMap();
            configuration.CreateMap<DynamicPropertyValue, DynamicPropertyValueDto>().ReverseMap();
            configuration.CreateMap<DynamicEntityProperty, DynamicEntityPropertyDto>()
                .ForMember(dto => dto.DynamicPropertyName,
                    options => options.MapFrom(entity => entity.DynamicProperty.DisplayName.IsNullOrEmpty() ? entity.DynamicProperty.PropertyName : entity.DynamicProperty.DisplayName));
            configuration.CreateMap<DynamicEntityPropertyDto, DynamicEntityProperty>();

            configuration.CreateMap<DynamicEntityPropertyValue, DynamicEntityPropertyValueDto>().ReverseMap();

            //User Delegations
            configuration.CreateMap<CreateUserDelegationDto, UserDelegation>();
            configuration.CreateMap<GetMyProfileOutput, User>().ReverseMap();

            configuration.CreateMap<MstEsignStatus, MstEsignStatusDto>();

            //EsignRequest
            configuration.CreateMap<EsignDocumentList, CreateOrEditDocumentDto>();
            configuration.CreateMap<EsignDocumentList, CreateOrEditDocumentDto>().ReverseMap();

            configuration.CreateMap<EsignRequest, CreateOrEditEsignRequestDto>();
            configuration.CreateMap<EsignRequest, CreateOrEditEsignRequestDto>().ReverseMap();

            configuration.CreateMap<EsignPosition, CreateOrEditPositionsDto>();
            configuration.CreateMap<EsignPosition, CreateOrEditPositionsDto>().ReverseMap();

            configuration.CreateMap<EsignSignerList, CreateOrEditSignersDto>();
            configuration.CreateMap<EsignSignerList, CreateOrEditSignersDto>().ReverseMap();

            //Master

            /*configuration.CreateMap<MstEsignDivisionDto, MstEsignDivision>();
            configuration.CreateMap<MstEsignDivision, MstEsignDivisionDto>();*/
            configuration.CreateMap<CreateOrEditMstEsignDivisionDto, MstEsignDivision>();
            configuration.CreateMap<CreateOrEditMstEsignDepartmentInputDto, MstEsignDepartment>();

            /*configuration.CreateMap<MstEsignDepartmentDto, MstEsignDepartment>();
            configuration.CreateMap<MstEsignDepartment, MstEsignDepartmentDto>();
            configuration.CreateMap<CreateOrEditMstEsignDepartmentDto, MstEsignDepartment>();*/

            configuration.CreateMap<EsignSignerNotificationResultDto, EsignSignerNotificationDto>();
            configuration.CreateMap<CreateOrEditMstEsignColorInputDto, MstEsignColor>().ReverseMap();

            configuration.CreateMap<CreateOrEditMstEsignSystemsDto, MstEsignSystems>().ReverseMap();
            configuration.CreateMap<CreateOrEditMstEsignLogoDto, MstEsignLogo>()
                .ForMember(dto => dto.LogoMinUrl, options => options.Ignore())
                .ForMember(dto => dto.LogoMaxUrl, options => options.Ignore())
                .ReverseMap();

            configuration.CreateMap<CreateOrEditMstEsignStatusInputDto, MstEsignStatus>().ReverseMap();

            configuration.CreateMap<CreateOrEditMstEsignCategoryDto, MstEsignCategory>().ReverseMap();

            configuration.CreateMap<CreateOrEditMstActivityHistoryDto, MstActivityHistory>().ReverseMap();

            configuration.CreateMap<CreateOrEditMstEsignColorInputDto, MstEsignColor>().ReverseMap();

            configuration.CreateMap<CreateOrEditEsignCommentsInputDto, EsignComments>();

            configuration.CreateMap<CreateOrEditEsignActivityHistoryInputDto, EsignActivityHistory>();

            configuration.CreateMap<CreateOrEditEsignFollowUpInputDto, EsignFollowUp>();

            configuration.CreateMap<CreateOrEditEsignApiOtherSystemDto, EsignRequest >();
            configuration.CreateMap<CreatOrEditEsignReferenceRequestDto, EsignReferenceRequest >();

            configuration.CreateMap<CreateSignersFromSystemDto, EsignSignerList>();

            configuration.CreateMap<CreateDocumentFromSystemDto, EsignDocumentList>();

            configuration.CreateMap<CreatePositionsFromSystemDto, EsignPosition>();

            configuration.CreateMap<CreateEsignUserImageCreatedInput, MstEsignUserImage>();

            configuration.CreateMap<CreateOrEditEsignNotificationDto, EsignSignerNotification>();

            configuration.CreateMap<CreateOrEditEsignNotificationDetailDto, EsignSignerNotificationDetail>();

            configuration.CreateMap<EsignSignerNotificationUnreadResultDto, EsignSignerNotificationResponseDto>();

            configuration.CreateMap<EsignRequestMultiAffiliateDto, EsignRequest>();
            configuration.CreateMap<CreateOrEditPositionsMultiAffiliateDto, EsignPosition>();

            configuration.CreateMap<CreatePositionsFromSystemDto, CreateOrEditPositionsDto>();
            configuration.CreateMap<CreatePositionsFromSystemDto, CreateOrEditPositionsDto>().ReverseMap();
            configuration.CreateMap<CreateOrEditMstEsignEmailTemplateDto, MstEsignEmailTemplate>().ReverseMap();
            configuration.CreateMap<CreateOrEditMstEsignConfigDto, MstEsignConfig>().ReverseMap();

            configuration.CreateMap<CreateOrEditStatusSignersMultiAffiliateDto, EsignStatusSignerHistory>();
            configuration.CreateMap<CreateOrEditEsignTransferSignerMultiAffiliateDto, EsignTransferSignerHistory>();

            configuration.CreateMap<MstEsignAffiliate, MstEsignAffiliateDto>();
            configuration.CreateMap<CreateOrEditMstEsignAffiliateDto, MstEsignAffiliate>();

        }
    }
}
