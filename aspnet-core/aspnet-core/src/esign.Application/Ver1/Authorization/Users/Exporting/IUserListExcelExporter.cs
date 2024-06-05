using System.Collections.Generic;
using esign.Authorization.Users.Dto.Ver1;
using esign.Dto;

namespace esign.Authorization.Users.Exporting.Ver1
{
    public interface IUserListExcelExporter
    {
        FileDto ExportToFile(List<UserListDto> userListDtos);
    }
}