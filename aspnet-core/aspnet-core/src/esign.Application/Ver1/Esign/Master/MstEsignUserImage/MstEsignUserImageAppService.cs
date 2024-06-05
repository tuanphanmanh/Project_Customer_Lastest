using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.UI;
using esign.Authorization;
using esign.Dto;
using esign.Master.Dto.Ver1;
using esign.Url;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore; 
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace esign.Master.Ver1
{
    [AbpAuthorize]
    public class MstEsignUserImageAppService : esignVersion1AppServiceBase, IMstEsignUserImageAppService
    {
        private readonly IRepository<MstEsignUserImage> _mstEsignUserImageRepo;
        private readonly IWebUrlService _webUrlService;
        public MstEsignUserImageAppService(IRepository<MstEsignUserImage> mstEsignUserImageRepo, IWebUrlService webUrlService)
        {
            _mstEsignUserImageRepo = mstEsignUserImageRepo;
            _webUrlService = webUrlService;
        }

        #region Api for Mobile
        // GetListSignatureByUserId  
        [HttpGet]
        [AbpAuthorize(AppPermissions.Pages_Master_EsignUserImageApi_GetListSignatureByUserId)]

        public async Task<ListResultDto<MstEsignUserImageDto>> GetListSignatureByUserId()
        {
            var listMstEsignUserImage = _mstEsignUserImageRepo.GetAll().Where(e => e.CreatorUserId == AbpSession.UserId)
                                                                                                                            .OrderByDescending(e => e.Id); ;
            var result = await (from o in listMstEsignUserImage
                                select new MstEsignUserImageDto
                         {
                             Id = o.Id,
                             ImgUrl = (o.ImgUrl=="") ? "": (_webUrlService.ServerRootAddressFormat.EnsureEndsWith('/') + o.ImgUrl),
                         }).ToListAsync();

            return new ListResultDto<MstEsignUserImageDto> { Items = result };            
        }
        #endregion Api for Mobile

        #region Api for Web

        #region Search User Signature Template
        [HttpGet]
        [AbpAuthorize(AppPermissions.Pages_Master_EsignUserImageApi_GetAllSignature)]
        public async Task<PagedResultDto<MstEsignUserImageOutputDto>> GetAllSignature([FromQuery] PagedInputDto input)
        {
            try
            {
                var result = from signature in _mstEsignUserImageRepo.GetAll().AsNoTracking()
                             .Where(e => e.CreatorUserId == AbpSession.UserId)
                             select new MstEsignUserImageOutputDto
                             {
                                 Id = signature.Id,
                                 ImgSize = signature.ImgSize,
                                 ImgUrl = string.IsNullOrWhiteSpace(signature.ImgUrl) ? "" : (_webUrlService.ServerRootAddressFormat.EnsureEndsWith('/') + signature.ImgUrl),
                                 IsUse = signature.IsUse ? "Yes" : "No",
                             };
                var pagedResult = result.PageBy(input);
                return new PagedResultDto<MstEsignUserImageOutputDto> { TotalCount = result.Count(), Items = await pagedResult.ToListAsync() };
            }
            catch
            {
                throw new UserFriendlyException(400, L("AnErrorOccurred"));
            }
        }
        #endregion Search Signature

        #region Save Image Signature Template


        // GetListSignatureByUserIdForWeb  
        [HttpGet]
        [AbpAuthorize(AppPermissions.Pages_Master_EsignUserImageApi_GetListSignatureByUserIdForWeb)]

        public async Task<ListResultDto<MstEsignUserImageWebOutputDto>> GetListSignatureByUserIdForWeb()
        {
            var listMstEsignUserImage = _mstEsignUserImageRepo.GetAll().Where(e => e.CreatorUserId == AbpSession.UserId)
                                                                                                               .OrderByDescending(e => e.Id);   
            var result = await (from o in listMstEsignUserImage
                                select new MstEsignUserImageWebOutputDto
                                {
                                    Id = o.Id,
                                    SignerId = AbpSession.UserId,
                                    ImgUrl = (o.ImgUrl == "") ? "" : (_webUrlService.ServerRootAddressFormat.EnsureEndsWith('/') + o.ImgUrl),
                                    IsUse = o.IsUse,

                                }).ToListAsync();
            
            return new ListResultDto<MstEsignUserImageWebOutputDto> { Items = result };
        }

        [HttpPost]
        [AbpAuthorize(AppPermissions.Pages_Master_EsignUserImageApi_UpdateSignatureDefautlForWeb)]

        public async Task UpdateSignatureDefautlForWeb(MstEsignUserImageDefaultWebInput input)
        {
            try
            {
                // lấy image default
                var ImageSignatureDefaults = _mstEsignUserImageRepo.GetAll().Where(e => e.CreatorUserId == input.SignerId &&  e.IsUse == true && e.Id != input.Id);
                foreach(var ImageSignatureDefault in ImageSignatureDefaults)
                {
                    if (ImageSignatureDefault != null)
                    {
                        ImageSignatureDefault.IsUse = false;
                        var newImageSignatureDefault = ObjectMapper.Map<MstEsignUserImage>(ImageSignatureDefault);
                        await _mstEsignUserImageRepo.UpdateAsync(newImageSignatureDefault);
                    }
                }
                // update image default
                var updateImageSignatureDefault = _mstEsignUserImageRepo.FirstOrDefault(e => e.Id == input.Id && e.CreatorUserId == input.SignerId);
                if (updateImageSignatureDefault != null)
                {
                    updateImageSignatureDefault.IsUse = true;
                    var newupdateImageSignatureDefault = ObjectMapper.Map<MstEsignUserImage>(updateImageSignatureDefault);
                    await _mstEsignUserImageRepo.UpdateAsync(newupdateImageSignatureDefault); 
                } 
            }
            catch
            {
                throw new UserFriendlyException(400, L("AnErrorOccurred"));
            }
        }

        [HttpPost]
        [AbpAuthorize(AppPermissions.Pages_Master_EsignUserImageApi_SaveImageSignature)]

        public async Task<string> SaveImageSignature(MstEsignUserImageSignatureInput input)
        {
            try
            {
                
                var serverPath = AppConsts.C_WWWROOT + "/Signatures/Template/" + input.SignerId.ToString() + "/";

                bool exists = Directory.Exists(serverPath); 
                if (!exists) Directory.CreateDirectory(serverPath);
                string _filename = input.SignerId.ToString() +  "_" + System.DateTime.Now.ToFileTimeUtc() + ".png";
                string _fullpartname = serverPath + _filename;


                MemoryStream imageStream = new MemoryStream(input.imageSignature);
                Image img = Image.FromStream(imageStream);
     
                img.Save(_fullpartname, ImageFormat.Png);

                //update not default
                // lấy image default
                var ImageSignatureDefault = _mstEsignUserImageRepo.FirstOrDefault(e => e.CreatorUserId == input.SignerId && e.IsUse == true);
                if (ImageSignatureDefault != null)
                {
                    ImageSignatureDefault.IsUse = false;
                    var newImageSignatureDefault = ObjectMapper.Map<MstEsignUserImage>(ImageSignatureDefault);
                    await _mstEsignUserImageRepo.UpdateAsync(newImageSignatureDefault);
                }


               var listSignatures =  _mstEsignUserImageRepo.GetAll().Where(e => e.CreatorUserId == AbpSession.UserId).ToList();
                if (listSignatures.Any(e=> e.Order == 4))
                {
                    //Delete record 
                    var _delete = listSignatures.FirstOrDefault(e => e.Order == 4);
                    // Delete image
                    var serverFullPath = AppConsts.C_WWWROOT + "/" + _delete.ImgUrl;
                    bool _exists = File.Exists(serverFullPath);
                    if (_exists) File.Delete(serverFullPath);

                    // có sử dụng hard delete? 
                    await _mstEsignUserImageRepo.HardDeleteAsync(_delete); 
                }

                // Cộng order lên 1 
                for(int i = 0; i< listSignatures.Count;i++)
                {
                    if (listSignatures[i].Order == 4) continue;
                    else listSignatures[i].Order = listSignatures[i].Order + 1; 
                    _mstEsignUserImageRepo.Update(listSignatures[i]);
                }
                

                //insert  
                string _partname = _fullpartname.Replace(AppConsts.C_WWWROOT + "/", "");
                CreateEsignUserImageCreatedInput obj = new CreateEsignUserImageCreatedInput();
                obj.ImgUrl = _partname;
                obj.ImgSize = imageStream.Length;
                obj.ImgName = _filename;
                obj.ImgWidth = img.Width;
                obj.ImgHeight = img.Height; 
                obj.IsUse = true;
                obj.Order = 1;

                var newImageData = ObjectMapper.Map<MstEsignUserImage>(obj);
                await _mstEsignUserImageRepo.InsertAsync(newImageData);

                return _partname;
            }
            catch
            {
                throw new UserFriendlyException(400, L("AnErrorOccurred"));
            }
        }

        [HttpPost]
        [AbpAuthorize(AppPermissions.Pages_Master_EsignUserImageApi_DeleteTemplateImageSignature)]

        public async Task DeleteTemplateImageSignature([FromQuery] MstEsignUserImageSignatureDeleteInput input)
        {
            try
            {   
                var ImageSignature = _mstEsignUserImageRepo.FirstOrDefault(e => e.Id == input.SignatureId && e.CreatorUserId == input.SignerId);
                if (ImageSignature == null)
                {
                    throw new UserFriendlyException(L("RecordExists"));
                }else
                {
                    // Delete image
                    var serverFullPath = AppConsts.C_WWWROOT + "/" + ImageSignature.ImgUrl;
                    bool exists = File.Exists(serverFullPath);
                    if (exists) File.Delete(serverFullPath);


                    // có sử dụng hard delete?
                    //Delete record 
                    await _mstEsignUserImageRepo.DeleteAsync(ImageSignature);
                }  
            }
            catch
            {
                throw new UserFriendlyException(400, L("AnErrorOccurred"));
            }
        }




        #endregion Save Image Signature Template

        #endregion Api for Web
    }
}

