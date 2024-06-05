using esign.Esign.Business.EsignRequest.Dto.Ver1;
using System;
using System.Collections.Generic;
using System.Text;

namespace esign.Esign.Business.EsignDocumentList.Dto.Ver1
{
    public class EsignDocumentListDto
    {
        public long Id { get; set; }
		public string DocumentName {get; set;}
		public string DocumentPath {get; set;}
		public bool? IsSignRequest { get; set;}
		public bool? IsDownload { get; set;}
		public int? DocumentOrder {get; set;}
		public int? TotalPage {get; set;}
		public int? TotalSize {get; set;}
        public bool IsAdditionalFile { get; set; }
        public bool IsDigitalSignatureFile { get; set; }

    }
    public class EsignDocumentListUpdateNameInputDto
    {
        public long Id { get; set; }
        public string DocumentName { get; set; }
     
    }
}
