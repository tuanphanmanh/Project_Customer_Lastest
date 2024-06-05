using System;
using System.Collections.Generic;
using System.Text;

namespace esign.Esign.Business.EsignRequestWeb.Dto.Ver1
{
    public class SignerSignDto
    {
        public long SignerId { get; set; }
        public long RequestId { get; set; }
        public byte[] ImageSign { get; set; }
    }
}
