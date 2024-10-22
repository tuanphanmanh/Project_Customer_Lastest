﻿using System.Threading.Tasks;
using Abp.Application.Services;
using esign.Install.Dto.Ver1;

namespace esign.Install.Ver1
{
    public interface IInstallAppService : IApplicationService
    {
        Task Setup(InstallDto input);

        AppSettingsJsonDto GetAppSettingsJson();

        CheckDatabaseOutput CheckDatabase();
    }
}