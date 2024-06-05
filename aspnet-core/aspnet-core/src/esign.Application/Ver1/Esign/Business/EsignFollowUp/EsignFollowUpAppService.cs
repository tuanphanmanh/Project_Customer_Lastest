using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.UI;
using esign.Authorization;
using esign.Esign;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace esign.Business.Ver1
{
    [AbpAuthorize]
    [AbpAuthorize(AppPermissions.Pages_EsignFollowUp)]

    public class EsignFollowUpAppService : esignVersion1AppServiceBase, IEsignFollowUpAppService
    {

        private readonly IRepository<EsignFollowUpHistory, long> _EsignFollowUpHistoryRepo;
        private readonly IRepository<EsignFollowUp, long> _EsignFollowUpRepo;
        public EsignFollowUpAppService(IRepository<EsignFollowUpHistory, long> EsignFollowUpHistoryRepo,
                                       IRepository<EsignFollowUp, long> EsignFollowUpRepo)
        {
            _EsignFollowUpHistoryRepo = EsignFollowUpHistoryRepo;
            _EsignFollowUpRepo = EsignFollowUpRepo;
        }

        [HttpPost]
        [AbpAuthorize(AppPermissions.Pages_EsignFollowUp_FollowUpRequest)]

        public async Task FollowUpRequest(CreateOrEditEsignFollowUpInputDto input)
        {
            var checkFollowUp = _EsignFollowUpRepo.FirstOrDefault(x => x.RequestId == input.RequestId && x.CreatorUserId == AbpSession.UserId);
            if (checkFollowUp == null)
                await Create(input);
            else
            {
                input.Id = checkFollowUp.Id;
                await Update(input);
            }
        }

        //CREATE
        private async Task Create(CreateOrEditEsignFollowUpInputDto input)
        {
            try
            {
                var newEsignFollowUp = ObjectMapper.Map<EsignFollowUp>(input);                
                long Id = await _EsignFollowUpRepo.InsertAndGetIdAsync(newEsignFollowUp);
                // insert into his
                EsignFollowUpHistory obj = new EsignFollowUpHistory();
                obj.FollowUpId = Id;
                obj.IsFollowUp = input.IsFollowUp;
                await _EsignFollowUpHistoryRepo.InsertAsync(obj);
            }
            catch (UserFriendlyException e)
            {
                throw e;
            }
        }

        // EDIT
        private async Task Update(CreateOrEditEsignFollowUpInputDto input)
        {
            try
            {
                var editEsignComments = _EsignFollowUpRepo.FirstOrDefault((long)input.Id);
                var update = ObjectMapper.Map(input, editEsignComments);
                await _EsignFollowUpRepo.UpdateAsync(update);
                EsignFollowUpHistory obj = new EsignFollowUpHistory();
                obj.FollowUpId = (long)input.Id;
                obj.IsFollowUp = input.IsFollowUp;
                await _EsignFollowUpHistoryRepo.InsertAsync(obj);
            }
            catch (UserFriendlyException e)
            {
                throw e;
            }
        }
    }
}
