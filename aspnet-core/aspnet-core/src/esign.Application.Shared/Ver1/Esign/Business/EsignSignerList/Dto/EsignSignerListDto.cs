using Abp.Application.Services.Dto;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace esign.Business.Dto.Ver1
{
    public class EsignSignerListForAttachmentWrapperDto
    {
        public List<EsignSignerListDto> ListSigners { get; set; }
    }

    public class EsignSignerListDto : EntityDto<long>
    {
        public long? UserId { get; set; }
        public int SigningOrder { get; set; }
        public string PrivateMessage { get; set; }
        [StringLength(50)]
        public string FullName { get; set; }
        [StringLength(50)]
        public string Title { get; set; }
        [StringLength(500)]
        public string ImgUrl { get; set; }
        public string ImageUrl { get; set; }
        public string StatusCode { get; set; }
        public string Color { get; set; }
        public string Email { get; set; }
        public bool? RequiresDigitalSignature { get; set; }

    }
    public class UpdateStatusInputDto
    {
        public long StatusId { get; set; }

        [StringLength(200)]
        public string Note { get; set; }
        public long RequestId { get; set; }
    }

    public class RevokeInputDto
    {
        public string Note { get; set; }
        public long RequestId { get; set; }
        public long UserId { get; set; }
    }

    public class RejectInputDto
    {
        public string Note { get; set; }
        public long RequestId { get; set; }
    }

    public class CloneToDraftRequest
    {
        public long RequestId { get; set; }
    }


    public class RemindInputDto
    {
        public long RequestId { get; set; }
        public string Note { get; set; } 
    }

    public class TransferInputDto
    {
        public string Note { get; set; }
        public long TransferUserId { get; set; }
        public List<long> RequestId { get; set; }
    }

    public class TransferInputMobileDto
    {
        public string Note { get; set; }
        public long TransferUserId { get; set; }
        public string RequestId { get; set; }
    }

    public class ShareInputDto
    {
        public List<long> ListUserId { get; set; }
        public long RequestId { get; set; }
    }

    public class ReAssignInputDto
    {
        public string Note { get; set; }
        public long ReAssignUserId { get; set; }
        public long RequestId { get; set; }
    }
    public class ResultReAssignDto
    {
        public string EmailReassign { get; set; }
        public string EmailBCC { get; set; } 
        public string EmailCc { get; set; }
        public string ListUserNoti { get; set; }

    }
    public class ResultRemindDto
    { 
        public string RemainEmails { get; set; }
        public string RemainIds { get; set; }
        public string ListUserNoti { get; set; }
    }

    public class ErrRevokeMessageDto
    {
        public string ErrRevokeMessage { get; set; }
        public string EmailRevoke { get; set; } 
        public string IdRevoke { get; set; } 
        public string EmailBCC { get; set; }
        public string IdBCC { get; set; }
        public string IdCC { get; set; }
        public string ListUserNoti { get; set; }

    }

    public class ResultTransferDto
    {
        public int ResultTransfer { get; set; }

        public string ListUserNoti { get; set; }
    }


    
        public class CloneToDraftDto
        {
        public long RequestId { get; set; }
    }


    public class ResultRejectDto
    { 
        public string IdPreviousSignerReject { get; set; }
        public string EmailPreviousSignerReject { get; set; }
        public string ListUserNoti { get;}
    }
}


