using System;
using System.Collections.Generic;
using System.Text;

namespace esign.Common.Dto
{
    public class SignDocumentInputDto
    {
        public long? TypeSignId { get; set; }
        public long? TemplateSignatureId { get; set; }
        public long RequestId { get; set; }
        public byte[] ImageSign { get; set; }
    }
}
