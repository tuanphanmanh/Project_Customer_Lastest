using System.Collections.Generic;
using esign.Authorization.Users.Importing.Dto.Ver1;
using Abp.Dependency;

namespace esign.Authorization.Users.Importing.Ver1
{
    public interface IUserListExcelDataReader: ITransientDependency
    {
        List<ImportUserDto> GetUsersFromExcel(byte[] fileBytes);
    }
}
