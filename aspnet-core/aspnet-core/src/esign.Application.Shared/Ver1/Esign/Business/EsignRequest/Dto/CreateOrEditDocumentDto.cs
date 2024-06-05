using System;
using System.Collections.Generic;
using System.Text;

namespace esign.Esign.Business.EsignRequest.Dto.Ver1
{
    public class CreateOrEditDocumentDto
    {
        public long Id { get; set; }
        public int? DocumentOrder { get; set; }
        public long? DocumentTempId { get; set; }
        public List<CreateOrEditPositionsDto> Positions { get; set; }
    }
}
