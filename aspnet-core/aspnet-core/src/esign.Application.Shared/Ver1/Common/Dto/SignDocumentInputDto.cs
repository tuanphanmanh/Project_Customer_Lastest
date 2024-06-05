using System;
using System.Collections.Generic;
using System.Text;

namespace esign.Common.Dto.Ver1
{
    public class SignDocumentInputDto
    {
        public long? TypeSignId { get; set; }
        public long? TemplateSignatureId { get; set; }
        public long RequestId { get; set; }
        public long CurrentUserId { get; set; }
        public byte[] ImageSign { get; set; }
        public bool? IsSave { get; set; }
        public string ImgHeight { get; set; }
        public string Imgwidth { get; set; }
    }
}
