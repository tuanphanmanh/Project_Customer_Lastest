using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp;
using Abp.Application.Services.Dto;
using Abp.Auditing;
using Abp.Authorization;
using Abp.Authorization.Users;
using Abp.BackgroundJobs;
using Abp.Collections.Extensions;
using Abp.Configuration;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Abp.Notifications;
using Abp.Organizations;
using Abp.Runtime.Session;
using Abp.UI;
using Microsoft.EntityFrameworkCore;
using esign.Authorization;
using esign.Authorization.Users;
using esign.Notifications.Dto.Ver1;
using esign.Organizations;
using esign.Esign;
using Microsoft.AspNetCore.Mvc;
using esign.Master;
using esign.Helper.Ver1;
using esign.Ver1.Notifications;

namespace esign.Notifications.Ver1
{
    [AbpAuthorize]
    public class NotificationAppService : esignVersion1AppServiceBase, INotificationAppService
    {
        //private readonly IRepository<MstEsignStatus, int> _mstEsignStatusRepo;
        //private readonly IWebUrlService _webUrlService;
        //private readonly ICommonEmailAppService _commonEmailAppService;
        //private readonly IRepository<EsignSignerList, long> _esignSignerListRepo;
        //private readonly IRepository<MstEsignStatus, int> _esignStatusRepo;

        //public ChatController(IRepository<MstEsignStatus, int> mstEsignStatusRepo, IRepository<MstEsignStatus, int> esignStatusRepo, IRepository<EsignSignerList, long> esignSignerListRepo, IWebUrlService webUrlService, ICommonEmailAppService commonEmailAppService)
        //{
        //    _mstEsignStatusRepo = mstEsignStatusRepo;
        //    _webUrlService = webUrlService;
        //    _commonEmailAppService = commonEmailAppService;
        //    _esignSignerListRepo = esignSignerListRepo;
        //    _esignStatusRepo = esignStatusRepo;
        //}
        private readonly INotificationDefinitionManager _notificationDefinitionManager;
        private readonly IUserNotificationManager _userNotificationManager;
        private readonly INotificationSubscriptionManager _notificationSubscriptionManager;
        private readonly IRepository<User, long> _userRepository;
        private readonly IRepository<OrganizationUnit, long> _organizationUnitRepository;
        private readonly IAppNotifier _appNotifier;
        private readonly IUserOrganizationUnitRepository _userOrganizationUnitRepository;
        private readonly INotificationConfiguration _notificationConfiguration;
        private readonly INotificationStore _notificationStore;
        private readonly IBackgroundJobManager _backgroundJobManager;
        private readonly IRepository<UserNotificationInfo, Guid> _userNotificationRepository;
        private readonly IRepository<EsignFCMDeviceToken, long> _esignFCMDeviceToken;
        private readonly IRepository<EsignSignerNotification, long> _esignSignerNotificationRepo;
        private readonly IRepository<EsignSignerNotificationDetail, long> _esignSignerNotificationDetailRepo;
        private readonly IRepository<EsignSignerList, long> _esignSignerListRepo;
        private readonly IRepository<MstEsignStatus, int> _esignStatusRepo;
        public NotificationAppService(
            IRepository<MstEsignStatus, int> esignStatusRepo,
            IRepository<EsignSignerList, long> esignSignerListRepo,
            INotificationDefinitionManager notificationDefinitionManager,
            IUserNotificationManager userNotificationManager,
            INotificationSubscriptionManager notificationSubscriptionManager,
            IRepository<User, long> userRepository,
            IRepository<OrganizationUnit, long> organizationUnitRepository,
            IAppNotifier appNotifier,
            IUserOrganizationUnitRepository userOrganizationUnitRepository,
            INotificationConfiguration notificationConfiguration,
            INotificationStore notificationStore,
            IBackgroundJobManager backgroundJobManager,
            IRepository<UserNotificationInfo, Guid> userNotificationRepository,
            IRepository<EsignFCMDeviceToken, long> esignFCMDeviceToken,
            IRepository<EsignSignerNotification, long> esignSignerNotificationRepo,
            IRepository<EsignSignerNotificationDetail, long> esignSignerNotificationDetailRepo)
        {
            _notificationDefinitionManager = notificationDefinitionManager;
            _userNotificationManager = userNotificationManager;
            _notificationSubscriptionManager = notificationSubscriptionManager;
            _userRepository = userRepository;
            _organizationUnitRepository = organizationUnitRepository;
            _appNotifier = appNotifier;
            _userOrganizationUnitRepository = userOrganizationUnitRepository;
            _notificationConfiguration = notificationConfiguration;
            _notificationStore = notificationStore;
            _backgroundJobManager = backgroundJobManager;
            _userNotificationRepository = userNotificationRepository;
            _esignFCMDeviceToken = esignFCMDeviceToken;
            _esignSignerNotificationRepo = esignSignerNotificationRepo;
            _esignSignerNotificationDetailRepo = esignSignerNotificationDetailRepo;
            _esignSignerListRepo = esignSignerListRepo;
            _esignStatusRepo = esignStatusRepo;
        }

        [HttpPost]
        [AbpAuthorize(AppPermissions.Pages_Notification_SendDeviceToken)]
        public async Task<long> SendDeviceToken([FromBody] DeviceTokenDto DeviceToken)
        {
            long deviceTokenId = 0;
            var input = new EsignFCMDeviceToken();
            input.DeviceToken = DeviceToken.DeviceToken;
            deviceTokenId = await _esignFCMDeviceToken.InsertAndGetIdAsync(input);
            return deviceTokenId;
        }
        [HttpPost]
        [AbpAuthorize(AppPermissions.Pages_Notification_DeleteDeviceToken)]

        public async Task DeleteDeviceToken([FromBody] DeviceTokenIdDto DeviceTokenId)
        {
            await _esignFCMDeviceToken.DeleteAsync(id: DeviceTokenId.DeviceTokenId);
        }

        [HttpPost]
        [AbpAuthorize(AppPermissions.Pages_Notification_CreateEsignNotification)]

        public async Task CreateEsignNotification(NotificationInputDto input)
        {

            try
            {
                var deviceTokens = _esignFCMDeviceToken.GetAll().Where(f => f.CreatorUserId == input.notification.UserId).Select(f => f.DeviceToken).Distinct().ToList();
                if (deviceTokens.Count != 0) await NotificationHelper.SendFCMNotification(input, deviceTokens);
                var newNotification = ObjectMapper.Map<EsignSignerNotification>(input.notification);
                long newNotificationId = await _esignSignerNotificationRepo.InsertAndGetIdAsync(newNotification);
                foreach (var notificationDetail in input.notification.NotificationDetail)
                {
                    var newNotificationDetail = ObjectMapper.Map<EsignSignerNotificationDetail>(notificationDetail);
                    newNotificationDetail.NotificationId = newNotificationId;
                    await _esignSignerNotificationDetailRepo.InsertAsync(newNotificationDetail);
                }
            }
            catch (UserFriendlyException ex)
            {
                throw ex;
            }

        }

        [DisableAuditing]
        [HttpGet]
        [AbpAuthorize(AppPermissions.Pages_Notification_GetUserNotifications)]

        public async Task<GetNotificationsOutput> GetUserNotifications([FromQuery] GetUserNotificationsInput input)
        {
            var totalCount = await _userNotificationManager.GetUserNotificationCountAsync(
                AbpSession.ToUserIdentifier(), input.State, input.StartDate, input.EndDate
            );

            var unreadCount = await _userNotificationManager.GetUserNotificationCountAsync(
                AbpSession.ToUserIdentifier(), UserNotificationState.Unread, input.StartDate, input.EndDate
            );
            var notifications = await _userNotificationManager.GetUserNotificationsAsync(
                AbpSession.ToUserIdentifier(), input.State, input.SkipCount, input.MaxResultCount, input.StartDate,
                input.EndDate
            );

            return new GetNotificationsOutput(totalCount, unreadCount, notifications);
        }
        [HttpPost]
        [AbpAuthorize(AppPermissions.Pages_Notification_ShouldUserUpdateApp)]

        public async Task<bool> ShouldUserUpdateApp()
        {
            var notifications = await _userNotificationManager.GetUserNotificationsAsync(
                AbpSession.ToUserIdentifier(), UserNotificationState.Unread
            );

            return notifications.Any(x => x.Notification.NotificationName == AppNotificationNames.NewVersionAvailable);
        }
        [HttpPost]
        [AbpAuthorize(AppPermissions.Pages_Notification_SetAllAvailableVersionNotificationAsRead)]

        public async Task<SetNotificationAsReadOutput> SetAllAvailableVersionNotificationAsRead()
        {
            var notifications = await _userNotificationManager.GetUserNotificationsAsync(
                AbpSession.ToUserIdentifier(), UserNotificationState.Unread
            );

            var filteredNotifications = notifications
                .Where(x => x.Notification.NotificationName == AppNotificationNames.NewVersionAvailable)
                .ToList();

            if (!filteredNotifications.Any())
            {
                return new SetNotificationAsReadOutput(false);
            }

            foreach (var notification in filteredNotifications)
            {
                if (notification.State == UserNotificationState.Read)
                {
                    continue;
                }

                await _userNotificationManager.UpdateUserNotificationStateAsync(
                    notification.TenantId,
                    notification.Id,
                    UserNotificationState.Read
                );
            }

            return new SetNotificationAsReadOutput(true);
        }
        [HttpPost]
        [AbpAuthorize(AppPermissions.Pages_Notification_SetAllNotificationsAsRead)]

        public async Task SetAllNotificationsAsRead()
        {
            await _userNotificationManager.UpdateAllUserNotificationStatesAsync(
                AbpSession.ToUserIdentifier(),
                UserNotificationState.Read
            );
        }
        [HttpPost]
        [AbpAuthorize(AppPermissions.Pages_Notification_SetNotificationAsRead)]

        public async Task<SetNotificationAsReadOutput> SetNotificationAsRead(EntityDto<Guid> input)
        {
            var userNotification =
                await _userNotificationManager.GetUserNotificationAsync(AbpSession.TenantId, input.Id);
            if (userNotification == null)
            {
                return new SetNotificationAsReadOutput(false);
            }

            if (userNotification.UserId != AbpSession.GetUserId())
            {
                throw new Exception(
                    $"Given user notification id ({input.Id}) is not belong to the current user ({AbpSession.GetUserId()})"
                );
            }

            if (userNotification.State == UserNotificationState.Read)
            {
                return new SetNotificationAsReadOutput(false);
            }

            await _userNotificationManager.UpdateUserNotificationStateAsync(AbpSession.TenantId, input.Id,
                UserNotificationState.Read);
            return new SetNotificationAsReadOutput(true);
        }
        [HttpGet]
        [AbpAuthorize(AppPermissions.Pages_Notification_GetNotificationSettings)]

        public async Task<GetNotificationSettingsOutput> GetNotificationSettings()
        {
            var output = new GetNotificationSettingsOutput();

            output.ReceiveNotifications =
                await SettingManager.GetSettingValueAsync<bool>(NotificationSettingNames.ReceiveNotifications);

            //Get general notifications, not entity related notifications.
            var notificationDefinitions =
                (await _notificationDefinitionManager.GetAllAvailableAsync(AbpSession.ToUserIdentifier())).Where(nd =>
                    nd.EntityType == null);

            output.Notifications =
                ObjectMapper.Map<List<NotificationSubscriptionWithDisplayNameDto>>(notificationDefinitions);

            var subscribedNotifications = (await _notificationSubscriptionManager
                    .GetSubscribedNotificationsAsync(AbpSession.ToUserIdentifier()))
                .Select(ns => ns.NotificationName)
                .ToList();

            output.Notifications.ForEach(n => n.IsSubscribed = subscribedNotifications.Contains(n.Name));

            return output;
        }
        [HttpPost]
        [AbpAuthorize(AppPermissions.Pages_Notification_UpdateNotificationSettings)]

        public async Task UpdateNotificationSettings(UpdateNotificationSettingsInput input)
        {
            await SettingManager.ChangeSettingForUserAsync(AbpSession.ToUserIdentifier(),
                NotificationSettingNames.ReceiveNotifications, input.ReceiveNotifications.ToString());

            foreach (var notification in input.Notifications)
            {
                if (notification.IsSubscribed)
                {
                    await _notificationSubscriptionManager.SubscribeAsync(AbpSession.ToUserIdentifier(),
                        notification.Name);
                } 
                else
                {
                    await _notificationSubscriptionManager.UnsubscribeAsync(AbpSession.ToUserIdentifier(),
                        notification.Name);
                }
            }
        }
        [HttpPost]
        [AbpAuthorize(AppPermissions.Pages_Notification_DeleteNotification)]
                       
        public async Task DeleteNotification([FromQuery] EntityDto<Guid> input)                                                 
        {
            var notification = await _userNotificationManager.GetUserNotificationAsync(AbpSession.TenantId, input.Id);                 
            if (notification == null)                                            
            {                                     
                return;                   
            }                                 
            if (notification.UserId != AbpSession.GetUserId())
            {
                throw new UserFriendlyException(L("ThisNotificationDoesntBelongToYou"));
            }

            await _userNotificationManager.DeleteUserNotificationAsync(AbpSession.TenantId, input.Id);
        }

        [HttpPost]
        [AbpAuthorize(AppPermissions.Pages_Notification_DeleteAllUserNotifications)]

        public async Task DeleteAllUserNotifications([FromQuery] DeleteAllUserNotificationsInput input)
        {
            //Check pentest ok
            await _userNotificationManager.DeleteAllUserNotificationsAsync(
                AbpSession.ToUserIdentifier(),
                input.State,
                input.StartDate,
                input.EndDate);
        }

        //[AbpAuthorize(AppPermissions.Pages_Administration_MassNotification)]
        [HttpGet]
        [AbpAuthorize(AppPermissions.Pages_Notification_GetAllUserForLookupTable)]

        public async Task<PagedResultDto<MassNotificationUserLookupTableDto>> GetAllUserForLookupTable(
            [FromQuery] GetAllForLookupTableInput input)
        {
            var query = _userRepository.GetAll()
                .WhereIf(!string.IsNullOrWhiteSpace(input.Filter),
                    e =>
                        (e.Name != null && e.Name.Contains(input.Filter)) ||
                        (e.Surname != null && e.Surname.Contains(input.Filter)) ||
                        (e.EmailAddress != null && e.EmailAddress.Contains(input.Filter))
                );

            var totalCount = await query.CountAsync();

            var userList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<MassNotificationUserLookupTableDto>();
            foreach (var user in userList)
            {
                lookupTableDtoList.Add(new MassNotificationUserLookupTableDto
                {
                    Id = user.Id,
                    DisplayName = user.Name + " " + user.Surname + " (" + user.EmailAddress + ")"
                });
            }

            return new PagedResultDto<MassNotificationUserLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        //[AbpAuthorize(AppPermissions.Pages_Administration_MassNotification)]
        [HttpGet]
        [AbpAuthorize(AppPermissions.Pages_Notification_GetAllOrganizationUnitForLookupTable)]

        public async Task<PagedResultDto<MassNotificationOrganizationUnitLookupTableDto>>
            GetAllOrganizationUnitForLookupTable([FromQuery] GetAllForLookupTableInput input)
        {
            var query = _organizationUnitRepository.GetAll()
                .WhereIf(!string.IsNullOrWhiteSpace(input.Filter),
                    e => e.DisplayName != null && e.DisplayName.Contains(input.Filter));

            var totalCount = await query.CountAsync();

            var organizationUnitList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<MassNotificationOrganizationUnitLookupTableDto>();
            foreach (var organizationUnit in organizationUnitList)
            {
                lookupTableDtoList.Add(new MassNotificationOrganizationUnitLookupTableDto
                {
                    Id = organizationUnit.Id,
                    DisplayName = organizationUnit.DisplayName
                });
            }

            return new PagedResultDto<MassNotificationOrganizationUnitLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        //[AbpAuthorize(AppPermissions.Pages_Administration_MassNotification_Create)]
        [HttpPost]
        [AbpAuthorize(AppPermissions.Pages_Notification_CreateMassNotification)]

        public async Task CreateMassNotification(CreateMassNotificationInput input)
        {
            if (input.TargetNotifiers.IsNullOrEmpty())
            {
                throw new UserFriendlyException(L("MassNotificationTargetNotifiersFieldIsRequiredMessage"));
            }

            var userIds = new List<UserIdentifier>();

            if (!input.UserIds.IsNullOrEmpty())
            {
                userIds.AddRange(input.UserIds.Select(i => new UserIdentifier(AbpSession.TenantId, i)));
            }

            if (!input.OrganizationUnitIds.IsNullOrEmpty())
            {
                userIds.AddRange(
                    await _userOrganizationUnitRepository.GetAllUsersInOrganizationUnitHierarchical(
                        input.OrganizationUnitIds)
                );
            }

            if (userIds.Count == 0)
            {
                if (input.OrganizationUnitIds.IsNullOrEmpty())
                {
                    // tried to get users from organization, but could not find any user
                    throw new UserFriendlyException(L("MassNotificationNoUsersFoundInOrganizationUnitMessage"));
                }

                throw new UserFriendlyException(L("MassNotificationUserOrOrganizationUnitFieldIsRequiredMessage"));
            }

            var targetNotifiers = new List<Type>();

            foreach (var notifier in _notificationConfiguration.Notifiers)
            {
                if (input.TargetNotifiers.Contains(notifier.FullName))
                {
                    targetNotifiers.Add(notifier);
                }
            }

            await _appNotifier.SendMassNotificationAsync(
                input.Message,
                userIds.DistinctBy(u => u.UserId).ToArray(),
                input.Severity,
                targetNotifiers.ToArray()
            );
        }

        //[AbpAuthorize(AppPermissions.Pages_Administration_NewVersion_Create)]
        [HttpPost]
        [AbpAuthorize(AppPermissions.Pages_Notification_CreateNewVersionReleasedNotification)]

        public async Task CreateNewVersionReleasedNotification()
        {
            var args = new SendNotificationToAllUsersArgs
            {
                NotificationName = AppNotificationNames.NewVersionAvailable,
                Message = L("NewVersionAvailableNotificationMessage")
            };

            await _backgroundJobManager.EnqueueAsync<SendNotificationToAllUsersBackgroundJob, SendNotificationToAllUsersArgs>(args);
        }
        [HttpGet]
        [AbpAuthorize(AppPermissions.Pages_Notification_GetAllNotifiers)]
        public List<string> GetAllNotifiers()
        {
            return _notificationConfiguration.Notifiers.Select(n => n.FullName).ToList();
        }

        //[AbpAuthorize(AppPermissions.Pages_Administration_MassNotification)]
        [HttpGet]
        [AbpAuthorize(AppPermissions.Pages_Notification_GetNotificationsPublishedByUser)]

        public async Task<GetPublishedNotificationsOutput> GetNotificationsPublishedByUser(
            [FromQuery] GetPublishedNotificationsInput input)
        {
            return new GetPublishedNotificationsOutput(
                await _notificationStore.GetNotificationsPublishedByUserAsync(AbpSession.ToUserIdentifier(),
                    AppNotificationNames.MassNotification, input.StartDate, input.EndDate)
            );
        }
    }
}