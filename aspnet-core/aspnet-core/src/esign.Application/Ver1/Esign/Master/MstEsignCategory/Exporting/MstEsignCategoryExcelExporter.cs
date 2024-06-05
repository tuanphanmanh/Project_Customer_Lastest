using esign.DataExporting.Excel.NPOI;
using esign.Dto;
using esign.Esign.Master.MstEsignCategory.Dto.Ver1;
using esign.Esign.Master.MstEsignSystems.Dto.Ver1;
using esign.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace esign.Esign.Master.MstEsignCategory.Exporting.Ver1
{
    public class MstEsignCategoryExcelExporter : NpoiExcelExporterBase, IMstEsignCategoryExcelExporter
    {
        public MstEsignCategoryExcelExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager) { }

        public FileDto ExportToFile(List<MstEsignCategoryOutputDto> category)
        {
            return CreateExcelPackage(
                "Category.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("Category");
                    AddHeader(
                                sheet,
                                ("Code"),
                                ("LocalName"),
                                ("InternationalName"),
                                ("LocalDescription"),
                                ("InternationalDescription"),
                                ("IsMadatory")
                               );
                    AddObjects(
                         sheet, category,
                                _ => _.Code,
                                _ => _.LocalName,
                                _ => _.InternationalName,
                                _ => _.LocalDescription,
                                _ => _.InternationalDescription,
                                _ => _.IsMadatory
                                );
                });

        }

    }
}
