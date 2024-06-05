using Abp.Application.Services;
using Abp.Application.Services.Dto;
using esign.Dto;
using esign.Master.Dto.Ver1;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace esign.Master.Ver1
{
    public interface IMstEsignDepartmentAppService : IApplicationService
    {
        Task<PagedResultDto<MstEsignDepartmentOutputDto>> GetAllDepartment(MstEsignDepartmentInputDto input);

        //Task CreateOrEdit(CreateOrEditMstEsignDepartmentInputDto input);

        //Task Delete(EntityDto input);

        Task<FileDto> GetDepartmentExcel(MstEsignDepartmentInputDto input);
    }
}
