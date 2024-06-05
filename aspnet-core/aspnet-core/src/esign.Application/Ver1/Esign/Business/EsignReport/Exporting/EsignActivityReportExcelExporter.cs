using esign.DataExporting.Excel.NPOI;
using esign.Dto;
using esign.Storage;
using esign.Ver1.Esign.Business.EsignReport.Dto;
using System.Collections.Generic;

namespace esign.Esign.Ver1.Business.EsignReport
{
    public class EsignActivityReportExcelExporter : NpoiExcelExporterBase, IEsignActivityReportExcelExporter
    {
        public EsignActivityReportExcelExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager) { }

        public FileDto ExportToFile(List<EsignActivityReportDto> activityHistory)
        {
            return CreateExcelPackage(
                "ActivityLogsReport.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("ActivityLogsReport");
                    AddHeader(
                                sheet,
                                ("Name"),
                                ("Username"),
                                ("EmailAddress"),
                                ("Request"),
                                ("Rejected"),
                                ("Viewed"),
                                ("Shared"),
                                ("Signed"),
                                ("Transferred"),
                                ("AdditionalRefDoc"),
                                ("Reminded"),
                                ("Commented"),
                                ("Revoked"),
                                ("Total")
                               );
                    AddObjects(
                         sheet, activityHistory,
                                _ => _.Name,
                                _ => _.Username,
                                _ => _.EmailAddress,
                                _ => _.Request,
                                _ => _.Rejected,
                                _ => _.Viewed,
                                _ => _.Shared,
                                _ => _.Signed,
                                _ => _.Transferred,
                                _ => _.AdditionalRefDoc,
                                _ => _.Reminded,
                                _ => _.Commented,
                                _ => _.Revoked,
                                _ => _.Total
                                );
                });

        }
    }
}
