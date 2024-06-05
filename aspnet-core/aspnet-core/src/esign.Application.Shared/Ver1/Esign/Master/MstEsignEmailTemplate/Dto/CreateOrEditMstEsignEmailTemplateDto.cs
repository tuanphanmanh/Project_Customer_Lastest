using Abp.Application.Services.Dto;
using esign.Dto;
using System.ComponentModel.DataAnnotations;

namespace esign.Esign.Master.Ver1
{
    public class CreateOrEditMstEsignEmailTemplateDto: EntityDto<int?>
    {
        [Required]
        [StringLength(50)]
        public string TemplateCode { get; set; }
        [StringLength(150)]
        public string Title { get; set; }
        [StringLength(50)]
        public string BCC { get; set; }
        public string Message { get; set; }
    }
    public class MstEsignEmailTemplateOutputDto : EntityDto<int>
    {
        public string TemplateCode { get; set; }
        public string Title { get; set; }
        public string BCC { get; set; }
        public string Message { get; set; }
    }
    public class MstEsignEmailTemplateInputDto : PagedInputDto
    {
        public string Code { get; set; }
    }
}
