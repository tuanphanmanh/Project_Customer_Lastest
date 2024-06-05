using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace esign.Business.Dto.Ver1
{
    public class EsignReferenceRequestDto
    {
        public long? RequestId { get; set; }
        public long? RequestRefId { get; set; }
        public string Title { get; set; }
        public long? Id { get; set; }
        public string Note { get; set; }
        public bool IsAdditionalDoc { get; set; }
    }

    public class CreatOrEditEsignReferenceRequestDto : EntityDto<int?>
    {
        public long? RequestId { get; set; }
        public long? RequestRefId { get; set; }
        public long? Id { get; set; }
        [StringLength(500)]
        public string Note { get; set; }
        public bool IsAddHistory { get; set; }
    }

    public class CreateNewReferenceRequestDto
    {
        [Required]
        public long RequestId { get; set; }
        public bool IsAddHistory { get; set; }
        [Required]
        public List<CreatEsignReferenceRequestDto> ReferenceRequests { get; set; }
    }

    public class CreatEsignReferenceRequestDto
    {
        [Required]
        public long RequestRefId { get; set; }
        [StringLength(500)]
        public string Note { get; set; }
    }
}
