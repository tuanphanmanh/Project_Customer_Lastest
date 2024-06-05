using System.Collections.Generic;
using esign.Authorization.Users.Importing.Dto.Ver1;
using esign.Dto;

namespace esign.Authorization.Users.Importing.Ver1
{
    public interface IInvalidUserExporter
    {
        FileDto ExportToFile(List<ImportUserDto> userListDtos);
    }
}
