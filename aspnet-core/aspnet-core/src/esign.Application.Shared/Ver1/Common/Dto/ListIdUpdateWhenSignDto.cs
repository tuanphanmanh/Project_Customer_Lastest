using esign.SendEmail.Dto.Ver1;
using System;
using System.Collections.Generic;
using System.Text;

namespace esign.Ver1.Common.Dto
{
    public class ListIdUpdateWhenSignDto
    {
        public List<ListSingerAndContentEmail> ListSigner { get; set; }
        public long CreatedUserId { get; set; } // send mail for Completed / Signed
        public long RequestId { get; set; } 
    }

    public class ListSingerAndContentEmail
    {
        public long SignerId { get; set; } 
        public EmailContentDto ContentEmail { get; set; }
    }


}
