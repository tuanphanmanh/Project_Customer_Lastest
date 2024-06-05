using System.Collections.Generic;
using Abp;
using esign.Chat.Dto.Ver1;
using esign.Dto;

namespace esign.Chat.Exporting.Ver1
{
    public interface IChatMessageListExcelExporter
    {
        FileDto ExportToFile(UserIdentifier user, List<ChatMessageExportDto> messages);
    }
}
