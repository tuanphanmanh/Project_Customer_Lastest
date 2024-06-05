using System;
using System.Collections.Generic;
using System.Text;

namespace esign.SendEmail.Dto.Ver1
{
    public class EmailContentDto
    {
        public List<string> ReceiveEmail { get; set; }
        public string Subject { get; set; }
        public string ContentEmail { get; set; }
        public string CCEmail { get; set; }
        public string BCCEmail { get; set; }
        public List<string> FilePath { get; set; }
    }
}
