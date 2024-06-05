using Abp.Application.Services.Dto;
using esign.Dto;

namespace esign.Ver1.Esign.Business.EsignReport.Dto
{
    public class EsignActivityReportDto : EntityDto<long?>
    {
        public string Name { get; set; }
        public string Username { get; set; }
        public string EmailAddress { get; set; }
        public int? Request { get; set; }
        public int? Reassigned { get; set; }
        public int? Rejected { get; set; }
        public int? Viewed { get; set; }
        public int? Shared { get; set; }
        public int? Signed { get; set; }
        public int? Transferred { get; set; }
        public int? AdditionalRefDoc { get; set; }
        public int? Reminded { get; set; }
        public int? Commented { get; set; }
        public int? Revoked { get; set; }
        public int? Total { get; set; }
        public int TotalCount { get; set; }
    }

    public class EsignActivityReportInput : PagedInputDto
    {
        public string Name { get; set; }
        public string EmailAddress { get; set; }
    }
}
