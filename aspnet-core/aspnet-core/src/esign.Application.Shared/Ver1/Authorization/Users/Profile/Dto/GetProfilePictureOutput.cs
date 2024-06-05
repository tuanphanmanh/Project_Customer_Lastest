namespace esign.Authorization.Users.Profile.Dto.Ver1
{
    public class GetProfilePictureOutput
    {
        public string ProfilePicture { get; set; }

        public GetProfilePictureOutput(string profilePicture)
        {
            ProfilePicture = profilePicture;
        }
    }
}
