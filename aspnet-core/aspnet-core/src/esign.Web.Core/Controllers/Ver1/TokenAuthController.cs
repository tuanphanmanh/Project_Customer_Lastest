using Abp;
using Abp.AspNetCore.Mvc.Authorization;
using Abp.AspNetCore.SignalR.Hubs;
using Abp.AspNetZeroCore.Web.Authentication.External;
using Abp.Authorization;
using Abp.Authorization.Users;
using Abp.Configuration;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Extensions;
using Abp.MultiTenancy;
using Abp.Net.Mail;
using Abp.Runtime.Caching;
using Abp.Runtime.Security;
using Abp.Runtime.Session;
using Abp.Runtime.Validation;
using Abp.Timing;
using Abp.UI;
using Abp.Zero.Configuration;
using esign.Authentication.TwoFactor;
using esign.Authentication.TwoFactor.Google;
using esign.Authorization;
using esign.Authorization.Accounts.Dto;
using esign.Authorization.Delegation;
using esign.Authorization.Impersonation;
using esign.Authorization.Roles;
using esign.Authorization.Users;
using esign.Chat;
using esign.Configuration;
using esign.Esign;
using esign.Identity;
using esign.MultiTenancy;
using esign.Net.Sms;
using esign.Notifications;
using esign.Security.Recaptcha;
using esign.Url;
using esign.Web.Authentication.External;
using esign.Web.Authentication.JwtBearer;
using esign.Web.Authentication.TwoFactor;
using esign.Web.Common;
using esign.Web.Models.TokenAuth;
using FP.Radius;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.DirectoryServices;
using System.Drawing;
using System.Drawing.Imaging;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Data.SqlClient;
using Dapper;
using IdentityModel.OidcClient;
using esign.Authorization.Accounts.Dto.Ver1;
using esign.Ver1.Authorization;

namespace esign.Web.Controllers.Ver1
{
    [ApiVersion("1")]
    public class TokenAuthController : esignVersionControllerBase
    {
        private readonly LogInManager _logInManager;
        private readonly ITenantCache _tenantCache;
        private readonly AbpLoginResultTypeHelper _abpLoginResultTypeHelper;
        private readonly TokenAuthConfiguration _configuration;
        private readonly UserManager _userManager;
        private readonly TenantManager _tenantManager;
        private readonly ICacheManager _cacheManager;
        private readonly IOptions<AsyncJwtBearerOptions> _jwtOptions;
        private readonly IExternalAuthConfiguration _externalAuthConfiguration;
        private readonly IExternalAuthManager _externalAuthManager;
        private readonly UserRegistrationManager _userRegistrationManager;
        private readonly IImpersonationManager _impersonationManager;
        private readonly IUserLinkManager _userLinkManager;
        private readonly IAppNotifier _appNotifier;
        private readonly ISmsSender _smsSender;
        private readonly IEmailSender _emailSender;
        private readonly IdentityOptions _identityOptions;
        private readonly GoogleAuthenticatorProvider _googleAuthenticatorProvider;
        private readonly ExternalLoginInfoManagerFactory _externalLoginInfoManagerFactory;
        private readonly ISettingManager _settingManager;
        private readonly IJwtSecurityStampHandler _securityStampHandler;
        private readonly AbpUserClaimsPrincipalFactory<User, Role> _claimsPrincipalFactory;
        public IRecaptchaValidator RecaptchaValidator { get; set; }
        private readonly IUserDelegationManager _userDelegationManager;
        private IAbpSession ChatAbpSession { get; }
        private readonly IChatMessageManager _chatMessageManager;
        public IAppUrlService AppUrlService { get; set; }
        private readonly IRepository<Tenant> _tenantRepository;
        private readonly IRepository<EsignUserDevice, long> _deviceRepository;
        private readonly IPasswordHasher<User> _passwordHasher;
        private string TenantId;
        private string ClientId;
        private string ClientSecret;
        private string RedirectUri;
        private string Scope;

        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IConfigurationRoot _appConfiguration;
        private string _connectionString;
        private readonly IRepository<User,long> _userRepository;
        public TokenAuthController(
            LogInManager logInManager,
            ITenantCache tenantCache,
            AbpLoginResultTypeHelper abpLoginResultTypeHelper,
            TokenAuthConfiguration configuration,
            UserManager userManager,
            TenantManager tenantManager,
            ICacheManager cacheManager,
            IOptions<AsyncJwtBearerOptions> jwtOptions,
            IExternalAuthConfiguration externalAuthConfiguration,
            IExternalAuthManager externalAuthManager,
            UserRegistrationManager userRegistrationManager,
            IImpersonationManager impersonationManager,
            IUserLinkManager userLinkManager,
            IAppNotifier appNotifier,
            ISmsSender smsSender,
            IEmailSender emailSender,
            IOptions<IdentityOptions> identityOptions,
            GoogleAuthenticatorProvider googleAuthenticatorProvider,
            ExternalLoginInfoManagerFactory externalLoginInfoManagerFactory,
            ISettingManager settingManager,
            IJwtSecurityStampHandler securityStampHandler,
            AbpUserClaimsPrincipalFactory<User, Role> claimsPrincipalFactory,
            IUserDelegationManager userDelegationManager,
            IChatMessageManager chatMessageManager,
            IRepository<Tenant> tenantRepository,
            IRepository<EsignUserDevice, long> deviceRepository,
            IPasswordHasher<User> passwordHasher,
            IWebHostEnvironment hostingEnvironment,
            IRepository<User, long> userRepository
            )
        {
            _logInManager = logInManager;
            _tenantCache = tenantCache;
            _abpLoginResultTypeHelper = abpLoginResultTypeHelper;
            _configuration = configuration;
            _userManager = userManager;
            _tenantManager = tenantManager;
            _cacheManager = cacheManager;
            _jwtOptions = jwtOptions;
            _externalAuthConfiguration = externalAuthConfiguration;
            _externalAuthManager = externalAuthManager;
            _userRegistrationManager = userRegistrationManager;
            _impersonationManager = impersonationManager;
            _userLinkManager = userLinkManager;
            _appNotifier = appNotifier;
            _smsSender = smsSender;
            _emailSender = emailSender;
            _googleAuthenticatorProvider = googleAuthenticatorProvider;
            _externalLoginInfoManagerFactory = externalLoginInfoManagerFactory;
            _settingManager = settingManager;
            _securityStampHandler = securityStampHandler;
            _identityOptions = identityOptions.Value;
            _claimsPrincipalFactory = claimsPrincipalFactory;
            RecaptchaValidator = NullRecaptchaValidator.Instance;
            _userDelegationManager = userDelegationManager;
            ChatAbpSession = NullAbpSession.Instance;
            _chatMessageManager = chatMessageManager;
            _tenantRepository = tenantRepository;
            _deviceRepository = deviceRepository;
            _passwordHasher = passwordHasher;
            _hostingEnvironment = hostingEnvironment;
            _appConfiguration = hostingEnvironment.GetAppConfiguration();
            TenantId = _appConfiguration[$"Parameters:TenantId"];
            ClientId = _appConfiguration[$"Parameters:ClientId"];
            ClientSecret = _appConfiguration[$"Parameters:ClientSecret"];
            RedirectUri = _appConfiguration[$"Parameters:RedirectUri"];
            Scope = _appConfiguration[$"Parameters:Scope"];
            _connectionString = _appConfiguration.GetConnectionString(esignConsts.ConnectionStringName);
            _userRepository = userRepository;
        }


        [HttpPost]
        public async Task<AuthenticateResultModel> Login([FromBody] AuthenticateModel model)
        {
            var checkTenant = await _tenantRepository.GetAll().AsNoTracking().FirstOrDefaultAsync(e => e.TenancyName == model.TenancyName);
            //set tenant for session
            if (checkTenant != null)
            {
                AbpSession.Use(checkTenant.Id, null);
            }
          
            var loginResult = await GetLoginResultAsync(
            model.UserName,
            model.Password,
            GetTenancyNameOrNull() );
            var s = _userManager.GetClaimsAsync(loginResult.User);
            if(loginResult.User.IsAD == true) throw new UserFriendlyException(404, L("Please sign in with Tmv account"));
            //Two factor auth
            await _userManager.InitializeOptionsAsync(loginResult.Tenant?.Id);
          
            await _cacheManager
                .GetTwoFactorCodeCache()
                .SetAsync(
                    loginResult.User.ToUserIdentifier().ToString(),
                    new TwoFactorCodeCacheItem()
                );
            SendTwoFactorAuthCodeModel emailModel = new SendTwoFactorAuthCodeModel();
            emailModel.UserId = loginResult.User.Id;
            emailModel.Provider = "Email";
            await SendTwoFactorAuthCode(emailModel);
            string tokenT = HttpUtility.UrlEncode(SimpleStringCipher.Instance.Encrypt("userName=" + model.UserName + "&password=" + HttpUtility.UrlEncode(model.Password) + "&tenancyName=" + model.TenancyName));
            return new AuthenticateResultModel
            {
                ReqVerifyCode = true,
                UserId = loginResult.User.Id,
                TokenTemp = tokenT
            };
        }
        [HttpPost]
        public async Task SendTwoFactorAuthCode([FromBody] SendTwoFactorAuthCodeModel model)
        {
            int? tenantId = 0;
            if (!string.IsNullOrWhiteSpace(model.TenancyName))
            {
                tenantId = _tenantRepository.FirstOrDefault(e => e.TenancyName ==  model.TenancyName)?.Id;
            }
            else
            {
                tenantId = AbpSession.TenantId;
            }
            var cacheKey = new UserIdentifier(tenantId, model.UserId).ToString();

            var cacheItem = await _cacheManager
                .GetTwoFactorCodeCache()
                .GetOrDefaultAsync(cacheKey);

            if (cacheItem == null)
            {
                //There should be a cache item added in Authenticate method! This check is needed to prevent sending unwanted two factor code to users.
                throw new UserFriendlyException(L("SendSecurityCodeErrorMessage"));
            }
            var user = new User();
            using (CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant, AbpDataFilters.MustHaveTenant))
            {
                user = await _userManager.FindByIdAsync(model.UserId.ToString());
            }
            if (model.Provider != GoogleAuthenticatorProvider.Name)
            {
                cacheItem.Code = await _userManager.GenerateTwoFactorTokenAsync(user, model.Provider);
                var message = GetMailContentSendOTP(await _userManager.GetEmailAsync(user), cacheItem.Code).ToString();

                if (model.Provider == "Email")
                {

                    await _emailSender.SendAsync(await _userManager.GetEmailAsync(user), L("EmailSecurityCodeSubject"),
                        message);
                }
                else if (model.Provider == "Phone")
                {
                    await _smsSender.SendAsync(await _userManager.GetPhoneNumberAsync(user), message);
                }
            }

            _cacheManager.GetTwoFactorCodeCache().Set(
                    cacheKey,
                    cacheItem
                );
            _cacheManager.GetCache("ProviderCache").Set(
                "Provider",
                model.Provider
            );
        }

        [HttpPost]
        public async Task<IActionResult> Authentication365()
        {
            var accessToken = "eyJ0eXAiOiJKV1QiLCJub25jZSI6ImxRTExheEYya0JGYVJBUEtWUlgxNUZIOTJsS3JKRkIzYzloYkhXczJtcGMiLCJhbGciOiJSUzI1NiIsIng1dCI6IjlHbW55RlBraGMzaE91UjIybXZTdmduTG83WSIsImtpZCI6IjlHbW55RlBraGMzaE91UjIybXZTdmduTG83WSJ9.eyJhdWQiOiIwMDAwMDAwMy0wMDAwLTAwMDAtYzAwMC0wMDAwMDAwMDAwMDAiLCJpc3MiOiJodHRwczovL3N0cy53aW5kb3dzLm5ldC83ZjVmMThlYi1lNDljLTQyZmMtYTUyZi0yNzMxZDE2NzRhYjEvIiwiaWF0IjoxNjk5MjYxODcwLCJuYmYiOjE2OTkyNjE4NzAsImV4cCI6MTY5OTI2NjIxMywiYWNjdCI6MCwiYWNyIjoiMSIsImFpbyI6IkFWUUFxLzhWQUFBQWxkOWlWODdOSDZBZlRVMFQweTNqS0w5MEJQZHR4SUpsQVMzOUtzN0RkQjd5cEM2UUZ3WnFvK2lWRk9SbzB3L0xqUk80bWJZWVI4WWY2WjRQSGtMNThrcllBN0g3ZTQ1MzFRbi95b3RyaUZBPSIsImFtciI6WyJwd2QiLCJtZmEiXSwiYXBwX2Rpc3BsYXluYW1lIjoiU2VjdXJpdHkgQ2VudGVyIC0gVGVzdCIsImFwcGlkIjoiOTljMDg4NjUtZjkxOC00ZTRhLTk4YzgtMzU4MDRiYTdmMmVmIiwiYXBwaWRhY3IiOiIxIiwiZmFtaWx5X25hbWUiOiJOZ3V5ZW4gVmlldCAtSVQiLCJnaXZlbl9uYW1lIjoiSHVuZyIsImlkdHlwIjoidXNlciIsImlwYWRkciI6IjExOC43MC4yNDAuMTYyIiwibmFtZSI6Ikh1bmcgTmd1eWVuIFZpZXQtSVQiLCJvaWQiOiIzYzg4MmQ2OS03NDQ0LTRmMTItYWE2NS0wN2JkYWZiNTM1YzUiLCJvbnByZW1fc2lkIjoiUy0xLTUtMjEtNDgyOTkyNjQ0LTE1NTg3MDczOTUtMzU3NDY5NjkyMS0zMTMzIiwicGxhdGYiOiIzIiwicHVpZCI6IjEwMDMyMDAwQTlBREUxNTUiLCJyaCI6IjAuQVZZQTZ4aGZmNXprX0VLbEx5Y3gwV2RLc1FNQUFBQUFBQUFBd0FBQUFBQUFBQUJXQUVBLiIsInNjcCI6IlVzZXIuUmVhZCBwcm9maWxlIG9wZW5pZCBlbWFpbCIsInNpZ25pbl9zdGF0ZSI6WyJpbmtub3dubnR3ayIsImttc2kiXSwic3ViIjoia2FNNXNvbGVKcDVHc3N1RzI3ellXWjdtSl9mMmw4d2xoM2xVekRUQlgyNCIsInRlbmFudF9yZWdpb25fc2NvcGUiOiJBUyIsInRpZCI6IjdmNWYxOGViLWU0OWMtNDJmYy1hNTJmLTI3MzFkMTY3NGFiMSIsInVuaXF1ZV9uYW1lIjoic3lodW5nbnZAdG95b3Rhdm4uY29tLnZuIiwidXBuIjoic3lodW5nbnZAdG95b3Rhdm4uY29tLnZuIiwidXRpIjoiZEhJYWYtOTFkRVdOZEVYbzlKMHNBUSIsInZlciI6IjEuMCIsIndpZHMiOlsiYjc5ZmJmNGQtM2VmOS00Njg5LTgxNDMtNzZiMTk0ZTg1NTA5Il0sInhtc19zdCI6eyJzdWIiOiJzdUtreHViblU4S3V5S1NQcklsQlhickk2WGREMi05WExLOEtHLUsycWgwIn0sInhtc190Y2R0IjoxNDgyOTIwNjM4fQ.Z0lgwqd6upKv17pu--3z-U3dbgYZG9HWHT26oGB351eBmbEPdsfzPnekcvTzyljpIbW0Eq83z7agGfaIkCNtfnVKmKaG3vV43rfNMp-t8oP7A5vwMtKv5wBt6R1lylc4PEq47ZB5nNqBnu1XPFpkExrEtT9VU2fnyXcKa_iyxEArox-tyzl48NESST41twli4HXD8u2glbqRg0S25n_82Htep1eF5r88gAt1RZCAxFocnmi-bKVdD9phpBz9ho5OoQeCrGZp_eHT0A_z8s0gzdH08txtuiKQ-qD4bSVDbvW-wi4PudVbgb_h6JuubCUK7MKd9NekX4ZKnxM-20e0sA";
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                    // Set the "Accept" header to specify the desired content type
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("image/jpeg"));

                    HttpResponseMessage response = await client.GetAsync("https://graph.microsoft.com/v1.0/me/photo/$value");

                    if (response.IsSuccessStatusCode)
                    {
                        byte[] photoBytes = await response.Content.ReadAsByteArrayAsync();
                        // Do something with the photo bytes, such as displaying or saving it.
                        // For example, you can return it as a file from your action.
                        return File(photoBytes, "image/jpeg");
                    }
                    else
                    {
                        // Handle non-successful responses here
                        return BadRequest("Error while fetching user photo.");
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the exception for debugging purposesyy
                return BadRequest("Error: " + ex.Message);
            }
        }




        private async Task<string> EncryptQueryParameters(long userId, string passwordResetCode)
        {
            var expirationHours = await _settingManager.GetSettingValueAsync<int>(
                AppSettings.UserManagement.Password.PasswordResetCodeExpirationHours
            );

            var expireDate = Uri.EscapeDataString(Clock.Now.AddHours(expirationHours)
                .ToString(esignConsts.DateTimeOffsetFormat));

            var query = $"userId={userId}&resetCode={passwordResetCode}&expireDate={expireDate}";

            return SimpleStringCipher.Instance.Encrypt(query);
        }


        [HttpPost]
        public async Task<AuthenticateResultModel> Login365([FromBody] AuthenticateModel model)
        {
            //declare marker flag 
            string dataState = "";
            string title = "";
            string givenName = "";
            string surname = "";
            bool reqVerifyCode = false;

            //get tenancy input
            var checkTenant = await _tenantRepository.GetAll().AsNoTracking().FirstOrDefaultAsync(e => e.TenancyName == model.TenancyName);
            //set tenant for session
            if (checkTenant != null)
            {
                AbpSession.Use(checkTenant.Id, null);
            }
            //default admin can login with out tenancy name
            else if (model.UserName.ToLower() != "admin")
            {
                throw new UserFriendlyException("TenancyName is invalid!");
            }
            //check user exist
            var checkUser = new User();
            TokenV1ResponseDto tokenResponse = await ExchangeCodeForToken(model.AccessCode);
            using (CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant, AbpDataFilters.MustHaveTenant))
            {
                checkUser = await _userManager.Users.Where(e => e.EmailAddress == tokenResponse.Email && e.TenantId == (checkTenant == null ? null : checkTenant.Id)).FirstOrDefaultAsync();
            }
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenResponse.AccessToken);
                // Make requests to Microsoft Graph API using this HttpClient
                HttpResponseMessage response = await client.GetAsync($"https://graph.microsoft.com/v1.0/me");
                if (response.IsSuccessStatusCode)
                {
                    string responseContent = await response.Content.ReadAsStringAsync();
                    dynamic data = JObject.Parse(responseContent);
                     title = data.jobTitle;
                     givenName = data.givenName;
                     surname = data.surname;
                }
            }
            var randomPassword = await _userManager.CreateRandomPassword();
            string username = ConvertEmaileToUsername(tokenResponse.Email);
            string oldImageUrl = checkUser == null ? null : checkUser.ImageUrl;

            if (checkUser == null)
            {
                var user = await _userRegistrationManager.RegisterAsync(
                        tokenResponse.Name,
                        surname,
                        tokenResponse.Email,
                        username,
                        randomPassword,
                        true,
                        AppUrlService.CreateEmailActivationUrlFormat(AbpSession.TenantId),
                        true
                    );
                user.IsAD = true;
                user.GivenName = givenName;
                user.ImageUrl = await CreateDefaultAvatar(tokenResponse.Name, oldImageUrl, tokenResponse.AccessToken);
                using (CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant, AbpDataFilters.MustHaveTenant))
                {
                    await _userRepository.UpdateAsync(user);
                }
            }
            else
            {
                checkUser.IsAD = true;
                checkUser.ImageUrl = await CreateDefaultAvatar(tokenResponse.Name, oldImageUrl, tokenResponse.AccessToken);
                checkUser.Password = _passwordHasher.HashPassword(checkUser, randomPassword);
                using (CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant, AbpDataFilters.MustHaveTenant))
                {
                    await _userRepository.UpdateAsync(checkUser);
                }

            }


            var loginResult = await GetLoginResultAsync(
                checkUser != null ? checkUser.UserName : username,
                randomPassword,
                model.TenancyName
                );

            await _userManager.InitializeOptionsAsync(loginResult.Tenant?.Id);

            var refreshToken = reqVerifyCode ? new("", "") : CreateRefreshToken(
                await CreateJwtClaims(
                    loginResult.Identity,
                    loginResult.User,
                    tokenType: TokenType.RefreshToken
                    )
            );

            var accessToken1 = reqVerifyCode ? null : CreateAccessToken(
                await CreateJwtClaims(
                    loginResult.Identity,
                    loginResult.User,
                    refreshTokenKey: refreshToken.key
                )
            );

            return new AuthenticateResultModel
            {
                AccessToken = accessToken1,
                ExpireInSeconds = (int)_configuration.AccessTokenExpiration.TotalSeconds,
                RefreshToken = refreshToken.token,
                RefreshTokenExpireInSeconds = (int)_configuration.RefreshTokenExpiration.TotalSeconds,
                EncryptedAccessToken = GetEncryptedAccessToken(accessToken1),
                UserId = loginResult.User.Id,
                ReqVerifyCode = reqVerifyCode,
                DataState = dataState
            };

        }

        [HttpPost]
        public async Task<string> CreateDefaultAvatarForUser(string name, string email)
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
        public async Task<string> CreateDefaultAvatar(string name, string oldImageUrl, string accessToken)
        {
            // Ensure you have a valid access token for authentication.
            try
            {
                // Get user's profile photo from Microsoft Graph
                var graphApiUrl = "https://graph.microsoft.com/v1.0/me/photo/$value";

                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                    HttpResponseMessage response = await client.GetAsync(graphApiUrl);


                    if (response.IsSuccessStatusCode)
                    {
                        // Read the photo as bytes
                        byte[] photoBytes = await response.Content.ReadAsByteArrayAsync();

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
            }
            catch (Exception ex)
            {
                return "";
            }

        }
        [HttpPost]
        public string ConvertFullNameToUsername(string fullName)
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
        public string ConvertShortnameToCreateDefaultAvatar(string fullName)
        {
            // Convert to lowercase
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
        public class TokenV1ResponseDto
        {
            public string PreferredUsername { get; set; }
            public string Name { get; set; }
            public string Email { get; set; }
            public string AccessToken { get; set; }
            // Add other properties as needed
        }


        [HttpPost]
        public async Task<TokenV1ResponseDto> ExchangeCodeForToken(string accessCode)
        {
            // Construct the token request URL
            string tokenRequestUrl = $"https://login.microsoftonline.com/{TenantId}/oauth2/v2.0/token";

            // Create the request body
            // Create the request body as a dictionary
            var requestBody = new Dictionary<string, string>
            {
                { "grant_type", "authorization_code" },
                { "client_id", ClientId },
                { "code", accessCode },
                { "redirect_uri", RedirectUri },
                { "scope", Scope },
                { "client_secret", ClientSecret }
            };

            // Create an HttpClient instance
            using (HttpClient client = new HttpClient())
            {
                // Send the POST request with form URL-encoded content
                HttpResponseMessage response = await client.PostAsync(tokenRequestUrl, new FormUrlEncodedContent(requestBody));

                if (response.IsSuccessStatusCode)
                {
                    string responseContent = await response.Content.ReadAsStringAsync();
                    dynamic data = JObject.Parse(responseContent);
                    string idToken = data.id_token;
                    string accessToken = data.access_token;

                    // Decode the idToken to get the preferred_username
                    var preferredUsername = DecodeAndRetrievePreferredUsername(idToken);
                    preferredUsername.AccessToken = accessToken;
                    // Create a TokenResponseDto to return the token and preferred_username


                    return preferredUsername;
                }
                else
                {
                    // Handle the error and return an error response
                    // You can also return a specific error DTO if needed
                    return null;
                }

            }

        }
        [HttpPost]
        public TokenV1ResponseDto DecodeAndRetrievePreferredUsername(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.ReadJwtToken(token);

            Claim preferredUsernameClaim = jwtToken.Claims.FirstOrDefault(claim => claim.Type == "preferred_username");
            Claim email = jwtToken.Claims.FirstOrDefault(claim => claim.Type == "email");
            Claim name = jwtToken.Claims.FirstOrDefault(claim => claim.Type == "name");
            if (preferredUsernameClaim != null)
            {
                // Return a TokenResponseDto with the preferred_username
                return new TokenV1ResponseDto
                {
                    PreferredUsername = preferredUsernameClaim.Value,
                    Email = email.Value,
                    Name = name.Value
                };
            }

            // If 'preferred_username' claim is not found, return an empty TokenResponseDto
            return new TokenV1ResponseDto();
        }




        #region Login Biometric
        [HttpPost]
        public async Task<AuthenticateResultModel> LoginBiometric([FromBody] AuthenticateBiometricModel model)
        {
            var checkTenant = await _tenantRepository.GetAll().AsNoTracking().FirstOrDefaultAsync(e => e.TenancyName == model.TenancyName);
            //set tenant for session
            if (checkTenant != null)
            {
                AbpSession.Use(checkTenant.Id, null);
            }
            var user = new User();

            using (CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant, AbpDataFilters.MustHaveTenant))
            {
                // get user by username
                user = await _userManager.Users.Where(e => e.UserName == model.UserName && e.TenantId == (checkTenant == null ? null : checkTenant.Id)).FirstOrDefaultAsync();
            }
            if (user != null)
            {
                //check device exist
                var checkDevice = await _deviceRepository.FirstOrDefaultAsync(e => e.DeviceCode == model.DeviceCode && e.UserId == user.Id);

                if (checkDevice != null)
                {
                    checkDevice.LoginCount = checkDevice.LoginCount + 1;
                    await _deviceRepository.UpdateAsync(checkDevice);
                    var loginResult = await GetLoginResultAsync(model.UserName, model.Password, model.TenancyName);

                    await _userManager.InitializeOptionsAsync(loginResult.Tenant?.Id);

                    var refreshToken = CreateRefreshToken(
                        await CreateJwtClaims(
                            loginResult.Identity,
                            loginResult.User,
                            tokenType: TokenType.RefreshToken
                            )
                    );

                    var accessToken1 = CreateAccessToken(
                        await CreateJwtClaims(
                            loginResult.Identity,
                            loginResult.User,
                            refreshTokenKey: refreshToken.key
                        )
                    );

                    return new AuthenticateResultModel
                    {
                        AccessToken = accessToken1,
                        ExpireInSeconds = (int)_configuration.AccessTokenExpiration.TotalSeconds,
                        RefreshToken = refreshToken.token,
                        RefreshTokenExpireInSeconds = (int)_configuration.RefreshTokenExpiration.TotalSeconds,
                        EncryptedAccessToken = GetEncryptedAccessToken(accessToken1),
                        UserId = loginResult.User.Id,
                    };
                }
                else
                {
                    throw new UserFriendlyException(404, L("InvalidDevice"));
                }
            }
            else
            {
                throw new UserFriendlyException(404, L("InvalidUserNameOrEmailAddress"));
            }
        }
        #endregion Login Biometric

        #region Save trust Device 
        [AbpAuthorize]
        [HttpPost]
        public async Task<SaveUserDeviceOutput> SaveUserDevice([FromBody]  SaveUserDeviceInput input)
        {
            //check user device 
            var device = _deviceRepository.FirstOrDefault(e => e.UserId == AbpSession.UserId && e.DeviceCode == input.DeviceCode);
            if (device != null)
            {
                device.LoginCount = device.LoginCount + 1;
                await _deviceRepository.UpdateAsync(device);
            }
            else
            {
                await _deviceRepository.InsertAsync(new EsignUserDevice { DeviceCode = input.DeviceCode, UserId = (long)AbpSession.UserId });
            }
            return new SaveUserDeviceOutput { IsSaved = true};
        }
        #endregion


        [HttpPost]
        public async Task<AuthenticateResultModel> VerifyOTP([FromBody] VerifyOTPInput input)
        {
            AuthenticateModel model = new AuthenticateModel();
            if (!string.IsNullOrEmpty(input.TokenTemp))
            {
                try
                {
                    string decodedParam = HttpUtility.UrlDecode(input.TokenTemp);
                    var parameters = SimpleStringCipher.Instance.Decrypt(decodedParam);
                    var query = HttpUtility.ParseQueryString(parameters);

                    if (query["userName"] != null)
                    {
                        model.UserName = query["userName"];
                    }
                    else
                    {
                        throw new AbpValidationException();
                    }

                    if (query["password"] != null)
                    {
                        model.Password = HttpUtility.UrlDecode(query["password"]);
                    }
                    else
                    {
                        throw new AbpValidationException();
                    }

                    if (query["tenancyName"] != null)
                    {
                        model.TenancyName = query["tenancyName"];
                    }
                    else
                    {
                        throw new AbpValidationException();
                    }
                }
                catch (Exception e)
                {
                    throw new AbpValidationException("Invalid token!");
                }
            }
            try
            {

                var loginResult = await GetLoginResultAsync(
                    model.UserName,
                    model.Password,
                    model.TenancyName
                );
                model.TwoFactorVerificationCode = input.InputCode;
                string twoFactorRememberClientToken = null;
                if (model.UserName.ToUpper() != "T_ESIGN_REVIEW")
                {
                    twoFactorRememberClientToken = await TwoFactorAuthenticateAsync(loginResult, model);
                }    
                
                // One Concurrent Login 
                if (AllowOneConcurrentLoginPerUser())
                {
                    await ResetSecurityStampForLoginResult(loginResult);
                }

                var refreshToken = CreateRefreshToken(
                await CreateJwtClaims(
                loginResult.Identity,
                        loginResult.User,
                        tokenType: TokenType.RefreshToken
                    )
                );

                var accessToken = CreateAccessToken(
                    await CreateJwtClaims(
                        loginResult.Identity,
                        loginResult.User,
                        refreshTokenKey: refreshToken.key
                    )
                );

                return new AuthenticateResultModel
                {
                    AccessToken = accessToken,
                    ExpireInSeconds = (int)_configuration.AccessTokenExpiration.TotalSeconds,
                    RefreshToken = refreshToken.token,
                    RefreshTokenExpireInSeconds = (int)_configuration.RefreshTokenExpiration.TotalSeconds,
                    EncryptedAccessToken = GetEncryptedAccessToken(accessToken),
                    UserId = loginResult.User.Id,
                };

            }
            catch (UserFriendlyException e)
            {
                throw e;
            }
        }

        [HttpPost]
        public async Task<TwoFactModel> ResendOTP([FromBody] ResendOtpInput model)
        {
            TwoFactModel output = new TwoFactModel();

            if (!string.IsNullOrEmpty(model.TokenTemp))
            {
                try
                {
                    string decodedParam = System.Web.HttpUtility.UrlDecode(model.TokenTemp);
                    var parameters = SimpleStringCipher.Instance.Decrypt(decodedParam);
                    var query = HttpUtility.ParseQueryString(parameters);

                    if (query["userName"] != null)
                    {
                        output.UserName = query["userName"];
                    }
                    else
                    {
                        throw new AbpValidationException();
                    }

                    if (query["password"] != null)
                    {
                        output.Password = query["password"];
                    }
                    else
                    {
                        throw new AbpValidationException();
                    }
                    if (query["tenancyName"] != null)
                    {
                        output.TenancyName = query["tenancyName"];
                    }
                    else
                    {
                        throw new AbpValidationException();
                    }
                }
                catch (Exception e)
                {
                    throw new AbpValidationException("Invalid token!");
                }
            }
            var loginResult = await GetLoginResultAsync(
                            output.UserName,
                            output.Password,
                            output.TenancyName);
            if (loginResult.User.IsAD == true) throw new UserFriendlyException(404, L("Please sign in with Tmv account"));
            //Two factor auth
            var checkTenant = await _tenantRepository.GetAll().AsNoTracking().FirstOrDefaultAsync(e => e.TenancyName == output.TenancyName);
            //set tenant for session
            if (checkTenant != null)
            {
                AbpSession.Use(checkTenant.Id, null);
            }
            await _userManager.InitializeOptionsAsync(loginResult.Tenant?.Id);
            await _cacheManager
                .GetTwoFactorCodeCache()
                .SetAsync(
                    loginResult.User.ToUserIdentifier().ToString(),
                    new TwoFactorCodeCacheItem()
                );
            SendTwoFactorAuthCodeModel emailModel = new SendTwoFactorAuthCodeModel();
            emailModel.UserId = loginResult.User.Id;
            emailModel.Provider = "Email";
            await SendTwoFactorAuthCode(emailModel);
            return output;
        }


        [HttpPost]
        public async Task<RefreshTokenResult> RefreshToken([FromBody] RefreshTokenInput input)
        {
            if (string.IsNullOrWhiteSpace(input.RefreshToken))
            {
                throw new ArgumentNullException(nameof(input.RefreshToken));
            }

            var (isRefreshTokenValid, principal) = await IsRefreshTokenValid(input.RefreshToken);
            if (!isRefreshTokenValid)
            {
                throw new ValidationException("Refresh token is not valid!");
            }

            try
            {
                var user = await _userManager.GetUserAsync(
                    UserIdentifier.Parse(principal.Claims.First(x => x.Type == AppConsts.UserIdentifier).Value)
                );

                if (user == null)
                {
                    throw new UserFriendlyException("Unknown user or user identifier");
                }

                if (AllowOneConcurrentLoginPerUser())
                {
                    await _userManager.UpdateSecurityStampAsync(user);
                    await _securityStampHandler.SetSecurityStampCacheItem(user.TenantId, user.Id, user.SecurityStamp);
                }

                principal = await _claimsPrincipalFactory.CreateAsync(user);

                var accessToken = CreateAccessToken(
                    await CreateJwtClaims(principal.Identity as ClaimsIdentity, user)
                );

                return await Task.FromResult(new RefreshTokenResult(
                    accessToken,
                    GetEncryptedAccessToken(accessToken),
                    (int)_configuration.AccessTokenExpiration.TotalSeconds)
                );
            }
            catch (UserFriendlyException)
            {
                throw;
            }
            catch (Exception e)
            {
                throw new ValidationException("Refresh token is not valid!", e);
            }
        }

        private bool UseCaptchaOnLogin()
        {
            return SettingManager.GetSettingValue<bool>(AppSettings.UserManagement.UseCaptchaOnLogin);
        }


        [HttpGet]
        [AbpAuthorize]
        public async Task LogOut()
        {
            if (AbpSession.UserId != null)
            {
                var tokenValidityKeyInClaims = User.Claims.First(c => c.Type == AppConsts.TokenValidityKey);
                await RemoveTokenAsync(tokenValidityKeyInClaims.Value);

                var refreshTokenValidityKeyInClaims =
                    User.Claims.FirstOrDefault(c => c.Type == AppConsts.RefreshTokenValidityKey);
                if (refreshTokenValidityKeyInClaims != null)
                {
                    await RemoveTokenAsync(refreshTokenValidityKeyInClaims.Value);
                }

                if (AllowOneConcurrentLoginPerUser())
                {
                    await _securityStampHandler.RemoveSecurityStampCacheItem(
                        AbpSession.TenantId,
                        AbpSession.GetUserId()
                    );
                }
            }
        }
        [HttpPost]
        public async Task RemoveTokenByUserName(string userName)
        {
            using (var cnn = new SqlConnection(_connectionString))
            {
                IEnumerable<GetTokenDto> listToken = await cnn.QueryAsync<GetTokenDto>(@"select t.Name TokenName, t.TenantId, u.Id UserId from AbpUserTokens t
                                                           join AbpUsers u on t.UserId = u.Id
                                                           where u.UserName = @userName", new { userName = userName });
                foreach (var token in listToken)
                {
                    var userIdentifier = new UserIdentifier(token.TenantId, token.UserId);
                    await _userManager.RemoveTokenValidityKeyAsync(_userManager.GetUser(userIdentifier), token.TokenName);
                    await _cacheManager.GetCache(AppConsts.TokenValidityKey).RemoveAsync(token.TokenName);

                    if (AllowOneConcurrentLoginPerUser())
                    {
                        await _securityStampHandler.RemoveSecurityStampCacheItem(token.TenantId, token.UserId);
                    }
                }
            } 

        }

        public class GetTokenDto
        {
            public string TokenName { get; set; }
            public int? TenantId { get; set; }
            public long UserId { get; set; }
        }
        private async Task RemoveTokenAsync(string tokenKey)
        {
            await _userManager.RemoveTokenValidityKeyAsync(
                await _userManager.GetUserAsync(AbpSession.ToUserIdentifier()), tokenKey
            );

            await _cacheManager.GetCache(AppConsts.TokenValidityKey).RemoveAsync(tokenKey);
        }

        [HttpPost]
        public async Task<ImpersonatedAuthenticateResultModel> ImpersonatedAuthenticate(string impersonationToken)
        {
            var result = await _impersonationManager.GetImpersonatedUserAndIdentity(impersonationToken);
            var accessToken = CreateAccessToken(await CreateJwtClaims(result.Identity, result.User));

            return new ImpersonatedAuthenticateResultModel
            {
                AccessToken = accessToken,
                EncryptedAccessToken = GetEncryptedAccessToken(accessToken),
                ExpireInSeconds = (int)_configuration.AccessTokenExpiration.TotalSeconds
            };
        }

        //[HttpPost]
        //public async Task<ImpersonatedAuthenticateResultModel> DelegatedImpersonatedAuthenticate(long userDelegationId,
        //    string impersonationToken)
        //{
        //    var result = await _impersonationManager.GetImpersonatedUserAndIdentity(impersonationToken);
        //    var userDelegation = await _userDelegationManager.GetAsync(userDelegationId);

        //    if (!userDelegation.IsCreatedByUser(result.User.Id))
        //    {
        //        throw new UserFriendlyException("User delegation error...");
        //    }

        //    var expiration = userDelegation.EndTime.Subtract(Clock.Now);
        //    var accessToken = CreateAccessToken(await CreateJwtClaims(result.Identity, result.User, expiration),
        //        expiration);

        //    return new ImpersonatedAuthenticateResultModel
        //    {
        //        AccessToken = accessToken,
        //        EncryptedAccessToken = GetEncryptedAccessToken(accessToken),
        //        ExpireInSeconds = (int) expiration.TotalSeconds
        //    };
        //}

        [HttpPost]
        public async Task<SwitchedAccountAuthenticateResultModel> LinkedAccountAuthenticate(string switchAccountToken)
        {
            var result = await _userLinkManager.GetSwitchedUserAndIdentity(switchAccountToken);
            var accessToken = CreateAccessToken(await CreateJwtClaims(result.Identity, result.User));

            return new SwitchedAccountAuthenticateResultModel
            {
                AccessToken = accessToken,
                EncryptedAccessToken = GetEncryptedAccessToken(accessToken),
                ExpireInSeconds = (int)_configuration.AccessTokenExpiration.TotalSeconds
            };
        }

        //[HttpGet]
        //public List<ExternalLoginProviderInfoModel> GetExternalAuthenticationProviders()
        //{
        //    var allProviders = _externalAuthConfiguration.ExternalLoginInfoProviders
        //        .Select(infoProvider => infoProvider.GetExternalLoginInfo())
        //        .Where(IsSchemeEnabledOnTenant)
        //        .ToList();
        //    return ObjectMapper.Map<List<ExternalLoginProviderInfoModel>>(allProviders);
        //}

        private bool IsSchemeEnabledOnTenant(ExternalLoginProviderInfo scheme)
        {
            if (!AbpSession.TenantId.HasValue)
            {
                return true;
            }

            switch (scheme.Name)
            {
                case "OpenIdConnect":
                    return !_settingManager.GetSettingValueForTenant<bool>(
                        AppSettings.ExternalLoginProvider.Tenant.OpenIdConnect_IsDeactivated, AbpSession.GetTenantId());
                case "Microsoft":
                    return !_settingManager.GetSettingValueForTenant<bool>(
                        AppSettings.ExternalLoginProvider.Tenant.Microsoft_IsDeactivated, AbpSession.GetTenantId());
                case "Google":
                    return !_settingManager.GetSettingValueForTenant<bool>(
                        AppSettings.ExternalLoginProvider.Tenant.Google_IsDeactivated, AbpSession.GetTenantId());
                case "Twitter":
                    return !_settingManager.GetSettingValueForTenant<bool>(
                        AppSettings.ExternalLoginProvider.Tenant.Twitter_IsDeactivated, AbpSession.GetTenantId());
                case "Facebook":
                    return !_settingManager.GetSettingValueForTenant<bool>(
                        AppSettings.ExternalLoginProvider.Tenant.Facebook_IsDeactivated, AbpSession.GetTenantId());
                case "WsFederation":
                    return !_settingManager.GetSettingValueForTenant<bool>(
                        AppSettings.ExternalLoginProvider.Tenant.WsFederation_IsDeactivated, AbpSession.GetTenantId());
                default: return true;
            }
        }

        //[HttpPost]
        //public async Task<ExternalAuthenticateResultModel> ExternalAuthenticate(
        //    [FromBody] ExternalAuthenticateModel model)
        //{
        //    var externalUser = await GetExternalUserInfo(model);

        //    var loginResult = await _logInManager.LoginAsync(
        //        new UserLoginInfo(model.AuthProvider, externalUser.ProviderKey, model.AuthProvider),
        //        GetTenancyNameOrNull()
        //    );

        //    switch (loginResult.Result)
        //    {
        //        case AbpLoginResultType.Success:
        //        {
        //            if (AllowOneConcurrentLoginPerUser())
        //            {
        //                await ResetSecurityStampForLoginResult(loginResult);
        //            }

        //            var refreshToken = CreateRefreshToken(
        //                await CreateJwtClaims(
        //                    loginResult.Identity,
        //                    loginResult.User,
        //                    tokenType: TokenType.RefreshToken
        //                )
        //            );

        //            var accessToken = CreateAccessToken(
        //                await CreateJwtClaims(
        //                    loginResult.Identity,
        //                    loginResult.User,
        //                    refreshTokenKey: refreshToken.key
        //                )
        //            );

        //            var returnUrl = model.ReturnUrl;

        //            if (model.SingleSignIn.HasValue && model.SingleSignIn.Value &&
        //                loginResult.Result == AbpLoginResultType.Success)
        //            {
        //                loginResult.User.SetSignInToken();
        //                returnUrl = AddSingleSignInParametersToReturnUrl(
        //                    model.ReturnUrl,
        //                    loginResult.User.SignInToken,
        //                    loginResult.User.Id,
        //                    loginResult.User.TenantId
        //                );
        //            }

        //            return new ExternalAuthenticateResultModel
        //            {
        //                AccessToken = accessToken,
        //                EncryptedAccessToken = GetEncryptedAccessToken(accessToken),
        //                ExpireInSeconds = (int) _configuration.AccessTokenExpiration.TotalSeconds,
        //                ReturnUrl = returnUrl,
        //                RefreshToken = refreshToken.token,
        //                RefreshTokenExpireInSeconds = (int) _configuration.RefreshTokenExpiration.TotalSeconds
        //            };
        //        }
        //        default:
        //        {
        //            throw _abpLoginResultTypeHelper.CreateExceptionForFailedLoginAttempt(
        //                loginResult.Result,
        //                externalUser.EmailAddress,
        //                GetTenancyNameOrNull()
        //            );
        //        }
        //    }
        //}

        private async Task ResetSecurityStampForLoginResult(AbpLoginResult<Tenant, User> loginResult)
        {
            await _userManager.UpdateSecurityStampAsync(loginResult.User);
            await _securityStampHandler.SetSecurityStampCacheItem(loginResult.User.TenantId, loginResult.User.Id,
                loginResult.User.SecurityStamp);
            loginResult.Identity.ReplaceClaim(new Claim(AppConsts.SecurityStampKey, loginResult.User.SecurityStamp));
        }

        //#region Etc

        //[AbpMvcAuthorize]
        //[HttpGet]
        //public async Task<ActionResult> TestNotification(string message = "", string severity = "info")
        //{
        //    if (message.IsNullOrEmpty())
        //    {
        //        message = "This is a test notification, created at " + Clock.Now;
        //    }

        //    await _appNotifier.SendMessageAsync(
        //        AbpSession.ToUserIdentifier(),
        //        message,
        //        severity.ToPascalCase().ToEnum<NotificationSeverity>()
        //    );
        //    return Content("Sent notification: " + message);
        //}

        //#endregion

        //private async Task<User> RegisterExternalUserAsync(ExternalAuthUserInfo externalLoginInfo)
        //{
        //    string username;

        //    using (var providerManager =
        //           _externalLoginInfoManagerFactory.GetExternalLoginInfoManager(externalLoginInfo.Provider))
        //    {
        //        username = providerManager.Object.GetUserNameFromExternalAuthUserInfo(externalLoginInfo);
        //    }

        //    var user = await _userRegistrationManager.RegisterAsync(
        //        externalLoginInfo.Name,
        //        externalLoginInfo.Surname,
        //        externalLoginInfo.EmailAddress,
        //        username,
        //        await _userManager.CreateRandomPassword(),
        //        true,
        //        null
        //    );

        //    user.Logins = new List<UserLogin>
        //    {
        //        new UserLogin
        //        {
        //            LoginProvider = externalLoginInfo.Provider,
        //            ProviderKey = externalLoginInfo.ProviderKey,
        //            TenantId = user.TenantId
        //        }
        //    };

        //    await CurrentUnitOfWork.SaveChangesAsync();

        //    return user;
        //}

        private async Task<ExternalAuthUserInfo> GetExternalUserInfo(ExternalAuthenticateModel model)
        {
            var userInfo = await _externalAuthManager.GetUserInfo(model.AuthProvider, model.ProviderAccessCode);
            if (!ProviderKeysAreEqual(model, userInfo))
            {
                throw new UserFriendlyException(L("CouldNotValidateExternalUser"));
            }

            return userInfo;
        }

        private bool ProviderKeysAreEqual(ExternalAuthenticateModel model, ExternalAuthUserInfo userInfo)
        {
            if (userInfo.ProviderKey == model.ProviderKey)
            {
                return true;
            }

            ;

            return userInfo.ProviderKey == model.ProviderKey.Replace("-", "").TrimStart('0');
        }

        private async Task<bool> IsTwoFactorAuthRequiredAsync(AbpLoginResult<Tenant, User> loginResult,
            AuthenticateModel authenticateModel)
        {
            if (!await SettingManager.GetSettingValueAsync<bool>(AbpZeroSettingNames.UserManagement.TwoFactorLogin
                    .IsEnabled))
            {
                return false;
            }

            if (!loginResult.User.IsTwoFactorEnabled)
            {
                return false;
            }

            if ((await _userManager.GetValidTwoFactorProvidersAsync(loginResult.User)).Count <= 0)
            {
                return false;
            }

            //if (await TwoFactorClientRememberedAsync(loginResult.User.ToUserIdentifier(), authenticateModel))
            //{
            //    return false;
            //}

            return true;
        }

        //private async Task<bool> TwoFactorClientRememberedAsync(UserIdentifier userIdentifier,
        //    AuthenticateModel authenticateModel)
        //{
        //    if (!await SettingManager.GetSettingValueAsync<bool>(
        //            AbpZeroSettingNames.UserManagement.TwoFactorLogin.IsRememberBrowserEnabled)
        //       )
        //    {
        //        return false;
        //    }

        //    if (string.IsNullOrWhiteSpace(authenticateModel.TwoFactorRememberClientToken))
        //    {
        //        return false;
        //    }

        //    try
        //    {
        //        var validationParameters = new TokenValidationParameters
        //        {
        //            ValidAudience = _configuration.Audience,
        //            ValidIssuer = _configuration.Issuer,
        //            IssuerSigningKey = _configuration.SecurityKey
        //        };

        //        foreach (var validator in _jwtOptions.Value.AsyncSecurityTokenValidators)
        //        {
        //            if (validator.CanReadToken(authenticateModel.TwoFactorRememberClientToken))
        //            {
        //                try
        //                {
        //                    var (principal, _) = await validator.ValidateToken(
        //                        authenticateModel.TwoFactorRememberClientToken,
        //                        validationParameters
        //                    );

        //                    var userIdentifierClaim = principal.FindFirst(c => c.Type == AppConsts.UserIdentifier);
        //                    if (userIdentifierClaim == null)
        //                    {
        //                        return false;
        //                    }

        //                    return userIdentifierClaim.Value == userIdentifier.ToString();
        //                }
        //                catch (Exception ex)
        //                {
        //                    Logger.Debug(ex.ToString(), ex);
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.Debug(ex.ToString(), ex);
        //    }

        //    return false;
        //}

        /* Checkes two factor code and returns a token to remember the client (browser) if needed */
        private async Task<string> TwoFactorAuthenticateAsync(AbpLoginResult<Tenant, User> loginResult,
            AuthenticateModel authenticateModel)
        {
            var twoFactorCodeCache = _cacheManager.GetTwoFactorCodeCache();
            var userIdentifier = loginResult.User.ToUserIdentifier().ToString();
            var cachedCode = await twoFactorCodeCache.GetOrDefaultAsync(userIdentifier);
            var provider = _cacheManager.GetCache("ProviderCache").Get("Provider", cache => cache).ToString();

            if (provider == GoogleAuthenticatorProvider.Name)
            {
                if (!await _googleAuthenticatorProvider.ValidateAsync("TwoFactor",
                        authenticateModel.TwoFactorVerificationCode, _userManager, loginResult.User))
                {
                    throw new UserFriendlyException(L("InvalidSecurityCode"));
                }
            }
            else if (cachedCode?.Code == null || cachedCode.Code != authenticateModel.TwoFactorVerificationCode)
            {
                throw new UserFriendlyException(L("InvalidSecurityCode"));
            }

            //Delete from the cache since it was a single usage code
            await twoFactorCodeCache.RemoveAsync(userIdentifier);

            //if (authenticateModel.RememberClient)
            //{
            //    if (await SettingManager.GetSettingValueAsync<bool>(AbpZeroSettingNames.UserManagement.TwoFactorLogin
            //            .IsRememberBrowserEnabled))
            //    {
            //        return CreateAccessToken(
            //            await CreateJwtClaims(
            //                loginResult.Identity,
            //                loginResult.User
            //            )
            //        );
            //    }
            //}

            return null;
        }

        private string GetTenancyNameOrNull()
        {
            if (!AbpSession.TenantId.HasValue)
            {
                return null;
            }

            return _tenantCache.GetOrNull(AbpSession.TenantId.Value)?.TenancyName;
        }

        private async Task<AbpLoginResult<Tenant, User>> GetLoginResultAsync(string usernameOrEmailAddress,
            string password, string tenancyName)
        {
            var shouldLockout = await SettingManager.GetSettingValueAsync<bool>(
                AbpZeroSettingNames.UserManagement.UserLockOut.IsEnabled
            );
            var loginResult = await _logInManager.LoginAsync(usernameOrEmailAddress, password, tenancyName, shouldLockout);

            switch (loginResult.Result)
            {
                case AbpLoginResultType.Success:
                    return loginResult;
                default:
                    throw _abpLoginResultTypeHelper.CreateExceptionForFailedLoginAttempt(
                        loginResult.Result,
                        usernameOrEmailAddress,
                        tenancyName
                    );
            }
        }

        private string CreateAccessToken(IEnumerable<Claim> claims, TimeSpan? expiration = null)
        {
            return CreateToken(claims, expiration ?? _configuration.AccessTokenExpiration);
        }

        private (string token, string key) CreateRefreshToken(IEnumerable<Claim> claims)
        {
            var claimsList = claims.ToList();
            return (CreateToken(claimsList, AppConsts.RefreshTokenExpiration),
                claimsList.First(c => c.Type == AppConsts.TokenValidityKey).Value);
        }

        private string CreateToken(IEnumerable<Claim> claims, TimeSpan? expiration = null)
        {
            var now = DateTime.UtcNow;

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _configuration.Issuer,
                audience: _configuration.Audience,
                claims: claims,
                notBefore: now,
                signingCredentials: _configuration.SigningCredentials,
                expires: expiration == null ? (DateTime?)null : now.Add(expiration.Value)
            );

            return new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
        }

        private static string GetEncryptedAccessToken(string accessToken)
        {
            return SimpleStringCipher.Instance.Encrypt(accessToken, AppConsts.DefaultPassPhrase);
        }

        private async Task<IEnumerable<Claim>> CreateJwtClaims(
            ClaimsIdentity identity, User user,
            TimeSpan? expiration = null,
            TokenType tokenType = TokenType.AccessToken,
            string refreshTokenKey = null)
        {
            var tokenValidityKey = Guid.NewGuid().ToString();
            var claims = identity.Claims.ToList();
            var nameIdClaim = claims.First(c => c.Type == _identityOptions.ClaimsIdentity.UserIdClaimType);

            if (_identityOptions.ClaimsIdentity.UserIdClaimType != JwtRegisteredClaimNames.Sub)
            {
                claims.Add(new Claim(JwtRegisteredClaimNames.Sub, nameIdClaim.Value));
            }

            claims.AddRange(new[]
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.Now.ToUnixTimeSeconds().ToString(),
                    ClaimValueTypes.Integer64),
                new Claim(AppConsts.TokenValidityKey, tokenValidityKey),
                new Claim(AppConsts.UserIdentifier, user.ToUserIdentifier().ToUserIdentifierString()),
                new Claim(AppConsts.TokenType, tokenType.To<int>().ToString())
            });

            if (!string.IsNullOrEmpty(refreshTokenKey))
            {
                claims.Add(new Claim(AppConsts.RefreshTokenValidityKey, refreshTokenKey));
            }

            if (!expiration.HasValue)
            {
                expiration = tokenType == TokenType.AccessToken
                    ? _configuration.AccessTokenExpiration
                    : _configuration.RefreshTokenExpiration;
            }

            var expirationDate = DateTime.UtcNow.Add(expiration.Value);

            await _cacheManager
                .GetCache(AppConsts.TokenValidityKey)
                .SetAsync(tokenValidityKey, "", absoluteExpireTime: new DateTimeOffset(expirationDate));

            await _userManager.AddTokenValidityKeyAsync(
                user,
                tokenValidityKey,
                expirationDate
            );

            return claims;
        }

        private static string AddSingleSignInParametersToReturnUrl(string returnUrl, string signInToken, long userId,
            int? tenantId)
        {
            returnUrl += (returnUrl.Contains("?") ? "&" : "?") +
                         "accessToken=" + signInToken +
                         "&userId=" + Convert.ToBase64String(Encoding.UTF8.GetBytes(userId.ToString()));
            if (tenantId.HasValue)
            {
                returnUrl += "&tenantId=" + Convert.ToBase64String(Encoding.UTF8.GetBytes(tenantId.Value.ToString()));
            }

            return returnUrl;
        }


        private async Task<(bool isValid, ClaimsPrincipal principal)> IsRefreshTokenValid(string refreshToken)
        {
            ClaimsPrincipal principal = null;

            try
            {
                var validationParameters = new TokenValidationParameters
                {
                    ValidAudience = _configuration.Audience,
                    ValidIssuer = _configuration.Issuer,
                    IssuerSigningKey = _configuration.SecurityKey
                };

                foreach (var validator in _jwtOptions.Value.AsyncSecurityTokenValidators)
                {
                    if (!validator.CanReadToken(refreshToken))
                    {
                        continue;
                    }

                    try
                    {
                        (principal, _) = await validator.ValidateRefreshToken(refreshToken, validationParameters);
                        return (true, principal);
                    }
                    catch (Exception ex)
                    {
                        Logger.Debug(ex.ToString(), ex);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Debug(ex.ToString(), ex);
            }

            return (false, principal);
        }


        private bool AllowOneConcurrentLoginPerUser()
        {
            return _settingManager.GetSettingValue<bool>(AppSettings.UserManagement.AllowOneConcurrentLoginPerUser);
        }

        private async Task ValidateReCaptcha(string captchaResponse)
        {
            var requestUserAgent = Request.Headers["User-Agent"].ToString();
            if (!requestUserAgent.IsNullOrWhiteSpace() &&
                WebConsts.ReCaptchaIgnoreWhiteList.Contains(requestUserAgent.Trim()))
            {
                return;
            }

            await RecaptchaValidator.ValidateAsync(captchaResponse);
        }

        private async Task<TwoFactModelOutput> SendTwoFactCode(TwoFactModel model)
        {
            TwoFactModelOutput output = new TwoFactModelOutput();
            RadiusClient rc = new RadiusClient(esignConsts.RadiusServerName, esignConsts.RadiusSharedSecret, 3000, esignConsts.RadiusPort);
            RadiusPacket authPacket = rc.Authenticate(model.UserName, model.Password);
            authPacket.SetAttribute(new VendorSpecificAttribute(10135, 1, UTF8Encoding.UTF8.GetBytes("Testing")));
            authPacket.SetAttribute(new VendorSpecificAttribute(10135, 2, new[] { (byte)7 }));
            RadiusPacket receivedPacket = await rc.SendAndReceivePacket(authPacket);
            if (receivedPacket == null)
            {
                output.ResMessage = "Can't contact remote radius server!";
            }
            else if (receivedPacket.PacketType == RadiusCode.ACCESS_CHALLENGE)
            {
                ViewBag.DataState = Encoding.Default.GetString(receivedPacket.Attributes[1].Data);
                output.DataState = Encoding.Default.GetString(receivedPacket.Attributes[1].Data);
                output.ResMessage = null;
                output.Success = true;
            }
            else
            {
                output.Success = false;
                output.ResMessage = "Can't contact remote radius server!";
            }
            return output;
        }

        private async Task<TwoFactModelOutput> VerifyTwoFactCode(TwoFactModel model)
        {
            TwoFactModelOutput output = new TwoFactModelOutput();
            RadiusClient rcVerify = new RadiusClient(esignConsts.RadiusServerName, esignConsts.RadiusSharedSecret, 3000, esignConsts.RadiusPort);
            RadiusPacket authPacketVerify = rcVerify.Authenticate(model.UserName, model.InputCode);
            authPacketVerify.SetAttribute(new RadiusAttribute(RadiusAttributeType.STATE, Encoding.Default.GetBytes(model.DataState)));
            RadiusPacket receivedPacket = await rcVerify.SendAndReceivePacket(authPacketVerify);
            if (receivedPacket == null)
            {
                output.Success = false;
                output.ResMessage = "Can't contact remote radius server!";
            }
            else if (receivedPacket.PacketType == RadiusCode.ACCESS_REJECT)
            {
                output.Success = false;
                output.ResMessage = "The code invalid!";
            }
            else if (receivedPacket.PacketType == RadiusCode.ACCESS_ACCEPT)
            {
                output.Success = true;
                output.ResMessage = null;
            }
            else
            {
                output.Success = false;
            }
            return output;
        }

        [HttpPost]
        public async Task<AuthenticateResultModel> LoginGuest([FromBody] AuthenticateModel model)
        {
            var checkTenant = await _tenantRepository.GetAll().AsNoTracking().FirstOrDefaultAsync(e => e.TenancyName == model.TenancyName);
            //set tenant for session
            if (checkTenant != null)
            {
                AbpSession.Use(checkTenant.Id, null);
            }
            var loginResult = await GetLoginResultAsync(
                model.UserName,
                model.Password,
                GetTenancyNameOrNull()
            );

            //Two factor auth
            await _userManager.InitializeOptionsAsync(loginResult.Tenant?.Id);
            string twoFactorRememberClientToken = null;
            //string twoFactorRememberClientToken = null;
            if (await IsTwoFactorAuthRequiredAsync(loginResult, model))
            {
                if (model.TwoFactorVerificationCode.IsNullOrEmpty())
                {
                    //Add a cache item which will be checked in SendTwoFactorAuthCode to prevent sending unwanted two factor code to users.
                    await _cacheManager
                        .GetTwoFactorCodeCache()
                        .SetAsync(
                            loginResult.User.ToUserIdentifier().ToString(),
                            new TwoFactorCodeCacheItem()
                        );

                    return new AuthenticateResultModel
                    {
                        ReqVerifyCode = true,
                        UserId = loginResult.User.Id,
                        TwoFactorAuthProviders = await _userManager.GetValidTwoFactorProvidersAsync(loginResult.User),
                    };
                }
                twoFactorRememberClientToken = await TwoFactorAuthenticateAsync(loginResult, model);
            }

            // One Concurrent Login 
            if (AllowOneConcurrentLoginPerUser())
            {
                await ResetSecurityStampForLoginResult(loginResult);
            }

            var refreshToken = CreateRefreshToken(
                await CreateJwtClaims(
                    loginResult.Identity,
                    loginResult.User,
                    tokenType: TokenType.RefreshToken
                )
            );

            var accessToken = CreateAccessToken(
                await CreateJwtClaims(
                    loginResult.Identity,
                    loginResult.User,
                    refreshTokenKey: refreshToken.key
                )
            );

            return new AuthenticateResultModel
            {
                AccessToken = accessToken,
                ExpireInSeconds = (int)_configuration.AccessTokenExpiration.TotalSeconds,
                RefreshToken = refreshToken.token,
                RefreshTokenExpireInSeconds = (int)_configuration.RefreshTokenExpiration.TotalSeconds,
                EncryptedAccessToken = GetEncryptedAccessToken(accessToken),
                UserId = loginResult.User.Id,
                ShouldResetPassword = loginResult.User.ShouldChangePasswordOnNextLogin,
                TokenTemp =  HttpUtility.UrlEncode(SimpleStringCipher.Instance.Encrypt("userId=" + loginResult.User.Id + "&tenantId=" + loginResult.Tenant?.Id)),
        };
        }

        private StringBuilder GetMailContentSendOTP(string email, string code)
        {
            var content = new StringBuilder();
            content.AppendLine("Dear " + email + ",<br /><br />");
            content.AppendLine("Please use the verification code below to confirm your identity.<br />");
            content.AppendLine("This code was requested at " + DateTime.Now.ToString("yyyy/MM/dd HH:mm") + ".<br />");
            content.AppendLine("----------------------------------------------------------------------------------------<br />");
            content.AppendLine("Verification Code: <strong>" + code + "</strong><br /><br />");
            content.AppendLine("< About the verification code ><br />");
            content.AppendLine("- It expires in 2 minutes.<br />");
            content.AppendLine("- It becomes invalid once it is used.<br />");
            content.AppendLine("- You can request another code when it becomes invalid.<br />");
            content.AppendLine("----------------------------------------------------------------------------------------<br /><br />");
            content.AppendLine("You received this notification because you have requested the verification code for login.<br />");
            content.AppendLine("If you believe you received this notification in error, please ignore this email.<br /><br />");
            content.AppendLine("This is an auto - generated email. Do not reply to this email.<br /><br />");
            return content;
        }
        [HttpPost]
        public string ConvertEmaileToUsername(string email)
        {
            // Convert to lowercase
            int atIndex = email.IndexOf('@');

            // Trích xuất phần ký tự trước dấu '@'
            string username = email.Substring(0, atIndex);

            return username;
        }
    }
}