﻿using System;
using System.Collections.Generic;
using System.Text;

namespace esign.Ver1.Esign.Business.EsignApiOtherSystem.Dto
{
    public class RevokeRequestOrtherSystemInputDto
    {
        public string UserName { get; set; }
        public long ReferenceId { get; set; }
        public string ReferenceType { get; set; }
        public string SystemCode { get; set; }
        public string Note { get; set; }
    }
}