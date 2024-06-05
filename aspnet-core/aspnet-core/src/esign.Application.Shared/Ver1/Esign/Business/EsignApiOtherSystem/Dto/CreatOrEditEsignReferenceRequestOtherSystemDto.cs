using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace esign.Ver1.Esign.Business.EsignApiOtherSystem.Dto
{
    public class CreatOrEditEsignReferenceRequestOtherSystemDto
    {
        public long? RequestId { get; set; }
        public long? RequestRefId { get; set; }
        [StringLength(500)]
        public string Note { get; set; }
        public bool IsAddHistory { get; set; }
    }
}
