using System.Threading.Tasks;
using Abp.Dependency;
using Microsoft.AspNetCore.Http;
using esign.Sessions;
using esign.Sessions.Dto;
using esign.Sessions.Ver1;
using esign.Sessions.Dto.Ver1;

namespace esign.Web.Session
{
    public class PerRequestSessionCache : IPerRequestSessionCache, ITransientDependency
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ISessionAppService _sessionAppService;

        public PerRequestSessionCache(
            IHttpContextAccessor httpContextAccessor,
            ISessionAppService sessionAppService)
        {
            _httpContextAccessor = httpContextAccessor;
            _sessionAppService = sessionAppService;
        }

        public async Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformationsAsync()
        {
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext == null)
            {
                return await _sessionAppService.GetCurrentLoginInformations();
            }

            var cachedValue = httpContext.Items["__PerRequestSessionCache"] as GetCurrentLoginInformationsOutput;
            if (cachedValue == null)
            {
                cachedValue = await _sessionAppService.GetCurrentLoginInformations();
                httpContext.Items["__PerRequestSessionCache"] = cachedValue;
            }

            return cachedValue;
        }
    }
}