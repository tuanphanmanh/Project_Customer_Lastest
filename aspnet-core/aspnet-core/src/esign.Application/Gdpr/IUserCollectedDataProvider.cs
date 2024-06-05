using System.Collections.Generic;
using System.Threading.Tasks;
using Abp;
using esign.Dto;

namespace esign.Gdpr
{
    public interface IUserCollectedDataProvider
    {
        Task<List<FileDto>> GetFiles(UserIdentifier user);
    }
}
