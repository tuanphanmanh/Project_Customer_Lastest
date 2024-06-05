using Abp.Application.Services;
using System.Threading.Tasks;

namespace esign.Business.Ver1
{
    public interface IEsignFollowUpAppService : IApplicationService
    {
        Task FollowUpRequest(CreateOrEditEsignFollowUpInputDto input);    
    }
}
