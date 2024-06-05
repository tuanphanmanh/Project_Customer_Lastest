using System;
using System.Collections.Generic;
using System.Text;

namespace esign.Common.Dto
{
    public class AuthorizeOtpSignatureDigitalDto
    {
        public string BillCode { get; set; }
        public string OtpCode { get; set; }
    }
}
