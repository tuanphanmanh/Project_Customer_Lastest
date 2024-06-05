using System;
using System.Collections.Generic;
using System.Text;

namespace esign.Common.Dto.Ver1
{
    public class SignatureImageAndPositionDto
    {
        public long Id { get; set; }
        public long SignatureId { get; set; }
        public string SignatureName { get; set; }
        public string SignaturePath { get; set; }
        public long? DocumentId { get; set; }
        public int? PageNum { get; set; }
        public long? PositionX { get; set; }
        public long? PositionY { get; set; }
        public long? PositionW { get; set; }
        public long? PositionH { get; set; }
        public long? Rotate { get; set; }
        public bool? IsDigitalSignature { get; set; }
        public long? TypeId { get; set; }
        public long? StatusId { get; set; }
        public byte[] UserSignature { get; set; }
        public string StatusCode { get; set; }
        public int? FontSize { get; set; }
        public string TextValue { get; set; }
        public string TextName { get; set; }
        public string FontFamily { get; set; }
        public string Color { get; set; }
        public string BackGroundColor { get; set; }
        public string TextAlignment { get; set; }
        public bool IsBold { get; set; }
        public bool IsItalic { get; set; }
        public bool IsUnderline { get; set; }
    }
}
