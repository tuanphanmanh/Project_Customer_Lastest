using System.Collections.Generic;
using System.Threading.Tasks;
using Abp;
using Abp.Authorization.Users;
using Abp.Domain.Repositories;

namespace esign.Organizations
{
    public interface IUserOrganizationUnitRepository : IRepository<UserOrganizationUnit, long>
    {
        Task<List<UserIdentifier>> GetAllUsersInOrganizationUnitHierarchical(long[] organizationUnitIds);
    }
}