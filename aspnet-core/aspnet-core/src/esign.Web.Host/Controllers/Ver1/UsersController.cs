using Abp.AspNetCore.Mvc.Authorization;
using esign.Authorization;
using esign.Storage;
using Abp.BackgroundJobs;
using Microsoft.AspNetCore.Mvc;

namespace esign.Web.Controllers.Ver1
{
    [ApiVersion("1")]
    [AbpMvcAuthorize(AppPermissions.Pages_Administration_Users)]
    public class UsersController : UsersControllerBase
    {
        public UsersController(IBinaryObjectManager binaryObjectManager, IBackgroundJobManager backgroundJobManager)
            : base(binaryObjectManager, backgroundJobManager)
        {
        }
    }
}