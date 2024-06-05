using Abp.Application.Services;
using esign.Dto;
using esign.Master.Dto.Ver1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace esign.Master.Divison.Exporting.Ver1
{
    public interface IMstEsignDivionExcelExporter : IApplicationService
    {
        FileDto ExportToFile(List<MstEsignDivisionOutputDto> divison);
    }
}
