using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using esign.MultiTenancy.Accounting.Dto.Ver1;

namespace esign.MultiTenancy.Accounting.Ver1
{
    public interface IInvoiceAppService
    {
        Task<InvoiceDto> GetInvoiceInfo(EntityDto<long> input);

        Task CreateInvoice(CreateInvoiceDto input);
    }
}
