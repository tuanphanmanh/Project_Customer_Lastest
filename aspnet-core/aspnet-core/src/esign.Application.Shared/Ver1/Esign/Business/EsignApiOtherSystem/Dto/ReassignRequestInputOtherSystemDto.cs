using System;
using System.Collections.Generic;
using System.Text;

namespace esign.Ver1.Esign.Business.EsignApiOtherSystem.Dto
{
    public class ReassignRequestInputOtherSystemDto
    {
        public long RequestId { get; set; }
        public long ReferenceRequestId { get; set; }
        public long ReAssignUserId { get; set; }
        public string ReferenceRequestType { get; set; }
        public long ReferenceSignerId { get; set; }
        public string CurrentUserName { get; set; }
        public string ForwardUserName { get; set; }
        public string Note { get; set; }
    }
}
