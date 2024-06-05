using Abp.Application.Services;
using esign.Dto;
using esign.Esign.Master.MstActivityHistory.Dto.Ver1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace esign.Esign.Master.MstActivityHistory.Exporting.Ver1
{
    public interface IMstActivityHistoryExcelExporter : IApplicationService
    {
        FileDto ExportToFile(List<MstActivityHistoryOutputDto> activityHistory);
    }
}
