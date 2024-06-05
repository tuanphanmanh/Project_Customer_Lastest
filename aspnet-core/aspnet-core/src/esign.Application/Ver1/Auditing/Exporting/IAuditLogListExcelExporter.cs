using System.Collections.Generic;
using esign.Auditing.Dto.Ver1;
using esign.Dto;

namespace esign.Auditing.Exporting.Ver1
{
    public interface IAuditLogListExcelExporter
    {
        FileDto ExportToFile(List<AuditLogListDto> auditLogListDtos);

        FileDto ExportToFile(List<EntityChangeListDto> entityChangeListDtos);
    }
}
