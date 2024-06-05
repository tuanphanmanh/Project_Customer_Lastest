using esign.DataExporting.Excel.NPOI;
using esign.Dto;
using esign.Storage;
using System.Collections.Generic;

namespace esign.Esign.Master.Ver1
{
    internal class MstEsignColorExcelExporter : NpoiExcelExporterBase, IMstEsignColorExcelExporter
    {
        public MstEsignColorExcelExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager) { }
        public FileDto ExportToFile(List<MstEsignColorWebOutputDto> divison)
        {
            return CreateExcelPackage(
                "Color.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("Color");
                    AddHeader(
                                sheet,
                                ("Code"),
                                ("Name"),
                                ("Order")
                               );
                    AddObjects(
                         sheet, divison,
                                _ => _.Code,
                                _ => _.Name,
                                _ => _.Order
                                );
                });

        }

    }
}
