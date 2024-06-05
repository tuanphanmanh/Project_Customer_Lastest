using Abp.Application.Services.Dto;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace esign.Business.Ver1
{
    public class CreateEsignFeedbackHubDto : EntityDto<int?>
    {
        public string UserComment { get; set; }
        public List<IFormFile> images { get; set; }
    }
}
