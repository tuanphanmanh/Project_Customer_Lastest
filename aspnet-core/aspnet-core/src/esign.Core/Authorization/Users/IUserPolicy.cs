using System.Threading.Tasks;
using Abp.Domain.Policies;

namespace esign.Authorization.Users
{
    public interface IUserPolicy : IPolicy
    {
        Task CheckMaxUserCountAsync(int tenantId);
    }
}
