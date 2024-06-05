using System;
using System.Collections.Generic;
using System.Text;

namespace esign.Esign.Business.EsignDocumentList.Dto.Ver2
{
    /// <summary>
    /// 
    /// </summary>
    public class EsignDocumentListDownloadRequestDto
    {
        public string Hash { get; set; }
        public string UserPass { get; set; }

        public bool? IsDownload { get; set; }
    }
}
