using System.Threading.Tasks;
using Abp.Webhooks;

namespace esign.WebHooks
{
    public interface IWebhookEventAppService
    {
        Task<WebhookEvent> Get(string id);
    }
}
