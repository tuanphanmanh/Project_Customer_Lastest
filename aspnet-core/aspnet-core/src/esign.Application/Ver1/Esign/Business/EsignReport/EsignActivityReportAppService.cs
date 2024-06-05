using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Dapper.Repositories;
using esign.Authorization;
using esign.Authorization.Users;
using esign.Dto;
using esign.Esign.Ver1.Business.EsignReport;
using esign.Ver1.Esign.Business.EsignReport.Dto;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace esign.Ver1.Esign.Business.EsignReport
{
    [AbpAuthorize]
    public class EsignActivityReportAppService: esignVersion1AppServiceBase
    {
        private readonly IDapperRepository<User, long> _dapperRepo;
        private readonly IEsignActivityReportExcelExporter _exporter;
        public EsignActivityReportAppService(
            IDapperRepository<User, long> dapperRepo,
            IEsignActivityReportExcelExporter exporter
            )
        {
            _dapperRepo = dapperRepo;
            _exporter = exporter;
        }


        [AbpAuthorize(AppPermissions.Pages_Administration_AuditReports)]
        [HttpGet]
        public async Task<PagedResultDto<EsignActivityReportDto>>GetAllEsignActivityReport([FromQuery]EsignActivityReportInput input)
        {
            string sql = @"Exec SP_GetUserActivityReport @p_name, @p_email, @p_skipCount, @p_maxResultCount";
            var res = await _dapperRepo.QueryAsync<EsignActivityReportDto>(sql, new
            {
                p_name = input.Name,
                p_email = input.EmailAddress,
                p_skipCount = input.SkipCount,
                p_maxResultCount = input.MaxResultCount,

            });
            if (res.Count() > 0)
            {
                return new PagedResultDto<EsignActivityReportDto>
                {
                    TotalCount = res.First().TotalCount,
                    Items = res.ToList()
                };
            }
            else
            {
                return new PagedResultDto<EsignActivityReportDto>();
            }
        }

        [AbpAuthorize(AppPermissions.Pages_Administration_AuditReports)]
        [HttpGet]
        public async Task<FileDto> GetAllEsignActivityReportExcel([FromQuery] EsignActivityReportInput input)
        {
            string sql = @"Exec SP_GetUserActivityReport @p_name, @p_email, @p_skipCount, @p_maxResultCount";
            var res = await _dapperRepo.QueryAsync<EsignActivityReportDto>(sql, new
            {
                p_name = input.Name,
                p_email = input.EmailAddress,
                p_skipCount = 0,
                p_maxResultCount = 10000,

            });
            if (res.Count() > 0)
            {
                var total = new EsignActivityReportDto 
                {
                    Name = "TOTAL",
                    Request = res.Sum(x => x.Request),
                    Rejected = res.Sum(x => x.Rejected),
                    Viewed = res.Sum(x => x.Viewed),
                    Shared = res.Sum(x => x.Shared),
                    Signed = res.Sum(x => x.Signed),
                    Transferred = res.Sum(x => x.Transferred),
                    AdditionalRefDoc = res.Sum(x => x.AdditionalRefDoc),
                    Reminded = res.Sum(x => x.Reminded),
                    Commented = res.Sum(x => x.Commented),
                    Revoked = res.Sum(x => x.Revoked),
                    Total = res.Sum(x => x.Total),
                };
                return _exporter.ExportToFile(res.Append(total).ToList());
            }
            else
            {
                return _exporter.ExportToFile(new List<EsignActivityReportDto>());
            }
        }
    }
}
