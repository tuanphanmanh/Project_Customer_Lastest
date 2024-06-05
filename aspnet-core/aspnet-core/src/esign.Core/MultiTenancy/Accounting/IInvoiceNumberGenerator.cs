using System.Threading.Tasks;
using Abp.Dependency;

namespace esign.MultiTenancy.Accounting
{
    public interface IInvoiceNumberGenerator : ITransientDependency
    {
        Task<string> GetNewInvoiceNumber();
    }
}