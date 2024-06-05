using System.ComponentModel.DataAnnotations;
using Abp.Organizations;

namespace esign.Organizations.Dto.Ver1
{
    public class CreateOrganizationUnitInput
    {
        public long? ParentId { get; set; }

        [Required]
        [StringLength(OrganizationUnit.MaxDisplayNameLength)]
        public string DisplayName { get; set; } 
    }
}