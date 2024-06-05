using System;
using System.Collections.Generic;
using System.Text;

namespace esign.Ver1.Esign.Business.EsignApiOtherSystem.Dto
{
    public class RejectFromOtherSystemDto
    {
        public long? RequestRefId { get; set; }
        public string SystemCode { get; set; }
        public string ReferenceType { get; set; }
        public string UserNameReject { get; set; }
        public string Note { get; set; }
    }
}
