using System;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Runtime.Session;
using Abp.Timing;
using Abp.UI;
using esign.Authorization;
using esign.Configuration;
using esign.Editions;
using esign.MultiTenancy.Accounting.Dto.Ver1;
using esign.MultiTenancy.Payments;
using Microsoft.AspNetCore.Mvc;

namespace esign.MultiTenancy.Accounting.Ver1
{
    public class InvoiceAppService : esignVersion1AppServiceBase, IInvoiceAppService
    {
        private readonly ISubscriptionPaymentRepository _subscriptionPaymentRepository;
        private readonly IInvoiceNumberGenerator _invoiceNumberGenerator;
        private readonly EditionManager _editionManager;
        private readonly IRepository<Invoice> _invoiceRepository;

        public InvoiceAppService(
            ISubscriptionPaymentRepository subscriptionPaymentRepository,
            IInvoiceNumberGenerator invoiceNumberGenerator,
            EditionManager editionManager,
            IRepository<Invoice> invoiceRepository)
        {
            _subscriptionPaymentRepository = subscriptionPaymentRepository;
            _invoiceNumberGenerator = invoiceNumberGenerator;
            _editionManager = editionManager;
            _invoiceRepository = invoiceRepository;
        }

        [HttpGet]
        [AbpAuthorize(AppPermissions.Pages_Business_Invoice_GetInvoiceInfo)]
        public async Task<InvoiceDto> GetInvoiceInfo([FromQuery] EntityDto<long> input)
        {
            var payment = await _subscriptionPaymentRepository.GetAsync(input.Id);

            if (string.IsNullOrEmpty(payment.InvoiceNo))
            {
                throw new Exception("There is no invoice for this payment !");
            }

            if (payment.TenantId != AbpSession.GetTenantId())
            {
                throw new UserFriendlyException(L("ThisInvoiceIsNotYours"));
            }

            var invoice = await _invoiceRepository.FirstOrDefaultAsync(b => b.InvoiceNo == payment.InvoiceNo);
            if (invoice == null)
            {
                throw new UserFriendlyException();
            }

            var edition = await _editionManager.FindByIdAsync(payment.EditionId);
            var hostAddress = await SettingManager.GetSettingValueAsync(AppSettings.HostManagement.BillingAddress);

            return new InvoiceDto
            {
                InvoiceNo = payment.InvoiceNo,
                InvoiceDate = invoice.InvoiceDate,
                Amount = payment.Amount,
                EditionDisplayName = edition.DisplayName,

                HostAddress = hostAddress.Replace("\r\n", "|").Split('|').ToList(),
                HostLegalName = await SettingManager.GetSettingValueAsync(AppSettings.HostManagement.BillingLegalName),

                TenantAddress = invoice.TenantAddress.Replace("\r\n", "|").Split('|').ToList(),
                TenantLegalName = invoice.TenantLegalName,
                TenantTaxNo = invoice.TenantTaxNo
            };
        }

        [UnitOfWork(IsolationLevel.ReadUncommitted)]
        [HttpPost]
        [AbpAuthorize(AppPermissions.Pages_Business_Invoice_CreateInvoice)]
        public async Task CreateInvoice(CreateInvoiceDto input)
        {
            var payment = await _subscriptionPaymentRepository.GetAsync(input.SubscriptionPaymentId);
            if (!string.IsNullOrEmpty(payment.InvoiceNo))
            {
                throw new Exception("Invoice is already generated for this payment.");
            }

            var invoiceNo = await _invoiceNumberGenerator.GetNewInvoiceNumber();

            var tenantLegalName = await SettingManager.GetSettingValueAsync(AppSettings.TenantManagement.BillingLegalName);
            var tenantAddress = await SettingManager.GetSettingValueAsync(AppSettings.TenantManagement.BillingAddress);
            var tenantTaxNo = await SettingManager.GetSettingValueAsync(AppSettings.TenantManagement.BillingTaxVatNo);

            if (string.IsNullOrEmpty(tenantLegalName) || string.IsNullOrEmpty(tenantAddress) || string.IsNullOrEmpty(tenantTaxNo))
            {
                throw new UserFriendlyException(L("InvoiceInfoIsMissingOrNotCompleted"));
            }

            await _invoiceRepository.InsertAsync(new Invoice
            {
                InvoiceNo = invoiceNo,
                InvoiceDate = Clock.Now,
                TenantLegalName = tenantLegalName,
                TenantAddress = tenantAddress,
                TenantTaxNo = tenantTaxNo
            });

            payment.InvoiceNo = invoiceNo;
        }
    }
}