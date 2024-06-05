using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Dapper.Repositories;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.UI;
using esign.Authorization;
using esign.Business.Dto.Ver1;
using esign.Dto;
using esign.Esign;
using esign.Notifications;
using esign.Url;
using esign.Ver1.Notifications.Dto;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using esign.Helper.Ver1;
using esign.Authorization.Users;

namespace esign.Business.Ver1
{
    [AbpAuthorize]
    public class EsignSignerNotificationAppService : esignVersion1AppServiceBase, IEsignSignerNotificationAppService
    {
        private readonly IDapperRepository<EsignSignerNotification, long> _dapperRepo;
        private readonly IRepository<EsignSignerNotification, long> _notiRepo;
        private readonly IWebUrlService _webUrlService;
        private readonly IRepository<EsignFCMDeviceToken, long> _esignFCMDeviceToken;

        public EsignSignerNotificationAppService(
            IDapperRepository<EsignSignerNotification, long> dapperRepo,
            IRepository<EsignSignerNotification, long> notiRepo,
            IRepository<EsignFCMDeviceToken, long> esignFCMDeviceToken,
            IWebUrlService webUrlService
        )
        {
            _dapperRepo = dapperRepo;
            _notiRepo = notiRepo;
            _esignFCMDeviceToken = esignFCMDeviceToken;
            _webUrlService = webUrlService;
        }
        [HttpGet]
        [AbpAuthorize(AppPermissions.Pages_Business_ViewNotification_View)]
        public async Task<EsignSignerNotificationResponseDto> GetUserNotification(int typeId, int tabTypeId, long skipCount, long maxResultCount)
        {
            long UserId = AbpSession.UserId.Value;
            var result = await _dapperRepo.QueryAsync<EsignSignerNotificationResultDto>(
                "exec Sp_EsignSignerNotification_GetUserNotification @p_UserId, @p_TypeId, @p_TabTypeId, @p_DomainUrl, @p_SkipCount, @p_MaxResultCount",
                new
                {
                    @p_UserId = AbpSession.UserId,
                    @p_TypeId = typeId,
                    @p_TabTypeId = tabTypeId,
                    @p_DomainUrl = _webUrlService.ServerRootAddressFormat.EnsureEndsWith('/'),
                    @p_SkipCount = skipCount,
                    @p_MaxResultCount = maxResultCount
                }
            );

            var resultCount = await _dapperRepo.QueryAsync<string>(
                "exec Sp_EsignSignerNotification_GetUserNotificationCount @p_UserId, @p_TypeId, @p_TabTypeId",
                new
                {
                    @p_UserId = AbpSession.UserId,
                    @p_TypeId = typeId,
                    @p_TabTypeId = tabTypeId
                }
            );

            var resultUnreadCount = await _dapperRepo.QueryAsync<EsignSignerNotificationUnreadResultDto>(
                "exec Sp_EsignSignerNotification_GetUserNotificationUnreadCount @p_UserId",
                new
                {
                    @p_UserId = AbpSession.UserId
                }
            );

            var response = new EsignSignerNotificationResponseDto
            {
                TotalCount = int.Parse(resultCount.FirstOrDefault() ?? "0"),
                Notifications = ObjectMapper.Map<List<EsignSignerNotificationDto>>(result.ToList())
            };

            ObjectMapper.Map(resultUnreadCount.FirstOrDefault() ?? new EsignSignerNotificationUnreadResultDto(), response);


            /*
              * Mobile tự update lại Badge
                // pust badge on moible
                var deviceTokens = _esignFCMDeviceToken.GetAll().Where(f => f.CreatorUserId == UserId).Select(f => f.DeviceToken).Distinct().ToList();
                if (deviceTokens != null && deviceTokens.Count > 0)
                {
                    var resultBadge = await _dapperRepo.QueryAsync<EsignSignerNotificationUnreadResultDto>(
                        "exec Sp_EsignSignerNotification_GetUserBadgeUnreadCount @p_UserId",
                        new { @p_UserId = UserId }
                    );
                    EsignSignerNotificationUnreadResultDto a = resultBadge.FirstOrDefault() ?? new EsignSignerNotificationUnreadResultDto();
                    PushNotificationBadgeDto notification = new PushNotificationBadgeDto();
                    notification.Badge = a.TotalAllUnread;
                    await NotificationHelper.PushNotificationBadge(notification, deviceTokens);
                }
             */


            return response;
        }
        [HttpPost]
        [AbpAuthorize(AppPermissions.Pages_Business_ViewNotification_View, AppPermissions.Pages_Business_ViewNotification_MarkAsReadAll)]
        public async Task<SavedResultDto> UpdateNotificationStatus(UpdateNotificationStatusInput input)
        {
            long UserId = AbpSession.UserId.Value;
            await _dapperRepo.ExecuteAsync(
                "exec Sp_EsignSignerNotification_UpdateUserNotification @p_UserId, @p_IsUpdateAll, @p_NotificationId, @p_IsRead, @p_TabTypeId",
                new
                {
                    @p_UserId = AbpSession.UserId,
                    @p_IsUpdateAll = input.IsUpdateAll,
                    @p_NotificationId = input.NotificationId,
                    @p_IsRead = input.IsRead,
                    @p_TabTypeId = input.TabTypeId
                }
            );

            /*
             * Mobile tự update lại Badge
             
                // pust badge on moible
                var deviceTokens = _esignFCMDeviceToken.GetAll().Where(f => f.CreatorUserId == UserId).Select(f => f.DeviceToken).Distinct().ToList();
                if (deviceTokens != null && deviceTokens.Count > 0)
                {
                    var resultUnreadCount = await _dapperRepo.QueryAsync<EsignSignerNotificationUnreadResultDto>(
                        "exec Sp_EsignSignerNotification_GetUserBadgeUnreadCount @p_UserId",
                        new { @p_UserId = UserId }
                    );
                    EsignSignerNotificationUnreadResultDto a = resultUnreadCount.FirstOrDefault() ?? new EsignSignerNotificationUnreadResultDto();
                    PushNotificationBadgeDto notification = new PushNotificationBadgeDto();
                    notification.Badge = a.TotalAllUnread;
                    await NotificationHelper.PushNotificationBadge(notification, deviceTokens);
                }

             */

            return new SavedResultDto { IsSave = true };
        }

        [HttpPost]
        [AbpAuthorize(AppPermissions.Pages_Business_ViewNotification_Delete)]
        public async Task<SavedResultDto> DeleteNotification(EntityDto<long> input)
        {
            var checkNoti = _notiRepo.FirstOrDefault(e => e.Id == input.Id);
            if (checkNoti != null)
            {
                await _notiRepo.DeleteAsync(checkNoti);
                return new SavedResultDto { IsSave = false };
            }
            else
                throw new UserFriendlyException(404, "NotificationNotFound");
        }
    }
}
