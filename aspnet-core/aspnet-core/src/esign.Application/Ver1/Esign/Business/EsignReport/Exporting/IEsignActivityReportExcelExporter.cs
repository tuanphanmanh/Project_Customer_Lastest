using Abp.Application.Services;
using esign.Dto;
using esign.Ver1.Esign.Business.EsignReport.Dto;
using System.Collections.Generic;

namespace esign.Esign.Ver1.Business.EsignReport
{
    public interface IEsignActivityReportExcelExporter : IApplicationService
    {
        FileDto ExportToFile(List<EsignActivityReportDto> activityHistory);
    }
}
