using Castle.MicroKernel.SubSystems.Conversion;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text;
using esign.Esign.Business.EsignDocumentList.Dto.Ver1;
using esign.Esign.Business.EsignSignerList.Dto.Ver1;
using esign.Business.Dto.Ver1;

namespace esign.Esign.Business.EsignRequest.Dto.Ver1
{
    public class CreateOrEditEsignRequestDto
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
        public int SystemId { get; set; }
        public long? TypeSignId { get; set; }
        public long? TemplateSignatureId { get; set; }
        public string ImageSign { get; set; }
        public bool? IsSave { get; set; }
        public long? ImgHeight { get; set; }
        public long? Imgwidth { get; set; }
        public List<CreatOrEditEsignReferenceRequestDto> RequestRefs { get; set; }
        public List<int> ListCategoryId { get; set; }
        public List<CreateOrEditSignersDto> Signers { get; set; }
        public List<CreateOrEditDocumentDto> Documents { get; set; }

        public bool IsDigitalSignatureSubmitAnyway { get; set; }
    }

    public class CreateShareRequestDto
    {
        public string ListUserId { get; set; }
        public long RequestId { get; set; }
    }

    public class CreateShareRequest_WebDto
    {
        public List<long> ListUserId { get; set; }
        public long RequestId { get; set; }
    }
    
    public class CreateRemindRequestDto
    {
        public string Content { get; set; }
        public long RequestId { get; set; }
    }

    public class ValidateDigitalSignatureResultDto
    {
        public string Code { get; set; }
        public string ErrMsgDigitalSignature { get; set; }
        public string ErrMsgDigitalSignatureExpired { get; set; }
    }
}
