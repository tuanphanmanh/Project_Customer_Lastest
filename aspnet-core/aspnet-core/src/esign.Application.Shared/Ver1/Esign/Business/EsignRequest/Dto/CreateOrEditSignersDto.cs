using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace esign.Esign.Business.EsignRequest.Dto.Ver1
{
    public class CreateOrEditSignersDto
    {
        public long? Id { get; set; }
        public long? UserId { get; set; }
        public int? SigningOrder { get; set; }
        public string PrivateMessage { get; set; }
        public long? ColorId { get; set; }
    }
}
