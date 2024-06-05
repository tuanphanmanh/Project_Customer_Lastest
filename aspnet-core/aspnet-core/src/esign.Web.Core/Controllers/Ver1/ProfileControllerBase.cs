using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Abp.AspNetZeroCore.Net;
using Abp.Extensions;
using Abp.IO.Extensions;
using Abp.UI;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using esign.Authorization.Users.Profile;
using esign.Dto;
using esign.Graphics;
using esign.Storage;
using esign.Authorization.Users.Profile.Ver1;

namespace esign.Web.Controllers.Ver1
{
    [ApiVersion("1")]
    public abstract class ProfileControllerBase : esignVersionControllerBase
    {
        private readonly ITempFileCacheManager _tempFileCacheManager;
        private readonly IProfileAppService _profileAppService;
        private readonly IImageFormatValidator _imageFormatValidator;
        
        private const int MaxProfilePictureSize = 5242880; //5MB

        protected ProfileControllerBase(
            ITempFileCacheManager tempFileCacheManager,
            IProfileAppService profileAppService, 
            IImageFormatValidator imageFormatValidator)
        {
            _tempFileCacheManager = tempFileCacheManager;
            _profileAppService = profileAppService;
            _imageFormatValidator = imageFormatValidator;
        }
        
        [HttpPost]
        public void UploadProfilePicture(FileDto input)
        {
            var profilePictureFile = Request.Form.Files.First();

            //Check input
            if (profilePictureFile == null)
            {
                throw new UserFriendlyException(L("ProfilePicture_Change_Error"));
            }

            if (profilePictureFile.Length > MaxProfilePictureSize)
            {
                throw new UserFriendlyException(L("ProfilePicture_Warn_SizeLimit",
                    AppConsts.MaxProfilePictureBytesUserFriendlyValue));
            }

            byte[] fileBytes;
            using (var stream = profilePictureFile.OpenReadStream())
            {
                fileBytes = stream.GetAllBytes();
                _imageFormatValidator.Validate(fileBytes);
            }

            _tempFileCacheManager.SetFile(input.FileToken, fileBytes);
        }

        [AllowAnonymous]
        [HttpGet]
        public FileResult GetDefaultProfilePicture()
        {
            return GetDefaultProfilePictureInternal();
        }

        private async Task<FileResult> GetProfilePictureByUser(long userId)
        {
            var output = await _profileAppService.GetProfilePictureByUser(userId);
            if (output.ProfilePicture.IsNullOrEmpty())
            {
                return GetDefaultProfilePictureInternal();
            }

            return File(Convert.FromBase64String(output.ProfilePicture), MimeTypeNames.ImageJpeg);
        }

        protected FileResult GetDefaultProfilePictureInternal()
        {
            return File(Path.Combine("Common", "Images", "default-profile-picture.png"), MimeTypeNames.ImagePng);
        }
    }
}