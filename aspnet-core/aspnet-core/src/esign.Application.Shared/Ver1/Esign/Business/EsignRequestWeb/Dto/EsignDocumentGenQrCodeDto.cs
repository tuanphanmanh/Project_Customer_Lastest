using System;
using System.Collections.Generic;
using System.Text;

namespace esign.Ver1.Esign.Business.EsignRequestWeb.Dto
{
    public class EsignDocumentGenQrCodeDto
    {
        public long DocumentId { get; set; }
        public long AttachmentId { get; set; }
        public string RandomString { get; set; }

    }
}
