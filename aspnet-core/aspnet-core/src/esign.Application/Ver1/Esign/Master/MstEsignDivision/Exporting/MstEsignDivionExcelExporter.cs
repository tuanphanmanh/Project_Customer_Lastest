using esign.DataExporting.Excel.NPOI;
using esign.Dto;
using esign.Master.Divison.Exporting.Ver1;
using esign.Master.Dto.Ver1;
using esign.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace esign.Master.Division.Exporting.Ver1
{
    public class MstEsignDivionExcelExporter : NpoiExcelExporterBase, IMstEsignDivionExcelExporter
    {
        public MstEsignDivionExcelExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager) { }
        public FileDto ExportToFile(List<MstEsignDivisionOutputDto> divison)
        {
            return CreateExcelPackage(
                "Divison.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("Division");
                    AddHeader(
                                sheet,
                                ("Code"),
                                ("LocalName"),
                                ("InternationalName"),
                                ("LocalDescription"),
                                ("InternationalDescription")
                               );
                    AddObjects(
                         sheet, divison,
                                _ => _.Code,
                                _ => _.LocalName,
                                _ => _.InternationalName,
                                _ => _.LocalDescription,
                                _ => _.InternationalDescription
                                );
                });

        }

    }
}
