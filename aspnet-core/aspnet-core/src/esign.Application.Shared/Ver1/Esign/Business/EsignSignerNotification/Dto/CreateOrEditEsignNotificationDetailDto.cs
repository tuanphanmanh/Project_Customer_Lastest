using System;
using System.Collections.Generic;
using System.Text;

namespace esign.Esign.Business.EsignSignerNotification.Dto.Ver1
{
    public class CreateOrEditEsignNotificationDetailDto
    {
        public string Content { get; set; }
        public string HyperlinkUrl { get; set; }
        public bool IsBold { get; set; }
        public bool IsUnderline { get; set; }
        public bool IsItalic { get; set; }
    }
}
