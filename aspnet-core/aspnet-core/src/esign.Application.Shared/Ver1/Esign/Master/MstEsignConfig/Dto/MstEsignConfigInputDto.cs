using esign.Dto;

namespace esign.Esign.Master.MstEsignConfig.Dto.Ver1
{
    public class MstEsignConfigInputDto : PagedInputDto
    {
        public virtual string Code { get; set; }
        public virtual string Description { get; set; }
    }
}
