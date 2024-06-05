using esign.Dto;

namespace esign.WebHooks.Dto.Ver1
{
    public class GetAllSendAttemptsInput : PagedInputDto
    {
        public string SubscriptionId { get; set; }
    }
}
