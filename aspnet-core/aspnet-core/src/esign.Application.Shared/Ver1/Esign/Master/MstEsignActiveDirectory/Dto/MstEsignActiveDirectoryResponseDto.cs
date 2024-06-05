using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;

namespace esign.Master.Dto.Ver1
{
    public class MstEsignActiveDirectoryResponseDto<T>
    {
        public string TenancyName { get; set; }
        public int TotalCount { get; set; }
        public List<T> Items { get; set; }
    }
}