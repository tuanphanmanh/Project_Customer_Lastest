using System.ComponentModel.DataAnnotations;
using Abp.Localization;

namespace esign.Localization.Dto.Ver1
{
    public class SetDefaultLanguageInput
    {
        [Required]
        [StringLength(ApplicationLanguage.MaxNameLength)]
        public virtual string Name { get; set; }
    }
}