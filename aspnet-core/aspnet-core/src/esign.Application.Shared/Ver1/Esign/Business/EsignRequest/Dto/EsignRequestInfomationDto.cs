using Castle.MicroKernel.SubSystems.Conversion;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text;
using esign.Business.Dto.Ver1;
using esign.Esign.Business.EsignDocumentList.Dto.Ver1;
using esign.Esign.Business.EsignSignerList.Dto.Ver1;

namespace esign.Esign.Business.EsignRequest.Dto.Ver1
{
    public class EsignRequestInfomationDto
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public decimal? TotalCost { get; set; }
        public decimal? Roi { get; set; }
        public DateTime? ProjectScheduleFrom { get; set; }
        public DateTime? ProjectScheduleTo { get; set; }
        public DateTime? RequestDate { get; set; }
        public string Content { get; set; }
        public string Message { get; set; }
        public string Category { get; set; }
        public string StatusCode { get; set; }
        public string System { get; set; }
        public string RequesterImgUrl { get; set; }
        public string FromRequester { get; set; }
        public string PrivateMessage { get; set; }
        public DateTime? ExpectedDate { get; set; }
        public bool? IsSummary { get; set; }
        public bool? IsDigitalSignature { get; set; }
        public List<EsignSignerForRequestDto> Signers { get; set; }
        public List<EsignDocumentListRequestDto> Documents { get; set; }
    }
}
