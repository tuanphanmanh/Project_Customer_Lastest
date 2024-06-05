﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Notifications;
using esign.Notifications.Dto.Ver1;

namespace esign.Notifications.Ver1
{
    public interface INotificationAppService : IApplicationService
    {
        Task<GetNotificationsOutput> GetUserNotifications(GetUserNotificationsInput input);
        
        Task<SetNotificationAsReadOutput> SetAllAvailableVersionNotificationAsRead();
        
        Task SetAllNotificationsAsRead();

        Task<SetNotificationAsReadOutput> SetNotificationAsRead(EntityDto<Guid> input);

        Task<GetNotificationSettingsOutput> GetNotificationSettings();

        Task UpdateNotificationSettings(UpdateNotificationSettingsInput input);

        Task DeleteNotification(EntityDto<Guid> input);

        Task DeleteAllUserNotifications(DeleteAllUserNotificationsInput input);

        Task CreateMassNotification(CreateMassNotificationInput input);
        
        Task CreateNewVersionReleasedNotification();
        
        Task<bool> ShouldUserUpdateApp();

        List<string> GetAllNotifiers();

        Task<GetPublishedNotificationsOutput> GetNotificationsPublishedByUser(GetPublishedNotificationsInput input);
    }
}
