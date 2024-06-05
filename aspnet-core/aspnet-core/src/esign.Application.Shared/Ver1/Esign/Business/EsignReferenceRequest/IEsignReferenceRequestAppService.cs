using Abp.Application.Services;
using esign.Business.Dto.Ver1;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace esign.Ver1.Esign.Business.EsignReferenceRequest
{
    public interface IEsignReferenceRequestAppService : IApplicationService
    {
        Task CreateOrEditReferenceRequest(CreatOrEditEsignReferenceRequestDto input);
    }
}
