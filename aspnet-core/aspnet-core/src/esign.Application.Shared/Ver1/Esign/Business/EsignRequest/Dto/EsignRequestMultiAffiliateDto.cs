using Abp.Application.Services.Dto;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace esign.Business.Dto.Ver1
{
    //common
    public class CreateOrEditActivitiesMultiAffiliateDto
    {
        public long? AffiliateReferenceId { get; set; }
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public string ActivityCode { get; set; }
        public string IpAddress { get; set; }
        public string Description { get; set; }
        public string CreationTimeUtc { get; set; }
    }
    public class CreateOrEditCommentsMultiAffiliateDto
    {
        public long? AffiliateReferenceId { get; set; }
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public string Content { get; set; }
        public bool IsPublic { get; set; }
        public string CreationTimeUtc { get; set; }
    }
    public class CreateOrEditStatusSignersMultiAffiliateDto
    {
        public long? AffiliateReferenceId { get; set; }
        public int? StatusId { get; set; }
        public int? SigningOrder { get; set; }
        public bool IsSharing { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public string Title { get; set; }
        public string Department { get; set; }
        public string Division { get; set; }
        public string Note { get; set; }
        public int? ColorId { get; set; }
        public string CreationUserName { get; set; }
        public string CreationUserEmail { get; set; }
        public string CreationTimeUtc { get; set; }
    }
    public class CreateOrEditEsignTransferSignerMultiAffiliateDto
    {
        public long? AffiliateReferenceId { get; set; }
        public int? StatusId { get; set; }
        public int? SigningOrder { get; set; }
        public bool IsSharing { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public string Title { get; set; }
        public string Department { get; set; }
        public string Division { get; set; }
        public string Note { get; set; }
        public int? ColorId { get; set; }
        public string FromUserName { get; set; }
        public string FromUserEmail { get; set; }
        public string ToUserName { get; set; }
        public string ToUserEmail { get; set; }
        public int? TypeId { get; set; }
        public bool IsCc { get; set; }
        public string CreationUserName { get; set; }
        public string CreationUserEmail { get; set; }
        public string CreationTimeUtc { get; set; }
    }
    public class NotificationMultiAffiliateDto
    {
        public long ToAffiliateId { get; set; }
        public long? AffiliateReferenceId { get; set; }
        public string Affiliate { get; set; } //Affiliate
        public string EmailTemplateCode { get; set; }
        public string MobileTemplateCode { get; set; }
        public string FromUserEmail { get; set; }
        public List<string> ToUserEmails { get; set; }
        public string Ccs { get; set; }
    }
    //user
    public class UserMultiAffiliateDto
    {
        public string UserName { get; set; }
        public string EmailAddress { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public string TitleFullName { get; set; }
        public string DepartmentName { get; set; }
        public string DivisionName { get; set; }
        public string ImageUrl { get; set; }
        public string ImageDataBase64 { get; set; }
    }
    //request
    public class EsignRequestMultiAffiliateDto
    {
        public long? AffiliateReferenceId { get; set; }
        public string Affiliate { get; set; } //Affiliate
        public string CreatorUserName { get; set; }
        public string CreatorUserEmail { get; set; }
        public string CreatorUserFullName { get; set; }
        public string CreatorUserTitle { get; set; }
        public string CreatorUserTitleFullName { get; set; }
        public string CreatorUserDepartment { get; set; }
        public string CreatorUserDivision { get; set; }
        public string CreatorUserImageUrl { get; set; }
        public string CreatorUserImageDataBase64 { get; set; }
        public string Title { get; set; }
        public decimal? TotalCost { get; set; }
        public decimal? Roi { get; set; }
        public string ProjectScheduleFromUtc { get; set; }
        public string ProjectScheduleToUtc { get; set; }
        public string RequestDateUtc { get; set; }
        public string ExpectedDateUtc { get; set; }
        public string Content { get; set; }
        public string Message { get; set; }
        public string Category { get; set; }
        public string AddCC { get; set; }
        public bool? IsDigitalSignature { get; set; }
        public int? StatusId { get; set; }
        public string System { get; set; }
        public string CreationTimeUtc { get; set; }
        public string LastModificationTimeUtc { get; set; }
        public string LastModifierUserName { get; set; }
        public string LastModifierUserEmail { get; set; }
        public string SignersJson { get; set; }
        public List<CreateOrEditSignersMultiAffiliateDto> Signers
        {
            get
            {
                return SignersJson == null ? null : JsonConvert.DeserializeObject<List<CreateOrEditSignersMultiAffiliateDto>>(SignersJson);
            }
        }
        public string DocumentsJson { get; set; }
        public List<CreateOrEditDocumentMultiAffiliateDto> Documents
        {
            get
            {
                return DocumentsJson == null ? null : JsonConvert.DeserializeObject<List<CreateOrEditDocumentMultiAffiliateDto>>(DocumentsJson);
            }
        }
        public string ActivitiesJson { get; set; }
        public List<CreateOrEditActivitiesMultiAffiliateDto> Activities
        {
            get
            {
                return ActivitiesJson == null ? null : JsonConvert.DeserializeObject<List<CreateOrEditActivitiesMultiAffiliateDto>>(ActivitiesJson);
            }
        }
    }
    public class CreateOrEditSignersMultiAffiliateDto
    {
        public long? AffiliateReferenceId { get; set; }
        public string Affiliate { get; set; }
        public string UserName { get; set; }
        public string PrivateMessage { get; set; }
        public int? StatusId { get; set; }
        public int SigningOrder { get; set; }
        public bool IsSharing { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string FullName { get; set; }
        public string Title { get; set; }
        public string TitleFullName { get; set; }
        public string Department { get; set; }
        public string Division { get; set; }
        public string ImageUrl { get; set; }
        public string ImageDataBase64 { get; set; }
        public string Color { get; set; }
        public string RequestDateUtc { get; set; }
        public string CreationTimeUtc { get; set; }
        public string LastModificationTimeUtc { get; set; }
        public string LastModifierUserName { get; set; }
        public string LastModifierUserEmail { get; set; }
    }
    public class CreateOrEditDocumentMultiAffiliateDto
    {
        public long? AffiliateReferenceId { get; set; }
        public string DocumentPath { get; set; }
        public string DocumentDataBase64 { get; set; }
        public string DocumentViewDataBase64 { get; set; }
        public string DocumentOriginalDataBase64 { get; set; }
        public string DocumentName { get; set; }
        public string Md5Hash { get; set; }
        public int DocumentOrder { get; set; }
        public string QrRandomCode { get; set; }
        public int TotalPage { get; set; }
        public int TotalSize { get; set; }
        public string EncryptedUserPassBase64 { get; set; }
        public string SecretKeyBase64 { get; set; }
        public bool IsAdditionalFile { get; set; }
        public bool IsDigitalSignatureFile { get; set; }
        public string CreationTimeUtc { get; set; }
        public string LastModificationTimeUtc { get; set; }
        public string LastModifierUserName { get; set; }
        public string LastModifierUserEmail { get; set; }
        public string PositionsJson { get; set; }
        public List<CreateOrEditPositionsMultiAffiliateDto> Positions
        {
            get
            {
                return PositionsJson == null ? null : JsonConvert.DeserializeObject<List<CreateOrEditPositionsMultiAffiliateDto>>(PositionsJson);
            }
        }
    }
    public class CreateOrEditPositionsMultiAffiliateDto
    {
        public long? AffiliateReferenceId { get; set; }
        public long? PageNum { get; set; }
        public string SignerUserName { get; set; }
        public string SignerUserEmail { get; set; }
        public string UserSignatureBase64 { get; set; }
        public long? PositionX { get; set; }
        public long? PositionY { get; set; }
        public long? PositionW { get; set; }
        public long? PositionH { get; set; }
        public long? Rotate { get; set; }
        public bool IsDigitalSignature { get; set; }
        public int? TypeId { get; set; }
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
        public string CreationTimeUtc { get; set; }
        public string LastModificationTimeUtc { get; set; }
        public string LastModifierUserName { get; set; }
        public string LastModifierUserEmail { get; set; }
    }
    //signing
    public class EsignRequestMultiAffiliateSigningInfoDto
    {
        public string FromAffiliate { get; set; }
        public long? AffiliateReferenceId { get; set; }
        public string Affiliate { get; set; } //Affiliate
        public string LastModifierUserName { get; set; }
        public string LastModifierUserEmail { get; set; }
        public string LastModificationTimeUtc { get; set; }
        public int? StatusId { get; set; }
        public string SignersJson { get; set; }
        public List<CreateOrEditSignersMultiAffiliateSigningInfoDto> Signers
        {
            get
            {
                return SignersJson == null ? null : JsonConvert.DeserializeObject<List<CreateOrEditSignersMultiAffiliateSigningInfoDto>>(SignersJson);
            }
        }
        public string DocumentsJson { get; set; }
        public List<CreateOrEditDocumentMultiAffiliateSigningInfoDto> Documents
        {
            get
            {
                return DocumentsJson == null ? null : JsonConvert.DeserializeObject<List<CreateOrEditDocumentMultiAffiliateSigningInfoDto>>(DocumentsJson);
            }
        }
        public string ActivitiesJson { get; set; }
        public List<CreateOrEditActivitiesMultiAffiliateDto> Activities
        {
            get
            {
                return ActivitiesJson == null ? null : JsonConvert.DeserializeObject<List<CreateOrEditActivitiesMultiAffiliateDto>>(ActivitiesJson);
            }
        }
    }
    public class CreateOrEditSignersMultiAffiliateSigningInfoDto
    {
        public long? AffiliateReferenceId { get; set; }
        public string Affiliate { get; set; }
        public string UserName { get; set; }
        public int? StatusId { get; set; }
        public string Email { get; set; }
        public string RequestDateUtc { get; set; }
        public string LastModificationTimeUtc { get; set; }
        public string LastModifierUserName { get; set; }
        public string LastModifierUserEmail { get; set; }
    }
    public class CreateOrEditDocumentMultiAffiliateSigningInfoDto
    {
        public long? AffiliateReferenceId { get; set; }
        public string DocumentPath { get; set; }
        public string DocumentDataBase64 { get; set; }
        public string DocumentViewDataBase64 { get; set; }
        public string DocumentOriginalDataBase64 { get; set; }
        public int TotalSize { get; set; }
        public string EncryptedUserPassBase64 { get; set; }
        public string SecretKeyBase64 { get; set; }
        public string LastModificationTimeUtc { get; set; }
        public string LastModifierUserName { get; set; }
        public string LastModifierUserEmail { get; set; }
    }
    //reject
    public class EsignRequestMultiAffiliateRejectInfoDto
    {
        public string FromAffiliate { get; set; }
        public long? AffiliateReferenceId { get; set; }
        public string Affiliate { get; set; } //Affiliate
        public string LastModifierUserName { get; set; }
        public string LastModifierUserEmail { get; set; }
        public string LastModificationTimeUtc { get; set; }
        public int? StatusId { get; set; }
        public string SignersJson { get; set; }
        public List<CreateOrEditSignersMultiAffiliateRejectInfoDto> Signers
        {
            get
            {
                return SignersJson == null ? null : JsonConvert.DeserializeObject<List<CreateOrEditSignersMultiAffiliateRejectInfoDto>>(SignersJson);
            }
        }
        public string ActivitiesJson { get; set; }
        public List<CreateOrEditActivitiesMultiAffiliateDto> Activities
        {
            get
            {
                return ActivitiesJson == null ? null : JsonConvert.DeserializeObject<List<CreateOrEditActivitiesMultiAffiliateDto>>(ActivitiesJson);
            }
        }
        public string CommentsJson { get; set; }
        public List<CreateOrEditCommentsMultiAffiliateDto> Comments
        {
            get
            {
                return CommentsJson == null ? null : JsonConvert.DeserializeObject<List<CreateOrEditCommentsMultiAffiliateDto>>(CommentsJson);
            }
        }
    }
    public class CreateOrEditSignersMultiAffiliateRejectInfoDto
    {
        public long? AffiliateReferenceId { get; set; }
        public string Affiliate { get; set; }
        public string UserName { get; set; }
        public int? StatusId { get; set; }
        public string Email { get; set; }
        public string LastModificationTimeUtc { get; set; }
        public string LastModifierUserName { get; set; }
        public string LastModifierUserEmail { get; set; }
        public string StatusSignersJson { get; set; }
        public List<CreateOrEditStatusSignersMultiAffiliateDto> StatusSigners
        {
            get
            {
                return StatusSignersJson == null ? null : JsonConvert.DeserializeObject<List<CreateOrEditStatusSignersMultiAffiliateDto>>(StatusSignersJson);
            }
        }
    }
    //add comment
    public class EsignRequestMultiAffiliateCommentInfoDto
    {
        public string FromAffiliate { get; set; }
        public long? AffiliateReferenceId { get; set; }
        public string Affiliate { get; set; } //Affiliate        
        public string CommentsJson { get; set; }
        public List<CreateOrEditCommentsMultiAffiliateDto> Comments
        {
            get
            {
                return CommentsJson == null ? null : JsonConvert.DeserializeObject<List<CreateOrEditCommentsMultiAffiliateDto>>(CommentsJson);
            }
        }
    }
    //reassign
    public class EsignRequestMultiAffiliateReassignInfoDto
    {
        public string FromAffiliate { get; set; }
        public long? AffiliateReferenceId { get; set; }
        public string Affiliate { get; set; } //Affiliate
        public string LastModifierUserName { get; set; }
        public string LastModifierUserEmail { get; set; }
        public string LastModificationTimeUtc { get; set; }
        public string AddCC { get; set; }
        public string SignersJson { get; set; }
        public List<CreateOrEditSignersMultiAffiliateReassignInfoDto> Signers
        {
            get
            {
                return SignersJson == null ? null : JsonConvert.DeserializeObject<List<CreateOrEditSignersMultiAffiliateReassignInfoDto>>(SignersJson);
            }
        }
        public string DocumentsJson { get; set; }
        public List<CreateOrEditDocumentMultiAffiliateReassignInfoDto> Documents
        {
            get
            {
                return DocumentsJson == null ? null : JsonConvert.DeserializeObject<List<CreateOrEditDocumentMultiAffiliateReassignInfoDto>>(DocumentsJson);
            }
        }
        public string ActivitiesJson { get; set; }
        public List<CreateOrEditActivitiesMultiAffiliateDto> Activities
        {
            get
            {
                return ActivitiesJson == null ? null : JsonConvert.DeserializeObject<List<CreateOrEditActivitiesMultiAffiliateDto>>(ActivitiesJson);
            }
        }
        public string CommentsJson { get; set; }
        public List<CreateOrEditCommentsMultiAffiliateDto> Comments
        {
            get
            {
                return CommentsJson == null ? null : JsonConvert.DeserializeObject<List<CreateOrEditCommentsMultiAffiliateDto>>(CommentsJson);
            }
        }
    }
    public class CreateOrEditSignersMultiAffiliateReassignInfoDto
    {
        public long? AffiliateReferenceId { get; set; }
        public string Affiliate { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string FullName { get; set; }
        public string Title { get; set; }
        public string TitleFullName { get; set; }
        public string Department { get; set; }
        public string Division { get; set; }
        public string ImageUrl { get; set; }
        public string ImageDataBase64 { get; set; }
        public string LastModificationTimeUtc { get; set; }
        public string LastModifierUserName { get; set; }
        public string LastModifierUserEmail { get; set; }
        public string RequestDateUtc { get; set; }
        public string TransferSignersJson { get; set; }
        public List<CreateOrEditEsignTransferSignerMultiAffiliateDto> TransferSigners
        {
            get
            {
                return TransferSignersJson == null ? null : JsonConvert.DeserializeObject<List<CreateOrEditEsignTransferSignerMultiAffiliateDto>>(TransferSignersJson);
            }
        }
    }
    public class CreateOrEditDocumentMultiAffiliateReassignInfoDto
    {
        public long? AffiliateReferenceId { get; set; }
        public string DocumentPath { get; set; }
        public string DocumentDataBase64 { get; set; }
        public string DocumentViewDataBase64 { get; set; }
        public string DocumentOriginalDataBase64 { get; set; }
        public int TotalSize { get; set; }
        public string LastModificationTimeUtc { get; set; }
        public string LastModifierUserName { get; set; }
        public string LastModifierUserEmail { get; set; }
        public string PositionsJson { get; set; }
        public List<CreateOrEditPositionsMultiAffiliateReassignDto> Positions
        {
            get
            {
                return PositionsJson == null ? null : JsonConvert.DeserializeObject<List<CreateOrEditPositionsMultiAffiliateReassignDto>>(PositionsJson);
            }
        }
    }
    public class CreateOrEditPositionsMultiAffiliateReassignDto
    {
        public long? AffiliateReferenceId { get; set; }
        public string SignerUserName { get; set; }
        public string SignerUserEmail { get; set; }
        public string TextValue { get; set; }
        public string LastModificationTimeUtc { get; set; }
        public string LastModifierUserName { get; set; }
        public string LastModifierUserEmail { get; set; }
    }
    //remind
    public class EsignRequestMultiAffiliateRemindInfoDto
    {
        public string FromAffiliate { get; set; }
        public long? AffiliateReferenceId { get; set; }
        public string Affiliate { get; set; } //Affiliate        
        public string ActivitiesJson { get; set; }
        public List<CreateOrEditActivitiesMultiAffiliateDto> Activities
        {
            get
            {
                return ActivitiesJson == null ? null : JsonConvert.DeserializeObject<List<CreateOrEditActivitiesMultiAffiliateDto>>(ActivitiesJson);
            }
        }
    }
    //revoke
    public class EsignRequestMultiAffiliateRevokeInfoDto
    {
        public string FromAffiliate { get; set; }
        public long? AffiliateReferenceId { get; set; }
        public string Affiliate { get; set; } //Affiliate
        public string LastModifierUserName { get; set; }
        public string LastModifierUserEmail { get; set; }
        public string LastModificationTimeUtc { get; set; }
        public int? StatusId { get; set; }
        public string SignersJson { get; set; }
        public List<CreateOrEditSignersMultiAffiliateRevokeInfoDto> Signers
        {
            get
            {
                return SignersJson == null ? null : JsonConvert.DeserializeObject<List<CreateOrEditSignersMultiAffiliateRevokeInfoDto>>(SignersJson);
            }
        }
        public string ActivitiesJson { get; set; }
        public List<CreateOrEditActivitiesMultiAffiliateDto> Activities
        {
            get
            {
                return ActivitiesJson == null ? null : JsonConvert.DeserializeObject<List<CreateOrEditActivitiesMultiAffiliateDto>>(ActivitiesJson);
            }
        }
    }
    public class CreateOrEditSignersMultiAffiliateRevokeInfoDto
    {
        public long? AffiliateReferenceId { get; set; }
        public string Affiliate { get; set; }
        public string UserName { get; set; }
        public int? StatusId { get; set; }
        public string Email { get; set; }
        public string LastModificationTimeUtc { get; set; }
        public string LastModifierUserName { get; set; }
        public string LastModifierUserEmail { get; set; }
        public string StatusSignersJson { get; set; }
        public List<CreateOrEditStatusSignersMultiAffiliateDto> StatusSigners
        {
            get
            {
                return StatusSignersJson == null ? null : JsonConvert.DeserializeObject<List<CreateOrEditStatusSignersMultiAffiliateDto>>(StatusSignersJson);
            }
        }
    }
    //transfer
    public class EsignRequestMultiAffiliateTransferInfoDto
    {
        public string FromAffiliate { get; set; }
        public long? AffiliateReferenceId { get; set; }
        public string Affiliate { get; set; } //Affiliate
        public string LastModifierUserName { get; set; }
        public string LastModifierUserEmail { get; set; }
        public string LastModificationTimeUtc { get; set; }
        public string CreatorUserName { get; set; }
        public string CreatorUserEmail { get; set; }
        public string AddCC { get; set; }
        public string SignersJson { get; set; }
        public List<CreateOrEditSignersMultiAffiliateTransferInfoDto> Signers
        {
            get
            {
                return SignersJson == null ? null : JsonConvert.DeserializeObject<List<CreateOrEditSignersMultiAffiliateTransferInfoDto>>(SignersJson);
            }
        }
        public string DocumentsJson { get; set; }
        public List<CreateOrEditDocumentMultiAffiliateTransferInfoDto> Documents
        {
            get
            {
                return DocumentsJson == null ? null : JsonConvert.DeserializeObject<List<CreateOrEditDocumentMultiAffiliateTransferInfoDto>>(DocumentsJson);
            }
        }
        public string ActivitiesJson { get; set; }
        public List<CreateOrEditActivitiesMultiAffiliateDto> Activities
        {
            get
            {
                return ActivitiesJson == null ? null : JsonConvert.DeserializeObject<List<CreateOrEditActivitiesMultiAffiliateDto>>(ActivitiesJson);
            }
        }
    }
    public class CreateOrEditSignersMultiAffiliateTransferInfoDto
    {
        public long? AffiliateReferenceId { get; set; }
        public string Affiliate { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string FullName { get; set; }
        public string Title { get; set; }
        public string TitleFullName { get; set; }
        public string Department { get; set; }
        public string Division { get; set; }
        public string ImageUrl { get; set; }
        public string ImageDataBase64 { get; set; }
        public string LastModificationTimeUtc { get; set; }
        public string LastModifierUserName { get; set; }
        public string LastModifierUserEmail { get; set; }
        public string RequestDateUtc { get; set; }
        public string TransferSignersJson { get; set; }
        public List<CreateOrEditEsignTransferSignerMultiAffiliateDto> TransferSigners
        {
            get
            {
                return TransferSignersJson == null ? null : JsonConvert.DeserializeObject<List<CreateOrEditEsignTransferSignerMultiAffiliateDto>>(TransferSignersJson);
            }
        }
    }
    public class CreateOrEditDocumentMultiAffiliateTransferInfoDto
    {
        public long? AffiliateReferenceId { get; set; }
        public string DocumentPath { get; set; }
        public string DocumentDataBase64 { get; set; }
        public string DocumentViewDataBase64 { get; set; }
        public string DocumentOriginalDataBase64 { get; set; }
        public int TotalSize { get; set; }
        public string LastModificationTimeUtc { get; set; }
        public string LastModifierUserName { get; set; }
        public string LastModifierUserEmail { get; set; }
        public string PositionsJson { get; set; }
        public List<CreateOrEditPositionsMultiAffiliateTransferDto> Positions
        {
            get
            {
                return PositionsJson == null ? null : JsonConvert.DeserializeObject<List<CreateOrEditPositionsMultiAffiliateTransferDto>>(PositionsJson);
            }
        }
    }
    public class CreateOrEditPositionsMultiAffiliateTransferDto
    {
        public long? AffiliateReferenceId { get; set; }
        public string SignerUserName { get; set; }
        public string SignerUserEmail { get; set; }
        public string TextValue { get; set; }
        public string LastModificationTimeUtc { get; set; }
        public string LastModifierUserName { get; set; }
        public string LastModifierUserEmail { get; set; }
    }
    //share
    public class EsignRequestMultiAffiliateShareInfoDto
    {
        public string FromAffiliate { get; set; }
        public long? AffiliateReferenceId { get; set; }
        public string Affiliate { get; set; } //Affiliate
        public string LastModifierUserName { get; set; }
        public string LastModifierUserEmail { get; set; }
        public string LastModificationTimeUtc { get; set; }
        public string AddCC { get; set; }
        public string ActivitiesJson { get; set; }
        public List<CreateOrEditActivitiesMultiAffiliateDto> Activities
        {
            get
            {
                return ActivitiesJson == null ? null : JsonConvert.DeserializeObject<List<CreateOrEditActivitiesMultiAffiliateDto>>(ActivitiesJson);
            }
        }
    }
    //additionfile
    public class EsignRequestMultiAffiliateAdditionFileInfoDto
    {
        public string FromAffiliate { get; set; }
        public long? AffiliateReferenceId { get; set; }
        public string Affiliate { get; set; } //Affiliate
        public string SignersJson { get; set; }
        public List<CreateOrEditSignersMultiAffiliateAdditionFileInfoDto> Signers
        {
            get
            {
                return SignersJson == null ? null : JsonConvert.DeserializeObject<List<CreateOrEditSignersMultiAffiliateAdditionFileInfoDto>>(SignersJson);
            }
        }
        public string DocumentsJson { get; set; }
        public List<CreateOrEditDocumentMultiAffiliateAdditionFileInfoDto> Documents
        {
            get
            {
                return DocumentsJson == null ? null : JsonConvert.DeserializeObject<List<CreateOrEditDocumentMultiAffiliateAdditionFileInfoDto>>(DocumentsJson);
            }
        }
        public string ActivitiesJson { get; set; }
        public List<CreateOrEditActivitiesMultiAffiliateDto> Activities
        {
            get
            {
                return ActivitiesJson == null ? null : JsonConvert.DeserializeObject<List<CreateOrEditActivitiesMultiAffiliateDto>>(ActivitiesJson);
            }
        }
    }
    public class CreateOrEditSignersMultiAffiliateAdditionFileInfoDto
    {
        public long? AffiliateReferenceId { get; set; }
        public string Affiliate { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string RequestDateUtc { get; set; }
        public string LastModificationTimeUtc { get; set; }
        public string LastModifierUserName { get; set; }
        public string LastModifierUserEmail { get; set; }
    }
    public class CreateOrEditDocumentMultiAffiliateAdditionFileInfoDto
    {
        public long? AffiliateReferenceId { get; set; }
        public string DocumentPath { get; set; }
        public string DocumentDataBase64 { get; set; }
        public string DocumentViewDataBase64 { get; set; }
        public string DocumentOriginalDataBase64 { get; set; }
        public string DocumentName { get; set; }
        public string Md5Hash { get; set; }
        public int DocumentOrder { get; set; }
        public string QrRandomCode { get; set; }
        public int TotalPage { get; set; }
        public int TotalSize { get; set; }
        public string EncryptedUserPassBase64 { get; set; }
        public string SecretKeyBase64 { get; set; }
        public bool IsAdditionalFile { get; set; }
        public bool IsDigitalSignatureFile { get; set; }
        public string CreatorUserName { get; set; }
        public string CreatorUserEmail { get; set; }
        public string CreationTimeUtc { get; set; }
        public string LastModificationTimeUtc { get; set; }
        public string LastModifierUserName { get; set; }
        public string LastModifierUserEmail { get; set; }
    }
}


