using esign.Business.Dto.Ver1;
using esign.Esign.Business.EsignRequest.Dto.Ver1;
using esign.Ver1.Esign.Business.EsignApiOtherSystem.Dto;
using System;
using System.Collections.Generic;

namespace esign.Business.Ver1
{
    public class CreateOrEditEsignApiOtherSystemDto
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public decimal? TotalCost { get; set; }
        public decimal? Roi { get; set; }
        public DateTime? ProjectScheduleFrom { get; set; }
        public DateTime? ProjectScheduleTo { get; set; }
        public DateTime? ExpectedDate { get; set; }
        public string Content { get; set; }
        public string Message { get; set; }
        public string Category { get; set; }
        public string AddCC { get; set; }
        public bool? IsDigitalSignature { get; set; }
        public int StatusType { get; set; }
        public string SystemCode { get; set; }
        public string ReferenceType { get; set; }
        public long? ReferenceId { get; set; }
        public long? TypeSignId { get; set; }
        public long? TemplateSignatureId { get; set; }
        public string RequesterUserName { get; set; }
        public List<CreatOrEditEsignReferenceRequestOtherSystemDto> RequestRefs { get; set; }
        public List<string> ListCategory { get; set; }
        public List<CreateSignersFromSystemDto> ListSigners { get; set; }
        public List<CreateDocumentFromSystemDto> ListDocuments { get; set; }
    }

    public class CreateSignersFromSystemDto
    {
        public string SignerUserName { get; set; }
        public int? SigningOrder { get; set; }
        public string StatusCode { get; set; }
        public long? ColorId { get; set; }
        public string Color { get; set; }
        public long? ReferenceSignerId { get; set; }
    }

    public class CreateDocumentFromSystemDto
    {
        public string DocumentName { get; set; }
        public int? DocumentOrder { get; set; }
        public byte[] PdfFileByte { get; set; }
        public List<CreatePositionsFromSystemDto> Positions { get; set; }

    }
    public class CreatePositionsFromSystemDto
    {
        public long Id { get; set; }
        public long? DocumentId { get; set; }
        public long? PageNum { get; set; }
        public string SignerUserName { get; set; }
        public long? PositionX { get; set; }
        public long? PositionY { get; set; }
        public long? PositionW { get; set; }
        public long? PositionH { get; set; }
        public long? Rotate { get; set; }
        public bool? IsDigitalSignature { get; set; }
        public long? TypeId { get; set; }
        public int? FontSize { get; set; }
        public string TextValue { get; set; }
        public string TextName { get; set; }
        public string FontFamily { get; set; }
        public string Color { get; set; }
        public string BackGroundColor { get; set; }
        public string TextAlignment { get; set; }
        public bool IsBold { get; set; }
        public bool IsItalic { get; set; }
        public bool IsUnderline { get; set; }

    }

    public class UpdateRequestStatusToOrtherSystemDto
    {
        public long RequestId { get; set; }
        public long ReferenceRequestId { get; set; }
        public string ReferenceRequestType { get; set; }
        public long ReferenceSignerId { get; set; }
        public string UserName { get; set; }
        public string Status { get; set; }
        public string Note { get; set; }
        public List<DocumentFromSystemDto> ListDocuments { get; set; }
    }
    public class DocumentFromSystemDto
    {
        public string DocumentName { get; set; }
        public byte[] PdfFileByte { get; set; }
    }

    public class DocumentForOtherSystemDto
    {
        public byte[] SecretKey { get; set; }
        public string IsUserPassInput { get; set; }
        public string Md5Hash { get; set; }
        public byte[] EncryptedUserPass { get; set; }
        public string DocumentName { get; set; }
        public int? DocumentOrder { get; set; }
        public string DocumentPath { get; set; }
    }
}


