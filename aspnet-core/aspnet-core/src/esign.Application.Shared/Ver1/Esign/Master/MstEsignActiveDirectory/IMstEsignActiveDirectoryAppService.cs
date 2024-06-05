using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using esign.Master.Dto.Ver1;

namespace esign.Master.Ver1
{
    public interface IMstEsignActiveDirectoryAppService : IApplicationService
    {
        Task<MstEsignActiveDirectoryResponseDto<MstEsignActiveDirectoryDto>> GetAllSigners(MstEsignActiveDirectoryRequestDto requestDto);
        Task<MstEsignActiveDirectoryResponseDto<MstEsignActiveDirectoryForWebDto>> GetAllSignersForWeb(MstEsignActiveDirectoryRequestDto requestDto);
        Task<MstEsignActiveDirectoryResponseDto<MstEsignActiveDirectoryDto>> GetAllSignerByGroup(MstEsignActiveDirectoryRequestDto requestDto);
        Task<MstEsignActiveDirectoryGetMyProfileDto> GetMyProfile();
        Task<UserAccountInfomationDto> GetMyAccountInfomation();
    }
}
