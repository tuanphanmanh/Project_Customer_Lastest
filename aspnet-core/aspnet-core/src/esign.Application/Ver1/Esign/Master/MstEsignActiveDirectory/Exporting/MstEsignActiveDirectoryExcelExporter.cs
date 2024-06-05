using esign.DataExporting.Excel.NPOI;
using esign.Dto;
using esign.Esign.Master.MstEsignActiveDirectory.Dto.Ver1;
using esign.Esign.Master.MstEsignSystems.Dto.Ver1;
using esign.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace esign.Esign.Master.MstEsignActiveDirectory.Exporting.Ver1
{
    public class MstEsignActiveDirectoryExcelExporter : NpoiExcelExporterBase, IMstEsignActiveDirectoryExcelExporter
    {
        public MstEsignActiveDirectoryExcelExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager) { }

        public FileDto ExportToFile(List<MstEsignActiveDirectoryOutputDto> activeDirectory)
        {
            return CreateExcelPackage(
                "ActiveDirectory.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("ActiveDirectory");
                    AddHeader(
                                sheet,
                                ("Email"),
                                ("Title"),
                                ("Department"),
                                ("FullName")
                               );
                    AddObjects(
                         sheet, activeDirectory,
                                _ => _.Email,
                                _ => _.Title,
                                _ => _.Department,
                                _ => _.FullName
                                );
                });

        }
    }
}
