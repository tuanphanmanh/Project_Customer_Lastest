using Abp.Application.Services.Dto;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace esign.Esign.Master.MstActivityHistory.Dto.Ver1
{
    public class CreateOrEditMstActivityHistoryDto : EntityDto<int?> 
    {
        public string Code { get; set; }
        public string Description { get; set; }
        public string InternationalName { get; set; }
        public string LocalName { get; set; }
        [CanBeNull]
        public IFormFile Image { get; set; }
    }
}
