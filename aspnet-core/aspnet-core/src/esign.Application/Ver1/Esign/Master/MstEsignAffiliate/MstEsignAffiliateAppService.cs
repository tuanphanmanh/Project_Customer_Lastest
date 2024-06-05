using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.UI;
using esign.Authorization;
using esign.Business.Ver1;
using esign.Dto;
using esign.Esign;
using esign.Esign.Master.MstEsignAffiliate.Dto.Ver1;
using esign.Master.Dto.Ver1;
using esign.Security;
using esign.Url;
using esign.Ver1.Esign.Business.EsignReferenceRequest;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NPOI.POIFS.Crypt;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace esign.Master.Ver1
{
    [AbpAuthorize]
    public class MstEsignAffiliateAppService : esignVersion1AppServiceBase, IMstEsignAffiliateAppService
    {
        private readonly IRepository<MstEsignAffiliate> _MstEsignAffiliateRepo;
        private readonly IEsignRequestMultiAffiliateAppService _esignRequestMultiAffiliateAppService;
        private readonly IRepository<MstEsignAffiliate, int> _esignAffiliateRepo;
        public MstEsignAffiliateAppService(
            IRepository<MstEsignAffiliate> MstEsignAffiliateRepo,
            IEsignRequestMultiAffiliateAppService esignRequestMultiAffiliateAppService,
            IRepository<MstEsignAffiliate, int> esignAffiliateRepo
        )
        {
            _MstEsignAffiliateRepo = MstEsignAffiliateRepo;
            _esignRequestMultiAffiliateAppService = esignRequestMultiAffiliateAppService;
            _esignAffiliateRepo = esignAffiliateRepo;
        }

        [HttpGet]
        [AbpAuthorize(AppPermissions.Pages_Master_Affiliate_View)]
        public async Task<List<MstEsignAffiliateDto>> GetAllSystems()
        {
            var listMstEsignAffiliate = await _MstEsignAffiliateRepo.GetAllListAsync();
            return ObjectMapper.Map<List<MstEsignAffiliateDto>>(listMstEsignAffiliate);
        }

        [HttpPost]
        [AbpAuthorize(AppPermissions.Pages_Master_Affiliate_CreateOrEdit)]
        public async Task CreateOrEdit(CreateOrEditMstEsignAffiliateDto input)
        {
            try
            {
                var duplicateSystem = _MstEsignAffiliateRepo.FirstOrDefault(e => e.Code == input.Code && (input.Id == null || input.Id == 0 || e.Id != input.Id));
                if (duplicateSystem != null)
                {
                    throw new UserFriendlyException(L("AffiliateExisted"));
                }
                //
                MstEsignAffiliate obj = null;
                if (input.Id == null || input.Id == 0)
                {
                    obj = new MstEsignAffiliate();
                }
                else
                {
                    obj = _MstEsignAffiliateRepo.FirstOrDefault((int)input.Id);
                }
                ObjectMapper.Map(input, obj);
                if (!string.IsNullOrEmpty(input.ApiPassword))
                {
                    var encryption = Cryptography.EncryptStringToBytes(input.ApiPassword);
                    obj.ApiEncryptedSecretKey = encryption.key;
                    obj.ApiEncryptedPassword = encryption.encrypted;
                }
                await _MstEsignAffiliateRepo.InsertOrUpdateAsync(obj);
            }
            catch (UserFriendlyException ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        [AbpAuthorize(AppPermissions.Pages_Master_Affiliate_Delete)]
        public async Task Delete([FromQuery] EntityDto input)
        {
            await _MstEsignAffiliateRepo.DeleteAsync(input.Id);
        }

        [HttpPost]
        [AbpAuthorize(AppPermissions.Pages_Master_Affiliate_ReceiveMultiAffiliateUsersInfo)]
        public async Task ReceiveMultiAffiliateUsersInfo(string affiliateCode)
        {
            var affiliate = await _esignAffiliateRepo.FirstOrDefaultAsync(x => x.Code == affiliateCode);
            if (affiliate != null)
            {
                await _esignRequestMultiAffiliateAppService.ReceiveMultiAffiliateUsersInfo(affiliate.Id, affiliate.ApiUrl, affiliate.Code, affiliate.ApiUsername,
                   Convert.ToBase64String(affiliate.ApiEncryptedSecretKey), Convert.ToBase64String(affiliate.ApiEncryptedPassword));
            }
            else
            {
                throw new UserFriendlyException("Affiliate not found");
            }
        }
    }
}
