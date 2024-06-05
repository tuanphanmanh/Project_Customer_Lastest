using esign.DataExporting.Excel.NPOI;
using esign.Dto;
using esign.Esign.Master.MstEsignSystems.Dto.Ver1;
using esign.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace esign.Esign.Master.MstEsignSystems.Exporting.Ver1
{
    internal class MstEsignSystemsExcelExporter : NpoiExcelExporterBase, IMstEsignSystemsExcelExporter
    {
        public MstEsignSystemsExcelExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager) { }

        public FileDto ExportToFile(List<MstEsignSystemsOutputDto> system)
        {
            return CreateExcelPackage(
                "Systems.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("Systems");
                    AddHeader(
                                sheet,
                                ("Code"),
                                ("LocalName"),
                                ("InternationalName"),
                                ("LocalDescription"),
                                ("InternationalDescription")
                               );
                    AddObjects(
                         sheet, system,
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
