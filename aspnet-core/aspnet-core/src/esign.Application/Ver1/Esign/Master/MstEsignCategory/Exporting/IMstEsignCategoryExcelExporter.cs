using esign.Dto;
using esign.Esign.Master.MstEsignCategory.Dto.Ver1;
using esign.Esign.Master.MstEsignSystems.Dto.Ver1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace esign.Esign.Master.MstEsignCategory.Exporting.Ver1
{
    public interface IMstEsignCategoryExcelExporter
    {
        FileDto ExportToFile(List<MstEsignCategoryOutputDto> category);
    }
}
