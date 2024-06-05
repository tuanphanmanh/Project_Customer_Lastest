using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Runtime.Caching;
using esign.Authorization;
using esign.Caching.Dto.Ver1;
using Microsoft.AspNetCore.Mvc;

namespace esign.Caching.Ver1
{
    [AbpAuthorize]
    [AbpAuthorize(AppPermissions.Pages_Cache)]
    public class CachingAppService : esignVersion1AppServiceBase, ICachingAppService
    {
        private readonly ICacheManager _cacheManager;

        public CachingAppService(ICacheManager cacheManager)
        {
            _cacheManager = cacheManager;
        }

        [AbpAuthorize(AppPermissions.Pages_Cache_GetAllCaches)]
        [HttpGet]
        public ListResultDto<CacheDto> GetAllCaches()
        {
            var caches = _cacheManager.GetAllCaches()
                                        .Select(cache => new CacheDto
                                        {
                                            Name = cache.Name
                                        })
                                        .ToList();

            return new ListResultDto<CacheDto>(caches);
        }

        [AbpAuthorize(AppPermissions.Pages_Cache_ClearCache)]
        [HttpPost]
        public async Task ClearCache(EntityDto<string> input)
        {
            var cache = _cacheManager.GetCache(input.Id);
            await cache.ClearAsync();
        }

        [AbpAuthorize(AppPermissions.Pages_Cache_ClearAllCaches)]
        [HttpPost]
        public async Task ClearAllCaches()
        {
            var caches = _cacheManager.GetAllCaches();
            foreach (var cache in caches)
            {
                await cache.ClearAsync();
            }
        }
    }
}