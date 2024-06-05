using Abp.Application.Services;
using esign.Dto;
using esign.Esign.Master.MstEsignActiveDirectory.Dto.Ver1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace esign.Esign.Master.MstEsignActiveDirectory.Exporting.Ver1
{
    public interface IMstEsignActiveDirectoryExcelExporter : IApplicationService
    {
        FileDto ExportToFile(List<MstEsignActiveDirectoryOutputDto> activeDirectory);
    }
}
