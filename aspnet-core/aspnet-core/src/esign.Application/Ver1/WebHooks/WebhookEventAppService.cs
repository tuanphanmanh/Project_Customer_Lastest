using System;
using System.Threading.Tasks;
using Abp.Authorization;
using Abp.Webhooks;
using esign.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace esign.WebHooks.Ver1
{
    [AbpAuthorize(AppPermissions.Pages_Administration_WebhookSubscription)]
    public class WebhookEventAppService : esignVersion1AppServiceBase, IWebhookEventAppService
    {
        private readonly IWebhookEventStore _webhookEventStore;

        public WebhookEventAppService(IWebhookEventStore webhookEventStore)
        {
            _webhookEventStore = webhookEventStore;
        }

        [HttpGet]
        public async Task<WebhookEvent> Get(string id)
        {
            return await _webhookEventStore.GetAsync(AbpSession.TenantId, Guid.Parse(id));
        }
    }
}
