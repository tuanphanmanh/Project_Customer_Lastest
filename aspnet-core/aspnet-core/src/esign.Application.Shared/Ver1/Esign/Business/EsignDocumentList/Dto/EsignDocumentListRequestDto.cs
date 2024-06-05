using esign.Esign.Business.EsignRequest.Dto.Ver1;
using System;
using System.Collections.Generic;
using System.Text;

namespace esign.Esign.Business.EsignDocumentList.Dto.Ver1
{
    public class EsignDocumentListRequestDto
    {
        public long Id { get; set; }
        public string DocumentName { get; set; }
        public string DocumentPath { get; set; }
        public int? DocumentOrder { get; set; }
        public int? TotalPage { get; set; }
        public int? TotalSize { get; set; }        

        public List<EsignPositionsDto> Positions { get; set; }
    }

    public class EsignDocumentListWebRequestDto : EsignDocumentListRequestDto
    {
        public List<EsignPositionsWebDto> PositionsWeb { get; set; }
    }
}
