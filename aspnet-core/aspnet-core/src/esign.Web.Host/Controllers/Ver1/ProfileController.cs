using Abp.AspNetCore.Mvc.Authorization;
using esign.Authorization.Users.Profile;
using esign.Authorization.Users.Profile.Ver1;
using esign.Graphics;
using esign.Storage;
using Microsoft.AspNetCore.Mvc;

namespace esign.Web.Controllers.Ver1
{
    [ApiVersion("1")]
    [AbpMvcAuthorize]
    public class ProfileController : ProfileControllerBase
    {
        public ProfileController(
            ITempFileCacheManager tempFileCacheManager,
            IProfileAppService profileAppService,
            IImageFormatValidator imageFormatValidator) :
            base(tempFileCacheManager, profileAppService, imageFormatValidator)
        {
        }
    }
}