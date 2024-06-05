using System.ComponentModel.DataAnnotations;

namespace esign.DynamicEntityPropertyValues.Dto.Ver1
{
    public class GetAllDynamicEntityPropertyValuesInput
    {
        [Required]
        public string EntityFullName { get; set; }

        [Required]
        public string EntityId { get; set; }
    }
}
