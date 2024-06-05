using Abp.Application.Services;
using esign.Common.Dto.Ver1;
using esign.Ver1.Common.Dto;
using esign.Ver1.Notifications.Dto;
using Syncfusion.Pdf.Graphics;
using Syncfusion.Pdf.Parsing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace esign.Ver1.Common
{
    public interface ICommonEsignWebAppService : IApplicationService
    {
        Task<bool> SignDocument(SignDocumentInputDto signDocumentInputDto, string pathRoot, byte[] imageSign);

        ImageToPdOutputDto AddImageToPdf(List<ParamAddImageToPdfDto> listFileDocument, string pathRoot, byte[] imageSign);
        Task RequestNextApprove(long requestId, int signingOrder);
        //Task RequestNextApproveV2(long requestId, int signingOrder, string CC, string BCC);
        string GetFilePath(long ReqId);
        string ConvertTypeSignature(long typeId);
        ResizeWidthHeightDto ResizeImage(float with, float height, MemoryStream image);
        Task CreateEsignNotification(List<NotificationInputFullDto> inputs);
        Task SendNoti(long reqId, long fromUserId, string code, List<long> listUser);
        byte[] SignDigitalToPdf(PdfLoadedDocument document, List<SignatureImageAndPositionDto> listPositions, byte[] imageSign, byte[] filePdf, string digitalPin, string digitalUuid, long currentUserId);
        //Task SendEmailEsignRequest(long reqId, string emailCode, long fromUserId, long toUserId);
    }
}
