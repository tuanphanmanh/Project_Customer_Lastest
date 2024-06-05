using Abp.Application.Services.Dto;
using esign.Dto;
using System;
using System.Collections.Generic;

namespace esign.Business.Ver1
{

    public class EsignRequestBySystemIdAllWebDto
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
        public bool IsRead { get; set; }
        public long? RequesterId { get; set; }
        public string CreationTime { get; set; }
        public int? SigningOrder { get; set; }
        public string LastModificationTime { get; set; }
        public string IsSigned { get; set; }
 
        public DateTime? ExpectedDate { get; set; }  // update 16/02/2024 add ExpectedDate
 
    }
    public class EsignRequestByIdForSelectedItemWebOutputDto
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
        public bool IsRead { get; set; }
        public long? RequesterId { get; set; }
        public string CreationTime { get; set; }
        public int? SigningOrder { get; set; }
        public string LastModificationTime { get; set; }
        public string IsSigned { get; set; }

        public int typeFilter { get; set; } 
    }

    
    

    public class EsignRequestBySystemIdWebDto
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
        public List<EsignRequestListSignerBySystemIdWebDto> ListSignerBySystemIdDto { get; set; }
        public bool IsRead { get; set; }
        public string CreationTime { get; set; }
        public string LastModificationTime { get; set; }
        public string IsSigned { get; set; }

        public int typeFilter { get; set; } // dùng cho lấy request detail đang ở tab nào
 
        public DateTime? ExpectedDate { get; set; }  // update 16/02/2024 add ExpectedDate
 
    }
    
    
    public class EsignRequestListSignerBySystemIdWebDto
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string SigningOrder { get; set; }
        public string Division { get; set; } 
        public string StatusCode { get; set; }
        public string StatusName { get; set; }
        public string ImgUrl { get; set; }
    }

    public class EsignRequestBySystemIdInputWebDto : PagedInputDto
    {
        public int TypeId { get; set; }
        public int SystemId { get; set; }
        public string StatusCode { get; set; }
        public string SearchValue { get; set; }
        public int SearchType { get; set; }
        public int IsFollowUp { get; set; }
        public long? RequesterId { get; set; }
        public string OrderCreationTime { get; set; } // RequestDate - CreationTime(Draft)
        public string OrderModifyTime { get; set; }
    }
 
 


    public class GetDataForDashboardDto
    {
        public int TotalActionRequired { get; set; }
        public int TotalWaitingForOther { get; set; }
        public int TotalTransfer { get; set; }
        public int TotalRejectComplete { get; set; }
        public List<GetRequestDetailDashboard> ListActionRequired { get; set; }
        public List<GetRequestDetailDashboard> ListWaitingForOther { get; set; }
        public List<GetRequestDetailDashboard> ListTransfer { get; set; }
        public List<GetRequestDetailDashboard> ListRejectComplete { get; set; }
    }

    public class GetRequestDetailDashboard
    {
        public long CreatedRequesterId { get; set; }
        public string FromRequester { get; set; }
        public string RequesterImgUrl { get; set; }
        public bool IsRead { get; set; }
        public bool IsFollowUp { get; set; }
        public bool IsTransfer { get; set; }
        public string Title { get; set; }
        public DateTime? RequestDate { get; set; }
        public IEnumerable<EsignRequestListSignerBySystemIdWebDto> SignerList { get; set; }
        public IEnumerable<EsignRequestListSignerBySystemIdWebDto> SignerListSign { get; set; }
        public long RequestId { get; set; }
        public int TotalSignerCount { get; set; } 
        public string CC { get; set; }
        public string StatusCode { get; set; }

        public DateTime? ExpectedDate { get; set; }  // update 16/02/2024 add ExpectedDate
    }

    public class TransferHistoryOutputDto : EntityDto<long>
    {
        public long RequestId { get; set; }
        public string RequestTitle { get; set; }
        public long FromUserId { get; set; }
        public string FromUser { get; set; }
        public long ToUserId { get; set; }
        public string ToUser { get; set; }
        public string TransferStatus { get; set; }
        public DateTime CreationTime { get; set; }
        public string Note { get; set; }
    }

    public class TransferHistoryInput: PagedInputDto
    {
        public string Title { get; set; }
        public TransferType Type { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }
}


