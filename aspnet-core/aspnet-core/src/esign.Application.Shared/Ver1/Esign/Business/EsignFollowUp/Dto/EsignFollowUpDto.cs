using Abp.Application.Services.Dto;
namespace esign.Business.Ver1
{
    public class CreateOrEditEsignFollowUpInputDto : EntityDto<long?>
    {
        public long RequestId { get; set; }    
        public bool IsFollowUp { get; set; }
    } 

}
