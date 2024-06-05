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

namespace esign.Master.Department.Exporting.Ver1
{
    public class MstEsignDepartmentExcelExporter : NpoiExcelExporterBase, IMstEsignDepartmentExcelExporter
    {
        public MstEsignDepartmentExcelExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager) { }
        public FileDto ExportToFile(List<MstEsignDepartmentOutputDto> divison)
        {
            return CreateExcelPackage(
                "Department.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("Department");
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
