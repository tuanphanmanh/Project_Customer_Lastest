using Abp.Authorization;
using Abp.Dapper.Repositories;
using Abp.UI;
using Dapper;
using esign.Authorization;
using esign.Authorization.Users;
using esign.Configuration;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace esign.Business.Ver1
{
    [AbpAuthorize]
    [AbpAuthorize(AppPermissions.Pages_EsignFeedbackHub)]
    public class EsignFeedbackHubAppService : esignVersion1AppServiceBase
    {
        private readonly IDapperRepository<User, long> _dapperRepo;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IConfigurationRoot _appConfiguration;

        public EsignFeedbackHubAppService(
            IDapperRepository<User, long> dapperRepo,
            IWebHostEnvironment hostingEnvironment
            )
        {
            _dapperRepo = dapperRepo;
            _hostingEnvironment = hostingEnvironment;
            _appConfiguration = hostingEnvironment.GetAppConfiguration();
        }

        [HttpPost]
        [Consumes("multipart/form-data")]
        [AbpAuthorize(AppPermissions.Pages_EsignFeedbackHub_Feedback)]
        public async Task FeedBack([FromForm]CreateEsignFeedbackHubDto input)
        {
            try
            {
                string imgUrls = "";
                if (input.images.Count() > 0)
                {
                    foreach (var image in input.images)
                    {
                        using (var memoryStream = new MemoryStream())
                        {
                            await image.CopyToAsync(memoryStream);
                            var fileName = Path.GetFileNameWithoutExtension(image.FileName);
                            var fileExtension = Path.GetExtension(image.FileName);
                            var newFileName = fileName + DateTime.Now.ToString("yyyyMMddHHmmssfff") + fileExtension;
                            string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Images", "Feedback");
                            if (!Directory.Exists(path))
                            {
                                Directory.CreateDirectory(path);
                            }
                            path = Path.Combine(path, newFileName);
                            string url = "Images/Feedback/" + newFileName;
                            imgUrls = imgUrls + "~" + url;
                            using (var fileStream = new FileStream(path, FileMode.Create))
                            {
                                await image.CopyToAsync(fileStream);
                            }
                        }
                    }
                }

                string connectionString = _appConfiguration.GetConnectionString("Second");
                using(SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.ExecuteAsync(@"
                    EXEC dbo.Feedback 
                        @CreatorUserId = @CreatorUserId, 
                        @UserComment = @UserComment, 
                        @ImgUrls = @ImgUrls
                    ", new
                    {
                        CreatorUserId = AbpSession.UserId,
                        UserComment = input.UserComment,
                        ImgUrls = imgUrls
                    });
                }    
            }
            catch(Exception ex)
            {
                 throw ex;
            }
        }
    }
}
