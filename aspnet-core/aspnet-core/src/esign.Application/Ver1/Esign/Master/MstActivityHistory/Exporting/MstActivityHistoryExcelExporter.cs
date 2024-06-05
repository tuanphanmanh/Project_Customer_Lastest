using esign.DataExporting.Excel.NPOI;
using esign.Dto;
using esign.Esign.Master.MstActivityHistory.Dto.Ver1;
using esign.Esign.Master.MstEsignSystems.Dto.Ver1;
using esign.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace esign.Esign.Master.MstActivityHistory.Exporting.Ver1
{
    public class MstActivityHistoryExcelExporter : NpoiExcelExporterBase, IMstActivityHistoryExcelExporter
    {
        public MstActivityHistoryExcelExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager) { }

        public FileDto ExportToFile(List<MstActivityHistoryOutputDto> activityHistory)
        {
            return CreateExcelPackage(
                "ActivityHistory.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("ActivityHistory");
                    AddHeader(
                                sheet,
                                ("Code"),
                                ("Description")
                               );
                    AddObjects(
                         sheet, activityHistory,
                                _ => _.Code,
                                _ => _.Description
                                );
                });

        }
    }
}
