using Abp.Authorization;
using Abp.Dapper.Repositories;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Castle.Core.Internal;
using Dapper;
using esign.Authorization;
using esign.Authorization.Users;
using esign.Configuration;
using esign.Master;
using esign.Url;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Quartz;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace esign.Ver1.Authorization.Users
{
    [AbpAuthorize]
    public class SchedulerSyncInforFrom365 : esignAppServiceBase, IJob
    {
        //private readonly esignDbContext _dbContext;
        private readonly UserManager _userManager;
        //private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly IDapperRepository<User, long> _dapperRepo;
        private readonly IConfigurationRoot _appConfiguration;
        private string _connectionString;
        private readonly IRepository<EsignJobLog> _esignJobLog;
        private readonly UserRegistrationManager _userRegistrationManager;
        public IAppUrlService AppUrlService { get; set; }

        private string tenantId;
        private string clientId;
        private string clientSecret;

        public SchedulerSyncInforFrom365(
            UserManager userManager,
            //IUnitOfWorkManager unitOfWorkManager,
            IDapperRepository<User, long> dapperRepo,
            //esignDbContext dbContext,
            IWebHostEnvironment env,
            IRepository<EsignJobLog> esignJobLog,
            UserRegistrationManager userRegistrationManager)
        {
            _userManager = userManager;
            //_unitOfWorkManager = unitOfWorkManager;
            _dapperRepo = dapperRepo;
           // _dbContext = dbContext;
            _appConfiguration = env.GetAppConfiguration();
            _connectionString = _appConfiguration.GetConnectionString(esignConsts.ConnectionStringName);
            _esignJobLog = esignJobLog;
            _userRegistrationManager = userRegistrationManager;

            tenantId = _appConfiguration[$"Parameters:TenantId"];
            clientId = _appConfiguration[$"Parameters:ClientId"];
            clientSecret = _appConfiguration[$"Parameters:ClientSecret"];
        }
        [HttpPost]
        public async Task Execute(IJobExecutionContext context)
        {

            try
            {
                await UpdateUserInforFrom365Job();
            }
            catch (Exception ex)
            {

            }

        }
        [HttpPost]
        private async Task UpdateUserInforFrom365Job()
        {
            var log = new EsignJobLog();
            log.Name = "SchedulerSyncInforFrom365/UpdateUserInforFrom365Job";
            try
            {
                // Microsoft Graph API endpoint
                string graphApiEndpoint = "https://graph.microsoft.com/v1.0/";

                // Get access token
                string accessToken = await GetAccessTokenAsync(clientId, clientSecret, tenantId);

                // Call Microsoft Graph API to get user information
                string usersEndpoint = $"{graphApiEndpoint}users";
                UserListResponse usersJson = new UserListResponse();
                usersJson = await CallGraphApiAsync(usersEndpoint, accessToken);
                #region update info
                foreach (var user in usersJson.value)
                {
                    try
                    {
                        if (user.Mail.Contains(_appConfiguration[$"KeyJob365"].ToString()))
                        {
                            string userEmail = user.Mail; // replace with the actual user email
                            string photoEndpoint = $"{graphApiEndpoint}users/{userEmail}/photo/$value";
                            byte[] photoData = await CallGraphApiBinaryAsync(photoEndpoint, accessToken);
                            using (var cnn = new SqlConnection(_connectionString))
                            {
                                // Retrieve user information based on the user's email
                                string sqlQuery = "exec [GetInforFrom365Job] @PartialEmailAddress";

                                var userInfor = cnn.QueryFirstOrDefault<User>(sqlQuery, new { PartialEmailAddress = $"{userEmail}" });

                                if (userInfor != null)
                                {
                                    string oldImageUrl = userInfor.ImageUrl;
                                    var imageUrl = await CreateDefaultAvatar(user.DisplayName, oldImageUrl, photoData);
                                    // Update the user's ImageUrl in the database
                                    //cnn.Execute("UPDATE AbpUsers SET ImageUrl = @ImageUrl WHERE Id = @Id", new { ImageUrl = imageUrl, Id = userInfor.Id });

                                    await cnn.ExecuteAsync(@"
                                        EXEC dbo.UpdateInforFrom365Job 
                                        @UserId = @UserId,
                                        @ImageUrl =  @ImageUrl,
                                        @DisplayName =  @DisplayName,
                                        @Surname = @Surname,
                                        @GivenName = @GivenName,
                                        @Title =  @Title",

                                    new
                                    {
                                        ImageUrl = imageUrl,
                                        UserId = userInfor.Id,
                                        DisplayName = user.DisplayName,
                                        Surname = user.Surname.IsNullOrEmpty() ? "" : user.Surname,
                                        GivenName = user.GivenName,
                                        Title = user.JobTitle
                                    });

                                }
                                else
                                {
                                    var randomPassword = await _userManager.CreateRandomPassword();
                                    string username = ConvertEmaileToUsername(user.Mail);
                                    var userRegistration = await _userRegistrationManager.RegisterAsync(
                                       user.DisplayName,
                                       user.Surname.IsNullOrEmpty() ? "" : user.Surname,
                                       user.Mail,
                                       username,
                                       randomPassword,
                                       true,
                                       AppUrlService.CreateEmailActivationUrlFormat(AbpSession.TenantId),
                                       true
                                   );
                                    userRegistration.IsAD = true;
                                    userRegistration.GivenName = user.GivenName;
                                    userRegistration.ImageUrl = await CreateDefaultAvatar(user.DisplayName, null, photoData);
                                    using (CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant, AbpDataFilters.MustHaveTenant))
                                    {
                                        await _userManager.UpdateAsync(userRegistration);
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex) {
                        string logQuery = "exec [sp_esign_insertJobLog] @UserId,@ExceptionMessage , @TenantId ";

                        await _dapperRepo.QueryAsync<EsignJobLog>(logQuery, new { UserId = AbpSession.UserId, ExceptionMessage = ex.Message.ToString(), TenantId = AbpSession.TenantId });
                    }

                }
                #endregion
                while (!string.IsNullOrWhiteSpace(usersJson.ODataNextLink))
                {
                    usersJson = await CallGraphApiAsync(usersJson.ODataNextLink, accessToken);
                    #region update info
                    foreach (var user in usersJson.value)
                    {
                        try
                        {
                            if (user.Mail.Contains(_appConfiguration[$"KeyJob365"].ToString()))
                            {
                                string userEmail = user.Mail; // replace with the actual user email
                                string photoEndpoint = $"{graphApiEndpoint}users/{userEmail}/photo/$value";
                                byte[] photoData = await CallGraphApiBinaryAsync(photoEndpoint, accessToken);
                                using (var cnn = new SqlConnection(_connectionString))
                                {
                                    // Retrieve user information based on the user's email
                                    string sqlQuery = "exec [GetInforFrom365Job] @PartialEmailAddress";

                                    var userInfor = cnn.QueryFirstOrDefault<User>(sqlQuery, new { PartialEmailAddress = $"{userEmail}" });

                                    if (userInfor != null)
                                    {
                                        string oldImageUrl = userInfor.ImageUrl;
                                        var imageUrl = await CreateDefaultAvatar(user.DisplayName, oldImageUrl, photoData);
                                        // Update the user's ImageUrl in the database
                                        //cnn.Execute("UPDATE AbpUsers SET ImageUrl = @ImageUrl WHERE Id = @Id", new { ImageUrl = imageUrl, Id = userInfor.Id });

                                        await cnn.ExecuteAsync(@"
                                        EXEC dbo.UpdateInforFrom365Job 
                                        @UserId = @UserId,
                                        @ImageUrl =  @ImageUrl,
                                        @DisplayName =  @DisplayName,
                                        @Surname = @Surname,
                                        @GivenName = @GivenName,
                                        @Title =  @Title",

                                        new
                                        {
                                            ImageUrl = imageUrl,
                                            UserId = userInfor.Id,
                                            DisplayName = user.DisplayName,
                                            Surname = user.Surname.IsNullOrEmpty() ? "" : user.Surname,
                                            GivenName = user.GivenName,
                                            Title = user.JobTitle
                                        });

                                    }
                                    else
                                    {
                                        var randomPassword = await _userManager.CreateRandomPassword();
                                        string username = ConvertEmaileToUsername(user.Mail);
                                        var userRegistration = await _userRegistrationManager.RegisterAsync(
                                           user.DisplayName,
                                           user.Surname.IsNullOrEmpty() ? "" : user.Surname,
                                           user.Mail,
                                           username,
                                           randomPassword,
                                           true,
                                           AppUrlService.CreateEmailActivationUrlFormat(AbpSession.TenantId),
                                           true
                                       );
                                        userRegistration.AuthenticationSource = "365Job";
                                        userRegistration.IsAD = true;
                                        userRegistration.GivenName = user.GivenName;
                                        userRegistration.ImageUrl = await CreateDefaultAvatar(user.DisplayName, null, photoData);
                                        using (CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant, AbpDataFilters.MustHaveTenant))
                                        {
                                            await _userManager.UpdateAsync(userRegistration);
                                        }
                                    }
                                }
                            }
                        }
                        catch (Exception ex) {
                            string logQuery = "exec sp_esign_insertJobLog @UserId,@ExceptionMessage , @TenantId ";

                            await _dapperRepo.QueryAsync<EsignJobLog>(logQuery, new { UserId = AbpSession.UserId, ExceptionMessage = ex.Message.ToString(), TenantId = AbpSession.TenantId });
                        }

                    }
                    #endregion

                }
                log.Status = "Success";
                await _esignJobLog.InsertAsync(log);
                // Example: Call Microsoft Graph API to get the user's photo
            }
            catch (Exception ex)
            {
                log.Status = "Fail";
                log.Message = ex.Message;
                await _esignJobLog.InsertAsync(log);
            }
            // Your Azure AD App registration details

        }
        [HttpPost]
        [AbpAuthorize(AppPermissions.Pages_SchedulerSync)]
        public async Task UpdateUserInforFrom365()
        {
            var log = new EsignJobLog();
            log.Name = "SchedulerSyncInforFrom365/UpdateUserInforFrom365Job";
            try
            {
                // Microsoft Graph API endpoint
                string graphApiEndpoint = "https://graph.microsoft.com/v1.0/";

                // Get access token
                string accessToken = await GetAccessTokenAsync(clientId, clientSecret, tenantId);

                // Call Microsoft Graph API to get user information
                string usersEndpoint = $"{graphApiEndpoint}users";
                UserListResponse usersJson = new UserListResponse();
                usersJson = await CallGraphApiAsync(usersEndpoint, accessToken);
                #region update info
                foreach (var user in usersJson.value)
                {
                    try
                    {
                        if (user.Mail.Contains(_appConfiguration[$"KeyJob365"].ToString()))
                        {
                            string userEmail = user.Mail; // replace with the actual user email
                            string photoEndpoint = $"{graphApiEndpoint}users/{userEmail}/photo/$value";
                            byte[] photoData = await CallGraphApiBinaryAsync(photoEndpoint, accessToken);
                            using (var cnn = new SqlConnection(_connectionString))
                            {
                                // Retrieve user information based on the user's email
                                string sqlQuery = "exec [GetInforFrom365Job] @PartialEmailAddress";

                                var userInfor = cnn.QueryFirstOrDefault<User>(sqlQuery, new { PartialEmailAddress = $"{userEmail}" });

                                if (userInfor != null)
                                {
                                    string oldImageUrl = userInfor.ImageUrl;
                                    var imageUrl = await CreateDefaultAvatar(user.DisplayName, oldImageUrl, photoData);
                                    // Update the user's ImageUrl in the database
                                    //cnn.Execute("UPDATE AbpUsers SET ImageUrl = @ImageUrl WHERE Id = @Id", new { ImageUrl = imageUrl, Id = userInfor.Id });

                                    await cnn.ExecuteAsync(@"
                                        EXEC dbo.UpdateInforFrom365Job 
                                        @UserId = @UserId,
                                        @ImageUrl =  @ImageUrl,
                                        @DisplayName =  @DisplayName,
                                        @Surname = @Surname,
                                        @GivenName = @GivenName,
                                        @Title =  @Title",

                                    new
                                    {
                                        ImageUrl = imageUrl,
                                        UserId = userInfor.Id,
                                        DisplayName = user.DisplayName,
                                        Surname = user.Surname.IsNullOrEmpty() ? "" : user.Surname,
                                        GivenName = user.GivenName,
                                        Title = user.JobTitle
                                    });

                                }
                                else
                                {
                                    var randomPassword = await _userManager.CreateRandomPassword();
                                    string username = ConvertEmaileToUsername(user.Mail);
                                    var userRegistration = await _userRegistrationManager.RegisterAsync(
                                       user.DisplayName,
                                       user.Surname.IsNullOrEmpty() ? "" : user.Surname,
                                       user.Mail,
                                       username,
                                       randomPassword,
                                       true,
                                       AppUrlService.CreateEmailActivationUrlFormat(AbpSession.TenantId),
                                       true
                                   );
                                    userRegistration.IsAD = true;
                                    userRegistration.GivenName = user.GivenName;
                                    userRegistration.ImageUrl = await CreateDefaultAvatar(user.DisplayName, null, photoData);
                                    using (CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant, AbpDataFilters.MustHaveTenant))
                                    {
                                        await _userManager.UpdateAsync(userRegistration);
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        string logQuery = "exec [sp_esign_insertJobLog] @UserId,@ExceptionMessage , @TenantId ";

                        await _dapperRepo.QueryAsync<EsignJobLog>(logQuery, new { UserId = AbpSession.UserId, ExceptionMessage = ex.Message.ToString(), TenantId = AbpSession.TenantId });
                    }

                }
                #endregion
                while (!string.IsNullOrWhiteSpace(usersJson.ODataNextLink))
                {
                    usersJson = await CallGraphApiAsync(usersJson.ODataNextLink, accessToken);
                    #region update info
                    foreach (var user in usersJson.value)
                    {
                        try
                        {
                            if (user.Mail.Contains(_appConfiguration[$"KeyJob365"].ToString()))
                            {
                                string userEmail = user.Mail; // replace with the actual user email
                                string photoEndpoint = $"{graphApiEndpoint}users/{userEmail}/photo/$value";
                                byte[] photoData = await CallGraphApiBinaryAsync(photoEndpoint, accessToken);
                                using (var cnn = new SqlConnection(_connectionString))
                                {
                                    // Retrieve user information based on the user's email
                                    string sqlQuery = "exec [GetInforFrom365Job] @PartialEmailAddress";

                                    var userInfor = cnn.QueryFirstOrDefault<User>(sqlQuery, new { PartialEmailAddress = $"{userEmail}" });

                                    if (userInfor != null)
                                    {
                                        string oldImageUrl = userInfor.ImageUrl;
                                        var imageUrl = await CreateDefaultAvatar(user.DisplayName, oldImageUrl, photoData);
                                        // Update the user's ImageUrl in the database
                                        //cnn.Execute("UPDATE AbpUsers SET ImageUrl = @ImageUrl WHERE Id = @Id", new { ImageUrl = imageUrl, Id = userInfor.Id });

                                        await cnn.ExecuteAsync(@"
                                        EXEC dbo.UpdateInforFrom365Job 
                                        @UserId = @UserId,
                                        @ImageUrl =  @ImageUrl,
                                        @DisplayName =  @DisplayName,
                                        @Surname = @Surname,
                                        @GivenName = @GivenName,
                                        @Title =  @Title",

                                        new
                                        {
                                            ImageUrl = imageUrl,
                                            UserId = userInfor.Id,
                                            DisplayName = user.DisplayName,
                                            Surname = user.Surname.IsNullOrEmpty() ? "" : user.Surname,
                                            GivenName = user.GivenName,
                                            Title = user.JobTitle
                                        });

                                    }
                                    else
                                    {
                                        var randomPassword = await _userManager.CreateRandomPassword();
                                        string username = ConvertEmaileToUsername(user.Mail);
                                        var userRegistration = await _userRegistrationManager.RegisterAsync(
                                           user.DisplayName,
                                           user.Surname.IsNullOrEmpty() ? "" : user.Surname,
                                           user.Mail,
                                           username,
                                           randomPassword,
                                           true,
                                           AppUrlService.CreateEmailActivationUrlFormat(AbpSession.TenantId),
                                           true
                                       );
                                        userRegistration.AuthenticationSource = "365Job";
                                        userRegistration.IsAD = true;
                                        userRegistration.GivenName = user.GivenName;
                                        userRegistration.ImageUrl = await CreateDefaultAvatar(user.DisplayName, null, photoData);
                                        using (CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant, AbpDataFilters.MustHaveTenant))
                                        {
                                            await _userManager.UpdateAsync(userRegistration);
                                        }
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            string logQuery = "exec sp_esign_insertJobLog @UserId,@ExceptionMessage , @TenantId ";

                            await _dapperRepo.QueryAsync<EsignJobLog>(logQuery, new { UserId = AbpSession.UserId, ExceptionMessage = ex.Message.ToString(), TenantId = AbpSession.TenantId });
                        }

                    }
                    #endregion

                }
                log.Status = "Success";
                await _esignJobLog.InsertAsync(log);
                // Example: Call Microsoft Graph API to get the user's photo
            }
            catch (Exception ex)
            {
                log.Status = "Fail";
                log.Message = ex.Message;
                await _esignJobLog.InsertAsync(log);
            }
            // Your Azure AD App registration details

        }
        [HttpPost]
        private async Task<string> CreateDefaultAvatar(string name, string oldImageUrl, byte[] photoData)
        {
            // Ensure you have a valid access token for authentication.
            try
            {

                if (!photoData.IsNullOrEmpty())
                {
                    // Read the photo as bytes
                    byte[] photoBytes = photoData;

                    // Generate a unique filename for the saved image
                    var newFileName = "profile_photo_" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".jpg";

                    // Define the image URL and path
                    var imageUrl = Path.Combine("Images", "Profile", newFileName).Replace("\\", "/");
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Images", "Profile", newFileName);

                    // Save the image to the server
                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        await fileStream.WriteAsync(photoBytes);

                        // Optionally, you can delete the old image if it exists
                        if (oldImageUrl != null)
                        {
                            var oldImageFileName = Path.GetFileName(oldImageUrl);
                            var oldImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", oldImageUrl);

                            if (System.IO.File.Exists(oldImagePath))
                            {
                                System.IO.File.Delete(oldImagePath);
                            }
                        }
                    }

                    // Update the user's ImageUrl in your database
                    return imageUrl;

                }
                else
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
                            string initials = shortName.Substring(0, 2).ToUpper();

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
                                var newFileName = "profile_photo_" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".jpg";

                                // Define the image URL and path
                                var imageUrl = Path.Combine("Images", "Profile", newFileName).Replace("\\", "/");
                                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Images", "Profile", newFileName);

                                // Save the image to the server
                                using (var fileStream = new FileStream(path, FileMode.Create))
                                {
                                    await fileStream.WriteAsync(photoBytes);

                                    // Optionally, you can delete the old image if it exists
                                    if (oldImageUrl != null)
                                    {
                                        var oldImageFileName = Path.GetFileName(oldImageUrl);
                                        var oldImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", oldImageUrl);

                                        if (System.IO.File.Exists(oldImagePath))
                                        {
                                            System.IO.File.Delete(oldImagePath);
                                        }
                                    }
                                }

                                // Update the user's ImageUrl in your database
                                return imageUrl;
                            }

                        }
                    }

                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
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
        [HttpPost]
        private async Task UpdateUserNameFrom365Job()
        {
            // Your Azure AD App registration details
            //string clientId = "701efa1a-6997-442e-b8ea-3f87cb370c88";
            //string clientSecret = "kKS8Q~wJ3tHevAzWKiKyA~MR-m2q1UeBg7dGwdzx";
            //string tenantId = "7f5f18eb-e49c-42fc-a52f-2731d1674ab1";

            // Microsoft Graph API endpoint
            string graphApiEndpoint = "https://graph.microsoft.com/v1.0/";

            // Get access token
            string accessToken = await GetAccessTokenAsync(clientId, clientSecret, tenantId);

            // Call Microsoft Graph API to get user information
            string usersEndpoint = $"{graphApiEndpoint}users";
            UserListResponse usersJson = new UserListResponse();
            usersJson = await CallGraphApiAsync(usersEndpoint, accessToken);
            while (!string.IsNullOrWhiteSpace(usersJson.ODataNextLink))
            {
                usersJson = await CallGraphApiAsync(usersJson.ODataNextLink, accessToken);
                foreach (var user in usersJson.value)
                {
                    try
                    {
                        string userEmail = user.Mail; // replace with the actual user email
                        using (var cnn = new SqlConnection(_connectionString))
                        {
                            // Retrieve user information based on the user's email
                            string sqlQuery = "SELECT Id, Name, EmailAddress FROM AbpUsers WHERE IsDeleted = 0 AND EmailAddress LIKE @PartialEmailAddress";

                            var userInfor = cnn.QueryFirstOrDefault<User>(sqlQuery, new { PartialEmailAddress = $"{userEmail}%" });
                            
                            if (userInfor != null)
                            {
                                // Update the user's ImageUrl in the database
                                cnn.Execute("UPDATE AbpUsers SET Name = @DisplayName WHERE Id = @Id", new { DisplayName = user.DisplayName, Id = userInfor.Id });
                            }
                        }
                    }
                    catch (Exception ex) { }
                }

                // Example: Call Microsoft Graph API to get the user's photo
            }



        }
        [HttpPost]
        private string ConvertFullNameToUsername(string fullName)
        {
            // Convert to lowercase
            string lowerCaseFullName = fullName.ToLower();

            // Split by spaces
            string[] nameParts = lowerCaseFullName.Split(' ');

            // Initialize the username
            string username = "";

            // Take the first character of each part and add it to the username
            foreach (string part in nameParts)
            {
                if (!string.IsNullOrWhiteSpace(part))
                {
                    username += part[0];
                }
            }
            username = nameParts[0] + username.Substring(1);
            return username;
        }
        [HttpPost]
        private string ConvertEmaileToUsername(string email)
        {
            // Convert to lowercase
            int atIndex = email.IndexOf('@');

            // Trích xuất phần ký tự trước dấu '@'
            string username = email.Substring(0, atIndex);

            return username;
        }
        [HttpPost]
        private async Task<string> GetAccessTokenAsync(string clientId, string clientSecret, string tenantId)
        {
            string tokenEndpoint = $"https://login.microsoftonline.com/{tenantId}/oauth2/v2.0/token";

            using (HttpClient client = new HttpClient())
            {
                var content = new FormUrlEncodedContent(new[]
                {
                new KeyValuePair<string, string>("grant_type", "client_credentials"),
                new KeyValuePair<string, string>("client_id", clientId),
                new KeyValuePair<string, string>("client_secret", clientSecret),
                new KeyValuePair<string, string>("scope", "https://graph.microsoft.com/.default")
            });

                HttpResponseMessage response = await client.PostAsync(tokenEndpoint, content);

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    return Newtonsoft.Json.JsonConvert.DeserializeObject<TokenResponse>(result).access_token;
                }
                else
                {
                    throw new HttpRequestException($"Failed to retrieve access token. Status: {response.StatusCode}");
                }
            }
        }
        [HttpPost]
        private async Task<UserListResponse> CallGraphApiAsync(string endpoint, string accessToken)
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                HttpResponseMessage response = await client.GetAsync(endpoint);

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();

                    return Newtonsoft.Json.JsonConvert.DeserializeObject<UserListResponse>(result);
                }
                else
                {
                    throw new HttpRequestException($"Failed to call Microsoft Graph API. Status: {response.StatusCode}");
                }
            }
        }
        [HttpPost]
        private async Task<byte[]> CallGraphApiBinaryAsync(string endpoint, string accessToken)
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                HttpResponseMessage response = await client.GetAsync(endpoint);

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsByteArrayAsync();

                }
                else
                {
                    return null;
                }
            }
        }
        public class UserListResponse
        {
            [JsonProperty("@odata.context")]
            public string ODataContext { get; set; }
            [JsonProperty("@odata.nextLink")]
            public string ODataNextLink { get; set; }

            public List<UserProfile> value { get; set; }
        }
        class TokenResponse
        {
            public string access_token { get; set; }
            public string token_type { get; set; }
            public int expires_in { get; set; }
        }
        public class UserProfile
        {
            public string[] BusinessPhones { get; set; }
            public string DisplayName { get; set; }
            public string GivenName { get; set; }
            public string JobTitle { get; set; }
            public string Mail { get; set; }
            public string MobilePhone { get; set; }
            public string OfficeLocation { get; set; }
            public string PreferredLanguage { get; set; }
            public string Surname { get; set; }
            public string UserPrincipalName { get; set; }
            public string Id { get; set; }
        }
    }
}
