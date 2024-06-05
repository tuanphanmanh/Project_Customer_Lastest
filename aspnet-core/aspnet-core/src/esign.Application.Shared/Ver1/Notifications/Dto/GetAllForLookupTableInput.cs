using Abp.Application.Services.Dto;

namespace esign.Notifications.Dto.Ver1
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
    }
}