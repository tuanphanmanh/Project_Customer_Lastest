using Abp.Application.Services;
using esign.Dto;
using esign.Logging.Dto.Ver1;

namespace esign.Logging.Ver1
{
    public interface IWebLogAppService : IApplicationService
    {
        GetLatestWebLogsOutput GetLatestWebLogs();

        FileDto DownloadWebLogs();
    }
}
