using System.Threading.Tasks;
using Abp.Authorization;
using esign.Master.Dto.Ver1;
using System.Collections.Generic;
using Abp.Dapper.Repositories;
using System.Linq;
using esign.Url;
using Abp.Extensions;
using Abp.Domain.Repositories;
using esign.Esign.Master.MstEsignActiveDirectory.Dto.Ver1;
using Microsoft.EntityFrameworkCore;
using Abp.Linq.Extensions;
using Abp.Application.Services.Dto;
using esign.Dto;
using esign.Esign.Master.MstEsignActiveDirectory.Exporting.Ver1;
using Microsoft.AspNetCore.Mvc;
using esign.Authorization.Users;
using System;
using esign.MultiTenancy;
using Abp.Localization;
using Abp.Application.Services;
using Abp;
using Abp.Runtime.Session;
using esign.Authorization;
using Abp.UI;

namespace esign.Master.Ver1
{
    [AbpAuthorize]
    public class MstEsignActiveDirectoryAppService : esignVersion1AppServiceBase, IMstEsignActiveDirectoryAppService
    {
        private readonly IDapperRepository<MstEsignActiveDirectory, int> _dapperRepo;
        private readonly IWebUrlService _webUrlService;
        private readonly IRepository<MstEsignActiveDirectory> _mstEsignActiveDirectory;
        private readonly IMstEsignActiveDirectoryExcelExporter _activeDirectoryExcelExporter;
        private readonly IRepository<User, long> _userRepo;
        private readonly IRepository<Tenant> _tenantRepo;

        public MstEsignActiveDirectoryAppService(
            IDapperRepository<MstEsignActiveDirectory, int> dapperRepo,
            IWebUrlService webUrlService,
            IRepository<MstEsignActiveDirectory> mstEsignActiveDirectory,
            IMstEsignActiveDirectoryExcelExporter activeDirectoryExcelExporter,
            IRepository<User, long> userRepo,
            IRepository<Tenant> tenantRepo)
        {
            _dapperRepo = dapperRepo;
            _webUrlService = webUrlService;
            _mstEsignActiveDirectory = mstEsignActiveDirectory;
            _activeDirectoryExcelExporter = activeDirectoryExcelExporter;
            _userRepo = userRepo;
            _tenantRepo = tenantRepo;
        }

        [HttpGet]
        [AbpAuthorize(AppPermissions.Pages_Master_MstEsignActiveDirectory_GetAllSigners)]
        public async Task<MstEsignActiveDirectoryResponseDto<MstEsignActiveDirectoryDto>> GetAllSigners([FromQuery] MstEsignActiveDirectoryRequestDto requestDto)
        {

            var result = await _dapperRepo.QueryAsync<MstEsignActiveDirectoryDto>(
                "exec Sp_MstEsignActiveDirectory_GetAllSigners @p_UserId, @p_SearchValue, @p_GroupCategory, @p_SkipCount, @p_MaxResultCount, @p_DomainUrl",
                new
                {
                    @p_UserId = AbpSession.UserId,
                    @p_SearchValue = requestDto.SearchValue,
                    @p_GroupCategory = 0,
                    @p_SkipCount = requestDto.SkipCount,
                    @p_MaxResultCount = requestDto.MaxResultCount,
                    @p_DomainUrl = _webUrlService.ServerRootAddressFormat.EnsureEndsWith('/')
                }
            );

            var resultCount = await _dapperRepo.QueryAsync<string>(
                "exec Sp_MstEsignActiveDirectory_GetAllSignersCount @p_UserId, @p_SearchValue, @p_GroupCategory",
                new
                {
                    @p_UserId = AbpSession.UserId,
                    @p_SearchValue = requestDto.SearchValue,
                    @p_GroupCategory = 0
                }
            );

            return new MstEsignActiveDirectoryResponseDto<MstEsignActiveDirectoryDto>
            {
                TotalCount = int.Parse(resultCount.FirstOrDefault() ?? "0"),
                Items = result.ToList()
            };
        }

        [HttpGet]
        [AbpAuthorize(AppPermissions.Pages_Master_MstEsignActiveDirectory_GetAllSignersForWeb)]
        public async Task<MstEsignActiveDirectoryResponseDto<MstEsignActiveDirectoryForWebDto>> GetAllSignersForWeb([FromQuery]MstEsignActiveDirectoryRequestDto requestDto)
        {

            var result = await _dapperRepo.QueryAsync<MstEsignActiveDirectoryForWebDto>(
                "exec Sp_MstEsignActiveDirectory_GetAllSigners @p_UserId, @p_SearchValue, @p_GroupCategory, @p_SkipCount, @p_MaxResultCount, @p_DomainUrl",
                new
                {
                    @p_UserId = AbpSession.UserId,
                    @p_SearchValue = requestDto.SearchValue,
                    @p_GroupCategory = 0,
                    @p_SkipCount = requestDto.SkipCount,
                    @p_MaxResultCount = requestDto.MaxResultCount,
                    @p_DomainUrl = _webUrlService.ServerRootAddressFormat.EnsureEndsWith('/')
                }
            );

            var resultCount = await _dapperRepo.QueryAsync<string>(
                "exec Sp_MstEsignActiveDirectory_GetAllSignersCount @p_UserId, @p_SearchValue, @p_GroupCategory",
                new
                {
                    @p_UserId = AbpSession.UserId,
                    @p_SearchValue = requestDto.SearchValue,
                    @p_GroupCategory = 0
                }
            );

            return new MstEsignActiveDirectoryResponseDto<MstEsignActiveDirectoryForWebDto>
            {
                TotalCount = int.Parse(resultCount.FirstOrDefault() ?? "0"),
                Items = result.ToList()
            };
        }

        [HttpGet]
        [AbpAuthorize(AppPermissions.Pages_Master_MstEsignActiveDirectory_GetAllSignerByGroup)]
        public async Task<MstEsignActiveDirectoryResponseDto<MstEsignActiveDirectoryDto>> GetAllSignerByGroup([FromQuery] MstEsignActiveDirectoryRequestDto requestDto)
        {

            var result = await _dapperRepo.QueryAsync<MstEsignActiveDirectoryDto>(
                "exec Sp_MstEsignActiveDirectory_GetAllSigners @p_UserId, @p_SearchValue, @p_GroupCategory, @p_SkipCount, @p_MaxResultCount, @p_DomainUrl",
                new
                {
                    @p_UserId = AbpSession.UserId,
                    @p_SearchValue = requestDto.SearchValue,
                    @p_GroupCategory = requestDto.GroupCategory,
                    @p_SkipCount = requestDto.SkipCount,
                    @p_MaxResultCount = requestDto.MaxResultCount,
                    @p_DomainUrl = _webUrlService.ServerRootAddressFormat.EnsureEndsWith('/')
                }
            );

            var resultCount = await _dapperRepo.QueryAsync<string>(
                "exec Sp_MstEsignActiveDirectory_GetAllSignersCount @p_UserId, @p_SearchValue, @p_GroupCategory",
                new
                {
                    @p_UserId = AbpSession.UserId,
                    @p_SearchValue = requestDto.SearchValue,
                    @p_GroupCategory = requestDto.GroupCategory
                }
            );

            return new MstEsignActiveDirectoryResponseDto<MstEsignActiveDirectoryDto>
            {
                TenancyName = (await _tenantRepo.FirstOrDefaultAsync(x => x.Id == AbpSession.TenantId))?.TenancyName,
                TotalCount = int.Parse(resultCount.FirstOrDefault() ?? "0"),
                Items = result.ToList()
            };
        }

        [HttpGet]
        [AbpAuthorize(AppPermissions.Pages_Master_MstEsignActiveDirectory_GetMyProfile)]
        public async Task<MstEsignActiveDirectoryGetMyProfileDto> GetMyProfile()
        {
            TimeZoneInfo localTimeZone = TimeZoneInfo.Local;
            string _sqlGetData = "Exec Sp_MstEsignActiveDirectory_GetMyProfile @p_UserID, @p_DomainUrl";

            IEnumerable<MstEsignActiveDirectoryGetMyProfileDto> _result = await _dapperRepo.QueryAsync<MstEsignActiveDirectoryGetMyProfileDto>(_sqlGetData, new
            {
                p_UserID = AbpSession.UserId,
                p_DomainUrl = _webUrlService.ServerRootAddressFormat.EnsureEndsWith('/')
            });
            MstEsignActiveDirectoryGetMyProfileDto output = _result.FirstOrDefault();
            output.LocalTimeZone = localTimeZone.DisplayName;
            return output;
        }

        [HttpGet]
        [AbpAuthorize(AppPermissions.Pages_Master_MstEsignActiveDirectory_GetUserInformationById)]
        public async Task<MstEsignActiveDirectoryGetMyProfileDto> GetUserInformationById(long userId)
        {
            if (userId != AbpSession.UserId)
            {
                throw new UserFriendlyException("Unauthorized!");
            }
            string _sqlGetData = "Exec Sp_MstEsignActiveDirectory_GetMyProfile @p_UserID, @p_DomainUrl";

            IEnumerable<MstEsignActiveDirectoryGetMyProfileDto> _result = await _dapperRepo.QueryAsync<MstEsignActiveDirectoryGetMyProfileDto>(_sqlGetData, new
            {
                p_UserID = userId,
                p_DomainUrl = _webUrlService.ServerRootAddressFormat.EnsureEndsWith('/')
            });

            return _result.FirstOrDefault();
        }

        [HttpGet]
        [AbpAuthorize(AppPermissions.Pages_Master_MstEsignActiveDirectory_GetAllActiveDirectory)]
        public async Task<PagedResultDto<MstEsignActiveDirectoryDto>> GetAllActiveDirectory([FromQuery]MstEsignActiveDirectoryInputDto input)
        {
            var listAD = _userRepo.GetAll().AsNoTracking()
                .Where(e => e.IsAD == true)
                .WhereIf(!string.IsNullOrWhiteSpace(input.TextFilter), e => e.EmailAddress.Contains(input.TextFilter)
                || e.FullName.Contains(input.TextFilter) || e.Department.Contains(input.TextFilter)
                || e.Title.Contains(input.TextFilter));

            var totalCount = listAD.Count();

            var result = (from o in listAD
                          select new MstEsignActiveDirectoryDto { 
                            //Id = o.Id,
                            Email = o.EmailAddress,
                            Title = o.Title,
                            Department = o.DepartmentName,
                            FullName = o.Name,
                            ImageUrl = string.IsNullOrWhiteSpace(o.ImageUrl) ? "" : _webUrlService.ServerRootAddressFormat.EnsureEndsWith('/') + o.ImageUrl
                          }).PageBy(input);

            return new PagedResultDto < MstEsignActiveDirectoryDto > { TotalCount = totalCount, Items = await result.ToListAsync() };
        }

        [HttpGet]
        [AbpAuthorize(AppPermissions.Pages_Master_MstEsignActiveDirectory_GetActiveDirectory)]
        public async Task<FileDto> GetActiveDirectory([FromQuery] MstEsignActiveDirectoryInputDto input)
        {
            var listAD = _userRepo.GetAll().AsNoTracking()
                .Where(e => e.IsAD == true)
                .WhereIf(!string.IsNullOrWhiteSpace(input.TextFilter), e => e.EmailAddress.Contains(input.TextFilter)
                || e.FullName.Contains(input.TextFilter) || e.Department.Contains(input.TextFilter)
                || e.Title.Contains(input.TextFilter));

            var result = (from o in listAD
                          select new MstEsignActiveDirectoryOutputDto
                          {
                              Email = o.EmailAddress,
                              Title = o.Title,
                              Department = o.DepartmentName,
                              FullName = o.Name
                          });
            var exportToExcel = await result.ToListAsync();
            return _activeDirectoryExcelExporter.ExportToFile(exportToExcel);
        }

        [HttpGet]
        [AbpAuthorize(AppPermissions.Pages_Master_MstEsignActiveDirectory_GetMyAccountInfomation)]
        public async Task<UserAccountInfomationDto> GetMyAccountInfomation()
        {
            var data = await _userRepo.FirstOrDefaultAsync(e => e.Id == AbpSession.UserId);
            return new UserAccountInfomationDto
            {
                Id = data.Id,
                Company = TenantManager.FindById((int)data.TenantId)?.TenancyName,
                Department = data.DepartmentName,
                Email = data.EmailAddress,
                Surname = data.Surname,
                GivenName = data.GivenName,
                ImageUrl = _webUrlService.ServerRootAddressFormat.EnsureEndsWith('/') + data.ImageUrl,
                OfficeLocation = data.Office,
                WorkPhone = data.WorkPhone,
                Title = data.Title,
                IsQuickSign = data.IsQuickSign,
                IsDigitalSignature = data.IsDigitalSignature,
                IsDigitalSignatureOtp = data.IsDigitalSignatureOtp,
                Division = data.DivisionName,
                IsReceiveRemind = data.IsReceiveRemind,
                Language = SettingManager.GetSettingValueForUser(LocalizationSettingNames.DefaultLanguage, AbpSession.ToUserIdentifier())
            };
        }

        [HttpPost]
        [AbpAuthorize(AppPermissions.Pages_Master_MstEsignActiveDirectory_SaveAccountInfomation)]
        public async Task SaveAccountInfomation([FromBody]UserAccountInfomationDto input)
        {
            //check OK
            if(input.Id != AbpSession.UserId)
            {
                throw new UserFriendlyException("Unauthorized change account!");
            }
            var user = _userRepo.FirstOrDefault((long)input.Id); 
            
            user.Email = input.Email;
            user.Surname = input.Surname;
            user.GivenName = input.GivenName;
            user.Name = input.GivenName + ' ' + input.Surname;
            user.PhoneNumber = input.WorkPhone;
            user.WorkPhone = input.WorkPhone;
            user.IsQuickSign = input.IsQuickSign;
            user.IsDigitalSignature = input.IsDigitalSignature;
            user.IsDigitalSignatureOtp = input.IsDigitalSignatureOtp;
            user.IsReceiveRemind = input.IsReceiveRemind;
            await SettingManager.ChangeSettingForUserAsync(AbpSession.ToUserIdentifier(), LocalizationSettingNames.DefaultLanguage, input.Language);
            await _userRepo.UpdateAsync(user);
        }

        [HttpGet]
        [AbpAuthorize(AppPermissions.Pages_Master_MstEsignActiveDirectory_GetAllSignersForTransfer)]
        public async Task<MstEsignActiveDirectoryResponseDto<MstEsignActiveDirectoryForWebDto>> GetAllSignersForTransfer([FromQuery] MstEsignActiveDirectoryRequestDto requestDto)
        {

            var result = await _dapperRepo.QueryAsync<MstEsignActiveDirectoryForWebDto>(
                "exec Sp_MstEsignActiveDirectory_GetAllSignerTransfer @p_UserId, @p_SearchValue, @p_SkipCount, @p_MaxResultCount, @p_DomainUrl",
                new
                {
                    @p_UserId = AbpSession.UserId,
                    @p_SearchValue = requestDto.SearchValue,
                    @p_SkipCount = requestDto.SkipCount,
                    @p_MaxResultCount = requestDto.MaxResultCount,
                    @p_DomainUrl = _webUrlService.ServerRootAddressFormat.EnsureEndsWith('/')
                }
            );

            var resultCount = await _dapperRepo.QueryAsync<string>(
                "exec Sp_MstEsignActiveDirectory_GetAllSignerTransferCount @p_UserId, @p_SearchValue",
                new
                {
                    @p_UserId = AbpSession.UserId,
                    @p_SearchValue = requestDto.SearchValue
                }
            );

            return new MstEsignActiveDirectoryResponseDto<MstEsignActiveDirectoryForWebDto>
            {
                TotalCount = int.Parse(resultCount.FirstOrDefault() ?? "0"),
                Items = result.ToList()
            };
        }
    }
}
