using System.ComponentModel.DataAnnotations;

namespace esign.Organizations.Dto.Ver1
{
    public class UserToOrganizationUnitInput
    {
        [Range(1, long.MaxValue)]
        public long UserId { get; set; }

        [Range(1, long.MaxValue)]
        public long OrganizationUnitId { get; set; }
    }
}