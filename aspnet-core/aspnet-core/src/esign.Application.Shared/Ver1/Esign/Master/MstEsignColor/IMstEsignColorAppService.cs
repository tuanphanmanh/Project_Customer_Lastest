using Abp.Application.Services;
using Abp.Application.Services.Dto;
using esign.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace esign.Esign.Master.Ver1
{
    public interface IMstEsignColorAppService: IApplicationService
    {
        Task CreateOrEdit(CreateOrEditMstEsignColorInputDto input);
        Task Delete(EntityDto input);
        Task<ListResultDto<MstEsignColorOutputDto>> GetAllColor(MstEsignColorInputDto input);
        Task<PagedResultDto<MstEsignColorWebOutputDto>> GetAllMstEsignColor(MstEsignColorWebInputDto input);
        Task<FileDto> GetColorExcel(MstEsignColorWebInputDto input);
    }
}
