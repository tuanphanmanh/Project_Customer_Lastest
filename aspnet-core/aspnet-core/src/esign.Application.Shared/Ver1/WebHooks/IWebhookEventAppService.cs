using System.Threading.Tasks;
using Abp.Webhooks;

namespace esign.WebHooks.Ver1
{
    public interface IWebhookEventAppService
    {
        Task<WebhookEvent> Get(string id);
    }
}
