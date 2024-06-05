using Abp.Application.Services;
using esign.Ver1.Esign.Business.EsignApiOtherSystem.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace esign.Business.Ver1
{
    public interface IEsignApiOtherSystemAppService : IApplicationService
    { 
        Task<string> CreateOrEditEsignRequestOtherSystem(CreateOrEditEsignApiOtherSystemDto input);
        Task<List<DocumentFromSystemDto>> SignDocumentFromOtherSystem(SignDocumentOtherSystemDto signDocumentOtherSystemDto);
        Task RejectRequestFromOtherSystem(RejectFromOtherSystemDto rejectFromOtherSystemDto);
    }

}


