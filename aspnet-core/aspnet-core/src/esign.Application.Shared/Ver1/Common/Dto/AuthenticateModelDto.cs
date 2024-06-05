using System;
using System.Collections.Generic;
using System.Text;

namespace esign.Ver1.Common.Dto
{
    public class AuthenticateModelDto
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string TenancyName { get; set; }
    }
}
