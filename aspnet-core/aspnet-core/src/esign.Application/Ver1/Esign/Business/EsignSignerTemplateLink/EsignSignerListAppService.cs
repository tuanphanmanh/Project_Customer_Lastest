using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Dapper.Repositories;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.UI;
using esign.Authorization;
using esign.Authorization.Users;
using esign.Business.Dto.Ver1;
using esign.Dto;
using esign.Esign;
using esign.Master;
using esign.Url;
using iTextSharp.text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Z.EntityFramework.Plus;

namespace esign.Business.Ver1
{
    [AbpAuthorize]
    public class EsignSignerTemplateLinkAppService : esignVersion1AppServiceBase, IEsignSignerTemplateLinkVersion1AppService
    {
        private readonly IDapperRepository<EsignSignerTemplateLink, long> _dapperRepo;
        private readonly IWebUrlService _webUrlService;
        private readonly IRepository<MstEsignSignerTemplate> _MstEsignSignerTemplateRepo;
        private readonly IRepository<EsignSignerTemplateLink, long> _EsignSignerTemplateLinkRepo;
        private readonly IRepository<User,long> _UserRepo;
        public EsignSignerTemplateLinkAppService(IDapperRepository<EsignSignerTemplateLink, long> dapperRepo,
                                                 IWebUrlService webUrlService,
                                                 IRepository<MstEsignSignerTemplate> MstEsignSignerTemplateRepo,
                                                 IRepository<EsignSignerTemplateLink, long> EsignSignerTemplateLinkRepo,
                                                 IRepository<User, long> UserRepo
                                                 )
        {
            _dapperRepo = dapperRepo;
            _webUrlService = webUrlService;
            _MstEsignSignerTemplateRepo = MstEsignSignerTemplateRepo;
            _EsignSignerTemplateLinkRepo = EsignSignerTemplateLinkRepo;
            _UserRepo = UserRepo;
        }
        [HttpPost]
        [AbpAuthorize(AppPermissions.Pages_Business_EsignSignerTemplateLink_CreateNewTemplateForRequester)]
        public async Task<SavedResultSaveTemplateDto> CreateNewTemplateForRequester(EsignSignerTemplateLinkCreateNewRequestv1Dto input)
        //(EsignSignerTemplateLinkCreateNewRequestDto input)
        {
            // Check xem tồn tại tên mẫu chữ ký chưa            
            //if (_MstEsignSignerTemplateRepo.GetAll().Any(e => e.LocalName == input.Name && e.CreatorUserId == AbpSession.UserId))
            //{
            //    throw new UserFriendlyException("Name Of Template Already Exists!");
            //}

            //MstEsignSignerTemplate newMstEsignSignerTemplate = new MstEsignSignerTemplate();
            //newMstEsignSignerTemplate.LocalName = input.Name;
            //newMstEsignSignerTemplate.LocalDescription = input.Description;
            //newMstEsignSignerTemplate.AddCC = input.AddCC;
            //// insert mẫu chữ ký
            //long newId = await _MstEsignSignerTemplateRepo.InsertAndGetIdAsync(newMstEsignSignerTemplate);
            //// insert người ký vào template link
            //for (int i = 0; i < input.listEsignSignerTemplateLink.Count; i++)
            //{
            //    EsignSignerTemplateLink newEsignSignerTemplateLink = new EsignSignerTemplateLink();
            //    newEsignSignerTemplateLink.UserId = input.listEsignSignerTemplateLink[i].Id;
            //    newEsignSignerTemplateLink.SigningOrder = input.listEsignSignerTemplateLink[i].SigningOrder;
            //    newEsignSignerTemplateLink.ColorId = input.listEsignSignerTemplateLink[i].ColorId;
            //    newEsignSignerTemplateLink.TemplateId = newId;
            //    await _EsignSignerTemplateLinkRepo.InsertAsync(newEsignSignerTemplateLink);
            //}
            long p_templateId;
            List<long> ids = new List<long>();
            

            // Validate list Signers
            for (int i = 0; i < input.listSigners.Count; i++)
            {
                if (input.listSigners[i].id.Split(',')[0]=="")
                {
                    throw new UserFriendlyException("List Signer Id can not be blank! Signing Order: " + input.listSigners[i].SigningOrder);
                }    
                for (int j = 0; j < input.listSigners[i].id.Split(',').Length; j++)
                {
                    ids.Add(long.Parse(input.listSigners[i].id.Split(',')[j]));
                }
            }

            if (input.listCC.Split(',')[0] != "")
            {
                for (int i = 0; i < input.listCC.Split(',').Length; i++)
                {
                    ids.Add(long.Parse(input.listCC.Split(',')[i]));
                }
            }

            for (int i =0;i<ids.Count;i++)
            {
                if (!_UserRepo.GetAll().Any(e => e.Id == ids[i]))
                    throw new UserFriendlyException("Id Not Exists:" + ids[i].ToString());
            }    
            var dupIds = ids.GroupBy(i => i).Where(i => i.Count() > 1).Select(i => i.Key).ToList();
            if (dupIds.Count > 0)
            {
                string strDupIds = "";
                foreach (var k in dupIds)
                {
                    var userName = _UserRepo.GetAll().Where(e => e.Id.ToString() == k.ToString()).FirstOrDefault().UserName;
                    strDupIds = strDupIds + userName + ";";
                }
                throw new UserFriendlyException("User Duplicate: " + strDupIds.Substring(0, strDupIds.Length - 1));
            }
            if (input.TemplateId == null || input.TemplateId == 0)
            {
                if (_MstEsignSignerTemplateRepo.GetAll().Any(e => e.LocalName == input.Name && e.CreatorUserId == AbpSession.UserId))
                {
                    throw new UserFriendlyException("Name Of Template Already Exists!");
                }
                MstEsignSignerTemplate newMstEsignSignerTemplate = new MstEsignSignerTemplate();
                newMstEsignSignerTemplate.LocalName = input.Name;
                // gen Add CC
                string p_emails = "";
                if (input.listCC.Split(',')[0] != "")
                {
                    for (int i = 0; i < input.listCC.Split(',').Length; i++)
                    {
                        string idTemp = input.listCC.Split(',')[i];
                        p_emails = p_emails + _UserRepo.GetAll().Where(e => e.Id == long.Parse(idTemp)).FirstOrDefault().EmailAddress + ";";
                    }
       
                }

                if (p_emails != "")
                    newMstEsignSignerTemplate.AddCC = p_emails.Substring(0, p_emails.Length - 1);
                long newId = await _MstEsignSignerTemplateRepo.InsertAndGetIdAsync(newMstEsignSignerTemplate);
                // gen List Signers
                for (int i = 0; i < input.listSigners.Count; i++)
                {
                    for (int j = 0; j < input.listSigners[i].id.Split(',').Length; j++)
                    {
                        EsignSignerTemplateLink newEsignSignerTemplateLink = new EsignSignerTemplateLink();
                        newEsignSignerTemplateLink.UserId = long.Parse(input.listSigners[i].id.Split(',')[j]);
                        newEsignSignerTemplateLink.SigningOrder = (int)input.listSigners[i].SigningOrder;
                        newEsignSignerTemplateLink.TemplateId = newId;
                        await _EsignSignerTemplateLinkRepo.InsertAsync(newEsignSignerTemplateLink);
                    }
                }
                p_templateId = newId;
            }
            else
            {
                if (_MstEsignSignerTemplateRepo.GetAll().Any(e => e.LocalName == input.Name && e.Id != input.TemplateId && e.CreatorUserId == AbpSession.UserId))
                {
                    throw new UserFriendlyException("Name Of Template Already Exists!");
                }
                var editTemplate = _MstEsignSignerTemplateRepo.GetAll().Where(e => e.Id == input.TemplateId).FirstOrDefault();
                editTemplate.LocalName = input.Name;
                string p_emails = "";

                if (input.listCC.Split(',')[0] != "")
                {
                    for (int i = 0; i < input.listCC.Split(',').Length; i++)
                    {
                        string idTemp = input.listCC.Split(',')[i];
                        p_emails = p_emails + _UserRepo.GetAll().Where(e => e.Id == long.Parse(idTemp)).FirstOrDefault().EmailAddress + ";";
                    }
                 
                }
                if (p_emails != "")
                    editTemplate.AddCC = p_emails.Substring(0, p_emails.Length - 1);
                else editTemplate.AddCC = p_emails;
                await _EsignSignerTemplateLinkRepo.GetAll().Where(e=>e.TemplateId == input.TemplateId).DeleteAsync();
                // gen List Signers
                for (int i = 0; i < input.listSigners.Count; i++)
                {
                    for (int j = 0; j < input.listSigners[i].id.Split(',').Length; j++)
                    {
                        EsignSignerTemplateLink newEsignSignerTemplateLink = new EsignSignerTemplateLink();
                        newEsignSignerTemplateLink.UserId = long.Parse(input.listSigners[i].id.Split(',')[j]);
                        newEsignSignerTemplateLink.SigningOrder = (int)input.listSigners[i].SigningOrder;
                        newEsignSignerTemplateLink.TemplateId = (long)input.TemplateId;
                        await _EsignSignerTemplateLinkRepo.InsertAsync(newEsignSignerTemplateLink);
                    }
                }
                p_templateId = (long)input.TemplateId;
            }
  
            
            return new SavedResultSaveTemplateDto { IsSave = true, TemplateId = p_templateId };
        }

        [HttpGet]
        [AbpAuthorize(AppPermissions.Pages_Business_EsignSignerTemplateLink_GetListSignerByTemplateId)]
        public async Task<EsignSignerTemplateLinkV1Dto> GetListSignerByTemplateId(long templateId)
        {
            EsignSignerTemplateLinkV1Dto result = new EsignSignerTemplateLinkV1Dto();

            List<EsignSignerTemplateLinkDto> resultSigner = (await _dapperRepo.QueryAsync<EsignSignerTemplateLinkDto>(
                "exec Sp_EsignSignerTemplateLink_GetListSignerByTemplateId @p_TemplateId, @p_DomainUrl",
                new
                {
                    @p_TemplateId = templateId,
                    p_DomainUrl = _webUrlService.ServerRootAddressFormat.EnsureEndsWith('/')
                }
            )).ToList();


            List<EsignSignerTemplateLinkListCCDto> resultCC = (await _dapperRepo.QueryAsync<EsignSignerTemplateLinkListCCDto>(
                "exec [Sp_EsignSignerTemplateLink_GetListCCByTemplateId] @p_TemplateId, @p_DomainUrl",
                new
                {
                    @p_TemplateId = templateId,
                    p_DomainUrl = _webUrlService.ServerRootAddressFormat.EnsureEndsWith('/')
                }
            )).ToList();

            result.listSigners = resultSigner;
            result.listCC = resultCC;
            return result;
        }


        [HttpPost]
        [AbpAuthorize(AppPermissions.Pages_Business_EsignSignerTemplateLink_CreateNewTemplateForWebRequesterForWeb)]
        public async Task CreateNewTemplateForWebRequesterForWeb(EsignSignerTemplateLinkCreateNewRequestForWebDto input) 
        {
            // Check xem tồn tại tên mẫu chữ ký chưa            
            //if (_MstEsignSignerTemplateRepo.GetAll().Any(e => e.LocalName == input.Name && e.CreatorUserId == AbpSession.UserId))
            //{
            //    throw new UserFriendlyException("Name Of Template Already Exists!");
            //}

            //MstEsignSignerTemplate newMstEsignSignerTemplate = new MstEsignSignerTemplate();
            //newMstEsignSignerTemplate.LocalName = input.Name;
            //newMstEsignSignerTemplate.LocalDescription = input.Description;
            //newMstEsignSignerTemplate.AddCC = input.AddCC;
            //// insert mẫu chữ ký
            //long newId = await _MstEsignSignerTemplateRepo.InsertAndGetIdAsync(newMstEsignSignerTemplate);
            //// insert người ký vào template link
            //for (int i = 0; i < input.listEsignSignerTemplateLink.Count; i++)
            //{
            //    EsignSignerTemplateLink newEsignSignerTemplateLink = new EsignSignerTemplateLink();
            //    newEsignSignerTemplateLink.UserId = input.listEsignSignerTemplateLink[i].Id;
            //    newEsignSignerTemplateLink.SigningOrder = input.listEsignSignerTemplateLink[i].SigningOrder;
            //    newEsignSignerTemplateLink.ColorId = input.listEsignSignerTemplateLink[i].ColorId;
            //    newEsignSignerTemplateLink.TemplateId = newId;
            //    await _EsignSignerTemplateLinkRepo.InsertAsync(newEsignSignerTemplateLink);
            //}

            if (_MstEsignSignerTemplateRepo.GetAll().Any(e => e.LocalName == input.Name && e.CreatorUserId == AbpSession.UserId))
            {
                throw new UserFriendlyException("Name Of Template Already Exists!");
            }
            List<long> ids = new List<long>();
            // Validate list Signers
            for (int i = 0; i < input.listSigners.Count; i++)
            {
                for (int j = 0; j < input.listSigners[i].id.Count; j++)
                {
                    ids.Add(input.listSigners[i].id[j]);
                }
            }
            for (int i = 0; i < input.listCC.Count; i++)
            {
                ids.Add(input.listCC[i]);
            }
            for (int i = 0; i < ids.Count; i++)
            {
                if (!_UserRepo.GetAll().Any(e => e.Id == ids[i]))
                    throw new UserFriendlyException("Id Not Exists:" + ids[i].ToString());
            }
            var dupIds = ids.GroupBy(i => i).Where(i => i.Count() > 1).Select(i => i.Key).ToList();
            if (dupIds.Count > 0)
            {
                string strDupIds = "";
                foreach (var k in dupIds)
                {
                    var userName = _UserRepo.GetAll().Where(e => e.Id.ToString() == k.ToString()).FirstOrDefault().UserName;
                    strDupIds = strDupIds + userName + ";";
                }
                throw new UserFriendlyException("User Duplicate: " + strDupIds.Substring(0, strDupIds.Length - 1));
            }
            MstEsignSignerTemplate newMstEsignSignerTemplate = new MstEsignSignerTemplate();
            newMstEsignSignerTemplate.LocalName = input.Name;
            // gen Add CC
            string p_emails = "";
            for (int i = 0; i < input.listCC.Count; i++)
            {
                p_emails = p_emails + _UserRepo.GetAll().Where(e => e.Id == input.listCC[i]).FirstOrDefault().EmailAddress + ";";
            }
            if (p_emails != "")
                newMstEsignSignerTemplate.AddCC = p_emails.Substring(0, p_emails.Length - 1);
            long newId = await _MstEsignSignerTemplateRepo.InsertAndGetIdAsync(newMstEsignSignerTemplate);
            // gen List Signers
            for (int i = 0; i < input.listSigners.Count; i++)
            {
                for (int j = 0; j < input.listSigners[i].id.Count; j++)
                {
                    EsignSignerTemplateLink newEsignSignerTemplateLink = new EsignSignerTemplateLink();
                    newEsignSignerTemplateLink.UserId = input.listSigners[i].id[j];
                    newEsignSignerTemplateLink.SigningOrder = (int)input.listSigners[i].SigningOrder;
                    newEsignSignerTemplateLink.TemplateId = newId;
                    newEsignSignerTemplateLink.ColorId = input.listSigners[i].ColorId[j];

                    await _EsignSignerTemplateLinkRepo.InsertAsync(newEsignSignerTemplateLink);
                }
            }
        }
    }
}
