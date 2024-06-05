using Abp.Authorization;
using Abp.Dapper.Repositories;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Dapper;
using esign.Authorization;
using esign.Authorization.Users;
using esign.Configuration;
using esign.EntityFrameworkCore;
using esign.Esign;
using esign.Master;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Quartz;
using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace esign.Ver1.Authorization.Users
{
    [AbpAuthorize]
    public class SchedulerSyncDefaultAvatar : esignAppServiceBase, IJob
    {
        //private readonly esignDbContext _dbContext;
        private readonly UserManager _userManager;
        //private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly IDapperRepository<User, long> _dapperRepo;
        private readonly IConfigurationRoot _appConfiguration;
        private string _connectionString;
        private readonly IRepository<EsignJobLog> _esignJobLog;
        public SchedulerSyncDefaultAvatar(
            UserManager userManager,
            //IUnitOfWorkManager unitOfWorkManager,
            IDapperRepository<User, long> dapperRepo,
            //esignDbContext dbContext,
            IWebHostEnvironment env,
            IRepository<EsignJobLog> esignJobLog)
        {
            _userManager = userManager;
            //_unitOfWorkManager = unitOfWorkManager;
            _dapperRepo = dapperRepo;
           // _dbContext = dbContext;
            _appConfiguration = env.GetAppConfiguration();
            _connectionString = _appConfiguration.GetConnectionString(esignConsts.ConnectionStringName);
            _esignJobLog = esignJobLog;
        }
        [HttpPost]
        public async Task Execute(IJobExecutionContext context)
        {

            try
            {
                await UpdateDefautAvatarJob();
            }
            catch (Exception ex)
            {

            }

        }
        [HttpPost]
        private async Task UpdateDefautAvatarJob()
        {
            var log = new EsignJobLog();
            log.Name = "SchedulerSyncDefaultAvatar/UpdateDefautAvatarJob";
            try
            {
                using (var cnn = new SqlConnection(_connectionString))
                {
                    //var users = cnn.Query<User>("SELECT Id, Name, EmailAddress, * FROM AbpUsers WHERE (ImageUrl IS NULL OR ImageUrl = '') AND IsDeleted = 0 AND Id = 1114").ToList();

                    var listUsers = await cnn.QueryAsync<User>(
                        "EXEC dbo.UpdateDefautAvatarJob_GetUsers"
                        );
                    var users = listUsers.ToList();

                    foreach (var user in users)
                    {
                        string imageUrl = await CreateDefaultAvatarForUser(user.Name, user.EmailAddress);
                        user.ImageUrl = imageUrl;

                        // Assuming you have an Update method in your Dapper repository
                        // Adjust this part based on your actual data access layer
                        //cnn.Execute("UPDATE AbpUsers SET ImageUrl = @ImageUrl WHERE Id = @Id", new { ImageUrl = user.ImageUrl, Id = user.Id });
                        await cnn.ExecuteAsync(
                            @"UpdateDefautAvatarJob_UpdateImageUrl
                            @Userid = @Userid,
                            @ImageUrl = @ImageUrl",
                            new
                            {
                                ImageUrl = user.ImageUrl,
                                Userid = user.Id
                            });
                    }

                    log.Status = "Success";
                    await _esignJobLog.InsertAsync(log);
                }
            }
            catch(Exception ex)
            {
                log.Status = "Fail";
                log.Message = ex.Message;
                await _esignJobLog.InsertAsync(log);
            }
            
        }
        [HttpPost]
        [AbpAuthorize(AppPermissions.Pages_SchedulerSync)]
        public async Task UpdateDefautAvatar()
        {
            var log = new EsignJobLog();
            log.Name = "SchedulerSyncDefaultAvatar/UpdateDefautAvatarJob";
            try
            {
                using (var cnn = new SqlConnection(_connectionString))
                {
                    //var users = cnn.Query<User>("SELECT Id, Name, EmailAddress, * FROM AbpUsers WHERE (ImageUrl IS NULL OR ImageUrl = '') AND IsDeleted = 0 AND Id = 1114").ToList();

                    var listUsers = await cnn.QueryAsync<User>(
                        "EXEC dbo.UpdateDefautAvatarJob_GetUsers"
                        );
                    var users = listUsers.ToList();

                    foreach (var user in users)
                    {
                        string imageUrl = await CreateDefaultAvatarForUser(user.Name, user.EmailAddress);
                        user.ImageUrl = imageUrl;

                        // Assuming you have an Update method in your Dapper repository
                        // Adjust this part based on your actual data access layer
                        //cnn.Execute("UPDATE AbpUsers SET ImageUrl = @ImageUrl WHERE Id = @Id", new { ImageUrl = user.ImageUrl, Id = user.Id });
                        await cnn.ExecuteAsync(
                            @"UpdateDefautAvatarJob_UpdateImageUrl
                            @Userid = @Userid,
                            @ImageUrl = @ImageUrl",
                            new
                            {
                                ImageUrl = user.ImageUrl,
                                Userid = user.Id
                            });
                    }

                    log.Status = "Success";
                    await _esignJobLog.InsertAsync(log);
                }
            }
            catch (Exception ex)
            {
                log.Status = "Fail";
                log.Message = ex.Message;
                await _esignJobLog.InsertAsync(log);
            }

        }
        [HttpPost]
        private async Task<string> CreateDefaultAvatarForUser(string name, string email)
        {
            // Ensure you have a valid access token for authentication.
            try
            {
                string shortName = ConvertShortnameToCreateDefaultAvatar(name);
                int width = 64;
                int height = 64;

                using (Bitmap bitmap = new Bitmap(width, height))
                {
                    using (System.Drawing.Graphics graphics = System.Drawing.Graphics.FromImage(bitmap))
                    {
                        // Set the background color
                        graphics.Clear(Color.DeepSkyBlue);

                        // Set the font and color
                        Font font = new Font("Baltica", 20, FontStyle.Bold);
                        SolidBrush brush = new SolidBrush(Color.White);

                        // Get the user's initials (first two characters of the name)
                        string initials = shortName.ToUpper();

                        // Calculate the position to write the initials
                        SizeF textSize = graphics.MeasureString(initials, font);
                        float x = (width - textSize.Width) / 2;
                        float y = (height - textSize.Height) / 2;

                        // Draw the initials on the image
                        graphics.DrawString(initials, font, brush, x, y);

                        // Create a MemoryStream to hold the image data
                        using (MemoryStream ms = new MemoryStream())
                        {
                            bitmap.Save(ms, ImageFormat.Png);
                            // Save the image to the MemoryStream in PNG format
                            // Read the photo as bytes
                            byte[] photoBytes = ms.ToArray();

                            // Generate a unique filename for the saved image
                            var newFileName = "profile_photo" + email + ".jpg";

                            // Define the image URL and path
                            var imageUrl = Path.Combine("Images", "ProfileDefault", newFileName).Replace("\\", "/");
                            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Images", "ProfileDefault", newFileName);

                            // Save the image to the server
                            using (var fileStream = new FileStream(path, FileMode.Create))
                            {
                                await fileStream.WriteAsync(photoBytes);

                                // Optionally, you can delete the old image if it exists
                            }

                            // Update the user's ImageUrl in your database
                            return imageUrl;
                        }

                    }

                }
            }
            catch (Exception ex)
            {
                return "";
            }

        }
        [HttpPost]
        private string ConvertShortnameToCreateDefaultAvatar(string fullName)
        {
            // Convert to lowercase
            string lowerCaseFullName = fullName.ToLower();

            // Split by spaces
            string[] nameParts = lowerCaseFullName.Split(' ');

            // Initialize the username
            string username = string.Empty;

            if (nameParts.Length > 0 && !string.IsNullOrEmpty(nameParts[0]))
            {
                username = nameParts[0].Substring(0, 1);

                if (nameParts.Length > 1 && !string.IsNullOrEmpty(nameParts[1]))
                {
                    username += nameParts[1].Substring(0, 1);
                }
            }


            // Take the first character of each part and add it to the username
            return username;
        }
    }
}
