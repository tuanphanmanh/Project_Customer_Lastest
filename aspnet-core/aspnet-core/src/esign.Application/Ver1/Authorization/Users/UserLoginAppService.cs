using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Auditing;
using Abp.Authorization;
using Abp.Authorization.Users;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.Runtime.Session;
using Microsoft.EntityFrameworkCore;
using esign.Authorization.Users.Dto.Ver1;
using Microsoft.AspNetCore.Mvc;

namespace esign.Authorization.Users.Ver1
{
    [AbpAuthorize]
    public class UserLoginAppService : esignVersion1AppServiceBase, IUserLoginAppService
    {
        private readonly IRepository<UserLoginAttempt, long> _userLoginAttemptRepository;

        public UserLoginAppService(IRepository<UserLoginAttempt, long> userLoginAttemptRepository)
        {
            _userLoginAttemptRepository = userLoginAttemptRepository;
        }

        [DisableAuditing]
        [HttpGet]
        public async Task<PagedResultDto<UserLoginAttemptDto>> GetUserLoginAttempts([FromQuery] GetLoginAttemptsInput input)
        {
            var userId = AbpSession.GetUserId();
            var query = _userLoginAttemptRepository.GetAll()
                .Where(la => la.UserId == userId)
                .WhereIf(!input.Filter.IsNullOrEmpty(), la => la.ClientIpAddress.Contains(input.Filter) ||  la.BrowserInfo.Contains(input.Filter))
                .WhereIf(input.StartDate.HasValue, la => la.CreationTime >= input.StartDate)
                .WhereIf(input.EndDate.HasValue, la => la.CreationTime <= input.EndDate)
                .WhereIf(input.Result.HasValue, la => la.Result == input.Result);

            var loginAttemptCount = await query.CountAsync();

            var loginAttempts = await query
                .OrderBy(input.Sorting)
                .PageBy(input)
                .ToListAsync();

            var loginAttemptDtos = ObjectMapper.Map<List<UserLoginAttemptDto>>(loginAttempts);
            
            return new PagedResultDto<UserLoginAttemptDto>(
                loginAttemptCount,
                loginAttemptDtos
            );
        }
    }
}
