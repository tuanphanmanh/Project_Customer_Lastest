using esign.Esign.Business.EsignRequest.Dto.Ver1;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace esign.Esign.Business.EsignSignerList.Dto.Ver1
{
    public class EsignSignerForRequestDto
    {
        public long Id { get; set; }
        public long? UserId { get; set; }
        public int SigningOrder { get; set; }
        public string PrivateMessage { get; set; }
        [StringLength(50)]
        public string FullName { get; set; }
        [StringLength(50)]
        public string Title { get; set; }
        [StringLength(500)]
        public string ImageUrl { get; set; }
        public string Color { get; set; }
        public bool? IsHaveDefaultSignature { get; set; }

        public List<EsignCommentDto> ListComments { get; set; }
    }
}
