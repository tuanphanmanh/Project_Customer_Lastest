using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.UI;
using Dapper;
using esign.Authorization;
using esign.Business.Dto.Ver1;
using esign.Configuration;
using esign.Esign;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace esign.Business.Ver1
{
   
    public class EsignVersionAppAppService : esignVersion1AppServiceBase
    {
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IConfigurationRoot _appConfiguration;
        private readonly IRepository<EsignVersionApp,long> _esignVersionAppRepo;
        private string _connectionString;
        public EsignVersionAppAppService(
            IWebHostEnvironment hostingEnvironment,
            IRepository<EsignVersionApp,long> esignVersionAppRepo
            )
        {
            _esignVersionAppRepo = esignVersionAppRepo;
            _hostingEnvironment = hostingEnvironment;
            _appConfiguration = hostingEnvironment.GetAppConfiguration();
            _connectionString = _appConfiguration.GetConnectionString(esignConsts.ConnectionStringName);
        }
        [AbpAllowAnonymous]
        [HttpPost]
        //[AbpAuthorize(AppPermissions.Pages_Business_EsignVersionApp_getEsignVersionApp)]
        public VersionAppDto getEsignVersionApp()
        {
            try
            {
                var res =new VersionAppDto();
                using (var cnn = new SqlConnection(_connectionString))
                {
                    string sqlQuery = "exec [Sp_EsignVersion_GetEsignVersion] @p_operation_type_id = @p_operation_type_id";

                    res.Ios = cnn.QueryFirstOrDefault<VersionDetailAppDto>(sqlQuery, new { p_operation_type_id = 1 });
                    sqlQuery = "exec [Sp_EsignVersion_GetEsignVersion] @p_operation_type_id = @p_operation_type_id";

                    res.Android = cnn.QueryFirstOrDefault<VersionDetailAppDto>(sqlQuery, new { p_operation_type_id = 2 });
                }
                return res;
            }
            catch(UserFriendlyException ex)
            {
                throw ex;
            }
        }
        [AbpAuthorize]
        [HttpPost]
        [AbpAuthorize(AppPermissions.Pages_Business_EsignVersionApp_CreateEsignVersion)]
        public async Task CreateEsignVersion(VersionDetailAppDto input)
        {
            try
            {
                var existSystem = _esignVersionAppRepo.FirstOrDefault(e => e.VersionName == input.VersionName && e.OperatingSystem == input.OperatingSystem);
                if (existSystem != null)
                {
                    throw new UserFriendlyException(L("Version Existed"));
                }
                else
                {
                    var esignVersion = new EsignVersionApp();
                    esignVersion.VersionName = input.VersionName;
                    esignVersion.UrlConfig = input.UrlConfig;
                    esignVersion.IsForceUpdate = input.IsForceUpdate;
                    esignVersion.OperatingSystem = input.OperatingSystem;
                    await _esignVersionAppRepo.InsertAsync(esignVersion);
                }
            }
            catch (UserFriendlyException ex)
            {
                throw ex;
            }
        }
    }
}
