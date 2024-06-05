using Abp.Application.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace esign.Ver1.Common
{
    public interface ICommonEmailAppService : IApplicationService
    {
        Task SendEmailEsignRequestTemp(long reqId, string emailCode, long fromUserId, long toUserId);
        Task SendEmailEsignRequest(long reqId, string emailCode, long fromUserId, long toUserId);
        Task SendEmailEsignRequest_v21(long reqId, string emailCode, long fromUserId, long toUserId, string CC, string BCC, string Note);
    }
}
