using System.Threading.Tasks;
using esign.Authorization.Users;

namespace esign.WebHooks
{
    public interface IAppWebhookPublisher
    {
        Task PublishTestWebhook();
    }
}
