using Abp.Application.Services;
using esign.Dto;
using System.Collections.Generic;

namespace esign.Esign.Master.Ver1
{
    public interface IMstEsignColorExcelExporter : IApplicationService
    {
        FileDto ExportToFile(List<MstEsignColorWebOutputDto> color);
    }
}
