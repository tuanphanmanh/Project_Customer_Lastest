using System;
using System.Net;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using esign.Chat;
using esign.Storage;
using Abp.Domain.Repositories;
using esign.Url;
using esign.Ver1.Common;
using esign.Master;
using esign.Esign;

namespace esign.Web.Controllers.Ver1
{
    [ApiVersion("1")]
    [AbpMvcAuthorize]
    public class ChatController : ChatControllerBase
    {
        //private readonly IRepository<MstEsignStatus, int> _mstEsignStatusRepo;
        //private readonly IWebUrlService _webUrlService;
        //private readonly ICommonEmailAppService _commonEmailAppService;
        //private readonly IRepository<EsignSignerList, long> _esignSignerListRepo;
        //private readonly IRepository<MstEsignStatus, int> _esignStatusRepo;

        //public ChatController(IRepository<MstEsignStatus, int> mstEsignStatusRepo, IRepository<MstEsignStatus, int> esignStatusRepo, IRepository<EsignSignerList, long> esignSignerListRepo, IWebUrlService webUrlService, ICommonEmailAppService commonEmailAppService)
        //{
        //    _mstEsignStatusRepo = mstEsignStatusRepo;
        //    _webUrlService = webUrlService;
        //    _commonEmailAppService = commonEmailAppService;
        //    _esignSignerListRepo = esignSignerListRepo;
        //    _esignStatusRepo = esignStatusRepo;
        //}
        public ChatController(IBinaryObjectManager binaryObjectManager, IChatMessageManager chatMessageManager) : 
            base(binaryObjectManager, chatMessageManager)
        {
        }
        [HttpGet]
        public async Task<ActionResult> GetUploadedObject(Guid fileId, string fileName, string contentType)
        {
            using (CurrentUnitOfWork.SetTenantId(null))
            {
                var fileObject = await BinaryObjectManager.GetOrNullAsync(fileId);



                if (fileObject == null)
                {
                    return StatusCode((int)HttpStatusCode.NotFound);
                }

                return File(fileObject.Bytes, contentType, fileName);
            }
        }
    }
}

