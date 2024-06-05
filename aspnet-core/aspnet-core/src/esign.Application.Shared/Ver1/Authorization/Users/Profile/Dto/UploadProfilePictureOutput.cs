using Abp.Web.Models;

namespace esign.Authorization.Users.Profile.Dto.Ver1
{
    public class UploadProfilePictureOutput : ErrorInfo
    {
        public UploadProfilePictureOutput()
        {
            
        }

        public UploadProfilePictureOutput(ErrorInfo error)
        {
            Code = error.Code;
            Details = error.Details;
            Message = error.Message;
            ValidationErrors = error.ValidationErrors;
        }
    }
}