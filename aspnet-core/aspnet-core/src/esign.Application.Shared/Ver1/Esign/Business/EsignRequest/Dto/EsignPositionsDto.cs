using System;
using System.Collections.Generic;
using System.Text;

namespace esign.Esign.Business.EsignRequest.Dto.Ver1
{
    public class EsignPositionsDto
    {
        public long Id { get; set; }
        public long? DocumentId { get; set; }
        public long? PageNum { get; set; }
        public long SignerId { get; set; }
        public string SignatureImage { get; set; }
        public long? TempSignatureDefaultId { get; set; }
        public string Content { get; set; }
        public long? PositionX { get; set; }
        public long? PositionY { get; set; }
        public long? PositionW { get; set; }
        public long? PositionH { get; set; }
        public bool? IsDigitalSignature { get; set; }
        public long? TypeId { get; set; }
    }

    public class EsignSignaturePageDto
    {
        public long? RequestId { get; set; }
        public long? DocumentId { get; set; }
        public int? PageNum { get; set; }
        public long SignerId { get; set; }  
        public bool? IsDigitalSignature { get; set; }
        public int? TypeId { get; set; }
        public int? Qty { get; set; }
        public string Code { get; set; } // color
        

    }

    public class EsignPositionsWebDto : EsignPositionsDto
    {
        public string BackgroundColor { get; set; }
        public string TextName { get; set; }
        public string TextValue { get; set; }
        public bool? IsBold { get; set; }
        public bool? IsItalic { get; set; }
        public bool? IsUnderline { get; set; }
        public string TextAlignment { get; set; }
        public long Rotate { get; set; }

    }
}
