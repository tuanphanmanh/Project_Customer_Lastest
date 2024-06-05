using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;

namespace esign.Master.Dto.Ver1
{
    public class MstEsignActiveDirectoryRequestDto
    {
        [StringLength(256)]
        public string SearchValue { get; set; }        
        public int GroupCategory { get; set; }
        public long SkipCount { get; set; }
        public long MaxResultCount { get; set; }
    }
}