using System.ComponentModel.DataAnnotations;

namespace esign.Authorization.Users.Dto.Ver1
{
    public class ChangeUserLanguageDto
    {
        [Required]
        public string LanguageName { get; set; }
    }
}
