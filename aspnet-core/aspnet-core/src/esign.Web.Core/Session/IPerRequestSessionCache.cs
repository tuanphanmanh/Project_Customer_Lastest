using System.Threading.Tasks;
using esign.Sessions.Dto;
using esign.Sessions.Dto.Ver1;

namespace esign.Web.Session
{
    public interface IPerRequestSessionCache
    {
        Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformationsAsync();
    }
}
