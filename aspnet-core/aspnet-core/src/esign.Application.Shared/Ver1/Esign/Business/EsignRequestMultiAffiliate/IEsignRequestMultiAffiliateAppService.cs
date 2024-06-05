using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Web.Models;
using esign.Business.Dto;
using esign.Business.Dto.Ver1;
using esign.Esign.Business.EsignRequest.Dto.Ver1;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace esign.Business.Ver1
{
    public interface IEsignRequestMultiAffiliateAppService : IApplicationService
    {
        Task<AffiliateAuthenticateResultModel> GetMultiAffiliateAuthenToken(string url, string tenancy, string user, string pass);
        //users
        Task<List<UserMultiAffiliateDto>> GetMultiAffiliateUsersInfo();
        Task ReceiveMultiAffiliateUsersInfo(int affiliateId, string ApiUrl, string affiliateCode, string ApiUsername,
            string ApiEncryptedSecretKeyBase64, string ApiEncryptedPasswordBase64);
        //request
        Task<EsignRequestMultiAffiliateDto> GetRequestForMultiAffiliate(long requestId);
        Task SendMultiAffiliateEsignRequest(EsignRequestMultiAffiliateDto dto, string ApiUrl, string affiliateCode, string ApiUsername,
            string ApiEncryptedSecretKeyBase64, string ApiEncryptedPasswordBase64);
        Task CreateMultiAffiliateRequest(EsignRequestMultiAffiliateDto requestDto);
        //signing
        Task<EsignRequestMultiAffiliateSigningInfoDto> GetRequestForMultiAffiliateSigningInfo(long requestId, long userId);
        Task SendMultiAffiliateEsignRequestSigningInfo(EsignRequestMultiAffiliateSigningInfoDto dto, string ApiUrl, string affiliateCode, string ApiUsername,
            string ApiEncryptedSecretKeyBase64, string ApiEncryptedPasswordBase64);
        Task UpdateMultiAffiliateRequestSigningInfo(EsignRequestMultiAffiliateSigningInfoDto requestDto);
        //reject
        Task<EsignRequestMultiAffiliateRejectInfoDto> GetRequestForMultiAffiliateRejectInfo(long requestId, long userId);
        Task SendMultiAffiliateEsignRequestRejectInfo(EsignRequestMultiAffiliateRejectInfoDto dto, string ApiUrl, string affiliateCode, string ApiUsername,
            string ApiEncryptedSecretKeyBase64, string ApiEncryptedPasswordBase64);
        Task UpdateMultiAffiliateRequestRejectInfo(EsignRequestMultiAffiliateRejectInfoDto requestDto);
        //comment
        Task<EsignRequestMultiAffiliateCommentInfoDto> GetRequestForMultiAffiliateCommentInfo(long requestId, long userId);
        Task SendMultiAffiliateEsignRequestCommentInfo(EsignRequestMultiAffiliateCommentInfoDto dto, string ApiUrl, string affiliateCode, string ApiUsername,
            string ApiEncryptedSecretKeyBase64, string ApiEncryptedPasswordBase64);
        Task UpdateMultiAffiliateRequestCommentInfo(EsignRequestMultiAffiliateCommentInfoDto requestDto);
        //reassign
        Task<EsignRequestMultiAffiliateReassignInfoDto> GetRequestForMultiAffiliateReassignInfo(long requestId, long userId, long additionUserId);
        Task SendMultiAffiliateEsignRequestReassignInfo(EsignRequestMultiAffiliateReassignInfoDto dto, string ApiUrl, string affiliateCode, string ApiUsername,
            string ApiEncryptedSecretKeyBase64, string ApiEncryptedPasswordBase64);
        Task UpdateMultiAffiliateRequestReassignInfo(EsignRequestMultiAffiliateReassignInfoDto requestDto);
        //remind
        Task<EsignRequestMultiAffiliateRemindInfoDto> GetRequestForMultiAffiliateRemindInfo(long requestId, long userId);
        Task SendMultiAffiliateEsignRequestRemindInfo(EsignRequestMultiAffiliateRemindInfoDto dto, string ApiUrl, string affiliateCode, string ApiUsername,
            string ApiEncryptedSecretKeyBase64, string ApiEncryptedPasswordBase64);
        Task UpdateMultiAffiliateRequestRemindInfo(EsignRequestMultiAffiliateRemindInfoDto requestDto);
        //revoke
        Task<EsignRequestMultiAffiliateRevokeInfoDto> GetRequestForMultiAffiliateRevokeInfo(long requestId, long userId);
        Task SendMultiAffiliateEsignRequestRevokeInfo(EsignRequestMultiAffiliateRevokeInfoDto dto, string ApiUrl, string affiliateCode, string ApiUsername,
            string ApiEncryptedSecretKeyBase64, string ApiEncryptedPasswordBase64);
        Task UpdateMultiAffiliateRequestRevokeInfo(EsignRequestMultiAffiliateRevokeInfoDto requestDto);
        //transfer
        Task<EsignRequestMultiAffiliateTransferInfoDto> GetRequestForMultiAffiliateTransferInfo(long requestId, long userId, long additionUserId);
        Task SendMultiAffiliateEsignRequestTransferInfo(EsignRequestMultiAffiliateTransferInfoDto dto, string ApiUrl, string affiliateCode, string ApiUsername,
            string ApiEncryptedSecretKeyBase64, string ApiEncryptedPasswordBase64);
        Task UpdateMultiAffiliateRequestTransferInfo(EsignRequestMultiAffiliateTransferInfoDto requestDto);
        //share
        Task<EsignRequestMultiAffiliateShareInfoDto> GetRequestForMultiAffiliateShareInfo(long requestId, long userId);
        Task SendMultiAffiliateEsignRequestShareInfo(EsignRequestMultiAffiliateShareInfoDto dto, string ApiUrl, string affiliateCode, string ApiUsername,
            string ApiEncryptedSecretKeyBase64, string ApiEncryptedPasswordBase64);
        Task UpdateMultiAffiliateRequestShareInfo(EsignRequestMultiAffiliateShareInfoDto requestDto);
        //additionfile
        Task<EsignRequestMultiAffiliateAdditionFileInfoDto> GetRequestForMultiAffiliateAdditionFileInfo(long requestId, long userId);
        Task SendMultiAffiliateEsignRequestAdditionFileInfo(EsignRequestMultiAffiliateAdditionFileInfoDto dto, string ApiUrl, string affiliateCode, string ApiUsername,
            string ApiEncryptedSecretKeyBase64, string ApiEncryptedPasswordBase64);
        Task UpdateMultiAffiliateRequestAdditionFileInfo(EsignRequestMultiAffiliateAdditionFileInfoDto requestDto);
    }

}


