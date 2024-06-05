using Microsoft.AspNetCore.Http;

namespace esign.Authorization.Users.Profile.Dto.Ver1
{
    public class UpdateProfilePictureInput
    {
        public IFormFile File { get; set; }
    }
}