using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace esign.Esign.Business.EsignRequest.Dto.Ver1
{
    public class CreateOrEditPositionsDto
    {
        public long Id { get; set; }
        public long? DocumentId { get; set; }
        public long? PageNum { get; set; }
        public long SignerId { get; set; }
        public string SignatureImage { get; set; }
        public long? PositionX { get; set; }
        public long? PositionY { get; set; }
        public long? PositionW { get; set; }
        public long? PositionH { get; set; }
        public long? Rotate { get; set; }
        public bool? IsDigitalSignature { get; set; }
        public long? TypeId { get; set; }
        public int? FontSize { get; set; }
        public string TextValue { get; set; }
        public string TextName { get; set; }
        public string FontFamily { get; set; }
        public string Color { get; set; }
        public string BackGroundColor { get; set; }
        public string TextAlignment { get; set; }
        public bool? IsBold { get; set; } = false;
        public bool? IsItalic { get; set; } = false;
        public bool? IsUnderline { get; set; } = false;
    }
}
