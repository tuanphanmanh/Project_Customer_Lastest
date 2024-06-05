using System.Threading.Tasks;

namespace esign.Net.Sms
{
    public interface ISmsSender
    {
        Task SendAsync(string number, string message);
    }
}