﻿using Abp;
using Abp.Authorization.Users;
using Abp.BackgroundJobs;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using esign.Notifications;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace esign.Ver1.Notifications
{
    public class SendNotificationToAllUsersBackgroundJob : AsyncBackgroundJob<SendNotificationToAllUsersArgs>,
        ITransientDependency
    {
        private const int MaxUserCount = 1000;

        private readonly IRepository<UserAccount, long> _userAccountRepository;
        private readonly IAppNotifier _appNotifier;

        public SendNotificationToAllUsersBackgroundJob(IRepository<UserAccount, long> userAccountRepository,
            IAppNotifier appNotifier)
        {
            _userAccountRepository = userAccountRepository;
            _appNotifier = appNotifier;
        }

        public override async Task ExecuteAsync(SendNotificationToAllUsersArgs toAllUsersArgs)
        {
            var userCount = await UnitOfWorkManager.WithUnitOfWorkAsync(async () =>
                await _userAccountRepository.GetAll().LongCountAsync()
            );

            if (userCount == 0)
            {
                return;
            }
            var loopCount = userCount / MaxUserCount + 1;

            for (var i = 0; i < loopCount; i++)
            {
                var userIds = await UnitOfWorkManager.WithUnitOfWorkAsync(async () =>
                {
                    return await _userAccountRepository.GetAll().Skip(i * MaxUserCount)
                        .AsNoTracking()
                        .Take(MaxUserCount)
                        .Select(u => new UserIdentifier(u.TenantId, u.Id))
                        .ToArrayAsync();
                });

                await _appNotifier.SendMessageAsync(
                    toAllUsersArgs.NotificationName,
                    toAllUsersArgs.Message,
                    userIds,
                    toAllUsersArgs.Severity
                );
            }
        }
    }
}
