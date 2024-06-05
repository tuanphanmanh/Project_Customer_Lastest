using esign.Esign.Business.EsignRequest.Dto.Ver1;
using System;
using System.Collections.Generic;
using System.Text;

namespace esign.Esign.Business.EsignDocumentList.Dto.Ver1
{
    /// <summary>
    /// 
    /// </summary>
    public class EsignDocumentListDownloadRequestDto
    {
        public string Hash { get; set; }
        public string UserPass { get; set; }
    }
}
