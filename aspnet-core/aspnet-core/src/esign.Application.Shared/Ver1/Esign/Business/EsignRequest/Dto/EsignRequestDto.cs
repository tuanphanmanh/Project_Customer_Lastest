using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
using esign.Dto;
using Newtonsoft.Json;

namespace esign.Business.Dto.Ver1
{
    public class EsignRequestGetTotalCountRequestsDto
    {
        public virtual int TotalActionRequired { get; set; }
        public virtual int TotalWaitingForOthers { get; set; }
        public virtual int TotalUnreadNoti { get; set; }
    }

    public class EsignRequestDto : EntityDto<long>
    {
        [StringLength(200)]
        public string Title { get; set; }
        [Column(TypeName = "decimal(15, 3)")]
        public decimal? TotalCost { get; set; }
        [Column(TypeName = "decimal(3, 3)")]
        public decimal? Roi { get; set; }
        public DateTime? ProjectScheduleFrom { get; set; }
        public DateTime? ProjectScheduleTo { get; set; }
        [StringLength(200)]
        public string Content { get; set; }
        public string Message { get; set; }
        public string Category { get; set; }
        public string StatusCode { get; set; }
        public string PrivateMessage { get; set; }
        public string RequesterDepartment { get; set; }
        public string RequesterImgUrl { get; set; }
        public string FromRequester { get; set; }
        public string System { get; set; }
        public DateTime? RequestDate { get; set; }
        public DateTime? ExpectedDate { get; set; }
        public bool IsSummary { get; set; }
        public bool IsDigitalSignature { get; set; }
        public long? RequesterUserId { get; set; }
        public bool IsShared { get; set; }
        public bool IsTransfered { get; set; }
        public bool IsFollowUp { get; set; }
        public bool IsRequester { get; set; }
        public bool IsAddFile { get; set; }
        public bool IsEdit { get; set; }
        public bool IsDelete { get; set; }
        public bool IsSubmit { get; set; }
        public bool IsSignAndSubmit { get; set; }
        public bool IsRemind { get; set; }
        public bool IsRevoke { get; set; }
        public bool IsRefDoc { get; set; }
        public bool IsClone { get; set; }
        public bool IsReject { get; set; }
        public bool IsReassign { get; set; }
        public bool IsShare { get; set; }
        public bool IsSign { get; set; }
        public bool IsAddComment { get; set; }
        public bool IsQuickSign { get; set; }
        public string CategoryIds { get; set; }
        public List<int> CategoryList
        {
            get
            {
                return CategoryIds == null ? null : JsonConvert.DeserializeObject<List<int>>(CategoryIds);
            }
            set { }

        }
    }

    public class EsignRequestWebDto : EsignRequestDto
    {
        public string TransferFromUser { get; set; }
        public string TransferImgUrl { get; set; }
        public string TransferDepartment { get; set; }
        public string AddCC { get; set; }
    }

    public class EsignRequestBySystemIdInputDto : PagedInputDto
    {
        public int TypeId { get; set; }
        public int SystemId { get; set; }
        public string StatusCode { get; set; }

    }
    public class EsignRequestBySystemIdAllDto
    {
        public string FromRequester { get; set; }
        public bool IsFollowUp { get; set; }
        public long RequestId { get; set; }
        public bool IsShared { get; set; }
        public bool IsTransfer { get; set; }
        public DateTime? RequestDate { get; set; }
        public string Title { get; set; }
        public string StatusCode { get; set; }
        public string StatusName { get; set; }
        public string StatusCodeEsl { get; set; }
        public string StatusNameEsl { get; set; }
        public string ImgUrl { get; set; }
        public string Message { get; set; }
        public int TotalSignerCount { get; set; }
        public string RequesterImgUrl { get; set; }
        public bool IsReaded { get; set; }
        public int? SigningOrder { get; set; }
        public bool IsReject { get; set; }
        public bool IsSign { get; set; }
        public bool IsSigned { get; set; }
        public bool IsShare { get; set; }
        public bool IsViewHistory { get; set; }
        public bool IsDelete { get; set; }
        public bool IsRemind { get; set; }
        public bool IsEdit { get; set; }
        public bool IsSubmitOrSign { get; set; }
        public bool IsRevoke { get; set; }
        public bool IsClone { get; set; }
        public long? UserId { get; set; }

        public long RequesterId { get; set; }

        public DateTime? ExpectedDate { get; set; }  // update 16/02/2024 add ExpectedDate
    }

    public class EsignRequestBySystemIdDto
    {
        public string FromRequester { get; set; }
        public string RequesterImgUrl { get; set; }
        public bool IsFollowUp { get; set; }
        public long RequestId { get; set; }
        public bool IsShared { get; set; }
        public bool IsTransfer { get; set; }
        public DateTime? RequestDate { get; set; }
        public string Title { get; set; }
        public string StatusCode { get; set; }
        public string StatusName { get; set; }
        public string Message { get; set; }
        public int TotalSignerCount { get; set; }
        public bool IsReaded { get; set; }
        public bool IsReject { get; set; }
        public bool IsSign { get; set; }
        public bool IsSigned { get; set; }
        public bool IsShare { get; set; }
        public bool IsViewHistory { get; set; }
        public bool IsDelete { get; set; }
        public bool IsRemind { get; set; }
        public bool IsEdit { get; set; }
        public bool IsSubmitOrSign { get; set; }
        public bool IsRevoke { get; set; }
        public bool IsClone { get; set; }
        public long RequesterId { get; set; } 
        public DateTime? ExpectedDate { get; set; }  // update 16/02/2024 add ExpectedDate

        public List<EsignRequestListSignerBySystemIdDto> ListSignerBySystemIdDto { get; set; }
    }

    public class EsignRequestListSignerBySystemIdDto
    {
        public string StatusCode { get; set; }
        public string StatusName { get; set; }
        public string ImgUrl { get; set; }
        public long? UserId { get; set; }
    }

    public class EsignRequestGetMessageSignerNoSignatureDto
    {
        public int Result;
    }

    public class EsignRequestGetShareRequestNotiDto
    {
        public string ListUserNoti { get; set; }
    }
    public class EsignRequestBySearchValueAllDto
    {
        public string Division { get; set; }
        public string Documentname { get; set; }
        public string FromRequester { get; set; }
        public string RequesterImgUrl { get; set; }
        public bool IsShared { get; set; }
        public bool IsTransfer { get; set; }
        public bool IsFollowUp { get; set; }
        public bool IsRequester { get; set; }
        public long RequestId { get; set; }
        public DateTime? RequestDate { get; set; }
        public string Title { get; set; }
        public string StatusCode { get; set; }
        public string StatusName { get; set; }
        public string Message { get; set; }
        public int TotalSignerCount { get; set; }
        public string StatusCodeEsl { get; set; }
        public string StatusNameEsl { get; set; }
        public string ImgUrl { get; set; }

    }

    public class EsignRequestBySearchValueInputDto
    {
        public int ScreenId { get; set; }
        public int TypeId { get; set; }
        public string SearchValue { get; set; }
        public long UserId { get; set; }
        public long SystemId { get; set; }
        public long DivisionId { get; set; }
        public int SkipCount { get; set; }
        public int MaxResultCount { get; set; }

    }

    public class EsignRequestSearchValueDto
    {
        public long TotalCount { get; set; }

        public List<EsignRequestSearchValueListItemsDto> Items { get; set; }

    }

    public class EsignRequestSearchValueListItemsDto
    {
        public string Division { get; set; }
        public List<EsignRequestSearchValueListRequestDto> ListRequest { get; set; }

    }

    public class EsignRequestSearchValueListRequestDto
    {
        public string Documentname { get; set; }
        public string FromRequester { get; set; }
        public string RequesterImgUrl { get; set; }
        public bool IsShared { get; set; }
        public bool IsTransfer { get; set; }
        public bool IsFollowUp { get; set; }
        public bool IsRequester { get; set; }
        public long RequestId { get; set; }
        public DateTime? RequestDate { get; set; }
        public string Title { get; set; }
        public string StatusCode { get; set; }
        public string StatusName { get; set; }
        public string Message { get; set; }
        public int TotalSignerCount { get; set; }
        public List<EsignRequestSearchValueListSignerDto> ListSigner { get; set; }
    }

    public class EsignRequestSearchValueListSignerDto
    {
        public string StatusCode { get; set; }
        public string StatusName { get; set; }
        public string ImgUrl { get; set; }
    }

    public class EsignRequerstFieldDetailsDto
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public string BackColor { get; set; }
        public string Color { get; set; }
        //B, I, U
        public string FontStyle { get; set; }
        public int? FontSize { get; set; }
        public string FontFamily { get; set; }

        public float X { get; set; }
        public float Y { get; set; }
        public float W { get; set; }
        public float H { get; set; }

        //0 Signature 1: text
        public int? Type { get; set; }
        public string TextAlignment { get; set; }
        public int PageNum { get; set; }

    }

}


