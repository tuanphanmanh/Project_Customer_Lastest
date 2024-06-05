using Abp.Application.Services;
using esign.Common.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace esign.Common
{
    public interface ICommonEsignAppService : IApplicationService
    {
        Task SignDocument(SignDocumentInputDto signDocumentInputDto, string pathRoot, byte[] imageSign);

        ImageToPdOutputDto AddImageToPdf(List<ParamAddImageToPdfDto> listFileDocument, string pathRoot, byte[] imageSign);
        void RequestNextApprove(long requestId, int signingOrder);
        string GetFilePath(long ReqId);
        string ConvertTypeSignature(long typeId);
    }
}
