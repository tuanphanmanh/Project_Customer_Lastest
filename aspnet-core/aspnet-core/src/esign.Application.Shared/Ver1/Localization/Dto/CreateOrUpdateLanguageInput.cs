using System.ComponentModel.DataAnnotations;

namespace esign.Localization.Dto.Ver1
{
    public class CreateOrUpdateLanguageInput
    {
        [Required]
        public ApplicationLanguageEditDto Language { get; set; }
    }
}