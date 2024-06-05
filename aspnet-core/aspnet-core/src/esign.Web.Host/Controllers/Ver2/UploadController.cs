using Abp.AspNetCore.Mvc.Authorization;
using Abp.BackgroundJobs;
using Abp.Dapper.Repositories;
using Abp.Domain.Repositories;
using esign.Authorization.Users;
using esign.Business.Ver1;
using esign.Common.Ver1;
using esign.Esign;
using esign.Master;
using esign.Storage;
using esign.Url;
using esign.Ver1.Common;
using Microsoft.AspNetCore.Mvc;

namespace esign.Web.Controllers.Ver2
{
    [ApiVersion("2")]
    public class UploadController : UploadControllerBase
    {
        public UploadController(
            IRepository<MstEsignUserImage, int> mstEsignUserImageRepo,
            IRepository<EsignDocumentList, long> esignDocumentListRepo,
            IRepository<EsignRequest, long> esignRequestRepo,
            IRepository<MstEsignStatus, int> mstEsignStatusRepo,
            IRepository<EsignActivityHistory, long> esignActivityHistoryRepo,
            IRepository<EsignPosition, long> esignPositionRepo,
            IWebUrlService webUrlService,
            IDapperRepository<EsignComments, long> dapperRepo,
            CommonEsignAppService common,
            IRepository<EsignSignerList, long> signerRepo,
            IEsignRequestMultiAffiliateAppService esignRequestMultiAffiliateAppService,
            IRepository<EsignMultiAffiliateAction, long> esignMultiAffiliateActionRepo,
            IRepository<MstEsignAffiliate, int> esignAffiliateRepo,
            IRepository<User, long> userRepo,
            ICommonEmailAppService _commonEmail
        ) : base (
            mstEsignUserImageRepo,
            esignDocumentListRepo,
            esignRequestRepo,
            mstEsignStatusRepo,
            esignActivityHistoryRepo,
            esignPositionRepo,
            webUrlService,
            dapperRepo,
            common,
            signerRepo,
            esignRequestMultiAffiliateAppService,
            esignMultiAffiliateActionRepo,
            esignAffiliateRepo,
            userRepo,
            _commonEmail)
        {
        }
    }
}
