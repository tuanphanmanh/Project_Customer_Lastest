using Abp.Application.Services;
using esign.Dto;
using esign.Master.Dto.Ver1;
using System.Collections.Generic;

namespace esign.Master.Department.Exporting.Ver1
{
    public interface IMstEsignDepartmentExcelExporter : IApplicationService
    {
        FileDto ExportToFile(List<MstEsignDepartmentOutputDto> divison);
    }
}
