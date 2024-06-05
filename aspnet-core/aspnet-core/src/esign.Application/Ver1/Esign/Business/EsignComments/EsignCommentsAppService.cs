using Abp.Authorization;
using Abp.Dapper.Repositories;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.UI;
using esign.Esign;
using esign.Url;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using esign.Common.Ver1;
using esign.Authorization;
using System;
using esign.Master;
using esign.Ver1.Common;

namespace esign.Business.Ver1
{
    [AbpAuthorize]
    public class EsignCommentsAppService : esignVersion1AppServiceBase, IEsignCommentsAppService
    {

        private readonly IRepository<EsignComments, long> _EsignCommentsRepo;
        private readonly IRepository<EsignCommentsHistory, long> _EsignCommentsHistorysRepo;
        private readonly IDapperRepository<EsignComments, long> _dapperRepo;
        private readonly CommonEsignAppService _common;
        private readonly IWebUrlService _webUrlService;
        private readonly IEsignRequestMultiAffiliateAppService _esignRequestMultiAffiliateAppService;
        private readonly IRepository<EsignMultiAffiliateAction, long> _esignMultiAffiliateActionRepo;
        private readonly IRepository<MstEsignAffiliate, int> _esignAffiliateRepo;
        private readonly ICommonEmailAppService _commonEmailAppService;
        public EsignCommentsAppService(
            IRepository<EsignComments, long> EsignCommentsRepo,
            IDapperRepository<EsignComments, long> dapperRepo,
            IRepository<EsignCommentsHistory, long> EsignCommentsHistorysRepo,
            CommonEsignAppService common,
            IWebUrlService webUrlService,
            IEsignRequestMultiAffiliateAppService esignRequestMultiAffiliateAppService,
            IRepository<EsignMultiAffiliateAction, long> esignMultiAffiliateActionRepo,
            IRepository<MstEsignAffiliate, int> esignAffiliateRepo,
            ICommonEmailAppService commonEmailAppService)
        {
            _EsignCommentsRepo = EsignCommentsRepo;
            _EsignCommentsHistorysRepo = EsignCommentsHistorysRepo;
            _dapperRepo = dapperRepo;
            _webUrlService = webUrlService;
            _common = common;
            _esignRequestMultiAffiliateAppService = esignRequestMultiAffiliateAppService;
            _esignMultiAffiliateActionRepo = esignMultiAffiliateActionRepo;
            _esignAffiliateRepo = esignAffiliateRepo;
            _commonEmailAppService = commonEmailAppService;
        }

        [HttpPost]
        [AbpAuthorize(AppPermissions.Pages_Business_ViewDocument_Comment)]
        public async Task CreateOrEditEsignComments(CreateOrEditEsignCommentsInputDto input)
        {
            if (input.Id == null || input.Id == 0) await Create(input);
            else await Update(input);

            long? UserId = AbpSession.UserId;

            string sql = "Exec [Sp_EsignComments_UpdateIsRead] @p_RequestId, @p_UserId";
            await _dapperRepo.ExecuteAsync(
                sql,
                new
                {
                    p_RequestId = input.RequestId,
                    p_UserId = UserId
                }
            );


            // gửi email  phuongdv
            if (input.Id == null || input.Id == 0)
            {

                string commentGetSigner = "Exec [Sp_EsignComments_GetUserListEmail] @p_RequestId, @p_UserId, @p_IsPublic";
                EsignCommentsLIstUserNotiDto _result = (await _dapperRepo.QueryAsync<EsignCommentsLIstUserNotiDto>(commentGetSigner, new
                {
                    p_RequestId = input.RequestId,
                    p_UserId = UserId,
                    p_IsPublic = input.IsPublic
                })).FirstOrDefault();

                if (_result.ListUserNoti != "")
                {
                    string[] userIds = _result.ListUserNoti.Split(',');
                    for (int i = 0; i < userIds.Length; i++)
                    {
                        await _commonEmailAppService.SendEmailEsignRequest_v21(input.RequestId, AppConsts.EMAIL_CODE_COMMENT, (long)UserId, long.Parse(userIds[i]), "", "", input.Content);
                    }
                }
            }


            sql = "Exec [Sp_EsignComments_GetUserListNoti] @p_RequestId, @p_UserId, @p_IsPublic";
            var result = (await _dapperRepo.QueryAsync<EsignCommentsGetListUserNoti>(
               sql,
               new
               {
                   p_RequestId = input.RequestId,
                   p_UserId = AbpSession.UserId,
                   p_IsPublic = input.IsPublic
               }
           )).FirstOrDefault();

            // Tudq Thêm Add Noti        
            if (result.ListUserNoti != null)
            {
                List<long> listUserNoti = new List<long>();
                for (int i = 0; i < result.ListUserNoti.Split(',').Length; i++)
                {
                    listUserNoti.Add(long.Parse(result.ListUserNoti.Split(',')[i]));
                }
                await _common.SendNoti(input.RequestId, (long)AbpSession.UserId, AppConsts.HISTORY_CODE_COMMENTED, listUserNoti);
            }

            //multi affiliate
            await CurrentUnitOfWork.SaveChangesAsync();
            var listAffiliate = (await _dapperRepo.QueryAsync<string>(
                "Exec Sp_EsignRequest_GetRequestAffiliateForMultiAffiliate @p_RequestId",
                new
                {
                    p_RequestId = input.RequestId
                }
            )).ToList();
            if (listAffiliate != null && listAffiliate.Any())
            {
                var requestdo = await _esignRequestMultiAffiliateAppService.GetRequestForMultiAffiliateCommentInfo(input.RequestId, AbpSession.UserId ?? 0);
                foreach (var affiliateCode in listAffiliate)
                {
                    //transfer data log
                    var esignMultiAffiliateAction = new EsignMultiAffiliateAction()
                    {
                        ToAffiliate = affiliateCode,
                        RequestId = input.RequestId,
                        ActionCode = MultiAffiliateActionCode.Comment.ToString()
                    };
                    //
                    try
                    {
                        var affiliate = await _esignAffiliateRepo.FirstOrDefaultAsync(x => x.Code == affiliateCode);
                        await _esignRequestMultiAffiliateAppService.SendMultiAffiliateEsignRequestCommentInfo(requestdo, affiliate.ApiUrl, affiliate.Code, affiliate.ApiUsername,
                            Convert.ToBase64String(affiliate.ApiEncryptedSecretKey), Convert.ToBase64String(affiliate.ApiEncryptedPassword));
                        esignMultiAffiliateAction.Status = true;
                    }
                    catch (Exception ex)
                    {
                        esignMultiAffiliateAction.Status = false;
                        esignMultiAffiliateAction.Remark = ex.Message;
                        Logger.Error(ex.Message, ex);
                    }
                    await _esignMultiAffiliateActionRepo.InsertAsync(esignMultiAffiliateAction);
                }
            }
        }

        [HttpGet]
        [AbpAuthorize(AppPermissions.Pages_Business_ViewDocument)]
        public async Task<EsignCommentsGetAllCommentsForRequestIdDto> GetAllCommentsForRequestId(long RequestId)
        {
            string sql = "Exec [Sp_EsignComments_GetAllCommentsForRequestId] @p_RequestId, @p_DomainUrl, @p_UserId";
            // lấy data request và esign list
            var listDataSql = (await _dapperRepo.QueryAsync<EsignCommentsListByRequestIdDto>(
                sql,
                new
                {
                    p_RequestId = RequestId,
                    p_DomainUrl = _webUrlService.ServerRootAddressFormat.EnsureEndsWith('/'),
                    p_UserId = AbpSession.UserId
                }
            )).ToList();

            EsignCommentsGetAllCommentsForRequestIdDto listData = new EsignCommentsGetAllCommentsForRequestIdDto();

            listData.TotalUnread = listDataSql.Count == 0 ? 0 : listDataSql[0].TotalUnread;
            listData.Items = listDataSql;

            return listData;
        }

        [HttpGet]
        [AbpAuthorize(AppPermissions.Pages_EsignComments_GetTotalUnreadComment)]
        public async Task<int> GetTotalUnreadComment(long RequestId)
        {
            int TotalRequest = await _EsignCommentsRepo.GetAll().AsNoTracking().CountAsync(e => e.RequestId == RequestId);
            var listTotalRead = await _EsignCommentsHistorysRepo.GetAll().AsNoTracking().Where(x => x.RequestId == RequestId && x.UserId == AbpSession.UserId).ToListAsync();
            int TotalRead = (listTotalRead.Count == 0) ? 0 : ((int)listTotalRead.FirstOrDefault().TotalRead);
            return TotalRequest - TotalRead;
        }

        //CREATE
        private async Task Create(CreateOrEditEsignCommentsInputDto input)
        {
            try
            {
                var newEsignComments = ObjectMapper.Map<EsignComments>(input);
                newEsignComments.UserId = (long)AbpSession.UserId;
                await _EsignCommentsRepo.InsertAsync(newEsignComments);
            }
            catch (UserFriendlyException e)
            {
                throw e;
            }
        }

        // EDIT
        private async Task Update(CreateOrEditEsignCommentsInputDto input)
        {
            try
            {
                var editEsignComments = _EsignCommentsRepo.FirstOrDefault((long)input.Id);
                editEsignComments.UserId = (long)AbpSession.UserId;
                var update = ObjectMapper.Map(input, editEsignComments);
                await _EsignCommentsRepo.UpdateAsync(update);
            }
            catch (UserFriendlyException e)
            {
                throw e;
            }
        }
    }
}
